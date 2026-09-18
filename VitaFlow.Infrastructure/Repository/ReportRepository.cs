using Dapper;
using Microsoft.IdentityModel.Tokens;
using Microsoft.SqlServer.Server;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using VitaFlow.Domain.Entities;
using VitaFlow.Domain.Interface;
using VitaFlow.Infrastructure.DapperContext;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace VitaFlow.Infrastructure.Repository
{
    public class ReportRepository : I_Report
    {
        private readonly DapperDbContext _context;

        public ReportRepository(DapperDbContext dapperContext)
        {
            _context = dapperContext;
        }
        public async Task<List<TrnPartyOrderDetail>> GetOrderdetail(TrnPartyOrderDetail req)
        {
            List<TrnPartyOrderDetail> obj = new List<TrnPartyOrderDetail>();
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    var storedProcedureName = "GetOrderDetail";
                    var values = new
                    {
                        @Action = "Confirmdetail",
                        @UserId = req.UserId,
                        @orderNo = req.OrderNo
                    };
                    obj = (await connection.QueryAsync<TrnPartyOrderDetail>(storedProcedureName, values, commandType: CommandType.StoredProcedure)).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return obj;
        }

        public async Task<FranchiseLimit> GetFranchiseLimit(string Fcode, int userid)
        {
            FranchiseLimit obj = new FranchiseLimit();
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    var storedProcedureName = "GetLimit";
                    var values = new
                    {
                        @Fcode = Fcode,
                        @UserId = userid
                    };
                    obj = (await connection.QueryAsync<FranchiseLimit>(storedProcedureName, values, commandType: CommandType.StoredProcedure)).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

            }
            return obj;
        }
        public async Task<List<clientProduct>> GetTopClientsProduct(string Action, string Fcode)
        {
            List<clientProduct> obj = new List<clientProduct>();
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    var storedProcedureName = "Sp_GetTopclientsProduct";
                    var values = new
                    {
                        @Action = Action,
                        @FCode = Fcode
                    };
                    var res = (await connection.QueryAsync<clientProduct>(storedProcedureName, values, commandType: CommandType.StoredProcedure)).ToList();
                    if (res != null)
                    {
                        obj = res;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return obj;
        }
        public async Task<List<Product>> GetTopSellingProduct(string Action, string Fcode)
        {
            List<Product> obj = new List<Product>();
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    var storedProcedureName = "Sp_GetTopSellingProduct";
                    var values = new
                    {
                        @Action = Action,
                        @FCode = Fcode
                    };
                    var res = (await connection.QueryAsync<Product>(storedProcedureName, values, commandType: CommandType.StoredProcedure)).ToList();
                    if (res != null)
                    {
                        obj = res;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return obj;
        }
        public async Task<List<PartyOrderModel>> GetOrderList(string Orderby)
        {
            List<PartyOrderModel> obj = new List<PartyOrderModel>();
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    var storedProcedureName = "GetPartyOrderlist";
                    var values = new
                    {
                        @FCode = Orderby
                    };
                    var res = (await connection.QueryAsync<PartyOrderModel>(storedProcedureName, values, commandType: CommandType.StoredProcedure)).ToList();
                    if (res != null)
                    {
                        obj = res;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return obj;
        }
        public async Task<List<PartyOrderModel>> GetOrderProductList(string Orderby, string OrderNo)
        {
            List<PartyOrderModel> obj = new List<PartyOrderModel>();
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    var storedProcedureName = "PartyOrderDetail";
                    var values = new
                    {
                        @OrderNo = OrderNo,
                        @OrderBy = Orderby
                    };
                    var res = (await connection.QueryAsync<PartyOrderModel>(storedProcedureName, values, commandType: CommandType.StoredProcedure)).ToList();
                    if (res != null)
                    {
                        obj = res;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return obj;
        }
        public async Task<List<PackageMasterDetail>> GetPackageList(int Groupid)
        {
            List<PackageMasterDetail> obj = new List<PackageMasterDetail>();
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    var storedProcedureName = "Sp_GetPackageList";
                    var values = new
                    {
                        @Groupid = Groupid
                    };
                    var res = (await connection.QueryAsync<PackageMasterDetail>(storedProcedureName, values, commandType: CommandType.StoredProcedure)).ToList();
                    if (res != null)
                    {
                        obj = res;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return obj;
        }
        public async Task<PackageMasterDetail> GetPartywisePackage(string Partycode)
        {
            PackageMasterDetail obj = new PackageMasterDetail();
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    var storedProcedureName = "Sp_GetPartywisePackage";
                    var values = new
                    {
                        @PartyCode = Partycode
                    };
                    var res = (await connection.QueryAsync<PackageMasterDetail>(storedProcedureName, values, commandType: CommandType.StoredProcedure)).FirstOrDefault();
                    if (res != null)
                    {
                        obj = res;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return obj;
        }
        public async Task<List<Product>> StockProducts(string FCode)
        {
            List<Product> obj = new List<Product>();
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    var storedProcedureName = "Sp_StockProducts";
                    var values = new { @FCode = FCode };
                    obj = (await connection.QueryAsync<Product>(storedProcedureName, values, commandType: CommandType.StoredProcedure)).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return obj;
        }
        public async Task<List<SalesReport>> GetSalesReport(string FromDate, string ToDate, string CustomerId, string ProductCode, string CategoryCode, string PartyCode, string BType, string SalesType, string InvoiceType, string BillNo, string FType, string OfferType, string ReportType)
        {
            List<SalesReport> objListSales = new List<SalesReport>();

            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    if (string.IsNullOrEmpty(InvoiceType))
                    {
                        InvoiceType = "";
                    }

                    DateTime startDate = DateTime.Now;
                    if (!string.IsNullOrEmpty(FromDate) && FromDate != "All")
                    {
                        startDate = Convert.ToDateTime(FromDate);
                    }

                    DateTime endDate = DateTime.Now;
                    if (!string.IsNullOrEmpty(ToDate) && ToDate != "All")
                    {
                        endDate = Convert.ToDateTime(ToDate);
                    }
                    if (FromDate.ToLower() == "all" || string.IsNullOrEmpty(FromDate))
                        startDate = DateTime.Now.AddYears(-5);
                    if (ToDate.ToLower() == "all" || string.IsNullOrEmpty(ToDate))
                        endDate = DateTime.Now;
                    var storedProcedureName = "sp_BillWiseSaleSummary";
                    var values = new
                    {
                        @StartDate = startDate,
                        @EndDate = endDate,
                        @PartyCode = PartyCode,
                        @Invoicetype = InvoiceType,
                        @Idno = CustomerId,
                        @OfferId = 0
                    };
                    objListSales = (await connection.QueryAsync<SalesReport>(storedProcedureName, values, commandType: CommandType.StoredProcedure)).ToList();

                }
            }
            catch (Exception ex)
            {

            }
            return objListSales;
        }
        public async Task<List<ProductDetails>> GetAllProducts(decimal CategoryCode)
        {
            List<ProductDetails> objProduct = new List<ProductDetails>();
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    string query;
                    if (CategoryCode == 0)
                    {
                        query = @"
                                SELECT CatId AS CategoryId, 
                                ProdId AS ProductCodeStr, 
                                ProductName
                                FROM M_ProductMaster
                                WHERE ActiveStatus = 'Y'";
                    }
                    else
                    {
                        query = @"
                                SELECT CatId AS CategoryId, 
                                ProdId AS ProductCodeStr, 
                                ProductName
                                FROM M_ProductMaster
                                WHERE ActiveStatus = 'Y' AND CatId = @CategoryCode";
                    }

                    objProduct = (await connection.QueryAsync<ProductDetails>(query, new { CategoryCode })).ToList();
                }

            }
            catch (Exception ex)
            {

            }
            return objProduct;
        }

        public async Task<List<CategoryDetails>> GetCategoryList(string ActiveFlag)
        {
            List<CategoryDetails> objCategoryList = new List<CategoryDetails>();
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    string query;
                    if (!string.IsNullOrEmpty(ActiveFlag))
                    {
                        query = @"
                                 SELECT CatId AS CategoryId, 
                                 CatName AS CategoryName, 
                                 CatDescription AS Description,
                                 CASE WHEN ActiveStatus = 'Y' THEN 1 ELSE 0 END AS IsActive
                                  FROM M_CatMaster
                                  WHERE ActiveStatus = @ActiveFlag";

                        objCategoryList = (await connection.QueryAsync<CategoryDetails>(query, new { ActiveFlag })).ToList();
                    }
                    else
                    {
                        query = @"
                       SELECT CatId AS CategoryId, 
                       CatName AS CategoryName, 
                       CatDescription AS Description,
                       CASE WHEN ActiveStatus = 'Y' THEN 1 ELSE 0 END AS IsActive
                       FROM M_CatMaster";

                        objCategoryList = (await connection.QueryAsync<CategoryDetails>(query)).ToList();
                    }
                }
            }
            catch
            {

            }
            return objCategoryList;
        }


        public async Task<List<SaleRegister>> GetSalesRegisterReport(string FromDate, string ToDate, string PartyCode, string type)
        {
            List<SaleRegister> objStockModel = new List<SaleRegister>();
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {



                    DateTime startDate = DateTime.Now;
                    if (!string.IsNullOrEmpty(FromDate) && FromDate != "All")
                    {
                        startDate = Convert.ToDateTime(FromDate);
                    }

                    DateTime endDate = DateTime.Now;
                    if (!string.IsNullOrEmpty(ToDate) && ToDate != "All")
                    {
                        endDate = Convert.ToDateTime(ToDate);
                    }
                    if (FromDate.ToLower() == "all" || string.IsNullOrEmpty(FromDate))
                        startDate = DateTime.Now.AddYears(-5);
                    if (ToDate.ToLower() == "all" || string.IsNullOrEmpty(ToDate))
                        endDate = DateTime.Now;

                    if (type == "Invoice")
                    {
                        var storedProcedureName = "sp_InvoicewiseSaleRegiter";
                        var values = new
                        {

                            @PartyCode = PartyCode,
                            @StartDate = startDate,
                            @EndDate = endDate
                        };
                        objStockModel = (await connection.QueryAsync<SaleRegister>(storedProcedureName, values, commandType: CommandType.StoredProcedure)).ToList();
                    }
                    else
                    {
                        var storedProcedureName = "sp_ProdcutwiseSaleRegiter";
                        var values = new
                        {
                            @PartyCode = PartyCode,
                            @FromDate = startDate,
                            @ToDate = endDate
                        };
                        objStockModel = (await connection.QueryAsync<SaleRegister>(storedProcedureName, values, commandType: CommandType.StoredProcedure)).ToList();
                    }

                }
            }
            catch (Exception ex)
            {

            }
            return objStockModel;
        }

        public async Task<List<WalletRequest>> GetAllWalletRequest(string datetype, string PartyCode, string FromDate, string ToDate, string Status)
        {
            List<WalletRequest> objWalletRequest = new List<WalletRequest>();
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {



                    DateTime startDate = DateTime.Now;
                    if (string.IsNullOrEmpty(FromDate))
                    {
                        startDate = DateTime.Now.AddYears(-5);
                    }
                    if (!string.IsNullOrEmpty(FromDate) && FromDate != "All")
                    {
                        startDate = Convert.ToDateTime(FromDate);
                    }

                    DateTime endDate = DateTime.Now;
                    if (string.IsNullOrEmpty(ToDate))
                    {
                        endDate = DateTime.Now;
                    }
                    if (!string.IsNullOrEmpty(ToDate) && ToDate != "All")
                    {
                        endDate = Convert.ToDateTime(ToDate);
                    }
                    //if (FromDate.ToLower() == "all" || string.IsNullOrEmpty(FromDate))
                    //    startDate = DateTime.Now.AddYears(-5);
                    //if (ToDate.ToLower() == "all" || string.IsNullOrEmpty(ToDate))
                    //    endDate = DateTime.Now;


                    var storedProcedureName = "sp_WalletRequestReport";
                    var values = new
                    {
                        @dateType = datetype,
                        @PartyCode = PartyCode,
                        @fromDate = startDate,
                        @Todate = endDate,
                        @Status = Status
                    };
                    objWalletRequest = (await connection.QueryAsync<WalletRequest>(storedProcedureName, values, commandType: CommandType.StoredProcedure)).ToList();

                }
            }
            catch (Exception ex)
            {

            }
            return objWalletRequest;
        }


        public async Task<List<StockReportModel>> GetStockReport(string CategoryCode, string ProductCode, string PartyCode, bool IsBatchWise, string StockType)
        {
            List<StockReportModel> objStockModel = new List<StockReportModel>();
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    if (IsBatchWise == true)
                    {
                        var storedProcedureName = "sp_CurrentStockBatchWiseDetail";
                        var values = new
                        {
                            @CatId = CategoryCode,
                            @ProductId = ProductCode,
                            @PartyCode = PartyCode,
                            @type = StockType
                        };
                        objStockModel = (await connection.QueryAsync<StockReportModel>(storedProcedureName, values, commandType: CommandType.StoredProcedure)).ToList();
                    }
                    else
                    {
                        var storedProcedureName = "sp_CurrentStockWithoutBatchDetail";
                        var values = new
                        {
                            @CatId = CategoryCode,
                            @ProductId = ProductCode,
                            @PartyCode = PartyCode,
                            @type = StockType
                        };
                        objStockModel = (await connection.QueryAsync<StockReportModel>(storedProcedureName, values, commandType: CommandType.StoredProcedure)).ToList();
                    }

                }
            }
            catch (Exception ex)
            {

            }
            return objStockModel;
        }

        public async Task<List<StockReportModel>> GetDateWiseStockReport(string CategoryCode, string ProductCode, string PartyCode, string FromDate, string ToDate)
        {
            List<StockReportModel> objStockModel = new List<StockReportModel>();
            decimal CatId = 0;
            decimal ProdCode = 0;
            try
            {
                DateTime startDate = DateTime.Now;
                if (!string.IsNullOrEmpty(FromDate))
                {
                    startDate = Convert.ToDateTime(FromDate);
                }

                DateTime endDate = DateTime.Now;
                if (!string.IsNullOrEmpty(ToDate))
                {
                    endDate = Convert.ToDateTime(ToDate);
                }

                if (!string.IsNullOrEmpty(ProductCode))
                {
                    ProdCode = decimal.Parse(ProductCode);
                }
                using (var connection = _context.CreateLiveconnInv())
                {
                    var storedProcedureName = "StockDetail";
                    var values = new
                    {
                        @PartyCode = PartyCode,
                        @FromDate = startDate,
                        @ToDate = endDate,

                    };
                    objStockModel = (await connection.QueryAsync<StockReportModel>(storedProcedureName, values, commandType: CommandType.StoredProcedure)).ToList();

                    if (ProdCode != 0)
                    {
                        objStockModel = objStockModel.Where(r => r.ProductCode == ProductCode).ToList();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return objStockModel;
        }
        public async Task<List<SalesReport>> GetProductWiseSalesReport(string FromDate, string ToDate, string CustomerId, string ProductCode, string CategoryCode, string PartyCode, string BType, string SalesType, string InvoiceType, string BillNo, string FType, string OfferType, string ReportType)
        {
            List<SalesReport> objListSales = new List<SalesReport>();

            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    if (string.IsNullOrEmpty(InvoiceType))
                    {
                        InvoiceType = "";
                    }

                    DateTime startDate = DateTime.Now;
                    if (!string.IsNullOrEmpty(FromDate) && FromDate != "All")
                    {
                        startDate = Convert.ToDateTime(FromDate);
                    }

                    DateTime endDate = DateTime.Now;
                    if (!string.IsNullOrEmpty(ToDate) && ToDate != "All")
                    {
                        endDate = Convert.ToDateTime(ToDate);
                    }
                    if (FromDate.ToLower() == "all" || string.IsNullOrEmpty(FromDate))
                        startDate = DateTime.Now.AddYears(-5);
                    if (ToDate.ToLower() == "all" || string.IsNullOrEmpty(ToDate))
                        endDate = DateTime.Now;
                    var storedProcedureName = "sp_ProductWiseSaleReport";
                    var values = new
                    {
                        @Startdate = startDate,
                        @EndDate = endDate,
                        @PartyCode = PartyCode,
                        @Catid = CategoryCode,
                        @Prodid = ProductCode,

                    };
                    objListSales = (await connection.QueryAsync<SalesReport>(storedProcedureName, values, commandType: CommandType.StoredProcedure)).ToList();

                }
            }
            catch (Exception ex)
            {

            }
            return objListSales;
        }

        public async Task<List<SalesReport>> GetDateWiseSalesReport(string FromDate, string ToDate, string CustomerId, string ProductCode, string CategoryCode, string PartyCode, string BType, string SalesType, string InvoiceType, string BillNo, string FType, string OfferType, string ReportType)
        {
            List<SalesReport> objListSales = new List<SalesReport>();
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {

                    DateTime startDate = DateTime.Now;
                    if (!string.IsNullOrEmpty(FromDate) && FromDate != "All")
                    {
                        startDate = Convert.ToDateTime(FromDate);
                    }

                    DateTime endDate = DateTime.Now;
                    if (!string.IsNullOrEmpty(ToDate) && ToDate != "All")
                    {
                        endDate = Convert.ToDateTime(ToDate);
                    }
                    if (FromDate.ToLower() == "all" || string.IsNullOrEmpty(FromDate))
                        startDate = DateTime.Now.AddYears(-5);
                    if (ToDate.ToLower() == "all" || string.IsNullOrEmpty(ToDate))
                        endDate = DateTime.Now;
                    var storedProcedureName = "sp_DateWiseSaleReport";
                    var values = new
                    {
                        @Startdate = startDate,
                        @EndDate = endDate,
                        @PartyCode = PartyCode,
                    };
                    objListSales = (await connection.QueryAsync<SalesReport>(storedProcedureName, values, commandType: CommandType.StoredProcedure)).ToList();

                }
            }
            catch (Exception ex)
            {

            }
            return objListSales;
        }

        public async Task<List<PaymentMode>> GetPaymodeList()
        {
            List<PaymentMode> paymode = new List<PaymentMode>();
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    string query = @"
                                   SELECT 
                                   PayMode AS payMode,
                                   Prefix AS prefix
                                   FROM 
                                   M_PayModeMaster
                                   WHERE 
                                   IsShow = @IsShow";
                    paymode = (await connection.QueryAsync<PaymentMode>(query, new { IsShow = "Y" })).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return (paymode);
        }
        public async Task<List<PaymentSummaryReport>> GetPaymentSummaryReport(string FromDate, string ToDate, string PartyCode, string Type)
        {
            List<PaymentSummaryReport> objReport = new List<PaymentSummaryReport>();
            string WhereCondition = string.Empty;
            string Fld = string.Empty;
            try
            {
                DateTime startDate = DateTime.Now;
                if (!string.IsNullOrEmpty(FromDate) && FromDate != "All")
                {
                    startDate = Convert.ToDateTime(FromDate);
                }

                DateTime endDate = DateTime.Now;
                if (!string.IsNullOrEmpty(ToDate) && ToDate != "All")
                {
                    endDate = Convert.ToDateTime(ToDate);
                }
                if (FromDate.ToLower() == "all" || string.IsNullOrEmpty(FromDate))
                    startDate = DateTime.Now.AddYears(-5);
                if (ToDate.ToLower() == "all" || string.IsNullOrEmpty(ToDate))
                    endDate = DateTime.Now;

                using (var connection = _context.CreateLiveconnInv())
                {
                    var storedProcedureName = "sp_PaymentSummaryReport";
                    var values = new
                    {
                        @PartyCode = PartyCode,
                        @Fromdate = startDate,
                        @ToDate = endDate,

                        @type = Type
                    };
                    objReport = (await connection.QueryAsync<PaymentSummaryReport>(storedProcedureName, values, commandType: CommandType.StoredProcedure)).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return objReport;
        }

        public async Task<List<SalesReturnReport>> GetSalesReturnReport(string FromDate, string ToDate, string ProductCode, string CategoryCode, string PartyCode, string PartyType, string Type)
        {
            List<SalesReturnReport> objReport = new List<SalesReturnReport>();
            string WhereCondition = string.Empty;
            string Fld = string.Empty;
            try
            {
                DateTime startDate = DateTime.Now;
                if (!string.IsNullOrEmpty(FromDate) && FromDate != "All")
                {
                    startDate = Convert.ToDateTime(FromDate);
                }

                DateTime endDate = DateTime.Now;
                if (!string.IsNullOrEmpty(ToDate) && ToDate != "All")
                {
                    endDate = Convert.ToDateTime(ToDate);
                }
                if (FromDate.ToLower() == "all" || string.IsNullOrEmpty(FromDate))
                    startDate = DateTime.Now.AddYears(-5);
                if (ToDate.ToLower() == "all" || string.IsNullOrEmpty(ToDate))
                    endDate = DateTime.Now;

                using (var connection = _context.CreateLiveconnInv())
                {
                    var storedProcedureName = "sp_SalesReturnReport";
                    var values = new
                    {
                        @fromDate = startDate,
                        @Todate = endDate,
                        @PartyCode = PartyCode,
                    };
                    objReport = (await connection.QueryAsync<SalesReturnReport>(storedProcedureName, values, commandType: CommandType.StoredProcedure)).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return objReport;
        }

        public async Task<List<StockReportModel>> GetStockReceiptReport(string CategoryCode, string ProductCode, string PartyCode, string StateCode, string FromDate, string ToDate, string LoginPartyCode, string isSummary)
        {
            List<StockReportModel> objStockModel = new List<StockReportModel>();
            try
            {
                decimal CatId = 0;
                string ProdCode = "0";
                decimal StCode = 0;
                string PCode = "All";
                DateTime StartDate = DateTime.Now;
                DateTime EndDate = DateTime.Now;

                if (!string.IsNullOrEmpty(CategoryCode))
                {
                    CatId = decimal.Parse(CategoryCode);
                }
                if (!string.IsNullOrEmpty(ProductCode))
                {
                    ProdCode = ProductCode;

                }
                if (!string.IsNullOrEmpty(StateCode))
                {
                    StCode = decimal.Parse(StateCode);
                }
                if (!string.IsNullOrEmpty(PartyCode))
                {
                    PCode = PartyCode;
                    if (PartyCode == "0")
                    {
                        PCode = "All";
                    }
                }

                if (!string.IsNullOrEmpty(FromDate) && FromDate != "All")
                {
                    StartDate = Convert.ToDateTime(FromDate);
                }

                if (!string.IsNullOrEmpty(ToDate) && ToDate != "All")
                {
                    EndDate = Convert.ToDateTime(ToDate);
                }

                if (FromDate.ToLower() == "all" || string.IsNullOrEmpty(FromDate))
                    StartDate = DateTime.Now.AddYears(-5);
                if (ToDate.ToLower() == "all" || string.IsNullOrEmpty(ToDate))
                    EndDate = DateTime.Now;

                using (var connection = _context.CreateLiveconnInv())
                {
                    var storedProcedureName = "sp_StockTransactionReport";
                    var values = new
                    {
                        @PartyCode = PartyCode,
                        @FromDate = StartDate,
                        @Todate = EndDate,
                        @Catid = CatId,
                        @Prodid = ProdCode,
                        @type = isSummary.ToUpper()
                    };
                    objStockModel = (await connection.QueryAsync<StockReportModel>(storedProcedureName, values, commandType: CommandType.StoredProcedure)).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return objStockModel;
        }

        public async Task<List<SalesReport>> GetWalletHistory(string FromDate, string ToDate, string PartyCode, string vtype)
        {
            List<SalesReport> objWalletHistory = new List<SalesReport>();
            try
            {
                //DateTime StartDate = DateTime.Now;
                //DateTime EndDate = DateTime.Now;
                DateTime StartDate = DateTime.Now.AddYears(-5);
                DateTime EndDate = DateTime.Now;
                if (!string.IsNullOrEmpty(FromDate) && (!string.IsNullOrEmpty(ToDate)))
                {
                    if (!string.IsNullOrEmpty(FromDate) && FromDate != "All")
                    {
                        var SplitDate = FromDate.Split('-');
                        var NewDate = (SplitDate[1].Length == 1 ? "0" + SplitDate[1] : SplitDate[1]) + "/" + (SplitDate[0].Length == 1 ? "0" + SplitDate[0] : SplitDate[0]) + "/" + SplitDate[2];
                        StartDate = Convert.ToDateTime(DateTime.ParseExact(NewDate, "MM/dd/yyyy", CultureInfo.InvariantCulture));
                        StartDate = StartDate.Date;
                    }
                    if (!string.IsNullOrEmpty(ToDate) && ToDate != "All")
                    {
                        var SplitDate = ToDate.Split('-');
                        var NewDate = (SplitDate[1].Length == 1 ? "0" + SplitDate[1] : SplitDate[1]) + "/" + (SplitDate[0].Length == 1 ? "0" + SplitDate[0] : SplitDate[0]) + "/" + SplitDate[2];
                        EndDate = Convert.ToDateTime(DateTime.ParseExact(NewDate, "MM/dd/yyyy", CultureInfo.InvariantCulture));
                        EndDate = EndDate.Date;
                    }
                }
                //if (!string.IsNullOrEmpty(FromDate) && FromDate != "All")
                //{
                //    StartDate = Convert.ToDateTime(FromDate);
                //}

                //if (!string.IsNullOrEmpty(ToDate) && ToDate != "All")
                //{
                //    EndDate = Convert.ToDateTime(ToDate);
                //}

                //if (FromDate.ToLower() == "all" || string.IsNullOrEmpty(FromDate))
                //    StartDate = DateTime.Now.AddYears(-5);
                //if (ToDate.ToLower() == "all" || string.IsNullOrEmpty(ToDate))
                //    EndDate = DateTime.Now;

                using (var connection = _context.CreateLiveconnInv())
                {
                    var storedProcedureName = "sp_WalletReport";
                    var values = new
                    {
                        @PartyCode = PartyCode,
                        @FromDate = StartDate,
                        @Todate = EndDate,
                        @VType = vtype
                    };
                    objWalletHistory = (await connection.QueryAsync<SalesReport>(storedProcedureName, values, commandType: CommandType.StoredProcedure)).ToList();
                }
            }
            catch
            {

            }
            return objWalletHistory;
        }

        public async Task<List<PartyOrderModel>> GetOrderList(string OrderBy, string OrderTo, string Status)
        {
            List<PartyOrderModel> objPartyOrderModel = new List<PartyOrderModel>();
            try
            {
                if (OrderTo == null)
                {
                    OrderTo = "";
                }

                using (var connection = _context.CreateLiveconnInv())
                {
                    var storedProcedureName = "sp_orderhistory";
                    var values = new
                    {
                        @PartyCode = OrderBy,
                        @Status = Status,
                        @OrderTo = OrderTo
                    };
                    objPartyOrderModel = (await connection.QueryAsync<PartyOrderModel>(storedProcedureName, values, commandType: CommandType.StoredProcedure)).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return objPartyOrderModel;
        }
        public async Task<List<ProductModel>> GetOrderProduct(string OrderNo, string OrderBy)
        {
            List<ProductModel> objPartyOrderModel = new List<ProductModel>();
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    var storedProcedureName = "sp_GetOrderProduct";
                    var values = new
                    {
                        @OrderNo = OrderNo,
                        @OrderBy = OrderBy
                    };
                    objPartyOrderModel = (await connection.QueryAsync<ProductModel>(storedProcedureName, values, commandType: CommandType.StoredProcedure)).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return objPartyOrderModel;
        }

        public async Task<List<FranchiseeCommission>> GetFranchiseeBVCommission(string FromDate, string ToDate, string code, string Billtype)
        {
            List<FranchiseeCommission> objPartyOrderModel = new List<FranchiseeCommission>();
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    DateTime StartDate = DateTime.Now;
                    DateTime EndDate = DateTime.Now;
                    if (!string.IsNullOrEmpty(FromDate) && FromDate != "All")
                    {
                        StartDate = Convert.ToDateTime(FromDate);
                        StartDate = StartDate.Date;
                    }
                    if (!string.IsNullOrEmpty(ToDate) && ToDate != "All")
                    {
                        EndDate = Convert.ToDateTime(ToDate);
                        EndDate = EndDate.Date;
                    }

                    if (FromDate.ToLower() == "all" || string.IsNullOrEmpty(FromDate))
                        StartDate = DateTime.Now.AddYears(-5);
                    if (ToDate.ToLower() == "all" || string.IsNullOrEmpty(ToDate))
                        EndDate = DateTime.Now;

                    var storedProcedureName = "sp_GetFranchiseBVCommission";
                    var values = new
                    {
                        PartyCode = code,
                        StartDate = StartDate.Date.ToString("yyyy/MM/dd"),
                        EndDate = EndDate.Date.ToString("yyyy/MM/dd"),
                        Billtype = Billtype
                    };
                    objPartyOrderModel = (await connection.QueryAsync<FranchiseeCommission>(storedProcedureName, values, commandType: CommandType.StoredProcedure)).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return objPartyOrderModel;
        }

        public async Task<List<MonthWiseIncome>> GetMonthWiseIncome(string Sessid, string PartyCode)
        {
            List<MonthWiseIncome> result = new List<MonthWiseIncome>();
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    var storedProcedureName = "GEtMonthWiseIncome";
                    var values = new
                    {
                        @Sessid = Sessid,
                        @PartyCode = PartyCode,
                    };
                    result = (await connection.QueryAsync<MonthWiseIncome>(storedProcedureName,
                        values, commandType: CommandType.StoredProcedure)).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return result;

        }

        public async Task<List<MSessids>> GetSessids()
        {
            List<MSessids> result = new List<MSessids>();
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    var storedProcedureName = "Sp_GetSessid";

                    result = (await connection.QueryAsync<MSessids>(storedProcedureName,
                        commandType: CommandType.StoredProcedure)).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<List<MPerformanceInc>> GetPerformanceInc(string Partycode, string Action, int SessID)
        {
            List<MPerformanceInc> result = new List<MPerformanceInc>();
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    var storedProcedureName = "Sp_GetPerformanceInc";
                    var values = new
                    {
                        @Partycode = Partycode,
                        @Action = Action,
                        @SessID=SessID
                    };
                    result = (await connection.QueryAsync<MPerformanceInc>(storedProcedureName,
                        values, commandType: CommandType.StoredProcedure)).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<M_IncentiveStatement> GetIncentiveStatement(string Partycode, string StatementPeriod)
        {
            M_IncentiveStatement obj = new M_IncentiveStatement();
            try
            {
                obj.StatementPeriodId = Convert.ToInt32(StatementPeriod);
                using (var connection = _context.CreateLiveconnInv())
                {

                    string query = @"select CONVERT(varchar(100),FrmDate,106)+' To '+CONVERT(varchar(100),ToDate,106) as SessMonth 
                     from M_MonthSessnMaster   
                     where SessID=@Sessid";


                    var resultperiod = connection.QueryFirstOrDefault<string>(query, new { Sessid = StatementPeriod });
                    obj.StatementPeriod = resultperiod;

                    var parameters = new DynamicParameters();
                    parameters.Add("@SessID", StatementPeriod);
                    parameters.Add("@Partycode", Partycode);

                    var resultPVBV = connection.QueryFirstOrDefault<PVBVResult>(
                        "sp_GetTotalPVBVValBySessID",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    ); 


                    var storedProcedureName = "GEtMonthWiseIncome";
                    var values = new
                    {
                        @Sessid = StatementPeriod,
                        @PartyCode = Partycode,
                    };
                    var result = (await connection.QueryAsync<MonthWiseIncome>(storedProcedureName,
                         values, commandType: CommandType.StoredProcedure)).ToList();
                    obj.ChequeAmount = result.Sum(x => x.NetIncome);
                    obj.self_Comm_BV = result.Sum(x => x.Self_Comm_BV);
                    obj.selfIncomePv = result.Sum(x => x.SelfIncomePv);
                    obj.bV_Slab = result.Sum(x => x.BV_Slab);
                    obj.pvSlab = result.Sum(x => x.PvSlab);
                    obj.AdminCharge = result.Sum(x => x.AdminCharge);
                    obj.Self_Comm_BV = result.Sum(x => x.Self_Comm_BV);
                    obj.Diff_Comm_BV = result.Sum(x => x.Diff_Comm_BV);
                    obj.Diff_Comm_Pv = result.Sum(x => x.Diff_Comm_Pv);
                    obj.TotalBVVal = resultPVBV.BvValue;
                    obj.TotalPVVal = resultPVBV.PVValue;
                    obj.CommOnPv = resultPVBV.CommOnPv;
                    List<MPerformanceInc> mPerformanceIncs = new List<MPerformanceInc>();
                    for (int i = 1; i <= 2; i++)
                    {
                        string Action = "";
                        if (i == 1)
                        {
                            Action = "BV";

                        }
                        else if (i == 2)
                        {
                            Action = "PV";
                        }
                        var storedProcedureNamedown = "Sp_GetPerformanceInc";
                        var valuesdown = new
                        {
                            @Partycode = Partycode,
                            @Action = Action,
                            @SessID = StatementPeriod
                        };
                        var resultdown = (await connection.QueryAsync<MPerformanceInc>(storedProcedureNamedown,
                            valuesdown, commandType: CommandType.StoredProcedure)).ToList();
                        mPerformanceIncs.AddRange(resultdown);
                    }

                    var mPerformanceIncsgroup = mPerformanceIncs
                       .GroupBy(x => x.FromPartyCode)
                                    .Select(g => new MPerformanceInc
                                    {
                                        FromPartyCode = g.Key,
                                        FranchiseeName = g.Select(x => x.FranchiseeName).FirstOrDefault(),
                                        Diff_Comm_BV = g.Sum(x => x.Diff_Comm_BV),
                                        Diff_Comm_Pv = g.Sum(x => x.Diff_Comm_Pv),
                                        BvComm = g.Sum(x => x.BvComm),
                                        Pv = g.Sum(x => x.Pv),
                                        PvComm = g.Sum(x => x.PvComm),
                                        Slab = g.Sum(x => x.Slab)
                                    })
                                    .ToList();

                    obj.DownlineFranchiseePerformance = mPerformanceIncsgroup;

                }
            }
            catch
            {

            }
            return obj;
        }
    }
}
