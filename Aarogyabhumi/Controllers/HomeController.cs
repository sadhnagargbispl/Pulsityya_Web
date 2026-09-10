using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using PagedList;
using Shopinv.Entity;
using Shopinv.Interface;
using Shopinv.Models;
using Shopinv.Repoistory;
using Shopinv.SiteExtension;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
namespace Shopinv.Controllers
{
    [KycRequired]
    public class HomeController : Controller
    {
        private readonly I_Category _icateogry = null;
        private readonly I_Product _iprod = null;
        private readonly I_Banner _ibanner = null;
        private readonly I_OrderReport iorderrept = null;
        private readonly I_DelistedDirectSeller iler = null;
        CompanyDetail companyDetail;
        private readonly static string Apiurl = ConfigurationManager.AppSettings["ApiUrl"];
        public HomeController(I_Category icateogry, I_Product i_Product, I_OrderReport iorderrept, I_DelistedDirectSeller iler, I_Banner ibanner)
        {
            this._icateogry = icateogry;
            this._iprod = i_Product;
            this.iorderrept = iorderrept;
            this.iler = iler;
            _ibanner = ibanner;
            companyDetail = new CompanyDetail(this._iprod);
            companyDetail.GetCompanydetail();
            //  this.icouponrept = icouponrept;
        }
        List<E_CheckOut> Items = new List<E_CheckOut>();
        string companyId = System.Configuration.ConfigurationManager.AppSettings["CompanyId"];
        public ActionResult Index()
        {
            M_Category objg = new M_Category();
            objg.SpecialProductList = _iprod.GetSpecialProduct();
            objg.FeaturedProduct = _iprod.GetFeaturedProduct();
            objg.TopSellerProduct = _iprod.GetTopSellerProduct();
            //objg.ProductReview = _iprod.GetTopProductReview();                               
            //objg.DealsOfTheWeek = _iprod.DealsOfTheWeek();


            return View(objg);
        }

        public ActionResult Desktopheader()
        {
            return PartialView("DesktopHeader");
        }
        public ActionResult IndexBanner()
        {
            M_Category objg = new M_Category();
            objg.ShowBanner = _ibanner.ShowBannerList("6");
            return PartialView("IndexBanner", objg);
        }
        public ActionResult CategoryList()
        {
            M_Category objg = new M_Category();
            objg.DDLCategory = _icateogry.DDLCategory();
            //foreach (var item in objg.DDLCategory)
            //{
            //    item.subCategory = _icateogry.DDLSubCategory(Convert.ToString(item.CatId));
            //}
            Session["DDLCategory"] = objg.DDLCategory;
            var tblOrder = Extension.RenderRazorViewToString(this.ControllerContext, "Mobile_header", objg);
            return Json(new { tblOrder });
        }

        public ActionResult OfferBanner()
        {
            return PartialView("OfferBanner");
        }
        public ActionResult GetCartCount()
        {
            try
            {
                M_Category objg = new M_Category();
                var userid = Session["UserId"];
                var Sessionid = "";
                List<E_CartDetails> CartDetail = _iprod.Cartdetailsftch(Convert.ToString(userid)).ToList();//Convert.ToString(Sessionid),
                Session["Cartdetailsftch"] = CartDetail;
                Session["cartcount"] = CartDetail.Count();
                Session["TotPrice"] = CartDetail != null ? CartDetail.Sum(s => s.Price * s.qty).ToString() : "0";
                return PartialView("GetCartCount");
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return RedirectToAction("Home", "Index");
            }
        }
        public ActionResult GetCartDetails(M_Category objg)
        {
            try
            {
                var userid = Session["UserId"];
                var Sessionid = "";
                objg.CartDetail = _iprod.Cartdetailsftch(Convert.ToString(userid));//Convert.ToString(Sessionid),
                Session["Cartdetailsftch"] = objg.CartDetail;
                return PartialView("GetCartDetails", objg);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return RedirectToAction("Home", "Index");
            }
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {


            return View();
        }
        public ActionResult Footer()
        {
            return PartialView("Footer");
        }

        public ActionResult Myorders()
        {
            if (Session["UserDetail"] != null)
            {
                M_Category obj = new M_Category();
                var userid = Session["UserId"];
                var Formno = Convert.ToString(Session["FormNo"]);
                obj.OrderReport = iorderrept.GetOrderdetail(Convert.ToString(userid), Formno);
                return View(obj);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Showofflineorderdetail(M_Category obj, string Action, string OrderId)
        {
            var userid = Session["UserId"];
            obj.OrderNoDetail = iorderrept.Showofflineorderdetail(OrderId, Convert.ToString(userid));
            var tblOrder = Extension.RenderRazorViewToString(this.ControllerContext, "OrderDetail", obj);
            return Json(new { tblOrder });
        }



        public ActionResult MyCoupon()
        {
            if (Session["UserDetail"] != null)
            {
                M_Category obj = new M_Category();
                var userid = Session["UserId"];
                var Formno = Convert.ToString(Session["FormNo"]);
                obj.GetCouponDetailNew = iorderrept.GetCoupondetailNew(Convert.ToString(userid), Formno);
                return View(obj);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult UpadteCartdetail(string ProdId, string Quantity)
        {
            var TotPrice = "";
            var Totbv = "";
            var TotPv = "";
            M_Category objm = new M_Category();
            try
            {
                var userid = Session["UserId"];
                int i = 0;
                var productId = ProdId.Split(',');
                var Quant = Quantity.Split(',');
                for (i = 0; i < productId.Length; i++)
                {
                    objm.CartDetail = _iprod.updateQuantity(productId[i], Quant[i], Convert.ToString(userid));
                    Session["Cartdetailsftch"] = objm.CartDetail;
                    TotPrice = objm.CartDetail.Sum(s => s.Price * s.qty).ToString();
                    Totbv = objm.CartDetail.Sum(s => s.bv * s.qty).ToString();
                    TotPv = objm.CartDetail.Sum(s => s.PV * s.qty).ToString();
                    Session["Cartdetailsftch"] = objm.CartDetail;
                    decimal CourierCharge = 0;
                    Session["CourierCharge"] = CourierCharge;
                }
            }
            catch (Exception ex)
            {

            }
            return Json(new { TotPrice, Totbv, TotPv });
        }

        public ActionResult DeleteLstRowCartPopup(M_Category objm, string Id, string Action, string ProdId, string ProdName, string imagePath, string qty, string Price, string bv, string IpAddress)
        {
            var userid = Session["UserId"];
            var uniqueId = Session["UniqueId"];
            var Sessionid = Session["CurrentUserSessionID"];
            //var Sessionid = Session.SessionID;
            string save = _iprod.deleteProd(Action, Id, ProdId, ProdName, imagePath, qty, Price, bv, IpAddress, "0", Convert.ToString(Sessionid), Convert.ToString(userid));
            objm.CartDetail = _iprod.Cartdetailsftch(Convert.ToString(userid));//Convert.ToString(Sessionid),
            Session["Cartdetailsftch"] = objm.CartDetail;
            var ParentParty = Session["ParentPartyList"] as List<SelectListItem>;
            ViewBag.ParentPartyList = ParentParty;
            var cartCount = objm.CartDetail != null ? objm.CartDetail.Count() : 0;
            var TotPrice = objm.CartDetail != null ? objm.CartDetail.Sum(s => s.Price * s.qty).ToString() : "0";
            return Json(new { cartCount, TotPrice });
        }
        public ActionResult SrchOrderDetail(M_Category obj, string Action, string OrderId)
        {
            var userid = Session["UserId"];
            obj.OrderNoDetail = iorderrept.GetGrdOrderNoDetail(OrderId, Convert.ToString(userid));

            var tblOrder = Extension.RenderRazorViewToString(this.ControllerContext, "OrderDetail", obj);
            return Json(new { tblOrder });
        }

        public ActionResult OrderHistory(M_Category obj)
        {
            var userid = Session["UserId"];

            var Formno = Convert.ToString(Session["FormNo"]);

            obj.OrderReport = iorderrept.GetOrderdetail(Convert.ToString(userid), Formno);

            List<E_OrderReport> lst = new List<E_OrderReport>();
            return View(obj);
        }

        public ActionResult PrivacyPolicy()
        {
            return View();
        }
        public ActionResult ReturnPolicy()
        {
            return View();
        }
        public ActionResult ShippingPolicy()
        {
            return View();
        }

        public ActionResult VisionMission()
        {
            return View();
        }

        public ActionResult BankDetail()
        {
            return View();
        }

        public ActionResult Selfdeclaration()
        {
            return View();
        }
        public ActionResult Termsofuse()
        {
            return View();
        }
        public ActionResult Grievanceredressal()
        {
            return View();
        }
        public ActionResult Disclaimer()
        {
            return View();
        }
        public ActionResult Cancellationpolicy()
        {
            return View();
        }
        //public ActionResult FranchiseList()
        //{
        //    M_ShowFranchiselist obj = new M_ShowFranchiselist();
        //    obj.FranchiseType = _iprod.GetFranchisetype();
        //    obj.Franchiselist = _iprod.GetFranchiseList(0);
        //    return View(obj);
        //}

        public ActionResult FranchiseList(int? franchiseType)
        {
            M_ShowFranchiselist obj = new M_ShowFranchiselist();

            // Load dropdown list (Franchise Types)
            obj.FranchiseType = _iprod.GetFranchisetype();

            // Load full franchise list only once and store in session
            if (Session["FranchiseList"] == null)
            {
                var fullList = _iprod.GetFranchiseList(0); // 0 means all
                Session["FranchiseList"] = fullList;
            }
            var sessionList = Session["FranchiseList"] as List<M_Franchiselist>; // replace with your DTO type
            // Filter based on dropdown selection
            if (franchiseType.HasValue && franchiseType.Value > 0)
            {
                obj.Franchiselist = sessionList.Where(x => x.GroupId == franchiseType.Value).ToList();
            }
            else
            {
                obj.Franchiselist = sessionList;
            }
            return View(obj); // This returns to FranchiseList.cshtml
        }

        public ActionResult WalletDetail()
        {
            if (Session["UserDetail"] != null)
            {
                try
                {
                    M_WalletDetail obj = new M_WalletDetail();
                    var userid = Session["UserId"];
                    var Formno = Convert.ToString(Session["FormNo"]);
                    obj.m_GetWallettypes = _iprod.GetWallettype();
                    obj.walletUseDetail = _iprod.GetWallettypeBalance(Formno, obj.m_GetWallettypes[0].Actype);
                    obj.allWalletDetails = _iprod.GetAllWalletDetail(Formno, obj.m_GetWallettypes[0].Actype);
                    return View(obj);
                }
                catch
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult GetWalletDetail(string wallettype)
        {
            M_WalletDetail obj = new M_WalletDetail();
            var userid = Session["UserId"];
            var Formno = Convert.ToString(Session["FormNo"]);
            obj.walletUseDetail = _iprod.GetWallettypeBalance(Formno, wallettype);
            obj.allWalletDetails = _iprod.GetAllWalletDetail(Formno, wallettype);
            var tblOrder = Extension.RenderRazorViewToString(this.ControllerContext, "WalletDetail_partail", obj);
            return Json(new { tblOrder });
        }

        public ActionResult Levelwisereport()
        {
            if (Session["UserDetail"] != null)
            {
                try
                {
                    GroupDirectReport obj = new GroupDirectReport();
                    var userid = Session["UserId"];
                    var Formno = Convert.ToString(Session["FormNo"]);
                    obj.m_Levels = _iprod.GetLevel(Formno, "");
                    obj.referalDownlineins = _iprod.GetReferalDownlineinfonew(Formno);
                    return View(obj);
                }
                catch
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult GetLevelReport(string MLevel, string Legno, string ActiveStatus, int? page)
        {
            int pageIndex = 1;
            int pagesize = 10;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            if (string.IsNullOrEmpty(ActiveStatus))
            {
                ActiveStatus = "";
            }
            var Formno = Convert.ToString(Session["FormNo"]);
            GroupDirectReport obj = new GroupDirectReport();
            obj.m_LevelDetails = new List<M_LevelDetail>();
            var list = new List<int>();
            DataSet ds = _iprod.sp_GetLevelDetail(MLevel, Legno, ActiveStatus, Formno, 1, 10);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    M_LevelDetail ml = new M_LevelDetail();
                    ml.SNo = Convert.ToInt64(dr["SNo"]);
                    ml.Doj = Convert.ToString(dr["Doj"]);
                    ml.Sponsorid = Convert.ToString(dr["Sponsorid"]);
                    ml.BV = Convert.ToDecimal(dr["BV"]);
                    ml.Idno = Convert.ToString(dr["Idno"]);
                    ml.MemberName = Convert.ToString(dr["MemberName"]);
                    ml.Status = Convert.ToString(dr["Status"]);
                    ml.MemName = Convert.ToString(dr["MemName"]);
                    ml.Mlevel = Convert.ToInt32(dr["Mlevel"]);
                    ml.Packagename = Convert.ToString(dr["Packagename"]);
                    ml.Position = Convert.ToString(dr["Position"]);
                    ml.UpgradeDate = Convert.ToString(dr["UpgradeDate"]);
                    obj.m_LevelDetails.Add(ml);
                }

                int recordCount = Convert.ToInt32(ds.Tables[1].Rows[0]["RecordCount"]);
                list = Enumerable.Range(1, recordCount).ToList();
                obj.pagerCount = list.ToPagedList(pageIndex, pagesize);
            }

            var tblOrder = Extension.RenderRazorViewToString(this.ControllerContext, "Levelwisereport_partail", obj);
            return Json(new { tblOrder });
        }
        public ActionResult LevelIncome()
        {
            if (Session["UserDetail"] != null)
            {
                try
                {
                    GroupDirectReport obj = new GroupDirectReport();
                    obj.m_LevelIncomes = new List<M_LevelIncome>();
                    var userid = Session["UserId"];
                    var Formno = Convert.ToString(Session["FormNo"]);
                    obj.m_Levels = _iprod.GetLevel(Formno, "");
                    DataSet ds = _iprod.GetLevelIncome(Convert.ToInt32(Formno));
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            M_LevelIncome mm = new M_LevelIncome();
                            mm.SNo = ds.Tables[0].Rows.IndexOf(dr) + 1;
                            mm.Slab = Convert.ToString(dr["Slab"]);
                            mm.Date = Convert.ToString(dr["Date"]);
                            mm.Level = Convert.ToString(dr["Level"]);
                            mm.MemberName = Convert.ToString(dr["Member Name"]);
                            mm.Business = Convert.ToString(dr["Business"]);
                            mm.LevelBonus = Convert.ToString(dr["Level Bonus"]);
                            mm.Idno = Convert.ToString(dr["Idno"]);
                            obj.m_LevelIncomes.Add(mm);
                        }

                    }
                    return View(obj);
                }
                catch
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult DirectSellerContrazt()
        {
            return View();
        }
        public ActionResult UserKyc()
        {
            if (Session["UserDetail"] != null)
            {
                //  string dojFromDb = Convert.ToString(Session["Doj"]);
                //  DateTime userDoj = DateTime.ParseExact(
                //    dojFromDb,
                //    "dd-MM-yyyy HH:mm:ss",
                //    CultureInfo.InvariantCulture
                //);
                string dojFromDb = Convert.ToString(Session["Doj"]);

                DateTime userDoj;

                if (DateTime.TryParseExact(
                        dojFromDb,
                        "dd-MM-yyyy HH:mm:ss",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out userDoj))
                {
                    // success
                }
                else
                {
                    // fallback parsing (auto detect format)
                    userDoj = Convert.ToDateTime(dojFromDb);
                }
                DateTime compareDate = DateTime.ParseExact(
                 "23-01-2026",
                 "dd-MM-yyyy",
                CultureInfo.InvariantCulture);
                if (Convert.ToString(Session["ispancard"]) == "Y" && userDoj.Date >= compareDate)
                {
                    return RedirectToAction("UserKycByapi", "Home");
                }

                M_UserKYC obj = new M_UserKYC();
                Getkycreq req = new Getkycreq();
                req.islogin = "N";
                req.reqtype = "getkyc";
                req.userid = Convert.ToString(Session["IDNO"]);
                req.passwd = Convert.ToString(Session["password"]);
                string jsonreq = JsonConvert.SerializeObject(req);
                var response = CallPostFunction(jsonreq, Apiurl);
                Getkycres kycresponse = JsonConvert.DeserializeObject<Getkycres>(response);
                obj.Userkycres = kycresponse;
                List<State> lst = SateList();
                obj.states = lst;
                obj.BankLists = _iprod.GetbankLists();
                obj.kycTypeMasters = _iprod.kycTypeMasters();


                return View(obj);
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult UserKycByapi()
        {
            if (Session["UserDetail"] != null)
            {
                M_UserKYC obj = new M_UserKYC();
                Getkycreq req = new Getkycreq();
                req.islogin = "N";
                req.reqtype = "getkyc";
                req.userid = Convert.ToString(Session["IDNO"]);
                req.passwd = Convert.ToString(Session["password"]);
                string jsonreq = JsonConvert.SerializeObject(req);
                var response = CallPostFunction(jsonreq, Apiurl);
                Getkycres kycresponse = JsonConvert.DeserializeObject<Getkycres>(response);
                obj.Userkycres = kycresponse;
                List<State> lst = SateList();
                obj.states = lst;
                obj.BankLists = _iprod.GetbankLists();
                obj.kycTypeMasters = _iprod.kycTypeMasters();
                return View(obj);
            }
            return RedirectToAction("Index", "Home");
        }
        public List<State> SateList()
        {
            Satarereq req = new Satarereq();
            req.islogin = "N";
            req.reqtype = "statelist";
            req.countrycode = "1";
            List<State> lst = new List<State>();
            var detail = JsonConvert.SerializeObject(req);
            var stateresponse = CallPostFunction(detail, Apiurl);
            var output = JsonConvert.DeserializeObject<Stateroot>(stateresponse);
            if (output != null && output.response == "OK")
            {
                lst = output.states;
            }
            return lst;
        }
        public ActionResult ReferalLinkPage()
        {
            if (Session["UserDetail"] != null)
            {
                M_Referral obj = new M_Referral();
                Referalreq referalreq = new Referalreq();
                referalreq.islogin = "N";
                referalreq.reqtype = "referrallink";
                referalreq.userid = Convert.ToString(Session["IDNO"]);
                referalreq.passwd = Convert.ToString(Session["password"]);
                var detail = JsonConvert.SerializeObject(referalreq);
                var response = CallPostFunction(detail, Apiurl);
                Referalres referalres = JsonConvert.DeserializeObject<Referalres>(response);
                if (referalres.response == "OK")
                {
                    obj.urlLeft = referalres.urlLeft;
                    obj.urlright = referalres.urlright;
                }
                return View(obj);
            }
            return RedirectToAction("Index", "Home");
        }
        public string CallPostFunction(string detail, string url)
        {
            try
            {
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
                        string result = streamReader.ReadToEnd();
                        return result; // Optionally deserialize this JSON string to an object
                    }
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }
            return "";
        }
        public ActionResult KYCWarning()
        {
            return View();
        }
        public ActionResult MemberProfile()
        {
            if (Session["UserDetail"] != null)
            {
                M_Profile obj = new M_Profile();
                Profilereq req = new Profilereq();
                req.islogin = "N";
                req.reqtype = "getprofile";
                req.memberid = Convert.ToString(Session["IDNO"]);
                req.userid = Convert.ToString(Session["IDNO"]);
                req.passwd = Convert.ToString(Session["password"]);
                string detail = JsonConvert.SerializeObject(req);
                var response = CallPostFunction(detail, Apiurl);
                Profileres profileres = JsonConvert.DeserializeObject<Profileres>(response);
                obj.profileres = profileres;
                return View(obj);
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Certificate()
        {
            return View();
        }
        public ActionResult RegisterOfDs(int? page)
        {
            int pageSize = 10;                 // kitne records per page
            int pageNumber = page ?? 1;        // current page

            M_DelistedDirectSeller obj = new M_DelistedDirectSeller();

            var list = iler.GetRegisterOrderdetail()
                           .OrderBy(x => x.MemberID)
                           .ToPagedList(pageNumber, pageSize);

            obj.DirectsellerReport = list;

            return View(obj);
        }
        //public ActionResult RegisterOfDs()
        //{
        //    M_DelistedDirectSeller obj = new M_DelistedDirectSeller();
        //    obj.DirectsellerReport = iler.GetRegisterOrderdetail();
        //    return View(obj);
        //}
        public ActionResult DeListedDsd(int? page)
        {

            //M_DelistedDirectSeller obj = new M_DelistedDirectSeller();
            //obj.DirectsellerReport = iler.GetOrderdetail();
            //return View(obj);
            int pageSize = 10;                 // kitne records per page
            int pageNumber = page ?? 1;        // current page

            M_DelistedDirectSeller obj = new M_DelistedDirectSeller();

            var list = iler.GetOrderdetail()
                           .OrderBy(x => x.MemberID)
                           .ToPagedList(pageNumber, pageSize);

            obj.DirectsellerReport = list;

            return View(obj);
        }
        public ActionResult GrievanceRRedressal()
        {
            return View();
        }
        public ActionResult GrievanceRedressalMechanism()
        {
            M_Complaint obj = new M_Complaint();
            obj.complainttype = GetComplainttype();
            return View(obj);
        }
        public List<Complainttype> GetComplainttype()
        {
            List<Complainttype> complist = new List<Complainttype>();
            try
            {
                Complainttypelist req = new Complainttypelist();
                req.islogin = "N";
                req.reqtype = "complainttypewithoutlogin";
                req.userid = Convert.ToString(Session["IDNO"]);
                req.passwd = Convert.ToString(Session["password"]);
                var detail = JsonConvert.SerializeObject(req);
                var response = CallPostFunction(detail, Apiurl);
                complainttyperes res = JsonConvert.DeserializeObject<complainttyperes>(response);
                if (res.response == "OK")
                {
                    complist = res.complainttype;
                }
            }
            catch
            {

            }
            return complist;
        }
        [HttpPost]
        public ActionResult SaveCompaint(string Complaintid, string Subject, string Email, string Name, string Mobileno, string Description, string memberid)
        {
            string msg = string.Empty;
            string status = "";
            try
            {
                Compaintreq req = new Compaintreq();
                req.islogin = "N";
                req.reqtype = "savecomplaintwithout";
                req.userid = Convert.ToString(Session["IDNO"]);
                req.passwd = Convert.ToString(Session["password"]);
                req.complaintid = Complaintid;
                req.idno = Convert.ToString(memberid);
                req.name = Name;
                req.mobileno = Mobileno;
                req.email = Email;
                req.subject = Subject;
                req.description = Description;
                var detail = JsonConvert.SerializeObject(req);
                var response = CallPostFunction(detail, Apiurl);
                Compaintres res = JsonConvert.DeserializeObject<Compaintres>(response);
                if (res.response == "OK")
                {
                    msg = res.msg;
                    status = "OK";
                }
                else
                {
                    msg = res.msg;
                }
            }
            catch
            {

            }
            return Json(new { msg, status });
        }
        [HttpPost]
        public ActionResult memberdatacheck(string memberid)
        {
            try
            {
                Compaintmemberdatareq req = new Compaintmemberdatareq
                {
                    islogin = "N",
                    reqtype = "getmemberdata",
                    memberid = memberid
                };
                var detail = JsonConvert.SerializeObject(req);
                var response = CallPostFunction(detail, Apiurl);
                Compaintmemberdatares res = JsonConvert.DeserializeObject<Compaintmemberdatares>(response);
                return Json(res); // 👈 Full JSON return
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    response = "ERROR",
                    msg = "Server error",
                    error = ex.Message
                });
            }
        }
        public ActionResult GetComplaintReplyDetails(string complaintid)
        {
            ComplaintReplyDetailsRes obj = new ComplaintReplyDetailsRes();
            try
            {
                ComplaintReplyDetailsReq req = new ComplaintReplyDetailsReq();
                req.islogin = "N";
                req.reqtype = "complaintreplywith";
                req.userid = Convert.ToString(Session["IDNO"]);
                req.passwd = Convert.ToString(Session["password"]);
                req.complaintid = complaintid;
                var detail = JsonConvert.SerializeObject(req);
                var response = CallPostFunction(detail, Apiurl);
                obj = JsonConvert.DeserializeObject<ComplaintReplyDetailsRes>(response);
            }
            catch
            {

            }
            var tblOrder = Extension.RenderRazorViewToString(this.ControllerContext, "Complaintdetail_partail", obj);
            return Json(new { tblOrder });
        }
        [HttpPost]
        public JsonResult CheckCheckStatus(string Complaintid, string memberid)
        {
            var list = Complaintdetail(memberid, Complaintid, "1", "100");

            if (list != null && list.Count > 0)
            {
                return Json(new
                {
                    response = "OK",
                    list = list,
                    msg = "Success"
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    response = "FAIL",
                    list = new List<Complaintdetail>(),
                    msg = "No record found"
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult CheckStatus()
        {
        return View();
        }
        private List<Complaintdetail> Complaintdetail(string idno, string Complaintno, string from, string to)
        {
            List<Complaintdetail> rlist = new List<Complaintdetail>();
            try
            {
                ComplaintDetailsreq req = new ComplaintDetailsreq();
                req.islogin = "N";
                req.reqtype = "complaintdetailwithno";
                req.userid = Convert.ToString(idno);
                req.passwd = Convert.ToString(Complaintno);
                req.from = from;
                req.to = to;
                var detail = JsonConvert.SerializeObject(req);
                var response = CallPostFunction(detail, Apiurl);
                ComplaintDetailsRes res = JsonConvert.DeserializeObject<ComplaintDetailsRes>(response);
                if (res.response == "OK")
                {
                    rlist = res.complaintdetail;
                }
            }
            catch
            {

            }
            return rlist;
        }
    }
}