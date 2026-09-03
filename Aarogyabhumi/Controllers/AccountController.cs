using Newtonsoft.Json;
using Shopinv.Entity;
using Shopinv.Interface;
using Shopinv.Models;
using Shopinv.SiteExtension;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Xml;

namespace Shopinv.Controllers
{
    public class AccountController : Controller
    {
        private readonly I_register register = null;
        private readonly I_Category icateogry = null;
        private readonly I_Product iprod = null;
        private readonly I_Login _ilogin = null;
        private readonly static string Apiurl = ConfigurationManager.AppSettings["ApiUrl"];
        private readonly static string SiteUrl = ConfigurationManager.AppSettings["SiteUrl"];
        private readonly static string CpanelUrl = ConfigurationManager.AppSettings["CpanelUrl"];
        private readonly static string GvPortalUrl = ConfigurationManager.AppSettings["GvPortalUrl"];
        private readonly static string StorePortalUrl = ConfigurationManager.AppSettings["StorePortalUrl"];
        private readonly static string Sendbox_xapikey = ConfigurationManager.AppSettings["Sendbox_xapikey"];
        private readonly static string Sendbox_xapisecret = ConfigurationManager.AppSettings["Sendbox_xapisecret"];
        private readonly static string Sendbox_authenticate = ConfigurationManager.AppSettings["Sendbox_authenticate"];
        private readonly static string Sendbox_panverify = ConfigurationManager.AppSettings["Sendbox_panverify"];
        private readonly static string Sendbox_accountverify = ConfigurationManager.AppSettings["Sendbox_accountverify"];
        CompanyDetail companyDetail;
        public AccountController(I_Login ilogin, I_Product iprod)
        {
            _ilogin = ilogin;
            this.iprod = iprod;
            companyDetail = new CompanyDetail(this.iprod);
            companyDetail.GetCompanydetail();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken()]
        public ActionResult Login(LoginLPlus Loginobj)
        {
            if (ModelState.IsValid)
            {
                string URL = System.Web.HttpContext.Current.Request.Url.Host.ToUpper().Replace("HTTP://", "").Replace("HTTPS://", "").Replace("WWW.", "").Replace("/", "");// System.Web.HttpContext.Current.Request.UserHostName;
                var isValidUser = IsValid(Loginobj);
                if (isValidUser != null)
                {
                    FormsAuthentication.SetAuthCookie(Loginobj.UserName, false);
                    var ReturnUrl = Session["RedirectUrl"];
                    var userid = Session["UserId"];
                    List<E_CartDetails> CartDetail = iprod.Cartdetailsftch(Convert.ToString(userid)).ToList();//Convert.ToString(Sessionid),
                    Session["Cartdetailsftch"] = CartDetail;
                    Session["cartcount"] = CartDetail.Count();
                    Session["TotPrice"] = CartDetail != null ? CartDetail.Sum(s => s.Price * s.qty).ToString() : "0";
                    Session["isKycCompleted"] = true;
                    if (!string.IsNullOrEmpty(Convert.ToString(Session["Isredirect"])) && Convert.ToString(Session["Isredirect"]) == "Y")
                    {
                        return RedirectToAction("ProductDetail", "ProductDetail", new { ProdId = Convert.ToString(Session["ProdId"]) });
                    }
                    else if (!string.IsNullOrEmpty(Convert.ToString(Session["IsCateredirect"])) && Convert.ToString(Session["IsCateredirect"]) == "Y")
                    {
                        return RedirectToAction("CategoryList", "CategoryList", new { CatName = Convert.ToString(Session["CatName"]), Subcate= Convert.ToString(Session["Subcate"]) });
                    }
                    // string idNo = Convert.ToString(Session["IDNO"]);
                    // string password = Convert.ToString(Session["password"]);
                    // var lgnT = "uid=" + idNo + "&pwd=" + password;
                    // var lgntenc = TextCrypto.Encrypt(lgnT);
                    // string url = CpanelUrl + "/Default.aspx?lgnT=" + lgntenc;
                    // // Step 4: Redirect
                    // return Redirect(url);
                    return RedirectToAction("Index", "Home");
                }
                else
                {

                    ViewBag.Message = "Invalid credentials.";
                }
            }
            else
            {
                ViewBag.Message = "Invalid credentials.";

            }
            return View(Loginobj);
        }
        public string IsValid(LoginLPlus model)
        {
            List<E_RegisterUser> listUserLog = new List<E_RegisterUser>();

            DataTable UserLogin = new DataTable();
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            string username = "";

            hst.Add("Action", "Login");
            hst.Add("UserName", model.UserName);
            hst.Add("Password", model.Password);

            using (DataSet ds = blldb.GetDataSet("sp_Login", CommandType.StoredProcedure, hst))
            {
                E_RegisterUser User = new E_RegisterUser();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        User.UserName = Convert.ToString(row["IdNo"]);
                        User.Password = Convert.ToString(row["Passw"]);
                        User.Firstname = Convert.ToString(row["MemFirstName"]);
                        User.LastName = Convert.ToString(row["MemLastName"]);
                        User.MobileNo = Convert.ToString(row["Mobl"]);
                        User.FormNo = Convert.ToString(row["FormNo"]);
                        User.ActiveStatus = Convert.ToString(row["ActiveStatus"]);
                        User.Fax = Convert.ToString(row["Fax"]);
                        User.Address = Convert.ToString(row["Address1"]);
                        User.City = Convert.ToString(row["City"]);
                        User.CityCode = Convert.ToInt32(row["CityCode"]);
                        User.District = Convert.ToString(row["District"]);
                        User.DistrictCode = Convert.ToInt32(row["DistrictCode"]);
                        User.StateCode = Convert.ToInt32(row["StateCode"]);
                        User.PinCode = Convert.ToString(row["PinCode"]);
                        User.CountryId = Convert.ToInt32(row["CountryId"]);
                        User.CountryName = Convert.ToString(row["CountryName"]);
                        User.Email = Convert.ToString(row["Email"]);

                        //User.randomId = Encrypt(Convert.ToString(row["randomId"]));
                        Session["UserName"] = User.Firstname;
                        Session["Email"] = User.Email;
                        Session["MobileNo"] = User.MobileNo;
                        Session["IDNO"] = User.UserName;
                        Session["FormNo"] = User.FormNo;
                        Session["password"] = User.Password;
                        Session["ActiveStatus"] = User.ActiveStatus;
                        Session["EPassw"] = Convert.ToString(row["EPassw"]);
                        Session["KitId"] = Convert.ToString(row["KitId"]);
                        Session["MemMode"] = Convert.ToString(row["Fld5"]);
                        Session["ispancard"] = Convert.ToString(row["ispancard"]);
                        Session["Doj"] = Convert.ToString(row["Doj"]);
                        username = User.UserName;
                        listUserLog.Add(User);
                        var RandomeNo = GenerateRandomCode();
                        Session["EncryptrandomId"] = RandomeNo;
                        Session["randomId"] = RandomeNo;
                        IEnumerable<E_RegisterUser> login = _ilogin.SaveloginDetails(Convert.ToString(row["IdNo"]),
                            Convert.ToString(row["Passw"]), Convert.ToString(row["MemFirstName"]),
                            Convert.ToString(row["MemLastName"]), Convert.ToString(row["Mobl"]),
                            Convert.ToString(row["FormNo"]), Convert.ToString(row["ActiveStatus"]),
                            Convert.ToString(row["Fax"]), Convert.ToString(row["Address1"]),
                            Convert.ToString(row["City"]), Convert.ToString(row["CityCode"]),
                            Convert.ToString(row["District"]),
                            Convert.ToString(row["DistrictCode"]),
                            Convert.ToString(row["StateCode"]),
                            Convert.ToString(row["PinCode"]),
                            Convert.ToString(row["CountryId"]),
                            Convert.ToString(row["CountryName"]),
                            Convert.ToString(RandomeNo)
                            , Convert.ToString(row["Email"]));
                        Session["UserId"] = login.ToList()[0].Id;
                        var randomid = login.ToList()[0].randomId;
                        var randomId = Encrypt(randomid);
                        //Session["EncryptrandomId"] = randomId;
                    }
                    Session["UserDetail"] = User;
                    Session["UserDetailList"] = listUserLog;
                    UserLogin = Extension.ToDataTable(listUserLog);

                }

                if (username == "")
                    return null;
                else
                {
                    return username;
                }

            }

        }

        private string GenerateRandomCode()
        {
            try
            {
                string[] strArray = new string[36];
                strArray = new string[] { "1", "2", "5", "7", "8", "9", "6", "3", "4" };

                Random autoRand = new Random();
                string strCaptcha = string.Empty;
                for (int i = 0; i < 5; i++)
                {
                    int j = Convert.ToInt32(autoRand.Next(0, 8));
                    strCaptcha += strArray[j].ToString();
                }

                return strCaptcha;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string Encrypt(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }

        public ActionResult SignUp(string refid)
        {
            string side = "", sponsorrefid = "";
            RegisterUser obj = new RegisterUser();
            ViewBag.isref = "N";
            if (!string.IsNullOrEmpty(refid))
            {
                string req = refid.Replace(" ", "+");
                string str = TextCrypto.Decrypt(req);
                string[] rfAr = str.Split('/');
                sponsorrefid = rfAr[0];
                side = rfAr[1];
                ViewBag.isref = "Y";
                obj.referralid = sponsorrefid;
            }
            obj.side = side;
            //List<State> lst = SateList();
            //obj.states = lst;
            return View(obj);
        }
        [HttpGet]
        public JsonResult GetSponsorid(string sponsor)
        {
            string msg = "";
            string err = "0";
            if (sponsor != "")
            {
                var req = new
                {
                    reqtype = "checksponsor",
                    islogin = "N",
                    sponsorid = sponsor
                };
                var detail = JsonConvert.SerializeObject(req);
                var response = CallPostFunction(detail, Apiurl);
                var output = JsonConvert.DeserializeObject<sponsorResponse>(response);
                if (output.response != null)
                {
                    if (output.response == "OK")
                    {
                        msg = output.sponsorname;
                    }
                    else
                    {
                        msg = output.msg;
                        err = "1";
                    }
                }
            }
            return Json(new { msg, err }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult SignUp(RegisterUser obj)
        {
            DataTable UserLogin = new DataTable();
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();

            string referralid = "", side = "";
            side = "1";
            referralid = obj.referralid;
            obj.fortype = "D";
            var reqregistration = new
            {
                islogin = "N",
                reqtype = "joining",
                fortype = obj.fortype,
                referralid = referralid,
                side = side,
                name = obj.name,
                email = obj.email,
                mobl = obj.mobl,
                dob = string.IsNullOrEmpty(obj.dob) ? "" : obj.dob,
                panno = string.IsNullOrEmpty(obj.panno) ? "" : obj.panno
            };
            //ViewBag.statcode = obj.statecode;
            var detail = JsonConvert.SerializeObject(reqregistration);
            var response = CallPostFunction(detail, Apiurl);
            var output = JsonConvert.DeserializeObject<Signupresponse>(response);
            if (output != null)
            {
                if (output.response == "OK")
                {
                    ViewBag.msg = output.msg;
                    ModelState.Clear();
                    obj = new RegisterUser();
                    ViewBag.response = "OK";
                    ViewBag.idno = output.idno;
                    ViewBag.password = output.password;
                }
                else
                {
                    ViewBag.msg = output.msg;
                }
            }
            else
            {
                ViewBag.msg = "Something went wrong";
            }
            //List<State> lst = SateList();
            //obj.states = lst;
            return View(obj);
        }
        public List<State> SateList()
        {
            Satarereq req = new Satarereq();
            req.islogin = "N";
            req.reqtype = "statelist";
            req.countrycode = "1";
            List<State> lst = new List<State>();
            var detail = JsonConvert.SerializeObject(req);
            var stateresponse = CallPostFunction(detail, "https://cpanel.bsnprojects.com//Processapiwithk");
            var output = JsonConvert.DeserializeObject<Stateroot>(stateresponse);
            if (output != null && output.response == "OK")
            {
                lst = output.states;
            }
            return lst;
        }
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            Session["UserName"] = null;
            Session["UserDetail"] = null;
            Session.Abandon();
            Session.Clear();
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

        public ActionResult RedirectToCPanel()
        {
            // Step 1: Compose the info1 string
            string idNo = Convert.ToString(Session["IDNO"]);
            string password = Convert.ToString(Session["password"]);
            var lgnT = "uid=" + idNo + "&pwd=" + password;
            var lgntenc = TextCrypto.Encrypt(lgnT);
            string url = CpanelUrl + "/Default.aspx?lgnT=" + lgntenc;
            // Step 4: Redirect
            return Redirect(url);
        }

        public ActionResult ResiterUserdetail(string idno, string passw)
        {
            ViewBag.idno = idno;
            ViewBag.passw = passw;
            return View();
        }

        private string Base64Encode(string plainText)
        {
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainBytes);
        }

        private string Base64Decode(string base64EncodedData)
        {
            byte[] decodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(decodedBytes);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Directlogin(string refs, string info)
        {
            try
            {
                List<E_RegisterUser> listUserLog = new List<E_RegisterUser>();
                DataTable UserLogin = new DataTable();
                string username = "";
                //testing
                string ref11 = "Login";
                string info1 = "DS123456;Make@654321";
                var red = Base64Encode(ref11);
                var ww = Base64Encode(info1);
                var ref1 = Base64Decode(refs);
                var detail = Base64Decode(info);
                if (detail != null && detail.Contains(";"))
                {
                    var dataArray = detail.Split(';');
                    if (dataArray.Length == 2)
                    {
                        //DateTime queryTime = DateTime.ParseExact(dataArray[2], "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                        //var currenttimeStr = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                        //DateTime currentTime = DateTime.ParseExact(currenttimeStr, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                        //var span = currentTime.Subtract(queryTime);
                        //if (span.TotalMinutes <= 1)
                        //{
                        var userDetail = new E_RegisterUser();
                        userDetail.UserName = dataArray[0];
                        userDetail.Password = dataArray[1];

                        var result = _ilogin.LoginApiUser("Login", userDetail);
                        E_RegisterUser User = new E_RegisterUser();
                        if (result.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in result.Tables[0].Rows)
                            {
                                User.UserName = Convert.ToString(row["IdNo"]);
                                User.Password = Convert.ToString(row["Passw"]);
                                User.Firstname = Convert.ToString(row["MemFirstName"]);
                                User.LastName = Convert.ToString(row["MemLastName"]);
                                User.MobileNo = Convert.ToString(row["Mobl"]);
                                User.FormNo = Convert.ToString(row["FormNo"]);
                                User.ActiveStatus = Convert.ToString(row["ActiveStatus"]);
                                User.Fax = Convert.ToString(row["Fax"]);
                                User.Address = Convert.ToString(row["Address1"]);
                                User.City = Convert.ToString(row["City"]);
                                User.CityCode = Convert.ToInt32(row["CityCode"]);
                                User.District = Convert.ToString(row["District"]);
                                User.DistrictCode = Convert.ToInt32(row["DistrictCode"]);
                                User.StateCode = Convert.ToInt32(row["StateCode"]);
                                User.PinCode = Convert.ToString(row["PinCode"]);
                                User.CountryId = Convert.ToInt32(row["CountryId"]);
                                User.CountryName = Convert.ToString(row["CountryName"]);
                                User.Email = Convert.ToString(row["Email"]);
                                Session["UserName"] = User.Firstname;
                                Session["IDNO"] = User.UserName;
                                Session["FormNo"] = User.FormNo;
                                Session["password"] = User.Password;
                                Session["MobileNo"] = User.MobileNo;
                                Session["Email"] = User.Email;
                                Session["password"] = User.Password;
                                Session["ActiveStatus"] = User.ActiveStatus;
                                Session["EPassw"] = Convert.ToString(row["EPassw"]);
                                Session["KitId"] = Convert.ToString(row["KitId"]);
                                Session["MemMode"] = Convert.ToString(row["Fld5"]);
                                Session["ispancard"] = Convert.ToString(row["ispancard"]);
                                Session["Doj"] = Convert.ToString(row["Doj"]);
                                username = User.UserName;
                                listUserLog.Add(User);
                                var RandomeNo = GenerateRandomCode();
                                Session["EncryptrandomId"] = RandomeNo;
                                Session["randomId"] = RandomeNo;
                                IEnumerable<E_RegisterUser> login = _ilogin.SaveloginDetails(Convert.ToString(row["IdNo"]), Convert.ToString(row["Passw"]), Convert.ToString(row["MemFirstName"]), Convert.ToString(row["MemLastName"]), Convert.ToString(row["Mobl"]), Convert.ToString(row["FormNo"]), Convert.ToString(row["ActiveStatus"]), Convert.ToString(row["Fax"]), Convert.ToString(row["Address1"]), Convert.ToString(row["City"]), Convert.ToString(row["CityCode"]), Convert.ToString(row["District"]), Convert.ToString(row["DistrictCode"]), Convert.ToString(row["StateCode"]), Convert.ToString(row["PinCode"]), Convert.ToString(row["CountryId"]), Convert.ToString(row["CountryName"]), Convert.ToString(RandomeNo), Convert.ToString(row["Email"]));
                                Session["UserId"] = login.ToList()[0].Id;
                                var randomid = login.ToList()[0].randomId;
                                var randomId = Encrypt(randomid);
                                //Session["EncryptrandomId"] = randomId;
                            }
                            Session["UserDetail"] = User;
                            Session["UserDetailList"] = listUserLog;
                            UserLogin = Extension.ToDataTable(listUserLog);
                        }
                        return RedirectToAction("Index", "Home");
                        //}
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            catch (Exception e)
            {

            }
            return RedirectToAction("Index", "Home");
        }

        [KycRequired]
        public ActionResult ChangePassword()
        {
            M_Changepassword obj = new M_Changepassword();
            return View(obj);
        }

        [HttpPost]
        public ActionResult ChangePassword(M_Changepassword obj)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    Changepasswordreq req = new Changepasswordreq();
                    req.islogin = "N";
                    req.reqtype = "cpassword";
                    req.userid = Convert.ToString(Session["IDNO"]);
                    req.passwd = obj.passwd;
                    req.npasswd = obj.npasswd;
                    var detail = JsonConvert.SerializeObject(req);
                    var response = CallPostFunction(detail, Apiurl);
                    var output = JsonConvert.DeserializeObject<Changepasswordres>(response);
                    if (output.response == "OK")
                    {
                        ViewBag.passerr = "Change";
                    }
                    else
                    {
                        ViewBag.passerr = output.msg;
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return View(obj);
        }
        [KycRequired]
        public ActionResult ChangeTxnPassword()
        {
            M_Changepassword obj = new M_Changepassword();
            return View(obj);
        }

        [HttpPost]
        public ActionResult ChangeTxnPassword(M_Changepassword obj)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    Changetxnpasswordreq req = new Changetxnpasswordreq();
                    req.islogin = "N";
                    req.reqtype = "ctpassword";
                    req.userid = Convert.ToString(Session["IDNO"]);
                    req.passwd = Convert.ToString(Session["password"]);
                    req.tpasswd = obj.passwd;
                    req.ntpasswd = obj.npasswd;
                    var detail = JsonConvert.SerializeObject(req);
                    var response = CallPostFunction(detail, Apiurl);
                    var output = JsonConvert.DeserializeObject<Changepasswordres>(response);
                    if (output.response == "OK")
                    {
                        ViewBag.passerr = "Change";
                        ModelState.Clear();
                        obj = new M_Changepassword();
                    }
                    else
                    {
                        ViewBag.passerr = output.msg;
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return View(obj);
        }
        [KycRequired]
        public ActionResult RaiseComplaint()
        {
            if (Session["UserDetail"] != null)
            {
                M_Complaint obj = new M_Complaint();
                obj.complainttype = GetComplainttype();
                return View(obj);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult SaveCompaint(string Complaintid, string Subject, string Email,
                string Name, string Mobileno, string Description)
        {
            string msg = string.Empty;
            string status = "";
            try
            {
                Compaintreq req = new Compaintreq();
                req.islogin = "N";
                req.reqtype = "savecomplaint";
                req.userid = Convert.ToString(Session["IDNO"]);
                req.passwd = Convert.ToString(Session["password"]);
                req.complaintid = Complaintid;
                req.idno = Convert.ToString(Session["IDNO"]);
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

        public List<Complainttype> GetComplainttype()
        {
            List<Complainttype> complist = new List<Complainttype>();
            try
            {
                Complainttypelist req = new Complainttypelist();
                req.islogin = "N";
                req.reqtype = "complainttype";
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
        [KycRequired]
        public ActionResult TicketList()
        {
            if (Session["UserDetail"] != null)
            {
                M_Complaint obj = new M_Complaint();
                obj.complaintdetail = Complaintdetail("1", "100");
                return View(obj);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private List<Complaintdetail> Complaintdetail(string from, string to)
        {
            List<Complaintdetail> rlist = new List<Complaintdetail>();
            try
            {
                ComplaintDetailsreq req = new ComplaintDetailsreq();
                req.islogin = "N";
                req.reqtype = "complaintdetail";
                req.userid = Convert.ToString(Session["IDNO"]);
                req.passwd = Convert.ToString(Session["password"]);
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

        public ActionResult GetComplaintReplyDetails(string complaintid)
        {
            ComplaintReplyDetailsRes obj = new ComplaintReplyDetailsRes();
            try
            {
                ComplaintReplyDetailsReq req = new ComplaintReplyDetailsReq();
                req.islogin = "N";
                req.reqtype = "complaintreply";
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

        public ActionResult UpdateUserKyc()
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
                obj.BankLists = iprod.GetbankLists();
                obj.kycTypeMasters = iprod.kycTypeMasters();
                string dojFromDb = Convert.ToString(Session["Doj"]);

                DateTime userDoj = DateTime.ParseExact(
                    dojFromDb,
                    "dd-MM-yyyy HH:mm:ss",
                    CultureInfo.InvariantCulture
                );

                DateTime compareDate = DateTime.ParseExact(
                 "23-01-2026",
                 "dd-MM-yyyy",
                CultureInfo.InvariantCulture);

                if (Convert.ToString(Session["ispancard"]) == "Y" && userDoj.Date >= compareDate)
                {
                    return View("~/Views/Home/UpdateUserKycByapi.cshtml", obj);
                }
                else
                {
                    return View("~/Views/Home/UpdateUserKyc.cshtml", obj);
                }
            }
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        /// Verifies a PAN (Permanent Account Number) against the provided name and date of birth using an external
        /// verification service.
        /// </summary>
        /// <remarks>The method normalizes and validates the PAN format before attempting verification. If
        /// the PAN, name, or date of birth do not match the official records, the response will indicate the specific
        /// mismatch. The method returns a generic error message if an unexpected error occurs. This action is intended
        /// for use in KYC (Know Your Customer) onboarding scenarios.</remarks>
        /// <param name="Panno">The PAN to verify. Must be a valid 10-character PAN in the format of five uppercase letters, four digits,
        /// and one uppercase letter. Cannot be null or empty.</param>
        /// <param name="Name">The full name to match against the name on record for the provided PAN. Cannot be null or empty.</param>
        /// <param name="DOB">The date of birth to match against the PAN record, as a string. Should be in a format that can be parsed as
        /// a date (e.g., 'dd/MM/yyyy').</param>
        /// <returns>A JSON result indicating whether the PAN, name, and date of birth match the official records. The result
        /// contains a status flag and a message describing the outcome.</returns>
        [HttpPost]
        public async Task<ActionResult> PanVerify(string Panno, string Name, string DOB)
        {
            string jsonres = "";
            string reqjsonn = "";
            string errorstep = "Start";
            try
            {
                DateTime parsedDate;
                string formattedDate = "";
                // Try parsing with current culture + invariant culture
                bool isValid = DateTime.TryParse(
                    DOB,
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out parsedDate
                );
                formattedDate = parsedDate.ToString("dd/MM/yyyy");
                if (formattedDate.Contains("-"))
                {
                    formattedDate = formattedDate.Replace("-", "/");
                }
                PanVerifyreq reqobj = new PanVerifyreq();
                reqobj.Pan = Panno;
                reqobj.Pan = reqobj.Pan.Trim().ToUpper();  // normalize
                if (!Regex.IsMatch(reqobj.Pan, @"^[A-Z]{5}[0-9]{4}[A-Z]$"))
                {
                    return Json(new { Status = false, Message = "Invalid pan" }, JsonRequestBehavior.AllowGet);
                }
                reqobj.NameAsPerPan = Name;
                reqobj.DateOfBirth = formattedDate;
                reqjsonn = JsonConvert.SerializeObject(reqobj);

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string accesstoken = await SendboxAuthenticate();
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, Sendbox_panverify);
                request.Headers.Add("Authorization", accesstoken);
                request.Headers.Add("x-accept-cache", "true");
                request.Headers.Add("x-api-key", Sendbox_xapikey);

                //var content = new StringContent("\n{\n  \"@entity\": \"in.co.sandbox.kyc.pan_verification.request\",\n  \"pan\": \"kqjps3219e\",\n  \"name_as_per_pan\": \"Ajay singh shaktawat\",\n  \"date_of_birth\": \"03/03/2000\",\n  \"consent\": \"Y\",\n  \"reason\": \"KYC onboarding\"\n}\n", null, "application/json");
                var content = new StringContent(reqjsonn, null, "application/json");

                request.Content = content;
                var response = await client.SendAsync(request);
                errorstep = "Pan response";
                if (response.IsSuccessStatusCode)
                {
                    errorstep = "Pan response if";
                    response.EnsureSuccessStatusCode();
                    jsonres = await response.Content.ReadAsStringAsync();
                    PanVerificationApiResponse resobj = JsonConvert.DeserializeObject<PanVerificationApiResponse>(jsonres);
                    errorstep = "Pan response deserialize";
                    if (resobj.Data.Status == "valid")
                    {
                        errorstep = "Pan response if";
                        if (resobj.Data.NameAsPerPanMatch != true)
                        {
                            return Json(new { Status = false, Message = "Enter name as per panmatch" }, JsonRequestBehavior.AllowGet);
                        }
                        if (resobj.Data.DateOfBirthMatch != true)
                        {
                            return Json(new { Status = false, Message = "Enter date Of birth match from pan" }, JsonRequestBehavior.AllowGet);
                        }

                        return Json(new { Status = true, Message = "Pan verified successfully" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { Status = false, Message = "Invalid pan" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { Status = false, Message = "Invalid pan" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Something Went Wrong" }, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// Authenticates with the Sendbox API and retrieves an access token for subsequent requests.
        /// </summary>
        /// <remarks>The returned access token can be used to authorize further API calls to the Sendbox
        /// service. If authentication fails or an error occurs, the method returns an empty string. This method
        /// performs an asynchronous HTTP request and should be awaited to avoid blocking the calling thread.</remarks>
        /// <returns>A string containing the access token if authentication is successful; otherwise, an empty string.</returns>
        public async Task<string> SendboxAuthenticate()
        {
            string accesstoken = "";
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, Sendbox_authenticate);
                request.Headers.Add("x-api-key", Sendbox_xapikey);
                request.Headers.Add("x-api-secret", Sendbox_xapisecret);
                var content = new StringContent("{}", null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                SendboxAuthenticate resp = JsonConvert.DeserializeObject<SendboxAuthenticate>(json);
                accesstoken = resp.access_token;

            }
            catch (Exception ex)
            {
            }
            return accesstoken;
        }

        /// <summary>
        /// Verifies a bank account number using the provided IFSC code and account details, and saves the account
        /// information if verification is successful.
        /// </summary>
        /// <remarks>This method performs an external verification of the provided bank account details and,
        /// upon successful verification, attempts to save the account information for the user. The method returns a
        /// JSON response suitable for use in AJAX scenarios. Ensure that all required parameters are provided and valid
        /// to avoid verification failure.</remarks>
        /// <param name="ifsc">The Indian Financial System Code (IFSC) of the bank branch to be used for account verification. Cannot be
        /// null or empty.</param>
        /// <param name="accountnumber">The bank account number to verify. Cannot be null or empty.</param>
        /// <param name="Actype">The type of the bank account (for example, 'Savings' or 'Current').</param>
        /// <param name="Bank">The code or identifier of the bank associated with the account.</param>
        /// <param name="BranchName">The name of the bank branch where the account is held.</param>
        /// <param name="Bankname">The full name of the bank associated with the account.</param>
        /// <returns>A JSON result indicating the outcome of the verification. Returns a status flag, a message describing the
        /// result, and, if successful, the verification data. If verification fails, the message provides the reason.</returns>
        [HttpPost]
        public async Task<ActionResult> AccountNumberVerify(string ifsc, string accountnumber,
            string Actype, string Bank, string BranchName, string Bankname)
        {
            string jsonres = "";
            try
            {
                //ifsc = "PUNB0051110";
                //accountnumber = "05112121021880";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string accesstoken = await SendboxAuthenticate();
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, Sendbox_accountverify.Replace("{ifsc}", ifsc).Replace("{account_number}", accountnumber));
                request.Headers.Add("Authorization", accesstoken);
                request.Headers.Add("x-accept-cache", "true");
                request.Headers.Add("x-api-key", Sendbox_xapikey);
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    response.EnsureSuccessStatusCode();
                    jsonres = await response.Content.ReadAsStringAsync();
                    PennyDropResponse result = JsonConvert.DeserializeObject<PennyDropResponse>(jsonres);
                    if (result != null && result.code == 200)
                    {
                        if (result.data != null && result.data.account_exists == true && result.data.message == "Bank Account details verified successfully.")
                        {
                            var requestData = new KycBankSaveRequest
                            {
                                islogin = "N",
                                reqtype = "kycbanksave",
                                userid = Convert.ToString(Session["IDNO"]),
                                passwd = Convert.ToString(Session["password"]),
                                accounttype = Actype,
                                accountno = accountnumber,
                                bankcode = Bank,
                                bankname = Bankname,
                                branchname = BranchName,
                                ifsccode = ifsc
                            };
                            string bankjsonData = JsonConvert.SerializeObject(requestData);
                            var bankresponse = CallPostFunction(bankjsonData, Apiurl);
                            var obj = JsonConvert.DeserializeObject<KycBankSaveResponse>(bankresponse);
                            if (obj.response == "OK")
                            {
                                return Json(new { Status = true, Message = "Account verified successfully", Data = jsonres }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json(new { Status = false, Message = obj.msg }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return Json(new { Status = false, Message = "Bank account does not exist" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new { Status = false, Message = "Account verification failed" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { Status = false, Message = "Invalid account details" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Something went wrong" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Submits and verifies address and identification details for KYC (Know Your Customer) processing, including
        /// uploading front and back images of the Aadhar card.
        /// </summary>
        /// <remarks>This method is intended for use in KYC workflows where address and identity
        /// verification are required. Uploaded files are saved to the server, and the verification request is forwarded
        /// to an external API. If an error occurs during processing, a generic failure message is returned. Ensure that
        /// all required fields are provided and that uploaded files meet any applicable size and format
        /// requirements.</remarks>
        /// <param name="Address">The residential address to be verified as part of the KYC process.</param>
        /// <param name="Pincode">The postal code associated with the provided address. Must be a valid pincode.</param>
        /// <param name="StateCode">The code representing the state of the address. Must correspond to a valid state code.</param>
        /// <param name="District">The name of the district for the provided address.</param>
        /// <param name="City">The name of the city for the provided address.</param>
        /// <param name="IdType">The type of identification document being submitted (for example, 'Aadhar').</param>
        /// <param name="Aadharno">The identification number from the Aadhar card or other specified ID document.</param>
        /// <param name="IFSCCode">The IFSC code of the bank branch, if required for the KYC process.</param>
        /// <param name="Aadharfront">The uploaded file containing the front image of the Aadhar card. Can be null if not provided.</param>
        /// <param name="Aadharback">The uploaded file containing the back image of the Aadhar card. Can be null if not provided.</param>
        /// <returns>A JSON result indicating whether the address verification was successful. The result contains a success flag
        /// and a message describing the outcome.</returns>
        [HttpPost]
        public JsonResult AddressVerify(
                        string Address, string Pincode, string StateCode, string District, string City,
                        string IdType, string Aadharno, string IFSCCode,
                        HttpPostedFileBase Aadharfront, HttpPostedFileBase Aadharback)
        {
            try
            {
                // Create upload directory if not exists
                string uploadPath = Server.MapPath("~/Uploads/KYC/");
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                string SaveFile(HttpPostedFileBase file)
                {
                    if (file == null) return null;
                    string filename = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    string path = Path.Combine(uploadPath, filename);
                    file.SaveAs(path);
                    return "/Uploads/KYC/" + filename;
                }

                // Save files
                string AadharFrontPath = SaveFile(Aadharfront);
                string AadharBackPath = SaveFile(Aadharback);

                // Save details to DB (example)
                // Assuming you have an Entity or Model class KycEntity
                KycAddressRequest requestData = new KycAddressRequest
                {
                    islogin = "N",
                    reqtype = "kycaddress",
                    userid = Convert.ToString(Session["IDNO"]),
                    passwd = Convert.ToString(Session["password"]),

                    address = Address,
                    citycode = "0",
                    cityname = City,
                    pincode = Pincode,
                    statecode = StateCode,

                    districtcode = "0",
                    district = District,

                    areaname = "",
                    areacode = "0",

                    idproofid = IdType,
                    idproofno = Aadharno,

                    frontaddressproof = SiteUrl + AadharFrontPath,
                    backaddressproof = SiteUrl + AadharBackPath,
                };
                var detail = JsonConvert.SerializeObject(requestData);
                var response = CallPostFunction(detail, Apiurl);
                var output = JsonConvert.DeserializeObject<KycCommonResponse>(response);
                if (output.response == "OK")
                {
                    return Json(new { success = true, message = output.msg });
                }
                return Json(new { success = false, message = output.msg });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Something Went Wrong" });
            }
        }

        /// <summary>
        /// Handles the upload of a KYC document file submitted via a user form and processes the file for storage and
        /// further handling.
        /// </summary>
        /// <remarks>The uploaded file is saved to the server and its details are sent to an external API
        /// for further processing. The method returns a JSON response suitable for AJAX form submissions.</remarks>
        /// <param name="kycdoc">The KYC document file uploaded by the user. Can be null if no file is provided.</param>
        /// <returns>A JSON result indicating whether the upload and processing were successful. The result contains a success
        /// flag and a message describing the outcome.</returns>
        [HttpPost]
        public JsonResult UserFormUpload(HttpPostedFileBase kycdoc)
        {
            try
            {
                // Create upload directory if not exists
                string uploadPath = Server.MapPath("~/Uploads/KYC/");
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                string SaveFile(HttpPostedFileBase file)
                {
                    if (file == null) return null;
                    string filename = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    string path = Path.Combine(uploadPath, filename);
                    file.SaveAs(path);
                    return "/Uploads/KYC/" + filename;
                }

                // Save files
                string kycdocpatch = SaveFile(kycdoc);

                // Save details to DB (example)
                // Assuming you have an Entity or Model class KycEntity

                FormUploadRequest requestData = new FormUploadRequest
                {
                    islogin = "N",
                    reqtype = "formupload",
                    userid = Convert.ToString(Session["IDNO"]),
                    passwd = Convert.ToString(Session["password"]),
                    formupload = SiteUrl + kycdocpatch
                };

                var detail = JsonConvert.SerializeObject(requestData);
                var response = CallPostFunction(detail, Apiurl);
                var output = JsonConvert.DeserializeObject<FormUploadResponse>(response);
                if (output.response == "OK")
                {
                    return Json(new { success = true, message = output.msg });
                }
                return Json(new { success = false, message = output.msg });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Something Went Wrong" });
            }
        }

        [HttpPost]
        public async Task<ActionResult> PanVerifyV2(string Panno, string Name, string DOB)
        {
            string jsonres = "";
            try
            {
                DateTime parsedDate;
                string formattedDate = "";
                // Try parsing with current culture + invariant culture
                bool isValid = DateTime.TryParse(
                    DOB,
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out parsedDate
                );
                formattedDate = parsedDate.ToString("dd/MM/yyyy");
                if (formattedDate.Contains("-"))
                {
                    formattedDate = formattedDate.Replace("-", "/");
                }
                PanVerifyreq reqobj = new PanVerifyreq();
                reqobj.Pan = Panno;
                reqobj.Pan = reqobj.Pan.Trim().ToUpper();  // normalize
                if (!Regex.IsMatch(reqobj.Pan, @"^[A-Z]{5}[0-9]{4}[A-Z]$"))
                {
                    return Json(new { Status = false, Message = "Invalid pan" }, JsonRequestBehavior.AllowGet);
                }
                reqobj.NameAsPerPan = Name;
                reqobj.DateOfBirth = formattedDate;
                string reqjsonn = JsonConvert.SerializeObject(reqobj);

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string accesstoken = await SendboxAuthenticate();
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, Sendbox_panverify);
                request.Headers.Add("Authorization", accesstoken);
                request.Headers.Add("x-accept-cache", "true");
                request.Headers.Add("x-api-key", Sendbox_xapikey);

                //var content = new StringContent("\n{\n  \"@entity\": \"in.co.sandbox.kyc.pan_verification.request\",\n  \"pan\": \"kqjps3219e\",\n  \"name_as_per_pan\": \"Ajay singh shaktawat\",\n  \"date_of_birth\": \"03/03/2000\",\n  \"consent\": \"Y\",\n  \"reason\": \"KYC onboarding\"\n}\n", null, "application/json");
                var content = new StringContent(reqjsonn, null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    response.EnsureSuccessStatusCode();
                    jsonres = await response.Content.ReadAsStringAsync();
                    PanVerificationApiResponse resobj = JsonConvert.DeserializeObject<PanVerificationApiResponse>(jsonres);
                    if (resobj.Data.Status == "valid")
                    {
                        if (resobj.Data.NameAsPerPanMatch != true)
                        {
                            return Json(new { Status = false, Message = "Enter name as per panmatch" }, JsonRequestBehavior.AllowGet);
                        }
                        if (resobj.Data.DateOfBirthMatch != true)
                        {
                            return Json(new { Status = false, Message = "Enter date Of birth match from pan" }, JsonRequestBehavior.AllowGet);
                        }

                        KycPanSaveRequest requestData = new KycPanSaveRequest
                        {
                            islogin = "N",
                            reqtype = "kycpancardsave",
                            userid = Convert.ToString(Session["IDNO"]),
                            passwd = Convert.ToString(Session["password"]),
                            panno = Panno
                        };
                        var detail = JsonConvert.SerializeObject(requestData);
                        var panresponse = CallPostFunction(detail, Apiurl);
                        var output = JsonConvert.DeserializeObject<KycPanSaveResponse>(panresponse);
                        if (output.response == "OK")
                        {
                            return Json(new { Status = true, Message = "Pan verified successfully" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { Status = true, Message = output.msg }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new { Status = false, Message = "Invalid pan" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { Status = false, Message = "Invalid pan" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Something went wrong" }, JsonRequestBehavior.AllowGet);
            }

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

        public ActionResult Forgotpass()
        {
            return View();
        }

        /// <summary>
        /// Initiates the password reset process for the specified user and returns a message indicating the result.
        /// </summary>
        /// <param name="Userid">The unique identifier of the user requesting a password reset. Cannot be null or empty.</param>
        /// <returns>A JSON result containing a message that indicates whether the password reset request was successful. If
        /// successful, the message states that the password has been sent to the registered email address; otherwise,
        /// it contains an error message.</returns>
        public ActionResult Forgotpassword(string Userid)
        {
            string msg = "";
            try
            {
                Forgotreq req = new Forgotreq();
                req.userid = Userid;
                req.reqtype = "forgot";
                req.islogin = "N";
                string detail = JsonConvert.SerializeObject(req);
                var response = CallPostFunction(detail, Apiurl);
                var obj = JsonConvert.DeserializeObject<Forgotres>(response);
                if (obj.response == "OK")
                {
                    msg = "Your password send on registered mail id.";
                }
                else
                {
                    msg = obj.msg;
                }
            }
            catch (Exception ex)
            {

            }
            return Json(new { msg });
        }

        public ActionResult DirectLoginGV()
        {
            string html = $@"
            <html>
            <body onload='document.forms[0].submit()'>
                <form method='POST' action='{GvPortalUrl}'>
                    <input type='hidden' name='token' value='1a027ace746dccad5151c31954e39be3' />
                    <input type='hidden' name='mod' value='interLogin' />
                    <input type='hidden' name='userid' value='{Session["IDNO"]}' />
                    <input type='hidden' name='password' value='{Session["Password"]}' />
                    <input type='hidden' name='getinfo' value='xyz' />
                </form>
            </body>
            </html>";


            //string apiUrl = GvPortalUrl;
            //string responseText = string.Empty;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            //try
            //{
            //    using (HttpClient client = new HttpClient())
            //    {
            //        // Optional timeout
            //        client.Timeout = TimeSpan.FromSeconds(30);

            //        // Create request
            //        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, apiUrl);

            //        // Add Cookie header
            //        request.Headers.Add("Cookie", "PHPSESSID=57456045d4f68e65fb1de321c876c94c");

            //        // Multipart form data
            //        MultipartFormDataContent content = new MultipartFormDataContent();
            //        content.Add(new StringContent("1a027ace746dccad5151c31954e39be3"), "token");
            //        content.Add(new StringContent("interLogin"), "mod");
            //        content.Add(new StringContent(Convert.ToString(Session["IDNO"])), "userid");
            //        content.Add(new StringContent(Convert.ToString(Session["Password"])), "password");

            //        request.Content = content;

            //        // Synchronous call (NO async/await)
            //        HttpResponseMessage response = client.SendAsync(request).Result;

            //        if (response.IsSuccessStatusCode)
            //        {
            //            responseText = response.Content.ReadAsStringAsync().Result;
            //        }
            //        else
            //        {
            //            responseText = "Error: " + response.StatusCode;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    responseText = ex.Message;
            //}
            // Pass response to View
            //ViewBag.ApiResponse = responseText;
            //return View();
            return Content(html, "text/html");
        }

        public ActionResult DirectLoginStore()
        {
            string html = $@"
    <html>
    <body onload='document.forms[0].submit()'>
        <form method='POST' action='{StorePortalUrl}'>
            <input type='hidden' name='token' value='453ecd0dca082bc94cac8d06406305f1' />
            <input type='hidden' name='mod' value='interLogin' />
            <input type='hidden' name='userid' value='{Session["IDNO"]}' />
            <input type='hidden' name='password' value='{Session["Password"]}' />
            <input type='hidden' name='getinfo' value='xyz' />
        </form>
    </body>
    </html>";

            return Content(html, "text/html");
        }

        public ActionResult Downloadpdf()
        {
            string filePath = Server.MapPath("~/SiteDoc/DIRECTSELLERCONTRACT.pdf");

            if (!System.IO.File.Exists(filePath))
            {
                return HttpNotFound("File not found");
            }

            // New name for download
            string downloadFileName = Convert.ToString(Session["IDNO"]) + "Direct_Seller_Agreement.pdf";

            return File(filePath, "application/pdf", downloadFileName);
        }

        /// <summary>
        /// Submits and saves Know Your Customer (KYC) information, including personal details, identification numbers,
        /// and supporting document files, for the current user.
        /// </summary>
        /// <remarks>This method is intended to be called via HTTP POST and requires the user to be
        /// authenticated. Uploaded files are saved to the server, and the KYC details are submitted for processing. If
        /// any required information is missing or an error occurs during processing, the response will indicate
        /// failure.</remarks>
        /// <param name="Address">The residential address of the user to be saved as part of the KYC information.</param>
        /// <param name="Pincode">The postal code associated with the user's address. Must be a valid pincode.</param>
        /// <param name="StateCode">The code representing the user's state of residence.</param>
        /// <param name="District">The name of the district corresponding to the user's address.</param>
        /// <param name="City">The name of the city corresponding to the user's address.</param>
        /// <param name="IdType">The type of identification document provided (for example, 'Aadhar', 'Passport', etc.).</param>
        /// <param name="Aadharno">The identification number associated with the provided ID type (for example, the Aadhar number).</param>
        /// <param name="Actype">The type of bank account held by the user (for example, 'Savings', 'Current').</param>
        /// <param name="Acno">The user's bank account number.</param>
        /// <param name="Bankname">The name of the user's bank.</param>
        /// <param name="Bank">The code or identifier for the user's bank.</param>
        /// <param name="BranchName">The name of the bank branch where the account is held.</param>
        /// <param name="IFSCCode">The IFSC code of the user's bank branch.</param>
        /// <param name="PanNo">The user's Permanent Account Number (PAN) for tax identification.</param>
        /// <param name="Aadharfront">The uploaded file containing the front image of the user's Aadhar or identification document. Can be null if
        /// not provided.</param>
        /// <param name="Aadharback">The uploaded file containing the back image of the user's Aadhar or identification document. Can be null if
        /// not provided.</param>
        /// <param name="Bankdoc">The uploaded file containing the user's bank document (such as a passbook or statement). Can be null if not
        /// provided.</param>
        /// <param name="Pandoc">The uploaded file containing the user's PAN card document. Can be null if not provided.</param>
        /// <param name="kycdoc">The uploaded file containing any additional KYC form or supporting document. Can be null if not provided.</param>
        /// <returns>A JSON result indicating whether the KYC information was saved successfully. The result contains a success
        /// flag and a message describing the outcome.</returns>

        [HttpPost]
        public JsonResult SaveKyc(
                                string Address, string Pincode, string StateCode, string District, string City,
                                string IdType, string Aadharno, string Actype, string Acno, string Bankname, string Bank, string BranchName,
                                string IFSCCode, string PanNo,
                                HttpPostedFileBase Aadharfront, HttpPostedFileBase Aadharback,
                                HttpPostedFileBase Bankdoc, HttpPostedFileBase Pandoc, HttpPostedFileBase kycdoc)
        {
            try
            {
                // Create upload directory if not exists
                string uploadPath = Server.MapPath("~/Uploads/KYC/");
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                string SaveFile(HttpPostedFileBase file)
                {
                    if (file == null) return null;
                    string filename = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    string path = Path.Combine(uploadPath, filename);
                    file.SaveAs(path);
                    return "/Uploads/KYC/" + filename;
                }

                // Save files
                string AadharFrontPath = SaveFile(Aadharfront);
                string AadharBackPath = SaveFile(Aadharback);
                string BankDocPath = SaveFile(Bankdoc);
                string PanDocPath = SaveFile(Pandoc);
                string kycdocpatch = SaveFile(kycdoc);

                // Save details to DB (example)
                // Assuming you have an Entity or Model class KycEntity
                var kyc = new KycSavereq
                {
                    islogin = "N",
                    reqtype = "saveallkyc",
                    userid = Convert.ToString(Session["IDNO"]),
                    passwd = Convert.ToString(Session["password"]),
                    address = Address,
                    pincode = Pincode,
                    statecode = StateCode,
                    districtcode = "0",
                    district = District,
                    cityname = City,
                    citycode = "0",
                    idproofid = IdType,
                    idproofno = Aadharno,
                    frontaddressproof = SiteUrl + AadharFrontPath,
                    backaddressproof = SiteUrl + AadharBackPath,
                    accounttype = Actype,
                    accountno = Acno,
                    bankcode = Bank,
                    bankname = Bankname,
                    branchname = BranchName,
                    ifsccode = IFSCCode,
                    bankimage = SiteUrl + BankDocPath,
                    panno = PanNo,
                    panimage = SiteUrl + PanDocPath,
                    areaname = "",
                    areacode = "0",
                    formupload = SiteUrl + kycdocpatch,
                };
                var detail = JsonConvert.SerializeObject(kyc);
                var response = CallPostFunction(detail, Apiurl);
                var output = JsonConvert.DeserializeObject<KycSaveres>(response);
                if (output.response == "OK")
                {
                    return Json(new { success = true, message = output.msg });
                }
                return Json(new { success = false, message = output.msg });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Something Went Wrong" });
            }
        }
    }
}