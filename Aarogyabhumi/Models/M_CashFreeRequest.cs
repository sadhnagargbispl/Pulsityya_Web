namespace Shopinv.Models
{
    public class M_CashFreeRequest
    {
        public string appId { get; set; }
        public string orderId { get; set; }
        public string orderAmount { get; set; }
        public string orderCurrency { get; set; }
        public string orderNote { get; set; }
        public string customerName { get; set; }
        public string customerEmail { get; set; }
        public string customerPhone { get; set; }
        public string notifyUrl { get; set; }
        public string returnUrl { get; set; }
    }


     public class M_ReqCashFree
    {
        public M_payreqCashFree data { get; set; }
    }
     public class M_payreqCashFree
    {
       
         public string order_id { get; set; }
        public float order_amount { get; set; }
        public string order_currency { get; set; }
       public customer_details customer_details { get; set; }
         public order_meta order_meta { get; set; }
        //public string customer_id { get; set; }
        //public string customer_email { get; set; }
        //public string customer_phone { get; set; }
        //public string returnUrl { get; set; }
    }

     public class customer_details
    {
        public string customer_id { get; set; }
        public string customer_email { get; set; }
        public string customer_phone { get; set; }
    }
     public class order_meta
    {
        public string return_url { get; set; }
         public string notify_url { get; set; }
         public string payment_methods { get; set; }
    }
    public class M_CashFreeApiRequest
    {
       // public string appId { get; set; }
        public string orderId { get; set; }
        public string orderAmount { get; set; }
        public string customerName { get; set; }
        public string customerEmail { get; set; }
        public string customerPhone { get; set; }
         public string FormNo { get; set; }
         public string IDno { get; set; }
         public string IsTest { get; set; }
       
    }
    public class M_Response
    {
        public string RESP_CODE { get; set; }
        public string RESPONSE { get; set; }
        public string RESP_MSG { get; set; }
        public string RESP_VALUE { get; set; }
        public string RESP_VALUE1 { get; set; }
    }

}