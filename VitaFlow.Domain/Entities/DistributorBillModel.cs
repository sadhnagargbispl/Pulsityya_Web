using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VitaFlow.Domain.Entities
{
    public class DistributorBillModel
    {
        public string OrderNo { get; set; }
        public CustomerDetail objCustomer { get; set; }
        public ProductModel objProduct { get; set; }
        public List<ProductModel> objListProduct { get; set; }
        public string objProductListStr { get; set; }
        public ConfigDetails objConfigDetails { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAdd { get; set; }
        public string BillNo { get; set; }
        public string UserBillNo { get; set; }  
        public DateTime BillDate { get; set; }
        public string GSTNo { get; set; }
        public string SoldBy { get; set; }
        public string SoldByName { get; set; }
        public string SoldByAddress { get; set; }
        public string SoldByCity { get; set; }
        public string MobileNo { get; set; }
        public List<TaxSummary> objTaxSummary { get; set; }
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
        public string CustTaxType { get; set; }
        public List<PaymentModeDetail> objPaymentMode { get; set; }
    }

    public class TaxSummary
    {
        //public decimal TaxPer { get; set; }
        //public decimal CGSTPer { get; set; }
        //public decimal SGSTPer { get; set; }
        //public decimal TaxAmt { get; set; }
        //public decimal CGSTAmt { get; set; }
        //public decimal SGSTAmt { get; set; }
        //public decimal Amount { get;set;
        public decimal SumTaxPer { get; set; }
        public decimal SumTaxAmt { get; set; }
        public decimal SumCGSTPer { get; set; }
        public decimal SumCGSTAmt { get; set; }
        public decimal SumSGSTPer { get; set; }
        public decimal SumSGSTAmt { get; set; }
        public decimal SumAmount { get; set; }
        public decimal TotalTaxAmount { get; set; }
        public decimal SumNetPayableAmount { get; set; }
    }
    public class CustomerDetail
    {
        public string IdNo { get; set; }
        public bool IsCustomerBill { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ReferenceIdNo { get; set; }
        public string ReferenceName { get; set; }
        public string Remarks { get; set; }
        public int KitId { get; set; }
        public decimal KitAmount { get; set; }
        public decimal MinBillAmt { get; set; }
        public List<string> InvoiceType { get; set; }
        public string SelectedInvoiceType { get; set; }
        public User UserDetails { get; set; }
        public decimal FormNo { get; set; }
        public bool IsActive { get; set; }

        public decimal StateCode { get; set; }
        public string PartyCode { get; set; }
        public string PartyName { get; set; }

        public bool IsBlock { get; set; }

        public bool IsBillOnMrp { get; set; }
        public bool IsFirstBill { get; set; }
        public decimal MinRepurch { get; set; }
        public string CustId { get; set; }
        public decimal WalletBalance { get; set; }
        public bool IsRegisteredCustomer { get; set; }
        public string CustomerType { get; set; }
        public string CardNo { get; set; }
        public string MobileNo { get; set; }
        public string PANNo { get; set; }
        public string Password { get; set; }
        public string GSTNo { get; set; }
        public decimal MaxBV { get; set; }
        public int NumberOfBill { get; set; }
        public bool isCurrentmonth { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
        public string Doj { get; set; }
        public string UpgradeDate { get; set; }
        public string DistrictName { get; set; }
        public int IsOldID { get; set; }
        public string ElligibleFor { get; set; }
        public string DeliveryAddress { get; set; }
    }
    public class ReferenceModel
    {
        public ResponseDetail objresponse { get; set; }
        public string RefId { get; set; }
        public string RefName { get; set; }
    }
    public class ProductModel
    {
        public decimal CourierCharges { get; set; }
        public decimal TotalWeight { get; set; }
        public string UserName { get; set; }
        public string Ftype { get; set; }
        public string billType { get; set; }
        public string BillSoldBy { get; set; }
        public decimal OrderedOty { get; set; }
        public string PartyName { get; set; }
        public string OrderType { get; set; }
        public decimal? OfferType { get; set; }
        public decimal ProdStateCode { get; set; }
        public string IdNo { get; set; }
        public string Mobileno { get; set; }
        public decimal CatId { get; set; }
        public decimal SubCatId { get; set; }
        public string CatName { get; set; }
        //public string ProductCodeStr { get; set; }
        public int IsCommissionAdd { get; set; }
        public int IsDiscountAdd { get; set; }
        public string ProductName { get; set; }
        public string ProductCodeStr { get; set; }
        public string Barcode { get; set; }
        public string BatchNo { get; set; }
        public decimal? BV { get; set; }
        public decimal? CV { get; set; }
        public decimal? PV { get; set; }
        public decimal? CommissionPer { get; set; }
        public decimal? CommissionAmt { get; set; }
        public decimal? FundPoint { get; set; }
        public string ProductTye { get; set; }
        public decimal? BVValue { get; set; }
        public decimal? CVValue { get; set; }
        public decimal? PVValue { get; set; }
        public decimal? FundPointValue { get; set; }
        public decimal Quantity { get; set; }
        public decimal? FreeQty { get; set; }
        public decimal StockAvailable { get; set; }
        public decimal? DP { get; set; }
        public decimal? DP1 { get; set; }
        public decimal? DiscPer { get; set; }
        public decimal? DiscAmt { get; set; }
        public decimal Amount { get; set; }
        public decimal? TaxPer { get; set; }
        public decimal? TaxAmt { get; set; }
        public decimal OldTaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal? Rate { get; set; }
        public decimal? MRP { get; set; }
        public decimal? RP { get; set; }
        public decimal? RPValue { get; set; }
        public int ProdCode { get; set; }
        public string TaxType { get; set; }
        public string ProductType { get; set; }
        public decimal TotalNetPayable { get; set; }
        public decimal TotalRP { get; set; }
        public decimal TotalPayAmount { get; set; }
        public decimal TotalTaxAmount { get; set; }
        public decimal TotalTaxPer { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalBV { get; set; }
        public decimal TotalCV { get; set; }
        public decimal TotalPV { get; set; }
        public decimal TotalQty { get; set; }
        public decimal TotalTotalAmount { get; set; }
        public decimal Roundoff { get; set; }
        public decimal CashAmount { get; set; }
        public decimal CashDiscPer { get; set; }
        public decimal CashDiscAmount { get; set; }
        public decimal PayableAmount { get; set; }
        public decimal TotalDiscPer { get; set; }
        public decimal TotalCommsonAmt { get; set; }
        public string DeliveryPlace { get; set; }
        public PayDetails PayDetails { get; set; }
        public bool IsExpirable { get; set; }
        public DateTime ExpDate { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal TotalCGSTPer { get; set; }
        public decimal TotalSGSTPer { get; set; }
        public decimal TotalCGSTAmt { get; set; }
        public decimal TotalSGSTAmt { get; set; }
        public string VoucherNo { get; set; }
        public decimal? VoucherAmt { get; set; }
        public decimal? VDiscountAmt { get; set; }
        public string Remarks { get; set; }
        public string UserBillNo { get; set; }
        public DateTime BillDate { get; set; }
        public string UID { get; set; }
        public decimal OfferUID { get; set; }
        public decimal DispQty { get; set; }
        public decimal GSTPer { get; set; }
        public decimal ReturnQty { get; set; }
        public string DispStatus { get; set; }
        public string HSNCode { get; set; }
        public int Row_number { get; set; }
        public decimal OfferProdQty { get; set; }
        public OfferProducts offerDetail { get; set; }
        public int? TFreeQty { get; set; }
        public string Size { get; set; }
        public decimal? Sequence { get; set; }
        public decimal MinToBV { get; set; }
        public string IsAvailableForOffer { get; set; }
        public string IsAvailableForBilling { get; set; }
        public decimal Weight { get; set; }
        public string isFixedQty { get; set; }
        public decimal FixedQty { get; set; }
        public string OrderNo { get; set; }
        public string UOM { get; set; } 
        public List<ProductBatchModel> batchdetail { get; set; }

    }
    public class ProductSearchModel
    {
        public List<BarcodeDetails> objBarcodeList { get; set; }
        public List<ProductModel> objProductList { get; set; }
        public string Barcode { get; set; }
        public string ProductName { get; set; }
        public decimal ProdId { get; set; }
    }
    public class PayDetails
    {
        public string PayMode { get; set; }
        public string PayPrefix { get; set; }
        public bool IsBD { get; set; }
        public bool IsQ { get; set; }
        public bool IsD { get; set; }
        public bool IsCC { get; set; }
        public bool IsV { get; set; }
        public bool IsW { get; set; }
        public bool IsT { get; set; }
        public bool IsP { get; set; }
        public bool IsPV { get; set; }
        public bool IsBV { get; set; }
        public bool IsCU { get; set; }
        public decimal AmountByBD { get; set; }
        public string BDBankName { get; set; }
        public string AccNo { get; set; }
        public string IFSCCode { get; set; }
        public decimal AmountByCheque { get; set; }
        public string ChequeNo { get; set; }
        public DateTime ChequeDate { get; set; }
        public string ChequeDateStr { get; set; }
        public string CHBankName { get; set; }
        public decimal AmountByDD { get; set; }
        public string DDBankName { get; set; }
        public string DDNo { get; set; }
        public DateTime DDDate { get; set; }
        public string DDDateStr { get; set; }
        public decimal AmountByCard { get; set; }
        public string CardNo { get; set; }
        public string CardType { get; set; }
        public decimal AmountByCredit { get; set; }
        public string Narration { get; set; }
        public decimal AvailBal { get; set; }
        public decimal AvailPVBal { get; set; }
        public decimal AvailBVBal { get; set; }
        public decimal RemBal { get; set; }
        public decimal TotalPayDisc { get; set; }
        public decimal UsedDisc { get; set; }
        public decimal RemainDisc { get; set; }
        public decimal AmountByWallet { get; set; }
        public decimal AmountPVWallet { get; set; }
        public decimal AmountBVWallet { get; set; }
        public decimal AmountByVoucher { get; set; }
        public string FpVoucher { get; set; }
        public decimal AmountByPaytm { get; set; }
        public string PaytmTransactionId { get; set; }
        public decimal BankCode { get; set; }
        public string Coupon { get; set; }
        public decimal AmountbyCoupon { get; set; }
        public bool IsPPW { get; set; }
        public decimal AmountByPPW { get; set; }
        public decimal PPWAvailBal { get; set; }
        public decimal PPWRemBal { get; set; }
        public bool IsBPW { get; set; }
        public decimal AmountByBPW { get; set; }
        public decimal BPWAvailBal { get; set; }
        public decimal BPWRemBal { get; set; }
    }

    public class ConfigDetails
    {
        public string C_IsBillOnMRP { get; set; }
        public string C_AllowDiscount { get; set; }
        public string C_DiscForAllCust { get; set; }
        public string C_AddDuplicateProd { get; set; }
        public string C_PrintBill { get; set; }

    }

    public class ResponseDetail
    {
        public int StatusCode { get; set; }
        public string ResponseStatus { get; set; }
        public string ResponseMessage { get; set; }
        public DistributorBillModel ResponseDetailsToPrint { get; set; }
        public string GeneratedOTP { get; set; }
        public List<TrnPartyOrderDetail> OrderConfirmation { get; set; }
        public M_CompanyMaster Comapnydetail { get; set; }
    }

    public class BarcodeDetails
    {
        public int ProductBarcodeId { get; set; }
        public int ProductId { get; set; }
        public string BarcodeType { get; set; }
        public string Barcode { get; set; }
        public String BType { get; set; }
        public string GeneratedBy { get; set; }
        public DateTime GenerateDate { get; set; }
        public decimal PurchaseRate { get; set; }
        public decimal MRP { get; set; }
        public decimal DP { get; set; }
        public string IsExpirable { get; set; }
        public string ExpDateStr { get; set; }
        public string MfgDateStr { get; set; }
        public DateTime? ExpDate { get; set; }
        public DateTime? MfgDate { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
        public string Remarks { get; set; }
        public string IsAdd { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string objProductListStr { get; set; }
        public string ExisitingBarcode { get; set; }
        public string ActiveStatus { get; set; }
        public decimal BarcodeId { get; set; }

    }

    public class OfferProducts
    {
        public decimal offerID { get; set; }
        public string ProdID { get; set; }
        public string ProdName { get; set; }
        public decimal Qty { get; set; }
        public decimal FreeQty { get; set; }
        public string IsFlexible { get; set; }
        public decimal MinBV { get; set; }
        public decimal MinToBV { get; set; }
        public decimal MinAmt { get; set; }
        public decimal TotalQty { get; set; }
        public decimal? OfferMrp { get; set; }
        public string ActiveStatus { get; set; }
        public string offerType { get; set; }
        public string BuyProduct { get; set; }

    }


    public class MemberAPIRoot  
    {
        public string Success { get; set; }
        public string ApiMessage { get; set; }
        public MemberResult Result { get; set; }
    }

    public class MemberResult 
    {
        public string loginid { get; set; }
        public string name { get; set; }
        public string Doj { get; set; }
        public string MobileNo { get; set; }
        public string email { get; set; }
        public string Address { get; set; }
        public string city { get; set; }
        public string Package { get; set; }
        public string ActiveDate { get; set; }
        public string ActiveStatus { get; set; }
        public string MemID { get; set; }
        public string status { get; set; }
        public string State { get; set; }
        public string VOUCHERBAL_Balance { get; set; }
        public string Fpv_Balance { get; set; }
    }

    public class BalanceAPIRoot
    {
        public string Success { get; set; }
        public string Balance { get; set; }
        public BalanceResult Result { get; set; }
    }

    public class BalanceResult
    {
        public string loginid { get; set; }
        public string status { get; set; }
    }

}
