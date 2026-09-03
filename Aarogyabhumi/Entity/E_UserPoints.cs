using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopinv.Entity
{
    public class E_UserPoints
    {
        public int ProdId { get; set; }
        public string UserName { get; set; }
        public string FormNo { get; set; }
        public string RandomId { get; set; }
        public decimal Points { get; set; }
    }
}