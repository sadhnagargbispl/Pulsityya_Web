using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopinv.Entity 
{
    public class E_CartDetails
    {
        public int id { get; set; }
        public string ProdId { get; set; }
        public string ProdName { get; set; }
        public string imagePath { get; set; }
        public string IpAddress { get; set; }
        public decimal Price { get; set; }
        public decimal BunchQty { get; set; }
        public decimal bv { get; set; }
        public Int32 PV { get; set; }
        public decimal qty { get; set; }
        public string PartyCode { get; set; }
        public decimal Weight { get; set; }
        public decimal Gst { get; set; }
        public decimal prodprice { get; set; }
        public decimal gstAmt { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public int WishlistID { get; set; }
        public decimal Stockqty { get; set; }
        public decimal MRP { get; set; }
        public string BatchNo { get; set; }
     
    }
}