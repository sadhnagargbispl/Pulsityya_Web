using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shopinv.Entity;
using Shopinv.Interface;
using Shopinv.Models;
using Shopinv.SiteExtension;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Xml;

namespace Shopinv.Controllers
{
    [KycRequired]
    public class CheckOutController : Controller
    {
        private readonly I_Category icateogry = null;
        private readonly I_Product iprod = null;
        private readonly I_Login _ilogin = null;
        private readonly I_PayMode ipaymode = null;
        CompanyDetail companyDetail;
        private readonly string IsTest = "True";
        private readonly static string CPanelUrl = System.Configuration.ConfigurationManager.AppSettings["CPanelUrl"];
        private readonly static string CashfreeOrderUrl = System.Configuration.ConfigurationManager.AppSettings["CashfreeOrderUrl"];
        private readonly static string CashfreeReturnUrl = System.Configuration.ConfigurationManager.AppSettings["CashfreeReturnUrl"];
        private readonly static string CashfreeSandboxOrderUrl = System.Configuration.ConfigurationManager.AppSettings["CashfreeSandboxOrderUrl"];
        private readonly static string CashfreeSandboxReturnUrl = System.Configuration.ConfigurationManager.AppSettings["CashfreeSandboxReturnUrl"];
        public CheckOutController(I_Category icateogry, I_Product iprod, I_Login _ilogin, I_PayMode ipaymode)
        {
            this.icateogry = icateogry;
            this.iprod = iprod;
            this._ilogin = _ilogin;
            this.ipaymode = ipaymode;
            companyDetail = new CompanyDetail(this.iprod);
            companyDetail.GetCompanydetail();
        }
        public ActionResult CheckOut(M_Category objg)
        {
            var UserName = Convert.ToString(Session["IDNO"]);
            var Password = Convert.ToString(Session["password"]);

            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
            {
                return RedirectToAction("Index", "Home");
            }
            var Id = Convert.ToString(Session["UserId"]);
            objg.RegisterUserDetails = _ilogin.GetUserLoginDetail(UserName, Password, Id);
            objg.userOtherDetail = _ilogin.GetUserotherLoginDetail(UserName, Password, Id);
            Session["Registeruser"] = objg.RegisterUserDetails;
            objg.DDLState = _ilogin.GetDDLState();
            objg.CartDetail = (IEnumerable<E_CartDetails>)Session["Cartdetailsftch"];
            if (Convert.ToDecimal(Session["couponamount"]) == 0)
            {
                Session["couponamount"] = 0;
                Session["coupon"] = "";
                Session["totalamount"] = objg.CartDetail.Sum(s => s.Price * s.qty).ToString();
                Session["totalbv"] = objg.CartDetail.Sum(s => s.Price * s.qty).ToString();
                Session["totalpv"] = objg.CartDetail.Sum(s => s.PV * s.qty).ToString();
            }

            decimal CourierCharge = 0;
            if (Convert.ToDecimal(Session["totalamount"]) < 5000)
            {
                int totalqty = Convert.ToInt32(objg.CartDetail.Sum(s => s.qty));
                CourierCharge = 65 * totalqty;
            }

            Session["CourierCharge"] = CourierCharge;
            ViewBag.StateList = objg.DDLState;
            if (objg.RegisterUserDetails != null)
            {
                objg.PlacerOrder = objg.RegisterUserDetails.FirstOrDefault();
            }

            if (objg.userOtherDetail != null)
            {
                if (objg.userOtherDetail.Count() == 0)
                {
                    objg.otherdetailUser = objg.RegisterUserDetails.FirstOrDefault();
                }
                else
                {
                    objg.otherdetailUser = objg.userOtherDetail.FirstOrDefault();
                }
                //objg.otherdetailUser = objg.userOtherDetail.FirstOrDefault();
            }

            var userid = Session["UserId"];
            var Sessionid = Session["CurrentUserSessionID"];

            DataSet ds = iprod.GetAllparty();
            List<SelectListItem> ParentParty = new List<SelectListItem>();
            List<SelectListItem> DeliveryAddress = new List<SelectListItem>();
            DeliveryAddress.Add(new SelectListItem { Text = "Select Address", Value = "0" });//ALL
            DeliveryAddress.Add(new SelectListItem { Text = "Self PickUP", Value = "1" });//ALL
            DeliveryAddress.Add(new SelectListItem { Text = "By Courier", Value = "2" });
            ViewBag.DeliveryAddressList = DeliveryAddress;
            if (ds.Tables[0].Rows.Count > 0)
            {
                ParentParty.Add(new SelectListItem
                {
                    Text = "--Select --",
                    Value = "0"
                });
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    ParentParty.Add(new SelectListItem
                    {
                        Text = Convert.ToString(row["Party"]),

                        Value = Convert.ToString(row["PartyCode"])
                    });
                }
            }
            Session["ParentPartyList"] = ParentParty;
            ViewBag.ParentPartyList = ParentParty;
            Session["IsSelfpickup"] = "N";
            return View(objg);
        }

        [HttpPost]
        public ActionResult SaveAddressDetail(M_Category obj, string Action, string FirstName, string Lastname, string Mobile,
            string StateCode, string District, string City, string Address, string PinCode, string PartyCode, string Email,
            string AlternateMobileno, string BillingAddress, string BillingCity,
            string BillingPinCode, string BillingStateCodebState, string Kitbilltype)
        {
            var UserName = Convert.ToString(Session["IDNO"]);
            var Password = Convert.ToString(Session["password"]);
            var Id = Convert.ToString(Session["UserId"]);
            var FormNo = Convert.ToString(Session["FormNo"]);
            Session["PartyCode"] = PartyCode;
            obj.RegisterUserDetails = _ilogin.SaveAddressDetail(Action, Id, UserName, Password, Email, FirstName,
                Lastname, Mobile, FormNo, StateCode, District, City, Address, PinCode,
                 AlternateMobileno, BillingAddress, BillingCity,
              BillingPinCode, BillingStateCodebState);
            var cartCount = obj.RegisterUserDetails != null ? obj.RegisterUserDetails.Count() : 0;
            //DataSet ds = pack.getPackage(FormNo);
            var msg = "NotExist";
            Session["Kitbilltype"] = Kitbilltype;
            if (Convert.ToString(Session["MemMode"]) == "D")
            {
                obj.CartDetail = (IEnumerable<E_CartDetails>)Session["Cartdetailsftch"];
                decimal totalbv = obj.CartDetail.Sum(s => (s.Price * s.qty));
                DataSet dscount = iprod.UpdateKitOnPurchaseUpdate(Convert.ToInt32(Session["FormNo"]), totalbv, Kitbilltype);
                if (dscount.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToString(dscount.Tables[0].Rows[0]["Status"]) == "FAILED")
                    {
                        msg = Convert.ToString(dscount.Tables[0].Rows[0]["msg"]);
                    }
                    if (Convert.ToString(dscount.Tables[0].Rows[0]["Status"]) == "SUCCESS")
                    {
                        Session["Newkitid"] = Convert.ToString(dscount.Tables[0].Rows[0]["NewKitID"]);
                    }
                }
            }
else
            {
                Session["Newkitid"] = "0";
            }
                return Json(new { cartCount, msg });
        }
        public ActionResult PaymentProceed(M_Category obj)
        {
            //DataSet dstrn = ipaymode.Get_MTRNCharge();
            //if (dstrn.Tables.Count > 0)
            //{
            //    //get merchant charge from db
            //    Session["transationcharge"] = Convert.ToDecimal(dstrn.Tables[0].Rows[0]["transationcharge"]);
            //    Session["MinBillValue"] = Convert.ToDecimal(dstrn.Tables[0].Rows[0]["MinBillValue"]);
            //}
            if (Session["UserId"] != null)
            {
                string randomNumber = DateTime.Now.ToString("mmssfff");
                Session["Randomordernumber"] = randomNumber;
                return View(obj);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        public ActionResult PaymentOfflineProceed(HttpPostedFileBase file, string Txnno)
        {
            var paymentid = "";
            M_Category obj = new M_Category();
            string message = "";
            string status = "false";
            int id = 0;
            try
            {
                string Amount = Convert.ToString(Session["totalamount"]);
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    DataSet dstrno = iprod.CheckTxno(Txnno);
                    if (dstrno != null && dstrno.Tables.Count > 0 && dstrno.Tables[0].Rows.Count > 0)
                    {
                        message = "Same Utr no. already exists";
                        return Json(new { message, status, id });
                    }

                    bool sta = false;
                    var randomordernumber = Convert.ToInt32(Session["Randomordernumber"]);
                    sta = iprod.SaveTransactionOrder(randomordernumber);
                    if (sta == true)
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            string uploadDir = Server.MapPath("~/UploadedPaymentImages/");
                            if (!Directory.Exists(uploadDir))
                                Directory.CreateDirectory(uploadDir);

                            string fileName = Path.GetFileName(file.FileName);
                            string filePath = Path.Combine(uploadDir, fileName);
                            file.SaveAs(filePath);


                            string Paymentimg = "UploadedPaymentImages/" + fileName;
                            string hostName = Dns.GetHostName();
                            string myIP = Dns.GetHostEntry(hostName).AddressList[0].ToString();
                            DataTable Stock = new DataTable();
                            dynamic sav = "N";
                            IEnumerable<E_CartDetails> CheckOutDetail = null;
                            CheckOutDetail = (IEnumerable<E_CartDetails>)Session["Cartdetailsftch"];
                            Session["Cartdetailsftch"] = obj.CartDetail;
                            var Sessionid = Session["CurrentUserSessionID"];
                            var userid = Session["UserId"];
                            var uniqueId = Session["UniqueId"];
                            var UserName = Session["UserName"];
                            var idNo = Session["IDNO"];
                            var FormNo = Session["FormNo"];
                            var OrderType = "";
                            decimal CourierCharge = 0;
                            var ShopType = "";

                            var PartyCode = string.Empty;
                            var Deliveryid = string.Empty; /*Session["DeliveryAddressID"].ToString()*/;

                            if (Deliveryid == "2")
                            {
                                CourierCharge = Convert.ToDecimal(Session["CourierCharge"]);
                            }
                            else
                            {
                                CourierCharge = 0;
                            }
                            if (ShopType.ToString() == "1")
                            {
                                OrderType = "T";
                            }
                            else
                            {
                                OrderType = "O";
                            }

                            StringBuilder sb = new StringBuilder();
                            foreach (var item in CheckOutDetail)
                            {
                                decimal qty = 0;
                                if (item.BunchQty > 0)
                                {
                                    qty = item.qty * item.BunchQty;
                                }
                                else
                                {
                                    qty = item.qty;
                                }
                                sb.AppendLine("<Cart>");
                                sb.AppendLine("<CartData>");
                                sb.AppendLine(("<ProdId>" + item.ProdId + "</ProdId>"));
                                sb.AppendLine(("<ProdName>" + item.ProdName.Replace("&", "").ToString() + "</ProdName>"));
                                sb.AppendLine(("<Qty>" + (item.qty) + "</Qty>"));
                                sb.AppendLine(("<Price>" + (item.Price) + "</Price>"));
                                sb.AppendLine(("<BV>" + (item.bv) + "</BV>"));
                                sb.AppendLine(("<PV>" + (item.PV) + "</PV>"));
                                sb.AppendLine(("<myIP>" + myIP + "</myIP>"));
                                sb.AppendLine(("<BunchQty>" + (item.BunchQty) + "</BunchQty>"));
                                sb.AppendLine(("<Sessionid>" + Convert.ToString(Sessionid) + "</Sessionid>"));
                                sb.AppendLine(("<userid>" + Convert.ToString(userid) + "</userid>"));
                                sb.AppendLine(("<Color>" + Convert.ToString(item.Color) + "</Color>"));
                                sb.AppendLine(("<Size>" + Convert.ToString(item.Size) + "</Size>"));
                                sb.AppendLine("</CartData>");
                                sb.AppendLine("</Cart>");
                            }
                            // save order in database
                            DataSet ds = iprod.SavePGCashFreeTemp(idNo.ToString(), FormNo.ToString(), Txnno,
                                "", "0", userid.ToString(), sb.ToString(),
                                Convert.ToDecimal(Amount), "", "", "", "C", "", Deliveryid, CourierCharge.ToString(),
                                PartyCode, Paymentimg, Convert.ToString(Session["SpecialInstruction"]));

                            if (ds != null && ds.Tables.Count > 0)
                            {
                                status = "true";
                                message = "Your order request has been successfully submitted.";
                                id = Convert.ToInt32(ds.Tables[0].Rows[0]["id"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                message = "Something went wrong";
            }
            return Json(new { message, status, id });
        }

        public int GenerateRandomInt(int min, int max)
        {
            Random rnd = new Random();
            return rnd.Next(min, max + 1); // max is exclusive, so add +1
        }
        public ActionResult GetwalletBalence(string WalletType)
        {
            M_Category obj = new M_Category();
            decimal Balance = 0;
            decimal RepurchaseBalance = 0;
            decimal TotPrice = 0;
            decimal Totbv = 0;
            IEnumerable<E_CartDetails> CheckOutDetail = null;
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                List<E_CheckOut> Items = new List<E_CheckOut>();

                CheckOutDetail = (IEnumerable<E_CartDetails>)Session["Cartdetailsftch"];
                foreach (var item in CheckOutDetail)
                {

                    E_CheckOut check = new E_CheckOut()
                    {
                        //Id = item.id,
                        ProdId = item.ProdId,
                        ProdName = item.ProdName,
                        Price = (item.qty * item.Price),
                        qty = item.qty,
                        bv = (item.qty * item.bv)//* item.BunchQty
                    };
                    Items.Add(check);
                }
                TotPrice = Convert.ToDecimal(CheckOutDetail.Sum(s => s.Price * s.qty).ToString());
                Totbv = Convert.ToDecimal(CheckOutDetail.Sum(s => s.bv * s.qty).ToString());
                var CourierCharge = Convert.ToDecimal(Session["CourierCharge"]);
                //var Deliveryid = Session["DeliveryAddressID"].ToString();

                //if (Deliveryid == "2")
                //{
                //    TotPrice = TotPrice + CourierCharge;
                //}
                //else
                //{
                TotPrice = TotPrice + 0;
                // }
                if (Convert.ToDecimal(Session["couponamount"]) == 0)
                {
                    Session["totalamount"] = TotPrice;

                }
                var FormNo = Session["FormNo"];
                obj.GetWalletBalence = ipaymode.GetBalence(WalletType, Convert.ToString(FormNo));
                Balance = Convert.ToDecimal(obj.GetWalletBalence.ToList()[0].Balance);

            }
            return Json(new { Balance, TotPrice, RepurchaseBalance }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PaymentWalletProceed(M_Category obj, string Amount)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (Convert.ToString(Session["MemMode"]) == "P")
                {
                    return RedirectToAction("PaymentWalletProceedCustomer", "CheckOut", new
                    {
                        // pass properties of obj (not the whole object)
                        Amount = Amount
                    });
                }

                bool sta = false;
                var randomordernumber = Convert.ToInt32(Session["Randomordernumber"]);
                sta = iprod.SaveTransactionOrder(randomordernumber);

                if (sta == true)
                {

                    var OrderType = "";
                    DataTable dtidstatus = iprod.GetIDStatus(Convert.ToString(Session["IDNO"]));
                    if (Convert.ToString(dtidstatus.Rows[0]["ActiveStatus"]) == "Y")
                    {
                        OrderType = "O";
                    }
                    else
                    {
                        OrderType = "T";
                    }
                    var Billno = "0";
                    BsnIdactivation req = new BsnIdactivation();
                    //req.amount = Convert.ToString(Session["totalbv"]);
                    //req.amount = Amount;
                    //req.bv = Convert.ToString(Session["totalbv"]);
                    //req.reqtype = "idactivation";
                    //req.toidno = Convert.ToString(Session["IDNO"]);
                    //req.passwd = Convert.ToString(Session["password"]);
                    //req.transpassword = Convert.ToString(Session["EPassw"]);
                    //req.islogin = "N";
                    //Session["KitId"] = Convert.ToString(dtidstatus.Rows[0]["KitId"]);

                    //req.Token = "abUnMar5489pidlAewUF4875brlE8a4i5n61096";
                    //req.UserName = Convert.ToString(Session["IDNO"]);
                    //req.Action = "addbv";
                    //req.Password = Convert.ToString(Session["password"]);
                    //req.TxnData = Billno + ";" + Amount + ";BVCredit";
                    //req.Amount = Amount;
                    Session["Newkitid"] = "2";
                    string apiurl = CPanelUrl.TrimEnd('/') + "/CheckLogin?token=abUnMar5489pidlAewUF4875brlE8a4i5n61108&UserName=" + Convert.ToString(Session["IDNO"]) + "&Password=" + Convert.ToString(Session["password"]) + "&action=addbv&amount=" + Convert.ToString(Session["totalamount"]) + "&totalpv=" + Convert.ToString(Session["totalpv"]) + "&billtype=" + Convert.ToString(Session["Kitbilltype"]) + "&kitid=" + Convert.ToString(Session["Newkitid"]) + "&TxnData=" + randomordernumber + ";" + Convert.ToString(Session["totalbv"]) + ";BVCredit";
                    var detail = JsonConvert.SerializeObject(req);
                    var response = Callgetfunction(apiurl);
                    var output = JsonConvert.DeserializeObject<Bsnaddbresponse>(response);
                    iprod.SaveAarogyaidactivationLog(Convert.ToString(Session["IDNO"]), apiurl, response);
                    if (output.status == "SUCCESS")
                    {
                        Billno = output.voucherno;
                        List<E_CheckOut> AddItems = new List<E_CheckOut>();
                        string hostName = Dns.GetHostName();
                        string myIP = Dns.GetHostEntry(hostName).AddressList[0].ToString();
                        DataTable Stock = new DataTable();
                        dynamic sav = "N";
                        var Sessionid = Session["CurrentUserSessionID"];
                        IEnumerable<E_CartDetails> CheckOutDetail = null;
                        CheckOutDetail = (IEnumerable<E_CartDetails>)Session["Cartdetailsftch"];
                        var userid = Session["UserId"];
                        var uniqueId = Session["UniqueId"];
                        var idNo = Session["IDNO"];
                        var FormNo = Session["FormNo"];
                        var ShopType = "";/*Session["ShopTye"];*/
                        var PartyCode = Session["PartyCode"];
                        //PartyCode = Session["PartyCode"].ToString();

                        decimal CourierCharge = 0;

                        var Deliveryid = string.Empty;

                        //if (Deliveryid == "2")
                        //{
                        //    CourierCharge = Convert.ToDecimal(Session["CourierCharge"]);
                        //}
                        //else
                        //{
                        //    CourierCharge = 0;
                        //}
                        CourierCharge = Convert.ToDecimal(Session["CourierCharge"]);

                        decimal qty = 0;
                        StringBuilder sb = new StringBuilder();
                        if (CheckOutDetail.Count() > 0)
                        {
                            foreach (var item in CheckOutDetail)
                            {
                                if (item.BunchQty > 0)
                                {
                                    qty = item.qty * item.BunchQty;
                                }
                                else
                                {
                                    qty = item.qty;
                                }
                                if (Convert.ToDecimal(Session["remainbv"]) == 0)
                                {
                                    Session["remainbv"] = (item.qty * item.bv);
                                }
                                sb.AppendLine("<orders>");
                                sb.AppendLine("<OrderData>");
                                sb.AppendLine(("<ProdId>" + item.ProdId + "</ProdId>"));
                                sb.AppendLine(("<ProdName>" + item.ProdName.Replace("&", "").ToString() + "</ProdName>"));
                                sb.AppendLine(("<Qty>" + qty + "</Qty>"));
                                sb.AppendLine(("<Price>" + (item.qty * item.Price) + "</Price>"));
                                sb.AppendLine(("<BV>" + (item.qty * item.bv) + "</BV>"));
                                sb.AppendLine(("<PV>" + (item.qty * item.PV) + "</PV>"));
                                sb.AppendLine(("<myIP>" + myIP + "</myIP>"));
                                sb.AppendLine(("<Sessionid>" + Convert.ToString(Sessionid) + "</Sessionid>"));
                                sb.AppendLine(("<userid>" + Convert.ToString(userid) + "</userid>"));
                                sb.AppendLine(("<idNo>" + Convert.ToString(idNo) + "</idNo>"));
                                sb.AppendLine(("<FormNo>" + Convert.ToString(FormNo) + "</FormNo>"));
                                sb.AppendLine(("<ShopingBillType>" + Convert.ToString(ShopType) + "</ShopingBillType>"));
                                sb.AppendLine(("<OrderType>" + Convert.ToString(OrderType) + "</OrderType>"));
                                sb.AppendLine(("<PartyCode>" + Convert.ToString(PartyCode) + "</PartyCode>"));
                                sb.AppendLine(("<Mode>" + "Wallet" + "</Mode>"));
                                sb.AppendLine(("<CourierCharge>" + Convert.ToString(CourierCharge) + "</CourierCharge>"));
                                sb.AppendLine(("<Color>" + Convert.ToString(item.Color) + "</Color>"));
                                sb.AppendLine(("<Size>" + Convert.ToString(item.Size) + "</Size>"));
                                sb.AppendLine(("<TRNCharge>" + Convert.ToString("0.00") + "</TRNCharge>"));
                                sb.AppendLine(("<coupon>" + Convert.ToString(Session["coupon"]) + "</coupon>"));
                                sb.AppendLine(("<couponamount>" + Convert.ToString(Session["couponamount"]) + "</couponamount>"));
                                sb.AppendLine(("<paidbv>" + Convert.ToString(Session["remainbv"]) + "</paidbv>"));
                                sb.AppendLine(("<Shoppingwallet>" + Convert.ToString("0") + "</Shoppingwallet>"));
                                sb.AppendLine(("<Repurchasewallet>" + Convert.ToString("0") + "</Repurchasewallet>"));
                                sb.AppendLine(("<IsSelfpickup>" + Convert.ToString(Session["IsSelfpickup"]) + "</IsSelfpickup>"));
                                sb.AppendLine(("<Batchcode>" + Convert.ToString(item.BatchNo) + "</Batchcode>"));
                             
                                sb.AppendLine("</OrderData>");
                                sb.AppendLine("</orders>");
                            }
                        }

                        // DataSet ds = iprod.InsertOrderDetail(sb.ToString());
                        var ordertransId = DateTime.Now.ToString("yyyyMMddHHmmssfff");

                        DataSet ds = iprod.InserttblTrnOrderWeb(sb.ToString(), Convert.ToDecimal(FormNo), Convert.ToDecimal(Amount), Convert.ToDecimal(ordertransId), "W", idNo.ToString(), "", Billno);
                        if (ds != null)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                Session["coupon"] = "";
                                Session["couponamount"] = "0";
                                Session["remainbv"] = "0";
                                E_SaveOrderDetail obg = new E_SaveOrderDetail();
                                List<E_SaveOrderDetail> lst = new List<E_SaveOrderDetail>();

                                //DataTable dtstatus = iprod.GetOrderStatus(Convert.ToString(Session["FormNo"]),Convert.ToString(ordertransId));

                                //if (dtstatus.Rows .Count>0)
                                //{

                                //Session["OrderId"] = dtstatus.Rows[0]["OrderId"];
                                //Session["ActiveStatus"] = dtstatus.Rows[0]["ActiveStatus"];
                                //}
                                //if (Convert.ToString(dtidstatus.Rows[0]["ActiveStatus"]) == "Y")
                                //{
                                //    Session["OrderId"] = ds.Tables[2].Rows[0]["OrderId"];
                                //    Session["ActiveStatus"] = ds.Tables[2].Rows[0]["ActiveStatus"];
                                //}
                                //else
                                //{
                                //    Session["OrderId"] = ds.Tables[3].Rows[0]["OrderId"];
                                //    Session["ActiveStatus"] = ds.Tables[3].Rows[0]["ActiveStatus"];
                                //}

                                //try
                                //{                      
                                //    
                                //}
                                //catch (Exception ex)
                                //{

                                //}
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    int qty1 = 0;
                                    if (Convert.ToInt32(dr["BunchQty"]) > 0)
                                    {
                                        qty1 = Convert.ToInt32(dr["qty"]) / Convert.ToInt32(dr["BunchQty"]);
                                    }
                                    else
                                    {
                                        qty1 = Convert.ToInt32(dr["qty"]);
                                    }

                                    lst.Add(new E_SaveOrderDetail
                                    {
                                        OrderId = Convert.ToInt32(dr["OrderId"]),
                                        ProdName = Convert.ToString(dr["ProdName"]),
                                        Price = Convert.ToDecimal(dr["Price"]),
                                        qty = Convert.ToDecimal(qty1),//Convert.ToDecimal(dr["qty"]),
                                        Mode = Convert.ToString(dr["Mode"]),
                                        OrderDate = Convert.ToString(dr["OrderDate"]),
                                        ImagePath = Convert.ToString(dr["ImagePath"]),
                                        MRP = Convert.ToDecimal(dr["MRP"]),
                                        BV = Convert.ToDecimal(dr["BV"]),
                                        //CourierCharge = Convert.ToDecimal(dr["CourierCharge"])
                                        //Imagepath = Convert.ToString(dr["ImagePath"]),
                                    });
                                }
                                Session["OrderId"] = lst[0].OrderId;
                                Session["BillNo"] = Billno;
                                Session["ActiveStatus"] = Convert.ToString(dtidstatus.Rows[0]["ActiveStatus"]);
                                Session["CheckOrderlst"] = lst;
                                Session["Status"] = "PAID";
                                obj.CartDetail = iprod.Cartdetailsftch(Convert.ToString(userid));//Convert.ToString(Sessionid),
                                Session["Cartdetailsftch"] = obj.CartDetail;
                                dtidstatus = iprod.GetIDStatus(Convert.ToString(Session["IDNO"]));
                                //update kit id
                                Session["KitId"] = Convert.ToString(dtidstatus.Rows[0]["KitId"]);
                                //var Sms = SMSApi(lst[0].OrderId);
                                return RedirectToAction("CheckOrder", "CheckOrder");
                            }
                            else
                            {
                                return RedirectToAction("CheckOut", "CheckOut");
                            }
                        }
                        else
                        {
                            return RedirectToAction("CheckOut", "CheckOut");
                            // check current date order 
                            //var formNo = Session["FormNo"].ToString();
                            //DataSet dsorder = iprod.Sp_GetCurrentOrderdate(formNo);
                            //if (dsorder != null)
                            //{
                            //    if (dsorder.Tables.Count > 0 && dsorder.Tables[0].Rows.Count > 0)
                            //    {
                            //        return RedirectToAction("CheckOrder", "CheckOrder");
                            //    }
                            //    else
                            //    {
                            //        return RedirectToAction("CheckOut", "CheckOut");
                            //    }
                            //}
                            //else
                            //{
                            //    return RedirectToAction("CheckOut", "CheckOut");
                            //}

                        }
                    }
                    //else
                    //{

                    //}
                }
                else
                {

                }
                return RedirectToAction("CheckOut", "CheckOut");
            }
        }

        public ActionResult PaymentWalletProceedCustomer(M_Category obj, string Amount)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {

                bool sta = false;
                var randomordernumber = Convert.ToInt32(Session["Randomordernumber"]);
                sta = iprod.SaveTransactionOrder(randomordernumber);

                if (sta == true)
                {

                    var OrderType = "";
                    DataTable dtidstatus = iprod.GetIDStatus(Convert.ToString(Session["IDNO"]));
                    if (Convert.ToString(dtidstatus.Rows[0]["ActiveStatus"]) == "Y")
                    {
                        OrderType = "O";
                    }
                    else
                    {
                        OrderType = "T";
                    }
                    var Billno = "0";
                    BsnIdactivation req = new BsnIdactivation();
                    //req.amount = Convert.ToString(Session["totalbv"]);
                    //req.amount = Amount;
                    //req.bv = Convert.ToString(Session["totalbv"]);
                    //req.reqtype = "idactivation";
                    //req.toidno = Convert.ToString(Session["IDNO"]);
                    //req.passwd = Convert.ToString(Session["password"]);
                    //req.transpassword = Convert.ToString(Session["EPassw"]);
                    //req.islogin = "N";
                    //Session["KitId"] = Convert.ToString(dtidstatus.Rows[0]["KitId"]);

                    //req.Token = "abUnMar5489pidlAewUF4875brlE8a4i5n61096";
                    //req.UserName = Convert.ToString(Session["IDNO"]);
                    //req.Action = "addbv";
                    //req.Password = Convert.ToString(Session["password"]);
                    //req.TxnData = Billno + ";" + Amount + ";BVCredit";
                    //req.Amount = Amount;
                    //string apiurl = "https://cpanel.bsnprojects.com/CheckLogin?token=abUnMar5489pidlAewUF4875brlE8a4i5n61096&UserName=" + Convert.ToString(Session["IDNO"]) + "&Password=" + Convert.ToString(Session["password"]) + "&action=addbv&TxnData=" + randomordernumber + ";" + Convert.ToString(Session["totalbv"]) + ";BVCredit";
                    //var detail = JsonConvert.SerializeObject(req);
                    //var response = Callgetfunction(apiurl);
                    //var output = JsonConvert.DeserializeObject<Bsnaddbresponse>(response);
                    //iprod.SaveAarogyaidactivationLog(req.UserName, detail, response);
                    //if (output.status == "SUCCESS")
                    //{
                    Billno = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    List<E_CheckOut> AddItems = new List<E_CheckOut>();
                    string hostName = Dns.GetHostName();
                    string myIP = Dns.GetHostEntry(hostName).AddressList[0].ToString();
                    DataTable Stock = new DataTable();
                    dynamic sav = "N";
                    var Sessionid = Session["CurrentUserSessionID"];
                    IEnumerable<E_CartDetails> CheckOutDetail = null;
                    CheckOutDetail = (IEnumerable<E_CartDetails>)Session["Cartdetailsftch"];
                    var userid = Session["UserId"];
                    var uniqueId = Session["UniqueId"];
                    var idNo = Session["IDNO"];
                    var FormNo = Session["FormNo"];
                    var ShopType = "";/*Session["ShopTye"];*/
                    var PartyCode = Session["PartyCode"];
                    //PartyCode = Session["PartyCode"].ToString();

                    decimal CourierCharge = 0;
                    CourierCharge = Convert.ToDecimal(Session["CourierCharge"]);
                    //var Deliveryid = string.Empty;

                    //if (Deliveryid == "2")
                    //{
                    //    CourierCharge = Convert.ToDecimal(Session["CourierCharge"]);
                    //}
                    //else
                    //{
                    //    CourierCharge = 0;
                    //}

                    decimal qty = 0;
                    StringBuilder sb = new StringBuilder();
                    if (CheckOutDetail.Count() > 0)
                    {
                        foreach (var item in CheckOutDetail)
                        {
                            if (item.BunchQty > 0)
                            {
                                qty = item.qty * item.BunchQty;
                            }
                            else
                            {
                                qty = item.qty;
                            }
                            if (Convert.ToDecimal(Session["remainbv"]) == 0)
                            {
                                Session["remainbv"] = (item.qty * item.bv);
                            }
                            sb.AppendLine("<orders>");
                            sb.AppendLine("<OrderData>");
                            sb.AppendLine(("<ProdId>" + item.ProdId + "</ProdId>"));
                            sb.AppendLine(("<ProdName>" + item.ProdName.Replace("&", "").ToString() + "</ProdName>"));
                            sb.AppendLine(("<Qty>" + qty + "</Qty>"));
                            sb.AppendLine(("<Price>" + (item.qty * item.Price) + "</Price>"));
                            sb.AppendLine(("<BV>" + (item.qty * item.bv) + "</BV>"));
                            sb.AppendLine(("<PV>" + (item.qty * item.PV) + "</PV>"));
                            sb.AppendLine(("<myIP>" + myIP + "</myIP>"));
                            sb.AppendLine(("<Sessionid>" + Convert.ToString(Sessionid) + "</Sessionid>"));
                            sb.AppendLine(("<userid>" + Convert.ToString(userid) + "</userid>"));
                            sb.AppendLine(("<idNo>" + Convert.ToString(idNo) + "</idNo>"));
                            sb.AppendLine(("<FormNo>" + Convert.ToString(FormNo) + "</FormNo>"));
                            sb.AppendLine(("<ShopingBillType>" + Convert.ToString(ShopType) + "</ShopingBillType>"));
                            sb.AppendLine(("<OrderType>" + Convert.ToString(OrderType) + "</OrderType>"));
                            sb.AppendLine(("<PartyCode>" + Convert.ToString(PartyCode) + "</PartyCode>"));
                            sb.AppendLine(("<Mode>" + "Wallet" + "</Mode>"));
                            sb.AppendLine(("<CourierCharge>" + Convert.ToString(CourierCharge) + "</CourierCharge>"));
                            sb.AppendLine(("<Color>" + Convert.ToString(item.Color) + "</Color>"));
                            sb.AppendLine(("<Size>" + Convert.ToString(item.Size) + "</Size>"));
                            sb.AppendLine(("<TRNCharge>" + Convert.ToString("0.00") + "</TRNCharge>"));
                            sb.AppendLine(("<coupon>" + Convert.ToString(Session["coupon"]) + "</coupon>"));
                            sb.AppendLine(("<couponamount>" + Convert.ToString(Session["couponamount"]) + "</couponamount>"));
                            sb.AppendLine(("<paidbv>" + Convert.ToString(Session["remainbv"]) + "</paidbv>"));
                            sb.AppendLine(("<Shoppingwallet>" + Convert.ToString("0") + "</Shoppingwallet>"));
                            sb.AppendLine(("<Repurchasewallet>" + Convert.ToString("0") + "</Repurchasewallet>"));
                            sb.AppendLine(("<IsSelfpickup>" + Convert.ToString(Session["IsSelfpickup"]) + "</IsSelfpickup>"));
                            sb.AppendLine("</OrderData>");
                            sb.AppendLine("</orders>");
                        }
                    }

                    // DataSet ds = iprod.InsertOrderDetail(sb.ToString());
                    var ordertransId = DateTime.Now.ToString("yyyyMMddHHmmssfff");

                    DataSet ds = iprod.InserttblTrnOrderWeb(sb.ToString(), Convert.ToDecimal(FormNo), Convert.ToDecimal(Amount), Convert.ToDecimal(ordertransId), "W", idNo.ToString(), "", Billno);
                    if (ds != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Session["coupon"] = "";
                            Session["couponamount"] = "0";
                            Session["remainbv"] = "0";
                            E_SaveOrderDetail obg = new E_SaveOrderDetail();
                            List<E_SaveOrderDetail> lst = new List<E_SaveOrderDetail>();

                            //DataTable dtstatus = iprod.GetOrderStatus(Convert.ToString(Session["FormNo"]),Convert.ToString(ordertransId));

                            //if (dtstatus.Rows .Count>0)
                            //{

                            //Session["OrderId"] = dtstatus.Rows[0]["OrderId"];
                            //Session["ActiveStatus"] = dtstatus.Rows[0]["ActiveStatus"];
                            //}
                            //if (Convert.ToString(dtidstatus.Rows[0]["ActiveStatus"]) == "Y")
                            //{
                            //    Session["OrderId"] = ds.Tables[2].Rows[0]["OrderId"];
                            //    Session["ActiveStatus"] = ds.Tables[2].Rows[0]["ActiveStatus"];
                            //}
                            //else
                            //{
                            //    Session["OrderId"] = ds.Tables[3].Rows[0]["OrderId"];
                            //    Session["ActiveStatus"] = ds.Tables[3].Rows[0]["ActiveStatus"];
                            //}

                            //try
                            //{                      
                            //    
                            //}
                            //catch (Exception ex)
                            //{

                            //}
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                int qty1 = 0;
                                if (Convert.ToInt32(dr["BunchQty"]) > 0)
                                {
                                    qty1 = Convert.ToInt32(dr["qty"]) / Convert.ToInt32(dr["BunchQty"]);
                                }
                                else
                                {
                                    qty1 = Convert.ToInt32(dr["qty"]);
                                }

                                lst.Add(new E_SaveOrderDetail
                                {
                                    OrderId = Convert.ToInt32(dr["OrderId"]),
                                    ProdName = Convert.ToString(dr["ProdName"]),
                                    Price = Convert.ToDecimal(dr["Price"]),
                                    qty = Convert.ToDecimal(qty1),//Convert.ToDecimal(dr["qty"]),
                                    Mode = Convert.ToString(dr["Mode"]),
                                    OrderDate = Convert.ToString(dr["OrderDate"]),
                                    ImagePath = Convert.ToString(dr["ImagePath"]),
                                    MRP = Convert.ToDecimal(dr["MRP"]),
                                    BV = Convert.ToDecimal(dr["BV"]),
                                    //CourierCharge = Convert.ToDecimal(dr["CourierCharge"])
                                    //Imagepath = Convert.ToString(dr["ImagePath"]),
                                });
                            }
                            Session["OrderId"] = lst[0].OrderId;
                            Session["BillNo"] = Billno;
                            Session["ActiveStatus"] = Convert.ToString(dtidstatus.Rows[0]["ActiveStatus"]);
                            Session["CheckOrderlst"] = lst;
                            Session["Status"] = "PAID";
                            obj.CartDetail = iprod.Cartdetailsftch(Convert.ToString(userid));//Convert.ToString(Sessionid),
                            Session["Cartdetailsftch"] = obj.CartDetail;

                            //var Sms = SMSApi(lst[0].OrderId);
                            return RedirectToAction("CheckOrder", "CheckOrder");
                        }
                        else
                        {
                            return RedirectToAction("CheckOut", "CheckOut");
                        }
                    }
                    else
                    {
                        return RedirectToAction("CheckOut", "CheckOut");
                        // check current date order 
                        //var formNo = Session["FormNo"].ToString();
                        //DataSet dsorder = iprod.Sp_GetCurrentOrderdate(formNo);
                        //if (dsorder != null)
                        //{
                        //    if (dsorder.Tables.Count > 0 && dsorder.Tables[0].Rows.Count > 0)
                        //    {
                        //        return RedirectToAction("CheckOrder", "CheckOrder");
                        //    }
                        //    else
                        //    {
                        //        return RedirectToAction("CheckOut", "CheckOut");
                        //    }
                        //}
                        //else
                        //{
                        //    return RedirectToAction("CheckOut", "CheckOut");
                        //}

                    }
                    //}
                    //else
                    //{

                    //}
                }
                else
                {

                }
                return RedirectToAction("CheckOut", "CheckOut");
            }
        }

        public string Callgetfunction(string Url)
        {
            string sResponseFromServer = string.Empty;
            try
            {
                WebRequest tRequest;
                Stream dataStream;
                tRequest = WebRequest.Create(Url);
                WebResponse tResponse = tRequest.GetResponse();
                dataStream = tResponse.GetResponseStream();
                StreamReader tReader = new StreamReader(dataStream);
                sResponseFromServer = tReader.ReadToEnd();
            }
            catch
            {

            }
            return sResponseFromServer;
        }
        public string Checkaarogyaidactivation(string detail, string url)
        {
            string result = string.Empty;
            try
            {
                //Aarogyaidactivation
                // Create a request
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json"; // Set content type to JSON
                // If the API requires headers (e.g., Authorization), add them here
                // request.Headers.Add("Authorization", "Bearer YOUR_TOKEN");
                // Write JSON data to request stream
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(detail);
                    streamWriter.Flush();
                }
                // Get the response
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        result = streamReader.ReadToEnd();
                        return result; // Optionally deserialize this JSON string to an object
                    }
                }
            }
            catch (Exception)
            {

            }
            return result;
        }

        [HttpPost]
        public async Task<ActionResult> CashfreeInitiate()
        {
            int orderid = Convert.ToInt32(Session["Randomordernumber"]);
            string url = "";
            string msg = "";
            string paymentid = "";
            try
            {
                M_Category obj = new M_Category();
                bool sta = false;
                var randomordernumber = orderid;
                sta = iprod.SaveTransactionOrder(randomordernumber);
                if (sta)
                {
                    //clsgen.ErrorLog(Server.MapPath("~/Logs/ErrorLog"), "CashfreeInitiate-0");
                    string str = string.Empty;
                    decimal value = 0;
                    string Code = "";
                    DataSet data;
                    try
                    {
                        string PGAmount = Convert.ToString(Convert.ToDecimal(Session["totalamount"]) + Convert.ToDecimal(Session["CourierCharge"]));
                        string Amount = Convert.ToString(Convert.ToDecimal(Session["totalamount"]));
                        M_payreqCashFreenew cash = new M_payreqCashFreenew();
                        var SecretKey = String.Empty;
                        var Url = string.Empty;
                        var appid = string.Empty;
                        var returnurl = string.Empty;
                        string notifyUrl = string.Empty;
                        string x_client_id = "";
                        string x_client_secret = "";
                        if (IsTest == "True")
                        {
                            x_client_id = "11649217141cb3d4d36a36ffa9c1294611";
                            x_client_secret = "cfsk_ma_prod_4f8ad5a28de217d91cb53ad6176d96ef_d9f03137";
                            Url = CashfreeOrderUrl;
                            returnurl = CashfreeReturnUrl;
                            // Mode = "PROD";
                        }
                        else
                        {
                            x_client_id = "TEST430329ae80e0f32e41a393d78b923034";
                            x_client_secret = "TESTaf195616268bd6202eeb3bf8dc458956e7192a85";
                            Url = CashfreeSandboxOrderUrl;
                            returnurl = CashfreeSandboxReturnUrl;
                            //Mode = "TEST";
                        }
                        cash.order_amount = (float)Convert.ToDouble(PGAmount);
                        cash.order_currency = "INR";
                        cash.order_id = Convert.ToString(orderid);
                        cash.order_meta = new Order_Meta();
                        cash.order_meta.return_url = returnurl;
                        cash.customer_details = new Customer_Details();
                        cash.customer_details.customer_id = Convert.ToString(Session["IDNO"]);
                        cash.customer_details.customer_phone = Convert.ToString(Session["MobileNo"]);

                        string output = JsonConvert.SerializeObject(cash);
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        //clsgen.ErrorLog(Server.MapPath("~/Logs/ErrorLog"), ("Billpayment/CashfreeInitiate-->Request " + output));
                        var client = new HttpClient();
                        var request = new HttpRequestMessage(HttpMethod.Post, Url);
                        request.Headers.Add("x-client-id", x_client_id);
                        request.Headers.Add("x-client-secret", x_client_secret);
                        request.Headers.Add("Accept", "application/json");
                        request.Headers.Add("x-api-version", "2025-01-01");
                        var content = new StringContent(output, null, "application/json");
                        request.Content = content;
                        var responsecash = await client.SendAsync(request);
                        responsecash.EnsureSuccessStatusCode();
                        var strresponse = await responsecash.Content.ReadAsStringAsync();
                        //clsgen.ErrorLog(Server.MapPath("~/Logs/ErrorLog"), ("addfund/CashfreeInitiate-->Response " + strresponse));
                        DataSet dsss = convertJsonStringToDataSet(strresponse);
                        if (dsss.Tables[0].Rows[0]["order_status"].ToString().ToUpper() == "ACTIVE")
                        {
                            string Bvapiurl = CPanelUrl.TrimEnd('/') + "/CheckLogin?token=abUnMar5489pidlAewUF4875brlE8a4i5n61102&UserName=" + Convert.ToString(Session["IDNO"]) + "&Password=" + Convert.ToString(Session["password"]) + "&action=addbv&amount=" + Convert.ToString(Session["totalamount"]) + "&billtype=" + Convert.ToString(Session["Kitbilltype"]) + "&kitid=" + Convert.ToString(Session["Newkitid"]) + "&TxnData=" + randomordernumber + ";" + Convert.ToString(Session["totalbv"]) + ";BVCredit";
                            paymentid = dsss.Tables[0].Rows[0]["payment_session_id"].ToString();
                            //save order in temp table
                            string hostName = Dns.GetHostName();
                            string myIP = Dns.GetHostEntry(hostName).AddressList[0].ToString();
                            DataTable Stock = new DataTable();
                            dynamic sav = "N";
                            IEnumerable<E_CartDetails> CheckOutDetail = null;
                            CheckOutDetail = (IEnumerable<E_CartDetails>)Session["Cartdetailsftch"];
                            Session["Cartdetailsftch"] = obj.CartDetail;
                            var Sessionid = Session["CurrentUserSessionID"];
                            var userid = Session["UserId"];
                            var uniqueId = Session["UniqueId"];
                            var UserName = Session["UserName"];
                            var idNo = Session["IDNO"];
                            var FormNo = Session["FormNo"];
                            var OrderType = "";
                            decimal CourierCharge = 0;
                            var ShopType = "";

                            string PartyCode = Session["PartyCode"].ToString();
                            var Deliveryid = string.Empty; /*Session["DeliveryAddressID"].ToString()*/;
                            CourierCharge = Convert.ToDecimal(Session["CourierCharge"]);
                            //if (Deliveryid == "2")
                            //{
                            //    CourierCharge = Convert.ToDecimal(Session["CourierCharge"]);
                            //}
                            //else
                            //{
                            //    CourierCharge = 0;
                            //}
                            if (ShopType.ToString() == "1")
                            {
                                OrderType = "T";
                            }
                            else
                            {
                                OrderType = "O";
                            }

                            StringBuilder sb = new StringBuilder();
                            foreach (var item in CheckOutDetail)
                            {
                                decimal qty = 0;
                                if (item.BunchQty > 0)
                                {
                                    qty = item.qty * item.BunchQty;
                                }
                                else
                                {
                                    qty = item.qty;
                                }
                                sb.AppendLine("<Cart>");
                                sb.AppendLine("<CartData>");
                                sb.AppendLine(("<ProdId>" + item.ProdId + "</ProdId>"));
                                sb.AppendLine(("<ProdName>" + item.ProdName.Replace("&", "").ToString() + "</ProdName>"));
                                sb.AppendLine(("<Qty>" + (item.qty) + "</Qty>"));
                                sb.AppendLine(("<Price>" + (item.Price) + "</Price>"));
                                sb.AppendLine(("<BV>" + (item.bv) + "</BV>"));
                                sb.AppendLine(("<PV>" + (item.PV) + "</PV>"));
                                sb.AppendLine(("<myIP>" + myIP + "</myIP>"));
                                sb.AppendLine(("<BunchQty>" + (item.BunchQty) + "</BunchQty>"));
                                sb.AppendLine(("<Sessionid>" + Convert.ToString(Sessionid) + "</Sessionid>"));
                                sb.AppendLine(("<userid>" + Convert.ToString(userid) + "</userid>"));
                                sb.AppendLine(("<Color>" + Convert.ToString(item.Color) + "</Color>"));
                                sb.AppendLine(("<Size>" + Convert.ToString(item.Size) + "</Size>"));
                                sb.AppendLine("</CartData>");
                                sb.AppendLine("</Cart>");
                            }

                            // save order in database
                            DataSet ds = iprod.SaveRazarpayTemp(idNo.ToString(), FormNo.ToString(), Convert.ToString(randomordernumber),
                                "", "0", userid.ToString(), sb.ToString(),
                                Convert.ToDecimal(Amount), "", "", "", "C", "", Deliveryid, CourierCharge.ToString(),
                                PartyCode, "", Convert.ToString(orderid), Bvapiurl, Convert.ToString(Session["MemMode"]));
                        }
                        else
                        {
                            msg = "Payment cannot initiate.";
                        }
                    }
                    catch (Exception ex)
                    {
                        //ExceptionLogging.SendErrorToText(ex);
                        //clsgen.ErrorLog(Server.MapPath("~/Logs/ErrorLog"), "ERROR" + ex.Message);
                        msg = "something went wrong";
                    }
                }
                else
                {
                    msg = "Already proceed.";
                }

            }
            catch (Exception ex)
            {
                msg = "Something went wrong";
                //clsgen.ErrorLog(Server.MapPath("~/Logs/ErrorLog"), ("addfund/CashfreeInitiate-->Error " + ex.Message));
            }
            return Json(new { msg, url, paymentid }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CashFreeApi(string paymentid)
        {
            ViewBag.paymentid = paymentid;
            return View();
        }

        public async Task<ActionResult> PaymentSuccessCashFree(string order_id)
        {
            try
            {
                string x_client_id = "";
                string x_client_secret = "";
                string Url = "";
                if (IsTest == "True")
                {
                    x_client_id = "11649217141cb3d4d36a36ffa9c1294611";
                    x_client_secret = "cfsk_ma_prod_4f8ad5a28de217d91cb53ad6176d96ef_d9f03137";
                    Url = CashfreeOrderUrl.TrimEnd('/') + "/" + order_id + "/payments";
                }
                else
                {
                    x_client_id = "TEST430329ae80e0f32e41a393d78b923034";
                    x_client_secret = "TESTaf195616268bd6202eeb3bf8dc458956e7192a85";
                    Url = CashfreeSandboxOrderUrl.TrimEnd('/') + "/" + order_id + "/payments";
                }
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, Url);
                request.Headers.Add("x-client-id", x_client_id);
                request.Headers.Add("x-client-secret", x_client_secret);
                request.Headers.Add("Accept", "application/json");
                request.Headers.Add("x-api-version", "2025-01-01");
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var strresponse = await response.Content.ReadAsStringAsync();
                //clsgen.ErrorLog(Server.MapPath("~/Logs/ErrorLog"), ("Billpayment/PaymentSuccessCashFree-->Response " + strresponse));
                JArray jsonArray = JArray.Parse(strresponse);
                string orderStatus = "FAILED"; // default
                if (jsonArray.Any(x => x["payment_status"]?.ToString() == "SUCCESS"))
                {
                    orderStatus = "PAID";
                }
                else if (jsonArray.Any(x => x["payment_status"]?.ToString() == "PENDING"))
                {
                    orderStatus = "PENDING";
                }
                JObject firstItem = (JObject)jsonArray[0];
                decimal Reqamount = firstItem["order_amount"]?.Value<decimal>() ?? 0;
                StringBuilder sb = new StringBuilder();
                string uniqueRefID = "";
                string tDate = string.Empty;
                string rs = string.Empty;
                string rrnNo = string.Empty;
                string rsv = string.Empty;
                string mandatoryField = string.Empty;
                string requestedId = string.Empty;
                DataSet Ds = iprod.GetOrderbyPgTxnid(order_id);
                if (orderStatus.ToUpper() == "PAID")
                {
                    string voucherno = "0";
                    DataSet ds = new DataSet();
                    decimal CourierCharge = 0;
                    var idNo = Ds.Tables[0].Rows[0]["idNo"].ToString();
                    DataSet dsproduct = iprod.GetTransId(Convert.ToString(Ds.Tables[0].Rows[0]["txnid"]));
                    var ShopType = Ds.Tables[0].Rows[0]["ShopType"].ToString();
                    var userid = Ds.Tables[0].Rows[0]["userid"].ToString();
                    var FormNo = Ds.Tables[0].Rows[0]["FormNo"].ToString();
                    var PartyCode = Ds.Tables[0].Rows[0]["PartyCode"].ToString();
                    var CourierCharge1 = Convert.ToDecimal(Ds.Tables[0].Rows[0]["CourierCharge"]);
                    var Deliveryid = Ds.Tables[0].Rows[0]["DecliveryId"].ToString();
                    CourierCharge = CourierCharge1;
                    //if (Deliveryid == "2")
                    //{
                    //    CourierCharge = CourierCharge1;
                    //}
                    //else
                    //{
                    //    CourierCharge = 0;
                    //}
                    var Amount = Convert.ToDouble(Ds.Tables[0].Rows[0]["Amount"]);
                    var OrderType = string.Empty;
                    string myIP = "";
                    if (ShopType.ToString() == "1")
                    {
                        OrderType = "T";
                    }
                    else
                    {
                        OrderType = "O";
                    }
                    decimal qty = 0;
                    sb = new StringBuilder();
                    foreach (DataRow row in dsproduct.Tables[0].Rows)
                    {
                        if (Convert.ToDecimal(row["BunchQty"]) > 0)
                        {
                            qty = Convert.ToDecimal(row["qty"]) * Convert.ToDecimal(row["BunchQty"]);
                        }
                        else
                        {
                            qty = Convert.ToDecimal(row["qty"]);
                        }
                        sb.AppendLine("<orders>");
                        sb.AppendLine("<OrderData>");
                        sb.AppendLine(("<ProdId>" + row["ProdId"].ToString() + "</ProdId>"));
                        sb.AppendLine(("<ProdName>" + row["ProdName"].ToString() + "</ProdName>"));
                        sb.AppendLine(("<Qty>" + qty + "</Qty>"));
                        sb.AppendLine(("<Price>" + (Convert.ToDecimal(row["qty"]) * Convert.ToDecimal(row["Price"])) + "</Price>"));
                        sb.AppendLine(("<BV>" + (Convert.ToDecimal(row["qty"]) * Convert.ToDecimal(row["Bv"])) + "</BV>"));
                        sb.AppendLine(("<PV>" + (Convert.ToInt32(row["qty"]) * Convert.ToInt32(row["Pv"])) + "</PV>"));
                        sb.AppendLine(("<myIP>" + myIP + "</myIP>"));
                        //sb.AppendLine(("<Sessionid>" + Convert.ToString(Sessionid) + "</Sessionid>"));
                        sb.AppendLine(("<userid>" + Convert.ToString(userid) + "</userid>"));
                        sb.AppendLine(("<idNo>" + Convert.ToString(idNo) + "</idNo>"));
                        sb.AppendLine(("<FormNo>" + Convert.ToString(FormNo) + "</FormNo>"));
                        sb.AppendLine(("<ShopingBillType>" + Convert.ToString(ShopType) + "</ShopingBillType>"));
                        sb.AppendLine(("<OrderType>" + Convert.ToString(OrderType) + "</OrderType>"));
                        sb.AppendLine(("<PartyCode>" + Convert.ToString(PartyCode) + "</PartyCode>"));
                        sb.AppendLine(("<Mode>" + "PaymentGateway" + "</Mode>"));
                        sb.AppendLine(("<CourierCharge>" + Convert.ToString(CourierCharge) + "</CourierCharge>"));
                        sb.AppendLine(("<Color>" + Convert.ToString("") + "</Color>"));
                        sb.AppendLine(("<Size>" + Convert.ToString("") + "</Size>"));
                        sb.AppendLine(("<TRNCharge>" + Convert.ToString("0") + "</TRNCharge>"));
                        sb.AppendLine(("<IsSelfpickup>" + Convert.ToString(Session["IsSelfpickup"]) + "</IsSelfpickup>"));
                        sb.AppendLine("</OrderData>");
                        sb.AppendLine("</orders>");
                    }
                    var ordertransId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    if (Ds.Tables[0].Rows[0]["Mememode"].ToString() == "D")
                    {
                        string apiurl = Ds.Tables[0].Rows[0]["Bvapiurl"].ToString();
                        var bvresponse = Callgetfunction(apiurl);
                        var output = JsonConvert.DeserializeObject<Bsnaddbresponse>(bvresponse);
                        iprod.SaveAarogyaidactivationLog(idNo, apiurl, bvresponse);
                        if (output.status == "SUCCESS")
                        {
                            voucherno = output.voucherno;
                            ds = iprod.InserttblTrnOrderWeb(sb.ToString(), Convert.ToDecimal(FormNo), Convert.ToDecimal(Amount), Convert.ToDecimal(ordertransId), "P", idNo.ToString(), "", output.voucherno);
                        }
                    }
                    else
                    {
                        ds = iprod.InserttblTrnOrderWeb(sb.ToString(), Convert.ToDecimal(FormNo), Convert.ToDecimal(Amount), Convert.ToDecimal(ordertransId), "P", idNo.ToString(), "", "0");
                    }

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        var id = ds.Tables[0].Rows[0][0];
                        DataSet Order = iprod.UpdatePaymentOrderId(id.ToString(), Convert.ToString(Ds.Tables[0].Rows[0]["txnid"]));
                        DataSet Status = iprod.UpdateStatus("Paid", Convert.ToString(Ds.Tables[0].Rows[0]["txnid"]));

                        Session["coupon"] = "";
                        Session["couponamount"] = "0";
                        Session["remainbv"] = "0";
                        E_SaveOrderDetail obg = new E_SaveOrderDetail();
                        List<E_SaveOrderDetail> lst = new List<E_SaveOrderDetail>();

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            int qty1 = 0;
                            if (Convert.ToInt32(dr["BunchQty"]) > 0)
                            {
                                qty1 = Convert.ToInt32(dr["qty"]) / Convert.ToInt32(dr["BunchQty"]);
                            }
                            else
                            {
                                qty1 = Convert.ToInt32(dr["qty"]);
                            }

                            lst.Add(new E_SaveOrderDetail
                            {
                                OrderId = Convert.ToInt32(dr["OrderId"]),
                                ProdName = Convert.ToString(dr["ProdName"]),
                                Price = Convert.ToDecimal(dr["Price"]),
                                qty = Convert.ToDecimal(qty1),//Convert.ToDecimal(dr["qty"]),
                                Mode = Convert.ToString(dr["Mode"]),
                                OrderDate = Convert.ToString(dr["OrderDate"]),
                                ImagePath = Convert.ToString(dr["ImagePath"]),
                                MRP = Convert.ToDecimal(dr["MRP"]),
                                BV = Convert.ToDecimal(dr["BV"])
                                //CourierCharge = Convert.ToDecimal(dr["CourierCharge"])
                                //Imagepath = Convert.ToString(dr["ImagePath"]),
                            });
                        }
                        DataTable dtidstatus = iprod.GetIDStatus(Convert.ToString(Session["IDNO"]));
                        Session["OrderId"] = lst[0].OrderId;
                        Session["BillNo"] = voucherno;
                        Session["ActiveStatus"] = Convert.ToString(dtidstatus.Rows[0]["ActiveStatus"]);
                        Session["CheckOrderlst"] = lst;
                        Session["Status"] = "PAID";
                        var CartDetail = iprod.Cartdetailsftch(Convert.ToString(userid));//Convert.ToString(Sessionid),
                        Session["Cartdetailsftch"] = CartDetail;
                    }

                    return RedirectToAction("CheckOrder", "CheckOrder");
                }
                else if (orderStatus.ToUpper() == "PENDING")
                {
                    return Redirect("/Home/Myorders");
                }
                else
                {
                    DataSet fail = iprod.RejectPGOrder(Convert.ToString(Ds.Tables[0].Rows[0]["txnid"]), "");
                    return Redirect("/Home/Index");
                }
            }
            catch (Exception ex)
            {
                //clsgen.ErrorLog(Server.MapPath("~/Logs/ErrorLog"), ("addfund/PaymentSuccessCashFree-->Error " + ex.Message));
            }
            return RedirectToAction("Index", "Home");
        }

        public DataSet convertJsonStringToDataSet(string jsonString)
        {
            XmlDocument xd = new XmlDocument();
            jsonString = "{ \"rootNode\": {" + jsonString.Trim().TrimStart('{').TrimEnd('}') + "} }";
            xd = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonString);
            DataSet ds = new DataSet();
            ds.ReadXml(new XmlNodeReader(xd));
            return ds;
        }
        [HttpPost]
        public ActionResult ProductWiseStock(string SpecialInstruction, string partycode)
        {
            M_Category objg = new M_Category();
            objg.CartDetail = (IEnumerable<E_CartDetails>)Session["Cartdetailsftch"];
            Session["SpecialInstruction"] = SpecialInstruction ?? "";

            string msg = "";

            try
            {
                DataSet ds = new DataSet();

                foreach (var item in objg.CartDetail)
                {
                    ds = iprod.Get_ProductWiseStockWithPartyCode(item.ProdId, partycode);

                    var SumQty = objg.CartDetail.Where(s => s.ProdId == item.ProdId).ToList();
                    int QtyPord = Convert.ToInt32(SumQty[0].qty);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        int stockQty = Convert.ToInt32(ds.Tables[0].Rows[0]["stockqty"]);

                        object val = ds.Tables[0].Rows[0]["trnqty"];
                        int trnQty = (val == DBNull.Value) ? 0 : Convert.ToInt32(val);

                        int remqty = stockQty - trnQty;

                        // ✅ CASE 1: OUT OF STOCK
                        if (remqty <= 0)
                        {
                            msg += item.ProdName + " (Out of Stock), ";
                        }
                        // ✅ CASE 2: LESS STOCK THAN REQUIRED
                        else if (remqty < QtyPord)
                        {
                            msg += item.ProdName + " (Only " + remqty + " available), ";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // optional logging
            }

            // ✅ Final Message
            if (!string.IsNullOrEmpty(msg))
            {
                msg = msg.TrimEnd(',', ' ');
                msg = "Stock issue: " + msg + " for selected Franchise (Delivery Center)";
            }

            return Json(new { msg });
        }
        //[HttpPost]
        //public ActionResult ProductWiseStock(string SpecialInstruction, string partycode)
        //{
        //    M_Category objg = new M_Category();
        //    objg.CartDetail = (IEnumerable<E_CartDetails>)Session["Cartdetailsftch"];
        //    Session["SpecialInstruction"] = SpecialInstruction == null ? "" : SpecialInstruction;
        //    string msg = "";
        //    try
        //    {
        //        DataSet ds = new DataSet();
        //        foreach (var item in objg.CartDetail)
        //        {
        //            ds = iprod.Get_ProductWiseStockWithPartyCode(item.ProdId, partycode);
        //            var SumQty = objg.CartDetail.Where(s => s.ProdId == item.ProdId).ToList();
        //            var QtyPord = SumQty[0].qty;
        //            if (ds != null)
        //            {
        //                if (ds.Tables[0].Rows.Count > 0)
        //                {
        //                    object val = ds.Tables[0].Rows[0]["trnqty"];
        //                    if (val == DBNull.Value)
        //                    {
        //                        if (Convert.ToInt32(ds.Tables[0].Rows[0]["stockqty"]) < Convert.ToInt32(QtyPord))
        //                        {
        //                            msg = item.ProdName + ",";
        //                        }
        //                    }
        //                    else
        //                    {
        //                        var remqty = Convert.ToInt32(ds.Tables[0].Rows[0]["stockqty"]) - Convert.ToInt32(ds.Tables[0].Rows[0]["trnqty"]);
        //                        if (remqty < Convert.ToInt32(QtyPord))
        //                        {
        //                            msg = item.ProdName + ",";
        //                        }
        //                        //if (Convert.ToInt32(ds.Tables[0].Rows[0]["stockqty"]) < Convert.ToInt32(ds.Tables[0].Rows[0]["trnqty"]))
        //                        //{
        //                        //    msg = item.ProdName + ",";
        //                        //}
        //                    }

        //                }
        //                else if (Convert.ToInt32(ds.Tables[0].Rows[0]["stockqty"]) < Convert.ToInt32(QtyPord))
        //                {
        //                    msg = item.ProdName + ",";
        //                }

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    if (msg != "")
        //    {
        //        msg = msg.TrimEnd(',');
        //        msg = "Please remove " + msg + " from the cart because it is out of stock";
        //    }

        //    //if (Convert.ToString(Session["MemMode"]) == "D" && msg == "")
        //    //{
        //    //    decimal totalbv = objg.CartDetail.Sum(s => (s.Price * s.qty));
        //    //    DataSet dscount = iprod.CheckKitOnPurchase(Convert.ToInt32(Session["FormNo"]), totalbv);
        //    //    if (dscount.Tables[0].Rows.Count > 0)
        //    //    {
        //    //        //if (Convert.ToString(dscount.Tables[0].Rows[0]["Status"])== "FAILED")
        //    //        //{
        //    //        //    msg = Convert.ToString(dscount.Tables[0].Rows[0]["msg"]);
        //    //        //}
        //    //        if (Convert.ToString(dscount.Tables[0].Rows[0]["Status"]) == "SUCCESS")
        //    //        {
        //    //            Session["Newkitid"] = Convert.ToString(dscount.Tables[0].Rows[0]["NewKitID"]);
        //    //        }
        //    //    }
        //    //}

        //    return Json(new { msg });
        //}
        [HttpPost]
        public JsonResult DeliverycheckCenterAddress(string PartyCode)
        {
            DataSet ds = iprod.GetDeliveryCenterAddress(PartyCode);

            var list = ds.Tables[0].AsEnumerable().Select(x => new
            {
                PartyCode = x["PartyCode"].ToString(),
                PartyName = x["PartyName"].ToString(),
                Address = x["Address1"].ToString()
            }).ToList();

            return Json(list);
        }
        [HttpPost]
        public JsonResult DeliveryCenterAddress()
        {
            DataSet ds = iprod.GetAllparty();
            var list = ds.Tables[0].AsEnumerable().Select(x => new
            {
                PartyCode = x["PartyCode"].ToString(),
                PartyName = x["PartyName"].ToString(),
                Address = x["Address1"].ToString()
            }).ToList();

            return Json(list);
        }
        [HttpPost]
        public JsonResult UpdateCourierCharge(decimal charge,string Pickuptype)
        {
            Session["CourierCharge"] = charge;
            if (Pickuptype == "Self PickUP")
            {
                Session["IsSelfpickup"] = "Y";
            }
            if (Pickuptype == "By Courier")
            {
                Session["IsSelfpickup"] = "N";
            }
            return Json(new
            {
                success = true,
                charge = charge,
                msg = "Updated"
            });
        }
    }
}