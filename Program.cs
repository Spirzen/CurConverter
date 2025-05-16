using CurConverter.Data;
using CurConverter.Services;
using Microsoft.EntityFrameworkCore;
using System.Text;

/// <summary>
/// Точка входа в приложение.
/// Настраивает сервисы и конвейер обработки HTTP-запросов.
/// </summary>
var builder = WebApplication.CreateBuilder(args);

// Регистрация провайдера кодировок для поддержки windows-1251
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

/// <summary>
/// Добавление сервисов в контейнер внедрения зависимостей.
/// </summary>
builder.Services.AddRazorPages();
builder.Services.AddScoped<ICurrencyService, CurrencyService>();
builder.Services.AddHttpClient();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

/// <summary>
/// Настройка конвейера обработки HTTP-запросов.
/// </summary>
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // Включаем HSTS для повышения безопасности в рабочей среде
    // Подробнее: https://aka.ms/aspnetcore-hsts 
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

app.Run();