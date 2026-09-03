using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopinv.Models
{
    public class M_Referral
    {
        public string urlLeft { get; set; }
        public string urlright { get; set; }
    }

    public class Referalreq
    {
        public string islogin { get; set; }
        public string reqtype { get; set; }
        public string userid { get; set; }
        public string passwd { get; set; }
    }
   
    public class Referalres
    {
        public string urlLeft { get; set; }
        public string urlright { get; set; }
        public string response { get; set; }
    }

}