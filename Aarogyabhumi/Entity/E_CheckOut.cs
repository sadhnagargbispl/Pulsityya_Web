using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopinv.Entity
{
    public class E_CheckOut
    {
        public int OrderId { get; set; }
        //public int Id { get; set; }
        public string ProdId { get; set; }
        public string ProdName{get;set;}
         public decimal Price { get; set; }
        public decimal qty { get; set;}
       
        public decimal bv { get; set; }
    }
     public class E_SaveOrderDetail
    {
        public int OrderId { get; set; }
        public string ProdId { get; set; }
        public string ProdName { get; set; }
        public decimal Price { get; set; }
        public decimal qty { get; set; }
         public decimal BunchQty { get; set; }
         public string Mode { get; set; }
        public decimal CourierCharge { get; set; }
         public string OrderDate { get; set; }
        public string ImagePath { get; set; }
        public decimal MRP { get; set; }
        public decimal BV { get; set; }
        public decimal PV { get; set; } 
        public decimal LessEB { get; set; }
        public decimal DP { get; set; }
        public decimal finalprice { get; set; }

        //public string Imagepath { get; set; }
    }
}