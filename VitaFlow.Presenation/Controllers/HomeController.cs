using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Collections.Generic;
using System.Globalization;
using VitaFlow.Domain.Entities;
using VitaFlow.Domain.Interface;
using VitaFlow.Presenation.Models;

namespace VitaFlow.Presenation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly I_Report iReport;
        private readonly I_Product i_Product;
        public HomeController(ILogger<HomeController> logger, I_Report i_Report, I_Product i_Product = null)
        {
            _logger = logger;
            iReport = i_Report;
            this.i_Product = i_Product;
        }

        public async Task<IActionResult> Index()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Status")))
            {
                User obj = new User();
                string FCode = HttpContext.Session.GetString("FCode");
                obj.dashboardSummary = await BuildDashboardSummary(FCode);
                return View(obj);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        /// <summary>
        /// Dashboard ke 4 tiles: Today Sale / Total Sale / Today Purchase / Total Purchase.
        /// Dono list ek hi baar "All" range me li jaati hain aur aaj ka figure yahin filter hota hai,
        /// taaki SP ko single-day range dene par time-component ki wajah se record miss na ho.
        /// </summary>
        private async Task<DashboardSummary> BuildDashboardSummary(string FCode)
        {
            var summary = new DashboardSummary();
            var today = DateTime.Today;

            try
            {
                var sales = await iReport.GetSalesReport("All", "All", "", "", "", FCode, "S", "", "", "", "", "", "");
                if (sales != null)
                {
                    foreach (var bill in sales)
                    {
                        decimal amount = ParseAmount(bill.NetAmount);
                        if (amount == 0) { amount = ParseAmount(bill.Amount); }

                        summary.TotalSale += amount;
                        if (bill.BillDate.HasValue && bill.BillDate.Value.Date == today)
                        {
                            summary.TodaySale += amount;
                        }
                    }
                }
            }
            catch
            {
            }

            try
            {
                // isSummary "I" = invoice wise, yaani ek row per purchase bill
                var purchases = await iReport.GetStockReceiptReport("0", "0", FCode, "0", "All", "All", FCode, "I");
                if (purchases != null)
                {
                    foreach (var stn in purchases)
                    {
                        decimal amount = ParseAmount(stn.TotalAmt);

                        summary.TotalPurchase += amount;
                        DateTime? stnDate = ParseReportDate(stn.StrDate, stn.StockDate);
                        if (stnDate.HasValue && stnDate.Value.Date == today)
                        {
                            summary.TodayPurchase += amount;
                        }
                    }
                }
            }
            catch
            {
            }

            return summary;
        }

        private static decimal ParseAmount(string value)
        {
            decimal parsed;
            if (!string.IsNullOrWhiteSpace(value) &&
                decimal.TryParse(value.Replace(",", "").Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out parsed))
            {
                return parsed;
            }
            return 0;
        }

        /// <summary>
        /// Stock transaction SP date ko string me deta hai, isliye common formats try karte hain.
        /// </summary>
        private static DateTime? ParseReportDate(string dateText, DateTime fallback)
        {
            if (!string.IsNullOrWhiteSpace(dateText))
            {
                string[] formats = { "dd/MM/yyyy", "dd-MM-yyyy", "dd MMM yyyy", "dd-MMM-yyyy", "yyyy-MM-dd", "dd/MM/yyyy HH:mm:ss", "dd-MM-yyyy HH:mm:ss" };
                DateTime parsed;
                if (DateTime.TryParseExact(dateText.Trim(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsed))
                {
                    return parsed;
                }
                if (DateTime.TryParse(dateText.Trim(), CultureInfo.GetCultureInfo("en-GB"), DateTimeStyles.None, out parsed))
                {
                    return parsed;
                }
            }
            return fallback == default(DateTime) ? (DateTime?)null : fallback;
        }
        public async Task<IActionResult> GetWalletBalance()
        {
            string WalletBalance = "0";
            try
            {
                string RWalletBalance = Convert.ToString(await i_Product.GetPartyWalletBalance(HttpContext.Session.GetString("FCode"), "R"));
                string promoWalletBalance = Convert.ToString(await i_Product.GetPartyWalletBalance(HttpContext.Session.GetString("FCode"), "X"));

                WalletBalance = Convert.ToString(Convert.ToDecimal(RWalletBalance) + Convert.ToDecimal(promoWalletBalance));
            }
            catch (Exception ex)
            {

            }
            return Json(WalletBalance);
        }
        public async Task<IActionResult> GetPromoWalletBalance()
        {
            string WalletBalance = "0";
            try
            {
                string promoWalletBalance = Convert.ToString(await i_Product.GetPartyWalletBalance(HttpContext.Session.GetString("FCode"), "X"));
                WalletBalance = promoWalletBalance;
            }
            catch (Exception ex)
            {

            }
            return Json(WalletBalance);
        }
        public async Task<IActionResult> GetPVWalletBalance()
        {
            string WalletBalance = "0";
            try
            {
                WalletBalance = Convert.ToString(await i_Product.GetPartyWalletBalance(HttpContext.Session.GetString("FCode"), "P"));
            }
            catch (Exception ex)
            {

            }
            return Json(WalletBalance);
        }
        public async Task<IActionResult> GetBVWalletBalance()
        {
            string WalletBalance = "0";
            try
            {
                WalletBalance = Convert.ToString(await i_Product.GetPartyWalletBalance(HttpContext.Session.GetString("FCode"), "B"));
            }
            catch (Exception ex)
            {

            }
            return Json(WalletBalance);
        }


        public async Task<IActionResult> GetWalletTypeBalance(string WalletType)
        {
            string WalletBalance = "0";
            try
            {
                WalletBalance = Convert.ToString(await i_Product.GetPartyWalletBalance(HttpContext.Session.GetString("FCode"), WalletType));
            }
            catch (Exception ex)
            {

            }
            return Json(WalletBalance);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}