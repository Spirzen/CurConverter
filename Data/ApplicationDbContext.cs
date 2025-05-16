using CurConverter.Models;
using Microsoft.EntityFrameworkCore;

namespace CurConverter.Data
{
    /// <summary>
    /// Контекст базы данных приложения.
    /// Определяет наборы данных (DbSet) для работы с таблицами.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Инициализирует новый экземпляр контекста базы данных.
        /// </summary>
        /// <param name="options">Параметры конфигурации контекста.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Набор данных для работы с таблицей валют.
        /// </summary>
        public DbSet<Currency> Currencies { get; set; }
    }
}