using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopinv.Entity
{
    public class E_CouponDetail
    {
        public decimal SNo { get; set; }
        public decimal CouponNo { get; set; }
        public string IssueDate { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
        public string Useddate { get; set; }
        public string Usedby { get; set; }
        public string Usedorderno { get; set; }
    }
}