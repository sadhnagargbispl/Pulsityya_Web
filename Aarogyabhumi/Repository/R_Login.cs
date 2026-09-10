using Newtonsoft.Json;
using Shopinv.Entity;
using Shopinv.Interface;
using Shopinv.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Util;

namespace Shopinv.Repoistory
{
    public class R_Login : I_Login
    {
        public IEnumerable<E_RegisterUser> SaveloginDetails(string UserName, string password, string FirstName, string LastName, string MobileNo, string FormNo, string ActiveStatus, string Fax, string Address, string City, string CityCode, string District, string DistrictCode, string StateCode, string PinCode, string CountryId, string CountryName, string randomId, string Email)
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            DataTable dt;
            hst.Add("Action", "SaveLogin");
            hst.Add("UserName", UserName);
            hst.Add("Password", password);
            hst.Add("FirstName", FirstName);
            hst.Add("LastName", LastName);
            hst.Add("MobileNo", MobileNo);
            hst.Add("FormNo", FormNo);
            hst.Add("ActiveStatus", ActiveStatus);
            hst.Add("Fax", Fax);
            hst.Add("Address", Address);
            hst.Add("CityCode", CityCode);
            hst.Add("City", City);
            hst.Add("District", District);
            hst.Add("DistrictCode", DistrictCode);
            hst.Add("StateCode", StateCode);
            hst.Add("PinCode", PinCode);
            hst.Add("CountryId", CountryId);
            hst.Add("CountryName", CountryName);
            hst.Add("randomId", randomId);
            hst.Add("Email", Email);
            dt = blldb.GetDataTable("sp_Login", CommandType.StoredProcedure, hst);
            //return dt.Rows[0][0].ToString();
            IEnumerable<E_RegisterUser> lst = DbOperation.ConvertDataTable<E_RegisterUser>(dt);
            return lst;
        }
        public IEnumerable<E_AadhaarOtpLog> SaveAadhaarOtpLog(
    string AadhaarNo,
    string ReferenceId,
    string Status,
    string ApiResponse, string TransactionId
)
        {
            BLLDBOperations blldb = new BLLDBOperations();

            Hashtable hst = new Hashtable();

            DataTable dt;

            hst.Add("Action", "InsertOtpLog");

            hst.Add("AadhaarNo", AadhaarNo);

            hst.Add("ReferenceId", ReferenceId);

            hst.Add("Status", Status);

            hst.Add("ApiResponse", ApiResponse);
            hst.Add("TransactionId", TransactionId);
            dt = blldb.GetDataTable(
                "sp_AadhaarOtpLogs",
                CommandType.StoredProcedure,
                hst
            );

            IEnumerable<E_AadhaarOtpLog> lst =
                DbOperation.ConvertDataTable<E_AadhaarOtpLog>(dt);

            return lst;
        }
        public IEnumerable<E_AadhaarOtpVerifyLog> SaveAadhaarOtpVerifyLog(
      string ReferenceId,
      string OTP,
      string Name,
      string DOB,
      string Gender,
      string Address,
      string Status,
      string Message,
      string CareOf,
      string EmailHash,
      string MobileHash,
      string YearOfBirth,
      string ShareCode,
      string Country,
      string District,
      string House,
      string Landmark,
      string Pincode,
      string PostOffice,
      string State,
      string Street,
      string Subdistrict,
      string VTC,
      string Photo,
      string ApiResponse,
      string TransactionId
      )
        {
            BLLDBOperations blldb =
                new BLLDBOperations();

            Hashtable hst =
                new Hashtable();

            DataTable dt;

            hst.Add("Action", "InsertVerifyLog");

            hst.Add("ReferenceId", ReferenceId);

            hst.Add("OTP", OTP);

            hst.Add("Name", Name);

            hst.Add("DOB", DOB);

            hst.Add("Gender", Gender);

            hst.Add("Address", Address);

            hst.Add("Status", Status);

            hst.Add("Message", Message);

            hst.Add("CareOf", CareOf);

            hst.Add("EmailHash", EmailHash);

            hst.Add("MobileHash", MobileHash);

            hst.Add("YearOfBirth", YearOfBirth);

            hst.Add("ShareCode", ShareCode);

            hst.Add("Country", Country);

            hst.Add("District", District);

            hst.Add("House", House);

            hst.Add("Landmark", Landmark);

            hst.Add("Pincode", Pincode);

            hst.Add("PostOffice", PostOffice);

            hst.Add("State", State);

            hst.Add("Street", Street);

            hst.Add("Subdistrict", Subdistrict);

            hst.Add("VTC", VTC);

            hst.Add("Photo", Photo);

            hst.Add("ApiResponse", ApiResponse);

            hst.Add("TransactionId", TransactionId);

            dt = blldb.GetDataTable(
                "sp_AadhaarOtpVerifyLog",
                CommandType.StoredProcedure,
                hst
            );

            IEnumerable<E_AadhaarOtpVerifyLog> lst =
                DbOperation.ConvertDataTable<E_AadhaarOtpVerifyLog>(dt);

            return lst;
        }
        public IEnumerable<E_RegisterUser> SrchUserDetail(string FormNo, string rnd)
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            DataTable dt;
            hst.Add("Action", "SrchUserDeatil");
            hst.Add("FormNo", FormNo);
            hst.Add("randomId", rnd);
            dt = blldb.GetDataTable("sp_Login", CommandType.StoredProcedure, hst);
            IEnumerable<E_RegisterUser> lst = DbOperation.ConvertDataTable<E_RegisterUser>(dt);
            return lst;
        }

        public string UserPoints(string rndNo, string FormNo, string ProdId, string username, string UserPoints)
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            DataTable dt;
            hst.Add("Action", "InsertUserPoints");
            hst.Add("FormNo", FormNo);
            hst.Add("randomId", rndNo);
            hst.Add("ProdId", ProdId);
            hst.Add("username", username);
            hst.Add("UserPoints", UserPoints);
            dt = blldb.GetDataTable("sp_Login", CommandType.StoredProcedure, hst);
            return dt.Rows[0][0].ToString();
        }

        public DataSet LoginApiUser(string Action, E_RegisterUser userDetail)
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            DataTable dt;
            hst.Add("Action", "Login");
            hst.Add("UserName", userDetail.UserName);
            hst.Add("Password", userDetail.Password);
            using (DataSet ds = blldb.GetDataSet("sp_Login", CommandType.StoredProcedure, hst))
            {
                return ds;
            }

        }

        public IEnumerable<StateList> GetDDLState()
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            DataTable dt;
            hst.Add("Action", "Sp_DDlState");
            dt = blldb.GetDataTable("sp_Login", CommandType.StoredProcedure, hst);
            IEnumerable<StateList> lst = DbOperation.ConvertDataTable<StateList>(dt);
            return lst;
        }

        public IEnumerable<StateList> GetDDLStateFranchise()
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            DataTable dt;
            hst.Add("Action", "DDlState");
            dt = blldb.GetDataTable("Sp_DDlState", CommandType.StoredProcedure, hst);
            IEnumerable<StateList> lst = DbOperation.ConvertDataTable<StateList>(dt);
            return lst;
        }
        public IEnumerable<E_RegisterUser> GetUserLoginDetail(string UserName, string Password, string id)
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            DataTable dt;
            hst.Add("Action", "GetUserLoginDetail");
            hst.Add("UserName", UserName);
            hst.Add("Password", Password);
            hst.Add("id", id);
            dt = blldb.GetDataTable("sp_Login", CommandType.StoredProcedure, hst);
            IEnumerable<E_RegisterUser> lst = DbOperation.ConvertDataTable<E_RegisterUser>(dt);
            return lst;
        }


        public IEnumerable<E_RegisterUser> GetUserotherLoginDetail(string UserName, string Password, string id)
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            DataTable dt;
            hst.Add("Action", "GetUserOtherLogin");
            hst.Add("UserName", UserName);
            hst.Add("Password", Password);
            hst.Add("id", id);
            dt = blldb.GetDataTable("sp_Login", CommandType.StoredProcedure, hst);
            IEnumerable<E_RegisterUser> lst = DbOperation.ConvertDataTable<E_RegisterUser>(dt);
            return lst;
        }

        public DataSet getRefdata(string refid)
        {
            DataSet dsreturn = new DataSet();
            try
            {
                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();
                DataSet ds;
                hst.Add("refid", refid);
                ds = blldb.GetDataSet("sp_getRefdata", CommandType.StoredProcedure, hst);

                dsreturn = ds;
            }
            catch (Exception ex)
            {

            }
            return dsreturn;
        }
        public IEnumerable<E_RegisterUser> SaveAddressDetail(string Action, string Id, string UserName,
            string Password, string Email, string FirstName, string Lastname, string Mobile, string FormNo,
            string StateCode, string District, string City, string Address, string PinCode,
             string AlternateMobileno, string BillingAddress, string BillingCity,
            string BillingPinCode, string BillingStateCodebState)
        {
            IEnumerable<E_RegisterUser> lst = null;
            try
            {
                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();
                DataTable dt;
                hst.Add("Action", Action);
                hst.Add("UserName", UserName);
                hst.Add("Password", Password);
                hst.Add("id", Id);
                hst.Add("FirstName", FirstName);
                hst.Add("LastName", Lastname);
                hst.Add("MobileNo", Mobile);
                hst.Add("FormNo", FormNo);
                hst.Add("Address", Address);
                hst.Add("City", City);
                hst.Add("District", District);
                hst.Add("StateCode", StateCode);
                hst.Add("PinCode", PinCode);
                hst.Add("Email", Email);
                dt = blldb.GetDataTable("sp_Login", CommandType.StoredProcedure, hst);
                lst = DbOperation.ConvertDataTable<E_RegisterUser>(dt);

                return lst;
            }
            catch (Exception ex)
            {

            }
            return lst;
        }

        public DataSet GetuserDetail(string userid, string IDno)
        {
            DataSet dsreturn = new DataSet();
            try
            {
                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();
                DataSet ds;
                hst.Add("Action", "GetuserDetail");
                hst.Add("userid", userid);
                hst.Add("IDno", IDno);

                ds = blldb.GetDataSet("sp_GetuserDetail", CommandType.StoredProcedure, hst);

                dsreturn = ds;
            }

            catch (Exception ex)
            {

            }
            return dsreturn;
        }

        public E_RegisterUser Checklogin(E_RegisterUser userdetail)
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            DataSet ds;
            E_RegisterUser User = new E_RegisterUser();
            hst.Add("Action", "Checklogin");
            hst.Add("UserName", userdetail.UserName);
            hst.Add("Password", userdetail.Password);
            hst.Add("FormNo", userdetail.FormNo);
            using (ds = blldb.GetDataSet("sp_Login", CommandType.StoredProcedure, hst))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    User.UserName = Convert.ToString(ds.Tables[0].Rows[0]["IdNo"]);
                    User.Password = Convert.ToString(ds.Tables[0].Rows[0]["Passw"]);
                    User.Firstname = Convert.ToString(ds.Tables[0].Rows[0]["MemFirstName"]);
                    User.LastName = Convert.ToString(ds.Tables[0].Rows[0]["MemLastName"]);
                    User.MobileNo = Convert.ToString(ds.Tables[0].Rows[0]["Mobl"]);
                    User.FormNo = Convert.ToString(ds.Tables[0].Rows[0]["FormNo"]);
                    User.ActiveStatus = Convert.ToString(ds.Tables[0].Rows[0]["ActiveStatus"]);
                    User.Fax = Convert.ToString(ds.Tables[0].Rows[0]["Fax"]);
                    User.Address = Convert.ToString(ds.Tables[0].Rows[0]["Address"]);
                    User.City = Convert.ToString(ds.Tables[0].Rows[0]["City"]);
                    User.CityCode = Convert.ToInt32(ds.Tables[0].Rows[0]["CityCode"]);
                    User.District = Convert.ToString(ds.Tables[0].Rows[0]["District"]);
                    User.DistrictCode = Convert.ToInt32(ds.Tables[0].Rows[0]["DistrictCode"]);
                    User.StateCode = Convert.ToInt32(ds.Tables[0].Rows[0]["StateCode"]);
                    User.PinCode = Convert.ToString(ds.Tables[0].Rows[0]["PinCode"]);
                    User.CountryId = Convert.ToInt32(ds.Tables[0].Rows[0]["CountryId"]);
                    User.CountryName = Convert.ToString(ds.Tables[0].Rows[0]["CountryName"]);

                }
            }
            return User;



        }

        public DataSet GetStateFranchise(string StateCode)
        {
            DataSet dsreturn = new DataSet();
            try
            {
                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();
                DataSet ds;
                E_RegisterUser User = new E_RegisterUser();
                hst.Add("Action", "GetStateFranchise");
                hst.Add("StateCode", StateCode);
                ds = blldb.GetDataSet("sp_GetStateFranchise", CommandType.StoredProcedure, hst);

                dsreturn = ds;
            }
            catch (Exception ex)
            {

            }
            return dsreturn;
        }
        public DataSet GetFranchiseProduct(string PartyCode)
        {
            DataSet dsreturn = new DataSet();
            try
            {
                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();
                DataSet ds;
                E_RegisterUser User = new E_RegisterUser();
                hst.Add("Fcode", PartyCode);
                ds = blldb.GetDataSet("sp_FranchiseProduct", CommandType.StoredProcedure, hst);

                dsreturn = ds;
            }
            catch (Exception ex)
            {

            }
            return dsreturn;
        }

        public DataSet GetMemberdetails(string IDno)
        {
            DataSet dsreturn = new DataSet();
            try
            {
                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();
                DataSet ds;
                hst.Add("IDno", IDno);

                ds = blldb.GetDataSet("sp_GetMemberdetails", CommandType.StoredProcedure, hst);

                dsreturn = ds;
            }

            catch (Exception ex)
            {

            }
            return dsreturn;
        }

        public IEnumerable<E_Grivancelstres> GetComplaintType()
        {
            BLLDBOperations blldb = new BLLDBOperations();
            DataTable dt;
            dt = blldb.GetDataTable("sp_companytype", CommandType.StoredProcedure);
            IEnumerable<E_Grivancelstres> lst = DbOperation.ConvertDataTable<E_Grivancelstres>(dt);
            return lst;
        }

        public DataSet GetSellerInformation(string Username)
        {
            BLLDBOperations blldb = new BLLDBOperations();
            DataSet ds;
            Hashtable hst = new Hashtable();
            hst.Add("Action", "GetInfo");
            hst.Add("IdNo", Username);
            ds = blldb.GetDataSet("Sp_SellerInformation", CommandType.StoredProcedure, hst);
            return ds;
        }
        public string SendWhatsappMessage(string mobileNo, string name, string idNo, string password)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                string reqId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string url = "https://wa.bisplindia.in/api/method/frappe_whatsapp.whatsapp_chat.send_template";

                // Template ke {{1}} {{2}} {{3}} order ke hisaab se message
                string whatsappMsg =
                    "Welcome To " + name + "," +
                    "\nThank You For Registration." +
                    "\nYour ID Is " + idNo +
                    " and Password is " + password + "." +
                    "\nBest of luck." +
                    "\n\nRegards" +
                    "\nBright Team";

                var request = new
                {
                    mobile_no = mobileNo,
                    message = whatsappMsg
                };

                string jsonData = JsonConvert.SerializeObject(request);
                SaveWhatsappRequest(reqId, url, jsonData);

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add(
                                    "Authorization",
                                 "token b246f118c913831:64827093075409a");


                     var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    // ⚠️ MVC (.NET Framework) mein .Result deadlock kar sakta hai.
                    // Task.Run se SynchronizationContext bypass ho jaata hai — safe.
                    HttpResponseMessage response = Task.Run(() => client.PostAsync(url, content)).Result;
                    string responseText = Task.Run(() => response.Content.ReadAsStringAsync()).Result;

                    SaveWhatsappResponse(reqId, responseText);
                    return responseText;
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        //public string SendWhatsappMessage(string mobileNo, string name, string idNo, string password)
        //{
        //    try
        //    {
        //        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        //        string reqId = DateTime.Now.ToString("yyyyMMddHHmmssfff");

        //        string url = "https://wa.bisplindia.in/api/method/frappe_whatsapp.whatsapp_chat.send_template";

        //        string whatsappMsg =
        //            "Welcome To " + name + "," +
        //            "\nThank You For Registration." +
        //            "\nYour ID Is " + idNo +
        //            " and Password is " + password + "." +
        //            "\nBest of luck." +
        //            "\n\nRegards" +
        //            "\nBright Team";

        //        var request = new
        //        {
        //            mobile_no = mobileNo,
        //            message = whatsappMsg
        //        };

        //        string jsonData = JsonConvert.SerializeObject(request);

        //        SaveWhatsappRequest(reqId, url, jsonData);

        //        using (HttpClient client = new HttpClient())
        //        {
        //            client.DefaultRequestHeaders.Add(
        //                "Authorization",
        //                "token b246f118c913831:64827093075409a");

        //            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

        //            HttpResponseMessage response = client.PostAsync(url, content).Result;

        //            string responseText = response.Content.ReadAsStringAsync().Result;

        //            SaveWhatsappResponse(reqId, responseText);

        //            return responseText;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.ToString();
        //    }
        //}
       

        public void SaveWhatsappRequest(string reqId, string requestUrl, string postData)
        {
            try
            {
                BLLDBOperations blldb = new BLLDBOperations();

                Hashtable hst = new Hashtable();
                hst.Add("ReqID", reqId);
                hst.Add("Request", requestUrl);
                hst.Add("PostData", postData);
                hst.Add("CompID", 1007);

                string query = @"INSERT INTO Tbl_ApiRequest_ResponseWhatsapp
                    (ReqID, Request, postdata, CompID)
                    VALUES
                    (@ReqID, @Request, @PostData, @CompID)";

                blldb.GetDataSet(query, CommandType.Text, hst);
            }
            catch (Exception ex)
            {
            }
        }
        public void SaveWhatsappResponse(string reqId, string response)
        {
            try
            {
                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();

                hst.Add("ReqID", reqId);
                hst.Add("Response", response);

                blldb.GetDataSet(
                    "UPDATE Tbl_ApiRequest_ResponseWhatsapp SET Response=@Response WHERE ReqID=@ReqID",
                    CommandType.Text,
                    hst);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void UpdateMessageSent(string formNo, string mobileNo)
        {
            try
            {
                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();

                hst.Add("FormNo", formNo);
                hst.Add("MobileNo", mobileNo);

                blldb.GetDataSet(
                    "UPDATE MessageSendLog SET IsSent='1', SentDate=GETDATE() WHERE MobileNo=@MobileNo AND FormNo=@FormNo",
                    CommandType.Text,
                    hst);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}