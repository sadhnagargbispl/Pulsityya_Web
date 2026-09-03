using Newtonsoft.Json;
using Shopinv.Entity;
using Shopinv.Interface;
using Shopinv.Models;
using Shopinv.SiteExtension;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Shopinv.Controllers
{
    //[KycRequired]
    public class ProductDetailController : Controller
    {
        private readonly I_Category icateogry = null;
        private readonly I_Product iprod = null;
        CompanyDetail companyDetail;
        private readonly static string Apiurl = ConfigurationManager.AppSettings["ApiUrl"];
        public ProductDetailController(I_Category icateogry, I_Product iprod)
        {
            this.icateogry = icateogry;
            this.iprod = iprod;
            companyDetail = new CompanyDetail(this.iprod);
            companyDetail.GetCompanydetail();
        }

        // GET: ProductDetail
        public ActionResult ProductDetail(M_Category objg, string ProdId)
        {
            // opened without a product id (or with a bogus one) - the stored
            // procedures return no result sets and Tables[0] would throw
            if (string.IsNullOrWhiteSpace(ProdId))
            {
                return RedirectToAction("CategoryList", "CategoryList");
            }

            Session["Isredirect"] = null;
            Session["ProdId"] = ProdId;
            List<E_Product> AsloAvailableProd = new List<E_Product>();
            List<E_SizeMaster> e_SizeMasters = new List<E_SizeMaster>();
            List<E_GetColor> e_GetColors = new List<E_GetColor>();

            objg.ProductDetail = iprod.ProductDetail(ProdId);
            DataSet dsprod1 = iprod.GetProdAvailable(ProdId);
            DataSet colorsize = iprod.Get_ColorSizeimgaeBYid("byprodid", ProdId, "0", "");

            if (dsprod1 != null && dsprod1.Tables.Count > 0 && dsprod1.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr1 in dsprod1.Tables[0].Rows)
                {
                    E_Product obj1 = new E_Product()
                    {
                        ProductName = dr1["ProductName"].ToString(),
                        ProdId = dr1["ProdId"].ToString(),
                        BV = (decimal)dr1["BV"],
                        PV = (int)dr1["PV"],
                        Discount = (decimal)dr1["Discount"],
                        ImagePath = SiteExtension.MediaUrl.Rehost(dr1["ImagePath"].ToString()),
                        Price = (decimal)dr1["Price"],
                        MRP = (decimal)dr1["MRP"],
                        BunchQty = (decimal)dr1["BunchQty"],
                        Weight = (decimal)dr1["Weight"],
                        Gst = (decimal)dr1["Gst"],
                        StockQTY = (decimal)dr1["StockQTY"]
                    };
                    AsloAvailableProd.Add(obj1);
                }
            }
            if (colorsize != null && colorsize.Tables.Count > 0 && colorsize.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr1 in colorsize.Tables[0].Rows)
                {
                    E_GetColor prodColor = new E_GetColor()
                    {
                        Id = Convert.ToDecimal(dr1["id"]),
                        ColorName = Convert.ToString(dr1["ColorName"])
                    };
                    e_GetColors.Add(prodColor);
                }
            }
            //------------------------size---------------------------
            if (colorsize != null && colorsize.Tables.Count > 1 && colorsize.Tables[1].Rows.Count > 0)
            {
                foreach (DataRow dr1 in colorsize.Tables[1].Rows)
                {
                    E_SizeMaster ss = new E_SizeMaster()
                    {
                        Id = Convert.ToDecimal(dr1["id"]),
                        Size = Convert.ToString(dr1["size"]),
                    };
                    e_SizeMasters.Add(ss);
                }
            }
            objg.GetColors = e_GetColors;
            objg.GetSizes = e_SizeMasters;
            objg.TopSellerProduct = iprod.GetTopSellerProduct();
            objg.RelatedProduct = AsloAvailableProd;
          
            if (Session["UserDetail"] != null)
            {
                Getkycreq req = new Getkycreq();
                req.islogin = "N";
                req.reqtype = "getkyc";
                req.userid = Convert.ToString(Session["IDNO"]);
                req.passwd = Convert.ToString(Session["password"]);
                string jsonreq = JsonConvert.SerializeObject(req);
                var response = CallPostFunction(jsonreq, Apiurl);
                Getkycres kycresponse = JsonConvert.DeserializeObject<Getkycres>(response);
                ViewBag.kycstatus = kycresponse.idverf;
            }
            return View(objg);
        }
        public ActionResult CheckLogin()
        {
            try
            {
                Session["Isredirect"] = "Y";
                string IsLoggedIn = string.Empty;
                IsLoggedIn = "No";
                if (Session["UserId"] != null && Session["Refid"] == null)
                {
                    IsLoggedIn = "Yes";
                }
                else if (Session["UserId"] == null && Session["Refid"] != null)
                {
                    IsLoggedIn = "No";
                }
                return Json(IsLoggedIn, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult GetImageBycolor(M_Category objg, string colorId, string Colorname)
        {
            DataSet ds = iprod.Get_ColorSizeimgaeBYid("bycolorid", Convert.ToString(Session["ProdId"]), colorId, Colorname);
            List<E_ProductDetail> lst = new List<E_ProductDetail>();
            var firstimg = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    E_ProductDetail dd = new E_ProductDetail();


                    if (Convert.ToString(item["Imagepath"]) != "")
                    {
                        dd.ImagePath = SiteExtension.MediaUrl.Rehost(Convert.ToString(item["Imagepath"]));

                    }
                    if (Convert.ToString(item["Imagepath1"]) != "")
                    {
                        dd.ImagePath1 = SiteExtension.MediaUrl.Rehost(Convert.ToString(item["Imagepath1"]));
                    }
                    if (Convert.ToString(item["Imagepath2"]) != "")
                    {
                        dd.ImagePath2 = SiteExtension.MediaUrl.Rehost(Convert.ToString(item["Imagepath2"]));
                    }
                    if (Convert.ToString(item["Imagepath3"]) != "")
                    {
                        dd.ImagePath3 = SiteExtension.MediaUrl.Rehost(Convert.ToString(item["Imagepath3"]));
                    }
                    if (Convert.ToString(item["Imagepath4"]) != "")
                    {
                        dd.ImagePath4 = SiteExtension.MediaUrl.Rehost(Convert.ToString(item["Imagepath4"]));
                    }
                    if (Convert.ToString(item["Imagepath5"]) != "")
                    {
                        dd.ImagePath5 = SiteExtension.MediaUrl.Rehost(Convert.ToString(item["Imagepath5"]));
                    }
                    lst.Add(dd);
                    break;
                }
                objg.ProductDetail = lst;
                firstimg = Convert.ToString(ds.Tables[0].Rows[0]["Imagepath"]);
            }
            List<E_SizeMaster> e_SizeMasters = new List<E_SizeMaster>();
            if (ds.Tables[1].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    E_SizeMaster ee = new E_SizeMaster
                    {
                        Id = Convert.ToDecimal(dr["Id"]),
                        Size = Convert.ToString(dr["Size"])
                    };
                    e_SizeMasters.Add(ee);
                }
            }
            var tblOrder = Extension.RenderRazorViewToString(this.ControllerContext, "_Imagedetail", objg);
            var count = lst.Count();
            return Json(new { tblOrder, e_SizeMasters, count, firstimg });
            //return PartialView("_Imagedetail", objg);
        }

        public ActionResult AddProductInToCart(M_Category objg, string Action, string ProductCode, string Qty, string ProdName, string Image, string Price, string Bv, string UniqId, string PV, string Weight, string Color, string SIZE)
        {
            string hostName = Dns.GetHostName();
            int? count = 0;
            string Message = string.Empty;
            string myIP = Dns.GetHostEntry(hostName).AddressList[0].ToString();
            var userid = Session["UserId"];
            var Sessionid = Session["CurrentUserSessionID"]; /*Session.SessionID*/
            List<E_ProductDetail> objprod = new List<E_ProductDetail>();
            string save = iprod.SaveProd(Action, ProductCode, ProdName, Image, Price, Bv, Qty, myIP, (Convert.ToString(Sessionid)), Convert.ToString(userid), PV, Weight, Color, SIZE);

            objg.CartDetail = iprod.Cartdetailsftch(Convert.ToString(userid));//Convert.ToString(Sessionid),

            Session["Cartdetailsftch"] = objg.CartDetail;
            var cartCount = objg.CartDetail != null ? objg.CartDetail.Count() : 0;
            var TotPrice = objg.CartDetail != null ? objg.CartDetail.Sum(s => s.Price * s.qty).ToString() : "0";
            var cartprodCount = objg.CartDetail != null ? objg.CartDetail.Where(p=>p.ProdId== ProductCode).Count() : 0;
            return Json(new { save, TotPrice, cartCount, cartprodCount });
        }

        public ActionResult AddProductInCartOuter(M_Category objg, string Action, string ProductCode, string Qty, string ProdName, string Image, string Price, string Bv, string UniqId, string PV, string Weight, string Color, string SIZE)
        {
            string hostName = Dns.GetHostName();
            int? count = 0;
            string Message = string.Empty;
            string myIP = Dns.GetHostEntry(hostName).AddressList[0].ToString();
            var userid = Session["UserId"];
            var Sessionid = Session["CurrentUserSessionID"]; /*Session.SessionID*/
            List<E_ProductDetail> objprod = new List<E_ProductDetail>();
            string save = iprod.SaveProdouter(Action, ProductCode, ProdName, Image, Price, Bv, Qty, myIP, (Convert.ToString(Sessionid)), Convert.ToString(userid), PV, Weight, Color, SIZE);

            objg.CartDetail = iprod.Cartdetailsftchouter(Convert.ToString(Sessionid));//Convert.ToString(Sessionid),

            Session["Cartdetailsftch"] = objg.CartDetail;
            var cartCount = objg.CartDetail != null ? objg.CartDetail.Count() : 0;
            var TotPrice = objg.CartDetail != null ? objg.CartDetail.Sum(s => s.Price * s.qty).ToString() : "0";

            return Json(new { save, TotPrice, cartCount });
        }

        public ActionResult SaveReview(string ReviewMessage, string ReviewRating, string ReviewName, string Productcode)
        {
            string msg = string.Empty;
            string status = "0";
            string tblOrder = "";
            try
            {
                int FormNo = Convert.ToInt32(Session["FormNo"]);
                DataSet ds = iprod.SaveReview(ReviewMessage, ReviewRating, ReviewName, Productcode, FormNo);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    msg = "Review save successfully";
                    status = "1";
                    M_Category objg = new M_Category();
                    string ProdId = Convert.ToString(Session["ProdId"]);
                    objg.ProductReview = iprod.GetProductReview(Convert.ToInt32(ProdId));
                    tblOrder = Extension.RenderRazorViewToString(this.ControllerContext, "ProductReview_partial", objg);
                }
                else
                {
                    msg = "Review not save";
                }
            }
            catch (Exception ex)
            {
                msg = "Something went wrong";
            }
            return Json(new { msg, status, tblOrder });
        }

        public ActionResult SaveToWishlist(int ProductID)
        {
            string msg = "Something went wrong";
            string status = "0";
            try
            {
                int FormNo = Convert.ToInt32(Session["FormNo"]);
                DataSet ds = iprod.SaveShoppingWishlist(FormNo, ProductID);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToString(ds.Tables[0].Rows[0]["msg"]) == "Save")
                    {
                        msg = "Product Add To Wishlist";
                        status = "1";
                    }
                    else
                    {
                        msg = "Product Remove To Wishlist";
                        status = "2";
                    }
                }
            }
            catch
            {

            }
            return Json(new { msg, status });
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
    }
}