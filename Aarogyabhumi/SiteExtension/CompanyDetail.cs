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
using System.Web.SessionState;

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

        /// <summary>
        /// Reads a column from the company row. Returns "" when the column does
        /// not exist yet, so new m_companymaster columns can be added at any
        /// time without the site breaking before/after the change.
        /// </summary>
        private static string Col(DataRow row, string name)
        {
            if (row == null || !row.Table.Columns.Contains(name))
            {
                return string.Empty;
            }
            object v = row[name];
            return v == null || v == DBNull.Value ? string.Empty : Convert.ToString(v).Trim();
        }

        /// <summary>Uses the first non-empty value, so a blank column falls back.</summary>
        private static string FirstNonEmpty(params string[] values)
        {
            foreach (string v in values)
            {
                if (!string.IsNullOrWhiteSpace(v))
                {
                    return v.Trim();
                }
            }
            return string.Empty;
        }

        public void GetCompanydetail()
        {
            DataSet ds = _iprod.GetCompanydetail();
            HttpSessionState session = HttpContext.Current.Session;

            DataRow row = (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                ? ds.Tables[0].Rows[0]
                : null;

            // ---- identity -----------------------------------------------------
            session["CompName"] = Col(row, "CompName");
            session["CompAdd"] = Col(row, "CompAdd");
            session["CompRegOffAdd"] = Col(row, "CompRegOffAdd");
            session["CompCity"] = Col(row, "CompCity");
            session["CompMail"] = Col(row, "CompMail");
            // ContactNo is the landline; fall back to the mobile number when it is blank
            session["ContactNo"] = FirstNonEmpty(Col(row, "ContactNo"), Col(row, "MobileNo"));
            session["MobileNo1"] = Col(row, "MobileNo");
            session["WebSite"] = Col(row, "WebSite");
            session["WebPortal"] = Col(row, "WebPortal");
            session["CompTitle"] = Col(row, "CompTitle");
            session["CompPANNo"] = Col(row, "CompPANNo");
            session["CompanyIDNo"] = Col(row, "CompanyIDNo");
            session["CompTerm"] = Col(row, "CompTerm");
            session["MsgOnInvoice"] = Col(row, "MsgOnInvoice");
            session["CompGSTNo"] = FirstNonEmpty(Col(row, "CompGSTNo"), Col(row, "CompTinNo"));

            // ---- presentation (add these columns to m_companymaster to drive
            //      them from the database; blank keeps the built-in default) ----
            session["CompLogo"] = ResolveLogo(Col(row, "CompLogo"));
            session["CompTagline"] = FirstNonEmpty(Col(row, "CompTagline"), Col(row, "CompTitle"));
            session["CompAboutUs"] = Col(row, "CompAboutUs");
            session["CompWorkingHours"] = Col(row, "CompWorkingHours");
            session["FreeShipAmount"] = Col(row, "FreeShipAmount");
            session["FacebookUrl"] = Col(row, "FacebookUrl");
            session["InstagramUrl"] = Col(row, "InstagramUrl");
            session["TwitterUrl"] = Col(row, "TwitterUrl");
            session["YoutubeUrl"] = Col(row, "YoutubeUrl");
            session["LinkedInUrl"] = Col(row, "LinkedInUrl");

            if (HttpContext.Current.Session["UserDetail"] != null)
            {
                var userid = HttpContext.Current.Session["UserId"];
                List<E_CartDetails> CartDetail = _iprod.Cartdetailsftch(Convert.ToString(userid)).ToList();
                HttpContext.Current.Session["Cartdetailsftch"] = CartDetail;
                HttpContext.Current.Session["cartcount"] = CartDetail.Count();
                HttpContext.Current.Session["TotPrice"] = CartDetail != null ? CartDetail.Sum(s => s.Price * s.qty).ToString() : "0";

                HttpContext.Current.Session["isKycCompleted"] = true;
            }
        }

        /// <summary>
        /// A logo stored in m_companymaster may be a full URL, a site-relative
        /// path, or just a file name uploaded through the admin panel.
        /// </summary>
        private static string ResolveLogo(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;               // views fall back to the theme logo
            }
            value = value.Trim();

            if (value.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                value.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                return value;
            }
            if (value.StartsWith("~") || value.StartsWith("/"))
            {
                return VirtualPathUtility.ToAbsolute(value.StartsWith("~") ? value : "~" + value);
            }

            // bare file name - it lives with the other admin uploads
            string uploads = ConfigurationManager.AppSettings["ImageUrl"];
            return string.IsNullOrWhiteSpace(uploads)
                ? value
                : uploads.TrimEnd('/') + "/" + value.TrimStart('/');
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