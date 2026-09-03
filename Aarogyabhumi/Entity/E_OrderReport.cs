using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopinv.Entity
{
    public class E_OrderReport
    {
        public decimal OrderId { get; set; }
        public int ProdId { get; set; }
        public string ProductName { get; set; }
        public string Mode { get; set; }
        public decimal Price { get; set; }
        // public decimal bvv { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        //public decimal Qty { get; set; }
        public int Qty { get; set; }
        public decimal Amount { get; set; }
        public string CreatedOn { get; set; }
        public string Formno { get; set; }
        public string Orderno { get; set; }
        public string Orderno1 {  get; set; }
        public string orderdate { get; set; }
        public decimal OrderQty { get; set; }
        public decimal DispatchQty { get; set; }
        public decimal OrderAmount { get; set; }
        public decimal Pinwallet { get; set; }
        public decimal OtherAmt { get; set; }
        public string status { get; set; }
        public string KitName { get; set; }
        public string CourierName { get; set; }
        public string DocketNo { get; set; }
        public string DocketDate { get; set; }
        public decimal BV { get; set; }
        public string OrderBy { get; set; }
        public string email { get; set; }
        public decimal mobl { get; set; }
        public string Website { get; set; }
        public decimal CourierCharge { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string ImagePath { get; set; }
        public decimal MRP { get; set; }
        public decimal Rate { get; set; }
        public decimal NetAmount { get; set; } 
        public decimal couponamount { get; set; }
        public string coupon { get; set; }
        public string BillNo { get; set; }
        public decimal Chamt { get; set; }
        public string InvBillno { get; set; }
        public string Transid { get; set; }
        public int id { get; set; }
        public string txnid { get; set; }
        public string remark { get; set; }
        public string paymentimg { get; set; }
        public string Ordertype { get; set; }
        public DateTime orderdate1 { get; set; } 
        public decimal PV { get; set; }

    }
}