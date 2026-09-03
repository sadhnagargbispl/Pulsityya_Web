using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopinv.Entity
{
    public class E_PendingTrans
    {

         public string txnid { get; set; }
         public string transid { get; set; }
         public string Amount { get; set; }
        public string Status { get; set; }
        public string idNo { get; set; }
         public string phnone { get; set; }
        public string email { get; set; }
        public string firstname { get; set; }
        public string createdon { get; set; }
    }

     public class E_pendingOrderDetail
    {
        public string ProdCode { get; set; }
        public string Prodname { get; set; }
        public string Qty { get; set; }
        public string BunchQty { get; set; }
        public string PV { get; set; }
        public string EP { get; set; }
        public decimal price { get; set; }
        public string txnid { get; set; }
    }



     public class E_pendingLstTrans
    {
        public IEnumerable<E_pendingOrderDetail> PendingOrder { get; set; }
         public IEnumerable<E_PendingTrans> PendingTrans { get; set; }
    }
}