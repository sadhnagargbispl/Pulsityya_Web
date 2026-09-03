namespace Shopinv.Models
{
    public class M_CashFreeResponse
    {

         public int cf_order_id { get; set; }
         public string created_at { get; set; }
         public customer_detailsres customer_details { get; set; }
         public string entity { get; set; }
         public float order_amount { get; set; }
         public string order_currency { get; set; }
         public string order_expiry_time { get; set; }
         public string order_id { get; set; }
         public order_meta order_meta { get; set; }
         public string order_note { get; set; }
         public string order_status { get; set; }
         public string order_token { get; set; }
         public string payment_link { get; set; }
         public URl payments { get; set; }
         public URl refunds { get; set; }
         public URl settlements { get; set; }
    }

     public class customer_detailsres
    {
        public string customer_id { get; set; }
        public string customer_email { get; set; }
        public string customer_phone { get; set; }
         public string customer_name { get; set; }
    }
     public class URl
    {
        public string url { get; set; }
    }
}