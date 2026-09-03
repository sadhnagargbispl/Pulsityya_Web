using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shopinv.Models
{
    public class M_Changepassword
    {
        [Required]
        public string passwd { get; set; }

        [Required]
        public string npasswd { get; set; }
    }

    public class Changepasswordreq
    {
        public string islogin { get; set; }
        public string reqtype { get; set; }
        public string userid { get; set; }
        public string passwd { get; set; }
        public string npasswd { get; set; }
    }
    public class Changepasswordres
    {
        public string response { get; set; }
        public string msg { get; set; }
    }

    public class Changetxnpasswordreq
    {
        public string islogin { get; set; }
        public string passwd { get; set; }
        public string reqtype { get; set; }
        public string userid { get; set; }
        public string tpasswd { get; set; }
        public string ntpasswd { get; set; }
    }
}