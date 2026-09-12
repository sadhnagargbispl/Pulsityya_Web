using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VitaFlow.Domain.Entities;

namespace VitaFlow.Domain.Interface
{
    public interface I_Report
    {
        Task<List<TrnPartyOrderDetail>> GetOrderdetail(TrnPartyOrderDetail req);
        Task<FranchiseLimit> GetFranchiseLimit(string Fcode, int userid);
        Task<List<Product>> GetTopSellingProduct(string Action, string Fcode);
        Task<List<PartyOrderModel>> GetOrderList(string Orderby);
        Task<List<PartyOrderModel>> GetOrderProductList(string Orderby, string OrderNo);
        Task<List<PackageMasterDetail>> GetPackageList(int Groupid);
        Task<PackageMasterDetail> GetPartywisePackage(string Partycode);
        Task<List<Product>> StockProducts(string FCode);
        Task<List<SalesReport>> GetSalesReport(string FromDate, string ToDate, string CustomerId, string ProductCode, string CategoryCode, string PartyCode, string BType, string SalesType, string InvoiceType, string BillNo, string FType, string OfferType, string ReportType);
        Task<List<ProductDetails>> GetAllProducts(decimal CategoryCode);
        Task<List<CategoryDetails>> GetCategoryList(string ActiveFlag);
        Task<List<StockReportModel>> GetStockReport(string CategoryCode, string ProductCode, string PartyCode, bool IsBatchWise, string StockType);
        Task<List<StockReportModel>> GetDateWiseStockReport(string CategoryCode, string ProductCode, string PartyCode, string FromDate, string ToDate);
        Task<List<SalesReport>> GetProductWiseSalesReport(string FromDate, string ToDate, string CustomerId, string ProductCode, string CategoryCode, string PartyCode, string BType, string SalesType, string InvoiceType, string BillNo, string FType, string OfferType, string ReportType);
        Task<List<SalesReport>> GetDateWiseSalesReport(string FromDate, string ToDate, string CustomerId, string ProductCode, string CategoryCode, string PartyCode, string BType, string SalesType, string InvoiceType, string BillNo, string FType, string OfferType, string ReportType);
        Task<List<PaymentMode>> GetPaymodeList();
        Task<List<PaymentSummaryReport>> GetPaymentSummaryReport(string FromDate, string ToDate, string PartyCode, string Type);
        Task<List<SalesReturnReport>> GetSalesReturnReport(string FromDate, string ToDate, string ProductCode, string CategoryCode, string PartyCode, string PartyType, string Type);
        Task<List<StockReportModel>> GetStockReceiptReport(string CategoryCode, string ProductCode, string PartyCode, string StateCode, string FromDate, string ToDate, string LoginPartyCode, string isSummary);
        Task<List<SaleRegister>> GetSalesRegisterReport(string FromDate, string ToDate, string PartyCode, string type);
        Task<List<SalesReport>> GetWalletHistory(string FromDate, string ToDate, string PartyCode, string vtype);
        Task<List<WalletRequest>> GetAllWalletRequest(string datetype, string PartyCode, string FromDate, string ToDate, string Status);
        Task<List<PartyOrderModel>> GetOrderList(string OrderBy, string OrderTo, string Status);
        Task<List<ProductModel>> GetOrderProduct(string OrderNo, string OrderBy);
        Task<List<FranchiseeCommission>> GetFranchiseeBVCommission(string FromDate, string ToDate, string code, string Billtype);
        Task<List<MonthWiseIncome>> GetMonthWiseIncome(string Sessid ,string PartyCode);
        Task<List<MSessids>> GetSessids();
        Task<List<MPerformanceInc>> GetPerformanceInc(string Partycode,string Action,int SessID);
        Task<M_IncentiveStatement> GetIncentiveStatement(string Partycode, string StatementPeriod);
    }
}
