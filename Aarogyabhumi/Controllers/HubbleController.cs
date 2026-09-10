using System;
using System.Configuration;
using System.Web.Mvc;
using Newtonsoft.Json;
using Shopinv.Interface;
using Shopinv.Repository;
using System.EnterpriseServices;
using System.Net.Sockets;
using System.Threading.Tasks;
using Shopinv.Models;

namespace Shopinv.Controllers
{
    public class HubbleController : Controller
    {
        private readonly IHubbleSSORepository _ssoRepo;

        public HubbleController()
        {
            _ssoRepo = new HubbleSSORepository();
        }

        // ✅ User yahan se aata hai
        public ActionResult Vouchers()
        {
            if (Session["IDNO"] == null)
                return RedirectToAction("Index", "Home");

            string userId = Session["IDNO"].ToString();

            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Index", "Home");

            string clientId = ConfigurationManager.AppSettings["HubbleClientId"];
            string appSecret = ConfigurationManager.AppSettings["HubbleAppSecret"];
            string token = GenerateSSOToken(userId);

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Index", "Home");
            string sdkUrl = "https://vouchers.myhubble.money/sdk/gc/" +
                            "?clientId=" + clientId +
                            "&appSecret=" + appSecret +
                            "&token=" + token;

            return Redirect(sdkUrl);
        }

        // ✅ Token generate + DB save
        private string GenerateSSOToken(string userId)
        {
            string token = Guid.NewGuid().ToString();
            _ssoRepo.SaveToken(token, userId);
            return token;
        }

        // ✅ Hubble backend call karega
        [AcceptVerbs(HttpVerbs.Post | HttpVerbs.Get)]
        public ActionResult Sso()
        {
            try
            {
                // GET — endpoint live check
                if (Request.HttpMethod == "GET")
                {
                    return Json(new { status = "SSO Endpoint is Live" },
                                JsonRequestBehavior.AllowGet);
                }

                // Step 1: Secret validate karo
                string expectedSecret = ConfigurationManager
                                            .AppSettings["HubbleSecret"];
                string incomingSecret = Request.Headers["X-Hubble-Secret"];

                if (string.IsNullOrEmpty(incomingSecret) ||
                    incomingSecret != expectedSecret)
                {
                    Response.StatusCode = 401;
                    return Json(new { message = "Invalid Hubble secret." },
                                JsonRequestBehavior.AllowGet);
                }

                // Step 2: Body se token padhho
                string body;
                Request.InputStream.Position = 0;
                using (var reader = new System.IO.StreamReader(Request.InputStream))
                {
                    body = reader.ReadToEnd();
                }

                if (string.IsNullOrEmpty(body))
                {
                    Response.StatusCode = 400;
                    return Json(new { userId = (string)null },
                                JsonRequestBehavior.AllowGet);
                }

                dynamic requestData = JsonConvert.DeserializeObject(body);
                string token = requestData?.token;

                if (string.IsNullOrEmpty(token))
                {
                    Response.StatusCode = 400;
                    return Json(new { userId = (string)null },
                                JsonRequestBehavior.AllowGet);
                }

                // Step 3: User nikalo
                var user = _ssoRepo.GetUserByToken(token);

                //if (user == null)
                //{
                //    Response.StatusCode = 401;
                //    return Json(new { userId = (string)null },
                //                JsonRequestBehavior.AllowGet);
                //}
                if (user == null)
                {
                    // ✅ Redirect to Home/Index
                    return Json(new
                    {
                        userId = (string)null,
                        email = (string)null,
                        firstName = (string)null,
                        lastName = (string)null,
                        phoneNumber = (string)null,
                        cohorts = (string)null,
                        redirectUrl = Url.Action("Index", "Home", null, Request.Url.Scheme)
                    }, JsonRequestBehavior.AllowGet);
                }
                // Step 4: Purane tokens cleanup
                _ssoRepo.MarkTokenUsed(token);

                // Step 5: Response
                Response.StatusCode = 200;
                return Json(new
                {
                    userId = user.UserId,
                    email = user.Email,
                    firstName = user.FirstName,
                    lastName = user.LastName,
                    phoneNumber = user.PhoneNumber,
                    cohorts = user.Cohorts,
                    // ✅ Fix — return as array
                    //cohorts = string.IsNullOrEmpty(user.Cohorts) ? new string[0] : new string[] { user.Cohorts }
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { message = "Server Error: " + ex.Message },
                            JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs(HttpVerbs.Post | HttpVerbs.Get)]
        public ActionResult Balance(string userId)
        {
            string expectedSecret =
                ConfigurationManager.AppSettings["HubbleSecret"];

            string incomingSecret =
                Request.Headers["X-Hubble-Secret"];

            if (incomingSecret != expectedSecret)
            {
                Response.StatusCode = 401;

                return Json(new
                {
                    message = "Invalid Secret"
                },
                JsonRequestBehavior.AllowGet);
            }

            var user = _ssoRepo.GetUserByUserId(userId);

            if (user == null)
            {
                Response.StatusCode = 404;

                return Json(new
                {
                    message = "User not found"
                },
                JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                userId = user.UserId,
                totalCoins = user.CoinBalance
            },
            JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post | HttpVerbs.Get)]
        public ActionResult Debit(CoinsDebitRequest request)
        {
            string expectedSecret =
                ConfigurationManager.AppSettings["HubbleSecret"];

            string incomingSecret = Request.Headers["X-Hubble-Secret"];

            if (incomingSecret != expectedSecret)
            {
                Response.StatusCode = 401;

                return Json(new
                {
                    status = "FAILED"
                });
            }
            var result = _ssoRepo.DebitCoins(
    request.userId,
    request.coins,
    request.referenceId,
    request.note
);

            if (result == null || result.Rows.Count == 0)
            {
                return Json(new
                {
                    status = "FAILED",
                    message = "No data returned"
                }, JsonRequestBehavior.AllowGet);
            }

            var row = result.Rows[0];

            return Json(new
            {
                status = row["Status"].ToString(),
                transactionId = row["TransactionId"].ToString(),
                balance = Convert.ToDecimal(row["Balance"]),
                referenceId = row["ReferenceId"].ToString()
            },
            JsonRequestBehavior.AllowGet);
            //var result = _ssoRepo.DebitCoins(
            //       request.userId,
            //       request.coins,
            //       request.referenceId,
            //       request.note);

            //        return Json(new
            //        {
            //            status = result.Status,
            //            transactionId = result.TransactionId,
            //            balance = result.Balance,
            //            referenceId = result.ReferenceId
            //        },
            //JsonRequestBehavior.AllowGet);
            //return Json(result, JsonRequestBehavior.AllowGet);
            //return Json(result);
        }
        [AcceptVerbs(HttpVerbs.Post | HttpVerbs.Get)]
        public ActionResult Reverse(CoinsReverseRequest request)
        {
            string expectedSecret =
                ConfigurationManager.AppSettings["HubbleSecret"];

            string incomingSecret =
                Request.Headers["X-Hubble-Secret"];

            if (incomingSecret != expectedSecret)
            {
                Response.StatusCode = 401;

                return Json(new
                {
                    status = "FAILED"
                }, JsonRequestBehavior.AllowGet);
            }

            var result = _ssoRepo.ReverseCoins(
                request.userId,
                request.referenceId,
                request.note
            );

            if (result == null || result.Rows.Count == 0)
            {
                return Json(new
                {
                    status = "FAILED",
                    message = "No data returned"
                }, JsonRequestBehavior.AllowGet);
            }

            var row = result.Rows[0];

            return Json(new
            {
                status = row["Status"].ToString(),
                transactionId = row["TransactionId"].ToString(),
                balance = row["Balance"] == DBNull.Value
                            ? 0
                            : Convert.ToDecimal(row["Balance"]),
                referenceId = row["ReferenceId"].ToString()
            },
            JsonRequestBehavior.AllowGet);
        }
        //[AcceptVerbs(HttpVerbs.Post | HttpVerbs.Get)]
        //public ActionResult Reverse(CoinsReverseRequest request)
        //{
        //    string expectedSecret =
        //        ConfigurationManager.AppSettings["HubbleSecret"];

        //    string incomingSecret =
        //        Request.Headers["X-Hubble-Secret"];

        //    if (incomingSecret != expectedSecret)
        //    {
        //        Response.StatusCode = 401;

        //        return Json(new
        //        {
        //            status = "FAILED"
        //        });
        //    }

        //    var result =
        //        _ssoRepo.ReverseCoins(
        //            request.userId,
        //            request.referenceId,
        //            request.note);

        //    return Json(result);
        //}
    }
}