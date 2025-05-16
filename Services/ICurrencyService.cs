using CurConverter.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CurConverter.Services
{
    /// <summary>
    /// Интерфейс для работы с валютами.
    /// Определяет методы для получения курсов валют и выполнения конвертации.
    /// </summary>
    public interface ICurrencyService
    {
        /// <summary>
        /// Получает список валют из внешнего источника и сохраняет их в базу данных.
        /// </summary>
        /// <returns>Список валют с актуальными курсами.</returns>
        Task<IEnumerable<Currency>> GetCurrenciesAsync();

        /// <summary>
        /// Конвертирует сумму из одной валюты в другую на основе текущих курсов.
        /// </summary>
        /// <param name="fromCode">Код исходной валюты.</param>
        /// <param name="toCode">Код целевой валюты.</param>
        /// <param name="amount">Сумма для конвертации.</param>
        /// <returns>Результат конвертации.</returns>
        Task<decimal> ConvertCurrencyAsync(string fromCode, string toCode, decimal amount);
    }
}