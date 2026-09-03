using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopinv.Entity
{
    public class E_SendOTP
    {
        public string islogin { get; set; }
        public string reqtype { get; set; }
        public string referralid { get; set; }
        public string Name { get; set; }
        public string IsEmail { get; set; }
        public string IsMobile { get; set; }
        public string EmailID { get; set; }
        public string MobileNO { get; set; }
    }

    public class E_SendOTPp
    {
        public string islogin { get; set; }
        public string reqtype { get; set; }
        public string Name { get; set; }
        public string IsEmail { get; set; }
        public string IsMobile { get; set; }
        public string EmailID { get; set; }
        public string MobileNO { get; set; }
    }
    public class E_SendOTPRes
    {
        public string response { get; set; }
        public string msg { get; set; }

    }

    public class E_SendRes
    {
        public string response { get; set; }
        public string msg { get; set; }
        public string idno { get; set; }
        public string password { get; set; }
        public string url { get; set; }
    }
    public class E_sregistration
    {
        public string islogin { get; set; }
        public string reqtype { get; set; }
        public string referralid { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string mobl { get; set; }
        public string otpcode { get; set; }
        public string password { get; set; }
    }
}