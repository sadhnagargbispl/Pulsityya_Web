using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopinv.Models
{
    public class ExternalLogin
    {
        public string token { get; set; }
        public string mod { get; set; }
        public string userid { get; set; }
        public string password { get; set; }    
    }
}