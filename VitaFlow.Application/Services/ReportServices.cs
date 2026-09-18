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
    public class ReportServices : I_Report_Service
    {
        private readonly I_Report i_Report;
        public ReportServices(I_Report ireport)
        {
            i_Report = ireport;
        }
        public Task<List<TrnPartyOrderDetail>> GetOrderdetail(TrnPartyOrderDetail req)
        {
            return i_Report.GetOrderdetail(req);
        }
        public Task<FranchiseLimit> GetFranchiseLimit(string Fcode, int userid)
        {
            return i_Report.GetFranchiseLimit(Fcode, userid);
        }
        public Task<List<Product>> GetTopSellingProduct(string Action, string Fcode)
        {
            return i_Report.GetTopSellingProduct(Action, Fcode);
        }
        public Task<List<PartyOrderModel>> GetOrderList(string Orderby)
        {
            return i_Report.GetOrderList(Orderby);
        }
        public Task<List<PartyOrderModel>> GetOrderProductList(string Orderby, string OrderNo)
        {
            return i_Report.GetOrderProductList(Orderby, OrderNo);
        }
        public Task<List<PackageMasterDetail>> GetPackageList(int Groupid)
        {
            return i_Report.GetPackageList(Groupid);
        }
        public Task<PackageMasterDetail> GetPartywisePackage(string Partycode)
        {
            return i_Report.GetPartywisePackage(Partycode);
        }
        public Task<List<Product>> StockProducts(string FCode)
        {
            return i_Report.StockProducts(FCode);
        }
        public Task<List<SalesReport>> GetSalesReport(string FromDate, string ToDate, string CustomerId, string ProductCode, string CategoryCode, string PartyCode, string BType, string SalesType, string InvoiceType, string BillNo, string FType, string OfferType, string ReportType)
        {
            return i_Report.GetSalesReport(FromDate, ToDate, CustomerId, ProductCode, CategoryCode, PartyCode, BType, SalesType, InvoiceType, BillNo, FType, OfferType, ReportType);
        }

        public Task<List<SaleRegister>> GetSalesRegisterReport(string FromDate, string ToDate, string PartyCode, string type)
        {
            return i_Report.GetSalesRegisterReport(FromDate, ToDate, PartyCode, type);
        }
        public Task<List<WalletRequest>> GetAllWalletRequest(string datetype, string PartyCode, string FromDate, string ToDate, string Status)
        {
            return i_Report.GetAllWalletRequest(datetype, PartyCode, FromDate, ToDate, Status);
        }
        public Task<List<ProductDetails>> GetAllProducts(decimal CategoryCode)
        {
            return i_Report.GetAllProducts(CategoryCode);
        }
        public Task<List<CategoryDetails>> GetCategoryList(string ActiveFlag)
        {
            return i_Report.GetCategoryList(ActiveFlag);
        }
        public Task<List<StockReportModel>> GetStockReport(string CategoryCode, string ProductCode, string PartyCode, bool IsBatchWise, string StockType)
        {
            return i_Report.GetStockReport(CategoryCode, ProductCode, PartyCode, IsBatchWise, StockType);
        }
        public Task<List<StockReportModel>> GetDateWiseStockReport(string CategoryCode, string ProductCode, string PartyCode, string FromDate, string ToDate)
        {
            return i_Report.GetDateWiseStockReport(CategoryCode, ProductCode, PartyCode, FromDate, ToDate);
        }
        public Task<List<SalesReport>> GetProductWiseSalesReport(string FromDate, string ToDate, string CustomerId, string ProductCode, string CategoryCode, string PartyCode, string BType, string SalesType, string InvoiceType, string BillNo, string FType, string OfferType, string ReportType)
        {
            return i_Report.GetSalesReport(FromDate, ToDate, CustomerId, ProductCode, CategoryCode, PartyCode, BType, SalesType, InvoiceType, BillNo, FType, OfferType, ReportType);
        }
        public Task<List<SalesReport>> GetDateWiseSalesReport(string FromDate, string ToDate, string CustomerId, string ProductCode, string CategoryCode, string PartyCode, string BType, string SalesType, string InvoiceType, string BillNo, string FType, string OfferType, string ReportType)
        {
            return i_Report.GetDateWiseSalesReport(FromDate, ToDate, CustomerId, ProductCode, CategoryCode, PartyCode, BType, SalesType, InvoiceType, BillNo, FType, OfferType, ReportType);
        }
        public Task<List<PaymentMode>> GetPaymodeList()
        {
            return i_Report.GetPaymodeList();
        }
        public Task<List<PaymentSummaryReport>> GetPaymentSummaryReport(string FromDate, string ToDate, string PartyCode, string Type)
        {
            return i_Report.GetPaymentSummaryReport(FromDate, ToDate, PartyCode, Type);
        }
        public Task<List<SalesReturnReport>> GetSalesReturnReport(string FromDate, string ToDate, string ProductCode, string CategoryCode, string PartyCode, string PartyType, string Type)
        {
            return i_Report.GetSalesReturnReport(FromDate, ToDate, ProductCode, CategoryCode, PartyCode, PartyType, Type);
        }
        public Task<List<StockReportModel>> GetStockReceiptReport(string CategoryCode, string ProductCode, string PartyCode, string StateCode, string FromDate, string ToDate, string LoginPartyCode, string isSummary)
        {
            return i_Report.GetStockReceiptReport(CategoryCode, ProductCode, PartyCode, StateCode, FromDate, ToDate, LoginPartyCode, isSummary);
        }
        public Task<List<SalesReport>> GetWalletHistory(string FromDate, string ToDate, string PartyCode, string vtype)
        {
            return i_Report.GetWalletHistory(FromDate, ToDate, PartyCode, vtype);
        }
        public Task<List<PartyOrderModel>> GetOrderList(string OrderBy, string OrderTo, string Status)
        {
            return i_Report.GetOrderList(OrderBy, OrderTo, Status);
        }
        public Task<List<ProductModel>> GetOrderProduct(string OrderNo, string OrderBy)
        {
            return i_Report.GetOrderProduct(OrderNo, OrderBy);
        }
        public Task<List<FranchiseeCommission>> GetFranchiseeBVCommission(string FromDate, string ToDate, string code, string Billtype)
        {
            return i_Report.GetFranchiseeBVCommission(FromDate, ToDate, code, Billtype);
        }
        public Task<List<MonthWiseIncome>> GetMonthWiseIncome(string Sessid, string PartyCode)
        {
            return i_Report.GetMonthWiseIncome(Sessid, PartyCode);
        }
        public Task<List<MSessids>> GetSessids()
        {
            return i_Report.GetSessids();
        }
        public Task<List<MPerformanceInc>> GetPerformanceInc(string Partycode, string Action,int SessID)
        {
            return i_Report.GetPerformanceInc(Partycode, Action,SessID);
        }
        public Task<M_IncentiveStatement> GetIncentiveStatement(string Partycode, string StatementPeriod)
        {
            return i_Report.GetIncentiveStatement(Partycode, StatementPeriod);
        }
    }
}
