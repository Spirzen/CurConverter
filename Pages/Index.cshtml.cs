using CurConverter.Models;
using CurConverter.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CurConverter.Pages
{
    /// <summary>
    /// Модель страницы для отображения курсов валют и выполнения конвертации.
    /// </summary>
    public class IndexModel : PageModel
    {
        private readonly ICurrencyService _currencyService;

        /// <summary>
        /// Инициализирует новый экземпляр модели страницы.
        /// </summary>
        /// <param name="currencyService">Сервис для работы с валютами.</param>
        public IndexModel(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        /// <summary>
        /// Код исходной валюты для конвертации.
        /// </summary>
        [BindProperty]
        public string FromCode { get; set; }

        /// <summary>
        /// Код целевой валюты для конвертации.
        /// </summary>
        [BindProperty]
        public string ToCode { get; set; }

        /// <summary>
        /// Сумма для конвертации.
        /// </summary>
        [BindProperty]
        public decimal Amount { get; set; }

        /// <summary>
        /// Список доступных валют.
        /// </summary>
        public IEnumerable<Currency> Currencies { get; set; } = Enumerable.Empty<Currency>();

        /// <summary>
        /// Результат конвертации валют.
        /// </summary>
        public decimal ConvertedAmount { get; set; }

        /// <summary>
        /// Обрабатывает GET-запрос для загрузки данных о курсах валют.
        /// </summary>
        /// <returns>Задача, представляющая асинхронную операцию.</returns>
        public async Task OnGetAsync()
        {
            Currencies = await _currencyService.GetCurrenciesAsync();
        }

        /// <summary>
        /// Обрабатывает POST-запрос для выполнения конвертации валют.
        /// </summary>
        /// <returns>Результат выполнения операции (перезагрузка страницы).</returns>
        public async Task<IActionResult> OnPostConvertAsync()
        {
            try
            {
                // Выполняем конвертацию
                ConvertedAmount = await _currencyService.ConvertCurrencyAsync(FromCode, ToCode, Amount);
                Currencies = await _currencyService.GetCurrenciesAsync();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return Page();
        }
    }
}