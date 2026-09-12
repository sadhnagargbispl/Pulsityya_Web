using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Collections.Generic;
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
                obj.franchiseLimit = await iReport.GetFranchiseLimit(HttpContext.Session.GetString("FCode"), Convert.ToInt32(HttpContext.Session.GetString("UserId")));
                obj.TopsellingProduct = await iReport.GetTopSellingProduct("Top10", HttpContext.Session.GetString("FCode"));
                obj.StockProduct = await iReport.StockProducts(HttpContext.Session.GetString("FCode"));
               // obj.TopclientProduct = await iReport.GetTopClientsProduct("Top10", HttpContext.Session.GetString("FCode"));
                // Pull product images from the SAME source Productlist uses, mapped by product name (display only)
                try
                {
                    var vfImgMap = new Dictionary<string, string>();
                    if (i_Product != null)
                    {
                        var vfAllProducts = await i_Product.Productlist(HttpContext.Session.GetString("ParentPartyCode"));
                        if (vfAllProducts != null)
                        {
                            foreach (var vfp in vfAllProducts)
                            {
                                var vfKey = System.Text.RegularExpressions.Regex.Replace((vfp.ProductName ?? "").ToUpper(), "[^A-Z0-9]", "");
                                if (!string.IsNullOrEmpty(vfKey) && !string.IsNullOrWhiteSpace(vfp.ImagePath) && !vfImgMap.ContainsKey(vfKey))
                                {
                                    vfImgMap[vfKey] = vfp.ImagePath;
                                }
                            }
                        }
                    }
                    ViewBag.ProductImageMap = vfImgMap;
                }
                catch
                {
                    ViewBag.ProductImageMap = new Dictionary<string, string>();
                }

                return View(obj);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
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