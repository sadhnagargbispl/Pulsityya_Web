using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopinv.Entity
{
    public class E_Payment
    {
        public string ProdId { get; set; }
        public string Mode { get; set; }
        public decimal Amount { get; set; }
        public decimal Avail_Bal { get; set; }
        public decimal REL_Bal { get; set; }
        public decimal Balance { get; set; }
        public decimal Creadit { get; set; }
        public decimal debit { get; set; }
    }
}