using System.Text;
using System.Globalization;
using System.Net.Http;
using System.Xml.Linq;
using CurConverter.Data;
using CurConverter.Models;
using Microsoft.EntityFrameworkCore;

namespace CurConverter.Services
{
    /// <summary>
    /// Сервис для работы с валютами.
    /// Получает данные о курсах валют из внешнего API и выполняет конвертацию.
    /// </summary>
    public class CurrencyService : ICurrencyService
    {
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Инициализирует новый экземпляр сервиса валют.
        /// </summary>
        /// <param name="context">Контекст базы данных.</param>
        /// <param name="httpClient">HTTP-клиент для выполнения запросов к API.</param>
        public CurrencyService(ApplicationDbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        /// <summary>
        /// Получает список валют из внешнего API и сохраняет их в базу данных.
        /// </summary>
        /// <returns>Список валют с актуальными курсами.</returns>
        public async Task<IEnumerable<Currency>> GetCurrenciesAsync()
        {
            try
            {
                var responseBytes = await _httpClient.GetByteArrayAsync("https://www.cbr.ru/scripts/XML_daily.asp ");

                var responseString = Encoding.GetEncoding("windows-1251").GetString(responseBytes);

                var document = XDocument.Parse(responseString);

                var currencies = new List<Currency>();
                foreach (var element in document.Root.Elements("Valute"))
                {
                    var code = element.Element("CharCode")?.Value;
                    var rate = decimal.Parse(element.Element("Value")?.Value.Replace(',', '.'), CultureInfo.InvariantCulture);
                    var name = element.Element("Name")?.Value;

                    var currency = await _context.Currencies.FirstOrDefaultAsync(c => c.Code == code);
                    if (currency == null)
                    {
                        currency = new Currency
                        {
                            Code = code,
                            Name = name,
                            Rate = rate,
                            Date = DateTime.Now
                        };
                        _context.Currencies.Add(currency);
                    }
                    else
                    {
                        currency.Rate = rate;
                        currency.Date = DateTime.Now;
                    }

                    currencies.Add(currency);
                }

                await _context.SaveChangesAsync();

                return currencies;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка в GetCurrenciesAsync: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Конвертирует сумму из одной валюты в другую на основе текущих курсов.
        /// </summary>
        /// <param name="fromCode">Код исходной валюты.</param>
        /// <param name="toCode">Код целевой валюты.</param>
        /// <param name="amount">Сумма для конвертации.</param>
        /// <returns>Результат конвертации.</returns>
        /// <exception cref="Exception">Выбрасывается, если валюта не найдена или курс недействителен.</exception>
        public async Task<decimal> ConvertCurrencyAsync(string fromCode, string toCode, decimal amount)
        {
            // Получаем курсы валют
            var fromCurrency = await _context.Currencies.FirstOrDefaultAsync(c => c.Code == fromCode);
            var toCurrency = await _context.Currencies.FirstOrDefaultAsync(c => c.Code == toCode);

            if (fromCurrency == null || toCurrency == null)
            {
                throw new Exception("Одна из валют не найдена.");
            }

            if (fromCurrency.Rate <= 0 || toCurrency.Rate <= 0)
            {
                throw new Exception("Курс одной из валют недействителен.");
            }

            return (amount * fromCurrency.Rate) / toCurrency.Rate;
        }
    }
}