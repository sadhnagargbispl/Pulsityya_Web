using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopinv.Models
{
    public class M_payreqCashFreenew
    {
        public float order_amount { get; set; }
        public string order_currency { get; set; }
        public string order_id { get; set; }
        public Customer_Details customer_details { get; set; }
        public Order_Meta order_meta { get; set; }
    }

    public class Customer_Details
    {
        public string customer_id { get; set; }
        public string customer_phone { get; set; }
    }
    public class Order_Meta
    {
        public string return_url { get; set; }
    }
    public class cashfreeresponse
    {
        public M_Property1[] Property1 { get; set; }
    }

    public class M_Property1 
    {
        public object auth_id { get; set; }
        public object authorization { get; set; }
        public string bank_reference { get; set; }
        public string cf_payment_id { get; set; }
        public string entity { get; set; }
        public object error_details { get; set; }
        public International_Payment international_payment { get; set; }
        public bool is_captured { get; set; }
        public int order_amount { get; set; }
        public string order_currency { get; set; }
        public string order_id { get; set; }
        public int payment_amount { get; set; }
        public DateTime payment_completion_time { get; set; }
        public string payment_currency { get; set; }
        public Payment_Gateway_Details payment_gateway_details { get; set; }
        public string payment_group { get; set; }
        public string payment_message { get; set; }
        public Payment_Method payment_method { get; set; }
        public object payment_offers { get; set; }
        public string payment_status { get; set; }
        public object payment_surcharge { get; set; }
        public DateTime payment_time { get; set; }
    }

    public class International_Payment
    {
        public bool international { get; set; }
    }

    public class Payment_Gateway_Details
    {
        public string gateway_name { get; set; }
        public object gateway_order_id { get; set; }
        public object gateway_payment_id { get; set; }
        public object gateway_order_reference_id { get; set; }
        public object gateway_status_code { get; set; }
        public string gateway_settlement { get; set; }
        public object gateway_reference_name { get; set; }
    }

    public class Payment_Method
    {
        public Upi upi { get; set; }
    }

    public class Upi
    {
        public string channel { get; set; }
        public string upi_id { get; set; }
        public string upi_instrument { get; set; }
        public string upi_instrument_number { get; set; }
        public string upi_payer_account_number { get; set; }
        public string upi_payer_ifsc { get; set; }
    }

}