using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Threading.Tasks;
using VitaFlow.Domain.Entities;
using VitaFlow.Domain.Interface;
using VitaFlow.Presenation.Extensions;

namespace VitaFlow.Presenation.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ILogger<CartController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly I_Transaction i_Transaction;
        private readonly I_Product i_Product;
        public TransactionController(ILogger<CartController> logger, IWebHostEnvironment webHostEnvironment,
            I_Transaction i_Transaction, I_Product i_Product)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            this.i_Transaction = i_Transaction;
            this.i_Product = i_Product;
        }

        //public IActionResult WalletRequest()
        //{
        //    if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Status")))
        //    {
        //        return View();
        //    }
        //    else
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }

        //}
        public IActionResult WalletRequest(string walletType)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Status")))
            {
                var model = new M_WalletRequest();

                // Button से आए तो वही wallet preselect कर दें
                if (!string.IsNullOrEmpty(walletType))
                {
                    model.VType = walletType; // "P" या "B"
                }

                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public async Task<IActionResult> WalletRequest(M_WalletRequest req)
        {
            var Response = "";
            var msg = "";
            try
            {
                var configurations = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                var ShoppingUrl = configurations["ShoppingUrl"];
                string Rndomstr = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                if (req.Image != null && req.Image.Length > 0)
                {
                    var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "WalletReqsImages");
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }
                    var fileName = Path.GetFileName(req.Image.FileName);
                    var ScannedFileName = Rndomstr + "" + fileName;
                    var filePath = Path.Combine(uploadPath, ScannedFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await req.Image.CopyToAsync(stream);
                    }
                    req.ReqBy = HttpContext.Session.GetString("FCode");
                    req.VType = req.VType;
                    req.ScannedFileName = ShoppingUrl + ScannedFileName;
                    Response = await i_Transaction.SaveWalletRequest(req);
                    if (Response != null && Response == "OK")
                    {
                        msg = "OK";
                    }
                    else
                    {
                        msg = "fail";
                    }

                }
            }
            catch (Exception ex)
            {
            }
            return Json(new { msg });

        }
        public IActionResult PendingOrder()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Status")))
            {
                var configurations = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                ViewBag.FranchiseUrl = configurations["FranchiseUrl"];
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        public async Task<IActionResult> GetOrderDetails(string OrderBy, string OrderTo, string Status)
        {
            List<PartyOrderModel> objOrderList = new List<PartyOrderModel>();
            try
            {
                objOrderList = await i_Transaction.GetOrderList(OrderBy, OrderTo, Status);
            }
            catch (Exception ex)
            {

            }
            return Json(objOrderList);
        }
        public async Task<IActionResult> RejectWalletRequest(string ReqNo, string RejectReason, decimal RejectedByUserId)
        {
            ResponseDetail objResponse = new ResponseDetail();
            try
            {
                objResponse = await i_Transaction.RejectWalletRequest(ReqNo, RejectReason, Convert.ToDecimal(HttpContext.Session.GetString("UserId")));
            }
            catch (Exception ex)
            {

            }
            return Json(objResponse);
        }
        public async Task<IActionResult> RejectFranchiseOrder(string OrderNo, string RejectReason)
        {
            ResponseDetail objResponse = new ResponseDetail();
            try
            {
                objResponse = await i_Transaction.RejectFranchiseOrder(OrderNo, RejectReason, Convert.ToDecimal(HttpContext.Session.GetString("UserId")));
            }
            catch (Exception ex)
            {

            }
            return Json(objResponse);
        }
        public async Task<IActionResult> GetOrderProductDetails(string OrderNo, string OrderBy)
        {
            List<ProductModel> objOrderList = new List<ProductModel>();
            try
            {
                objOrderList = await i_Transaction.GetOrderProductList(OrderNo, OrderBy, Convert.ToString(HttpContext.Session.GetString("PartyCode")));
            }
            catch (Exception ex)
            {

            }
            return Json(objOrderList);
        }
        public async Task<IActionResult> DistributorBill()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Status")))
            {
                DistributorBillModel objDistributorModel = new DistributorBillModel();
                try
                {
                    var configurations = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                    var ShoppingUrl = configurations["ShoppingUrl"];
                    ViewBag.CVCaption = configurations["CVCaption"];
                    ViewBag.BVCaption = configurations["BVCaption"];
                    ViewBag.PVCaption = configurations["PVCaption"];
                    ViewBag.RPCaption = configurations["RPCaption"];
                    ViewBag.BarcodeCaption = configurations["BarcodeCaption"];
                    objDistributorModel.objCustomer = new CustomerDetail();
                    objDistributorModel.objProduct = new ProductModel();
                    List<SelectListItem> objBankList = new List<SelectListItem>();
                    objDistributorModel.objProduct.PayDetails = new PayDetails();
                    var result = await i_Transaction.GetBankList();
                    if (result != null)
                    {
                        objBankList = result.Select(obj => new SelectListItem
                        {
                            Text = obj.BankName,
                            Value = obj.BankCode.ToString(),
                            Selected = obj.BankCode == 0
                        }).ToList();
                        if (objBankList.Any(b => b.Selected))
                        {
                            objDistributorModel.objProduct.PayDetails.BDBankName = objBankList.First(b => b.Selected).Value;
                        }
                        ViewBag.BankNames = objBankList;
                    }
                    List<SelectListItem> CardTypes = new List<SelectListItem>();
                    CardTypes.Add(new SelectListItem { Text = "Credit Card", Value = "CC" });
                    CardTypes.Add(new SelectListItem { Text = "Debit Card", Value = "DB" });
                    ViewBag.CardTypes = CardTypes;
                    objDistributorModel.objProduct.PayDetails.CardType = "CC";
                    objDistributorModel.objCustomer.CustomerType = "New";
                    List<SelectListItem> OfferSelectList = new List<SelectListItem>();
                    OfferSelectList.Add(new SelectListItem { Text = "--Choose Offer--", Value = "0" });
                    ViewBag.OfferList = OfferSelectList;
                    var KitIdlist = await i_Transaction.GetKitIdList();
                    if (KitIdlist != null)
                    {
                        var KidIsListObj = KitIdlist
                            .Select(obj => new SelectListItem
                            {
                                Text = obj.KitName,
                                Value = obj.KitId.ToString()
                            })
                            .ToList();
                        // Add the default item at the beginning of the list
                        KidIsListObj.Insert(0, new SelectListItem { Text = "--Select Kit--", Value = "0" });
                        ViewBag.objKitList = KidIsListObj;
                    }
                    objDistributorModel.objCustomer.KitId = 0;
                }
                catch (Exception ex)
                {

                }
                return View(objDistributorModel);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        public async Task<IActionResult> GetProductNamesOnly()
        {
            List<string> model = new List<string>();
            try
            {
                model = await i_Transaction.GetAutocompProductsOnly(Convert.ToString(HttpContext.Session.GetString("PartyCode")));
            }
            catch (Exception ex)
            {

            }
            return Json(model);
        }
        public async Task<IActionResult> GetProductBarcodeOnly()
        {
            List<string> model = new List<string>();
            try
            {
                model = await i_Transaction.GetProductBarcodeOnly(Convert.ToString(HttpContext.Session.GetString("PartyCode")));
            }
            catch (Exception ex)
            {

            }
            return Json(model);
        }
        [HttpPost]
        public async Task<IActionResult> GetCustInfo(string IdNo)
        {
            CustomerDetail model = new CustomerDetail();
            try
            {
                model = await i_Transaction.GetCustInfo(IdNo);
            }
            catch (Exception ex)
            {

            }
            return Json(model);
        }

        [HttpGet]
        public async Task<IActionResult> ValidateCustomerbyAPI(string IdNo, string Password)
        {
            MemberAPIRoot model = new MemberAPIRoot();
            string Message = "", Code = "101";
            string Voucherbal = "";
            string Fpv_Balance = "";
            try
            {
                model = await i_Transaction.ValidateCustomerbyAPI(IdNo, Password);
                if (model.Success == "true")
                {
                    Message = "Password Varify Successfully";
                    Code = "200";
                    Voucherbal = model.Result.VOUCHERBAL_Balance;
                    Fpv_Balance = model.Result.Fpv_Balance;
                }
                else
                {
                    Message = model.ApiMessage;
                }
            }
            catch (Exception ex)
            {

            }
            return Json(new { Message, Code, Voucherbal, Fpv_Balance });
        }

        [HttpPost]
        public async Task<IActionResult> GetProductInfo(string SearchType, string data, bool isCForm, string BillType, bool IsBillOnMrp, string OfferID, string StockforParty, bool allhalf, string Invoice, string IsSpclOffer)
        {
            List<ProductModel> model = new List<ProductModel>();
            try
            {
                if (string.IsNullOrEmpty(StockforParty))
                    StockforParty = Convert.ToString(HttpContext.Session.GetString("PartyCode"));

                model = await i_Transaction.GetproductInfo(SearchType, data, isCForm, BillType, Convert.ToDecimal(HttpContext.Session.GetString("StateCode")), StockforParty, IsBillOnMrp, OfferID, allhalf, Invoice, IsSpclOffer);
            }
            catch (Exception ex)
            {

            }
            return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> GetproductInfoBatchWise(string SearchType, string data, bool isCForm, string BillType, bool IsBillOnMrp, string OfferID, string StockforParty, bool allhalf, string Invoice, string IsSpclOffer)
        {
            List<ProductModel> model = new List<ProductModel>();
            try
            {
                if (string.IsNullOrEmpty(StockforParty))
                    StockforParty = Convert.ToString(HttpContext.Session.GetString("PartyCode"));

                model = await i_Transaction.GetproductInfoBatchWise(SearchType, data, isCForm, BillType, Convert.ToDecimal(HttpContext.Session.GetString("StateCode")), StockforParty, IsBillOnMrp, OfferID, allhalf, Invoice, IsSpclOffer);
            }
            catch (Exception ex)
            {

            }
            return Json(model);
        }
        public async Task<IActionResult> GetWalletBalance(string Waltype)
        {
            string WalletBalance = "0";
            try
            {
                WalletBalance = Convert.ToString(await i_Product.GetPartyWalletBalance(Convert.ToString(HttpContext.Session.GetString("FCode")), Waltype));
            }
            catch (Exception ex)
            {

            }
            return Json(WalletBalance);
        }
        [HttpGet]
        public async Task<IActionResult> CheckFpVoucher(string Code, string Idno)
        {
            string Msg = string.Empty;
            string stscode = string.Empty;
            decimal amount = 0;
            try
            {
                FPVoucher obj = new FPVoucher();
                obj = await i_Transaction.CheckFpVoucher(Code, Idno);
                if (obj != null && obj.Code != null)
                {
                    if (obj.Isuse == true)
                    {
                        stscode = "100";
                        Msg = "Voucher code already used";
                    }
                    else
                    {
                        stscode = "200";
                        amount = obj.Amount;
                    }
                }
                else
                {
                    stscode = "100";
                    Msg = "Voucher code not found";
                }
            }
            catch (Exception ex)
            {
                stscode = "100";
                Msg = "Something went wrong";
            }
            return Json(new { Msg, stscode, amount });
        }
        [HttpGet]
        public async Task<IActionResult> CheckFpWallet(string Idno)
        {
            string Msg = string.Empty;
            string stscode = string.Empty;
            decimal amount = 0;
            try
            {
                FPVoucher obj = new FPVoucher();
                FPVoucherEligibilityResult fPVoucherEligibilityResult = await i_Transaction.CheckFPVoucherEligibilityAsync(Idno);
                if (fPVoucherEligibilityResult.EligibilityStatus == "Eligible")
                {
                    obj = await i_Transaction.GetCheckFpWallet(Idno);
                    stscode = "200";
                    amount = obj.TotalBalance;
                }
                else
                {
                    stscode = "100";
                    Msg = fPVoucherEligibilityResult.Reason;
                }

            }
            catch (Exception ex)
            {
                stscode = "100";
                Msg = "Something went wrong";
            }
            return Json(new { Msg, stscode, amount });
        }
        public async Task<IActionResult> CheckCoupon(string Code, string Idno)
        {
            string Msg = string.Empty;
            string stscode = string.Empty;
            decimal amount = 0;
            try
            {
                Coupon obj = new Coupon();
                obj = await i_Transaction.CheckCoupon(Code, Idno);
                if (obj != null && obj.Code != null)
                {
                    if (obj.Isuse == true)
                    {
                        stscode = "100";
                        Msg = "Coupon code already used";
                    }
                    else
                    {
                        stscode = "200";
                        amount = obj.Amount;
                    }
                }
                else
                {
                    stscode = "100";
                    Msg = "Coupon code not found";
                }
            }
            catch (Exception ex)
            {
                stscode = "100";
                Msg = "Something went wrong";
            }
            return Json(new { Msg, stscode, amount });
        }

        [HttpPost]
        public async Task<IActionResult> SaveDistributorBill(DistributorBillModel objModel)
        {
            ResponseDetail objResponse = new ResponseDetail();
            string WalletBalance = "0";
            try
            {
                if (objModel != null)
                {
                    objModel.objListProduct = new List<ProductModel>();
                    if (!string.IsNullOrEmpty(objModel.objProductListStr))
                    {
                        var objects = JArray.Parse(objModel.objProductListStr); // parse as array  
                        foreach (JObject root in objects)
                        {
                            ProductModel objTemp = new ProductModel();
                            foreach (KeyValuePair<String, JToken> app in root)
                            {
                                // var appName = app.Key;
                                //    var ProductGrid = [{"AvailStock":"", "SNo": "", "Code": "", "ProductName": "", "MRP": "", "DP": "", "Rate": "","BatchNo":"", "Barcode": "", "RP": "", "BV": "", "CV": "", "PV": "", "Qty": "", "RPValue": "", "BVValue": "", "CVValue": "", "PVValue": "", "CommsnPer": "", "CommsnAmt": "", "DiscPer": "", "DiscAmt": "", "Amount": "", "TaxType": "", "TaxPer": "", "TaxAmt": "", "TotalAmount": ""}];
                                if (app.Key == "Code")
                                {
                                    objTemp.ProdCode = (int)app.Value;
                                }
                                else if (app.Key == "ProductName")
                                {
                                    objTemp.ProductName = (string)app.Value;
                                }
                                else if (app.Key == "Rate")
                                {
                                    objTemp.Rate = (decimal)app.Value;
                                }
                                else if (app.Key == "Barcode")
                                {
                                    objTemp.Barcode = app.Value.ToString();
                                }
                                else if (app.Key == "BatchNo")
                                {
                                    objTemp.BatchNo = app.Value.ToString();
                                }
                                else if (app.Key == "MRP")
                                {
                                    objTemp.MRP = (decimal?)app.Value;
                                }
                                else if (app.Key == "Qty")
                                {
                                    objTemp.Quantity = (decimal)app.Value;
                                }
                                else if (app.Key == "FreeQty")
                                {
                                    objTemp.FreeQty = (decimal)app.Value;
                                }
                                else if (app.Key == "BuyFreeQty")
                                {
                                    objTemp.TFreeQty = (int)app.Value;
                                }
                                else if (app.Key == "PV")
                                {
                                    objTemp.PV = (decimal)app.Value;
                                }
                                else if (app.Key == "CV")
                                {
                                    objTemp.CV = (decimal)app.Value;
                                }
                                else if (app.Key == "BV")
                                {
                                    objTemp.BV = (decimal)app.Value;
                                }
                                else if (app.Key == "RP")
                                {
                                    objTemp.RP = (decimal)app.Value;
                                }
                                else if (app.Key == "DP")
                                {
                                    objTemp.DP = (decimal)app.Value;
                                }
                                else if (app.Key == "CVValue")
                                {
                                    objTemp.CVValue = (decimal)app.Value;
                                }
                                else if (app.Key == "PVValue")
                                {
                                    objTemp.PVValue = (decimal)app.Value;
                                }
                                else if (app.Key == "BVValue")
                                {
                                    objTemp.BVValue = (decimal)app.Value;
                                }
                                else if (app.Key == "DiscPer")
                                {
                                    objTemp.DiscPer = (decimal)app.Value;
                                }
                                else if (app.Key == "DiscAmt")
                                {
                                    objTemp.DiscAmt = (decimal)app.Value;
                                }
                                else if (app.Key == "TaxAmt")
                                {
                                    objTemp.TaxAmt = (decimal)app.Value;
                                }
                                else if (app.Key == "TaxPer")
                                {
                                    objTemp.TaxPer = (decimal)app.Value;
                                }
                                else if (app.Key == "Amount")
                                {
                                    objTemp.Amount = (decimal)app.Value;
                                }
                                else if (app.Key == "TotalAmount")
                                {
                                    objTemp.TotalAmount = (decimal)app.Value;
                                }
                                else if (app.Key == "TaxType")
                                {
                                    objTemp.TaxType = (string)app.Value;
                                }
                                else if (app.Key == "PVValue")
                                {
                                    objTemp.DiscAmt = (decimal)app.Value;

                                }
                                else if (app.Key == "AvailStock")
                                {
                                    objTemp.StockAvailable = (decimal)app.Value;
                                }
                                else if (app.Key == "CommsnPer")
                                {
                                    objTemp.CommissionPer = (decimal)app.Value;
                                }
                                else if (app.Key == "CommsnAmt")
                                {
                                    objTemp.CommissionAmt = (decimal)app.Value;
                                }
                                else if (app.Key == "RPValue")
                                {
                                    objTemp.RPValue = (decimal)app.Value;
                                }
                                else if (app.Key == "ProductTye")
                                {
                                    objTemp.ProductTye = (string)app.Value;
                                }
                                else if (app.Key == "TotalWeight")
                                {
                                    objTemp.TotalWeight = (decimal)app.Value;
                                }
                            }
                            objModel.objListProduct.Add(objTemp);
                        }
                        if (objModel.SelectedInvoiceType == "PV")
                        {
                            WalletBalance = Convert.ToString(await i_Product.GetPartyWalletBalance(Convert.ToString(HttpContext.Session.GetString("FCode")), "P"));
                            if (Convert.ToDecimal(WalletBalance) < Math.Round(objModel.objProduct.TotalNetPayable))
                            {
                                objResponse.ResponseStatus = "FAILED";
                                objResponse.ResponseMessage = "Insufficient Balance in your PV wallet";
                            }
                            else
                            {
                                objModel.objCustomer.UserDetails = HttpContext.Session.GetComplexData<User>("LoginUser");
                                string myIP = HttpContext.Connection.RemoteIpAddress?.ToString();
                                string currentDate = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                                objModel.objProduct.UID = myIP + currentDate;
                                objResponse = await i_Transaction.SaveDistributorBill(objModel);
                            }
                        }
                        else if (objModel.SelectedInvoiceType == "BV")
                        {
                            WalletBalance = Convert.ToString(await i_Product.GetPartyWalletBalance(Convert.ToString(HttpContext.Session.GetString("FCode")), "B"));
                            if (Convert.ToDecimal(WalletBalance) < Math.Round(objModel.objProduct.CashAmount))
                            {
                                objResponse.ResponseStatus = "FAILED";
                                objResponse.ResponseMessage = "Insufficient Balance in your BV wallet";
                            }
                            else
                            {
                                objModel.objCustomer.UserDetails = HttpContext.Session.GetComplexData<User>("LoginUser");
                                string myIP = HttpContext.Connection.RemoteIpAddress?.ToString();
                                string currentDate = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                                objModel.objProduct.UID = myIP + currentDate;
                                objResponse = await i_Transaction.SaveDistributorBill(objModel);
                            }
                        }
                        else if (objModel.BillType == "party")
                        {
                            if (objModel.PartyInvoice == "P")
                            {
                                WalletBalance = Convert.ToString(await i_Product.GetPartyWalletBalance(Convert.ToString(HttpContext.Session.GetString("FCode")), "P"));
                                if (Convert.ToDecimal(WalletBalance) < Math.Round(objModel.objProduct.TotalNetPayable))
                                {
                                    objResponse.ResponseStatus = "FAILED";
                                    objResponse.ResponseMessage = "Insufficient Balance in your PV Wallet Sale Balance";
                                    return Json(objResponse);
                                }
                            }
                            if (objModel.PartyInvoice == "B")
                            {
                                WalletBalance = Convert.ToString(await i_Product.GetPartyWalletBalance(Convert.ToString(HttpContext.Session.GetString("FCode")), "B"));
                                if (Convert.ToDecimal(WalletBalance) < Math.Round(objModel.objProduct.TotalNetPayable))
                                {
                                    objResponse.ResponseStatus = "FAILED";
                                    objResponse.ResponseMessage = "Insufficient Balance in your BV Wallet Sale Balance";
                                    return Json(objResponse);
                                }
                            }
                            objModel.objCustomer.UserDetails = HttpContext.Session.GetComplexData<User>("LoginUser");
                            string myIP = HttpContext.Connection.RemoteIpAddress?.ToString();
                            string currentDate = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                            objModel.objProduct.UID = myIP + currentDate;
                            objResponse = await i_Transaction.SaveDistributorBill(objModel);
                        }
                        else if (objModel.BillType == "customer")
                        {
                            objModel.objCustomer.UserDetails = HttpContext.Session.GetComplexData<User>("LoginUser");
                            string myIP = HttpContext.Connection.RemoteIpAddress?.ToString();
                            string currentDate = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                            objModel.objProduct.UID = myIP + currentDate;
                            objResponse = await i_Transaction.SaveDistributorBill(objModel);
                        }
                    }
                }
                else
                {
                    objResponse.ResponseMessage = "Something went wrong!";
                    objResponse.ResponseStatus = "FAILED";
                }
            }
            catch (Exception ex)
            {
                objResponse.ResponseMessage = ex.Message;
                objResponse.ResponseStatus = "FAILED";
            }

            return Json(objResponse);
        }

        [HttpPost]
        public async Task<ActionResult> SaveDispatchOrder(PartyOrderModel objPartyOrderModel)
        {
            ResponseDetail objResponse = new ResponseDetail();
            if (objPartyOrderModel != null)
            {
                objPartyOrderModel.objListProduct = new List<ProductModel>();
                if (!string.IsNullOrEmpty(objPartyOrderModel.objProductListStr))
                {
                    var objects = JArray.Parse(objPartyOrderModel.objProductListStr); // parse as array  
                    foreach (JObject root in objects)
                    {
                        ProductModel objTemp = new ProductModel();
                        foreach (KeyValuePair<String, JToken> app in root)
                        {
                            // var appName = app.Key;
                            //    var ProductGrid = [{"AvailStock":"", "SNo": "", "Code": "", "ProductName": "", "MRP": "", "DP": "", "Rate": "","BatchNo":"", "Barcode": "", "RP": "", "BV": "", "CV": "", "PV": "", "Qty": "", "RPValue": "", "BVValue": "", "CVValue": "", "PVValue": "", "CommsnPer": "", "CommsnAmt": "", "DiscPer": "", "DiscAmt": "", "Amount": "", "TaxType": "", "TaxPer": "", "TaxAmt": "", "TotalAmount": ""}];
                            if (app.Key == "Code")
                            {
                                objTemp.ProdCode = (int)app.Value;
                            }
                            else if (app.Key == "ProductName")
                            {
                                objTemp.ProductName = (string)app.Value;
                            }
                            else if (app.Key == "Rate")
                            {
                                objTemp.Rate = (decimal)app.Value;
                            }
                            else if (app.Key == "Barcode")
                            {
                                objTemp.Barcode = app.Value.ToString();
                            }
                            else if (app.Key == "BatchNo")
                            {
                                objTemp.BatchNo = app.Value.ToString();
                            }
                            else if (app.Key == "MRP")
                            {
                                objTemp.MRP = (decimal?)app.Value;
                            }
                            else if (app.Key == "Qty")
                            {
                                objTemp.Quantity = (decimal)app.Value;
                            }
                            else if (app.Key == "PV")
                            {
                                objTemp.PV = (decimal)app.Value;
                            }
                            else if (app.Key == "CV")
                            {
                                objTemp.CV = (decimal)app.Value;
                            }
                            else if (app.Key == "BV")
                            {
                                objTemp.BV = (decimal)app.Value;
                            }
                            else if (app.Key == "RP")
                            {
                                objTemp.RP = (decimal)app.Value;
                            }
                            else if (app.Key == "DP")
                            {
                                objTemp.DP = (decimal)app.Value;
                            }
                            else if (app.Key == "CVValue")
                            {
                                objTemp.CVValue = (decimal)app.Value;
                            }
                            else if (app.Key == "PVValue")
                            {
                                objTemp.PVValue = (decimal)app.Value;
                            }
                            else if (app.Key == "BVValue")
                            {
                                objTemp.BVValue = (decimal)app.Value;
                            }
                            else if (app.Key == "DiscPer")
                            {
                                objTemp.DiscPer = (decimal)app.Value;
                            }
                            else if (app.Key == "DiscAmt")
                            {
                                objTemp.DiscAmt = (decimal)app.Value;
                            }
                            else if (app.Key == "TaxAmt")
                            {
                                objTemp.TaxAmt = (decimal)app.Value;
                            }
                            else if (app.Key == "TaxPer")
                            {
                                objTemp.TaxPer = (decimal)app.Value;
                            }
                            else if (app.Key == "Amount")
                            {
                                objTemp.Amount = (decimal)app.Value;
                            }
                            else if (app.Key == "TotalAmount")
                            {
                                objTemp.TotalAmount = (decimal)app.Value;
                            }
                            else if (app.Key == "TaxType")
                            {
                                objTemp.TaxType = (string)app.Value;
                            }
                            else if (app.Key == "PVValue")
                            {
                                objTemp.DiscAmt = (decimal)app.Value;

                            }
                            else if (app.Key == "AvailStock")
                            {
                                objTemp.StockAvailable = (decimal)app.Value;
                            }
                            else if (app.Key == "CommsnPer")
                            {
                                objTemp.CommissionPer = (decimal)app.Value;
                            }
                            else if (app.Key == "CommsnAmt")
                            {
                                objTemp.CommissionAmt = (decimal)app.Value;
                            }
                            else if (app.Key == "RPValue")
                            {
                                objTemp.RPValue = (decimal)app.Value;
                            }
                            else if (app.Key == "OfferUID")
                            {
                                objTemp.OfferUID = (decimal)app.Value;
                            }
                            else if (app.Key == "ProductType")
                            {
                                objTemp.ProductType = app.Value.ToString();
                            }
                        }
                        objPartyOrderModel.objListProduct.Add(objTemp);
                    }
                    //objPartyOrderModel.LoginUser = Session["LoginUser"] as User;
                    objPartyOrderModel.LoginUser = HttpContext.Session.GetComplexData<User>("LoginUser");
                    // Retrive the Name of HOST
                    string hostName = Dns.GetHostName();
                    // Get the IP  
                    string myIP = Dns.GetHostEntry(hostName).AddressList[0].ToString();
                    string currentDate = DateTime.Now.ToString("yyyyMMddHHmmssfff"); ;
                    objPartyOrderModel.objProduct.UID = myIP + currentDate;
                    objResponse = await i_Transaction.SaveDispatchOrder(objPartyOrderModel);
                    //if (objResponse.ResponseStatus == "OK")
                    //{
                    //    InventorySession.StoredDistributorValues = objResponse.ResponseDetailsToPrint;
                    //}
                }

            }
            else
            {
                objResponse.ResponseMessage = "Something went wrong!";
                objResponse.ResponseStatus = "FAILED";
            }

            return Json(objResponse);
        }

        public async Task<IActionResult> InvoicePrint(string Pm)
        {
            DistributorBillModel model = new DistributorBillModel();
            var configurations = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var ShoppingUrl = configurations["ShoppingUrl"];
            ViewBag.CVCaption = configurations["CVCaption"];
            ViewBag.BVCaption = configurations["BVCaption"];
            ViewBag.PVCaption = configurations["PVCaption"];
            ViewBag.RPCaption = configurations["RPCaption"];
            ViewBag.JoiningCaption = configurations["JoiningCaption"];
            ViewBag.RepurchaseCaption = configurations["RepurchaseCaption"];
            ViewBag.WRPartyCode = configurations["WRPartyCode"];
            var base64DecodedBytes = System.Convert.FromBase64String(Pm);
            string BillNoValue = System.Text.Encoding.UTF8.GetString(base64DecodedBytes);
            string CurrentPartyCode = "";
            CurrentPartyCode = Convert.ToString(HttpContext.Session.GetString("PartyCode"));
            model = await i_Transaction.getInvoice(BillNoValue, CurrentPartyCode, "F");
            return View(model);
        }

        public async Task<IActionResult> GetInvoice(string BillNo)
        {
            DistributorBillModel model = new DistributorBillModel();
            var configurations = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var ShoppingUrl = configurations["ShoppingUrl"];
            ViewBag.CVCaption = configurations["CVCaption"];
            ViewBag.BVCaption = configurations["BVCaption"];
            ViewBag.PVCaption = configurations["PVCaption"];
            ViewBag.RPCaption = configurations["RPCaption"];
            ViewBag.JoiningCaption = configurations["JoiningCaption"];
            ViewBag.RepurchaseCaption = configurations["RepurchaseCaption"];
            ViewBag.WRPartyCode = configurations["WRPartyCode"];
            var base64DecodedBytes = System.Convert.FromBase64String(BillNo);
            string BillNoValue = System.Text.Encoding.UTF8.GetString(base64DecodedBytes);
            string CurrentPartyCode = "";
            var billsoldby = await i_Transaction.GetSoldBy(BillNoValue, "F");
            if (billsoldby != null && billsoldby.SoldBy != "")
            {
                CurrentPartyCode = billsoldby.SoldBy;
                BillNoValue = billsoldby.BillNo;
            }

            model = await i_Transaction.getInvoice(BillNoValue, CurrentPartyCode, "F");

            M_InvoiceDetail invdetail = new M_InvoiceDetail();
            invdetail.OrderNo = model.OrderNo;
            invdetail.CompanyName = model.CompanyName;
            invdetail.CompanyAdd = model.CompanyAdd;
            invdetail.BillNo = model.BillNo;
            invdetail.BillDate = model.BillDate;
            invdetail.GSTNo = model.GSTNo;
            invdetail.SoldBy = model.SoldBy;
            invdetail.SoldByName = model.SoldByName;
            invdetail.SoldByAddress = model.SoldByAddress;
            invdetail.SoldByCity = model.SoldByCity;
            invdetail.MobileNo = model.MobileNo;
            invdetail.BillDateStr = model.BillDateStr;
            invdetail.OldTaxAmount = model.OldTaxAmount;
            invdetail.NumberOfBill = model.NumberOfBill;
            invdetail.BillType = model.BillType;
            invdetail.offerId = model.offerId;
            invdetail.NewofferId = model.NewofferId;
            invdetail.StateGSTName = model.StateGSTName;
            invdetail.CompCity = model.CompCity;
            invdetail.TaxORStock = model.TaxORStock;
            invdetail.TaxType = model.TaxType;
            invdetail.Username = model.Username;
            invdetail.IsSequneceproduct = model.IsSequneceproduct;
            invdetail.UserId = model.UserId;
            invdetail.OfferName = model.OfferName;
            invdetail.DeliveryBy = model.DeliveryBy;
            invdetail.UserType = model.UserType;
            invdetail.IsGSTRegistered = model.IsGSTRegistered;
            invdetail.SelectedInvoiceType = model.SelectedInvoiceType;
            invdetail.FpVoucherAmt = model.FpVoucherAmt;
            invdetail.FpVoucher = model.FpVoucher;
            invdetail.CouponAmt = model.CouponAmt;
            invdetail.CouponCode = model.CouponCode;
            invdetail.PartyInvoice = model.PartyInvoice;
            invdetail.EInvoice = model.EInvoice;
            invdetail.AckNo = model.AckNo;
            invdetail.CINNo = model.CINNo;
            invdetail.Customer = model.objCustomer;
            invdetail.ProductList = model.objListProduct;
            invdetail.TaxSummary = model.objTaxSummary;
            invdetail.PaymentMode = model.objPaymentMode;
            invdetail.Producttotal = new ProductTotal();
            invdetail.Producttotal.TotalTaxPer = model.objProduct.TotalTaxPer;
            invdetail.Producttotal.TotalPayAmount = model.objProduct.TotalPayAmount;
            invdetail.Producttotal.TotalTaxAmount = model.objProduct.TotalTaxAmount;
            invdetail.Producttotal.TotalCGSTAmt = model.objProduct.TotalCGSTAmt;
            invdetail.Producttotal.TotalSGSTPer = model.objProduct.TotalSGSTPer;
            invdetail.Producttotal.TotalSGSTAmt = model.objProduct.TotalSGSTAmt;
            invdetail.Producttotal.TotalNetPayable = model.objProduct.TotalNetPayable;
            invdetail.Producttotal.TotalAmount = model.objProduct.TotalAmount;
            invdetail.Producttotal.Roundoff = model.objProduct.Roundoff;
            invdetail.Producttotal.TotalBV = model.objProduct.TotalBV;
            invdetail.Producttotal.TotalRP = model.objProduct.TotalRP;
            invdetail.Producttotal.TotalCGSTPer = model.objProduct.TotalCGSTPer;
            invdetail.Producttotal.TotalDiscount = model.objProduct.TotalDiscount;
            invdetail.Producttotal.TotalCV = model.objProduct.TotalCV;
            invdetail.Producttotal.TotalPV = model.objProduct.TotalPV;
            invdetail.Producttotal.TotalQty = model.objProduct.TotalQty;
            invdetail.Producttotal.TotalWeight = model.objProduct.TotalWeight;
            invdetail.Producttotal.TotalTotalAmount = model.objProduct.TotalTotalAmount;
            return Ok(invdetail);
        }

        public async Task<IActionResult> PartyBill()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Status")))
            {
                var configurations = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                var ShoppingUrl = configurations["ShoppingUrl"];
                ViewBag.CVCaption = configurations["CVCaption"];
                ViewBag.BVCaption = configurations["BVCaption"];
                ViewBag.PVCaption = configurations["PVCaption"];
                ViewBag.RPCaption = configurations["RPCaption"];
                ViewBag.BarcodeCaption = configurations["BarcodeCaption"];
                ViewBag.PartyCaption = configurations["PartyCaption"];
                DistributorBillModel objDistributorModel = new DistributorBillModel();
                objDistributorModel.objCustomer = new CustomerDetail();
                objDistributorModel.objProduct = new ProductModel();
                List<SelectListItem> objBankList = new List<SelectListItem>();
                var result = await i_Transaction.GetBankList();
                objDistributorModel.objProduct.PayDetails = new PayDetails();
                foreach (var obj in result)
                {
                    if (obj.BankCode == 0)
                    {
                        objBankList.Add(new SelectListItem { Text = obj.BankName, Value = obj.BankCode.ToString(), Selected = true });
                        objDistributorModel.objProduct.PayDetails.BDBankName = obj.BankCode.ToString();
                    }
                    else
                    {
                        objBankList.Add(new SelectListItem { Text = obj.BankName, Value = obj.BankCode.ToString() });
                    }
                }
                ViewBag.BankNames = objBankList;
                List<SelectListItem> CardTypes = new List<SelectListItem>();
                CardTypes.Add(new SelectListItem { Text = "Credit Card", Value = "CC" });
                CardTypes.Add(new SelectListItem { Text = "Debit Card", Value = "DB" });
                ViewBag.CardTypes = CardTypes;

                objDistributorModel.objProduct.PayDetails.CardType = "CC";

                //List<SelectListItem> objListCustomerTypes = new List<SelectListItem>();
                //objListCustomerTypes.Add(new SelectListItem { Text = "New", Value = "New" });
                //objListCustomerTypes.Add(new SelectListItem { Text = "Existing", Value = "Existing" });
                //ViewBag.CustomerType = objListCustomerTypes;

                //ViewBag.ConfigDetails = objTransacManager.GetConfigDetails();
                //objDistributorModel.objConfigDetails = objTransacManager.GetConfigDetails();

                objDistributorModel.objCustomer.CustomerType = "New";
                //objDistributorModel.objCustomer.IsRegisteredCustomer = true;
                //objDistributorModel.objCustomer.ReferenceIdNo = InventorySession.LoginUser.PartyCode;
                //objDistributorModel.objCustomer.ReferenceName = InventorySession.LoginUser.PartyName;
                return View(objDistributorModel);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public async Task<ActionResult> GetAvailStockProductNamesOnly(string StockforParty)
        {
            if (string.IsNullOrEmpty(StockforParty))
                StockforParty = Convert.ToString(HttpContext.Session.GetString("PartyCode"));

            List<string> model = await i_Transaction.GetAvailStockProductNamesOnly(StockforParty);
            return Json(model);
        }

        public async Task<ActionResult> GetAllPartywithBal()
        {
            List<PartyModel> objparty = new List<PartyModel>();
            string LoginPartyCode = "";
            decimal LoginStateCode = 0;
            if (Convert.ToString(HttpContext.Session.GetString("PartyCode")) != null)
            {
                LoginPartyCode = Convert.ToString(HttpContext.Session.GetString("PartyCode"));
                LoginStateCode = Convert.ToDecimal(HttpContext.Session.GetString("StateCode"));
            }
            objparty = await i_Transaction.GetAllPartyNew(LoginPartyCode, LoginStateCode, true);
            return Json(objparty);
        }


        public async Task<ActionResult> GetAllBarcode()
        {
            List<string> model = await i_Transaction.GetAllBarcode();
            return Json(model);
        }

        public async Task<IActionResult> GetWalletTypeBalance(string Shoppe, string Vtype)
        {
            string WalletBalance = "0";
            try
            {
                WalletBalance = Convert.ToString(await i_Product.GetPartyWalletBalance(Shoppe, Vtype));
            }
            catch (Exception ex)
            {

            }
            return Json(WalletBalance);
        }

        public async Task<IActionResult> CustomerBill()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Status")))
            {
                var configurations = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                var ShoppingUrl = configurations["ShoppingUrl"];
                ViewBag.CVCaption = configurations["CVCaption"];
                ViewBag.BVCaption = configurations["BVCaption"];
                ViewBag.PVCaption = configurations["PVCaption"];
                ViewBag.RPCaption = configurations["RPCaption"];
                ViewBag.BarcodeCaption = configurations["BarcodeCaption"];
                ViewBag.PartyCaption = configurations["PartyCaption"];
                DistributorBillModel objDistributorModel = new DistributorBillModel();
                objDistributorModel.objCustomer = new CustomerDetail();
                objDistributorModel.objProduct = new ProductModel();
                List<SelectListItem> objBankList = new List<SelectListItem>();
                var result = await i_Transaction.GetBankList();
                objDistributorModel.objProduct.PayDetails = new PayDetails();
                foreach (var obj in result)
                {
                    if (obj.BankCode == 0)
                    {
                        objBankList.Add(new SelectListItem { Text = obj.BankName, Value = obj.BankCode.ToString(), Selected = true });
                        objDistributorModel.objProduct.PayDetails.BDBankName = obj.BankCode.ToString();
                    }
                    else
                    {
                        objBankList.Add(new SelectListItem { Text = obj.BankName, Value = obj.BankCode.ToString() });
                    }
                }
                ViewBag.BankNames = objBankList;
                List<SelectListItem> CardTypes = new List<SelectListItem>();
                CardTypes.Add(new SelectListItem { Text = "Credit Card", Value = "CC" });
                CardTypes.Add(new SelectListItem { Text = "Debit Card", Value = "DB" });
                ViewBag.CardTypes = CardTypes;
                objDistributorModel.objProduct.PayDetails.CardType = "CC";
                objDistributorModel.objCustomer.CustomerType = "New";
                var objStateList = await i_Transaction.GetStateList();
                List<SelectListItem> StateList = new List<SelectListItem>();
                foreach (var obj in objStateList)
                {
                    if (obj.StateCode != 0)
                    {
                        StateList.Add(new SelectListItem
                        {
                            Text = obj.StateName,
                            Value = obj.StateCode.ToString()
                        });
                    }
                }
                ViewBag.StateList = StateList;
                return View(objDistributorModel);
            }
            return RedirectToAction("Login", "Account");
        }

        public async Task<ActionResult> GetAllProductNames(string InvType)
        {
            List<string> model = await i_Transaction.GetAutocompleteProductNames(InvType);
            return Json(model);
        }

        public async Task<ActionResult> CheckBillCustomer(string mobile)
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse = await i_Transaction.CheckBillCustomer(mobile);
            return Json(objResponse);
        }

    }
}
