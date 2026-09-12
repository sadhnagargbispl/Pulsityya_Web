using VitaFlow.Domain.Entities;

namespace VitaFlow.Domain.Interface
{
    public interface I_Transaction
    {
        Task<string> SaveWalletRequest(M_WalletRequest objWallet);
        Task<List<PartyOrderModel>> GetOrderList(string OrderBy, string OrderTo, string Status);
        Task<ResponseDetail> RejectFranchiseOrder(string OrderNo, string RejectReason, decimal RejectedByUserId);
        Task<ResponseDetail> RejectWalletRequest(string ReqNo, string RejectReason, decimal RejectedByUserId);

        Task<List<ProductModel>> GetOrderProductList(string OrderNo, string OrderBy, string StockforParty);
        Task<List<BankModel>> GetBankList();
        Task<List<KitDetail>> GetKitIdList();
        Task<List<string>> GetAutocompProductsOnly(string FCode);
        Task<List<string>> GetProductBarcodeOnly(string FCode);
        Task<CustomerDetail> GetCustInfo(string IdNo);
        Task<MemberAPIRoot> ValidateCustomerbyAPI(string IdNo, string Password);
        Task<List<ProductModel>> GetproductInfo(string SearchType, string data, bool isCForm, string BillType, decimal CurrentStateCode, string CurrentPartyCode, bool IsBillOnMrp, string OfferID, bool bvhalf, string Invoice, string IsSpclOffer);
        Task<List<ProductModel>> GetproductInfoBatchWise(string SearchType, string data, bool isCForm, string BillType, decimal CurrentStateCode, string CurrentPartyCode, bool IsBillOnMrp, string OfferID, bool bvhalf, string Invoice, string IsSpclOffer);

        Task<ResponseDetail> SaveDistributorBill(DistributorBillModel objModel);
        Task<FPVoucher> CheckFpVoucher(string Code, string Idno);
        Task<Coupon> CheckCoupon(string Code, string Idno);
        Task<ResponseDetail> SaveDispatchOrder(PartyOrderModel objPartyDispatchOrder);
        Task<DistributorBillModel> getInvoice(string BillNo, string CurrentPartyCode, string id);
        Task<FPVoucher> GetCheckFpWallet(string Idno);
        Task<FPVoucherEligibilityResult> CheckFPVoucherEligibilityAsync(string idno);
        Task<List<string>> GetAvailStockProductNamesOnly(string StockforParty);
        Task<List<PartyModel>> GetAllPartyNew(string LoginPartyCode, decimal LoginStateCode, bool NeedWallet);
        Task<List<string>> GetAllBarcode();
        Task<List<StateModel>> GetStateList();
        Task<List<string>> GetAutocompleteProductNames(string InvType);
        Task<ResponseDetail> CheckBillCustomer(string mobile);
        Task<DistributorBillModel> GetSoldBy(string BillNo, string id); 
    }
}
