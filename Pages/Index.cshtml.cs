using CurConverter.Models;
using CurConverter.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CurConverter.Pages
{
    /// <summary>
    /// ������ �������� ��� ����������� ������ ����� � ���������� �����������.
    /// </summary>
    public class IndexModel : PageModel
    {
        private readonly ICurrencyService _currencyService;

        /// <summary>
        /// �������������� ����� ��������� ������ ��������.
        /// </summary>
        /// <param name="currencyService">������ ��� ������ � ��������.</param>
        public IndexModel(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        /// <summary>
        /// ��� �������� ������ ��� �����������.
        /// </summary>
        [BindProperty]
        public string FromCode { get; set; }

        /// <summary>
        /// ��� ������� ������ ��� �����������.
        /// </summary>
        [BindProperty]
        public string ToCode { get; set; }

        /// <summary>
        /// ����� ��� �����������.
        /// </summary>
        [BindProperty]
        public decimal Amount { get; set; }

        /// <summary>
        /// ������ ��������� �����.
        /// </summary>
        public IEnumerable<Currency> Currencies { get; set; } = Enumerable.Empty<Currency>();

        /// <summary>
        /// ��������� ����������� �����.
        /// </summary>
        public decimal ConvertedAmount { get; set; }

        /// <summary>
        /// ������������ GET-������ ��� �������� ������ � ������ �����.
        /// </summary>
        /// <returns>������, �������������� ����������� ��������.</returns>
        public async Task OnGetAsync()
        {
            Currencies = await _currencyService.GetCurrenciesAsync();
        }

        /// <summary>
        /// ������������ POST-������ ��� ���������� ����������� �����.
        /// </summary>
        /// <returns>��������� ���������� �������� (������������ ��������).</returns>
        public async Task<IActionResult> OnPostConvertAsync()
        {
            try
            {
                // ��������� �����������
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