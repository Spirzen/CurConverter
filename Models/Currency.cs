namespace CurConverter.Models
{
    /// <summary>
    /// Модель, представляющая валюту.
    /// Содержит информацию о коде, названии, курсе и дате обновления.
    /// </summary>
    public class Currency
    {
        /// <summary>
        /// Уникальный идентификатор валюты.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Код валюты (например, USD, EUR).
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Название валюты (например, Доллар США, Евро).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Курс валюты к рублю.
        /// Значение по умолчанию: 0.
        /// </summary>
        public decimal Rate { get; set; } = 0;

        /// <summary>
        /// Дата и время последнего обновления курса.
        /// Значение по умолчанию: текущая дата и время.
        /// </summary>
        public DateTime Date { get; set; } = DateTime.Now;
    }
}