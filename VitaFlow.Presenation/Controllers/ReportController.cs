using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR.Protocol;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using VitaFlow.Application.IServices;
using VitaFlow.Domain.Entities;
using VitaFlow.Domain.Interface;

namespace VitaFlow.Presenation.Controllers
{
    public class ReportController : Controller
    {
        private readonly ILogger<ReportController> _logger;
        private readonly I_Report i_Report;
        private readonly I_Login i_Login_Service;
        public ReportController(ILogger<ReportController> logger, I_Report iReport, I_Login i_Login_Service)
        {
            _logger = logger;
            i_Report = iReport;
            this.i_Login_Service = i_Login_Service;
        }
        public async Task<IActionResult> OrderReport()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Status")))
            {
                PartyOrderList obj = new PartyOrderList();
                obj.GetPartyOrderlist = await i_Report.GetOrderList(HttpContext.Session.GetString("FCode"));
                return View(obj);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }

        public async Task<IActionResult> PartyOrderDetail(string OrderNo)
        {
            PartyOrderList obj = new PartyOrderList();
            try
            {
                obj.GetPartyOrderlist = await i_Report.GetOrderProductList(HttpContext.Session.GetString("FCode"), OrderNo);
            }
            catch (Exception ex)
            {

            }
            return PartialView("~/Views/Report/PartyOrderDetail.cshtml", obj);
        }
        public IActionResult SalesReport()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Status")))
            {
                var configurations = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                List<SelectListItem> objListInvoiceType = new List<SelectListItem>();
                objListInvoiceType.Add(new SelectListItem { Text = "All", Value = "" });
                objListInvoiceType.Add(new SelectListItem { Text = "PV Invoice", Value = "B" });
                objListInvoiceType.Add(new SelectListItem { Text = "Repurchase Invoice", Value = "R" });
                //objListInvoiceType.Add(new SelectListItem { Text = "FPV Invoice", Value = "FPV" });
                //objListInvoiceType.Add(new SelectListItem { Text = "GV Invoice", Value = "G" });
                //objListInvoiceType.Add(new SelectListItem { Text = "Stock Transfer", Value = "S" });
                //objListInvoiceType.Add(new SelectListItem { Text = "Customer Invoice", Value = "GC" });
                //objListInvoiceType.Add(new SelectListItem { Text = "MRI Coupon Invoice", Value = "C" });
                //objListInvoiceType.Add(new SelectListItem { Text = "Shopping Jackpot Invoice", Value = "J" });
                //objListInvoiceType.Add(new SelectListItem { Text = "SJP Scratch Card Invoice", Value = "X" });
                //objListInvoiceType.Add(new SelectListItem { Text = "CPV Invoice", Value = "P" });
                ViewBag.InvoiceTypes = objListInvoiceType;
                ViewBag.PartyCaption = configurations["PartyCaption"];
                ViewBag.CustomerCaption = configurations["CustomerCaption"];
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        public async Task<IActionResult> GetSalesReport(string FromDate, string ToDate, string CustomerId, string ProductCode, string CategoryCode, string PartyCode, string BType, string SalesType, string InvoiceType, string BillNo, string FType, string OfferType, string ReportType)
        {
            List<SalesReport> objSalesList = new List<SalesReport>();
            try
            {
                string CurrentPartyCode = "";

                objSalesList = await i_Report.GetSalesReport(FromDate, ToDate, CustomerId, ProductCode, CategoryCode, PartyCode, BType, SalesType, InvoiceType, BillNo, FType, OfferType, ReportType);
            }
            catch (Exception ex)
            {

            }
            return Json(objSalesList);
        }

        public IActionResult StockReport()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Status")))
            {
                var configurations = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                ViewBag.PartyCaption = configurations["PartyCaption"];
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public IActionResult SaleRegisterReport()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Status")))
            {
                var configurations = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                ViewBag.PartyCaption = configurations["PartyCaption"];
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        [HttpPost]
        public async Task<IActionResult> GetSaleRegisterReport(string FromDate, string ToDate, string PartyCode, string type)
        {
            List<SaleRegister> objSaleRegisterModel = new List<SaleRegister>();
            string LoginPartyCode = Convert.ToString(HttpContext.Session.GetString("PartyCode"));
            objSaleRegisterModel = await i_Report.GetSalesRegisterReport(FromDate, ToDate, PartyCode, type);
            var jsonProduct = Json(objSaleRegisterModel);
            return jsonProduct;
        }

        public async Task<IActionResult> StockTransactionReport()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Status")))
            {
                StockReportModel objModel = new StockReportModel();
                var configurations = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                ViewBag.PartyCaption = configurations["PartyCaption"];
                objModel.CategoryList = await i_Report.GetCategoryList("Y");
                objModel.State_List = await i_Login_Service.GetstateList();
                objModel.PartyName = Convert.ToString(HttpContext.Session.GetString("PartyName"));
                objModel.PartyCode = Convert.ToString(HttpContext.Session.GetString("PartyCode"));
                objModel.GroupId = Convert.ToInt32(HttpContext.Session.GetString("GroupId"));
                return View(objModel);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public async Task<IActionResult> GetStockReceiptReport(string CategoryCode, string ProductCode, string PartyCode, string StateCode, string FromDate, string ToDate, string isSummary)
        {
            List<StockReportModel> objStockReportModel = new List<StockReportModel>();
            string LoginPartyCode = Convert.ToString(HttpContext.Session.GetString("PartyCode"));
            objStockReportModel = await i_Report.GetStockReceiptReport(CategoryCode, ProductCode, PartyCode, StateCode, FromDate, ToDate, LoginPartyCode, isSummary);
            var jsonProduct = Json(objStockReportModel);
            return jsonProduct;
        }

        [HttpPost]
        public async Task<ActionResult> GetAllProduct(decimal CategoryCode)
        {
            List<ProductDetails> objProduct = new List<ProductDetails>();
            objProduct = await i_Report.GetAllProducts(CategoryCode);
            var jsonProduct = Json(objProduct);
            return jsonProduct;
        }
        public async Task<ActionResult> GetAllCategory()
        {
            List<CategoryDetails> objCategory = new List<CategoryDetails>();
            objCategory = await i_Report.GetCategoryList("Y");

            return Json(objCategory);
        }

        [HttpPost]
        public async Task<ActionResult> GetStockReport(string CategoryCode, string ProductCode, string PartyCode, bool IsBatchWise, string StockType)
        {
            //**Added on 21Nov18
            string CurrentPartyCode = "";

            CurrentPartyCode = Convert.ToString(HttpContext.Session.GetString("PartyCode"));

            //if (PartyCode == "" || CurrentPartyCode != System.Web.Configuration.WebConfigurationManager.AppSettings["WRPartyCode"])
            //    PartyCode = CurrentPartyCode;
            //**            

            List<StockReportModel> objStockReportModel = new List<StockReportModel>();
            objStockReportModel = await i_Report.GetStockReport(CategoryCode, ProductCode, PartyCode, IsBatchWise, StockType);
            return Json(objStockReportModel);
        }

        public IActionResult DateWiseStockReport()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Status")))
            {
                var configurations = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                ViewBag.PartyCaption = Convert.ToString(configurations["ShoppeCaption"]);

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        [HttpPost]
        public async Task<ActionResult> GetDateWiseStockReport(string CategoryCode, string ProductCode, string PartyCode, string FromDate, string ToDate)
        {
            string CurrentPartyCode = "";
            CurrentPartyCode = Convert.ToString(HttpContext.Session.GetString("PartyCode"));

            //if (PartyCode == "" || CurrentPartyCode != System.Web.Configuration.WebConfigurationManager.AppSettings["WRPartyCode"])
            //    PartyCode = CurrentPartyCode;
            //**
            List<StockReportModel> objStockReportModel = new List<StockReportModel>();
            objStockReportModel = await i_Report.GetDateWiseStockReport(CategoryCode, ProductCode, CurrentPartyCode, FromDate, ToDate);
            return Json(objStockReportModel);
        }

        public IActionResult ProductSalesReport()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Status")))
            {
                var configurations = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                ViewBag.PartyCaption = configurations["PartyCaption"];
                ViewBag.CustomerCaption = configurations["CustomerCaption"];
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public async Task<IActionResult> GetProductSalesReport(string FromDate, string ToDate, string CustomerId, string ProductCode, string CategoryCode, string PartyCode, string BType, string SalesType, string InvoiceType, string BillNo, string FType, string OfferType, string ReportType)
        {
            List<SalesReport> objSalesList = new List<SalesReport>();
            try
            {
                string CurrentPartyCode = "";

                objSalesList = await i_Report.GetProductWiseSalesReport(FromDate, ToDate, CustomerId, ProductCode, CategoryCode, PartyCode, BType, SalesType, InvoiceType, BillNo, FType, OfferType, ReportType);
            }
            catch (Exception ex)
            {

            }
            return Json(objSalesList);
        }

        public IActionResult DateSalesReport()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Status")))
            {
                var configurations = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                ViewBag.PartyCaption = configurations["PartyCaption"];
                ViewBag.CustomerCaption = configurations["CustomerCaption"];
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DateSalesReport(string FromDate, string ToDate, string CustomerId, string ProductCode, string CategoryCode, string PartyCode, string BType, string SalesType, string InvoiceType, string BillNo, string FType, string OfferType, string ReportType)
        {
            List<SalesReport> objSalesList = new List<SalesReport>();
            try
            {
                objSalesList = await i_Report.GetDateWiseSalesReport(FromDate, ToDate, CustomerId, ProductCode, CategoryCode, PartyCode, BType, SalesType, InvoiceType, BillNo, FType, OfferType, ReportType);
            }
            catch (Exception ex)
            {

            }
            return Json(objSalesList);
        }

        public async Task<IActionResult> PaymentSummary()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Status")))
            {
                PaymentSummary objPaymentSummary = new PaymentSummary();
                objPaymentSummary.PaymentMode = await i_Report.GetPaymodeList();
                objPaymentSummary.PartyName = Convert.ToString(HttpContext.Session.GetString("PartyName"));
                objPaymentSummary.PartyCode = Convert.ToString(HttpContext.Session.GetString("PartyCode"));
                objPaymentSummary.GroupId = Convert.ToInt32(HttpContext.Session.GetString("GroupId"));
                return View(objPaymentSummary);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public async Task<IActionResult> GetPaymentSummaryReport(string FromDate, string ToDate, string PartyCode, string Type)
        {
            List<PaymentSummaryReport> objWalletDetails = new List<PaymentSummaryReport>();
            objWalletDetails = await i_Report.GetPaymentSummaryReport(FromDate, ToDate, PartyCode, Type);
            var jsonProduct = Json(objWalletDetails);
            return jsonProduct;
        }

        public IActionResult SalesReturnReport()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Status")))
            {
                var configurations = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                ViewBag.PartyCaption = configurations["PartyCaption"];
                ViewBag.CustomerCaption = configurations["CustomerCaption"];
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetSalesReturnReport(string FromDate, string ToDate, string ProductCode, string CategoryCode, string PartyCode, string PartyType, string Type)
        {
            List<SalesReturnReport> objWalletDetails = new List<SalesReturnReport>();
            objWalletDetails = await i_Report.GetSalesReturnReport(FromDate, ToDate, ProductCode, CategoryCode, PartyCode, PartyType, Type);
            var jsonProduct = Json(objWalletDetails);
            return jsonProduct;
        }

        public IActionResult GstSaleRegister()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Status")))
            {
                var configurations = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                ViewBag.PartyCaption = configurations["PartyCaption"];
                ViewBag.CustomerCaption = configurations["CustomerCaption"];
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public IActionResult WalletReport()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Status")))
            {
                var configurations = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                ViewBag.PartyCaption = configurations["PartyCaption"];
                ViewBag.CustomerCaption = configurations["CustomerCaption"];
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        [HttpPost]
        public async Task<IActionResult> GetWalletHistory(string FromDate, string ToDate, string PartyCode, string vtype)
        {
            List<SalesReport> objSalesReport = new List<SalesReport>();
            //string vtype = "M";
            string CurrentPartyCode;
            CurrentPartyCode = Convert.ToString(HttpContext.Session.GetString("PartyCode"));
            objSalesReport = await i_Report.GetWalletHistory(FromDate, ToDate, PartyCode, vtype);
            return Json(objSalesReport);
        }

        public async Task<IActionResult> GetAllWalletRequest(string datetype, string PartyCode, string FromDate, string ToDate, string Status)
        {
            List<WalletRequest> objWalletRequest = new List<WalletRequest>();


            objWalletRequest = await i_Report.GetAllWalletRequest(datetype, PartyCode, FromDate, ToDate, Status);
            return Json(objWalletRequest);
        }

        public IActionResult WalletRequestReport()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Status")))
            {
                var configurations = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                ViewBag.PartyCaption = configurations["PartyCaption"];
                ViewBag.CustomerCaption = configurations["CustomerCaption"];
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public IActionResult OrderHistory()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Status")))
            {
                var configurations = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                ViewBag.PartyCaption = configurations["PartyCaption"];
                ViewBag.CustomerCaption = configurations["CustomerCaption"];
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public async Task<IActionResult> GetOrderDetails(string OrderBy, string OrderTo, string Status)
        {
            string LoginPartyCode = Convert.ToString(HttpContext.Session.GetString("PartyCode"));
            List<PartyOrderModel> objOrderList = await i_Report.GetOrderList(OrderBy, OrderTo, Status);
            return Json(objOrderList);
        }
        public async Task<IActionResult> GetOrderProductDetails(string OrderNo, string OrderBy)
        {
            List<ProductModel> objOrderList = await i_Report.GetOrderProduct(OrderNo, OrderBy);
            return Json(objOrderList);
        }
        public ActionResult FranchiseeCommission()
        {
            FranchiseeCommission objfCommission = new FranchiseeCommission();
            return View(objfCommission);
        }
        [HttpPost]
        public async Task<ActionResult> GetFranchiseeBVCommission(string FromDate, string ToDate, string code, string Billtype)
        {
            List<FranchiseeCommission> lstFCommission = null;
            lstFCommission = await i_Report.GetFranchiseeBVCommission(FromDate, ToDate, code, Billtype);
            return Json(lstFCommission);
        }

        public async Task<IActionResult> PayoutSummary()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Status")))
            {
                M_PayoutSummary obj = new M_PayoutSummary();
                obj.MSessids = await i_Report.GetSessids();
                return View(obj);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public async Task<IActionResult> GetMonthWiseIncome(string Sessid)
        {
            string LoginPartyCode = Convert.ToString(HttpContext.Session.GetString("PartyCode"));
            List<MonthWiseIncome> objMonthWiseIncome = new List<MonthWiseIncome>();
            objMonthWiseIncome = await i_Report.GetMonthWiseIncome(Sessid, LoginPartyCode);
            return Json(objMonthWiseIncome);
        }

        public async Task<IActionResult> GetPerformanceInc(string Action,int SessID)
        {
            string LoginPartyCode = Convert.ToString(HttpContext.Session.GetString("PartyCode"));
            List<MPerformanceInc> obj = await i_Report.GetPerformanceInc(LoginPartyCode, Action,SessID);
            return Json(obj);
        }

        public async Task<IActionResult> PayoutStatement(string Prm)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Status")))
            {
                var base64DecodedBytes = System.Convert.FromBase64String(Prm);
                string StatementPeriod = System.Text.Encoding.UTF8.GetString(base64DecodedBytes);
                M_IncentiveStatement obj = new M_IncentiveStatement();
                obj = await i_Report.GetIncentiveStatement(Convert.ToString(HttpContext.Session.GetString("PartyCode")), StatementPeriod);
                obj.partycode = HttpContext.Session.GetString("PartyCode");
                obj.Address = HttpContext.Session.GetString("Address1");
                obj.GroupCode = HttpContext.Session.GetString("GroupPrefix");
                obj.PartyName = HttpContext.Session.GetString("PartyName");
                return View(obj);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
    }
}
