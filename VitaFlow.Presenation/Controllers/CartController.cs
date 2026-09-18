using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using VitaFlow.Domain.Entities;
using VitaFlow.Domain.Interface;
using VitaFlow.Presenation.Extensions;

namespace VitaFlow.Presenation.Controllers
{
    public class CartController : Controller
    {

        private readonly ILogger<CartController> _logger;
        private readonly I_Product i_Product;
        private readonly I_Login i_Login_Service;
        private readonly I_Report ireport;
        public CartController(ILogger<CartController> logger, I_Product iProduct, I_Login iLoginService, I_Report i_Report)
        {
            _logger = logger;
            i_Product = iProduct;
            i_Login_Service = iLoginService;
            ireport = i_Report;
        }
        public async Task<IActionResult> ViewCart()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Status")))
            {
                int Selectedpackageid = 0;
                Selectedpackageid = await i_Product.CheckPackageSelection(Convert.ToString(HttpContext.Session.GetString("FCode")));
                if (Selectedpackageid == 0)
                {
                    return RedirectToAction("PackageSelection", "Cart");
                }
                var result = await i_Product.GetPartyPackageAmount(Convert.ToString(HttpContext.Session.GetString("FCode")));

                M_CartDetails req = new M_CartDetails();
                req.Action = "srchCartDetails";
                req.userid = HttpContext.Session.GetString("UserId");
                req.FCode = HttpContext.Session.GetString("FCode");
                req.ParentPartycode = HttpContext.Session.GetString("ParentPartyCode");
                CartDetails obj = new CartDetails();
                obj.CartList = await i_Product.CartDetails(req);
                obj.TotalAmont = Convert.ToDecimal(obj.CartList.Sum(p => p.TotalProductamount));
                var GetPartyOrderlist = await ireport.GetOrderList(HttpContext.Session.GetString("FCode"));
                if (GetPartyOrderlist.Count() == 0)
                {
                    if (obj.TotalAmont <= result.PackageAmount)
                    {
                        HttpContext.Session.SetString("lessPackageAmount", Convert.ToString(result.PackageAmount - obj.TotalAmont));
                    }
                    else
                    {
                        HttpContext.Session.SetString("lessPackageAmount", Convert.ToString("0"));
                    }
                }

                return View(obj);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }

        public async Task<IActionResult> Updatecartproduct(string Productid, string Qty)
        {

            string msg = string.Empty;
            string code = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Status")))
                {
                    M_CartDetails req = new M_CartDetails();
                    req.Action = "updateQuantity";
                    req.userid = HttpContext.Session.GetString("UserId");
                    req.ProdId = Productid;
                    req.qty = Convert.ToDecimal(Qty);
                    var response = await i_Product.Updatecart(req);
                    if (response == "U")
                    {
                        msg = "Quntity updated";
                        code = "1";
                    }
                    else
                    {
                        msg = "Quntity not updated";
                        code = "0";
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Json(new { msg, code });
        }

        public async Task<IActionResult> DeleteCartDeatils(int Cid)
        {
            string msg = string.Empty;
            string code = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Status")))
                {
                    M_CartDetails req = new M_CartDetails();
                    req.Action = "deleteCartDeatils";
                    req.id = Cid;
                    var response = await i_Product.DeleteCart(req);
                    if (response == "S")
                    {
                        msg = "Row remove successfully";
                        code = "1";
                    }
                    else
                    {
                        msg = "Something went wrong";
                        code = "0";
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Json(new { msg, code });
        }

        [HttpPost]
        public async Task<IActionResult> Checkpackage()
        {
            string status = "0";
            try
            {
                int Selectedpackageid = 0;
                Selectedpackageid = await i_Product.CheckPackageSelection(Convert.ToString(HttpContext.Session.GetString("FCode")));
                if (Selectedpackageid == 0)
                {
                    status = "0";
                }
                else
                {
                    int GetOrderCount = 0;
                    //Get order count from OrderHistroy table
                    GetOrderCount = await i_Product.GetOrderCount(Convert.ToString(HttpContext.Session.GetString("FCode")));
                    if (GetOrderCount <= 0)
                    {
                        M_CartDetails Creq = new M_CartDetails();
                        Creq.Action = "srchCartDetails";
                        Creq.userid = HttpContext.Session.GetString("UserId");
                        Creq.FCode = HttpContext.Session.GetString("FCode");
                        Creq.ParentPartycode = HttpContext.Session.GetString("ParentPartyCode");
                        CartDetails obj = new CartDetails();
                        obj.CartList = await i_Product.CartDetails(Creq);
                        obj.TotalAmont = Convert.ToDecimal(obj.CartList.Sum(p => p.TotalProductamount));
                        PackageMasterDetail package = new PackageMasterDetail();
                        package = await ireport.GetPartywisePackage(Creq.FCode);
                        if (Convert.ToDecimal(obj.TotalAmont) >= Convert.ToDecimal(package.PackageAmount))
                        {
                            status = "2";
                        }
                        else
                        {
                            status = "1";
                        }
                    }
                    else
                    {
                        status = "2";
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Json(new { status });
        }
        public async Task<IActionResult> GetWalletBalance()
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
        public async Task<IActionResult> GetPVWalletBalance()
        {
            string WalletPVBalance = "0";
            try
            {
                WalletPVBalance = Convert.ToString(await i_Product.GetPartyWalletBalance(HttpContext.Session.GetString("FCode"), "P"));
            }
            catch (Exception ex)
            {

            }
            return Json(WalletPVBalance);
        }

        public async Task<IActionResult> GetPurchaseWalletBalance(string Wallettype)
        {
            string WalletPVBalance = "0";
            try
            {
                WalletPVBalance = Convert.ToString(await i_Product.GetPartyWalletBalance(HttpContext.Session.GetString("FCode"), Wallettype));
            }
            catch (Exception ex)
            {

            }
            return Json(WalletPVBalance);
        }

        public async Task<IActionResult> Checkout()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Status")))
            {
                HttpContext.Session.SetString("promobalance", "0");
                int Selectedpackageid = 0;
                Selectedpackageid = await i_Product.CheckPackageSelection(Convert.ToString(HttpContext.Session.GetString("FCode")));
                if (Selectedpackageid == 0)
                {
                    return RedirectToAction("PackageSelection", "Cart");
                }
                string isfirstorder = "Yes";
                string Ordermethod = "";
                ShippingDetail req = new ShippingDetail();
                req.Userid = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
                ShippingDetail obj = new ShippingDetail();
                var response = await i_Product.GetShippingDetail(req);
                if (response != null)
                {
                    obj = response;
                }
                else
                {
                    obj.BusinessName = Convert.ToString(HttpContext.Session.GetString("PartyName"));
                    obj.Address1 = Convert.ToString(HttpContext.Session.GetString("Address1"));
                    obj.Pincode = Convert.ToString(HttpContext.Session.GetString("Pincode"));
                    obj.MobileNo = Convert.ToString(HttpContext.Session.GetString("MobileNo"));
                    obj.Email = Convert.ToString(HttpContext.Session.GetString("E_MailAdd"));
                }
                obj.Statelist = await i_Login_Service.GetstateList();
                if (obj.Statelist != null)
                {
                    var stateremove = obj.Statelist.Single(r => r.StateCode == 0);
                    obj.Statelist.Remove(stateremove);
                    if (response == null)
                    {
                        obj.StateCode = Convert.ToInt32(HttpContext.Session.GetString("StateCode"));
                        obj.CityList = await i_Product.GetCityList(Convert.ToString(HttpContext.Session.GetString("StateCode")));
                    }
                    else
                    {
                        obj.CityList = await i_Product.GetCityList(Convert.ToString(obj.StateCode));
                    }
                }
                int GetOrderCount = 0;
                //Get order count from OrderHistroy table
                GetOrderCount = await i_Product.GetOrderCount(Convert.ToString(HttpContext.Session.GetString("FCode")));
                if (GetOrderCount > 0)
                {
                    isfirstorder = "No";
                }
                if (isfirstorder == "No")
                {
                    var res = await i_Product.GetorderMethodSelection(Convert.ToInt32(HttpContext.Session.GetString("UserId")));
                    if (res != null)
                    {
                        Ordermethod = res;
                        obj.OrderMethod = Ordermethod;
                    }
                }
                obj.IsFirstOrder = isfirstorder;
                M_CartDetails Creq = new M_CartDetails();
                Creq.Action = "srchCartDetails";
                Creq.userid = HttpContext.Session.GetString("UserId");
                Creq.FCode = HttpContext.Session.GetString("FCode");
                Creq.ParentPartycode = HttpContext.Session.GetString("ParentPartyCode");
                obj.CartList = await i_Product.CartDetails(Creq);
                obj.TotalAmont = Convert.ToDecimal(obj.CartList.Sum(p => p.TotalProductamount));
                return View(obj);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        public async Task<IActionResult> PackageSelection()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Status")))
            {
                int Selectedpackageid = 0;
                Selectedpackageid = await i_Product.CheckPackageSelection(Convert.ToString(HttpContext.Session.GetString("FCode")));
                if (Selectedpackageid != 0)
                {
                    return RedirectToAction("Productlist", "Product");
                }

                M_PackageMaster obj = new M_PackageMaster();
                obj.PackageMasters = await ireport.GetPackageList(Convert.ToInt32(HttpContext.Session.GetString("GroupId")));
                return View(obj);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        public async Task<IActionResult> GetCity(string Statecode)
        {
            var CityList = await i_Product.GetCityList(Statecode);
            return Json(CityList);
        }

        [HttpPost]
        public async Task<IActionResult> SaveAddressdetail(string BusinessName, string CustomerName,
            string Address1, string Address2, string StateCode, string CityCode,
              string pincode, string MobileNo, string Email, string OrderMethod, string promowalletBal)
        {
            string msg = string.Empty;
            string statuscode = "0";
            string isfirstorder = "Yes";
            string IsPVorder = "No";
            string IsBVorder = "No";
            string Ordermethod = "";
            try
            {
                HttpContext.Session.SetString("promobalance", promowalletBal);
                ShippingDetail shipping = new ShippingDetail();
                shipping.BusinessName = BusinessName;
                shipping.CustomerName = CustomerName;
                shipping.Address1 = Address1;
                shipping.StateCode = Convert.ToInt32(StateCode);
                shipping.CityCode = Convert.ToInt32(CityCode);
                shipping.Pincode = pincode;
                shipping.MobileNo = MobileNo;
                shipping.Email = Email;
                if (Address2 == null)
                {
                    shipping.Address2 = "";
                }
                shipping.Userid = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
                var response = await i_Product.SaveFranchiseShippingDetail(shipping);
                if (response != "")
                {
                    statuscode = "1";
                    msg = "Shipping detail save";
                    int GetOrderCount = 0;
                    //Get order count from db
                    GetOrderCount = await i_Product.GetOrderCount(Convert.ToString(HttpContext.Session.GetString("FCode")));
                    //if (GetOrderCount > 0)
                    //{
                    //    isfirstorder = "No";
                    //}
                    //if (isfirstorder == "No")
                    //{
                    // save order method in table 
                    var str = await i_Product.SaveOrderMethod(Convert.ToInt32(HttpContext.Session.GetString("UserId")), OrderMethod);
                    //}
                }
                else
                {
                    statuscode = "0";
                    msg = "Shipping detail not save";
                }
            }
            catch (Exception ex)
            {
                statuscode = "0";
            }
            return Json(new { statuscode, msg });
        }

        [HttpPost]
        public async Task<IActionResult> SavePackage(int Packageid)
        {
            string msg = string.Empty;
            try
            {
                int i = await i_Product.SaveFranchisepackage(Convert.ToString(HttpContext.Session.GetString("FCode")), Packageid);
                if (i > 0)
                {
                    msg = "1";
                }
                else
                {
                    msg = "0";
                }
            }
            catch (Exception ex)
            {
                msg = "0";
            }
            return Json(new { msg });
        }
        public async Task<IActionResult> SaveOrder()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Status")))
            {
                OrderReq orderreq = new OrderReq();

                ResponseDetail objResponse = new ResponseDetail();
                List<M_CartDetails> objProductModel = new List<M_CartDetails>();

                decimal Walletbalance = 0;
                int Selectedpackageid = 0;
                Selectedpackageid = await i_Product.CheckPackageSelection(Convert.ToString(HttpContext.Session.GetString("FCode")));
                if (Selectedpackageid == 0)
                {
                    return RedirectToAction("PackageSelection", "Cart");
                }
                var result = await i_Product.GetPartyPackageAmount(Convert.ToString(HttpContext.Session.GetString("FCode")));

                var GetPartyOrderlist = await ireport.GetOrderList(HttpContext.Session.GetString("FCode"));
                var ordermethod = await i_Product.GetorderMethodSelection(Convert.ToInt32(HttpContext.Session.GetString("UserId")));
                if (ordermethod != null)
                {
                    orderreq.OrderMethod = ordermethod;
                }
                else
                {
                    orderreq.OrderMethod = "";
                }
                if (ordermethod == "BV")
                {
                    Walletbalance = await i_Product.GetPartyWalletBalance(HttpContext.Session.GetString("FCode"), "Z");
                    orderreq.wallettype = "Z";

                    decimal Totaladjustamount = 0;
                    Totaladjustamount = Convert.ToDecimal(HttpContext.Session.GetString("promobalance"));
                    Walletbalance = Walletbalance - Totaladjustamount;
                    orderreq.Promobalance = Totaladjustamount;
                }
                else
                {
                    Walletbalance = await i_Product.GetPartyWalletBalance(HttpContext.Session.GetString("FCode"), "W");
                    orderreq.wallettype = "W";
                }
                //Walletbalance = await i_Product.GetPartyWalletBalance(HttpContext.Session.GetString("FCode"), "R");
                //orderreq.wallettype = "R";
                if (Walletbalance > 0)
                {
                    //--------------------------------------------------------------
                    M_CartDetails req = new M_CartDetails();
                    req.Action = "GetCartOrderDetail";
                    req.userid = HttpContext.Session.GetString("UserId");
                    req.FCode = HttpContext.Session.GetString("FCode");

                    var TempResult = await i_Product.CartDetails(req);
                    //----------------------------------------------------------
                    orderreq.OrderBy = HttpContext.Session.GetString("FCode");
                    orderreq.OrderTo = Convert.ToString(HttpContext.Session.GetString("ParentPartyCode"));
                    bool IsDistributorBill = false;
                    bool IsPartyBill = false;
                    bool IsCustomerBill = false;
                    bool IsPurchaseInvoice = false;
                    bool IsOrderCreation = true;
                    bool IsPendingOrder = false;
                    bool IsBillOnMrp = false;
                    decimal CurrentStateCode;
                    bool allhalf = false;
                    string Invoice = "";

                    //discount calculation
                    decimal Totaladjustamount = 0;
                    Totaladjustamount = Convert.ToDecimal(HttpContext.Session.GetString("promobalance"));
                    if (ordermethod == "BV")
                    {
                        if (Totaladjustamount != 0)
                        {
                            var totalDP = (TempResult.Sum(p => p.DPValue));
                            decimal DiscPercent = 0;
                            DiscPercent = (Totaladjustamount / totalDP) * 100;
                            DiscPercent = Math.Round(DiscPercent, 2);
                            foreach (var item in TempResult)
                            {

                                //ProductGrid[i].DiscPer = parseFloat(DiscPer);
                                var DiscountAmt = (item.DP * item.qty) * (DiscPercent / 100);
                                DiscountAmt = Math.Round(DiscountAmt, 2);
                                item.DiscAmt = DiscountAmt;
                                var NETDP = item.DP - (item.DP * (DiscPercent / 100));
                                NETDP = Math.Round(NETDP, 2);
                                var TaxPer = item.TaxPer / 100;
                                var Price = ((NETDP * item.qty) * 100) / (item.TaxPer + 100);
                                Price = Math.Round(Price, 2);
                                var Taxamount = Price * TaxPer;
                                var finalamount = NETDP * item.qty;
                                finalamount = Math.Round(finalamount, 2);
                                // var Amount=ProductGrid[i].DP-Taxamount;
                                // ProductGrid[i].Amount=Amount.toFixed(2);
                                item.TaxAmt = Taxamount;
                                item.TotalAmt = finalamount;
                                //item.DP = NETDP;
                                item.Price = Price;
                                item.Rate = Price;
                                item.Amount = Price;
                                //item.DPValue = item.DP * item.qty;
                                item.DP1 = item.DP;
                                item.DiscPer = DiscPercent;
                            }
                            objProductModel = TempResult;
                        }
                    }


                    if (Totaladjustamount == 0)
                    {
                        foreach (var item in TempResult)
                        {
                            M_CartDetails TempObj = new M_CartDetails();
                            //if ((item.IsExpirable && item.ExpDate > DateTime.Now) || (item.IsExpirable == false))
                            //{
                            TempObj = item;
                            object valueIsDiscountAdd = 0;
                            object valueIsCommissonAdd = 0;
                            if (IsDistributorBill || IsCustomerBill || IsPurchaseInvoice || IsOrderCreation || IsPendingOrder)
                            {
                                valueIsCommissonAdd = Enum.Parse(typeof(VitaFlow.Domain.CommonDTO.EnumCalculation.Enums.CalculationConditionalVar), VitaFlow.Domain.CommonDTO.EnumCalculation.Enums.CalculationConditionalVar.IsCommissonAdd.ToString());
                                valueIsDiscountAdd = Enum.Parse(typeof(VitaFlow.Domain.CommonDTO.EnumCalculation.Enums.CalculationConditionalVar), VitaFlow.Domain.CommonDTO.EnumCalculation.Enums.CalculationConditionalVar.IsDiscountAdd.ToString());
                            }
                            else
                            {
                                valueIsCommissonAdd = Enum.Parse(typeof(VitaFlow.Domain.CommonDTO.EnumCalculation.Enums.CalculationConditionalVar), VitaFlow.Domain.CommonDTO.EnumCalculation.Enums.CalculationConditionalVar.IsCommissonAddOnPartyBill.ToString());
                                valueIsDiscountAdd = Enum.Parse(typeof(VitaFlow.Domain.CommonDTO.EnumCalculation.Enums.CalculationConditionalVar), VitaFlow.Domain.CommonDTO.EnumCalculation.Enums.CalculationConditionalVar.IsDiscountAddOnPartyBill.ToString());
                            }
                            int IsCommission = Convert.ToInt32(valueIsCommissonAdd);
                            int IsDiscount = Convert.ToInt32(valueIsDiscountAdd);
                            TempObj.IsCommissionAdd = IsCommission;
                            TempObj.IsDiscountAdd = IsDiscount;
                            TempObj.DP1 = TempObj.DP;
                            if (IsCustomerBill)
                            {
                                TempObj.DP = item.MRP;
                            }
                            else
                            {
                                if (!IsPurchaseInvoice && IsBillOnMrp)
                                {
                                    TempObj.DP = item.MRP;
                                }
                            }
                            M_CompanyMaster comp = new M_CompanyMaster();
                            comp = await i_Product.GetCompanyDetail();
                            CurrentStateCode = comp.CompState;
                            if (allhalf)
                            {
                                TempObj.DP = TempObj.DP / 2;
                                TempObj.bv = TempObj.bv / 2;
                                TempObj.PV = TempObj.PV / 2;
                                TempObj.RP = TempObj.RP / 2;
                                TempObj.DiscAmt = TempObj.DP;
                                TempObj.IsDiscountAdd = 1;
                            }
                            if (!string.IsNullOrEmpty(Invoice) && Invoice == "CED")
                            {
                                var oridp = TempObj.DP;
                                TempObj.DP = (TempObj.DP * 1) / 4;
                                TempObj.bv = 0;
                                TempObj.PV = (TempObj.PV * 1) / 4;
                                TempObj.RP = (TempObj.RP * 1) / 4;
                                TempObj.DiscAmt = oridp - TempObj.DP;
                                TempObj.IsDiscountAdd = 1;
                            }
                            decimal dpValue = 0;
                            if (IsCommission == 1)
                            {
                                var Commssn = TempObj.DP * TempObj.CommissionPer / 100;
                                dpValue = TempObj.DP - Commssn;
                                var CommissionLessDp = dpValue;
                                TempObj.CommssnAmt = Commssn * TempObj.qty;
                            }
                            else
                            {
                                dpValue = TempObj.DP;
                            }
                            var DiscPer = TempObj.DiscPer;
                            var DiscAmt = TempObj.DiscAmt;

                            if (orderreq.Promobalance <= 0)
                            {
                                if (TempObj.IsDiscountAdd == 1)
                                {
                                    if (DiscAmt == 0 && DiscPer != 0)
                                    {
                                        var Discount = dpValue * DiscPer / 100;
                                        dpValue = dpValue - Discount;
                                        TempObj.DiscAmt = Discount * TempObj.qty;
                                    }
                                    else if (DiscPer == 0 && DiscAmt != 0)
                                    {
                                        var Discount = dpValue * DiscAmt;
                                        dpValue = dpValue - Discount;
                                        TempObj.DiscAmt = Discount * TempObj.qty;
                                    }
                                    else
                                    {
                                        TempObj.DiscAmt = 0;
                                    }
                                }
                                else
                                {
                                    TempObj.DiscAmt = 0;
                                }
                            }


                            TempObj.Rate = dpValue * 100 / (100 + TempObj.TaxPer);
                            TempObj.Amount = dpValue * TempObj.qty * 100 / (100 + TempObj.TaxPer);
                            TempObj.TaxAmt = (dpValue * TempObj.qty) - TempObj.Amount;
                            var temp = Math.Round(TempObj.TaxAmt, 2).ToString("F2");
                            var lastNum = Convert.ToInt32(temp[temp.Length - 1]);
                            if ((Convert.ToInt32(lastNum) % 2) != 0)
                            {
                                TempObj.Amount = TempObj.Amount - 0.01m;
                                TempObj.TaxAmt = TempObj.TaxAmt - 0.01m;
                            }
                            else
                            {
                                TempObj.Amount = Math.Round(TempObj.Amount, 2);
                                TempObj.TaxAmt = Math.Round(TempObj.TaxAmt, 2);
                            }
                            TempObj.TotalAmt = (dpValue * TempObj.qty);

                            // }
                            objProductModel.Add(TempObj);
                        }
                    }



                    if (ordermethod == "BV")
                    {
                        orderreq.NetAmount = (objProductModel.Sum(p => p.TotalAmt));
                    }
                    else
                    {
                        orderreq.NetAmount = (objProductModel.Sum(p => p.DPValue));
                    }

                    orderreq.Orderdetail = objProductModel;
                    orderreq.OrderNo = await i_Product.GetOrderNo(HttpContext.Session.GetString("FCode"));
                    orderreq.IsP = true;
                    orderreq.TotalBV = objProductModel.Sum(p => p.BVValue);
                    orderreq.TotalRP = objProductModel.Sum(p => p.RPValue);
                    orderreq.TotalPV = objProductModel.Sum(p => p.PVValue);
                    orderreq.TotalTaxAmount = objProductModel.Sum(p => p.TaxAmt);
                    orderreq.TotalAmount = objProductModel.Sum(p => p.Amount);
                    orderreq.AmountByPaytm = 0;
                    orderreq.TotalQty = Convert.ToInt32(objProductModel.Sum(p => p.qty));
                    orderreq.UserId = Convert.ToDecimal(HttpContext.Session.GetString("UserId"));
                    orderreq.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
                    orderreq.PartyName = Convert.ToString(HttpContext.Session.GetString("PartyName"));
                    orderreq.GroupId = Convert.ToInt32(HttpContext.Session.GetString("GroupId"));

                    HttpContext.Session.SetString("OrderNo", orderreq.OrderNo);
                    if (Walletbalance >= orderreq.NetAmount)
                    {
                        //save order detail---------------------------------------------
                        objResponse = await i_Product.SavePartyOrderDetails(orderreq);
                    }
                    else
                    {
                        objResponse.ResponseStatus = "FAILED";
                        objResponse.ResponseMessage = "Sorry!Insufficient Wallet Balance.";
                        objResponse.StatusCode = 101;
                    }
                    //if (result.PackageAmount > (orderreq.NetAmount + orderreq.Promobalance) && GetPartyOrderlist.Count == 0)
                    //{
                    //    objResponse.ResponseStatus = "FAILED";
                    //    objResponse.ResponseMessage = "Sorry!Your cart amount is less than selected package";
                    //    objResponse.StatusCode = 101;
                    //}
                    //else
                    //{

                    //}
                }
                else
                {
                    objResponse.ResponseStatus = "FAILED";
                    objResponse.ResponseMessage = "Sorry!Insufficient Wallet Balance.";
                    objResponse.StatusCode = 101;
                }
                HttpContext.Session.SetComplexData("OrderResponse", objResponse);
                return RedirectToAction("Thankyouorder", "Cart");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        public async Task<IActionResult> BvOrder()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Status")))
            {

                M_CartDetails Creq = new M_CartDetails();
                Creq.Action = "srchCartDetails";
                Creq.userid = HttpContext.Session.GetString("UserId");
                Creq.FCode = HttpContext.Session.GetString("FCode");
                var CartList = await i_Product.CartDetails(Creq);
                ViewBag.TotalAmont = Convert.ToDecimal(CartList.Sum(p => p.TotalProductamount));
                ViewBag.Walletbalance = await i_Product.GetPartyWalletBalance(HttpContext.Session.GetString("FCode"), "R");
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }
        public async Task<IActionResult> Thankyouorder()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Status")))
            {
                ResponseDetail obj = new ResponseDetail();
                var Robj = HttpContext.Session.GetComplexData<ResponseDetail>("OrderResponse");
                if (Robj != null)
                {
                    TrnPartyOrderDetail req = new TrnPartyOrderDetail();
                    req.UserId = Convert.ToDecimal(HttpContext.Session.GetString("UserId"));
                    req.OrderNo = HttpContext.Session.GetString("OrderNo");
                    obj = Robj;
                    obj.Comapnydetail = await i_Product.GetCompanyDetail();
                    obj.OrderConfirmation = await ireport.GetOrderdetail(req);
                }
                return View(obj);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
    }
}
