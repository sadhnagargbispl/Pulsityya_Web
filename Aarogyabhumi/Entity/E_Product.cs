//using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopinv.Entity
{
    public class E_Product
    {
        //public IPagedList<int> pagerCount { get; set; }
        public string order { get; set; }
        public string sortby { get; set; }
        public string SearchString { get; set; }
        public string ProdId { get; set; }
        public string ProductName { get; set; }
        public string ImagePath { get; set; }
        public decimal Price { get; set; }
        public decimal MRP { get; set; }
        public string SubcatName { get; set; }
        public string CatName { get; set; }
        public decimal BV { get; set; }
        public Int32 PV { get; set; }
        public decimal Discount { get; set; }
        public string Liner { get; set; }
        public string ProductDiscription { get; set; }
        public decimal BunchQty { get; set; }
        public decimal Weight { get; set; }
        public decimal StockQuantiy { get; set; }
        public string FranchisepartyCode { get; set; }
        public decimal Gst { get; set; }
        public decimal StockQTY { get; set; }
        public Boolean isCart { get; set; } 
        public decimal CartQty { get; set; }    
        public IEnumerable<E_ProductDetail> ProductDetail { get; set; }

    }
    public class E_ProductDetail
    {
        public string ProductName { get; set; }
        public string ProdId { get; set; }
        public string SubcatName { get; set; }
        public string CatName { get; set; }
        public decimal BV { get; set; }
        public decimal PV { get; set; }
        public decimal Discount { get; set; }
        public string ImagePath { get; set; }
        public decimal Price { get; set; }
        public string ImagePath1 { get; set; }
        public string ImagePath2 { get; set; }
        public string ImagePath3 { get; set; }
        public string ImagePath4 { get; set; }
        public string ImagePath5 { get; set; }
        public string ProductDesc { get; set; }
        public string Liner { get; set; }
        public decimal MRP { get; set; }
        public string Stock_Check { get; set; }
        public string Stock { get; set; }
        public decimal Qty { get; set; }
        public decimal BunchQty { get; set; }
        public decimal Weight { get; set; }
        public decimal Gst { get; set; }
        public decimal StockQTY { get; set; }
        public string IsWishlist { get; set; }
        public decimal ProdCommssn { get; set; }    
    }

    public class E_ProductReview
    { 
        public int id { get; set; }
        public int productcode { get; set; }
        public int formno { get; set; }
        public string username { get; set; }
        public int Rating { get; set; }
        public string Review { get; set; }
    }
}