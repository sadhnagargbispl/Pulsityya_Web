using Newtonsoft.Json;
using Shopinv.Entity;
using Shopinv.Interface;
using Shopinv.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace Shopinv.SiteExtension
{ 
    public class CompanyDetail
    {
        private readonly I_Product _iprod = null;
        private readonly static string Apiurl = ConfigurationManager.AppSettings["ApiUrl"];
        public CompanyDetail(I_Product iprod)
        {
            _iprod = iprod;
        }

        public void GetCompanydetail()
        {
            DataSet ds = _iprod.GetCompanydetail();
            HttpContext.Current.Session["CompCity"] = Convert.ToString(ds.Tables[0].Rows[0]["CompCity"]);
            HttpContext.Current.Session["CompAdd"] = Convert.ToString(ds.Tables[0].Rows[0]["CompAdd"]);
            HttpContext.Current.Session["CompMail"] = Convert.ToString(ds.Tables[0].Rows[0]["CompMail"]);
            HttpContext.Current.Session["ContactNo"] = Convert.ToString(ds.Tables[0].Rows[0]["ContactNo"]);
            HttpContext.Current.Session["CompName"] = Convert.ToString(ds.Tables[0].Rows[0]["CompName"]);
            HttpContext.Current.Session["WebPortal"] = Convert.ToString(ds.Tables[0].Rows[0]["WebPortal"]);
            HttpContext.Current.Session["WebSite"] = Convert.ToString(ds.Tables[0].Rows[0]["WebSite"]);
            // CompTitle is stored as a greeting ("Welcome to XYZ"); the views use it as the brand name
            string compTitle = Convert.ToString(ds.Tables[0].Rows[0]["CompTitle"]).Trim();
            if (compTitle.StartsWith("Welcome to ", StringComparison.OrdinalIgnoreCase))
                compTitle = compTitle.Substring("Welcome to ".Length).Trim();
            HttpContext.Current.Session["CompTitle"] = compTitle != "" ? compTitle : Convert.ToString(ds.Tables[0].Rows[0]["CompName"]);
            HttpContext.Current.Session["MobileNo"] = Convert.ToString(ds.Tables[0].Rows[0]["MobileNo"]);

            // optional m_companymaster columns used by the theme header/footer (SiteDoc/company-master-setup.sql).
            // Read only when Sp_GetCompanydetail returns them, so the site keeps working before the script is run.
            DataColumnCollection compCols = ds.Tables[0].Columns;
            foreach (string col in new[] { "CompTagline", "CompAboutUs", "CompWorkingHours", "FreeShipAmount", "FacebookUrl", "InstagramUrl", "TwitterUrl", "YoutubeUrl", "LinkedInUrl", "CompGSTNo" })
            {
                HttpContext.Current.Session[col] = compCols.Contains(col) ? Convert.ToString(ds.Tables[0].Rows[0][col]) : "";
            }

            // the logo comes only from m_companymaster.logourl: a full URL, a site path,
            // or a file name on the ImageUrl server (Web.config). Empty = no logo image.
            string compLogo = compCols.Contains("logourl") ? Convert.ToString(ds.Tables[0].Rows[0]["logourl"]).Trim() : "";
            if (compLogo != "" && !compLogo.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            {
                if (compLogo.StartsWith("~") || compLogo.StartsWith("/"))
                    compLogo = VirtualPathUtility.ToAbsolute(compLogo.StartsWith("/") ? "~" + compLogo : compLogo);
                else
                    compLogo = Convert.ToString(ConfigurationManager.AppSettings["ImageUrl"]).TrimEnd('/') + "/" + compLogo;
            }
            HttpContext.Current.Session["CompLogoUrl"] = compLogo;

            if (HttpContext.Current.Session["UserDetail"] != null)
            {
                var userid = HttpContext.Current.Session["UserId"];
                List<E_CartDetails> CartDetail = _iprod.Cartdetailsftch(Convert.ToString(userid)).ToList();
                HttpContext.Current.Session["Cartdetailsftch"] = CartDetail;
                HttpContext.Current.Session["cartcount"] = CartDetail.Count();
                HttpContext.Current.Session["TotPrice"] = CartDetail != null ? CartDetail.Sum(s => s.Price * s.qty).ToString() : "0";

                if (Convert.ToString(HttpContext.Current.Session["MemMode"]) == "D")
                {
                    Getkycreq req = new Getkycreq();
                    req.islogin = "N";
                    req.reqtype = "getkyc";
                    req.userid = Convert.ToString(HttpContext.Current.Session["IDNO"]);
                    req.passwd = Convert.ToString(HttpContext.Current.Session["password"]);
                    string jsonreq = JsonConvert.SerializeObject(req);
                    var response = CallPostFunction(jsonreq, Apiurl);
                    Getkycres kycresponse = JsonConvert.DeserializeObject<Getkycres>(response);
                    HttpContext.Current.Session["isKycCompleted"] = true;
                    //if (kycresponse.idverf != "Verified")
                    //{
                    //    HttpContext.Current.Session["isKycCompleted"] = false;
                    //}
                    //else
                    //{
                    //    HttpContext.Current.Session["isKycCompleted"] = true;
                    //}
                }
            }
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