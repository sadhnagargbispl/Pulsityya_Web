using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VitaFlow.Application.IServices;
using VitaFlow.Domain.Entities;
using VitaFlow.Domain.Interface;

namespace VitaFlow.Application.Services
{
    public class TransactionServices : I_Transaction_Services
    {
        private readonly I_Transaction i_Transaction;

        public TransactionServices(I_Transaction i_Transaction)
        {
            this.i_Transaction = i_Transaction;
        }

        public Task<string> SaveWalletRequest(M_WalletRequest objWallet)
        {
            return i_Transaction.SaveWalletRequest(objWallet);
        }
        public Task<List<PartyOrderModel>> GetOrderList(string OrderBy, string OrderTo, string Status)
        {
            return i_Transaction.GetOrderList(OrderBy, OrderTo, Status);
        }
        public Task<ResponseDetail> RejectFranchiseOrder(string OrderNo, string RejectReason, decimal RejectedByUserId)
        {
            return i_Transaction.RejectFranchiseOrder(OrderNo, RejectReason, RejectedByUserId);
        }
        public Task<ResponseDetail> RejectWalletRequest(string ReqNo, string RejectReason, decimal RejectedByUserId)
        {

            return i_Transaction.RejectWalletRequest(ReqNo, RejectReason, RejectedByUserId);
        }
        public Task<List<ProductModel>> GetOrderProductList(string OrderNo, string OrderBy, string StockforParty)
        {
            return i_Transaction.GetOrderProductList(OrderNo, OrderBy, StockforParty);
        }
        public Task<List<BankModel>> GetBankList()
        {
            return i_Transaction.GetBankList();
        }
        public Task<List<KitDetail>> GetKitIdList()
        {
            return i_Transaction.GetKitIdList();
        }
        public Task<List<string>> GetAutocompProductsOnly(string FCode)
        {
            return i_Transaction.GetAutocompProductsOnly(FCode);
        }
        public Task<List<string>> GetProductBarcodeOnly(string FCode)
        {
            return i_Transaction.GetProductBarcodeOnly(FCode);
        }
        public Task<CustomerDetail> GetCustInfo(string IdNo)
        {
            return i_Transaction.GetCustInfo(IdNo);
        }
        public Task<MemberAPIRoot> ValidateCustomerbyAPI(string IdNo, string Password)
        {
            return i_Transaction.ValidateCustomerbyAPI(IdNo, Password);
        }
        public Task<List<ProductModel>> GetproductInfo(string SearchType, string data, bool isCForm, string BillType, decimal CurrentStateCode, string CurrentPartyCode, bool IsBillOnMrp, string OfferID, bool bvhalf, string Invoice, string IsSpclOffer)
        {
            return i_Transaction.GetproductInfo(SearchType, data, isCForm, BillType, CurrentStateCode, CurrentPartyCode, IsBillOnMrp, OfferID, bvhalf, Invoice, IsSpclOffer);
        }
        public Task<List<ProductModel>> GetproductInfoBatchWise(string SearchType, string data, bool isCForm, string BillType, decimal CurrentStateCode, string CurrentPartyCode, bool IsBillOnMrp, string OfferID, bool bvhalf, string Invoice, string IsSpclOffer)
        {
            return i_Transaction.GetproductInfoBatchWise(SearchType, data, isCForm, BillType, CurrentStateCode, CurrentPartyCode, IsBillOnMrp, OfferID, bvhalf, Invoice, IsSpclOffer);
        }
        public Task<ResponseDetail> SaveDistributorBill(DistributorBillModel objModel)
        {
            return i_Transaction.SaveDistributorBill(objModel);
        }
        public Task<FPVoucher> CheckFpVoucher(string Code, string Idno)
        {
            return i_Transaction.CheckFpVoucher(Code, Idno);
        }
        public Task<Coupon> CheckCoupon(string Code, string Idno)
        {
            return i_Transaction.CheckCoupon(Code, Idno);
        }
        public Task<ResponseDetail> SaveDispatchOrder(PartyOrderModel objPartyDispatchOrder)
        {
            return i_Transaction.SaveDispatchOrder(objPartyDispatchOrder);
        }
        public Task<DistributorBillModel> getInvoice(string BillNo, string CurrentPartyCode, string id)
        {
            return i_Transaction.getInvoice(BillNo, CurrentPartyCode, id);
        }
        public Task<FPVoucher> GetCheckFpWallet(string Idno)
        {
            return i_Transaction.GetCheckFpWallet(Idno);
        }
        public Task<FPVoucherEligibilityResult> CheckFPVoucherEligibilityAsync(string idno)
        {
            return i_Transaction.CheckFPVoucherEligibilityAsync(idno);
        }
        public Task<List<string>> GetAvailStockProductNamesOnly(string StockforParty)
        {
            return i_Transaction.GetAvailStockProductNamesOnly(StockforParty);
        }
        public Task<List<PartyModel>> GetAllPartyNew(string LoginPartyCode, decimal LoginStateCode, bool NeedWallet)
        {
            return i_Transaction.GetAllPartyNew(LoginPartyCode, LoginStateCode, NeedWallet);
        }
        public Task<List<string>> GetAllBarcode()
        {
            return i_Transaction.GetAllBarcode();
        }
        public Task<List<StateModel>> GetStateList()
        {
            return i_Transaction.GetStateList();
        }
        public Task<List<string>> GetAutocompleteProductNames(string InvType)
        {
            return i_Transaction.GetAutocompleteProductNames(InvType);
        }

        public Task<ResponseDetail> CheckBillCustomer(string mobile)
        {
            return i_Transaction.CheckBillCustomer(mobile);
        }
        public Task<DistributorBillModel> GetSoldBy(string BillNo, string id)
        {
            return i_Transaction.GetSoldBy(BillNo,id);
        }
    }
}
