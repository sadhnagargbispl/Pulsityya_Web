using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.RegularExpressions;
using VitaFlow.Application.IServices;
using VitaFlow.Domain.Entities;
using VitaFlow.Domain.Interface;
using VitaFlow.Presenation.Extensions;

namespace VitaFlow.Presenation.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly I_Login i_Login_Service;
        private IHttpContextAccessor _httpContextAccessor;
        public AccountController(ILogger<AccountController> logger, I_Login iLoginService, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            i_Login_Service = iLoginService;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User userdata = new User();
                    userdata = await i_Login_Service.ValidateUser(obj);
                    if (userdata != null)
                    {
                        if (userdata.UserName != null)
                        {
                            HttpContext.Session.SetString("UserId", Convert.ToString(userdata.UserId));
                            HttpContext.Session.SetString("UserName", Convert.ToString(userdata.UserName));
                            HttpContext.Session.SetString("password", Convert.ToString(userdata.Password));
                            //HttpContext.Session.SetString("Name", Convert.ToString(userdata.Name));
                            HttpContext.Session.SetString("BranchCode", Convert.ToString(userdata.BranchCode));
                            HttpContext.Session.SetString("PartyCode", Convert.ToString(userdata.PartyCode));
                            HttpContext.Session.SetString("PartyName", Convert.ToString(userdata.PartyName));
                            HttpContext.Session.SetString("FCode", Convert.ToString(userdata.FCode));
                            HttpContext.Session.SetString("PartyId", Convert.ToString(userdata.PartyId));
                            HttpContext.Session.SetString("GroupId", Convert.ToString(userdata.GroupId));
                            HttpContext.Session.SetString("StateCode", Convert.ToString(userdata.StateCode));
                            HttpContext.Session.SetString("StateName", Convert.ToString(userdata.StateName));
                            HttpContext.Session.SetString("CityName", Convert.ToString(userdata.CityName));
                            HttpContext.Session.SetString("ParentPartyCode", Convert.ToString(userdata.ParentPartyCode));
                            HttpContext.Session.SetString("Address1", Convert.ToString(userdata.Address1));
                            HttpContext.Session.SetString("PinCode", Convert.ToString(userdata.PinCode));
                            HttpContext.Session.SetString("MobileNo", Convert.ToString(userdata.MobileNo));
                            HttpContext.Session.SetString("E_MailAdd", Convert.ToString(userdata.E_MailAdd));
                            HttpContext.Session.SetString("GroupPrefix", Convert.ToString(userdata.GroupPrefix));
                            HttpContext.Session.SetString("Status", "OK");
                            HttpContext.Session.SetString("IsSoldByHo", "false");
                            HttpContext.Session.SetString("LoginUserType", "shoppe");
                            if (userdata.GroupId == 0)
                            {
                                HttpContext.Session.SetString("IsSoldByHo", "true");
                            }
                            HttpContext.Session.SetComplexData("LoginUser", userdata);
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ViewBag.loginerr = "Please provide valid username and password";
                        }
                    }
                    else
                    {
                        ViewBag.loginerr = "Please provide valid username and password";
                    }
                }
                else
                {
                    return View(obj);
                }
            }
            catch (Exception ex)
            {
                ViewBag.loginerr = "Something went wrong";
            }
            return View(obj);
        }

        public async Task<ActionResult> Registration()
        {
            M_Registration obj = new M_Registration();
            try
            {
                obj.GroupList = await i_Login_Service.GetGroupList();
                obj.Statelist = await i_Login_Service.GetstateList();
            }
            catch (Exception ex)
            {

            }
            return View(obj);
        }
        [HttpGet]
        public async Task<IActionResult> GetParentPartyList(int GroupId, int stateCode, string District)
        {
            List<PartyModel> objParentPartyList = new List<PartyModel>();
            objParentPartyList = await i_Login_Service.GetParentPartyList(GroupId, stateCode, District);
            return Json(objParentPartyList);
        }
        [HttpPost]
        public async Task<ActionResult> GetParentParty(string PartyCode, string Groupid)
        {
            string msg = string.Empty;
            string err = string.Empty;
            string Partyname = string.Empty;
            try
            {
                PartyModel party = await i_Login_Service.GetParentParty(PartyCode, Groupid);
                if (party != null)
                {
                    if (party.PartyName != null)
                    {
                        if (Convert.ToInt32(party.GroupId) < Convert.ToInt32(Groupid))
                        {
                            err = "0";
                            Partyname = party.PartyName;
                        }
                        else if (Convert.ToInt32(party.GroupId) == Convert.ToInt32(Groupid))
                        {
                            err = "1";
                            msg = "You do not create same level franshise";
                        }
                        else if (Convert.ToInt32(party.GroupId) > Convert.ToInt32(Groupid))
                        {
                            err = "1";
                            msg = "You can not choose lower level franshise";
                        }
                    }
                    else
                    {
                        err = "1";
                        msg = "Please enter valid party code";
                    }

                }
                else
                {
                    err = "1";
                    msg = "Please enter valid party code";
                }
            }
            catch (Exception ex)
            {
                err = "1";
                msg = "Something went wrong";
            }
            return Json(new { msg, err, Partyname });
        }
        public async Task<IActionResult> GetPincodeDetail(string Pincode)
        {
            List<M_PincodeDetail> m_PincodeDetail = new List<M_PincodeDetail>();
            string State = string.Empty;
            string District = string.Empty;
            string Code = "101";
            try
            {
                string response = string.Empty;
                response = await i_Login_Service.GetPincode(Pincode);
                if (!string.IsNullOrEmpty(response))
                {
                    m_PincodeDetail = JsonConvert.DeserializeObject<List<M_PincodeDetail>>(response);
                    if (m_PincodeDetail[0].Status.ToUpper() == "SUCCESS")
                    {
                        State = m_PincodeDetail[0].PostOffice[0].State;
                        District = m_PincodeDetail[0].PostOffice[0].District;
                        Code = "200";
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Json(new { Code, State, District });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Registration(M_Registration obj)
        {
            try
            {
                obj.GroupList = await i_Login_Service.GetGroupList();
                obj.Statelist = await i_Login_Service.GetstateList();
                if (obj.GroupId == 0)
                {
                    ViewBag.groupiderr = "Please Select Franshise";
                    return View(obj);
                }
                if (obj.StateCode == 0)
                {
                    ViewBag.stateerr = "Please Select State";
                    return View(obj);
                }
                if (obj.GST == null)
                {
                    obj.GST = "";
                }
                if (obj.GroupId != 4 && obj.GroupId != 6)
                {
                    if (obj.GroupId == 1)
                    {
                        List<PartyModel> existparty = await i_Login_Service.CheckExistsGroupid(Convert.ToInt32(obj.GroupId), Convert.ToInt32(obj.StateCode), obj.City, obj.PinCode);
                        if (existparty.Count > 0)
                        {
                            ViewBag.groupiderr = "Oops! A franchise already exists in this state, so you can’t make another one.";
                            return View(obj);
                        }
                    }
                    else if (obj.GroupId == 2)
                    {
                        List<PartyModel> existparty = await i_Login_Service.CheckExistsGroupid(Convert.ToInt32(obj.GroupId), Convert.ToInt32(obj.StateCode), obj.City, obj.PinCode);
                        if (existparty.Count > 0)
                        {
                            ViewBag.groupiderr = "Oops! A franchise already exists in this district, so you can’t make another one.";
                            return View(obj);
                        }
                    }
                    else if (obj.GroupId == 3)
                    {
                        List<PartyModel> existparty = await i_Login_Service.CheckExistsGroupid(Convert.ToInt32(obj.GroupId), Convert.ToInt32(obj.StateCode), obj.City, obj.PinCode);
                        if (existparty.Count > 0)
                        {
                            ViewBag.groupiderr = "Oops! A franchise already exists in this pincode, so you can’t make another one.";
                            return View(obj);
                        }
                    }
                }

                ModelState.Remove("GroupList");
                ModelState.Remove("Statelist");
                ModelState.Remove("GST");
                ModelState.Remove("ParentPartyCode");
                ModelState.Remove("ParentpartyName");
                if (ModelState.IsValid)
                {
                    PartyModel party = await i_Login_Service.GetParentParty(obj.PartyCode, Convert.ToString(obj.GroupId));
                    if (party != null)
                    {
                        if (party.PartyName != null)
                        {
                            if (Convert.ToInt32(party.GroupId) < Convert.ToInt32(obj.GroupId))
                            {
                                var PartyCode = await i_Login_Service.GetPartyCode(obj.PartyCode.ToUpper(), Convert.ToString(obj.GroupId));
                                PartyModel Pobj = new PartyModel();
                                Pobj.GroupId = obj.GroupId;
                                Pobj.PGroupId = party.GroupId;
                                Pobj.PartyCode = PartyCode;
                                Pobj.PartyName = obj.Name;
                                Pobj.ParentPartyCode = party.PartyCode;
                                Pobj.Address1 = obj.Address;
                                Pobj.StateCode = obj.StateCode;
                                Pobj.CityCode = 0;
                                Pobj.CityName = obj.City;
                                Pobj.PinCode = Convert.ToDecimal(obj.PinCode);
                                Pobj.PhoneNo = obj.MobileNo;
                                Pobj.MobileNo = Convert.ToDecimal(obj.MobileNo);
                                Pobj.CstNo = obj.GST;
                                Pobj.ContactPerson = obj.Name;
                                Pobj.ActiveStatus = "Y";
                                Pobj.OnWebsite = "Y";
                                Pobj.CreditLimit = 0;
                                Pobj.RecTimeStamp = DateTime.Now;
                                Pobj.Password = obj.Password;
                                Pobj.CstNo = obj.GST;
                                int invuser = await i_Login_Service.SavePartyDetails(Pobj);
                                if (invuser > 0)
                                {
                                    HttpContext.Session.SetString("Upassword", Convert.ToString(obj.Password));
                                    HttpContext.Session.SetString("user", Convert.ToString(PartyCode));
                                    return RedirectToAction("Thankyou", "Account");
                                }
                                else
                                {
                                    ViewBag.Rerr = "You registration is not successful";
                                    return View(obj);
                                }

                            }
                            else
                            {
                                ViewBag.partyerr = "You do not create same level franshise";
                                return View(obj);
                            }
                        }
                        else
                        {
                            ViewBag.partyerr = "Please enter valid party code";
                            return View(obj);
                        }

                    }
                    else
                    {
                        ViewBag.partyerr = "Please enter valid party code";
                        return View(obj);
                    }
                }
                else
                {
                    return View(obj);
                }
            }
            catch
            {

            }
            return View();
        }

        public ActionResult Thankyou()
        {
            return View();
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }



    }
}
