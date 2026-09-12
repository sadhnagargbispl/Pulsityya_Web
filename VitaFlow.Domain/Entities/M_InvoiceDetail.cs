using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VitaFlow.Domain.Entities
{
    public  class M_InvoiceDetail
    {
        public string OrderNo { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAdd { get; set; }
        public string BillNo { get; set; }
        public DateTime BillDate { get; set; }
        public string GSTNo { get; set; }
        public string SoldBy { get; set; }
        public string SoldByName { get; set; }
        public string SoldByAddress { get; set; }
        public string SoldByCity { get; set; }
        public string MobileNo { get; set; }
        public string BillDateStr { get; set; }
        public decimal OldTaxAmount { get; set; }
        public int NumberOfBill { get; set; }
        public string BillType { get; set; }
        public int offerId { get; set; }
        public int NewofferId { get; set; }
        public string StateGSTName { get; set; }
        public string CompCity { get; set; }
        public string TaxORStock { get; set; }
        public string TaxType { get; set; }
        public string Username { get; set; }
        public int IsSequneceproduct { get; set; }
        public decimal UserId { get; set; }
        public string OfferName { get; set; }
        public string DeliveryBy { get; set; }
        public string UserType { get; set; }
        public string IsGSTRegistered { get; set; }
        public string SelectedInvoiceType { get; set; }
        public decimal FpVoucherAmt { get; set; }
        public string FpVoucher { get; set; }
        public decimal CouponAmt { get; set; }
        public string CouponCode { get; set; }
        public string PartyInvoice { get; set; }
        public string EInvoice { get; set; }
        public string AckNo { get; set; }
        public string PANNo { get; set; }
        public string CINNo { get; set; }
        public CustomerDetail Customer { get; set; }
        public ProductTotal Producttotal { get; set; }
        public List<ProductModel> ProductList { get; set; }
        public List<TaxSummary> TaxSummary { get; set; }
        public List<PaymentModeDetail> PaymentMode { get; set; }

    }

    public class ProductTotal  
    {
        public decimal TotalTaxPer { get; set; }
        public decimal TotalPayAmount { get; set; }
        public decimal TotalTaxAmount { get; set; }
        public decimal TotalCGSTAmt { get; set; }
        public decimal TotalSGSTPer { get; set; }
        public decimal TotalSGSTAmt { get; set; }
        public decimal TotalNetPayable { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Roundoff { get; set; }
        public decimal TotalBV { get; set; }
        public decimal TotalRP { get; set; }
        public decimal TotalCGSTPer { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalCV { get; set; }
        public decimal TotalPV { get; set; }
        public decimal TotalQty { get; set; }
        public decimal TotalWeight { get; set; }
        public decimal TotalTotalAmount { get; set; }
    }
}
