using CurConverter.Data;
using CurConverter.Services;
using Microsoft.EntityFrameworkCore;
using System.Text;

/// <summary>
/// ����� ����� � ����������.
/// ����������� ������� � �������� ��������� HTTP-��������.
/// </summary>
var builder = WebApplication.CreateBuilder(args);

// ����������� ���������� ��������� ��� ��������� windows-1251
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

/// <summary>
/// ���������� �������� � ��������� ��������� ������������.
/// </summary>
builder.Services.AddRazorPages();
builder.Services.AddScoped<ICurrencyService, CurrencyService>();
builder.Services.AddHttpClient();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

/// <summary>
/// ��������� ��������� ��������� HTTP-��������.
/// </summary>
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // �������� HSTS ��� ��������� ������������ � ������� �����
    // ���������: https://aka.ms/aspnetcore-hsts 
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

app.Run();