using Dapper;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Transactions;
using VitaFlow.Domain.CommonDTO;
using VitaFlow.Domain.Entities;
using VitaFlow.Domain.Interface;
using VitaFlow.Infrastructure.DapperContext;
using VitaFlow.Infrastructure.Helper;
using static Dapper.SqlMapper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Net.Mime.MediaTypeNames;
using static VitaFlow.Domain.CommonDTO.EnumCalculation;

namespace VitaFlow.Infrastructure.Repository
{
    public class TransactionRepository : I_Transaction
    {
        private readonly DapperDbContext _context;

        public TransactionRepository(DapperDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseDetail> RejectWalletRequest(string ReqNo, string RejectReason, decimal RejectedByUserId)
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse.ResponseMessage = "Something went wrong!";
            objResponse.ResponseStatus = "FAILED";

            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    string Sql = "Select * from WalletReq WHERE ReqNo=@ReqNo";
                    var result = await connection.QueryFirstOrDefaultAsync(Sql, new { ReqNo });
                    if (result != null)
                    {

                        string Status = result.IsApprove;
                        var db = ConfigurationManager.AppSetting.GetSection("Database").Value;
                        if (Status == "N")
                        {
                            Sql = ";Update WalletReq Set IsApprove='R',ApproveDate=GetDate(),ApproveRemark='" + RejectReason + "',ApproveBy='" + RejectedByUserId + "' where ReqNo='" + ReqNo + "' ";
                        }
                        var rowsAffected = await connection.ExecuteAsync(Sql);
                        if (rowsAffected > 0)
                        {
                            objResponse.ResponseStatus = "OK";
                            objResponse.ResponseMessage = "Rejected Successfully!";
                        }
                        else
                        {
                            objResponse.ResponseStatus = "Failed";
                            objResponse.ResponseMessage = "Something went wrong.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objResponse.ResponseMessage = "Something went wrong!";
                objResponse.ResponseStatus = "FAILED";
            }
            return objResponse;
        }
        public async Task<string> SaveWalletRequest(M_WalletRequest objWallet)
        {
            string Rstr = string.Empty;
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    M_WalletRequest objWReq = new M_WalletRequest();
                    decimal reqNo = 0;
                    string reqnosql = @"select ISNULL(max(ReqNo),0) from WalletReq";

                    reqNo = await connection.QuerySingleOrDefaultAsync<decimal>(reqnosql);
                    reqNo = reqNo + 1;
                    objWReq.ReqNo = Convert.ToString(reqNo);
                    objWReq.ReqDate = DateTime.Now.Date;
                    objWReq.Amount = objWallet.Amount;
                    objWReq.BankID = objWallet.BankID;// != null ? objWallet.BankID :0; 
                    objWReq.BankName = objWallet.BankName != null ? objWallet.BankName : "";
                    objWReq.BranchName = objWallet.BranchName != null ? objWallet.BranchName : "";
                    objWReq.ChqDate = objWallet.ChqDate;
                    objWReq.ChqNo = objWallet.ChqNo ?? "0";
                    objWReq.Paymode = objWallet.Paymode;
                    objWReq.PID = objWallet.PID;
                    objWReq.Remarks = objWallet.Remarks != null ? objWallet.Remarks : "";
                    objWReq.ReqBy = objWallet.ReqBy;
                    objWReq.ScannedFileName = objWallet.ScannedFileName != null ? objWallet.ScannedFileName : "";
                    //objWReq.RecTimeStamp = DateTime.Now;
                    objWReq.TransNo = "0";
                    objWReq.IsApproved = "N";
                    objWReq.ApproveBy = 0;
                    objWReq.ApproveRemark = "";
                    objWReq.VType = objWallet.VType;
                    var walsql = @"insert into WalletReq(ReqNo,ReqDate,ReqBy,PID,Paymode,Amount,ChqNo,ChqDate,BankName,
                                   BranchName,ScannedFile,RecTimeStamp,IsApprove,ApproveBy,
                                   Remarks,TransNo,BankId,ApproveRemark,VType)
                                   values(@ReqNo,getdate(),@ReqBy,@PID,@Paymode,@Amount,@ChqNo,getdate(),@BankName,
                                   @BranchName,@ScannedFileName,getdate(),@IsApproved,@ApproveBy,
                                   @Remarks,@TransNo,@BankId,@ApproveRemark,@VType)";
                    var rowsAffected = await connection.ExecuteAsync(walsql, objWReq);
                    if (rowsAffected > 0)
                    {
                        Rstr = "OK";
                    }
                    else
                    {
                        Rstr = "Fail";
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Rstr;
        }
        public async Task<List<PartyOrderModel>> GetOrderList(string OrderBy, string OrderTo, string Status)
        {
            List<PartyOrderModel> objPartyOrderModel = new List<PartyOrderModel>();
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    var storedProcedureName = @"SELECT 
                                                r.OrderNo,
                                                r.OrderBy AS PartyCode,
                                                l.PartyName,
                                                r.OrderDate,
                                                r.OrderAmount AS OrderAmt,
                                                ISNULL(CAST(r.chNo AS VARCHAR), '0') AS ChNo,
                                                ISNULL(r.ChDate, GETDATE()) AS ChDate,
                                                ISNULL(r.ChAmt, 0) AS ChAmt,
                                                r.BankName,
                                                r.TotalWeight AS WalletAmt,
                                                r.OrderBy,
                                                r.OrderTo,
                                                r.Status AS DispStatus,
                                                r.OrderMethod
                                                FROM 
                                                TrnPartyOrderMain r
                                                INNER JOIN 
                                                M_LedgerMaster l ON r.OrderBy = l.PartyCode
                                                WHERE 
                                                r.ActiveStatus = 'Y';";
                    objPartyOrderModel = (await connection.QueryAsync<PartyOrderModel>(storedProcedureName, commandType: CommandType.Text)).ToList();

                }
                if (OrderBy.ToUpper() != "ALL")
                    objPartyOrderModel = objPartyOrderModel.Where(m => m.OrderBy == OrderBy).ToList();
                if (OrderTo.ToUpper() != "ALL")
                    objPartyOrderModel = objPartyOrderModel.Where(m => m.OrderTo == OrderTo).ToList();
                if (Status.ToUpper() != "A")
                    objPartyOrderModel = objPartyOrderModel.Where(m => m.DispStatus == Status).ToList();
            }
            catch (Exception ex)
            {

            }
            return objPartyOrderModel;
        }
        public async Task<ResponseDetail> RejectFranchiseOrder(string OrderNo, string RejectReason, decimal RejectedByUserId)
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse.ResponseMessage = "Something went wrong!";
            objResponse.ResponseStatus = "FAILED";
            TrnPartyOrderMain objPartyOrderMain = new TrnPartyOrderMain();
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    string Sql = "Select CASE WHEN a.OrderAmount-b.NetPayable<TotalWeight THEN a.OrderAmount-b.NetPayable ELSE TotalWeight END AS WalletAmt,a.OrderBy,a.OrderTo ";
                    Sql = Sql + " FROM TrnPartyOrderMain a LEFT JOIN (Select OrderNo, SUM(NetPayable) NetPayable FROM TrnBillMain WHERE OrderNo='" + OrderNo + "' GROUP BY OrderNo) b  ";
                    Sql = Sql + " On a.OrderNo=b.OrderNo AND a.ActiveStatus='Y' WHERE a.OrderNo=@OrderNo";
                    var result = await connection.QueryFirstOrDefaultAsync(Sql, new { OrderNo });
                    Sql = "";
                    if (result != null)
                    {
                        decimal walletAmt = result.WalletAmt;
                        string orderTo = result.OrderTo;
                        string orderBy = result.OrderBy;
                        var db = ConfigurationManager.AppSetting.GetSection("Database").Value;
                        if (walletAmt > 0)
                        {
                            Sql = ";INSERT INTO TrnVoucher(VoucherNo,VoucherDate,DrTo,Crto,Amount,Narration,Refno,VType,BType,AccDocType,SessID,FSessID) ";
                            Sql = Sql + " Select CASE WHEN Max(VoucherNo) is NULL THEN 1 ELSE Max(VoucherNo)+1 END ,Cast(Convert(varchar,Getdate(),106) as Datetime),'" + orderTo + "','" + orderBy + "','" + walletAmt + "','Order " + OrderNo + " generated for product.','" + (OrderNo) + "','R','O','Order Generated',(Select Max(SessID) FROM " + db + "..M_SessnMaster),(Select Max(FSessID) FROM M_FiscalMaster) FROM TrnVoucher ;";
                        }
                    }
                    Sql = Sql + "UPDATE TrnPartyOrderMain SET Status='C',ActiveStatus='D',OrderNo= 'Del:'+ OrderNo ,Remarks=Remarks +'; Del:  " + RejectReason + " by " + RejectedByUserId + " on " + DateTime.Now + "' WHERE OrderNo='" + OrderNo + "';";
                    Sql = Sql + "UPDATE TrnPartyOrderDetail SET Status='C',ActiveStatus='D',OrderNo= 'Del:'+ OrderNo WHERE OrderNo='" + OrderNo + "';";
                    var rowsAffected = await connection.ExecuteAsync(Sql);

                    var psql = "Exec Sp_RevertOrderFromlimit '" + (OrderNo) + "';";
                    var rowsaffe = await connection.ExecuteAsync(psql);
                }
            }
            catch (Exception ex)
            {
                objResponse.ResponseMessage = "Something went wrong!";
                objResponse.ResponseStatus = "FAILED";
            }
            return objResponse;
        }
        public async Task<List<ProductModel>> GetOrderProductList(string OrderNo, string OrderBy, string CurrentPartyCode)
        {
            List<ProductModel> objOrderProductModel = new List<ProductModel>();
            List<ProductModel> Returnlist = new List<ProductModel>();
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    var sql = @"SELECT 
                                r.ProductName,
                                r.Barcode,
                                r.BatchNo,
                                r.DP,
                                r.RP,
                                r.DiscPer,
                                r.Discount AS DiscAmt,
                                r.ProductCode AS ProductCodeStr,
                                r.Tax,
                                r.TaxAmount AS TaxAmt,
                                r.MRP,
                                r.BV,
                                0 AS PV,
                                0 AS CV,
                                r.Rate,
                                r.Amount,
                                p.NetPayable AS TotalNetPayable,
                                CASE WHEN r.RemQty = 0 THEN r.Qty ELSE r.RemQty END AS OrderedOty,
                                r.DispatchQty AS DispQty,
                                r.OfferUId AS OfferUID,
                                r.ProdType AS ProductType
                                FROM TrnPartyOrderDetail r
                               JOIN TrnPartyOrderMain p ON p.OrderNo = r.OrderNo
                               WHERE r.ActiveStatus = 'Y' 
                               AND r.OrderNo = @OrderNo 
                               AND r.OrderBy = @OrderBy 
                               AND r.Status = 'P'";
                    var parameters = new { OrderNo = OrderNo, OrderBy = OrderBy };
                    objOrderProductModel = (await connection.QueryAsync<ProductModel>(sql, parameters)).ToList();
                    bool IsDistributorBill = false;
                    bool IsPartyBill = true;
                    bool IsCustomerBill = false;
                    bool IsPurchaseInvoice = false;
                    bool IsOrderCreation = false;
                    bool IsPendingOrder = false;
                    int CurrentStateCode = 0;
                    bool IsBillOnMrp = false;
                    string IsSpclOffer = "";
                    string Invoice = "";
                    bool allhalf = false;
                    if (objOrderProductModel != null)
                    {
                        foreach (var item in objOrderProductModel)
                        {
                            string query = @"
                                            SELECT 
                                            p.ProductName,
                                            p.CatId,
                                            c.CatName,
                                            b.BarCode AS Barcode,
                                            bm.BatchNo,
                                            bm.Dp AS DP,
                                            p.RP,
                                            p.Discount AS DiscPer,
                                            p.DiscInRs AS DiscAmt,
                                            CAST(p.ProductCode AS INT) AS ProdCode,
                                            p.ProdId AS ProductCodeStr,
                                            t.VatTax AS TaxPer,
                                            t.StateCode AS ProdStateCode,
                                            bm.Mrp,
                                            p.BV,
                                            p.PV,
                                            p.CV,
                                            CASE WHEN bm.IsExpired = 'Y' THEN 1 ELSE 0 END AS IsExpirable,
                                            bm.ExpDate,
                                            'GST' AS TaxType,
                                            p.PurchaseRate AS Rate,
                                            p.ProdCommssn AS CommissionPer,
                                            p.SubCatId,
                                            p.IsAvailableforOffers AS IsAvailableForOffer,
                                            p.IsBillingAllowed AS IsAvailableForBilling,
                                            p.Weight,
                                            ISNULL(p.SJDiscount, 0) AS TotalDiscPer
                                            FROM M_ProductMaster p
                                            JOIN M_BarCodeMaster b ON p.ProdId = b.ProdId
                                            JOIN M_BatchMaster bm ON p.ProdId = bm.ProdId
                                            JOIN M_TaxMaster t ON p.ProdId = t.ProdCode
                                            JOIN M_CatMaster c ON p.CatId = c.CatId
                                            WHERE 
                                            LOWER(p.ProductName) = LOWER(@ProductName)
                                            AND p.ActiveStatus = 'Y'
                                            AND b.ActiveStatus = 'Y'
                                            AND bm.ActiveStatus = 'Y'
                                            -- Uncomment the following line if you want to filter by the tax state code
                                            -- AND t.StateCode = @CurrentStateCode
                                             ";

                            // Use Dapper to execute the query
                            var tempparameters = new
                            {
                                ProductName = item.ProductName,
                                // Uncomment the next line if you're filtering by state code
                                // CurrentStateCode = yourCurrentStateCode
                            };
                            var TempResult = connection.Query<ProductModel>(query, tempparameters).ToList();
                            List<ProductModel> objProductModel = new List<ProductModel>();
                            foreach (var obj in TempResult)
                            {
                                ProductModel TempObj = new ProductModel();
                                if ((obj.IsExpirable && obj.ExpDate > DateTime.Now) || (obj.IsExpirable == false))
                                {
                                    TempObj = obj;
                                    object valueIsDiscountAdd = 0;
                                    object valueIsCommissonAdd = 0;
                                    if (IsDistributorBill || IsCustomerBill || IsPurchaseInvoice || IsOrderCreation || IsPendingOrder)
                                    {
                                        valueIsCommissonAdd = Enum.Parse(typeof(Enums.CalculationConditionalVar), Enums.CalculationConditionalVar.IsCommissonAdd.ToString());
                                        valueIsDiscountAdd = Enum.Parse(typeof(Enums.CalculationConditionalVar), Enums.CalculationConditionalVar.IsDiscountAdd.ToString());
                                    }
                                    else
                                    {
                                        valueIsCommissonAdd = Enum.Parse(typeof(Enums.CalculationConditionalVar), Enums.CalculationConditionalVar.IsCommissonAddOnPartyBill.ToString());
                                        valueIsDiscountAdd = Enum.Parse(typeof(Enums.CalculationConditionalVar), Enums.CalculationConditionalVar.IsDiscountAddOnPartyBill.ToString());
                                    }
                                    int IsCommission = Convert.ToInt32(valueIsCommissonAdd);
                                    int IsDiscount = Convert.ToInt32(valueIsDiscountAdd);
                                    TempObj.IsCommissionAdd = IsCommission;
                                    TempObj.IsDiscountAdd = IsDiscount;
                                    string stockquery = @"
                                                    SELECT ISNULL(SUM(Qty), 0) AS StockAvailable
                                                    FROM Im_CurrentStock
                                                    WHERE ProdId = @ProductCodeStr
                                                    AND FCode = @CurrentPartyCode
                                                    AND BatchCode = @BatchNo
                                                    ";
                                    var stockparameters = new
                                    {
                                        ProductCodeStr = TempObj.ProductCodeStr.ToString(),
                                        CurrentPartyCode = CurrentPartyCode,
                                        BatchNo = TempObj.BatchNo
                                    };
                                    // Execute the query using Dapper and assign the result to StockAvailable
                                    TempObj.StockAvailable = connection.QuerySingleOrDefault<int>(stockquery, stockparameters);
                                    TempObj.DP1 = TempObj.DP;
                                    if (IsCustomerBill)
                                    {
                                        TempObj.DP = obj.MRP;
                                    }
                                    else
                                    {
                                        if (!IsPurchaseInvoice && IsBillOnMrp)
                                        {
                                            TempObj.DP = obj.MRP;
                                        }
                                    }
                                    string StateCodequery = @"
                                                              SELECT TOP 1 CompState
                                                              FROM M_CompanyMaster
                                                              ";

                                    CurrentStateCode = connection.QueryFirstOrDefault<int>(StateCodequery);

                                    if (allhalf)
                                    {
                                        TempObj.DP = TempObj.DP / 2;
                                        TempObj.BV = TempObj.BV / 2;
                                        TempObj.PV = TempObj.PV / 2;
                                        TempObj.RP = TempObj.RP / 2;
                                        TempObj.DiscAmt = TempObj.DP;
                                        TempObj.IsDiscountAdd = 1;
                                    }
                                    if (!string.IsNullOrEmpty(Invoice) && Invoice == "CED")
                                    {
                                        var oridp = TempObj.DP;
                                        TempObj.DP = (TempObj.DP * 1) / 4;
                                        TempObj.BV = 0;
                                        TempObj.PV = (TempObj.PV * 1) / 4;
                                        TempObj.RP = (TempObj.RP * 1) / 4;
                                        TempObj.DiscAmt = oridp - TempObj.DP;
                                        TempObj.IsDiscountAdd = 1;
                                    }
                                    if (!string.IsNullOrEmpty(Convert.ToString(item.OfferUID)))
                                    {
                                        decimal iOfferID = Convert.ToDecimal(item.OfferUID);
                                        if (iOfferID != 0)
                                        {
                                            TempObj.offerDetail = await GetOfferDetail(iOfferID, obj.ProductCodeStr, IsSpclOffer);
                                            if (!string.IsNullOrEmpty(TempObj.offerDetail.offerType))
                                            {
                                                decimal offerType = Convert.ToDecimal(TempObj.offerDetail.offerType);
                                                if (offerType == 2 || offerType == 3)
                                                {
                                                    TempObj.DP = TempObj.offerDetail.OfferMrp / 1;// TempObj.offerDetail.FreeQty;
                                                    TempObj.OfferProdQty = TempObj.offerDetail.FreeQty;
                                                    TempObj.OfferProdQty = 1;
                                                    TempObj.BV = 0;
                                                }
                                            }
                                        }
                                    }
                                    objProductModel.Add(TempObj);
                                }
                            }
                            objProductModel = objProductModel.Where(m => m.StockAvailable > 0).OrderBy(m => m.ExpDate).ThenBy(m => m.StockAvailable).ToList();
                            if (objProductModel.Count == 0)
                            {
                                ProductModel pp = new ProductModel();
                                pp.ProductName = item.ProductName;
                                pp.Barcode = item.Barcode;
                                pp.BatchNo = item.BatchNo;
                                pp.DP = item.DP;
                                pp.RP = item.RP;
                                pp.DiscPer = item.DiscPer;
                                pp.DiscAmt = item.DiscAmt;
                                pp.ProductCodeStr = item.ProductCodeStr;
                                pp.TaxPer = item.TaxPer;
                                pp.TaxAmt = item.TaxAmt;
                                pp.MRP = item.MRP;
                                pp.BV = item.BV;
                                pp.PV = item.PV;
                                pp.CV = item.CV;
                                pp.Rate = item.Rate;
                                pp.Amount = item.Amount;
                                pp.TotalNetPayable = item.TotalNetPayable;
                                pp.OrderedOty = item.OrderedOty;
                                pp.DispQty = item.DispQty;
                                pp.OfferUID = item.OfferUID;
                                pp.ProductType = item.ProductType;
                                pp.IsCommissionAdd = item.IsCommissionAdd;
                                pp.IsDiscountAdd = item.IsDiscountAdd;
                                pp.OfferProdQty = item.OfferProdQty;
                                pp.IsAvailableForBilling = item.IsAvailableForBilling;
                                pp.IsAvailableForOffer = item.IsAvailableForOffer;
                                pp.StockAvailable = 0;
                                Returnlist.Add(pp);
                            }
                            else
                            {
                                foreach (var prodcalc in objProductModel)
                                {
                                    ProductModel pp = new ProductModel();
                                    var totalqty = item.OrderedOty;
                                    if (totalqty == prodcalc.StockAvailable)
                                    {
                                        pp.OrderedOty = item.OrderedOty;
                                        pp.DispQty = totalqty;
                                        pp.ProductName = item.ProductName;
                                        pp.Barcode = prodcalc.Barcode;
                                        pp.BatchNo = prodcalc.BatchNo;
                                        pp.DP = prodcalc.DP;
                                        pp.RP = prodcalc.RP;
                                        pp.DiscPer = item.DiscPer;
                                        pp.DiscAmt = item.DiscAmt;
                                        pp.ProductCodeStr = item.ProductCodeStr;
                                        pp.TaxPer = prodcalc.TaxPer;
                                        pp.TaxAmt = prodcalc.TaxAmt;
                                        pp.MRP = item.MRP;
                                        pp.BV = prodcalc.BV;
                                        pp.PV = prodcalc.PV;
                                        pp.CV = prodcalc.CV;
                                        pp.Rate = item.Rate;
                                        pp.Amount = item.Amount;
                                        pp.TotalNetPayable = item.TotalNetPayable;
                                        pp.OfferUID = item.OfferUID;
                                        pp.ProductType = item.ProductType;
                                        pp.IsCommissionAdd = prodcalc.IsCommissionAdd;
                                        pp.IsDiscountAdd = prodcalc.IsDiscountAdd;
                                        pp.OfferProdQty = prodcalc.OfferProdQty;
                                        pp.IsAvailableForBilling = prodcalc.IsAvailableForBilling;
                                        pp.IsAvailableForOffer = prodcalc.IsAvailableForOffer;
                                        pp.StockAvailable = prodcalc.StockAvailable;
                                        Returnlist.Add(pp);
                                        break;
                                    }
                                    else if (prodcalc.StockAvailable >= totalqty)
                                    {
                                        pp.OrderedOty = item.OrderedOty;
                                        pp.DispQty = totalqty;
                                        pp.ProductName = item.ProductName;
                                        pp.Barcode = prodcalc.Barcode;
                                        pp.BatchNo = prodcalc.BatchNo;
                                        pp.DP = prodcalc.DP;
                                        pp.RP = prodcalc.RP;
                                        pp.DiscPer = item.DiscPer;
                                        pp.DiscAmt = item.DiscAmt;
                                        pp.ProductCodeStr = item.ProductCodeStr;
                                        pp.TaxPer = prodcalc.TaxPer;
                                        pp.TaxAmt = prodcalc.TaxAmt;
                                        pp.MRP = item.MRP;
                                        pp.BV = prodcalc.BV;
                                        pp.PV = prodcalc.PV;
                                        pp.CV = prodcalc.CV;
                                        pp.Rate = item.Rate;
                                        pp.Amount = item.Amount;
                                        pp.TotalNetPayable = item.TotalNetPayable;
                                        pp.OfferUID = item.OfferUID;
                                        pp.ProductType = item.ProductType;
                                        pp.IsCommissionAdd = prodcalc.IsCommissionAdd;
                                        pp.IsDiscountAdd = prodcalc.IsDiscountAdd;
                                        pp.OfferProdQty = prodcalc.OfferProdQty;
                                        pp.IsAvailableForBilling = prodcalc.IsAvailableForBilling;
                                        pp.IsAvailableForOffer = prodcalc.IsAvailableForOffer;
                                        pp.StockAvailable = prodcalc.StockAvailable;
                                        Returnlist.Add(pp);
                                        break;
                                    }
                                    else if (totalqty >= prodcalc.StockAvailable)
                                    {
                                        totalqty = totalqty - item.StockAvailable;
                                        pp.OrderedOty = item.OrderedOty;
                                        pp.DispQty = totalqty;
                                        pp.ProductName = item.ProductName;
                                        pp.Barcode = prodcalc.Barcode;
                                        pp.BatchNo = prodcalc.BatchNo;
                                        pp.DP = prodcalc.DP;
                                        pp.RP = prodcalc.RP;
                                        pp.DiscPer = item.DiscPer;
                                        pp.DiscAmt = item.DiscAmt;
                                        pp.ProductCodeStr = item.ProductCodeStr;
                                        pp.TaxPer = prodcalc.TaxPer;
                                        pp.TaxAmt = prodcalc.TaxAmt;
                                        pp.MRP = item.MRP;
                                        pp.BV = prodcalc.BV;
                                        pp.PV = prodcalc.PV;
                                        pp.CV = prodcalc.CV;
                                        pp.Rate = item.Rate;
                                        pp.Amount = item.Amount;
                                        pp.TotalNetPayable = item.TotalNetPayable;
                                        pp.OfferUID = item.OfferUID;
                                        pp.ProductType = item.ProductType;
                                        pp.IsCommissionAdd = prodcalc.IsCommissionAdd;
                                        pp.IsDiscountAdd = prodcalc.IsDiscountAdd;
                                        pp.OfferProdQty = prodcalc.OfferProdQty;
                                        pp.IsAvailableForBilling = prodcalc.IsAvailableForBilling;
                                        pp.IsAvailableForOffer = prodcalc.IsAvailableForOffer;
                                        pp.StockAvailable = prodcalc.StockAvailable;
                                        Returnlist.Add(pp);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Returnlist;
        }

        public async Task<List<BankModel>> GetBankList()
        {
            List<BankModel> objListBanks = new List<BankModel>();
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    var sql = @"SELECT 
                           CAST(BankCode AS INT) AS BankCode,
                           BankName,
                           ActiveStatus,
                           AcNo AS AccNo,
                           Remarks,
                           IFSCode AS IFSCCode
                           FROM M_BankMaster
                           WHERE ActiveStatus = 'Y'";
                    objListBanks = (await connection.QueryAsync<BankModel>(sql)).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return objListBanks;
        }
        public async Task<List<KitDetail>> GetKitIdList()
        {
            List<KitDetail> KidIDs = new List<KitDetail>();
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    string query = "select KitId,KitName from M_KitMaster where KitAmount>0";
                    KidIDs = (await connection.QueryAsync<KitDetail>(query)).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return KidIDs;
        }
        public async Task<List<string>> GetAutocompProductsOnly(string FCode)
        {
            List<string> objProductNames = new List<string>();
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    //var sql = @"select ProductName from M_ProductMaster
                    //        where
                    //        ActiveStatus='Y' 
                    //        and (IsBillingAllowed ='Y' or IsAvailableforOffers ='Y')
                    //        and IsCardIssue ='N'
                    //        and PType != 'K'";
                    var sql = @"with stock as
                                (
                                select ISNULL(sum(Qty),0) as qty,ProdId from Im_CurrentStock where FCode=@FCode
                                group by ProdId
                                )
                                select ProductName,s.qty,Barcode from M_ProductMaster as p
                                inner join stock as s on s.ProdId=p.ProdId
                                where
                                ActiveStatus='Y' 
                                and (IsBillingAllowed ='Y' or IsAvailableforOffers ='Y')
                                and IsCardIssue ='N'
                                and PType != 'K'";
                    var parameters = new { FCode = FCode };
                    objProductNames = (await connection.QueryAsync<string>(sql, parameters)).ToList();
                }
            }
            catch
            {

            }
            return objProductNames;
        }
        public async Task<List<string>> GetProductBarcodeOnly(string FCode)
        {
            List<string> objProductNames = new List<string>();
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    //var sql = @"select ProductName from M_ProductMaster
                    //        where
                    //        ActiveStatus='Y' 
                    //        and (IsBillingAllowed ='Y' or IsAvailableforOffers ='Y')
                    //        and IsCardIssue ='N'
                    //        and PType != 'K'";
                    var sql = @"with stock as
                                (
                                select ISNULL(sum(Qty),0) as qty,ProdId from Im_CurrentStock where FCode=@FCode
                                group by ProdId
                                )
                                select Barcode,s.qty from M_ProductMaster as p
                                inner join stock as s on s.ProdId=p.ProdId
                                where
                                ActiveStatus='Y' 
                                and (IsBillingAllowed ='Y' or IsAvailableforOffers ='Y')
                                and IsCardIssue ='N'
                                and PType != 'K'";
                    var parameters = new { FCode = FCode };
                    objProductNames = (await connection.QueryAsync<string>(sql, parameters)).ToList();
                }
            }
            catch
            {

            }
            return objProductNames;
        }
        private async Task<string> GetMemeberDetail(string IdNo)
        {
            bool IsExist = false;
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    // M_MemberMaster mein IdNo exist karti hai ya nahi
                    string query = "SELECT COUNT(1) FROM M_MemberMaster WHERE IDno = @IdNo";
                    int count = await connection.ExecuteScalarAsync<int>(query, new { IdNo });
                    IsExist = count > 0;
                }
            }
            catch (Exception ex)
            {
                IsExist = false;
            }
            return IsExist ? "true" : "false";
        }
        public async Task<CustomerDetail> GetCustInfo(string IdNo)
        {
            CustomerDetail objCustomerDetail = new CustomerDetail();
            try
            {
                string MemberResponse = await GetMemeberDetail(IdNo);
                if (!string.IsNullOrEmpty(MemberResponse))
                {
                    //var cleanJson = MemberResponse.Replace("\r", "").Replace("\n", "");
                    //MemberAPIRoot memapi = JsonSerializer.Deserialize<MemberAPIRoot>(cleanJson);
                    if (MemberResponse == "true")
                    {
                        string ElligibleFor = "";
                        int isoldID = 0;
                        using (var connection = _context.CreateConnection())
                        {
                            //if (string.IsNullOrEmpty(memapi.Result.MobileNo))
                            //{
                            //    memapi.Result.MobileNo = "0";
                            //}
                            ////Insert new user 
                            //var parameters = new DynamicParameters();
                            //parameters.Add("@Username", memapi.Result.loginid);
                            //parameters.Add("@Password", "123");
                            //parameters.Add("@MemName", memapi.Result.name);
                            //parameters.Add("@MobileNo", memapi.Result.MobileNo);
                            //parameters.Add("@EMail", memapi.Result.email);
                            //parameters.Add("@Address1", memapi.Result.Address);
                            //parameters.Add("@State", memapi.Result.State);
                            //parameters.Add("@City", memapi.Result.city);

                            //int result = connection.Execute("Sp_AddMemberDetail", parameters,
                            //    commandType: CommandType.StoredProcedure
                            //);

                            //Check user
                            string query = "select a.Passw,a.Doj,a.UpgradeDate,a.Mobl,a.FormNo,a.MemFirstName+' '+ a.MemLastName as Name,a.KitId,a.IDno as IDno,a.Address1+','+a.Address2+','+a.City as Address,a.StateCode as StateCode,a.City,ISNULL(s.StateName,'') StateName,a.ActiveStatus as ActiveStatus,a.IsBlock as IsBlock,IsOldID=IIF(a.RefID>0,1,0),a.Imported as ElligibleFor,a.PanNo,a.District FROM M_MemberMaster a LEFT JOIN M_StateDivMaster s ON a.StateCode=s.StateCode WHERE a.IDno=@IdNo";
                            var reader = await connection.QueryFirstOrDefaultAsync(query, new { IdNo });
                            if (reader != null)
                            {
                                isoldID = Convert.ToInt16(reader.IsOldID);
                                ElligibleFor = reader.ElligibleFor;
                                objCustomerDetail.Password = reader.Passw != null ? reader.Passw : "";
                                objCustomerDetail.IdNo = reader.IDno != null ? reader.IDno : "";
                                objCustomerDetail.ReferenceIdNo = reader.RefId != null ? reader.RefId : "";
                                objCustomerDetail.ReferenceName = reader.RefName != null ? reader.RefName : "";
                                objCustomerDetail.Name = reader.Name != null ? reader.Name : "";
                                objCustomerDetail.Address = reader.Address != null ? reader.Address : "";
                                objCustomerDetail.CityName = reader.City != null ? reader.City : "";
                                objCustomerDetail.StateName = reader.StateName != null ? reader.StateName : "";
                                objCustomerDetail.FormNo = Convert.ToString(reader.FormNo) != null ? decimal.Parse(Convert.ToString(reader.FormNo)) : 0;
                                objCustomerDetail.StateCode = Convert.ToString(reader.StateCode) != null ? decimal.Parse(Convert.ToString(reader.StateCode)) : 0;
                                objCustomerDetail.MobileNo = Convert.ToString(reader.Mobl) != null ? Convert.ToString(reader.Mobl) : "";
                                objCustomerDetail.KitId = Convert.ToString(reader.KitId) != null ? int.Parse(Convert.ToString(reader.KitId)) : 0;
                                objCustomerDetail.PANNo = Convert.ToString(reader.PanNo) != null ? Convert.ToString(reader.PanNo) : "";
                                objCustomerDetail.Doj = Convert.ToString(reader.Doj) != null ? Convert.ToString(reader.Doj) : "";
                                objCustomerDetail.UpgradeDate = Convert.ToString(reader.UpgradeDate) != null ? Convert.ToString(reader.UpgradeDate) : "";
                                objCustomerDetail.IsActive = Convert.ToString(reader.ActiveStatus) == "Y" ? true : false;
                                objCustomerDetail.DistrictName = Convert.ToString(reader.District) != null ? reader.District : "";
                                if (reader.IsBlock != null)
                                {
                                    var BlockValue = reader.IsBlock;
                                    if (BlockValue == "Y")
                                    {
                                        objCustomerDetail.IsBlock = true;
                                    }
                                    else
                                    {
                                        objCustomerDetail.IsBlock = false;
                                    }
                                }
                                else
                                {
                                    objCustomerDetail.IsBlock = false;
                                }

                                if (ElligibleFor == "B")
                                {
                                    objCustomerDetail.IsFirstBill = true;
                                }
                                else
                                {
                                    objCustomerDetail.IsFirstBill = false;
                                }
                                objCustomerDetail.IsBillOnMrp = false;
                                objCustomerDetail.MinRepurch = 0;
                                using (var connectioninv = _context.CreateLiveconnInv())
                                {
                                    string DelvPlacesql = @"SELECT TOP 1 DelvPlace 
                                             FROM TrnBillMain 
                                             WHERE FCode = @FCode 
                                             ORDER BY BillId DESC";
                                    string delvPlace = connectioninv.QueryFirstOrDefault<string>(DelvPlacesql, new { FCode = IdNo });
                                    objCustomerDetail.DeliveryAddress = delvPlace;
                                }

                                if (objCustomerDetail != null)
                                {
                                    query = "Select * FROM dbo.ufnGetBalance('" + objCustomerDetail.FormNo + "','S')";
                                    var resbal = await connection.QueryFirstOrDefaultAsync(query);
                                    if (resbal != null)
                                    {
                                        objCustomerDetail.WalletBalance = decimal.Parse(Convert.ToString(resbal.Balance));
                                    }
                                }
                                decimal Ktamt = 0;
                                var db = ConfigurationManager.AppSetting.GetSection("Database").Value;
                                var dbInv = ConfigurationManager.AppSetting.GetSection("INVDatabase").Value;
                                query = "Select BV FROM " + db + "..M_KitMaster WHERE ActiveStatus='Y' AND TopUpSeq>(Select TopUpSeq From " + db + "..M_KitMaster WHERE KitID=" + objCustomerDetail.KitId.ToString() + ") AND BV>0 AND KitAmount=0 AND IsBill='N' ORDER BY BV";
                                var resbv = await connection.QueryFirstOrDefaultAsync(query);
                                if (resbv != null)
                                {
                                    Ktamt = !String.IsNullOrEmpty(Convert.ToString(resbv.BV)) ? decimal.Parse(Convert.ToString(resbv.BV)) : 0;
                                }
                                objCustomerDetail.MinBillAmt = Ktamt;
                                objCustomerDetail.MaxBV = 1000000;
                                query = "Select BVValue FROM " + dbInv + "..FPVConfig WHERE ActiveStatus='Y' AND RowStatus='Y'";
                                var resbvval = await connection.QueryFirstOrDefaultAsync(query);
                                if (resbvval != null)
                                {
                                    Ktamt = !String.IsNullOrEmpty(Convert.ToString(resbvval.BVValue)) ? decimal.Parse(Convert.ToString(resbvval.BVValue)) : 0;
                                    objCustomerDetail.MaxBV = Ktamt;
                                }
                                objCustomerDetail.InvoiceType = new List<string>();
                                var configquery = "select * from M_ConfigMaster";
                                var config = await connection.QueryFirstOrDefaultAsync(configquery);
                                if (objCustomerDetail.IsActive == true || ElligibleFor == "R")
                                {
                                    if (Ktamt > 0)
                                    {
                                        if (config != null)
                                        {
                                            if (config.CanIDBeUpgraded == "Y")
                                            {
                                                objCustomerDetail.InvoiceType.Add("Activation Upgrade,B");
                                            }
                                        }
                                        objCustomerDetail.InvoiceType.Add("Repurchase Bill,R");
                                    }
                                    else
                                    {
                                        objCustomerDetail.MinBillAmt = 0;
                                        objCustomerDetail.InvoiceType.Add("Repurchase Bill,R");
                                    }
                                }
                                else
                                {
                                    objCustomerDetail.InvoiceType.Add("Activation Purchase,B");
                                    //if (isoldID == 1)//Added on 18Jun19
                                    objCustomerDetail.InvoiceType.Add("General Billing,A");//18Jun19
                                }
                            }
                            else
                            {
                                objCustomerDetail = new CustomerDetail();
                                objCustomerDetail.IdNo = "Record does not exists!";
                                objCustomerDetail.Name = "";
                            }
                        }
                    }
                    else
                    {
                        objCustomerDetail = new CustomerDetail();
                        objCustomerDetail.IdNo = "Record does not exists!";
                        objCustomerDetail.Name = "";
                    }

                }
                else
                {
                    objCustomerDetail = new CustomerDetail();
                    objCustomerDetail.IdNo = "Record does not exists!";
                    objCustomerDetail.Name = "";
                }

            }
            catch (Exception ex)
            {
                objCustomerDetail = new CustomerDetail();
                objCustomerDetail.IdNo = "Record does not exists!";
                objCustomerDetail.Name = "";
            }
            return objCustomerDetail;
        }
        public async Task<MemberAPIRoot> ValidateCustomerbyAPI(string IdNo, string Password)
        {
            MemberAPIRoot memapi = new MemberAPIRoot();
            try
            {
                string MemberResponse = await GetMemeberPasswordDetail(IdNo, Password);
                if (!string.IsNullOrEmpty(MemberResponse))
                {
                    memapi = JsonSerializer.Deserialize<MemberAPIRoot>(MemberResponse);
                    if (memapi.Success == "true")
                    {
                        string FPBALbalance = GetMemeberBalance("FPBAL", IdNo);
                        var fpvpalres = JsonSerializer.Deserialize<BalanceAPIRoot>(FPBALbalance);
                        if (fpvpalres.Success == "true")
                        {
                            memapi.Result.Fpv_Balance = fpvpalres.Balance;
                        }
                        var VOUCHERBAL = GetMemeberBalance("VOUCHERBAL", IdNo);
                        var VOUCHERBALres = JsonSerializer.Deserialize<BalanceAPIRoot>(VOUCHERBAL);
                        if (VOUCHERBALres.Success == "true")
                        {
                            memapi.Result.VOUCHERBAL_Balance = VOUCHERBALres.Balance;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return memapi;
        }
        private string GetMemeberBalance(string act, string Username)
        {
            string Response = string.Empty;
            try
            {
                string jsonrequest = "{\"act\":\"" + act + "\",\"uid\":\"" + Username + "\",\"logkey\":\"ZoewellnessgugddkhJJHJsddd\"}";
                HttpWebRequest request = WebRequest.Create("https://zoewellness.co.in/Api/api.ashx") as HttpWebRequest;
                request.ContentType = @"application/json";
                request.Method = @"POST";
                StreamWriter requestWriter = new StreamWriter(request.GetRequestStream());
                requestWriter.Write(jsonrequest);
                requestWriter.Close();
                HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse();
                StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
                Response = responseReader.ReadToEnd();
            }
            catch (Exception ex)
            {

            }
            return Response;
        }
        private async Task<string> GetMemeberPasswordDetail(string Username, string Password)
        {
            string Response = string.Empty;
            try
            {
                string jsonrequest = "{\"act\":\"CHECKPASS\",\"uid\":\"" + Username + "\",\"pwd\":\"" + Password + "\",\"logkey\":\"ZoewellnessgugddkhJJHJsddd\"}";
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "https://zoewellness.co.in/Api/api.ashx");
                var content = new StringContent(jsonrequest, null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                Response = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {

            }
            return Response;
        }
       
        //private async Task<string> GetMemeberDetail(string Username)
        //{
        //    string Response = string.Empty;
        //    try
        //    {
        //        var client = new HttpClient();
        //        var request = new HttpRequestMessage(HttpMethod.Post, "https://zoewellness.co.in/Api/api.ashx");
        //        var content = new StringContent("{\"act\":\"LOGIN\",\"uid\":\"" + Username + "\",\"pwd\":\"234\",\"logkey\":\"ZoewellnessgugddkhJJHJsddd\"}", null, "application/json");
        //        request.Content = content;
        //        var response = await client.SendAsync(request);
        //        response.EnsureSuccessStatusCode();
        //        Response = await response.Content.ReadAsStringAsync();
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return Response;
        //}
        private async Task<string> GetMemeberDetail(string Username, string Password)
        {
            string Response = string.Empty;
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "https://zoewellness.co.in/Api/api.ashx");
                var content = new StringContent("{\"act\":\"LOGIN\",\"uid\":\"" + Username + "\",\"pwd\":\"" + Password + "\",\"logkey\":\"ZoewellnessgugddkhJJHJsddd\"}", null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                Response = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {

            }
            return Response;
        }

        public async Task<List<ProductModel>> GetproductInfoBatchWise(string SearchType, string data, bool isCForm, string BillType, decimal CurrentStateCode, string CurrentPartyCode, bool IsBillOnMrp, string OfferID, bool allhalf, string Invoice, string IsSpclOffer)
        {
            List<ProductModel> objProductModel = new List<ProductModel>();
            List<ProductModel> TempResult = new List<ProductModel>();
            bool searchByProductFlag = true;
            if (SearchType == "B")
            {
                searchByProductFlag = false;
            }

            try
            {
                if (!string.IsNullOrEmpty(data))
                {
                    using (var connection = _context.CreateLiveconnInv())
                    {

                        if (searchByProductFlag)
                        {

                            var TempResultSql = @"SELECT 
                                                 product.ProductName,
                                                 product.CatId,
                                                 c.CatName,
                                                 barcode.BarCode AS Barcode,
                                                          product.RP,
                                                 product.Discount AS DiscPer,
                                                 product.DiscInRs AS DiscAmt,
                                                 CAST(product.ProductCode AS INT) AS ProdCode,
                                                 product.ProdId AS ProductCodeStr,
                                                 tax.VatTax AS TaxPer,
                                                 tax.StateCode AS ProdStateCode,
                                                 product.MRP,
                                                 product.BV,
                                                 product.PV,
                                                 product.CV,
                                                 CASE WHEN product.IsExpired = 'Y' THEN 1 ELSE 0 END AS IsExpirable,
                                                 product.ExpDate,
                                                 'GST' AS TaxType,
                                                 product.PurchaseRate AS Rate,
                                                 product.ProdCommssn AS CommissionPer,
                                                 product.SubCatId,
                                                 product.IsAvailableforOffers AS IsAvailableForOffer,
                                                 product.IsBillingAllowed AS IsAvailableForBilling,
                                                 product.Weight,
                                                 COALESCE(product.SJDiscount, 0) AS TotalDiscPer
                                             FROM 
                                                 M_ProductMaster product
                                             JOIN 
                                                 M_BarCodeMaster barcode ON product.ProdId = barcode.ProdId
                                             JOIN 
                                                 M_TaxMaster tax ON product.ProdId = tax.ProdCode
                                             JOIN 
                                                 M_CatMaster c ON c.CatId = product.CatId
                                            
                                             WHERE 
                                                 LOWER(product.ProductName) = LOWER(@data)
                                                 AND product.ActiveStatus = 'Y'
                                                 AND barcode.ActiveStatus = 'Y'
                                               
                                                 ";
                            var parameters = new { data = data };
                            TempResult = (await connection.QueryAsync<ProductModel>(TempResultSql, parameters)).ToList();


                        }
                        else
                        {
                            var TempResultSql = @"SELECT 
                                                  p.ProductName,
                                                  p.CatId,
                                                  c.CatName,
                                                  b.BarCode AS Barcode,
                                                  p.BatchNo,
                                                  p.DP,
                                                  p.RP,
                                                  t.StateCode AS ProdStateCode,
                                                  p.Discount AS DiscPer,
                                                  p.DiscInRs AS DiscAmt,
                                                  CAST(p.ProductCode AS INT) AS ProdCode,
                                                  p.ProdId AS ProductCodeStr,
                                                  t.VatTax AS TaxPer,
                                                  p.MRP,
                                                  p.BV,
                                                  p.PV,
                                                  p.CV,
                                                  CASE WHEN p.IsExpired = 'Y' THEN 1 ELSE 0 END AS IsExpirable,
                                                  batch.ExpDate,
                                                  'GST' AS TaxType,
                                                  p.PurchaseRate AS Rate,
                                                  p.ProdCommssn AS CommissionPer,
                                                  p.IsAvailableforOffers AS IsAvailableForOffer,
                                                  ISNULL(p.SJDiscount, 0) AS TotalDiscPer
                                                  FROM 
                                                      M_ProductMaster p
                                                      JOIN M_BarCodeMaster b ON p.ProdId = b.ProdId
                                                      JOIN M_TaxMaster t ON p.ProdId = t.ProdCode
                                                      JOIN M_CatMaster c ON p.CatId = c.CatId
                                                  WHERE 
                                                      p.ActiveStatus = 'Y' 
                                                      AND p.IsCardIssue = 'N'
                                                      AND b.Barcode = @Barcode
                                                     
                                                       ";
                            var parameters = new { Barcode = data };
                            TempResult = (await connection.QueryAsync<ProductModel>(TempResultSql, parameters)).ToList();
                        }

                        foreach (var obj in TempResult)
                        {
                            if (allhalf)
                            {

                                obj.BV = obj.BV / 2;
                                obj.PV = obj.PV / 2;
                                obj.RP = obj.RP / 2;
                                obj.DiscAmt = obj.DP;
                                obj.IsDiscountAdd = 1;
                            }
                            if (!string.IsNullOrEmpty(Invoice) && Invoice == "CED")
                            {
                                obj.BV = 0;
                                obj.PV = (obj.PV * 1) / 4;
                                obj.DiscAmt = obj.DP;
                                obj.IsDiscountAdd = 1;
                            }

                            objProductModel.Add(obj);
                        }

                        PopulateBatchDetails(objProductModel, BillType, CurrentStateCode, CurrentPartyCode, IsBillOnMrp, OfferID, allhalf, Invoice, IsSpclOffer);


                    }
                }
            }
            catch (Exception e)
            {

            }
            return objProductModel;
        }

        private async Task PopulateBatchDetails(List<ProductModel> products, string BillType, decimal CurrentStateCode, string CurrentPartyCode, bool IsBillOnMrp, string OfferID, bool allhalf, string Invoice, string IsSpclOffer)
        {
            bool IsDistributorBill = false;
            bool IsPartyBill = false;
            bool IsCustomerBill = false;
            bool IsPurchaseInvoice = false;
            bool IsOrderCreation = false;
            bool IsPendingOrder = false;
            if (BillType == "distributor")
            {
                IsDistributorBill = true;
            }
            else
            {
                IsDistributorBill = false;
            }
            if (BillType == "party")
            {
                IsPartyBill = true;
            }
            else
            {
                IsPartyBill = false;
            }
            if (BillType == "customer")
            {
                IsCustomerBill = true;
            }
            else
            {
                IsCustomerBill = false;
            }
            if (BillType == "purchase")
            {
                IsPurchaseInvoice = true;
            }
            else
            {
                IsPurchaseInvoice = false;
            }
            if (BillType == "order")
            {
                IsOrderCreation = true;
            }
            else
            {
                IsOrderCreation = false;
            }
            if (BillType == "pendingorder")
            {
                IsPendingOrder = true;
            }
            else
            {
                IsPendingOrder = false;
            }

            using (var connection = _context.CreateLiveconnInv())
            {
                foreach (var product in products)
                {
                    // Fetch batch details for each product

                    //var batchDetails = (from batch in M_BatchMaster
                    //                    where batch.ProdId == product.ProductCodeStr // or however you identify the product
                    //                                                                 && batch.ActiveStatus == "Y" // ensuring the batch is active
                    //                    select new ProductBatchModel
                    //                    {
                    //                        ProductName = product.ProductName,
                    //                        ProdCode = product.ProdCode,
                    //                        ProductCodeStr = product.ProductCodeStr,
                    //                        ProdId = batch.ProdId,
                    //                        BatchNo = batch.BatchNo,
                    //                        Batchcode = batch.BatchNo, // or any other unique code you have
                    //                        Barcode = product.Barcode,
                    //                        IsExpirable = batch.IsExpired == "Y",
                    //                        ExpDate = batch.ExpDate,
                    //                        MRP = batch.Mrp, // ensure you have MRP field in your batch model
                    //                        DP = batch.Dp, // ensuring you have DP field in your batch model
                    //                        BV = batch.Bv,
                    //                        PV = product.PV,
                    //                        CV = product.CV,
                    //                        RP = product.RP,
                    //                        DiscPer = product.DiscPer,
                    //                        DiscAmt = product.DiscAmt,
                    //                        TaxType = "GST",
                    //                        Rate = product.Rate,
                    //                        Weight = product.Weight,
                    //                        CommissionPer = product.CommissionPer,
                    //                        IsAvailableForOffer = product.IsAvailableForOffer,
                    //                        TotalDiscPer = product.TotalDiscPer,
                    //                        TaxPer = product.TaxPer,
                    //                        StockAvailable = (from stockAvail in entity.Im_CurrentStock
                    //                                          where stockAvail.ProdId == product.ProductCodeStr.ToString() &&
                    //                                                stockAvail.FCode.Equals(CurrentPartyCode)
                    //                                                && stockAvail.BatchCode == batch.BatchNo
                    //                                          select stockAvail.Qty
                    //                                         ).DefaultIfEmpty(0).Sum()
                    //                    }).ToList();

                    var query = @"
                                                        
SELECT 
                                                            p.ProductName,
                                                            p.Prodid as ProdCode,
                                                           p.Prodid as ProductCodeStr,
                                                            b.ProdId,
                                                            b.BatchNo,
                                                            b.BatchNo AS Batchcode,
                                                            p.Barcode,
                                                            CASE WHEN b.IsExpired = 'Y' THEN 1 ELSE 0 END AS IsExpirable,
                                                            b.ExpDate,
                                                            b.Mrp AS MRP,
                                                            b.Dp AS DP,
                                                            b.Bv AS BV,
                                                            b.PV,
                                                            p.CV,
                                                            p.RP,
                                                            'GST' AS TaxType,
                                                            p.PurchaseRate as Rate,p.ProdCommssn as CommissionPer,
                                                            p.Weight,p.Discount as DiscPer,p.DiscInRs as DiscAmt,
                                                            ISNULL(stock.Qty, 0) AS StockAvailable, tax.VatTax AS TaxPer,
                                                        tax.StateCode AS ProdStateCode
                                                        FROM M_BatchMaster b
                                                        INNER JOIN M_Productmaster p ON p.Prodid = b.ProdId
                                                        Inner Join M_TaxMaster as tax on p.Prodid=tax.Prodcode
                                                        Inner JOIN (
                                                            SELECT 
                                                                ProdId,
                                                                BatchCode,
                                                                SUM(Qty) AS Qty
                                                            FROM Im_CurrentStock
                                                            WHERE FCode = @CurrentPartyCode
                                                            GROUP BY ProdId, BatchCode
                                                        ) stock ON 
														stock.BatchCode = b.BatchNo and stock.Qty>0
                                                        WHERE 
                                                            b.ProdId = stock.ProdId
                                                            AND b.ActiveStatus = 'Y'  and b.Prodid=@ProductCodeStr;
                                                    ";

                    // Define parameters
                    var parameters = new
                    {
                        ProductCodeStr = product.ProductCodeStr,
                        CurrentPartyCode = CurrentPartyCode
                    };

                    // Execute the query with Dapper
                    var batchDetails = connection.Query<ProductBatchModel>(query, parameters).ToList();


                    // Initialize a new list to store valid batches
                    List<ProductBatchModel> validBatchDetails = new List<ProductBatchModel>();

                    foreach (var obj in batchDetails)
                    {
                        if (obj.StockAvailable > 0)
                        {
                            if ((obj.IsExpirable && obj.ExpDate > DateTime.Now) || !obj.IsExpirable)
                            {
                                // Logic to process TempObj
                                ProductBatchModel TempObj = obj;  // Process obj directly without re-adding it
                                object valueIsDiscountAdd = 0;
                                object valueIsCommissonAdd = 0;

                                if (IsDistributorBill || IsCustomerBill || IsPurchaseInvoice || IsOrderCreation || IsPendingOrder)
                                {
                                    valueIsCommissonAdd = Enum.Parse(typeof(Enums.CalculationConditionalVar), Enums.CalculationConditionalVar.IsCommissonAdd.ToString());
                                    valueIsDiscountAdd = Enum.Parse(typeof(Enums.CalculationConditionalVar), Enums.CalculationConditionalVar.IsDiscountAdd.ToString());
                                }
                                else
                                {
                                    valueIsCommissonAdd = Enum.Parse(typeof(Enums.CalculationConditionalVar), Enums.CalculationConditionalVar.IsCommissonAddOnPartyBill.ToString());
                                    valueIsDiscountAdd = Enum.Parse(typeof(Enums.CalculationConditionalVar), Enums.CalculationConditionalVar.IsDiscountAddOnPartyBill.ToString());
                                }

                                int IsCommission = Convert.ToInt32(valueIsCommissonAdd);
                                int IsDiscount = Convert.ToInt32(valueIsDiscountAdd);
                                product.IsCommissionAdd = IsCommission;
                                product.IsDiscountAdd = IsDiscount;

                                product.DP1 = TempObj.DP;

                                if (IsCustomerBill)
                                {
                                    TempObj.DP = obj.MRP;
                                }
                                else
                                {
                                    if (!IsPurchaseInvoice && IsBillOnMrp)
                                    {
                                        TempObj.DP = obj.MRP;
                                    }
                                }

                                if (allhalf)
                                {
                                    TempObj.DP = TempObj.DP / 2;
                                    TempObj.RP = TempObj.RP / 2;
                                }

                                if (!string.IsNullOrEmpty(Invoice) && Invoice == "CED")
                                {
                                    TempObj.DP = (TempObj.DP * 1) / 4;
                                    TempObj.RP = (TempObj.RP * 1) / 4;
                                }

                                if (!string.IsNullOrEmpty(OfferID))
                                {
                                    decimal iOfferID = Convert.ToDecimal(OfferID);
                                    if (iOfferID != 0)
                                    {
                                        TempObj.offerDetail = await GetOfferDetail(iOfferID, obj.ProductCodeStr, IsSpclOffer);
                                        if (!string.IsNullOrEmpty(product.offerDetail.offerType))
                                        {
                                            decimal offerType = Convert.ToDecimal(product.offerDetail.offerType);
                                            if (offerType == 2 || offerType == 3)
                                            {
                                                TempObj.DP = product.offerDetail.OfferMrp / 1;
                                                product.OfferProdQty = product.offerDetail.FreeQty;
                                                product.OfferProdQty = 1;
                                                product.BV = 0;
                                            }
                                        }
                                    }
                                }

                                // Add processed TempObj to the valid batch list
                                validBatchDetails.Add(TempObj);
                            }
                        }

                    }

                    // Assign the filtered batch list to the product
                    product.batchdetail = validBatchDetails;
                }

                // Optionally, filter and sort products by stock and expiration date
                //if (products.Count > 1 && !IsPurchaseInvoice)
                //{
                //    products = products.Where(m => m.StockAvailable > 0)
                //                       .OrderBy(m => m.ExpDate)
                //                       .ThenBy(m => m.StockAvailable)
                //                       .ToList();
                //}
            }

        }


        public async Task<List<ProductModel>> GetproductInfo(string SearchType, string data, bool isCForm, string BillType, decimal CurrentStateCode, string CurrentPartyCode, bool IsBillOnMrp, string OfferID, bool allhalf, string Invoice, string IsSpclOffer)
        {
            List<ProductModel> objProductModel = new List<ProductModel>();
            try
            {
                List<ProductModel> TempResult = new List<ProductModel>();
                bool searchByProductFlag = true;
                if (SearchType == "B")
                {
                    searchByProductFlag = false;
                }
                using (var connection = _context.CreateLiveconnInv())
                {
                    if (!string.IsNullOrEmpty(data))
                    {
                        if (searchByProductFlag)
                        {
                            var TempResultSql = @"SELECT 
                                                 product.ProductName,
                                                 product.CatId,
                                                 c.CatName,
                                                 barcode.BarCode AS Barcode,
                                                 batch.BatchNo,
                                                 batch.DP,
                                                 product.RP,
                                                 product.Discount AS DiscPer,
                                                 product.DiscInRs AS DiscAmt,
                                                 CAST(product.ProductCode AS INT) AS ProdCode,
                                                 product.ProdId AS ProductCodeStr,
                                                 tax.VatTax AS TaxPer,
                                                 tax.StateCode AS ProdStateCode,
                                                 batch.MRP,
                                                 product.BV,
                                                 product.PV,
                                                 product.CV,
                                                 CASE WHEN batch.IsExpired = 'Y' THEN 1 ELSE 0 END AS IsExpirable,
                                                 batch.ExpDate,
                                                 'GST' AS TaxType,
                                                 product.PurchaseRate AS Rate,
                                                 product.ProdCommssn AS CommissionPer,
                                                 product.SubCatId,
                                                 product.IsAvailableforOffers AS IsAvailableForOffer,
                                                 product.IsBillingAllowed AS IsAvailableForBilling,
                                                 product.Weight,
                                                 COALESCE(product.SJDiscount, 0) AS TotalDiscPer
                                             FROM 
                                                 M_ProductMaster product
                                             JOIN 
                                                 M_BarCodeMaster barcode ON product.ProdId = barcode.ProdId
                                             JOIN 
                                                 M_TaxMaster tax ON product.ProdId = tax.ProdCode
                                             JOIN 
                                                 M_CatMaster c ON c.CatId = product.CatId
                                             JOIN
											    M_BatchMaster batch on product.ProdId = batch.ProdId
                                             WHERE 
                                                 LOWER(product.ProductName) = LOWER(@data)
                                                 AND product.ActiveStatus = 'Y'
                                                 AND barcode.ActiveStatus = 'Y'
                                                 AND batch.ActiveStatus = 'Y'
                                                 ";
                            var parameters = new { data = data };
                            TempResult = (await connection.QueryAsync<ProductModel>(TempResultSql, parameters)).ToList();
                        }
                        else
                        {
                            //decimal? BarCodeData = decimal.Parse(data);
                            var TempResultSql = @"SELECT 
                                                  p.ProductName,
                                                  p.CatId,
                                                  c.CatName,
                                                  b.BarCode AS Barcode,
                                                  batch.BatchNo,
                                                  batch.DP,
                                                  p.RP,
                                                  t.StateCode AS ProdStateCode,
                                                  p.Discount AS DiscPer,
                                                  p.DiscInRs AS DiscAmt,
                                                  CAST(p.ProductCode AS INT) AS ProdCode,
                                                  p.ProdId AS ProductCodeStr,
                                                  t.VatTax AS TaxPer,
                                                  batch.MRP,
                                                  p.BV,
                                                  p.PV,
                                                  p.CV,
                                                  CASE WHEN batch.IsExpired = 'Y' THEN 1 ELSE 0 END AS IsExpirable,
                                                  batch.ExpDate,
                                                  'GST' AS TaxType,
                                                  p.PurchaseRate AS Rate,
                                                  p.ProdCommssn AS CommissionPer,
                                                  p.IsAvailableforOffers AS IsAvailableForOffer,
                                                  ISNULL(p.SJDiscount, 0) AS TotalDiscPer
                                                  FROM 
                                                      M_ProductMaster p
                                                      JOIN M_BarCodeMaster b ON p.ProdId = b.ProdId
                                                      JOIN M_TaxMaster t ON p.ProdId = t.ProdCode
                                                      JOIN M_CatMaster c ON p.CatId = c.CatId
                                                      JOIN
											          M_BatchMaster batch on p.ProdId = batch.ProdId
                                                  WHERE 
                                                      p.ActiveStatus = 'Y' 
                                                      AND p.IsCardIssue = 'N'
                                                      AND batch.BatchNo = @Barcode
                                                      AND batch.ActiveStatus = 'Y'
                                                       ";
                            var parameters = new { Barcode = data };
                            TempResult = (await connection.QueryAsync<ProductModel>(TempResultSql, parameters)).ToList();
                        }
                        bool IsDistributorBill = false;
                        bool IsPartyBill = false;
                        bool IsCustomerBill = false;
                        bool IsPurchaseInvoice = false;
                        bool IsOrderCreation = false;
                        bool IsPendingOrder = false;
                        if (BillType == "distributor")
                        {
                            IsDistributorBill = true;
                        }
                        else
                        {
                            IsDistributorBill = false;
                        }
                        if (BillType == "party")
                        {
                            IsPartyBill = true;
                        }
                        else
                        {
                            IsPartyBill = false;
                        }
                        if (BillType == "customer")
                        {
                            IsCustomerBill = true;
                        }
                        else
                        {
                            IsCustomerBill = false;
                        }
                        if (BillType == "purchase")
                        {
                            IsPurchaseInvoice = true;
                        }
                        else
                        {
                            IsPurchaseInvoice = false;
                        }
                        if (BillType == "order")
                        {
                            IsOrderCreation = true;
                        }
                        else
                        {
                            IsOrderCreation = false;
                        }
                        if (BillType == "pendingorder")
                        {
                            IsPendingOrder = true;
                        }
                        else
                        {
                            IsPendingOrder = false;
                        }
                        foreach (var obj in TempResult)
                        {
                            ProductModel TempObj = new ProductModel();
                            if ((obj.IsExpirable && obj.ExpDate > DateTime.Now) || (obj.IsExpirable == false))
                            {
                                TempObj = obj;
                                object valueIsDiscountAdd = 0;
                                object valueIsCommissonAdd = 0;
                                if (IsDistributorBill || IsCustomerBill || IsPurchaseInvoice || IsOrderCreation || IsPendingOrder)
                                {
                                    valueIsCommissonAdd = Enum.Parse(typeof(Enums.CalculationConditionalVar), Enums.CalculationConditionalVar.IsCommissonAdd.ToString());
                                    valueIsDiscountAdd = Enum.Parse(typeof(Enums.CalculationConditionalVar), Enums.CalculationConditionalVar.IsDiscountAdd.ToString());
                                }
                                else
                                {
                                    valueIsCommissonAdd = Enum.Parse(typeof(Enums.CalculationConditionalVar), Enums.CalculationConditionalVar.IsCommissonAddOnPartyBill.ToString());
                                    valueIsDiscountAdd = Enum.Parse(typeof(Enums.CalculationConditionalVar), Enums.CalculationConditionalVar.IsDiscountAddOnPartyBill.ToString());
                                }
                                int IsCommission = Convert.ToInt32(valueIsCommissonAdd);
                                int IsDiscount = Convert.ToInt32(valueIsDiscountAdd);
                                TempObj.IsCommissionAdd = IsCommission;
                                TempObj.IsDiscountAdd = IsDiscount;
                                var StockAvailableSQL = "select ISNULL(sum(Qty),0) from Im_CurrentStock where Barcode='" + TempObj.Barcode.ToString() + "' and ProdId=" + TempObj.ProductCodeStr.ToString() + " and FCode='" + CurrentPartyCode + "' and BatchCode='" + TempObj.BatchNo + "'";
                                TempObj.StockAvailable = await connection.QuerySingleOrDefaultAsync<decimal>(StockAvailableSQL);
                                TempObj.DP1 = TempObj.DP;
                                if (IsCustomerBill)
                                {
                                    TempObj.DP = obj.MRP;
                                }
                                else
                                {
                                    if (!IsPurchaseInvoice && IsBillOnMrp)
                                    {
                                        TempObj.DP = obj.MRP;
                                    }
                                }
                                var compsql = "select CompState from M_CompanyMaster";
                                CurrentStateCode = await connection.QuerySingleOrDefaultAsync<decimal>(StockAvailableSQL);
                                if (allhalf)
                                {
                                    TempObj.DP = TempObj.DP / 2;
                                    TempObj.BV = TempObj.BV / 2;
                                    TempObj.PV = TempObj.PV / 2;
                                    TempObj.RP = TempObj.RP / 2;
                                    TempObj.DiscAmt = TempObj.DP;
                                    TempObj.IsDiscountAdd = 1;
                                }
                                if (!string.IsNullOrEmpty(Invoice) && Invoice == "CED")
                                {
                                    var oridp = TempObj.DP;
                                    TempObj.DP = (TempObj.DP * 1) / 4;
                                    TempObj.BV = 0;
                                    TempObj.PV = (TempObj.PV * 1) / 4;
                                    TempObj.RP = (TempObj.RP * 1) / 4;
                                    TempObj.DiscAmt = oridp - TempObj.DP;
                                    TempObj.IsDiscountAdd = 1;
                                }
                                if (!string.IsNullOrEmpty(OfferID))
                                {
                                    decimal iOfferID = Convert.ToDecimal(OfferID);
                                    if (iOfferID != 0)
                                    {
                                        TempObj.offerDetail = await GetOfferDetail(iOfferID, obj.ProductCodeStr, IsSpclOffer);
                                        if (!string.IsNullOrEmpty(TempObj.offerDetail.offerType))
                                        {
                                            decimal offerType = Convert.ToDecimal(TempObj.offerDetail.offerType);
                                            if (offerType == 2 || offerType == 3)
                                            {
                                                TempObj.DP = TempObj.offerDetail.OfferMrp / 1;// TempObj.offerDetail.FreeQty;
                                                TempObj.OfferProdQty = TempObj.offerDetail.FreeQty;
                                                TempObj.OfferProdQty = 1;
                                                TempObj.BV = 0;
                                            }
                                        }
                                    }
                                }
                                objProductModel.Add(TempObj);
                            }
                        }
                        objProductModel = objProductModel.Where(m => m.StockAvailable > 0).OrderBy(m => m.ExpDate).ThenBy(m => m.StockAvailable).ToList();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return objProductModel;
        }
        public async Task<OfferProducts> GetOfferDetail(decimal offerId, string prodCode, string IsSpclOffer)
        {
            OfferProducts objProds = new OfferProducts();
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    if (IsSpclOffer.ToLower() == "false")
                    {
                        M_Offers Offers = new M_Offers();
                        string sql = "SELECT * FROM M_Offers WHERE ActiveStatus = 'Y' AND AID = @AID";
                        Offers = await connection.QueryFirstOrDefaultAsync<M_Offers>(sql, new { AID = offerId });
                        var MinBV = Offers.OfferOnBV;
                        var MinToBV = Offers.OfferOnToBV;
                        var TotalQty = Offers.TotalQty;
                        var offerType = Offers.OfferType;
                        string objProdssql = @"
                                       SELECT 
                                       ProdID, 
                                       @offerType AS offerType, 
                                       @MinBV AS MinBV, 
                                       @MinToBV AS MinToBV, 
                                       @TotalQty AS TotalQty, 
                                       Qty, 
                                       FreeQty, 
                                       IsFlexible, 
                                       OfferMrp 
                                       FROM M_OfferProducts 
                                       WHERE OfferID = @OfferID AND ProdID = @ProdID AND IsBuyProduct != 'Y'";
                        objProds = await connection.QueryFirstOrDefaultAsync<OfferProducts>(objProdssql, new
                        {
                            OfferID = offerId,
                            ProdID = prodCode,
                            offerType,
                            MinBV,
                            MinToBV,
                            TotalQty
                        });
                    }
                    else
                    {
                        M_OtherOffers Offers = new M_OtherOffers();
                        string sql = "SELECT * FROM M_Offers WHERE  AND AID = @AID";
                        Offers = await connection.QueryFirstOrDefaultAsync<M_OtherOffers>(sql, new { AID = offerId });
                        var MinBV = Offers.MinBV;
                        var MinToBV = Offers.OfferOnToBV;
                        var TotalQty = 0;
                        var offerType = 9999;
                        string objProdssql = @"
                                       SELECT 
                                       ProdID, 
                                       @offerType AS offerType, 
                                       @MinBV AS MinBV, 
                                       @MinToBV AS MinToBV, 
                                       @TotalQty AS TotalQty, 
                                       Qty, 
                                       FreeQty, 
                                       IsFlexible, 
                                       OfferMrp 
                                       FROM M_OfferProducts 
                                       WHERE OfferID = @OfferID AND ProdID = @ProdID AND IsBuyProduct != 'Y'";
                        objProds = await connection.QueryFirstOrDefaultAsync<OfferProducts>(objProdssql, new
                        {
                            OfferID = offerId,
                            ProdID = prodCode,
                            offerType,
                            MinBV,
                            MinToBV,
                            TotalQty
                        });
                    }
                }
            }
            catch
            {

            }
            return objProds;
        }
        public async Task<ResponseDetail> SaveDistributorBill(DistributorBillModel objModel)
        {
            ResponseDetail objResponse = new ResponseDetail();
            DistributorBillModel TempDistributor = new DistributorBillModel();
            try
            {
                int TrnBillDatasAffected = 0;
                decimal maxUserSBillNo = 0;
                decimal? SessId = 0;
                string billPrefix = "";
                decimal maxSbillNo = 0;
                decimal? FsessId = 0;
                string UserBillNo = "";
                string version = "";
                //SqlTransaction objTrans = null;
                decimal WalletBalance = 0;
                decimal LastBillAmt = 0;
                int NewKitId = 0;
                string NewKitName = "";
                TrnPayModeDetail objDtPayModeDetail = new TrnPayModeDetail();
                List<string> Paymode = new List<string>();
                List<string> PayPrefix = new List<string>();
                List<TrnPayModeDetail> objDTListPayMode = new List<TrnPayModeDetail>();
                objResponse.ResponseMessage = "Something went wrong!";
                objResponse.ResponseStatus = "FAILED";
                string billno_ = "", narration_ = "", soldby_ = "", fcode_ = "";
                decimal netpayable_ = 0;
                using (var connection = _context.CreateLiveconnInv())
                {
                    var db = ConfigurationManager.AppSetting.GetSection("Database").Value;
                    var sql = "Select Max(SessID) as MaxSessId from " + db + "..M_SessnMaster";
                    SessId = (await connection.QueryAsync<decimal>(sql, commandType: CommandType.Text)).FirstOrDefault();

                    string maxSBillNosql = @"
                                          SELECT ISNULL(MAX(SBillNo), 0)
                                          FROM TrnBillMain";

                    maxSbillNo = await connection.QuerySingleOrDefaultAsync<int>(maxSBillNosql);
                    maxSbillNo = maxSbillNo + 1;

                    var FsessIdsql = "select  FSessId from M_FiscalMaster where ActiveStatus='Y'";
                    FsessId = (await connection.QueryAsync<decimal>(FsessIdsql, commandType: CommandType.Text)).FirstOrDefault();

                    var billPrefixsql = "select BillPrefix from M_ConfigMaster";
                    billPrefix = (await connection.QueryAsync<string>(billPrefixsql, commandType: CommandType.Text)).FirstOrDefault();

                    string BillSeries = "";
                    string BillSeriesquery = @"
                                             SELECT MAX(BillSeries) 
                                             FROM M_FiscalMaster 
                                             WHERE ActiveStatus = @ActiveStatus";

                    var billSeries = connection.QueryFirstOrDefault<string>(
                        BillSeriesquery,
                        new { ActiveStatus = "Y" }
                    );
                    BillSeries = billSeries?.TrimEnd();

                    var maxUserSBillNoSql = "";
                    if (!string.IsNullOrEmpty(objModel.TaxORStock) && objModel.TaxORStock.ToLower() == "stock")
                    {
                        maxUserSBillNoSql = @"SELECT ISNULL(MAX(UserSBillNo), 0)
                                              FROM TrnBillMain
                                              WHERE FSessId = @FsessId
                                              AND SoldBy = @SoldBy
                                              AND BillType = 'S'";
                        var parameters = new
                        {
                            @FsessId = FsessId,
                            @SoldBy = objModel.objCustomer.UserDetails.PartyCode
                        };
                        maxUserSBillNo = await connection.QuerySingleAsync<int>(maxUserSBillNoSql, parameters);
                    }
                    else
                    {
                        maxUserSBillNoSql = @"
                                              SELECT ISNULL(MAX(UserSBillNo),0)
                                              FROM TrnBillMain
                                              WHERE FSessId = @FsessId AND SoldBy = @PartyCode AND BillType != 'S'";
                        var parameters = new
                        {
                            @FsessId = FsessId,
                            @PartyCode = objModel.objCustomer.UserDetails.PartyCode
                        };
                        maxUserSBillNo = await connection.QuerySingleAsync<int>(maxUserSBillNoSql, parameters);
                    }
                    maxUserSBillNo = maxUserSBillNo + 1;
                    string strMaxUserSBillNo = maxUserSBillNo.ToString();
                    if (strMaxUserSBillNo.Count() < 5)
                    {
                        var countNum = strMaxUserSBillNo.Count();
                        var ToBeAddedDigits = 5 - countNum;
                        for (var j = 0; j < ToBeAddedDigits; j++)
                        {
                            strMaxUserSBillNo = "0" + strMaxUserSBillNo;
                        }
                    }
                    string sqlQuery = @"
                                       SELECT TOP 1 UserPartyCode 
                                       FROM M_LedgerMaster 
                                       WHERE ActiveStatus = 'Y' 
                                       AND PartyCode = @PartyCode";
                    var userPCode = await connection.QueryFirstOrDefaultAsync<string>(
                                                sqlQuery,
                                              new { @PartyCode = objModel.objCustomer.UserDetails.PartyCode }
                                          );
                    if (!string.IsNullOrEmpty(objModel.TaxORStock) && objModel.TaxORStock.ToLower() == "stock")
                    {
                        //UserBillNo = billPrefix + "/ST/" + userPCode + "/" + strMaxUserSBillNo;
                        UserBillNo = billPrefix + "/ST/" + userPCode + "/" + BillSeries + "/" + strMaxUserSBillNo;
                    }
                    else
                    {
                        // UserBillNo = billPrefix + "/" + userPCode + "/" + strMaxUserSBillNo;
                        UserBillNo = billPrefix + "/" + userPCode + "/" + BillSeries + "/" + strMaxUserSBillNo;
                    }
                    var versionsql = "select VersionNo from M_NewHOVersionInfo";
                    version = (await connection.QueryAsync<string>(versionsql, commandType: CommandType.Text)).FirstOrDefault();
                    bool IsWalletEntry = false;
                    if (string.IsNullOrEmpty(objModel.BillType) || objModel.BillType == "X" || objModel.BillType == "J" || objModel.BillType == "F" || objModel.BillType == "G" || objModel.BillType == "C" || objModel.BillType == "P")
                    {
                        if (objModel != null)
                        {
                            if (objModel.SelectedInvoiceType == "BV")
                            {
                                if (objModel.objProduct.PayDetails.IsV)
                                {
                                    EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Voucher;
                                    string value = EnumPayModes.GetEnumDescription(enumVar);
                                    PayPrefix.Add(value);
                                    objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = DateTime.Now.Date, BillType = "V", BillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo, PayPrefix = value, Amount = objModel.objProduct.PayDetails.AmountByVoucher, CardNo = "", AcNo = "", IFSCode = "", BankCode = 0, DUserId = 0, DRecTimeStamp = null, ChqDDDate = null, ChqDDNo = "", Narration = "", BankName = "", ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                                }
                                if (objModel.objProduct.PayDetails.IsCU)
                                {
                                    EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Coupon;
                                    string value = EnumPayModes.GetEnumDescription(enumVar);
                                    PayPrefix.Add(value);
                                    objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = DateTime.Now.Date, BillType = "V", BillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo, PayPrefix = value, Amount = objModel.objProduct.PayDetails.AmountByVoucher, CardNo = "", AcNo = "", IFSCode = "", BankCode = 0, DUserId = 0, DRecTimeStamp = null, ChqDDDate = null, ChqDDNo = "", Narration = "", BankName = "", ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                                }
                                if (objModel.objProduct.PayDetails.IsW)
                                {
                                    if (objModel.objProduct.PayDetails.AmountByWallet > 0)
                                    {
                                        EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Wallet;
                                        string value = EnumPayModes.GetEnumDescription(enumVar);
                                        PayPrefix.Add(value);
                                        objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable,
                                            SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = DateTime.Now.Date, 
                                            BillType = objModel.objCustomer.SelectedInvoiceType,
                                            BillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo, 
                                            PayPrefix = value, Amount = objModel.objProduct.PayDetails.AmountByWallet,
                                            BankCode = 0, BankName = "", AcNo = "", IFSCode = "", Narration = "",
                                            DUserId = 0, DRecTimeStamp = null, ChqDDNo = "", ChqDDDate = null, 
                                            CardNo = objModel.objCustomer.CardNo, ActiveStatus = "Y", RecTimeStamp = DateTime.Now,
                                            UserId = objModel.objCustomer.UserDetails.UserId, Version = version,
                                            UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                                        ////insert entry into couponsalesdetails for wallet
                                        IsWalletEntry = true;

                                        string walletsql = @"
INSERT INTO TrnVoucher
(
    VoucherNo,
    VoucherDate,
    DrTo,
    Crto,
    Amount,
    Narration,
    Refno,
    VType,
    BType,
    AccDocType,
    SessID,
    FSessID
)
SELECT 
    ISNULL(MAX(VoucherNo),0)+1,
    CAST(CONVERT(VARCHAR,GETDATE(),106) AS DATETIME),
    '',
    @PartyCode,
    @Amount,
    @Narration,
    @RefNo,
    'Z',
    'O',
    'Party Bill.',
    @SessId,
    @FSessId
FROM TrnVoucher";

                                        var parameters = new
                                        {
                                            PartyCode = objModel.objCustomer.UserDetails.PartyCode,
                                            Amount = objModel.objProduct.PayDetails.AmountByWallet,
                                            Narration = $"Wallet credit against bill {UserBillNo}.",
                                            RefNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo,
                                            SessId = SessId,
                                            FSessId = FsessId
                                        };

                                        int j= await connection.ExecuteAsync(walletsql, parameters);

                                    }

                                    //if (WalletBalance >= objModel.objProduct.PayDetails.AmountByWallet)
                                    //{
                                    //    using (var Mainconnection = _context.CreateConnection())
                                    //    {
                                    //        var walletquery = "INSERT INTO TrnVoucher(VoucherNo,VoucherDate,DrTo,Crto,Amount,Narration,Refno,AcType,VType,SessID,WSEssID) " +
                                    //        "Select CASE WHEN Max(VoucherNo) is NULL THEN 1 ELSE Max(VoucherNo)+1 END ,Cast(Convert(varchar,Getdate(),106) as Datetime),'" + objModel.objCustomer.FormNo + "','0','" + objModel.objProduct.PayDetails.AmountByWallet + "','Product purchased Against " + UserBillNo + ".','" + billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo + "','R','D','" + SessId + "','" + SessId + "' FROM TrnVoucher";
                                    //        var wallaffect = await Mainconnection.ExecuteAsync(walletquery);
                                    //        if (wallaffect > 0)
                                    //        {
                                    //            EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Wallet;
                                    //            string value = EnumPayModes.GetEnumDescription(enumVar);
                                    //            PayPrefix.Add(value);
                                    //            objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = DateTime.Now.Date, BillType = objModel.objCustomer.SelectedInvoiceType, BillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo, PayPrefix = value, Amount = objModel.objProduct.PayDetails.AmountByWallet, BankCode = 0, BankName = "", AcNo = "", IFSCode = "", Narration = "", DUserId = 0, DRecTimeStamp = null, ChqDDNo = "", ChqDDDate = null, CardNo = objModel.objCustomer.CardNo, ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                                    //            ////insert entry into couponsalesdetails for wallet
                                    //            IsWalletEntry = true;
                                    //        }
                                    //        else
                                    //        {
                                    //            objResponse.ResponseStatus = "FAILED";
                                    //            objResponse.ResponseMessage = "Something went wrong";
                                    //            return objResponse;
                                    //        }
                                    //    }

                                    //}
                                }
                                if (objModel.objProduct.PayDetails.IsBD)
                                {
                                    EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.BankDeposit;
                                    string value = EnumPayModes.GetEnumDescription(enumVar);
                                    PayPrefix.Add(value);
                                    objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = DateTime.Now.Date, BillType = objModel.objCustomer.SelectedInvoiceType, BillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo, PayPrefix = value, Amount = objModel.objProduct.PayDetails.AmountByBD, BankCode = 0, ChqDDDate = null, ChqDDNo = "", CardNo = "", Narration = "", DUserId = 0, DRecTimeStamp = null, BankName = objModel.objProduct.PayDetails.BDBankName, AcNo = objModel.objProduct.PayDetails.AccNo, IFSCode = objModel.objProduct.PayDetails.IFSCCode, ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });


                                }
                                if (objModel.objProduct.CashAmount > 0)
                                {
                                    EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Cash;
                                    string value = EnumPayModes.GetEnumDescription(enumVar);
                                    PayPrefix.Add(value);
                                    objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, 
                                        SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = DateTime.Now.Date,
                                        BillType = objModel.objCustomer.SelectedInvoiceType, 
                                        BillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo,
                                        PayPrefix = value, Amount = objModel.objProduct.CashAmount, BankCode = 0, BankName = "", AcNo = "", IFSCode = "", Narration = "", DUserId = 0, DRecTimeStamp = null, ChqDDNo = "", ChqDDDate = null, CardNo = "", ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                                }
                                if (PayPrefix.Count > 0)
                                {
                                    string commaSeparatedString = "'" + string.Join("','", PayPrefix) + "'";
                                    var query = "SELECT PayMode FROM M_PayModeMaster WHERE Prefix IN(" + commaSeparatedString + ")";
                                    //string commaSeparatedString = string.Join(",", PayPrefix);

                                    // string commaSeparatedStringwithsingle= string.Join("''", PayPrefix);
                                    Paymode = (await connection.QueryAsync<string>(query)).ToList();
                                }
                            }
                            else if (objModel.SelectedInvoiceType == "PV")
                            {
                                if (objModel.objProduct.CashAmount > 0)
                                {
                                    EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Cash;
                                    string value = EnumPayModes.GetEnumDescription(enumVar);
                                    PayPrefix.Add(value);
                                    objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable,
                                        SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = DateTime.Now.Date,
                                        BillType = objModel.objCustomer.SelectedInvoiceType,
                                        BillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo, 
                                        PayPrefix = value, Amount = objModel.objProduct.CashAmount, BankCode = 0, BankName = "", 
                                        AcNo = "", IFSCode = "", Narration = "", DUserId = 0, DRecTimeStamp = null, ChqDDNo = "", 
                                        ChqDDDate = null, CardNo = "", ActiveStatus = "Y", RecTimeStamp = DateTime.Now, 
                                        UserId = objModel.objCustomer.UserDetails.UserId, Version = version, 
                                        UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                                }
                                if (PayPrefix.Count > 0)
                                {
                                    var query = "SELECT PayMode FROM M_PayModeMaster WHERE Prefix IN(@Prefixes)";
                                    string commaSeparatedString = string.Join(",", PayPrefix);
                                    Paymode = (await connection.QueryAsync<string>(query, new { Prefixes = commaSeparatedString })).ToList();
                                }
                            }
                        }
                        string SoldByCode = "";
                        List<TrnBillData> tempTableList = new List<TrnBillData>();
                        List<ProductModel> objListProductModel = new List<ProductModel>();
                        try
                        {
                            foreach (var obj in objModel.objListProduct)
                            {
                                objListProductModel.Add(obj);
                                TrnBillData objDTBillData = new TrnBillData();
                                objDTBillData.SBillNo = maxSbillNo;
                                objDTBillData.FSessId = FsessId ?? 0;
                                objDTBillData.SessId = SessId ?? 0;
                                objDTBillData.ActiveStatus = "Y";
                                objDTBillData.BillDate = DateTime.Now.Date;

                                objDTBillData.RefNo = "";
                                objDTBillData.RefId = 0;
                                objDTBillData.RefName = "";
                                objDTBillData.Remarks = string.IsNullOrEmpty(objModel.objCustomer.Remarks) ? "" : objModel.objCustomer.Remarks;
                                objDTBillData.CType = "M";
                                objDTBillData.SoldBy = objModel.objCustomer.UserDetails.PartyCode;
                                SoldByCode = objDTBillData.SoldBy;
                                objDTBillData.BillBy = objDTBillData.SoldBy;
                                objDTBillData.BillNo = billPrefix + "/" + objDTBillData.BillBy + "/" + maxSbillNo;

                                objDTBillData.FType = "M";
                                objDTBillData.FCode = objModel.objCustomer.IdNo;
                                objDTBillData.PartyName = objModel.objCustomer.Name;
                                objDTBillData.SupplierId = 0;
                                objDTBillData.ChDDNo = 0;
                                objDTBillData.ChDate = DateTime.Now;
                                objDTBillData.ChAmt = objModel.objProduct.CourierCharges;
                                objDTBillData.BankCode = 0;
                                objDTBillData.BankName = "";
                                objDTBillData.FormNo = objModel.objCustomer.FormNo;
                                objDTBillData.TotalTaxAmount = objModel.objProduct.TotalTaxAmount;
                                objDTBillData.TotalSTaxAmount = 0;
                                objDTBillData.TotalDiscount = objModel.objProduct.TotalDiscount;
                                objDTBillData.TotalKitBvValue = 0;
                                objDTBillData.TotalBvValue = objModel.objProduct.TotalBV;
                                objDTBillData.TotalCVValue = objModel.objProduct.TotalCV;
                                objDTBillData.TotalPVValue = objModel.objProduct.TotalPV;
                                objDTBillData.TotalRPValue = objModel.objProduct.TotalRP;

                                objDTBillData.DP = obj.DP ?? 0;
                                objDTBillData.RP = obj.RP ?? 0;
                                objDTBillData.MRP = obj.MRP ?? 0;
                                objDTBillData.CVValue = obj.CVValue ?? 0;
                                objDTBillData.CV = obj.CV ?? 0;
                                objDTBillData.PV = obj.PV ?? 0;
                                objDTBillData.BV = obj.BV ?? 0;
                                objDTBillData.BVValue = obj.BVValue ?? 0;
                                objDTBillData.PVValue = obj.PVValue ?? 0;
                                objDTBillData.RPValue = obj.RPValue ?? 0;
                                objDTBillData.Barcode = obj.Barcode.ToString();
                                objDTBillData.BatchNo = obj.BatchNo.ToString();
                                if (objModel.objCustomer.StateCode != objModel.objCustomer.UserDetails.StateCode)
                                {
                                    objDTBillData.TaxAmount = obj.TaxAmt ?? 0;
                                    if (obj.OldTaxAmount != 0 && obj.OldTaxAmount != obj.TaxAmt)
                                    {
                                        objDTBillData.TaxAmount = Decimal.Parse((Convert.ToDouble(objDTBillData.TaxAmount) + 0.01).ToString());
                                        objDTBillData.NetAmount = Decimal.Parse((Convert.ToDouble(objDTBillData.NetAmount) - 0.01).ToString());
                                    }
                                    objDTBillData.Tax = obj.TaxPer ?? 0;
                                    objDTBillData.CGST = 0;
                                    objDTBillData.CGSTAmt = 0;
                                    objDTBillData.SGST = 0;
                                    objDTBillData.SGSTAmt = 0;
                                    objDTBillData.TaxType = "I";
                                }
                                else
                                {
                                    objDTBillData.TaxAmount = 0;
                                    objDTBillData.Tax = 0;
                                    objDTBillData.CGST = obj.TaxPer / 2 ?? 0;
                                    objDTBillData.CGSTAmt = obj.TaxAmt / 2 ?? 0;
                                    objDTBillData.SGST = obj.TaxPer / 2 ?? 0;
                                    objDTBillData.SGSTAmt = obj.TaxAmt / 2 ?? 0;
                                    objDTBillData.TaxType = "S";
                                }
                                objDTBillData.DiscountPer = obj.DiscPer ?? 0;
                                objDTBillData.Discount = obj.DiscAmt ?? 0;
                                objDTBillData.ProdCommssn = obj.CommissionPer ?? 0;
                                objDTBillData.ProdCommssnAmt = obj.CommissionAmt ?? 0;
                                objDTBillData.ProductId = obj.ProdCode.ToString();
                                objDTBillData.ProductName = obj.ProductName;
                                objDTBillData.Qty = obj.Quantity;
                                objDTBillData.FreeQty = obj.FreeQty;
                                if (!string.IsNullOrEmpty(obj.ProductTye) && obj.ProductTye.ToUpper() == "F")
                                {
                                    objDTBillData.TFreeQty = 0;
                                }
                                else
                                {
                                    objDTBillData.TFreeQty = obj.TFreeQty;
                                }
                                objDTBillData.Rate = obj.Rate ?? 0;
                                objDTBillData.IsKitBV = "N";
                                objDTBillData.DSeries = "";
                                objDTBillData.DImported = "N";
                                objDTBillData.IMEINo = "";
                                objDTBillData.BNo = "";
                                objDTBillData.ItemType = "";
                                objDTBillData.JType = "Cash:" + objModel.objProduct.TotalNetPayable;
                                objDTBillData.BillTo = "R";
                                objDTBillData.BillFor = "RB";
                                objDTBillData.IsReceive = "R";
                                objDTBillData.IsCredit = "F";
                                //objDTBillData.BillType = "R";
                                //if (objModel.objCustomer.IsFirstBill)
                                //{
                                //    objDTBillData.BillType = (objModel.objProduct.VoucherNo ?? "") != "" ? objModel.BillType : "B";
                                //}
                                //else
                                //{
                                //    if (string.IsNullOrEmpty(objModel.BillType))
                                //        objDTBillData.BillType = (objModel.objProduct.VoucherNo ?? "") != "" ? objModel.BillType : "R";
                                //    else
                                //        objDTBillData.BillType = objModel.BillType;
                                //}
                                if (objModel.SelectedInvoiceType == "BV")
                                {
                                    objDTBillData.BillType = "B";
                                }
                                else if (objModel.SelectedInvoiceType == "PV")
                                {
                                    objDTBillData.BillType = "P";
                                }
                                if (!string.IsNullOrEmpty(obj.ProductTye))
                                {
                                    objDTBillData.ProdType = obj.ProductTye;
                                }
                                else
                                {
                                    objDTBillData.ProdType = "P";
                                }
                                objDTBillData.PaymentDtl = "Cash:" + objModel.objProduct.TotalNetPayable;

                                objDTBillData.TotalAmount = objModel.objProduct.TotalTotalAmount;
                                //tax excluding
                                objDTBillData.NetAmount = obj.Amount;
                                objDTBillData.CashDiscPer = obj.CashDiscPer;
                                objDTBillData.CashDiscAmount = obj.CashDiscAmount;

                                objDTBillData.NetPayable = Math.Round(objModel.objProduct.TotalNetPayable);
                                if (objModel.objProduct.Roundoff == 0)
                                {
                                    objDTBillData.RndOff = objDTBillData.NetPayable - objModel.objProduct.TotalNetPayable;
                                }
                                else
                                {
                                    objDTBillData.RndOff = objModel.objProduct.Roundoff;
                                }
                                objDTBillData.CardAmount = 0;
                                objDTBillData.PayMode = Paymode.Count > 1 ? string.Join(",", Paymode) : Paymode[0];
                                objDTBillData.PayPrefix = PayPrefix.Count > 1 ? string.Join(",", PayPrefix) : PayPrefix[0];
                                objDTBillData.BvTransfer = "N";

                                //objDTBillData.UserSBillNo = maxSbillNo;
                                //objDTBillData.UserBillNo = billPrefix + "/" + objDTBillData.BillBy + "/" + maxSbillNo;
                                objDTBillData.UserSBillNo = maxUserSBillNo;
                                objDTBillData.UserBillNo = UserBillNo;
                                objDTBillData.DispatchStatus = "N";
                                objDTBillData.LR = "0";
                                objDTBillData.LRDate = DateTime.Now;
                                objDTBillData.TransporterName = "";
                                objDTBillData.DispatchTo = objModel.objCustomer.IdNo;
                                objDTBillData.FreightType = "";
                                objDTBillData.Series = "";
                                objDTBillData.Scratch = "";
                                objDTBillData.Unit = 0;
                                // objDTBillData.LocId = objModel.objCustomer.KitId;
                                //}
                                objDTBillData.PSessId = 0;
                                objDTBillData.DcNo = objModel.objProduct.VoucherNo ?? "";
                                objDTBillData.Imported = "N";
                                objDTBillData.FPoint = 0;
                                objDTBillData.FPointValue = 0;
                                objDTBillData.OrdStatus = "";
                                objDTBillData.OrdQty = 0;
                                // objDTBillData.OrderType = "";
                                objDTBillData.OrderDate = DateTime.Now;
                                objDTBillData.OrderNo = "";
                                objDTBillData.RemQty = 0;
                                objDTBillData.DP1 = 0;
                                objDTBillData.DReason = "";
                                objDTBillData.DUserId = 0;
                                objDTBillData.DRecTimeStamp = DateTime.Now;
                                objDTBillData.DocWeight = objModel.objProduct.TotalWeight;
                                objDTBillData.DocketNo = "";
                                objDTBillData.DocketDate = DateTime.Now;
                                objDTBillData.STNFormNo = "";
                                objDTBillData.StkRecv = "N";
                                objDTBillData.StkRecvDate = DateTime.Now;
                                objDTBillData.StkRecvUserId = 0;
                                objDTBillData.InTransit = "N";
                                objDTBillData.UID = string.IsNullOrEmpty(objModel.objProduct.UID) ? "" : objModel.objProduct.UID;
                                objDTBillData.OfferUID = objModel.offerId;
                                objDTBillData.IsKit = "N";
                                objDTBillData.TotalCorton = "";
                                objDTBillData.TotalMonoCorton = "";
                                objDTBillData.SpclOfferId = 0;
                                objDTBillData.VAT = 0;
                                objDTBillData.BuyerAddress = "";
                                objDTBillData.BuyerTIN = "";

                                objDTBillData.TotalDiscount = objModel.objProduct.TotalDiscPer;
                                objDTBillData.TotalDiscountAmt = objModel.objProduct.TotalDiscount;
                                objDTBillData.VDiscountAmt = objModel.objProduct.VDiscountAmt ?? 0;
                                objDTBillData.VDiscount = 0;
                                objDTBillData.ReceiverID = "";
                                objDTBillData.ReceiverName = "";
                                objDTBillData.ReceiverMNo = "";
                                objDTBillData.ReceiverIDProof = "";
                                objDTBillData.TotalFPoint = 0;
                                objDTBillData.TotalQty = objModel.objProduct.TotalQty;
                                objDTBillData.CashReward = objModel.objProduct.VoucherAmt ?? 0;
                                objDTBillData.CommssnAmt = objModel.objProduct.TotalCommsonAmt;
                                objDTBillData.RecvAmount = 0;
                                objDTBillData.ReturnToCustAmt = 0;
                                objDTBillData.ActiveStatus = "Y";
                                objDTBillData.RecTimeStamp = DateTime.Now;
                                objDTBillData.UserId = objModel.objCustomer.UserDetails.UserId;
                                objDTBillData.UserName = objModel.objCustomer.UserDetails.UserName;
                                objDTBillData.DelvPlace = string.IsNullOrEmpty(objModel.objProduct.DeliveryPlace) ? "" : objModel.objProduct.DeliveryPlace;
                                objDTBillData.DelvStatus = "";
                                objDTBillData.DelvUserId = 0;
                                objDTBillData.DelvRecTimeStamp = DateTime.Now;
                                objDTBillData.Version = version;
                                objDTBillData.IDType = "";
                                objDTBillData.BranchName = "";
                                objDTBillData.CourierId = 0;
                                objDTBillData.CourierName = "";
                                objDTBillData.LocId = 0;
                                objDTBillData.LocName = "";
                                objDTBillData.DelvAddress = "";
                                objDTBillData.Pincode = "";
                                objDTBillData.OrderType = "";
                                billno_ = objDTBillData.BillNo;
                                if (objModel.UserType == "shoppe")
                                    soldby_ = objDTBillData.SoldBy;
                                else
                                    soldby_ = objDTBillData.UserName;
                                fcode_ = objDTBillData.FCode;
                                netpayable_ = objDTBillData.NetPayable;
                                narration_ = "Wallet deducted against " + objDTBillData.UserBillNo + ".";
                                var TrnPartyOrder = @"insert into TrnBillData(
                                   FSessId,SessId,SBillNo,BillNo,RefNo,BillDate,CType,SoldBy,BillBy,FType,FCode,PartyName,SupplierId,
                                   ChDDNo,ChDate,ChAmt,BankCode,BankName,FormNo,TotalTaxAmount,TotalSTaxAmount,TotalDiscount,
                                   TotalKitBvValue,TotalBvValue,TotalCVValue,TotalPVValue,TotalRPValue,CashDiscPer,
                                   CashDiscAmount,NetPayable,TotalAmount,RndOff,CardAmount,PayMode,PayPrefix,
                                   BvTransfer,Remarks,DispatchStatus,LR,LRDate,TransporterName,DispatchTo,
                                   FreightType,FreightAmt,Series,Scratch,RefId,RefName,JType,Unit,BillTo,
                                   PSessId,BillFor,DcNo,Imported,IsReceive,IsCredit,BillType,
                                   TotalDiscountAmt,VDiscountAmt,ReceiverID,ReceiverName,
                                   ReceiverMNo,ReceiverIDProof,TotalFPoint,TotalQty,
                                   CommssnAmt,CashReward,RecvAmount,ReturnToCustAmt,
                                   ActiveStatus,RecTimeStamp,UserId,UserName,Version,DelvPlace,PaymentDtl,IDType,
                                   BranchName,LocId,LocName,Pincode,CourierId,CourierName,ProductId,ProductName,
                                   BatchNo,Barcode,Qty,MRP,DP,Rate,BV,BVValue,CV,CVValue,PV,PVValue,RP,RPValue,
                                   IsKitBV,TaxType,Tax,TaxAmount,DiscountPer,Discount,NetAmount,DSeries,DImported,
                                   IMEINo,BNo,ItemType,VDiscount,VDiscountValue,FPoint,FPointValue,ProdCommssn,
                                   ProdCommssnAmt,OrdStatus,OrdQty,RemQty,DP1,DReason,DUserId,DRecTimeStamp,
                                   DocWeight,DocketNo,DOD,DelvAddress,OrderNo,OrderDate,DocketDate,DelvStatus,
                                   DelvUserId,DelvRecTimeStamp,OrderType,UserBillNo,UserSBillNo,STNFormNo,
                                   StkRecv,StkRecvDate,StkRecvUserId,InTransit,UID,OfferUID,IsKit,ProdType,
                                   TotalCorton,TotalMonoCorton,SpclOfferId,VAT,BuyerAddress,BuyerTIN,CGST,
                                   CGSTAmt,SGST,SGSTAmt,FreeQty,TFreeQty,BillGSTType)
                                   values(@FSessId,@SessId,@SBillNo,@BillNo,@RefNo,@BillDate,@CType,@SoldBy,@BillBy,@FType,@FCode,@PartyName,@SupplierId,
                                   @ChDDNo,@ChDate,@ChAmt,@BankCode,@BankName,@FormNo,@TotalTaxAmount,@TotalSTaxAmount,@TotalDiscount,
                                   @TotalKitBvValue,@TotalBvValue,@TotalCVValue,@TotalPVValue,@TotalRPValue,@CashDiscPer,
                                   @CashDiscAmount,@NetPayable,@TotalAmount,@RndOff,@CardAmount,@PayMode,@PayPrefix,
                                   @BvTransfer,@Remarks,@DispatchStatus,@LR,@LRDate,@TransporterName,@DispatchTo,
                                   @FreightType,@FreightAmt,@Series,@Scratch,@RefId,@RefName,@JType,@Unit,@BillTo,
                                   @PSessId,@BillFor,@DcNo,@Imported,@IsReceive,@IsCredit,@BillType,
                                   @TotalDiscountAmt,@VDiscountAmt,@ReceiverID,@ReceiverName,
                                   @ReceiverMNo,@ReceiverIDProof,@TotalFPoint,@TotalQty,
                                   @CommssnAmt,@CashReward,@RecvAmount,@ReturnToCustAmt,
                                   @ActiveStatus,@RecTimeStamp,@UserId,@UserName,@Version,@DelvPlace,@PaymentDtl,@IDType,
                                   @BranchName,@LocId,@LocName,@Pincode,@CourierId,@CourierName,@ProductId,@ProductName,
                                   @BatchNo,@Barcode,@Qty,@MRP,@DP,@Rate,@BV,@BVValue,@CV,@CVValue,@PV,@PVValue,@RP,@RPValue,
                                   @IsKitBV,@TaxType,@Tax,@TaxAmount,@DiscountPer,@Discount,@NetAmount,@DSeries,@DImported,
                                   @IMEINo,@BNo,@ItemType,@VDiscount,@VDiscountValue,@FPoint,@FPointValue,@ProdCommssn,
                                   @ProdCommssnAmt,@OrdStatus,@OrdQty,@RemQty,@DP1,@DReason,@DUserId,@DRecTimeStamp,
                                   @DocWeight,@DocketNo,@DOD,@DelvAddress,@OrderNo,@OrderDate,@DocketDate,@DelvStatus,
                                   @DelvUserId,@DelvRecTimeStamp,@OrderType,@UserBillNo,@UserSBillNo,@STNFormNo,
                                   @StkRecv,@StkRecvDate,@StkRecvUserId,@InTransit,@UID,@OfferUID,@IsKit,@ProdType,
                                   @TotalCorton,@TotalMonoCorton,@SpclOfferId,@VAT,@BuyerAddress,@BuyerTIN,@CGST,
                                   @CGSTAmt,@SGST,@SGSTAmt,@FreeQty,@TFreeQty,'G')";
                                TrnBillDatasAffected = await connection.ExecuteAsync(TrnPartyOrder, objDTBillData);
                            }
                            if (TrnBillDatasAffected > 0)
                            {

                                if (objModel.SelectedInvoiceType == "BV")
                                {
                                    if (objModel.objProduct.PayDetails.IsV)
                                    {
                                        string voucherno = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                                        //string fpvwalletsql = "insert into FPVucherWallet(Idno,Remark,credit,debit,Vtype,VoucherNo,BillNo)values('" + objModel.objCustomer.IdNo + "','FPV debit againest bill " + billno_ + "',0," + objModel.objProduct.PayDetails.AmountByVoucher + ",'F','" + voucherno + "','" + billno_ + "')";

                                        //var fpvwallet = await connection.QueryFirstOrDefaultAsync<FPVucherWallet>(fpvwalletsql, commandType: CommandType.Text);
                                        string fpvwalletsql = @"
                                                             INSERT INTO FPVucherWallet (Idno, Remark, credit, debit, Vtype, VoucherNo, BillNo)
                                                             OUTPUT INSERTED.*
                                                             VALUES (@IdNo, @Remark, 0, @AmountByVoucher, 'F', @VoucherNo, @BillNo)";
                                        var fpvwallet = await connection.QueryFirstOrDefaultAsync<FPVucherWallet>(fpvwalletsql, new
                                        {
                                            IdNo = objModel.objCustomer.IdNo,
                                            Remark = "FPV debit against bill " + billno_,
                                            AmountByVoucher = objModel.objProduct.PayDetails.AmountByVoucher,
                                            VoucherNo = voucherno,
                                            BillNo = billno_
                                        });

                                        string fpvusesql = "insert into FPVoucherUsed(IDno,BillNo,AdjustAmount,FPVucherWallet_id)values('" + objModel.objCustomer.IdNo + "','" + billno_ + "'," + objModel.objProduct.PayDetails.AmountByVoucher + "," + fpvwallet.id + ")";
                                        await connection.ExecuteAsync(fpvusesql);
                                        //var fpsql = @"update FPVoucher set Isuse=1,BillNo='" + billno_ + "' where Code='" + objModel.objProduct.PayDetails.FpVoucher + "' and IdNo='" + objModel.objCustomer.IdNo + "'";
                                        //var fpaff = await connection.ExecuteAsync(fpsql); 
                                        narration_ = UserBillNo + " against F.P. Voucher adjust " + (objModel.objProduct.PayDetails.AmountByVoucher);
                                        int fp = await CreditPartyWallet(billno_, narration_, "", soldby_, (objModel.objProduct.PayDetails.AmountByVoucher), "X"); 
                                    }
                                    if (objModel.objProduct.PayDetails.IsCU)
                                    {
                                        var csql = @"update Coupon set Isuse=1,BillNo='" + billno_ + "' where Code='" + objModel.objProduct.PayDetails.Coupon + "' and IdNo='" + objModel.objCustomer.IdNo + "'";
                                        var caff = await connection.ExecuteAsync(csql);
                                        narration_ = UserBillNo + " against Coupon adjust " + objModel.objProduct.PayDetails.AmountbyCoupon;
                                        int cp = await CreditPartyWallet(billno_, narration_, "", soldby_, objModel.objProduct.PayDetails.AmountbyCoupon, "R");
                                    }
                                    int i = await DeductPartyWallet(billno_, narration_, soldby_, fcode_, netpayable_, objModel.UserType, objModel.SelectedInvoiceType);
                                }
                                else if (objModel.SelectedInvoiceType == "PV")
                                {
                                    int i = await DeductPartyWallet(billno_, narration_, soldby_, fcode_, netpayable_, objModel.UserType, objModel.SelectedInvoiceType);
                                }

                                try
                                {
                                    string paymodequery = "SELECT * FROM M_PayModeMaster";

                                    var resultPayMode = connection.Query<M_PayModeMaster>(paymodequery).ToList();
                                    foreach (var obj in objDTListPayMode)
                                    {
                                        TrnPayModeDetail objTemp = new TrnPayModeDetail();
                                        objTemp = obj;
                                        if (objModel.objCustomer.IsFirstBill)
                                        {
                                            objTemp.BillType = "B";
                                        }
                                        else
                                        {
                                            objTemp.BillType = "R";
                                        }
                                        objTemp.PayMode = (from r in resultPayMode where r.Prefix.Trim() == obj.PayPrefix.Trim() select r.PayMode).FirstOrDefault();
                                        if (string.IsNullOrEmpty(objTemp.CardNo))
                                        {
                                            objTemp.CardNo = "";
                                        }
                                    }

                                    string insertSql = @"
                                                       INSERT INTO TrnPayModeDetail
                                                       (
                                                          FSessId,SBillNo,BillNo,SoldBy,BillDate,PayPrefix,PayMode,ChqDDNo, 
                                                          ChqDDDate,CardNo,BankCode,BankName,Amount,BillAmt,Narration, 
                                                          ActiveStatus,RecTimeStamp,Version,UserId,UserName,BillType, 
                                                          DUserId,DRecTimeStamp,AcNo,IFSCode 
                                                        )
                                                        VALUES
                                                        (
                                                         @FSessId,@SBillNo,@BillNo,@SoldBy,@BillDate,@PayPrefix,@PayMode, 
                                                         @ChqDDNo, @ChqDDDate,@CardNo,@BankCode,@BankName,@Amount,@BillAmt, 
                                                         @Narration,@ActiveStatus,@RecTimeStamp, @Version, 
                                                         @UserId, @UserName, @BillType, @DUserId, @DRecTimeStamp, @AcNo,@IFSCode 
                                                        )";

                                    int rowsAffected = connection.Execute(insertSql, objDTListPayMode);
                                }
                                catch(Exception ex)
                                {

                                }

                                //hit api 
                                string result = "";
                                string detail = "", apierror = "";
                                try
                                {
                                    string Bvvalue = "0";
                                    string Pvvalue = "0";
                                    string fpamt = "0";
                                    string voucheramt = "0";
                                    if (objModel.SelectedInvoiceType == "BV")
                                    {
                                        decimal totalBV = Convert.ToDecimal(objModel.objProduct.TotalBV);
                                        decimal voucherAmount = Convert.ToDecimal(objModel.objProduct.PayDetails.AmountByVoucher);

                                        if (voucherAmount > 0)
                                        {
                                            decimal calculatedBV = totalBV - (voucherAmount / 2);

                                            // Prevent negative value
                                            Bvvalue = Convert.ToString(calculatedBV < 0 ? 0 : calculatedBV);
                                        }
                                        else
                                        {
                                            Bvvalue = Convert.ToString(totalBV);
                                        }
                                    }
                                    else if (objModel.SelectedInvoiceType == "PV")
                                    {
                                        Pvvalue = Convert.ToString(objModel.objProduct.TotalPV);
                                    }

                                    if (objModel.objProduct.PayDetails.AmountByVoucher > 0)
                                    {
                                        fpamt = Convert.ToString(objModel.objProduct.PayDetails.AmountByVoucher);
                                    }
                                    if (objModel.objProduct.PayDetails.AmountByWallet > 0)
                                    {
                                        voucheramt = Convert.ToString(objModel.objProduct.PayDetails.AmountByWallet);
                                    }

                                    var reqobj = new
                                    {
                                        act = "INSERTPVBV",
                                        uid = Convert.ToString(objModel.objCustomer.IdNo),
                                        logkey = "ZoewellnessgugddkhJJHJsddd",
                                        billno = UserBillNo,
                                        billdate = DateTime.Now.Date.ToString("dd-MMM-yyyy").ToUpper(),
                                        bv = Bvvalue,
                                        partycode = "WR",
                                        PV = Pvvalue,
                                        billamount = Convert.ToString(objModel.objProduct.TotalNetPayable),
                                        fpamt = fpamt,
                                        voucheramt = voucheramt
                                    };

                                    detail = JsonSerializer.Serialize(reqobj);
                                    // Create a request
                                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://zoewellness.co.in/api/api.ashx");
                                    request.Method = "POST";
                                    request.ContentType = "application/json"; // Set content type to JSON
                                                                              // If the API requires headers (e.g., Authorization), add them here
                                                                              // request.Headers.Add("Authorization", "Bearer YOUR_TOKEN");
                                                                              // Write JSON data to request stream
                                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                                    {
                                        streamWriter.Write(detail);
                                        streamWriter.Flush();
                                    }
                                    // Get the response
                                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                                    {
                                        using (var streamReader = new StreamReader(response.GetResponseStream()))
                                        {
                                            result = streamReader.ReadToEnd();
                                            //return result; // Optionally deserialize this JSON string to an object
                                        }
                                    }

                                    var parameters = new
                                    {
                                        UserBillNo = UserBillNo,
                                        ApiURL = "https://zoewellness.co.in/api/api.ashx",
                                        Request = detail,
                                        Response = result,
                                        Error = apierror
                                    };

                                    int i = connection.Execute(
                                        "Sp_InsertApiRequestResponse",
                                        parameters,
                                        commandType: CommandType.StoredProcedure
                                    );
                                }
                                catch (Exception ex)
                                {
                                    //var message = ex.Message;
                                    apierror = ex.Message;
                                    var parameters = new
                                    {
                                        UserBillNo = UserBillNo,
                                        ApiURL = "https://zoewellness.co.in/api/api.ashx",
                                        Request = detail,
                                        Response = result,
                                        Error = apierror
                                    };

                                    int i = connection.Execute(
                                        "Sp_InsertApiRequestResponse",
                                        parameters,
                                        commandType: CommandType.StoredProcedure
                                    );
                                }

                                objResponse.ResponseMessage = "Saved Successfully.";
                                objResponse.ResponseStatus = "OK";
                            }
                        }
                        catch (Exception ex)
                        {
                            objResponse.ResponseMessage = "Something went wrong!";
                            objResponse.ResponseStatus = "FAILED";
                        }
                    }
                    else if (objModel.BillType == "party")
                    {
                        DateTime BillDate = DateTime.Now;
                        if (!string.IsNullOrEmpty(objModel.BillDateStr))
                        {
                            var SplitDate = objModel.BillDateStr.Split('-');
                            string NewDate = SplitDate[1] + "/" + SplitDate[0] + "/" + SplitDate[2];
                            var NewDate1 = Convert.ToDateTime(DateTime.ParseExact(NewDate, "MM/dd/yyyy", CultureInfo.InvariantCulture));
                            BillDate = Convert.ToDateTime(NewDate1);
                            BillDate = BillDate.Date;
                            IsWalletEntry = false;
                            if (objModel != null)
                            {
                                if (objModel.objProduct.PayDetails != null)
                                {
                                    if (objModel.objProduct.PayDetails.IsBD)
                                    {
                                        EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.BankDeposit;
                                        string value = EnumPayModes.GetEnumDescription(enumVar);
                                        PayPrefix.Add(value);
                                        objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = BillDate.Date, BillType = "V", BillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo, PayPrefix = value, Amount = objModel.objProduct.PayDetails.AmountByBD, BankCode = 0, ChqDDDate = null, ChqDDNo = "", CardNo = "", Narration = "", DUserId = 0, DRecTimeStamp = null, BankName = objModel.objProduct.PayDetails.BDBankName, AcNo = objModel.objProduct.PayDetails.AccNo, IFSCode = objModel.objProduct.PayDetails.IFSCCode, ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });

                                    }
                                    if (objModel.objProduct.PayDetails.IsCC)
                                    {
                                        EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Card;
                                        string value = EnumPayModes.GetEnumDescription(enumVar);
                                        PayPrefix.Add(value);
                                        objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = BillDate.Date, BillType = "V", BillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo, PayPrefix = value, AcNo = "", IFSCode = "", BankCode = 0, Narration = "", BankName = "", DUserId = 0, DRecTimeStamp = null, ChqDDNo = "", ChqDDDate = null, Amount = objModel.objProduct.PayDetails.AmountByCard, CardNo = objModel.objProduct.PayDetails.CardNo, ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                                    }
                                    if (objModel.objProduct.PayDetails.IsQ)
                                    {
                                        EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Cheque;
                                        string value = EnumPayModes.GetEnumDescription(enumVar);
                                        PayPrefix.Add(value);
                                        objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = BillDate.Date, BillType = "V", BillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo, PayPrefix = value, Amount = objModel.objProduct.PayDetails.AmountByCheque, CardNo = "", AcNo = "", IFSCode = "", BankCode = 0, Narration = "", DUserId = 0, DRecTimeStamp = null, BankName = objModel.objProduct.PayDetails.CHBankName, ChqDDNo = objModel.objProduct.PayDetails.ChequeNo, ChqDDDate = objModel.objProduct.PayDetails.ChequeDate, ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                                    }
                                    if (objModel.objProduct.PayDetails.IsD)
                                    {
                                        EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.DD;
                                        string value = EnumPayModes.GetEnumDescription(enumVar);
                                        PayPrefix.Add(value);
                                        objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = BillDate.Date, BillType = "V", BillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo, PayPrefix = value, Amount = objModel.objProduct.PayDetails.AmountByDD, CardNo = "", AcNo = "", IFSCode = "", BankCode = 0, Narration = "", DUserId = 0, DRecTimeStamp = null, BankName = objModel.objProduct.PayDetails.DDBankName, ChqDDNo = objModel.objProduct.PayDetails.DDNo, ChqDDDate = objModel.objProduct.PayDetails.DDDate, ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                                    }
                                    if (objModel.objProduct.PayDetails.IsT)
                                    {
                                        EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Credit;
                                        string value = EnumPayModes.GetEnumDescription(enumVar);
                                        PayPrefix.Add(value);
                                        objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = BillDate.Date, BillType = "V", BillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo, PayPrefix = value, BankName = "", Amount = objModel.objProduct.PayDetails.AmountByCredit, CardNo = "", AcNo = "", IFSCode = "", BankCode = 0, DUserId = 0, DRecTimeStamp = null, ChqDDDate = null, ChqDDNo = "", Narration = objModel.objProduct.PayDetails.Narration, ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                                    }

                                    if (objModel.objProduct.PayDetails.IsV)
                                    {
                                        EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Voucher;
                                        string value = EnumPayModes.GetEnumDescription(enumVar);
                                        PayPrefix.Add(value);


                                        objDTListPayMode.Add(new TrnPayModeDetail
                                        {
                                            BillAmt = objModel.objProduct.TotalNetPayable,
                                            SoldBy = objModel.objCustomer.UserDetails.PartyCode,
                                            BillDate = BillDate.Date,
                                            BillType = "V",
                                            BillNo = $"{billPrefix}/{objModel.objCustomer.UserDetails.PartyCode}/{maxSbillNo}",
                                            PayPrefix = value,
                                            Amount = objModel.objProduct.PayDetails.AmountByVoucher,
                                            CardNo = "",
                                            AcNo = "",
                                            IFSCode = "",
                                            BankCode = 0,
                                            DUserId = 0,
                                            DRecTimeStamp = null,
                                            ChqDDDate = null,
                                            ChqDDNo = "",
                                            Narration = "",
                                            BankName = "",
                                            ActiveStatus = "Y",
                                            RecTimeStamp = DateTime.Now,
                                            UserId = objModel.objCustomer.UserDetails.UserId,
                                            Version = version,
                                            UserName = objModel.objCustomer.UserDetails.UserName,
                                            FSessId = FsessId ?? 0,
                                            SBillNo = maxSbillNo
                                        });
                                        string query = ";INSERT INTO TrnVoucher(VoucherNo,VoucherDate,DrTo,Crto,Amount,Narration,Refno,VType,BType,AccDocType,SessID,FSessID) " +
                                                 " Select ISNULL(Max(VoucherNo),0)+1, Cast(Convert(varchar,Getdate(),106) as Datetime),'" + objModel.objCustomer.PartyCode + "','" + objModel.objCustomer.UserDetails.PartyCode + "','" + objModel.objProduct.PayDetails.AmountByVoucher + "','Wallet deducted against bill " + UserBillNo + ".','" + billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo + "','X','O','Party Bill.','" + SessId + "','" + FsessId + "' FROM TrnVoucher";


                                        int result = connection.Execute(query);
                                    }

                                    if (objModel.objProduct.PayDetails.IsBPW)
                                    {
                                        EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.BVPurchaseWallet;
                                        string value = EnumPayModes.GetEnumDescription(enumVar);
                                        PayPrefix.Add(value);


                                        objDTListPayMode.Add(new TrnPayModeDetail
                                        {
                                            BillAmt = objModel.objProduct.TotalNetPayable,
                                            SoldBy = objModel.objCustomer.UserDetails.PartyCode,
                                            BillDate = BillDate.Date,
                                            BillType = "V",
                                            BillNo = $"{billPrefix}/{objModel.objCustomer.UserDetails.PartyCode}/{maxSbillNo}",
                                            PayPrefix = value,
                                            Amount = objModel.objProduct.PayDetails.AmountByBPW,
                                            CardNo = "",
                                            AcNo = "",
                                            IFSCode = "",
                                            BankCode = 0,
                                            DUserId = 0,
                                            DRecTimeStamp = null,
                                            ChqDDDate = null,
                                            ChqDDNo = "",
                                            Narration = "",
                                            BankName = "",
                                            ActiveStatus = "Y",
                                            RecTimeStamp = DateTime.Now,
                                            UserId = objModel.objCustomer.UserDetails.UserId,
                                            Version = version,
                                            UserName = objModel.objCustomer.UserDetails.UserName,
                                            FSessId = FsessId ?? 0,
                                            SBillNo = maxSbillNo
                                        });
                                        string query = ";INSERT INTO TrnVoucher(VoucherNo,VoucherDate,DrTo,Crto,Amount,Narration,Refno,VType,BType,AccDocType,SessID,FSessID) " +
                                                 " Select ISNULL(Max(VoucherNo),0)+1, Cast(Convert(varchar,Getdate(),106) as Datetime),'" + objModel.objCustomer.PartyCode + "','" + objModel.objCustomer.UserDetails.PartyCode + "','" + objModel.objProduct.PayDetails.AmountByBPW + "','Wallet deducted against bill " + UserBillNo + ".','" + billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo + "','Z','O','Party Bill.','" + SessId + "','" + FsessId + "' FROM TrnVoucher";


                                        int result = connection.Execute(query);
                                    }

                                    if (objModel.objProduct.PayDetails.IsPPW)
                                    {
                                        EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.BVPurchaseWallet;
                                        string value = EnumPayModes.GetEnumDescription(enumVar);
                                        PayPrefix.Add(value);


                                        objDTListPayMode.Add(new TrnPayModeDetail
                                        {
                                            BillAmt = objModel.objProduct.TotalNetPayable,
                                            SoldBy = objModel.objCustomer.UserDetails.PartyCode,
                                            BillDate = BillDate.Date,
                                            BillType = "V",
                                            BillNo = $"{billPrefix}/{objModel.objCustomer.UserDetails.PartyCode}/{maxSbillNo}",
                                            PayPrefix = value,
                                            Amount = objModel.objProduct.PayDetails.AmountByPPW,
                                            CardNo = "",
                                            AcNo = "",
                                            IFSCode = "",
                                            BankCode = 0,
                                            DUserId = 0,
                                            DRecTimeStamp = null,
                                            ChqDDDate = null,
                                            ChqDDNo = "",
                                            Narration = "",
                                            BankName = "",
                                            ActiveStatus = "Y",
                                            RecTimeStamp = DateTime.Now,
                                            UserId = objModel.objCustomer.UserDetails.UserId,
                                            Version = version,
                                            UserName = objModel.objCustomer.UserDetails.UserName,
                                            FSessId = FsessId ?? 0,
                                            SBillNo = maxSbillNo
                                        });
                                        string query = ";INSERT INTO TrnVoucher(VoucherNo,VoucherDate,DrTo,Crto,Amount,Narration,Refno,VType,BType,AccDocType,SessID,FSessID) " +
                                                 " Select ISNULL(Max(VoucherNo),0)+1, Cast(Convert(varchar,Getdate(),106) as Datetime),'" + objModel.objCustomer.PartyCode + "','" + objModel.objCustomer.UserDetails.PartyCode + "','" + objModel.objProduct.PayDetails.AmountByPPW + "','Wallet deducted against bill " + UserBillNo + ".','" + billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo + "','W','O','Party Bill.','" + SessId + "','" + FsessId + "' FROM TrnVoucher";


                                        int result = connection.Execute(query);
                                    }

                                    if (objModel.objProduct.PayDetails.IsW)
                                    {
                                        string balanceQuery = "SELECT Balance FROM V#PartyBalance WHERE PartyCode = @PartyCode";
                                        WalletBalance = connection.QueryFirstOrDefault<decimal?>(balanceQuery,
                                                                new { PartyCode = objModel.objCustomer.PartyCode }) ?? 0;


                                        if (WalletBalance >= objModel.objProduct.PayDetails.AmountByWallet &&
                                             objModel.objProduct.PayDetails.AmountByWallet > 0)
                                        {
                                            decimal cashAmt = objModel.objProduct.TotalNetPayable -
                                                                          objModel.objProduct.PayDetails.AmountByWallet;

                                            var walletquery = ";INSERT INTO TrnVoucher(VoucherNo,VoucherDate,DrTo,Crto,Amount,Narration,Refno,VType,BType,AccDocType,SessID,FSessID) " +
                                       " Select ISNULL(Max(VoucherNo),0)+1, Cast(Convert(varchar,Getdate(),106) as Datetime),'" + objModel.objCustomer.PartyCode + "','" + objModel.objCustomer.UserDetails.PartyCode + "','" + objModel.objProduct.PayDetails.AmountByWallet + "','Wallet deducted against bill " + UserBillNo + ".','" + billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo + "','R','O','Party Bill.','" + SessId + "','" + FsessId + "' FROM TrnVoucher";


                                            var wallaffect = await connection.ExecuteAsync(walletquery);

                                            if (wallaffect > 0)
                                            {
                                                EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Wallet;
                                                string value = EnumPayModes.GetEnumDescription(enumVar);
                                                PayPrefix.Add(value);

                                                objDTListPayMode.Add(new TrnPayModeDetail
                                                {
                                                    BillAmt = objModel.objProduct.TotalNetPayable,
                                                    SoldBy = objModel.objCustomer.UserDetails.PartyCode,
                                                    BillDate = BillDate.Date,
                                                    BillType = "V",
                                                    BillNo = $"{billPrefix}/{objModel.objCustomer.UserDetails.PartyCode}/{maxSbillNo}",
                                                    PayPrefix = value,
                                                    Amount = objModel.objProduct.PayDetails.AmountByWallet,
                                                    CardNo = objModel.objCustomer.CardNo,
                                                    Narration = "",
                                                    ActiveStatus = "Y",
                                                    RecTimeStamp = DateTime.Now,
                                                    UserId = objModel.objCustomer.UserDetails.UserId,
                                                    UserName = objModel.objCustomer.UserDetails.UserName,
                                                    Version = version,
                                                    FSessId = FsessId ?? 0,
                                                    SBillNo = maxSbillNo,
                                                    BankCode = 0,
                                                    BankName = "",
                                                    AcNo = "",
                                                    IFSCode = "",
                                                    DUserId = 0,
                                                    DRecTimeStamp = null,
                                                    ChqDDDate = null,
                                                    ChqDDNo = ""
                                                });

                                                IsWalletEntry = true; // notify wallet entry created
                                            }
                                            else
                                            {
                                                objResponse.ResponseStatus = "FAILED";
                                                objResponse.ResponseMessage = "Something went wrong";
                                                return objResponse;
                                            }
                                        }
                                    }

                                    if (objModel.objProduct.PayDetails.IsP)
                                    {
                                        EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Paytm;
                                        string value = EnumPayModes.GetEnumDescription(enumVar);
                                        PayPrefix.Add(value);
                                        objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = BillDate.Date, BillType = "V", BillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo, PayPrefix = value, Amount = objModel.objProduct.PayDetails.AmountByPaytm, BankCode = 0, BankName = "", AcNo = "", IFSCode = "", Narration = "", DUserId = 0, DRecTimeStamp = null, ChqDDNo = objModel.objProduct.PayDetails.PaytmTransactionId, ChqDDDate = DateTime.Now, CardNo = "", ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                                    }
                                    if (objModel.objProduct.CashAmount > 0)
                                    {
                                        EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Cash;
                                        string value = EnumPayModes.GetEnumDescription(enumVar);
                                        PayPrefix.Add(value);
                                        objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = BillDate.Date, BillType = "V", BillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo, PayPrefix = value, Amount = objModel.objProduct.CashAmount, BankCode = 0, BankName = "", AcNo = "", IFSCode = "", Narration = "", DUserId = 0, DRecTimeStamp = null, ChqDDNo = "", ChqDDDate = null, CardNo = "", ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                                    }

                                    if (PayPrefix.Count > 0)
                                    {
                                        string query = @"SELECT PayMode 
                                                       FROM M_PayModeMaster 
                                                      WHERE Prefix IN @Prefixes";


                                        Paymode = connection.Query<string>(query, new { Prefixes = PayPrefix }).ToList();
                                    }
                                }
                            }

                            if (!string.IsNullOrEmpty(objModel.TaxORStock) && objModel.TaxORStock.ToLower() == "stock")
                            {
                                string sqlrrr = @"
                                 SELECT ISNULL(MAX(UserSBillNo), 0)
        FROM TrnBillMain
        WHERE FSessId = @FSessId 
          AND SoldBy = @SoldBy 
          AND BillType = 'S'";

                                maxUserSBillNo = connection.ExecuteScalar<int>(sqlrrr, new
                                {
                                    FSessId = FsessId,
                                    SoldBy = objModel.objCustomer.UserDetails.PartyCode
                                });

                                maxUserSBillNo += 1;

                                strMaxUserSBillNo = maxUserSBillNo.ToString().PadLeft(3, '0');

                                UserBillNo = $"{billPrefix}/ST/{strMaxUserSBillNo}";
                            }

                            string SoldByCode = "";
                            List<TrnBillData> tempTableList = new List<TrnBillData>();
                            string GroupPrefix = "";
                            string BillingPartyCode = objModel.objCustomer.PartyCode;
                            string GroupPrefixquery = @"
                                      SELECT TOP 1 g.Prefix
                                     FROM M_GroupMaster g
                                     INNER JOIN M_LedgerMaster l ON g.GroupId = l.GroupId
                                     WHERE l.PartyCode = @BillingPartyCode";
                            GroupPrefix = connection.QueryFirstOrDefault<string>(GroupPrefixquery, new
                            {
                                BillingPartyCode = BillingPartyCode
                            });

                            try
                            {
                                List<ProductModel> objListProductModel = new List<ProductModel>();
                                foreach (var obj in objModel.objListProduct)
                                {
                                    objListProductModel.Add(obj);
                                    TrnBillData objDTBillData = new TrnBillData();
                                    objDTBillData.SBillNo = maxSbillNo;
                                    objDTBillData.FSessId = FsessId ?? 0;
                                    objDTBillData.SessId = SessId ?? 0;
                                    objDTBillData.ActiveStatus = "Y";
                                    objDTBillData.BillDate = BillDate.Date;

                                    objDTBillData.RefNo = string.IsNullOrEmpty(objModel.objCustomer.ReferenceIdNo) ? "" : objModel.objCustomer.ReferenceIdNo;
                                    objDTBillData.RefId = 0;
                                    objDTBillData.RefName = "";
                                    objDTBillData.Remarks = string.IsNullOrEmpty(objModel.objCustomer.Remarks) ? "" : objModel.objCustomer.Remarks;
                                    objDTBillData.CType = GroupPrefix;
                                    objDTBillData.SoldBy = objModel.objCustomer.UserDetails.PartyCode;
                                    SoldByCode = objDTBillData.SoldBy;
                                    objDTBillData.BillBy = objDTBillData.SoldBy;
                                    objDTBillData.BillNo = billPrefix + "/" + objDTBillData.BillBy + "/" + maxSbillNo;
                                    objDTBillData.FType = GroupPrefix;
                                    objDTBillData.FCode = objModel.objCustomer.PartyCode;
                                    objDTBillData.PartyName = objModel.objCustomer.PartyName;
                                    objDTBillData.SupplierId = 0;
                                    objDTBillData.ChDDNo = 0;
                                    objDTBillData.ChDate = DateTime.Now;
                                    objDTBillData.ChAmt = 0;
                                    objDTBillData.BankCode = 0;
                                    objDTBillData.BankName = "";
                                    objDTBillData.FormNo = 0;
                                    objDTBillData.TotalTaxAmount = objModel.objProduct.TotalTaxAmount;
                                    objDTBillData.TotalSTaxAmount = 0;
                                    objDTBillData.TotalDiscount = objModel.objProduct.TotalDiscount;
                                    objDTBillData.TotalKitBvValue = 0;
                                    objDTBillData.TotalBvValue = objModel.objProduct.TotalBV;
                                    objDTBillData.TotalCVValue = objModel.objProduct.TotalCV;
                                    objDTBillData.TotalPVValue = objModel.objProduct.TotalPV;
                                    objDTBillData.TotalRPValue = objModel.objProduct.TotalRP;

                                    objDTBillData.DP = obj.DP ?? 0;
                                    objDTBillData.RP = obj.RP ?? 0;
                                    objDTBillData.MRP = obj.MRP ?? 0;
                                    objDTBillData.CVValue = obj.CVValue ?? 0;
                                    objDTBillData.CV = obj.CV ?? 0;
                                    objDTBillData.PV = obj.PV ?? 0;
                                    objDTBillData.BV = obj.BV ?? 0;
                                    objDTBillData.BVValue = obj.BVValue ?? 0;
                                    objDTBillData.PVValue = obj.PVValue ?? 0;
                                    objDTBillData.RPValue = obj.RPValue ?? 0;
                                    objDTBillData.Barcode = obj.Barcode.ToString();
                                    objDTBillData.BatchNo = obj.BatchNo.ToString();

                                    objDTBillData.DiscountPer = obj.DiscPer ?? 0;
                                    objDTBillData.Discount = obj.DiscAmt ?? 0;
                                    objDTBillData.ProdCommssn = obj.CommissionPer ?? 0;
                                    objDTBillData.ProdCommssnAmt = obj.CommissionAmt ?? 0;
                                    objDTBillData.ProductId = obj.ProdCode.ToString();
                                    objDTBillData.ProductName = obj.ProductName;
                                    objDTBillData.Qty = obj.Quantity;
                                    objDTBillData.Rate = obj.Rate ?? 0;
                                    objDTBillData.IsKitBV = "N";
                                    objDTBillData.DSeries = "";
                                    objDTBillData.DImported = "N";
                                    objDTBillData.IMEINo = "D";
                                    objDTBillData.BNo = "";
                                    objDTBillData.ItemType = "N";



                                    objDTBillData.JType = "Cash:" + objModel.objProduct.TotalNetPayable;
                                    objDTBillData.BillTo = objModel.objCustomer.PartyCode;
                                    objDTBillData.BillFor = objModel.objCustomer.PartyCode;
                                    objDTBillData.IsReceive = "N";
                                    objDTBillData.IsCredit = "F";
                                    //objDTBillData.BillType = "R";
                                    if (objModel.TaxORStock.ToLower() == "tax")
                                    {
                                        objDTBillData.BillType = "V";
                                    }
                                    else
                                    {
                                        objDTBillData.BillType = "S";
                                    }

                                    objDTBillData.ProdType = "P";
                                    objDTBillData.PaymentDtl = "Cash:" + objModel.objProduct.TotalNetPayable;

                                    objDTBillData.TotalAmount = objModel.objProduct.TotalTotalAmount;
                                    //tax excluding
                                    objDTBillData.NetAmount = obj.Amount;
                                    if (objModel.objCustomer.StateCode == objModel.objCustomer.UserDetails.StateCode)
                                    {
                                        objDTBillData.TaxAmount = 0;
                                        objDTBillData.Tax = 0;
                                        objDTBillData.CGST = obj.TaxPer / 2 ?? 0;
                                        objDTBillData.CGSTAmt = obj.TaxAmt / 2 ?? 0;
                                        objDTBillData.SGST = obj.TaxPer / 2 ?? 0;
                                        objDTBillData.SGSTAmt = obj.TaxAmt / 2 ?? 0;
                                        objDTBillData.TaxType = "S";
                                    }
                                    else
                                    {

                                        objDTBillData.TaxAmount = obj.TaxAmt ?? 0;
                                        if (obj.OldTaxAmount != 0 && obj.OldTaxAmount != obj.TaxAmt)
                                        {
                                            objDTBillData.TaxAmount = Decimal.Parse((Convert.ToDouble(objDTBillData.TaxAmount) + 0.01).ToString());
                                            objDTBillData.NetAmount = Decimal.Parse((Convert.ToDouble(objDTBillData.NetAmount) - 0.01).ToString());
                                        }
                                        objDTBillData.Tax = obj.TaxPer ?? 0;
                                        objDTBillData.CGST = 0;
                                        objDTBillData.CGSTAmt = 0;
                                        objDTBillData.SGST = 0;
                                        objDTBillData.SGSTAmt = 0;
                                        objDTBillData.TaxType = "I";
                                    }
                                    objDTBillData.CashDiscPer = obj.CashDiscPer;
                                    objDTBillData.CashDiscAmount = obj.CashDiscAmount;

                                    objDTBillData.NetPayable = Math.Round(objModel.objProduct.TotalNetPayable);
                                    if (objModel.objProduct.Roundoff == 0)
                                    {
                                        objDTBillData.RndOff = objDTBillData.NetPayable - objModel.objProduct.TotalNetPayable;
                                    }
                                    else
                                    {
                                        objDTBillData.RndOff = objModel.objProduct.Roundoff;
                                    }
                                    objDTBillData.CardAmount = 0;
                                    objDTBillData.PayMode = Paymode.Count > 1 ? string.Join(",", Paymode) : Paymode[0];
                                    objDTBillData.PayPrefix = PayPrefix.Count > 1 ? string.Join(",", PayPrefix) : PayPrefix[0];
                                    objDTBillData.BvTransfer = "N";

                                    //objDTBillData.UserSBillNo = maxSbillNo;
                                    //objDTBillData.UserBillNo = billPrefix + "/" + objDTBillData.BillBy + "/" + maxSbillNo;
                                    objDTBillData.UserSBillNo = maxUserSBillNo;
                                    objDTBillData.UserBillNo = UserBillNo;
                                    objDTBillData.DispatchStatus = "N";
                                    objDTBillData.LR = "0";
                                    objDTBillData.LRDate = DateTime.Now;
                                    objDTBillData.TransporterName = "";
                                    objDTBillData.DispatchTo = "";
                                    objDTBillData.FreightType = "";
                                    objDTBillData.Series = "";
                                    objDTBillData.Scratch = "";

                                    objDTBillData.Unit = 0;

                                    objDTBillData.PSessId = 0;
                                    objDTBillData.DcNo = "";
                                    objDTBillData.Imported = "N";
                                    objDTBillData.FPoint = 0;
                                    objDTBillData.FPointValue = 0;
                                    objDTBillData.OrdStatus = "";
                                    objDTBillData.OrdQty = 0;
                                    // objDTBillData.OrderType = "";
                                    objDTBillData.OrderDate = DateTime.Now;
                                    objDTBillData.OrderNo = "";
                                    objDTBillData.RemQty = 0;
                                    objDTBillData.DP1 = 0;
                                    objDTBillData.DReason = "";
                                    objDTBillData.DUserId = 0;
                                    objDTBillData.DRecTimeStamp = DateTime.Now;
                                    objDTBillData.DocWeight = objModel.objProduct.TotalWeight;
                                    objDTBillData.DocketNo = "";
                                    objDTBillData.DocketDate = DateTime.Now;
                                    //objDTBillData.UserBillNo = "";
                                    //objDTBillData.UserSBillNo = 0;
                                    objDTBillData.STNFormNo = "";
                                    objDTBillData.StkRecv = "N";
                                    objDTBillData.StkRecvDate = DateTime.Now;
                                    objDTBillData.StkRecvUserId = 0;
                                    objDTBillData.InTransit = "N";
                                    objDTBillData.UID = string.IsNullOrEmpty(objModel.objProduct.UID) ? "" : objModel.objProduct.UID;
                                    objDTBillData.OfferUID = 0;
                                    objDTBillData.IsKit = "N";
                                    objDTBillData.TotalCorton = "";
                                    objDTBillData.TotalMonoCorton = "";
                                    objDTBillData.SpclOfferId = 0;
                                    objDTBillData.VAT = 0;
                                    objDTBillData.BuyerAddress = "";
                                    objDTBillData.BuyerTIN = "";

                                    objDTBillData.TotalDiscount = objModel.objProduct.TotalDiscPer;
                                    objDTBillData.TotalDiscountAmt = objModel.objProduct.TotalDiscount;
                                    objDTBillData.VDiscountAmt = 0;
                                    objDTBillData.VDiscount = 0;
                                    objDTBillData.ReceiverID = "";
                                    objDTBillData.ReceiverName = "";
                                    objDTBillData.ReceiverMNo = "";
                                    objDTBillData.ReceiverIDProof = "";
                                    objDTBillData.TotalFPoint = 0;
                                    objDTBillData.TotalQty = objModel.objProduct.TotalQty;
                                    objDTBillData.CashReward = 0;
                                    objDTBillData.CommssnAmt = objModel.objProduct.TotalCommsonAmt;
                                    objDTBillData.RecvAmount = 0;
                                    objDTBillData.ReturnToCustAmt = 0;
                                    objDTBillData.ActiveStatus = "Y";
                                    objDTBillData.RecTimeStamp = DateTime.Now;
                                    objDTBillData.UserId = objModel.objCustomer.UserDetails.UserId;
                                    objDTBillData.UserName = objModel.objCustomer.UserDetails.UserName;
                                    objDTBillData.DelvPlace = string.IsNullOrEmpty(objModel.objProduct.DeliveryPlace) ? "" : objModel.objProduct.DeliveryPlace;
                                    objDTBillData.DelvStatus = "";
                                    objDTBillData.DelvUserId = 0;
                                    objDTBillData.DelvRecTimeStamp = DateTime.Now;
                                    objDTBillData.Version = version;
                                    objDTBillData.IDType = "";
                                    objDTBillData.BranchName = "";
                                    objDTBillData.CourierId = 0;
                                    objDTBillData.CourierName = "";
                                    objDTBillData.LocId = 0;
                                    objDTBillData.LocName = "";
                                    objDTBillData.DelvAddress = "";
                                    objDTBillData.Pincode = "";
                                    objDTBillData.OrderType = objModel.PartyInvoice;

                                    objDTBillData.Coupon = "";
                                    objDTBillData.CouponAmount = 0;
                                    objDTBillData.PaidBV = 0;
                                    objDTBillData.IRNNo = "";
                                    objDTBillData.AckNo = "";
                                    objDTBillData.AckDate = DateTime.Now;
                                    objDTBillData.QrCodeimage = "";
                                    objDTBillData.QrCode = "";
                                    objDTBillData.SignedInvoice = "";

                                    billno_ = objDTBillData.BillNo;
                                    soldby_ = objDTBillData.SoldBy;
                                    fcode_ = objDTBillData.FCode;
                                    netpayable_ = objDTBillData.NetPayable;
                                    narration_ = "Wallet deducted against " + objDTBillData.UserBillNo + ".";


                                    string insertSql = @"
            INSERT INTO TrnBillData
            (
               SBillNo, FSessId, SessId, ActiveStatus, BillDate, RefNo, RefId, RefName, Remarks,
    CType, SoldBy, BillBy, BillNo, FType, FCode, PartyName, SupplierId, ChDDNo, ChDate,
    ChAmt, BankCode, BankName, FormNo, TotalTaxAmount, TotalSTaxAmount, 
    TotalKitBvValue, TotalBvValue, TotalCVValue, TotalPVValue, TotalRPValue, DP, RP, MRP,
    CVValue, CV, PV, BV, BVValue, PVValue, RPValue, Barcode, BatchNo, DiscountPer,
    Discount, ProdCommssn, ProdCommssnAmt, ProductId, ProductName, Qty, Rate, IsKitBV,
    DSeries, DImported, IMEINo, BNo, ItemType, JType, BillTo, BillFor, IsReceive, IsCredit,
    BillType, ProdType, PaymentDtl, TotalAmount, NetAmount, TaxAmount, Tax, CGST, CGSTAmt,
    SGST, SGSTAmt, TaxType, CashDiscPer, CashDiscAmount, NetPayable, RndOff, CardAmount,
    PayMode, PayPrefix, BvTransfer, UserSBillNo, UserBillNo, DispatchStatus, LR, LRDate,
    TransporterName, DispatchTo, FreightType, Series, Scratch, Unit, PSessId, DcNo, Imported,
    FPoint, FPointValue, OrdStatus, OrdQty, OrderDate, OrderNo, RemQty, DP1, DReason, DUserId,
    DRecTimeStamp, DocWeight, DocketNo, DocketDate, STNFormNo, StkRecv, StkRecvDate, StkRecvUserId,
    InTransit, UID, OfferUID, IsKit, TotalCorton, TotalMonoCorton, SpclOfferId, VAT, BuyerAddress,
    BuyerTIN, TotalDiscountAmt, VDiscountAmt, VDiscount, ReceiverID, ReceiverName,
    ReceiverMNo, ReceiverIDProof, TotalFPoint, TotalQty, CashReward, CommssnAmt, RecvAmount,
    ReturnToCustAmt, RecTimeStamp, UserId, UserName, DelvPlace, DelvStatus, DelvUserId,
    DelvRecTimeStamp, Version, IDType, BranchName, CourierId, CourierName, LocId, LocName,
    DelvAddress, Pincode, OrderType, Coupon, CouponAmount, PaidBV, IRNNo, AckNo, AckDate, 
    QrCodeimage, QrCode, SignedInvoice,BillGSTType
            )
            VALUES
            (
               @SBillNo, @FSessId, @SessId, @ActiveStatus, @BillDate, @RefNo, @RefId, @RefName, @Remarks,
    @CType, @SoldBy, @BillBy, @BillNo, @FType, @FCode, @PartyName, @SupplierId, @ChDDNo, @ChDate,
    @ChAmt, @BankCode, @BankName, @FormNo, @TotalTaxAmount, @TotalSTaxAmount, 
    @TotalKitBvValue, @TotalBvValue, @TotalCVValue, @TotalPVValue, @TotalRPValue, @DP, @RP, @MRP,
    @CVValue, @CV, @PV, @BV, @BVValue, @PVValue, @RPValue, @Barcode, @BatchNo, @DiscountPer,
    @Discount, @ProdCommssn, @ProdCommssnAmt, @ProductId, @ProductName, @Qty, @Rate, @IsKitBV,
    @DSeries, @DImported, @IMEINo, @BNo, @ItemType, @JType, @BillTo, @BillFor, @IsReceive, @IsCredit,
    @BillType, @ProdType, @PaymentDtl, @TotalAmount, @NetAmount, @TaxAmount, @Tax, @CGST, @CGSTAmt,
    @SGST, @SGSTAmt, @TaxType, @CashDiscPer, @CashDiscAmount, @NetPayable, @RndOff, @CardAmount,
    @PayMode, @PayPrefix, @BvTransfer, @UserSBillNo, @UserBillNo, @DispatchStatus, @LR, @LRDate,
    @TransporterName, @DispatchTo, @FreightType, @Series, @Scratch, @Unit, @PSessId, @DcNo, @Imported,
    @FPoint, @FPointValue, @OrdStatus, @OrdQty, @OrderDate, @OrderNo, @RemQty, @DP1, @DReason, @DUserId,
    @DRecTimeStamp, @DocWeight, @DocketNo, @DocketDate, @STNFormNo, @StkRecv, @StkRecvDate, @StkRecvUserId,
    @InTransit, @UID, @OfferUID, @IsKit, @TotalCorton, @TotalMonoCorton, @SpclOfferId, @VAT, @BuyerAddress,
    @BuyerTIN, @TotalDiscountAmt, @VDiscountAmt, @VDiscount, @ReceiverID, @ReceiverName,
    @ReceiverMNo, @ReceiverIDProof, @TotalFPoint, @TotalQty, @CashReward, @CommssnAmt, @RecvAmount,
    @ReturnToCustAmt, @RecTimeStamp, @UserId, @UserName, @DelvPlace, @DelvStatus, @DelvUserId,
    @DelvRecTimeStamp, @Version, @IDType, @BranchName, @CourierId, @CourierName, @LocId, @LocName,
    @DelvAddress, @Pincode, @OrderType, @Coupon, @CouponAmount, @PaidBV, @IRNNo, @AckNo, @AckDate,
    @QrCodeimage, @QrCode, @SignedInvoice,'G'
            )
        ";

                                    TrnBillDatasAffected = await connection.ExecuteAsync(insertSql, objDTBillData);
                                }

                                if (TrnBillDatasAffected > 0)
                                {
                                    if(objModel.PartyInvoice=="B" && objModel.objProduct.PayDetails.AmountByVoucher>0)
                                    {
                                        //Jab bhi franchise / party bill banta he to usme Promo/Voucher use hota he to uska BV sale limit nahi badhana he
                                        netpayable_ = netpayable_- objModel.objProduct.PayDetails.AmountByVoucher;
                                    }

                                    CreditPartyWallet(billno_, "Wallet Credited against " + UserBillNo + ".", SoldByCode, fcode_, netpayable_, objModel.PartyInvoice);

                                    var resultPayMode = connection.Query<M_PayModeMaster>(
                                            "SELECT Prefix, PayMode FROM M_PayModeMaster").ToList();

                                    foreach (var obj in objDTListPayMode)
                                    {
                                        TrnPayModeDetail objTemp = obj;
                                        objTemp.BillType = "V";

                                        objTemp.PayMode = resultPayMode
                                            .Where(x => x.Prefix.Trim() == obj.PayPrefix.Trim())
                                            .Select(x => x.PayMode)
                                            .FirstOrDefault();

                                        if (string.IsNullOrEmpty(objTemp.CardNo))
                                            objTemp.CardNo = "";

                                        string insertSql = @"
            INSERT INTO TrnPayModeDetail
            (
                 FSessId,SBillNo,BillNo,SoldBy,BillDate,PayPrefix,PayMode, 
  ChqDDNo, 
 ChqDDDate, 
  CardNo, 
 BankCode, 
  BankName, 
 Amount, 
 BillAmt, 
  Narration, 
  ActiveStatus, 
 RecTimeStamp, 
  Version, 
 UserId, 
  UserName, 
  BillType, 
 DUserId, 
 DRecTimeStamp, 
  AcNo, 
  IFSCode 
            )
            VALUES
            (
                @FSessId,@SBillNo,@BillNo,@SoldBy,@BillDate,@PayPrefix,@PayMode, 
  @ChqDDNo, @ChqDDDate,@CardNo, 
 @BankCode, 
  @BankName, 
 @Amount, 
 @BillAmt, 
  @Narration, 
  @ActiveStatus, 
 @RecTimeStamp, 
  @Version, 
 @UserId, 
  @UserName, 
  @BillType, 
 @DUserId, 
 @DRecTimeStamp, 
  @AcNo, 
  @IFSCode 
            )";

                                        connection.Execute(insertSql, objTemp);
                                    }
                                    //if (objModel.EInvoice == "Y")
                                    //{
                                    //    Make_Inv_json(UserBillNo);

                                    //}
                                    objResponse.ResponseMessage = "Saved Successfully!";
                                    objResponse.ResponseStatus = "OK";
                                    objResponse.ResponseDetailsToPrint = new DistributorBillModel();
                                    objResponse.ResponseDetailsToPrint.BillNo = UserBillNo;
                                    objResponse.ResponseDetailsToPrint.SoldBy = SoldByCode;
                                }
                                else
                                {
                                    objResponse.ResponseMessage = "Bill not saved";
                                    objResponse.ResponseStatus = "FAILED";
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                    else
                    {
                        // saving process of customer bill
                        //customer bill
                        //customer bill
                        //customer bill
                        DateTime BillDate = DateTime.Now.Date;
                        IsWalletEntry = false;
                        if (objModel != null)
                        {
                            if (objModel.objProduct.PayDetails != null)
                            {
                                if (objModel.objProduct.PayDetails.IsBD)
                                {
                                    EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.BankDeposit;
                                    string value = EnumPayModes.GetEnumDescription(enumVar);
                                    PayPrefix.Add(value);
                                    objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = BillDate.Date, BillType = "G", BillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo, PayPrefix = value, Amount = objModel.objProduct.PayDetails.AmountByBD, BankCode = objModel.objProduct.PayDetails.BankCode, ChqDDDate = null, ChqDDNo = "", CardNo = "", Narration = "", DUserId = 0, DRecTimeStamp = null, BankName = objModel.objProduct.PayDetails.BDBankName, AcNo = "", IFSCode = "", ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                                }
                                if (objModel.objProduct.PayDetails.IsCC)
                                {
                                    EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Card;
                                    string value = EnumPayModes.GetEnumDescription(enumVar);
                                    PayPrefix.Add(value);
                                    objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = BillDate.Date, BillType = "G", BillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo, PayPrefix = value, AcNo = "", IFSCode = "", BankCode = 0, Narration = "", BankName = "", DUserId = 0, DRecTimeStamp = null, ChqDDNo = "", ChqDDDate = null, Amount = objModel.objProduct.PayDetails.AmountByCard, CardNo = objModel.objProduct.PayDetails.CardNo, ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                                }
                                if (objModel.objProduct.PayDetails.IsQ)
                                {
                                    EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Cheque;
                                    string value = EnumPayModes.GetEnumDescription(enumVar);
                                    PayPrefix.Add(value);
                                    objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = BillDate.Date, BillType = "G", BillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo, PayPrefix = value, Amount = objModel.objProduct.PayDetails.AmountByCheque, CardNo = "", AcNo = "", IFSCode = "", BankCode = 0, Narration = "", DUserId = 0, DRecTimeStamp = null, BankName = objModel.objProduct.PayDetails.CHBankName, ChqDDNo = objModel.objProduct.PayDetails.ChequeNo, ChqDDDate = objModel.objProduct.PayDetails.ChequeDate, ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                                }
                                if (objModel.objProduct.PayDetails.IsD)
                                {
                                    EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.DD;
                                    string value = EnumPayModes.GetEnumDescription(enumVar);
                                    PayPrefix.Add(value);
                                    objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = BillDate.Date, BillType = "G", BillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo, PayPrefix = value, Amount = objModel.objProduct.PayDetails.AmountByDD, CardNo = "", AcNo = "", IFSCode = "", BankCode = 0, Narration = "", DUserId = 0, DRecTimeStamp = null, BankName = objModel.objProduct.PayDetails.DDBankName, ChqDDNo = objModel.objProduct.PayDetails.DDNo, ChqDDDate = objModel.objProduct.PayDetails.DDDate, ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                                }
                                if (objModel.objProduct.PayDetails.IsT)
                                {
                                    EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Credit;
                                    string value = EnumPayModes.GetEnumDescription(enumVar);
                                    PayPrefix.Add(value);
                                    objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = BillDate.Date, BillType = "G", BillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo, PayPrefix = value, BankName = "", Amount = objModel.objProduct.PayDetails.AmountByCredit, CardNo = "", AcNo = "", IFSCode = "", BankCode = 0, DUserId = 0, DRecTimeStamp = null, ChqDDDate = null, ChqDDNo = "", Narration = objModel.objProduct.PayDetails.Narration, ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                                }
                                if (objModel.objProduct.PayDetails.IsV)
                                {
                                    EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Voucher;
                                    string value = EnumPayModes.GetEnumDescription(enumVar);
                                    PayPrefix.Add(value);
                                    objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = BillDate.Date, BillType = "G", BillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo, PayPrefix = value, Amount = objModel.objProduct.PayDetails.AmountByVoucher, CardNo = "", AcNo = "", IFSCode = "", BankCode = 0, DUserId = 0, DRecTimeStamp = null, ChqDDDate = null, ChqDDNo = "", Narration = "", BankName = "", ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                                }
                                if (objModel.objProduct.PayDetails.IsW)
                                {
                                    if (WalletBalance >= objModel.objProduct.PayDetails.AmountByWallet)
                                    {
                                        var query = "INSERT INTO TrnVoucher(VoucherNo,VoucherDate,DrTo,Crto,Amount,Narration,Refno,AcType,VType,SessID,WSEssID) " +
                                                       "Select CASE WHEN Max(VoucherNo) is NULL THEN 1 ELSE Max(VoucherNo)+1 END ,Cast(Convert(varchar,Getdate(),106) as Datetime),'" + objModel.objCustomer.FormNo + "','0','" + objModel.objProduct.PayDetails.AmountByWallet + "','Product purchased Against " + UserBillNo + ".','" + billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo + "','R','D','" + SessId + "','" + SessId + "' FROM TrnVoucher";
                                        var wallaffect = await connection.ExecuteAsync(query);
                                        if (wallaffect > 0)
                                        {
                                            EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Wallet;
                                            string value = EnumPayModes.GetEnumDescription(enumVar);
                                            PayPrefix.Add(value);
                                            objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = BillDate.Date, BillType = "G", BillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo, PayPrefix = value, Amount = objModel.objProduct.PayDetails.AmountByWallet, BankCode = 0, BankName = "", AcNo = "", IFSCode = "", Narration = "", DUserId = 0, DRecTimeStamp = null, ChqDDNo = "", ChqDDDate = null, CardNo = objModel.objCustomer.CardNo, ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                                            ////insert entry into couponsalesdetails for wallet
                                            IsWalletEntry = true;
                                        }
                                        else
                                        {
                                            objResponse.ResponseStatus = "FAILED";
                                            objResponse.ResponseMessage = "Something went wrong";
                                            return objResponse;
                                        }
                                    }
                                    else
                                    {
                                        objResponse.ResponseStatus = "FAILED";
                                        objResponse.ResponseMessage = "Sorry!Insufficient Wallet Balance.";
                                        return objResponse;
                                    }
                                }
                                if (objModel.objProduct.PayDetails.IsP)
                                {
                                    EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Paytm;
                                    string value = EnumPayModes.GetEnumDescription(enumVar);
                                    PayPrefix.Add(value);
                                    objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = BillDate.Date, BillType = "G", BillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo, PayPrefix = value, Amount = objModel.objProduct.PayDetails.AmountByPaytm, BankCode = 0, BankName = "", AcNo = "", IFSCode = "", Narration = "", DUserId = 0, DRecTimeStamp = null, ChqDDNo = objModel.objProduct.PayDetails.PaytmTransactionId, ChqDDDate = DateTime.Now, CardNo = "", ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                                }
                                if (objModel.objProduct.CashAmount > 0)
                                {
                                    EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Cash;
                                    string value = EnumPayModes.GetEnumDescription(enumVar);
                                    PayPrefix.Add(value);
                                    objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, 
                                        SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = BillDate.Date, 
                                        BillType = "G", BillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo,
                                        PayPrefix = value, Amount = objModel.objProduct.CashAmount, BankCode = 0, BankName = "", AcNo = "",
                                        IFSCode = "", Narration = "", DUserId = 0, DRecTimeStamp = null, ChqDDNo = "", ChqDDDate = null, CardNo = "",
                                        ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, 
                                        Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                                }

                                if (PayPrefix.Count > 0)
                                {
                                    string commaSeparatedString = "'" + string.Join("','", PayPrefix) + "'";
                                    var query = "SELECT PayMode FROM M_PayModeMaster WHERE Prefix IN(" + commaSeparatedString + ")";
                                    //string commaSeparatedString = string.Join(",", PayPrefix);

                                    // string commaSeparatedStringwithsingle= string.Join("''", PayPrefix);
                                    Paymode = (await connection.QueryAsync<string>(query)).ToList();
                                }
                            }
                        }

                        string SoldByCode = "";
                        List<TrnBillData> tempTableList = new List<TrnBillData>();
                        string GroupPrefix = "";
                        string BillingPartyCode = objModel.objCustomer.PartyCode;
                        string GroupPrefixquery = @"
                                      SELECT TOP 1 g.Prefix
                                     FROM M_GroupMaster g
                                     INNER JOIN M_LedgerMaster l ON g.GroupId = l.GroupId
                                     WHERE l.PartyCode = @BillingPartyCode";
                        GroupPrefix = connection.QueryFirstOrDefault<string>(GroupPrefixquery, new
                        {
                            BillingPartyCode = BillingPartyCode
                        });
                        if (string.IsNullOrEmpty(GroupPrefix))
                        {
                            GroupPrefix = "W";
                        }
                        try
                        {
                            List<ProductModel> objListProductModel = new List<ProductModel>();
                            //TempDistributor.objListProduct.AddRange(objModel.objListProduct);
                            foreach (var obj in objModel.objListProduct)
                            {
                                objListProductModel.Add(obj);
                                TrnBillData objDTBillData = new TrnBillData();
                                objDTBillData.SBillNo = maxSbillNo;
                                objDTBillData.FSessId = FsessId ?? 0;
                                objDTBillData.SessId = SessId ?? 0;
                                objDTBillData.ActiveStatus = "Y";
                                objDTBillData.BillDate = BillDate.Date;

                                objDTBillData.RefNo = string.IsNullOrEmpty(objModel.objCustomer.ReferenceIdNo) ? "" : objModel.objCustomer.ReferenceIdNo;
                                objDTBillData.RefId = 0;
                                objDTBillData.RefName = "";
                                objDTBillData.Remarks = string.IsNullOrEmpty(objModel.objCustomer.Remarks) ? "" : objModel.objCustomer.Remarks;
                                objDTBillData.CType = GroupPrefix;
                                objDTBillData.SoldBy = objModel.objCustomer.UserDetails.PartyCode;
                                SoldByCode = objDTBillData.SoldBy;
                                objDTBillData.BillBy = objDTBillData.SoldBy;
                                objDTBillData.BillNo = billPrefix + "/" + objDTBillData.BillBy + "/" + maxSbillNo;
                                objDTBillData.FType = GroupPrefix;
                                objDTBillData.FCode = string.IsNullOrEmpty(objModel.objCustomer.Name) ? "" : objModel.objCustomer.Name;
                                objDTBillData.PartyName = string.IsNullOrEmpty(objModel.objCustomer.Name) ? "" : objModel.objCustomer.Name;
                                objDTBillData.SupplierId = 0;
                                objDTBillData.ChDDNo = 0;
                                objDTBillData.ChDate = DateTime.Now;
                                objDTBillData.ChAmt = 0;
                                objDTBillData.BankCode = objModel.objProduct.PayDetails.BankCode == null ? 0 : objModel.objProduct.PayDetails.BankCode;
                                objDTBillData.BankName = string.IsNullOrEmpty(objModel.objProduct.PayDetails.BDBankName) ? "" : objModel.objProduct.PayDetails.BDBankName;
                                objDTBillData.FormNo = 0;
                                objDTBillData.TotalTaxAmount = objModel.objProduct.TotalTaxAmount;
                                objDTBillData.TotalSTaxAmount = 0;
                                objDTBillData.TotalDiscount = objModel.objProduct.TotalDiscount;
                                objDTBillData.TotalKitBvValue = 0;
                                objDTBillData.TotalBvValue = objModel.objProduct.TotalBV;
                                objDTBillData.TotalCVValue = objModel.objProduct.TotalCV;
                                objDTBillData.TotalPVValue = objModel.objProduct.TotalPV;
                                objDTBillData.TotalRPValue = objModel.objProduct.TotalRP;

                                objDTBillData.DP = obj.DP ?? 0;
                                objDTBillData.RP = obj.RP ?? 0;
                                objDTBillData.MRP = obj.MRP ?? 0;
                                objDTBillData.CVValue = obj.CVValue ?? 0;
                                objDTBillData.CV = obj.CV ?? 0;
                                objDTBillData.PV = obj.PV ?? 0;
                                objDTBillData.BV = obj.BV ?? 0;
                                objDTBillData.BVValue = obj.BVValue ?? 0;
                                objDTBillData.PVValue = obj.PVValue ?? 0;
                                objDTBillData.RPValue = obj.RPValue ?? 0;
                                objDTBillData.Barcode = obj.Barcode.ToString();
                                objDTBillData.BatchNo = obj.BatchNo.ToString();

                                objDTBillData.DiscountPer = obj.DiscPer ?? 0;
                                objDTBillData.Discount = obj.DiscAmt ?? 0;
                                objDTBillData.ProdCommssn = obj.CommissionPer ?? 0;
                                objDTBillData.ProdCommssnAmt = obj.CommissionAmt ?? 0;
                                objDTBillData.ProductId = obj.ProdCode.ToString();
                                objDTBillData.ProductName = obj.ProductName;
                                objDTBillData.Qty = obj.Quantity;
                                objDTBillData.Rate = obj.Rate ?? 0;
                                objDTBillData.IsKitBV = "N";
                                objDTBillData.DSeries = "";
                                objDTBillData.DImported = "N";
                                objDTBillData.IMEINo = "D";
                                objDTBillData.BNo = "";
                                objDTBillData.ItemType = "N";



                                objDTBillData.JType = "Cash:" + objModel.objProduct.TotalNetPayable;
                                objDTBillData.BillTo = string.IsNullOrEmpty(objModel.objCustomer.Name) ? "" : objModel.objCustomer.Name;
                                objDTBillData.BillFor = string.IsNullOrEmpty(objModel.objCustomer.Name) ? "" : objModel.objCustomer.Name;
                                objDTBillData.IsReceive = "G";
                                objDTBillData.IsCredit = "F";
                                //objDTBillData.BillType = "R";
                                objDTBillData.BillType = "GC";
                                objDTBillData.ProdType = "P";
                                objDTBillData.PaymentDtl = "Cash:" + objModel.objProduct.TotalNetPayable;

                                objDTBillData.TotalAmount = objModel.objProduct.TotalTotalAmount;
                                //customer bill tax calaulating
                                objDTBillData.NetAmount = obj.Amount;
                                if (objModel.CustTaxType == "I")
                                {
                                    objDTBillData.TaxAmount = obj.TaxAmt ?? 0;
                                    if (obj.OldTaxAmount != 0 && obj.OldTaxAmount != obj.TaxAmt)
                                    {
                                        objDTBillData.TaxAmount = Decimal.Parse((Convert.ToDouble(objDTBillData.TaxAmount) + 0.01).ToString());
                                        objDTBillData.NetAmount = Decimal.Parse((Convert.ToDouble(objDTBillData.NetAmount) - 0.01).ToString());
                                    }
                                    objDTBillData.Tax = obj.TaxPer ?? 0;
                                    objDTBillData.CGST = 0;
                                    objDTBillData.CGSTAmt = 0;
                                    objDTBillData.SGST = 0;
                                    objDTBillData.SGSTAmt = 0;
                                    objDTBillData.TaxType = "I";
                                }
                                else
                                {
                                    objDTBillData.TaxAmount = 0;
                                    objDTBillData.Tax = 0;
                                    objDTBillData.CGST = obj.TaxPer / 2 ?? 0;
                                    objDTBillData.CGSTAmt = obj.TaxAmt / 2 ?? 0;
                                    objDTBillData.SGST = obj.TaxPer / 2 ?? 0;
                                    objDTBillData.SGSTAmt = obj.TaxAmt / 2 ?? 0;
                                    objDTBillData.TaxType = "S";
                                }
                                    

                                objDTBillData.CashDiscPer = obj.CashDiscPer;
                                objDTBillData.CashDiscAmount = obj.CashDiscAmount;

                                objDTBillData.NetPayable = Math.Round(objModel.objProduct.TotalNetPayable);
                                if (objModel.objProduct.Roundoff == 0)
                                {
                                    objDTBillData.RndOff = objDTBillData.NetPayable - objModel.objProduct.TotalNetPayable;
                                }
                                else
                                {
                                    objDTBillData.RndOff = objModel.objProduct.Roundoff;
                                }
                                objDTBillData.CardAmount = 0;
                                objDTBillData.PayMode = Paymode.Count > 1 ? string.Join(",", Paymode) : Paymode[0];
                                objDTBillData.PayPrefix = PayPrefix.Count > 1 ? string.Join(",", PayPrefix) : PayPrefix[0];
                                objDTBillData.BvTransfer = "N";

                                //objDTBillData.UserSBillNo = maxSbillNo;
                                //objDTBillData.UserBillNo = billPrefix + "/" + objDTBillData.BillBy + "/" + maxSbillNo;
                                objDTBillData.UserSBillNo = maxUserSBillNo;
                                objDTBillData.UserBillNo = UserBillNo;
                                objDTBillData.DispatchStatus = "N";
                                objDTBillData.LR = "0";
                                objDTBillData.LRDate = DateTime.Now;
                                objDTBillData.TransporterName = "";
                                objDTBillData.DispatchTo = "";
                                objDTBillData.FreightType = "";
                                objDTBillData.Series = "";
                                objDTBillData.Scratch = "";

                                objDTBillData.Unit = 0;

                                objDTBillData.PSessId = 0;
                                objDTBillData.DcNo = "";
                                objDTBillData.Imported = "N";
                                objDTBillData.FPoint = 0;
                                objDTBillData.FPointValue = 0;
                                objDTBillData.OrdStatus = "";
                                objDTBillData.OrdQty = 0;
                                // objDTBillData.OrderType = "";
                                objDTBillData.OrderDate = DateTime.Now;
                                objDTBillData.OrderNo = "";
                                objDTBillData.RemQty = 0;
                                objDTBillData.DP1 = 0;
                                objDTBillData.DReason = "";
                                objDTBillData.DUserId = 0;
                                objDTBillData.DRecTimeStamp = DateTime.Now;
                                objDTBillData.DocWeight = objModel.objProduct.TotalWeight;
                                objDTBillData.DocketNo = "";
                                objDTBillData.DocketDate = DateTime.Now;
                                //objDTBillData.UserBillNo = "";
                                //objDTBillData.UserSBillNo = 0;
                                objDTBillData.STNFormNo = "";
                                objDTBillData.StkRecv = "N";
                                objDTBillData.StkRecvDate = DateTime.Now;
                                objDTBillData.StkRecvUserId = 0;
                                objDTBillData.InTransit = "N";
                                objDTBillData.UID = string.IsNullOrEmpty(objModel.objProduct.UID) ? "" : objModel.objProduct.UID;
                                objDTBillData.OfferUID = 0;
                                objDTBillData.IsKit = "N";
                                objDTBillData.TotalCorton = "";
                                objDTBillData.TotalMonoCorton = "";
                                objDTBillData.SpclOfferId = 0;
                                objDTBillData.VAT = 0;
                                objDTBillData.BuyerAddress = "";
                                objDTBillData.BuyerTIN = "";

                                objDTBillData.TotalDiscount = objModel.objProduct.TotalDiscPer;
                                objDTBillData.TotalDiscountAmt = objModel.objProduct.TotalDiscount;
                                objDTBillData.VDiscountAmt = 0;
                                objDTBillData.VDiscount = 0;
                                objDTBillData.ReceiverID = "";
                                objDTBillData.ReceiverName = "";
                                objDTBillData.ReceiverMNo = string.IsNullOrEmpty(objModel.objCustomer.MobileNo) ? "" : objModel.objCustomer.MobileNo;
                                objDTBillData.ReceiverIDProof = "";
                                objDTBillData.TotalFPoint = 0;
                                objDTBillData.TotalQty = objModel.objProduct.TotalQty;
                                objDTBillData.CashReward = 0;
                                objDTBillData.CommssnAmt = objModel.objProduct.TotalCommsonAmt;
                                objDTBillData.RecvAmount = 0;
                                objDTBillData.ReturnToCustAmt = 0;
                                objDTBillData.ActiveStatus = "Y";
                                objDTBillData.RecTimeStamp = DateTime.Now;
                                objDTBillData.UserId = objModel.objCustomer.UserDetails.UserId;
                                objDTBillData.UserName = objModel.objCustomer.UserDetails.UserName;
                                objDTBillData.DelvPlace = string.IsNullOrEmpty(objModel.objProduct.DeliveryPlace) ? "" : objModel.objProduct.DeliveryPlace;
                                objDTBillData.DelvStatus = "";
                                objDTBillData.DelvUserId = 0;
                                objDTBillData.DelvRecTimeStamp = DateTime.Now;
                                objDTBillData.Version = version;
                                objDTBillData.IDType = "";
                                objDTBillData.BranchName = "";
                                objDTBillData.CourierId = 0;
                                objDTBillData.CourierName = "";
                                objDTBillData.LocId = 0;
                                objDTBillData.LocName = "";
                                objDTBillData.DelvAddress = "";
                                objDTBillData.Pincode = "";
                                objDTBillData.OrderType = "";
                                objDTBillData.Coupon = "";
                                objDTBillData.CouponAmount = 0;
                                objDTBillData.IRNNo = "";
                                objDTBillData.AckNo = "";
                                objDTBillData.AckDate = DateTime.Now;
                                objDTBillData.QrCodeimage = "";
                                objDTBillData.QrCode = "";
                                objDTBillData.SignedInvoice = "";
                                billno_ = objDTBillData.BillNo;
                                soldby_ = objDTBillData.SoldBy;
                                fcode_ = objDTBillData.FCode;
                                netpayable_ = objDTBillData.NetPayable;
                                narration_ = "Wallet deducted against " + objDTBillData.UserBillNo + ".";

                                string insertBillSql = @"
                INSERT INTO TrnBillData
                (SBillNo, FSessId, SessId, ActiveStatus, BillDate, RefNo, RefId, RefName, Remarks,
                 CType, SoldBy, BillBy, BillNo, FType, FCode, PartyName, SupplierId, ChDDNo, ChDate,
                 ChAmt, BankCode, BankName, FormNo, TotalTaxAmount, TotalSTaxAmount, TotalDiscount, 
                 TotalKitBvValue, TotalBvValue, TotalCVValue, TotalPVValue, TotalRPValue, DP, RP, MRP,
                 CVValue, CV, PV, BV, BVValue, PVValue, RPValue, Barcode, BatchNo, DiscountPer, Discount,
                 ProdCommssn, ProdCommssnAmt, ProductId, ProductName, Qty, Rate, IsKitBV, DSeries, 
                 DImported, IMEINo, BNo, ItemType, JType, BillTo, BillFor, IsReceive, IsCredit, BillType,
                 ProdType, PaymentDtl, TotalAmount, NetAmount, TaxAmount, Tax, CGST, CGSTAmt, SGST,
                 SGSTAmt, TaxType, CashDiscPer, CashDiscAmount, NetPayable, RndOff, CardAmount, PayMode,
                 PayPrefix, BvTransfer, UserSBillNo, UserBillNo, DispatchStatus, LR, LRDate, TransporterName,
                 DispatchTo, FreightType, Series, Scratch, Unit, PSessId, DcNo, Imported, FPoint, FPointValue,
                 OrdStatus, OrdQty, OrderDate, OrderNo, RemQty, DP1, DReason, DUserId, DRecTimeStamp,
                 DocWeight, DocketNo, DocketDate, STNFormNo, StkRecv, StkRecvDate, StkRecvUserId, InTransit,
                 UID, OfferUID, IsKit, TotalCorton, TotalMonoCorton, SpclOfferId, VAT, BuyerAddress,
                 BuyerTIN, TotalDiscountAmt, VDiscountAmt, VDiscount, ReceiverID, ReceiverName, ReceiverMNo,
                 ReceiverIDProof, TotalFPoint, TotalQty, CashReward, CommssnAmt, RecvAmount, ReturnToCustAmt,
                 RecTimeStamp, UserId, UserName, DelvPlace, DelvStatus, DelvUserId, DelvRecTimeStamp, Version,
                 IDType, BranchName, CourierId, CourierName, LocId, LocName, DelvAddress, Pincode, OrderType,
                 Coupon, CouponAmount, IRNNo, AckNo, AckDate, QrCodeimage, QrCode, SignedInvoice,BillGSTType,invoicetype)
                VALUES
                (
                 @SBillNo, @FSessId, @SessId, @ActiveStatus, @BillDate, @RefNo, @RefId, @RefName, @Remarks,
                 @CType, @SoldBy, @BillBy, @BillNo, @FType, @FCode, @PartyName, @SupplierId, @ChDDNo, @ChDate,
                 @ChAmt, @BankCode, @BankName, @FormNo, @TotalTaxAmount, @TotalSTaxAmount, @TotalDiscount,
                 @TotalKitBvValue, @TotalBvValue, @TotalCVValue, @TotalPVValue, @TotalRPValue, @DP, @RP, @MRP,
                 @CVValue, @CV, @PV, @BV, @BVValue, @PVValue, @RPValue, @Barcode, @BatchNo, @DiscountPer, 
                 @Discount, @ProdCommssn, @ProdCommssnAmt, @ProductId, @ProductName, @Qty, @Rate, @IsKitBV,
                 @DSeries, @DImported, @IMEINo, @BNo, @ItemType, @JType, @BillTo, @BillFor, @IsReceive, @IsCredit,
                 @BillType, @ProdType, @PaymentDtl, @TotalAmount, @NetAmount, @TaxAmount, @Tax, @CGST,
                 @CGSTAmt, @SGST, @SGSTAmt, @TaxType, @CashDiscPer, @CashDiscAmount, @NetPayable, @RndOff,
                 @CardAmount, @PayMode, @PayPrefix, @BvTransfer, @UserSBillNo, @UserBillNo, @DispatchStatus,
                 @LR, @LRDate, @TransporterName, @DispatchTo, @FreightType, @Series, @Scratch, @Unit, @PSessId,
                 @DcNo, @Imported, @FPoint, @FPointValue, @OrdStatus, @OrdQty, @OrderDate, @OrderNo,
                 @RemQty, @DP1, @DReason, @DUserId, @DRecTimeStamp, @DocWeight, @DocketNo, @DocketDate,
                 @STNFormNo, @StkRecv, @StkRecvDate, @StkRecvUserId, @InTransit, @UID, @OfferUID, @IsKit,
                 @TotalCorton, @TotalMonoCorton, @SpclOfferId, @VAT, @BuyerAddress, @BuyerTIN,
                 @TotalDiscountAmt, @VDiscountAmt, @VDiscount, @ReceiverID, @ReceiverName, @ReceiverMNo,
                 @ReceiverIDProof, @TotalFPoint, @TotalQty, @CashReward, @CommssnAmt, @RecvAmount, 
                 @ReturnToCustAmt, @RecTimeStamp, @UserId, @UserName, @DelvPlace, @DelvStatus, @DelvUserId,
                 @DelvRecTimeStamp, @Version, @IDType, @BranchName, @CourierId, @CourierName, @LocId,
                 @LocName, @DelvAddress, @Pincode, @OrderType, @Coupon, @CouponAmount, @IRNNo, @AckNo,
                 @AckDate, @QrCodeimage, @QrCode, @SignedInvoice,'G','GST'
                )";

                                TrnBillDatasAffected = await connection.ExecuteAsync(insertBillSql, objDTBillData);
                            }

                            if (TrnBillDatasAffected > 0)
                            {
                                //objcustmerDetail.DeliveryAddress = objModel.objCustomer.Address;
                                string insertCustomerSql = @"
                                INSERT INTO TrnCustomerDetail (CustomerName, MobileNo, ActiveStatus, BillNo, Address1)
                                VALUES (@CustomerName, @MobileNo, @ActiveStatus, @BillNo, @Address1)";
                                var objCustomerDetail = new
                                {
                                    CustomerName = objModel.objCustomer.Name==null?"": objModel.objCustomer.Name,
                                    MobileNo = objModel.objCustomer.MobileNo==null?"0": objModel.objCustomer.MobileNo,
                                    ActiveStatus = "Y",
                                    BillNo = billPrefix + "/" + SoldByCode + "/" + maxSbillNo,
                                    Address1 = objModel.objCustomer.Address==null?"": objModel.objCustomer.Address  
                                };

                                connection.Execute(insertCustomerSql, objCustomerDetail);

                                await DeductPartyWallet(billno_, narration_, soldby_, "", netpayable_, "shoppe");

                                string sqlPayModeMaster = "SELECT * FROM M_PayModeMaster";
                                var resultPayMode = connection.Query<M_PayModeMaster>(sqlPayModeMaster).ToList();
                                // Prepare list of objects to insert
                                foreach (var obj in objDTListPayMode)
                                {
                                    obj.BillType = "G";
                                    obj.PayMode = resultPayMode
                                                    .Where(x => x.Prefix.Trim() == obj.PayPrefix.Trim())
                                                    .Select(x => x.PayMode)
                                                    .FirstOrDefault();

                                    obj.CardNo = string.IsNullOrEmpty(obj.CardNo) ? "" : obj.CardNo;
                                }
                                string insertSql = @"
                INSERT INTO TrnPayModeDetail
            (
                 FSessId,SBillNo,BillNo,SoldBy,BillDate,PayPrefix,PayMode,ChqDDNo, 
                 ChqDDDate,CardNo,BankCode,BankName,Amount,BillAmt,Narration, 
                 ActiveStatus,RecTimeStamp,Version,UserId,UserName,BillType, 
                 DUserId,DRecTimeStamp,AcNo,IFSCode 
            )
            VALUES
            (
                @FSessId,@SBillNo,@BillNo,@SoldBy,@BillDate,@PayPrefix,@PayMode, 
                @ChqDDNo, @ChqDDDate,@CardNo,@BankCode,@BankName,@Amount,@BillAmt, 
                @Narration,@ActiveStatus,@RecTimeStamp, @Version, 
                @UserId, @UserName, @BillType, @DUserId, @DRecTimeStamp, @AcNo,@IFSCode 
            )";

                                int rowsAffected = connection.Execute(insertSql, objDTListPayMode);
                                if (rowsAffected > 0)
                                {
                                    objResponse.ResponseMessage = "Saved Successfully!";
                                    objResponse.ResponseStatus = "OK";
                                    objResponse.ResponseDetailsToPrint = new DistributorBillModel();
                                    objResponse.ResponseDetailsToPrint.BillNo = UserBillNo;
                                    objResponse.ResponseDetailsToPrint.SoldBy = SoldByCode;
                                }
                                else
                                {
                                    objResponse.ResponseMessage = "Failed to save pay mode details.";
                                    objResponse.ResponseStatus = "FAILED";
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            objResponse.ResponseMessage = "Something went wrong!";
                            objResponse.ResponseStatus = "FAILED";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objResponse.ResponseMessage = "Something went wrong!";
                objResponse.ResponseStatus = "FAILED";
            }
            return objResponse;
        }

        private void Make_Inv_json(string UserBillNo)
        {

            //string strres = "";
            //SqlTransaction objTrans = null;
            //string Atk = "";
            //string s = "";
            ////Einvoice eInvoice = new Einvoice();
            //JsonReadVal AthToken;
            //strres = QPostJSON("http://gstsandbox.charteredinfo.com/eivital/dec/v1.04/auth?");
            //AthToken = JsonConvert.DeserializeObject<JsonReadVal>(strres);
            //Atk = AthToken.Data.AuthToken;
            //strres = "";



            //string InvConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["InventoryServices"].ConnectionString;
            //SqlConnection SC1 = new SqlConnection(InvConnectionString);
            //string query = "exec sp_getinvoiceNew @BillNo";
            //DataSet dataSet = new DataSet();

            //using (SqlConnection connection = new SqlConnection(InvConnectionString))
            //{
            //    try
            //    {
            //        connection.Open();

            //        using (SqlCommand command = new SqlCommand(query, connection))
            //        {
            //            // Use a parameterized query
            //            command.Parameters.AddWithValue("@BillNo", UserBillNo);

            //            // Set the command timeout (e.g., 120 seconds)
            //            command.CommandTimeout = 120;

            //            using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
            //            {
            //                dataAdapter.Fill(dataSet);
            //            }
            //        }
            //    }
            //    catch (SqlException ex)
            //    {
            //        Console.WriteLine("SQL Exception: " + ex.Message);
            //    }
            //}
            //DataTable dt = new DataTable();
            //DataTable dt1 = new DataTable();
            //DataTable dt3 = new DataTable();




            //if (dataSet.Tables.Count > 0)
            //{
            //    Einvoice eInvoice = new Einvoice();
            //    eInvoice.TranDtls = new Trandtls();
            //    eInvoice.DocDtls = new DocDtls();
            //    eInvoice.SellerDtls = new Sellerdtls();
            //    eInvoice.BuyerDtls = new Buyerdtls();
            //    eInvoice.DispDtls = new Dispdtls();
            //    eInvoice.ShipDtls = new Shipdtls();
            //    eInvoice.ValDtls = new Valdtls();
            //    eInvoice.RefDtls = new Refdtls();
            //    eInvoice.PayDtls = new Paydtls();
            //    dt = dataSet.Tables[0];
            //    dt1 = dataSet.Tables[1];
            //    if (dataSet.Tables[3].Rows.Count > 0)
            //        dt3 = dataSet.Tables[3];
            //    else
            //        dt3 = dataSet.Tables[4];
            //    eInvoice.Version = "1.1";
            //    eInvoice.TranDtls.TaxSch = "GST";
            //    eInvoice.TranDtls.SupTyp = "B2B";
            //    eInvoice.TranDtls.RegRev = "N";
            //    eInvoice.TranDtls.EcmGstin = null;
            //    eInvoice.TranDtls.IgstOnIntra = "N";
            //    eInvoice.DocDtls.Typ = "INV";
            //    eInvoice.DocDtls.No = UserBillNo;

            //    eInvoice.DocDtls.Dt = dt.Rows[0]["BillDatestr"].ToString().Trim();
            //    eInvoice.SellerDtls.Gstin = "34AACCC1596Q002";//dt1.Rows[0]["TinNo"].ToString();
            //    eInvoice.SellerDtls.LglNm = dt1.Rows[0]["PartyName"].ToString();

            //    eInvoice.SellerDtls.TrdNm = dt1.Rows[0]["PartyName"].ToString();
            //    eInvoice.SellerDtls.Addr1 = dt1.Rows[0]["Address1"].ToString();

            //    eInvoice.SellerDtls.Addr2 = dt1.Rows[0]["Address2"].ToString() != "" ? dt1.Rows[0]["Address2"].ToString() : "   ";
            //    eInvoice.SellerDtls.Loc = dt1.Rows[0]["CityName"].ToString();
            //    eInvoice.SellerDtls.Pin = Convert.ToInt32("605001"); //dt1.Rows[0]["PinCode"].ToString () != ""
            //                                                         // ? Convert.ToInt32(dt1.Rows[0]["PinCode"])
            //                                                         //: 100000;
            //    eInvoice.SellerDtls.Stcd = "34";//dt1.Rows[0]["StateCode"].ToString();
            //                                    // eInvoice.SellerDtls.Ph = dt.Rows[0]["PhoneNo"].ToString() != "" ? dt.Rows[0]["PhoneNo"].ToString() : "999999999999";
            //                                    //eInvoice.SellerDtls.Em = dt.Rows[0]["E_mailAdd"].ToString() != "" ? dt.Rows[0]["E_mailAdd"].ToString() :"abc@gmail.com";
            //    eInvoice.DispDtls.Nm = dt1.Rows[0]["PartyName"].ToString();
            //    eInvoice.DispDtls.Addr1 = dt1.Rows[0]["Address1"].ToString();
            //    eInvoice.DispDtls.Addr2 = dt1.Rows[0]["Address2"].ToString() != "" ? dt1.Rows[0]["Address2"].ToString() : "   ";
            //    eInvoice.DispDtls.Loc = dt1.Rows[0]["CityName"].ToString();
            //    eInvoice.DispDtls.Pin = dt1.Rows[0]["PinCode"].ToString() != ""
            //            ? Convert.ToInt32(dt1.Rows[0]["PinCode"])
            //            : 100000;
            //    eInvoice.DispDtls.Stcd = dt1.Rows[0]["StateCode"].ToString();
            //    List<ItemList> products = new List<ItemList>();
            //    double cgstamt = 0;
            //    double sgstamt = 0;
            //    double AssVal = 0;
            //    double discount = 0;
            //    double netpayable = 0;
            //    double igstamt = 0;

            //    double roundoff = 0;
            //    foreach (DataRow reader in dt.Rows)
            //    {
            //        cgstamt += reader["CGSTAmt"] != DBNull.Value ? Convert.ToDouble(reader["CGSTAmt"]) : 0.0;
            //        sgstamt += reader["SGSTAmt"] != DBNull.Value ? Convert.ToDouble(reader["SGSTAmt"]) : 0.0;
            //        igstamt += reader["TaxAmount"] != DBNull.Value ? Convert.ToDouble(reader["TaxAmount"]) : 0.0;
            //        discount += reader["Discount"] != DBNull.Value ? Convert.ToDouble(reader["Discount"]) : 0.0;
            //        AssVal += reader["NetAmount"] != DBNull.Value ? Convert.ToDouble(reader["NetAmount"]) : 0.0;
            //        netpayable += reader["NetPayable"] != DBNull.Value ? Convert.ToDouble(reader["NetPayable"]) : 0.0;
            //        roundoff += reader["rndoff"] != DBNull.Value ? Convert.ToDouble(reader["rndoff"]) : 0.0;

            //        products.Add(new ItemList
            //        {
            //            SlNo = reader["SNo"]?.ToString() ?? string.Empty,
            //            PrdDesc = reader["ProductName"]?.ToString() ?? string.Empty,
            //            IsServc = "N",
            //            HsnCd = reader["HsnCode"]?.ToString() ?? string.Empty,
            //            Barcde = reader["Barcode"]?.ToString() ?? string.Empty,
            //            Qty = reader["Qty"] != DBNull.Value ? Convert.ToInt32(reader["Qty"]) : 0,
            //            FreeQty = reader["FreeQty"] != DBNull.Value ? Convert.ToInt32(reader["FreeQty"]) : 0,
            //            Unit = "pcs",
            //            UnitPrice = reader["Rate"] != DBNull.Value ? Convert.ToDouble(reader["Rate"]) : 0.0,
            //            TotAmt = reader["NetAmount"] != DBNull.Value ? Convert.ToDouble(reader["NetAmount"]) : 0.0,
            //            Discount = reader["Discount"] != DBNull.Value ? Convert.ToDouble(reader["Discount"]) : 0.0,
            //            PreTaxVal = 0,
            //            AssAmt = (reader["NetAmount"] != DBNull.Value && reader["Discount"] != DBNull.Value)
            // ? Convert.ToDouble(reader["NetAmount"]) - Convert.ToDouble(reader["Discount"])
            // : 0.0,
            //            GstRt = reader["Tax"] != DBNull.Value ? Convert.ToDouble(reader["Tax"]) : 0.0,
            //            IgstAmt = reader["TaxAmount"] != DBNull.Value ? Convert.ToDouble(reader["TaxAmount"]) : 0.0,
            //            CgstAmt = reader["CGSTAmt"] != DBNull.Value ? Convert.ToDouble(reader["CGSTAmt"]) : 0.0,
            //            SgstAmt = reader["SGSTAmt"] != DBNull.Value ? Convert.ToDouble(reader["SGSTAmt"]) : 0.0,
            //            CesRt = 0,
            //            CesAmt = 0,
            //            CesNonAdvlAmt = 0,
            //            StateCesRt = 0,
            //            StateCesAmt = 0,
            //            StateCesNonAdvlAmt = 0,
            //            OthChrg = 0,
            //            TotItemVal = (reader["NetAmount"] != DBNull.Value ? Convert.ToDouble(reader["NetAmount"]) : 0.0) + (reader["TaxAmount"] != DBNull.Value ? Convert.ToDouble(reader["TaxAmount"]) : 0.0)
            //            + (reader["CGSTAmt"] != DBNull.Value ? Convert.ToDouble(reader["CGSTAmt"]) : 0.0) + (reader["SGSTAmt"] != DBNull.Value ? Convert.ToDouble(reader["SGSTAmt"]) : 0.0),
            //            OrdLineRef = string.IsNullOrEmpty(reader["RefNo"]?.ToString()) ? "R" : reader["RefNo"].ToString(),
            //            OrgCntry = "IN",
            //            //PrdSlNo = "",
            //            BchDtls = new Bchdtls
            //            {
            //                Nm = reader["BatchNo"]?.ToString() ?? string.Empty,
            //                ExpDt = reader["ExpDate"]?.ToString() ?? string.Empty,
            //                WrDt = reader["Mfgdate"]?.ToString() ?? string.Empty
            //            }

            //        });
            //    }

            //    eInvoice.ItemList.AddRange(products);
            //    //  dt3 = dataSet.Tables[3];
            //    eInvoice.BuyerDtls.Gstin = "29AWGPV7107B1Z1"; //dt3.Rows[0]["TinNo"]?.ToString() ?? string.Empty;
            //    eInvoice.BuyerDtls.LglNm = dt3.Rows[0]["PartyName"]?.ToString() ?? string.Empty;
            //    eInvoice.BuyerDtls.TrdNm = dt3.Rows[0]["PartyName"]?.ToString() ?? string.Empty;
            //    eInvoice.BuyerDtls.Addr1 = dt3.Rows[0]["Address1"]?.ToString() ?? string.Empty;
            //    eInvoice.BuyerDtls.Addr2 = dt3.Rows[0]["Address2"]?.ToString() != "" ? dt3.Rows[0]["Address2"].ToString() : "   ";
            //    eInvoice.BuyerDtls.Loc = dt3.Rows[0]["CityName"]?.ToString() ?? string.Empty;
            //    eInvoice.BuyerDtls.Pin = Convert.ToInt32(562160);// dt3.Rows[0]["PinCode"].ToString() != ""
            //                                                     //? Convert.ToInt32(dt3.Rows[0]["PinCode"])
            //                                                     //: 100000;
            //    eInvoice.BuyerDtls.Stcd = "29";//dt3.Rows[0]["StateCode"]?.ToString() ?? string.Empty;
            //    eInvoice.BuyerDtls.Pos = "12";//dt3.Rows[0]["StateCode"].ToString();
            //    //  eInvoice.BuyerDtls.Ph = dt.Rows[0]["PhoneNo"]?.ToString() != "" ? dt.Rows[0]["PhoneNo"].ToString() : "999999999999";
            //    //eInvoice.BuyerDtls.Em = dt.Rows[0]["E_mailAdd"]?.ToString() != "" ? dt.Rows[0]["E_mailAdd"].ToString() : "abc@gmail.com";

            //    // Populate Ship Details
            //    eInvoice.ShipDtls.Gstin = "29AWGPV7107B1Z1";//dt3.Rows[0]["TinNo"]?.ToString() ?? string.Empty;
            //    eInvoice.ShipDtls.LglNm = dt3.Rows[0]["PartyName"]?.ToString() ?? string.Empty;
            //    eInvoice.ShipDtls.TrdNm = dt3.Rows[0]["PartyName"]?.ToString() ?? string.Empty;
            //    eInvoice.ShipDtls.Addr1 = dt3.Rows[0]["Address1"]?.ToString() ?? string.Empty;
            //    eInvoice.ShipDtls.Addr2 = dt3.Rows[0]["Address2"]?.ToString() != "" ? dt3.Rows[0]["Address2"].ToString() : "   ";
            //    eInvoice.ShipDtls.Loc = dt3.Rows[0]["CityName"]?.ToString() ?? string.Empty;
            //    eInvoice.ShipDtls.Pin = dt3.Rows[0]["PinCode"].ToString() != ""
            //        ? Convert.ToInt32(dt3.Rows[0]["PinCode"])
            //        : 100000;
            //    eInvoice.ShipDtls.Stcd = dt3.Rows[0]["StateCode"]?.ToString() ?? string.Empty;

            //    //eInvoice.ValDtls.AssVal = Convert.ToDouble(dt.Rows[0]["NetPayable"]);
            //    eInvoice.ValDtls.CgstVal = cgstamt;
            //    eInvoice.ValDtls.SgstVal = sgstamt;
            //    eInvoice.ValDtls.IgstVal = igstamt;
            //    eInvoice.ValDtls.AssVal = AssVal - discount;
            //    eInvoice.ValDtls.TotInvVal = netpayable;
            //    eInvoice.ValDtls.RndOffAmt = roundoff;
            //    eInvoice.ValDtls.Discount = discount;
            //    eInvoice.ValDtls.StCesVal = 0;
            //    eInvoice.ValDtls.TotInvValFc = netpayable;
            //    eInvoice.ValDtls.CesVal = 0;
            //    eInvoice.PayDtls.AccDet = "0";
            //    eInvoice.PayDtls.CrDay = 0;
            //    eInvoice.PayDtls.CrTrn = eInvoice.ShipDtls.LglNm;
            //    eInvoice.PayDtls.DirDr = eInvoice.BuyerDtls.LglNm;
            //    eInvoice.PayDtls.Nm = eInvoice.BuyerDtls.LglNm;

            //    eInvoice.PayDtls.Mode = dataSet.Tables[2].Rows[0]["Paymode"].ToString();
            //    eInvoice.PayDtls.PaymtDue = 0;
            //    eInvoice.PayDtls.PaidAmt = Convert.ToInt32(dataSet.Tables[2].Rows[0]["BillAmt"]);
            //    //  eInvoice.RefDtls.InvRm = "";
            //    // eInvoice.RefDtls.I =  dt.Rows[0]["Billdate"].ToString  ();

            //    strres = JsonConvert.SerializeObject(eInvoice);
            //    if (SC1.State == ConnectionState.Closed)
            //        SC1.Open();

            //    objTrans = SC1.BeginTransaction();
            //    SqlCommand cmd = new SqlCommand();
            //    s = "insert into EnVoiceRequest(UserBilNo,ERequest)Values('" + UserBillNo + "','" + strres + "')";

            //    cmd = new SqlCommand();
            //    cmd.CommandText = s;
            //    cmd.Connection = SC1;
            //    cmd.Transaction = objTrans;



            //    int i = cmd.ExecuteNonQuery();

            //    objTrans.Commit();
            //    SC1.Close();
            //    s = GetQrCode(strres, AthToken.Data.AuthToken, UserBillNo);
            //    IRNClass respon = new IRNClass();
            //    IRNData irndataa = new IRNData();
            //    respon = JsonConvert.DeserializeObject<IRNClass>(s);
            //    if (respon.Status == "1")
            //    {
            //        if (SC1.State == ConnectionState.Closed)
            //            SC1.Open();
            //        irndataa = JsonConvert.DeserializeObject<IRNData>(respon.Data.ToString());




            //        // Decode the Base64 string

            //        byte[] bytes = Convert.FromBase64String(irndataa.QrCodeImage);
            //        // string decodedPayload = Encoding.UTF8.GetString(bytes);
            //        // Image image;
            //        string fileName = "";
            //        using (MemoryStream ms = new MemoryStream(bytes))
            //        {
            //            //Image image = Image.FromStream(ms);
            //            //string serverPath = HttpContext.Current.Server.MapPath("~/images/QrCode");
            //            //if (!Directory.Exists(serverPath))
            //            //{
            //            //    Directory.CreateDirectory(serverPath);
            //            //}
            //            //string FlNm = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            //            //fileName = Path.Combine("'https://vitaflow.cryptpayapi.com/images/QrCode/", FlNm+".png" );
            //            //image.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);
            //            ////     Console.WriteLine("Image saved successfully at: " + fileName);


            //            Image image = Image.FromStream(ms);

            //            // Define the server path to save the image locally
            //            string serverPath = HttpContext.Current.Server.MapPath("~/images/QrCode");

            //            // Ensure the directory exists
            //            if (!Directory.Exists(serverPath))
            //            {
            //                Directory.CreateDirectory(serverPath);
            //            }

            //            // Generate a unique file name with a timestamp
            //            string FlNm = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            //            string localFilePath = Path.Combine(serverPath, FlNm + ".png");

            //            // Save the image locally
            //            image.Save(localFilePath, System.Drawing.Imaging.ImageFormat.Png);

            //            // Construct the URL to access the saved image
            //            fileName = $"https://vitaflow.cryptpayapi.com/images/QrCode/{FlNm}.png";

            //        }


            //        objTrans = SC1.BeginTransaction();
            //        cmd = new SqlCommand();
            //        s = "Update TrnBillmain set AckNo='" + irndataa.AckNo + "',Ackdate='" + irndataa.AckDt + "',IrnNo='" + irndataa.Irn + "',SignedInvoice='" + irndataa.SignedInvoice + "'" +
            //            ", Qrcode='" + irndataa.SignedQRCode + "',QrCodeImage='" + fileName + "' where UserBillNo='" + UserBillNo + "'";

            //        cmd = new SqlCommand();
            //        cmd.CommandText = s;
            //        cmd.Connection = SC1;
            //        cmd.Transaction = objTrans;

            //        i = cmd.ExecuteNonQuery();


            //        objTrans.Commit();
            //        SC1.Close();


            //    }
            //}
        }


        public async Task<int> DeductPartyWallet(string refno, string narration, string drto, string crto, decimal amount, string UserType, string VType)
        {
            int i = 0;
            try
            {
                if (VType == "PV")
                {
                    VType = "P";
                }
                else
                {
                    VType = "B";
                }
                using (var connection = _context.CreateLiveconnInv())
                {
                    var db = ConfigurationManager.AppSetting.GetSection("Database").Value;
                    string query = string.Empty;
                    if (drto == crto)
                    {
                        crto = "";
                    }
                    if (UserType == "shoppe")
                    {
                        query = ";INSERT INTO TrnVoucher(VoucherNo,VoucherDate,DrTo,Crto,Amount,Narration,Refno,VType,BType,AccDocType,SessID,FSessID) " +
                       " Select CASE WHEN Max(VoucherNo) is NULL THEN 1 ELSE Max(VoucherNo)+1 END ,Cast(Convert(varchar,Getdate(),106) as Datetime),'" + drto + "','" + crto + "','" + amount + "','" + narration + "','" + refno + "','" + VType + "','O','Wallet Deducted.',(Select Max(SessID) FROM " + db + "..M_SessnMaster),(Select Max(FSessID) FROM M_FiscalMaster) FROM TrnVoucher ;";
                    }
                    else
                    {
                        query = ";INSERT INTO TrnVoucher(VoucherNo,VoucherDate,DrTo,Crto,Amount,Narration,Refno,VType,BType,AccDocType,SessID,FSessID) " +
                       " Select CASE WHEN Max(VoucherNo) is NULL THEN 1 ELSE Max(VoucherNo)+1 END ,Cast(Convert(varchar,Getdate(),106) as Datetime),'" + drto + "','" + crto + "','" + amount + "','" + narration + "','" + refno + "','" + VType + "','O','Wallet Deducted.',(Select Max(SessID) FROM " + db + "..M_SessnMaster),(Select Max(FSessID) FROM M_FiscalMaster) FROM TrnVoucher ;";
                    }
                    i = await connection.ExecuteAsync(query);
                }
            }
            catch (Exception ex)
            {

            }
            return i;
        }

        public async Task<int> DeductPartyWallet(string refno, string narration, string drto, string crto, decimal amount, string UserType)
        {
            int i = 0;
            try
            {
                string VType = "";
                if (UserType == "shoppe")
                {
                    VType = "R";
                }
                else
                {
                    VType = "M";
                }
                if (drto == crto)
                {
                    crto = "";
                }
                using (var connection = _context.CreateLiveconnInv())
                {
                    var db = ConfigurationManager.AppSetting.GetSection("Database").Value;
                    string query = string.Empty;
                    if (drto == crto)
                    {
                        crto = "";
                    }
                    if (UserType == "shoppe")
                    {
                        query = ";INSERT INTO TrnVoucher(VoucherNo,VoucherDate,DrTo,Crto,Amount,Narration,Refno,VType,BType,AccDocType,SessID,FSessID) " +
                       " Select CASE WHEN Max(VoucherNo) is NULL THEN 1 ELSE Max(VoucherNo)+1 END ,Cast(Convert(varchar,Getdate(),106) as Datetime),'" + drto + "','" + crto + "','" + amount + "','" + narration + "','" + refno + "','" + VType + "','O','Wallet Deducted.',(Select Max(SessID) FROM " + db + "..M_SessnMaster),(Select Max(FSessID) FROM M_FiscalMaster) FROM TrnVoucher ;";
                    }
                    else
                    {
                        query = ";INSERT INTO TrnVoucher(VoucherNo,VoucherDate,DrTo,Crto,Amount,Narration,Refno,VType,BType,AccDocType,SessID,FSessID) " +
                       " Select CASE WHEN Max(VoucherNo) is NULL THEN 1 ELSE Max(VoucherNo)+1 END ,Cast(Convert(varchar,Getdate(),106) as Datetime),'" + drto + "','" + crto + "','" + amount + "','" + narration + "','" + refno + "','" + VType + "','O','Wallet Deducted.',(Select Max(SessID) FROM " + db + "..M_SessnMaster),(Select Max(FSessID) FROM M_FiscalMaster) FROM TrnVoucher ;";
                    }
                    i = await connection.ExecuteAsync(query);
                }
            }
            catch (Exception ex)
            {

            }
            return i;
        }
        public async Task<int> CreditPartyWallet(string refno, string narration, string drto, string crto, decimal amount, string VType)
        {
            int i = 0;
            try
            {

                var db = ConfigurationManager.AppSetting.GetSection("Database").Value;
                string query = string.Empty;
                using (var connection = _context.CreateLiveconnInv())
                {
                    query = ";INSERT INTO TrnVoucher(VoucherNo,VoucherDate,DrTo,Crto,Amount,Narration,Refno,VType,BType,AccDocType,SessID,FSessID) " +
                      " Select CASE WHEN Max(VoucherNo) is NULL THEN 1 ELSE Max(VoucherNo)+1 END ,Cast(Convert(varchar,Getdate(),106) as Datetime),'" + drto + "','" + crto + "','" + amount + "','" + narration + "','" + refno + "','" + VType + "','O','Wallet credit.',(Select Max(SessID) FROM " + db + "..M_SessnMaster),(Select Max(FSessID) FROM M_FiscalMaster) FROM TrnVoucher ;";
                    i = await connection.ExecuteAsync(query);
                }
            }
            catch (Exception ex)
            {

            }
            return i;
        }
        public async Task<FPVoucher> CheckFpVoucher(string Code, string Idno)
        {
            FPVoucher fPVoucher = new FPVoucher();
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    var query = @"select Id,Code,IdNo,Isuse,Amount from FPVoucher where  Code=@Code and Idno=@Idno";
                    fPVoucher = await connection.QueryFirstOrDefaultAsync<FPVoucher>(query, new
                    {
                        Code = Code,
                        Idno = @Idno
                    });
                }
            }
            catch (Exception ex)
            {

            }
            return fPVoucher;
        }
        public async Task<Coupon> CheckCoupon(string Code, string Idno)
        {
            Coupon obj = new Coupon();
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    var query = @"select Id,Code,IdNo,Isuse,Amount from Coupon where  Code=@Code and Idno=@Idno";
                    obj = await connection.QueryFirstOrDefaultAsync<Coupon>(query, new
                    {
                        Code = Code,
                        Idno = @Idno
                    });
                }
            }
            catch (Exception ex)
            {

            }
            return obj;
        }
        public async Task<ResponseDetail> SaveDispatchOrder(PartyOrderModel objPartyDispatchOrder)
        {
            ResponseDetail objResponse = new ResponseDetail();
            TrnPartyOrderDetail objDTPartyOrderDetail = new TrnPartyOrderDetail();
            TrnPartyOrderMain objDtPartyOrderMain = new TrnPartyOrderMain();
            decimal maxUserSBillNo = 0;
            decimal? SessId = 0;
            string billPrefix = "";
            decimal maxSbillNo = 0;
            decimal? FsessId = 0;
            string UserBillNo = "";
            string version = "";
            objResponse.ResponseMessage = "Something went wrong!";
            objResponse.ResponseStatus = "FAILED";
            int i = 0;
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    var db = ConfigurationManager.AppSetting.GetSection("Database").Value;
                    var sql = "Select Max(SessID) as MaxSessId from " + db + "..M_SessnMaster";
                    SessId = (await connection.QueryAsync<decimal>(sql, commandType: CommandType.Text)).FirstOrDefault();

                    string maxSBillNosql = @"
                                          SELECT ISNULL(MAX(SBillNo), 0)
                                          FROM TrnBillMain";

                    maxSbillNo = await connection.QuerySingleOrDefaultAsync<int>(maxSBillNosql);
                    maxSbillNo = maxSbillNo + 1;

                    var FsessIdsql = "select  FSessId from M_FiscalMaster where ActiveStatus='Y'";
                    FsessId = (await connection.QueryAsync<decimal>(FsessIdsql, commandType: CommandType.Text)).FirstOrDefault();

                    var billPrefixsql = "select BillPrefix from M_ConfigMaster";
                    billPrefix = (await connection.QueryAsync<string>(billPrefixsql, commandType: CommandType.Text)).FirstOrDefault();

                    string sqlQuery = @"
                                       SELECT TOP 1 UserPartyCode 
                                       FROM M_LedgerMaster 
                                       WHERE ActiveStatus = 'Y' 
                                       AND PartyCode = @PartyCode";
                    var userPCode = await connection.QueryFirstOrDefaultAsync<string>(
                                                sqlQuery,
                                              new { @PartyCode = objPartyDispatchOrder.LoginUser.PartyCode }
                                          );

                    var maxUserSBillNoSql = @"
                                              SELECT ISNULL(MAX(UserSBillNo),0)
                                              FROM TrnBillMain
                                              WHERE FSessId = @FsessId AND SoldBy = @PartyCode AND BillType != 'S'";
                    var parameters = new
                    {
                        @FsessId = FsessId,
                        @PartyCode = objPartyDispatchOrder.LoginUser.PartyCode
                    };
                    maxUserSBillNo = await connection.QuerySingleAsync<int>(maxUserSBillNoSql, parameters);
                    maxUserSBillNo = maxUserSBillNo + 1;

                    UserBillNo = billPrefix + "/" + userPCode + "/" + maxUserSBillNo;

                    var versionsql = "select VersionNo from M_NewHOVersionInfo";
                    version = (await connection.QueryAsync<string>(versionsql, commandType: CommandType.Text)).FirstOrDefault();

                    DateTime BillDate = DateTime.Now.Date;

                    string SoldByCode = "";
                    List<TrnBillData> tempTableList = new List<TrnBillData>();
                    string GroupPrefix = "";
                    string BillingPartyCode = objPartyDispatchOrder.PartyCode;
                    var groupId = connection.QuerySingleOrDefaultAsync<int?>(
                               @"SELECT GroupId FROM M_LedgerMaster WHERE PartyCode = @BillingPartyCode",
                               new { BillingPartyCode });
                    if (groupId != null)
                    {
                        GroupPrefix = connection.QuerySingleOrDefault<string>(
                           @"SELECT Prefix FROM M_GroupMaster WHERE GroupId = @GroupId",
                           new { GroupId = groupId });
                    }

                    List<ProductModel> objListProductModel = new List<ProductModel>();

                    foreach (var obj in objPartyDispatchOrder.objListProduct)
                    {

                        TrnBillData objDTBillData = new TrnBillData();
                        if (obj.Quantity > 0)
                        {
                            objListProductModel.Add(obj);
                            objDTBillData.SBillNo = maxSbillNo;
                            objDTBillData.FSessId = FsessId ?? 0;
                            objDTBillData.SessId = SessId ?? 0;
                            objDTBillData.ActiveStatus = "Y";
                            objDTBillData.BillDate = BillDate.Date;

                            objDTBillData.RefNo = "";
                            objDTBillData.RefId = 0;
                            objDTBillData.RefName = "";
                            objDTBillData.Remarks = "";
                            objDTBillData.CType = GroupPrefix;
                            objDTBillData.SoldBy = objPartyDispatchOrder.LoginUser.PartyCode;
                            SoldByCode = objDTBillData.SoldBy;
                            objDTBillData.BillBy = objDTBillData.SoldBy;
                            objDTBillData.BillNo = billPrefix + "/" + objDTBillData.BillBy + "/" + maxSbillNo;
                            objDTBillData.FType = GroupPrefix;
                            objDTBillData.FCode = objPartyDispatchOrder.PartyCode;
                            objDTBillData.PartyName = objPartyDispatchOrder.PartyName;
                            objDTBillData.SupplierId = 0;
                            objDTBillData.ChDDNo = 0;
                            objDTBillData.ChDate = DateTime.Now;
                            objDTBillData.ChAmt = 0;
                            objDTBillData.BankCode = 0;
                            objDTBillData.BankName = "";
                            objDTBillData.FormNo = 0;
                            objDTBillData.TotalTaxAmount = objPartyDispatchOrder.objProduct.TotalTaxAmount;
                            objDTBillData.TotalSTaxAmount = 0;
                            //objDTBillData.TotalDiscount = objPartyDispatchOrder.objProduct.TotalDiscount;
                            objDTBillData.TotalKitBvValue = 0;
                            objDTBillData.TotalBvValue = objPartyDispatchOrder.objProduct.TotalBV;
                            objDTBillData.TotalCVValue = objPartyDispatchOrder.objProduct.TotalCV;
                            objDTBillData.TotalPVValue = objPartyDispatchOrder.objProduct.TotalPV;
                            objDTBillData.TotalRPValue = objPartyDispatchOrder.objProduct.TotalRP;

                            objDTBillData.DP = obj.DP ?? 0;
                            objDTBillData.RP = obj.RP ?? 0;
                            objDTBillData.MRP = obj.MRP ?? 0;
                            objDTBillData.CVValue = obj.CVValue ?? 0;
                            objDTBillData.CV = obj.CV ?? 0;
                            objDTBillData.PV = obj.PV ?? 0;
                            objDTBillData.BV = obj.BV ?? 0;
                            objDTBillData.BVValue = obj.BVValue ?? 0;
                            objDTBillData.PVValue = obj.PVValue ?? 0;
                            objDTBillData.RPValue = obj.RPValue ?? 0;
                            objDTBillData.Barcode = obj.Barcode.ToString();
                            objDTBillData.BatchNo = obj.BatchNo.ToString();

                            objDTBillData.DiscountPer = obj.DiscPer ?? 0;
                            objDTBillData.Discount = obj.DiscAmt ?? 0;
                            objDTBillData.ProdCommssn = obj.CommissionPer ?? 0;
                            objDTBillData.ProdCommssnAmt = obj.CommissionAmt ?? 0;
                            objDTBillData.ProductId = obj.ProdCode.ToString();
                            objDTBillData.ProductName = obj.ProductName;
                            objDTBillData.Qty = obj.Quantity;
                            objDTBillData.Rate = obj.Rate ?? 0;
                            objDTBillData.IsKitBV = "N";
                            objDTBillData.DSeries = "";
                            objDTBillData.DImported = "N";
                            objDTBillData.IMEINo = "D";
                            objDTBillData.BNo = "";
                            objDTBillData.ItemType = "N";
                            objDTBillData.JType = "Cash:" + objPartyDispatchOrder.objProduct.TotalNetPayable;
                            objDTBillData.BillTo = objPartyDispatchOrder.PartyCode;
                            objDTBillData.BillFor = objPartyDispatchOrder.PartyCode;
                            objDTBillData.IsReceive = "N";
                            objDTBillData.IsCredit = "F";
                            //objDTBillData.BillType = "R";
                            objDTBillData.BillType = "V";
                            objDTBillData.ProdType = obj.ProductType;
                            objDTBillData.PaymentDtl = "Cash:" + objPartyDispatchOrder.objProduct.TotalNetPayable;

                            objDTBillData.TotalAmount = objPartyDispatchOrder.objProduct.TotalTotalAmount;
                            //tax excluding
                            objDTBillData.NetAmount = obj.Amount;
                            // var PartyStateCode = (from r in entity.M_LedgerMaster where r.PartyCode == objPartyDispatchOrder.PartyCode select r.StateCode).FirstOrDefault();
                            // Define the SQL query
                            var query = "SELECT StateCode FROM M_LedgerMaster WHERE PartyCode = @PartyCode";

                            // Execute the query and get the PartyStateCode
                            var PartyStateCode = await connection.QuerySingleOrDefaultAsync<int>(
                                query,
                                new { PartyCode = objPartyDispatchOrder.PartyCode }
                            );
                            if (PartyStateCode == objPartyDispatchOrder.LoginUser.StateCode)
                            {
                                objDTBillData.TaxAmount = 0;
                                objDTBillData.Tax = 0;
                                objDTBillData.CGST = obj.TaxPer / 2 ?? 0;
                                objDTBillData.CGSTAmt = obj.TaxAmt / 2 ?? 0;
                                objDTBillData.SGST = obj.TaxPer / 2 ?? 0;
                                objDTBillData.SGSTAmt = obj.TaxAmt / 2 ?? 0;
                                objDTBillData.TaxType = "S";
                            }
                            else
                            {

                                objDTBillData.TaxAmount = obj.TaxAmt ?? 0;
                                if (obj.OldTaxAmount != 0 && obj.OldTaxAmount != obj.TaxAmt)
                                {
                                    objDTBillData.TaxAmount = Decimal.Parse((Convert.ToDouble(objDTBillData.TaxAmount) + 0.01).ToString());
                                    objDTBillData.NetAmount = Decimal.Parse((Convert.ToDouble(objDTBillData.NetAmount) - 0.01).ToString());
                                }
                                objDTBillData.Tax = obj.TaxPer ?? 0;
                                objDTBillData.CGST = 0;
                                objDTBillData.CGSTAmt = 0;
                                objDTBillData.SGST = 0;
                                objDTBillData.SGSTAmt = 0;
                                objDTBillData.TaxType = "I";
                            }
                            objDTBillData.CashDiscPer = obj.CashDiscPer;
                            objDTBillData.CashDiscAmount = obj.CashDiscAmount;

                            objDTBillData.NetPayable = objPartyDispatchOrder.objProduct.TotalNetPayable;
                            objDTBillData.RndOff = objPartyDispatchOrder.objProduct.Roundoff;
                            objDTBillData.CardAmount = 0;
                            objDTBillData.PayMode = "Cash";
                            objDTBillData.PayPrefix = "";
                            objDTBillData.BvTransfer = "N";

                            //objDTBillData.UserSBillNo = maxSbillNo;
                            //objDTBillData.UserBillNo = billPrefix + "/" + objDTBillData.BillBy + "/" + maxSbillNo;
                            objDTBillData.UserSBillNo = maxUserSBillNo;
                            objDTBillData.UserBillNo = UserBillNo;
                            objDTBillData.DispatchStatus = "N";
                            objDTBillData.LR = "0";
                            objDTBillData.LRDate = DateTime.Now;
                            objDTBillData.TransporterName = "";
                            objDTBillData.DispatchTo = "";
                            objDTBillData.FreightType = "";
                            objDTBillData.Series = "";
                            objDTBillData.Scratch = "";

                            objDTBillData.Unit = 0;

                            objDTBillData.PSessId = 0;
                            objDTBillData.DcNo = "";
                            objDTBillData.Imported = "N";
                            objDTBillData.FPoint = 0;
                            objDTBillData.FPointValue = 0;
                            objDTBillData.OrdStatus = "";
                            objDTBillData.OrdQty = 0;
                            // objDTBillData.OrderType = "";
                            objDTBillData.OrderDate = DateTime.Now;
                            objDTBillData.OrderNo = objPartyDispatchOrder.OrderNo;
                            objDTBillData.RemQty = 0;
                            objDTBillData.DP1 = 0;
                            objDTBillData.DReason = "";
                            objDTBillData.DUserId = 0;
                            objDTBillData.DRecTimeStamp = DateTime.Now;
                            objDTBillData.DocWeight = 0;
                            objDTBillData.DocketNo = "";
                            objDTBillData.DocketDate = DateTime.Now;
                            //objDTBillData.UserBillNo = "";
                            //objDTBillData.UserSBillNo = 0;
                            objDTBillData.STNFormNo = "";
                            objDTBillData.StkRecv = "N";
                            objDTBillData.StkRecvDate = DateTime.Now;
                            objDTBillData.StkRecvUserId = 0;
                            objDTBillData.InTransit = "N";
                            objDTBillData.UID = string.IsNullOrEmpty(objPartyDispatchOrder.objProduct.UID) ? "" : objPartyDispatchOrder.objProduct.UID;
                            objDTBillData.OfferUID = obj.OfferUID;
                            objDTBillData.IsKit = "N";
                            objDTBillData.TotalCorton = "";
                            objDTBillData.TotalMonoCorton = "";
                            objDTBillData.SpclOfferId = Convert.ToInt32(obj.OfferUID);
                            objDTBillData.VAT = 0;
                            objDTBillData.BuyerAddress = "";
                            objDTBillData.BuyerTIN = "";

                            objDTBillData.TotalDiscount = objPartyDispatchOrder.objProduct.TotalDiscPer;
                            objDTBillData.TotalDiscountAmt = objPartyDispatchOrder.objProduct.TotalDiscount;
                            objDTBillData.VDiscountAmt = 0;
                            objDTBillData.VDiscount = 0;
                            objDTBillData.ReceiverID = "";
                            objDTBillData.ReceiverName = "";
                            objDTBillData.ReceiverMNo = "";
                            objDTBillData.ReceiverIDProof = "";
                            objDTBillData.TotalFPoint = 0;
                            objDTBillData.TotalQty = objPartyDispatchOrder.objProduct.TotalQty;
                            objDTBillData.CashReward = 0;
                            objDTBillData.CommssnAmt = objPartyDispatchOrder.objProduct.TotalCommsonAmt;
                            objDTBillData.RecvAmount = 0;
                            objDTBillData.ReturnToCustAmt = 0;
                            objDTBillData.ActiveStatus = "Y";
                            objDTBillData.RecTimeStamp = DateTime.Now;
                            objDTBillData.UserId = objPartyDispatchOrder.LoginUser.UserId;
                            objDTBillData.UserName = objPartyDispatchOrder.LoginUser.UserName;
                            objDTBillData.DelvPlace = string.IsNullOrEmpty(objPartyDispatchOrder.objProduct.DeliveryPlace) ? "" : objPartyDispatchOrder.objProduct.DeliveryPlace;
                            objDTBillData.DelvStatus = "";
                            objDTBillData.DelvUserId = 0;
                            objDTBillData.DelvRecTimeStamp = DateTime.Now;
                            objDTBillData.Version = version;
                            objDTBillData.IDType = "";
                            objDTBillData.BranchName = "";
                            objDTBillData.CourierId = 0;
                            objDTBillData.CourierName = "";
                            objDTBillData.LocId = 0;
                            objDTBillData.LocName = "";
                            objDTBillData.DelvAddress = "";
                            objDTBillData.Pincode = "";
                            objDTBillData.OrderType = "S";
                            var TrnPartyOrder = @"insert into TrnBillData(
                                   FSessId,SessId,SBillNo,BillNo,RefNo,BillDate,CType,SoldBy,BillBy,FType,FCode,PartyName,SupplierId,
                                   ChDDNo,ChDate,ChAmt,BankCode,BankName,FormNo,TotalTaxAmount,TotalSTaxAmount,TotalDiscount,
                                   TotalKitBvValue,TotalBvValue,TotalCVValue,TotalPVValue,TotalRPValue,CashDiscPer,
                                   CashDiscAmount,NetPayable,TotalAmount,RndOff,CardAmount,PayMode,PayPrefix,
                                   BvTransfer,Remarks,DispatchStatus,LR,LRDate,TransporterName,DispatchTo,
                                   FreightType,FreightAmt,Series,Scratch,RefId,RefName,JType,Unit,BillTo,
                                   PSessId,BillFor,DcNo,Imported,IsReceive,IsCredit,BillType,
                                   TotalDiscountAmt,VDiscountAmt,ReceiverID,ReceiverName,
                                   ReceiverMNo,ReceiverIDProof,TotalFPoint,TotalQty,
                                   CommssnAmt,CashReward,RecvAmount,ReturnToCustAmt,
                                   ActiveStatus,RecTimeStamp,UserId,UserName,Version,DelvPlace,PaymentDtl,IDType,
                                   BranchName,LocId,LocName,Pincode,CourierId,CourierName,ProductId,ProductName,
                                   BatchNo,Barcode,Qty,MRP,DP,Rate,BV,BVValue,CV,CVValue,PV,PVValue,RP,RPValue,
                                   IsKitBV,TaxType,Tax,TaxAmount,DiscountPer,Discount,NetAmount,DSeries,DImported,
                                   IMEINo,BNo,ItemType,VDiscount,VDiscountValue,FPoint,FPointValue,ProdCommssn,
                                   ProdCommssnAmt,OrdStatus,OrdQty,RemQty,DP1,DReason,DUserId,DRecTimeStamp,
                                   DocWeight,DocketNo,DOD,DelvAddress,OrderNo,OrderDate,DocketDate,DelvStatus,
                                   DelvUserId,DelvRecTimeStamp,OrderType,UserBillNo,UserSBillNo,STNFormNo,
                                   StkRecv,StkRecvDate,StkRecvUserId,InTransit,UID,OfferUID,IsKit,ProdType,
                                   TotalCorton,TotalMonoCorton,SpclOfferId,VAT,BuyerAddress,BuyerTIN,CGST,
                                   CGSTAmt,SGST,SGSTAmt,FreeQty,TFreeQty,BillGSTType)
                                   values(@FSessId,@SessId,@SBillNo,@BillNo,@RefNo,@BillDate,@CType,@SoldBy,@BillBy,@FType,@FCode,@PartyName,@SupplierId,
                                   @ChDDNo,@ChDate,@ChAmt,@BankCode,@BankName,@FormNo,@TotalTaxAmount,@TotalSTaxAmount,@TotalDiscount,
                                   @TotalKitBvValue,@TotalBvValue,@TotalCVValue,@TotalPVValue,@TotalRPValue,@CashDiscPer,
                                   @CashDiscAmount,@NetPayable,@TotalAmount,@RndOff,@CardAmount,@PayMode,@PayPrefix,
                                   @BvTransfer,@Remarks,@DispatchStatus,@LR,@LRDate,@TransporterName,@DispatchTo,
                                   @FreightType,@FreightAmt,@Series,@Scratch,@RefId,@RefName,@JType,@Unit,@BillTo,
                                   @PSessId,@BillFor,@DcNo,@Imported,@IsReceive,@IsCredit,@BillType,
                                   @TotalDiscountAmt,@VDiscountAmt,@ReceiverID,@ReceiverName,
                                   @ReceiverMNo,@ReceiverIDProof,@TotalFPoint,@TotalQty,
                                   @CommssnAmt,@CashReward,@RecvAmount,@ReturnToCustAmt,
                                   @ActiveStatus,@RecTimeStamp,@UserId,@UserName,@Version,@DelvPlace,@PaymentDtl,@IDType,
                                   @BranchName,@LocId,@LocName,@Pincode,@CourierId,@CourierName,@ProductId,@ProductName,
                                   @BatchNo,@Barcode,@Qty,@MRP,@DP,@Rate,@BV,@BVValue,@CV,@CVValue,@PV,@PVValue,@RP,@RPValue,
                                   @IsKitBV,@TaxType,@Tax,@TaxAmount,@DiscountPer,@Discount,@NetAmount,@DSeries,@DImported,
                                   @IMEINo,@BNo,@ItemType,@VDiscount,@VDiscountValue,@FPoint,@FPointValue,@ProdCommssn,
                                   @ProdCommssnAmt,@OrdStatus,@OrdQty,@RemQty,@DP1,@DReason,@DUserId,@DRecTimeStamp,
                                   @DocWeight,@DocketNo,@DOD,@DelvAddress,@OrderNo,@OrderDate,@DocketDate,@DelvStatus,
                                   @DelvUserId,@DelvRecTimeStamp,@OrderType,@UserBillNo,@UserSBillNo,@STNFormNo,
                                   @StkRecv,@StkRecvDate,@StkRecvUserId,@InTransit,@UID,@OfferUID,@IsKit,@ProdType,
                                   @TotalCorton,@TotalMonoCorton,@SpclOfferId,@VAT,@BuyerAddress,@BuyerTIN,@CGST,
                                   @CGSTAmt,@SGST,@SGSTAmt,@FreeQty,@TFreeQty,'G')";
                            i = await connection.ExecuteAsync(TrnPartyOrder, objDTBillData);

                            //entity.TrnBillDatas.Add(objDTBillData);

                            //updating entries in trnpartyorderdetails
                            //objDTPartyOrderDetail = (from r in entity.TrnPartyOrderDetails
                            //                         where r.ProductCode == objDTBillData.ProductId && r.OrderNo == objPartyDispatchOrder.OrderNo
                            //                         select r
                            //                       ).FirstOrDefault();
                            //if (objDTPartyOrderDetail != null)
                            //{

                            //    objDTPartyOrderDetail.Status = "D";

                            //    objDTPartyOrderDetail.RemQty = objDTPartyOrderDetail.Qty - obj.Quantity;
                            //    objDTPartyOrderDetail.DispatchQty = obj.Quantity;

                            //}
                            //objDtPartyOrderMain = (from r in entity.TrnPartyOrderMains
                            //                       where r.OrderNo == objPartyDispatchOrder.OrderNo
                            //                       select r
                            //                       ).FirstOrDefault();
                            //if (objDtPartyOrderMain != null)
                            //{
                            //    objDtPartyOrderMain.Status = "D";
                            //    objDtPartyOrderMain.BillNo = UserBillNo;
                            //    objDtPartyOrderMain.BillDate = DateTime.Now.Date;

                            //}
                        }
                    }

                    if (i > 0)
                    {
                        var query = "Exec SaveFranchiseLimit @PartyCode, @FsessId, @SessId, @OrderNo";
                        var parametersFranchiseLimit = new
                        {
                            PartyCode = objPartyDispatchOrder.PartyCode,
                            FsessId = FsessId,
                            SessId = SessId,
                            OrderNo = objPartyDispatchOrder.OrderNo
                        };

                        // Execute the query and return the number of affected rows
                        int rowsAffected = connection.Execute(query, parametersFranchiseLimit);

                        string Sql = "Update TrnPartyOrderDetail Set DispatchQty=a.DispQty,DispatchAmt=a.DispAmt,Discount=a.DiscountAmt";
                        Sql = Sql + " FROM (";
                        Sql = Sql + " Select a.FSessId,a.OrderNo,b.ProductId,b.ProdType,b.OfferUId,IsNull(SUM(b.Discount),0) as DiscountAmt,IsNull(SUM(b.Qty),0) as DispQty,IsNull(SUM(b.TaxAmount),0)+IsNull(SUM(b.NetAmount),0) as DispAmt";
                        Sql = Sql + " FROM TrnBillMain as a,TrnBillDetails as b Where a.FSessId=b.FSessId And a.BillNo=b.BillNo And a.OrderNo='" + objPartyDispatchOrder.OrderNo + "'";
                        Sql = Sql + " Group BY a.FSessId,a.OrderNo,b.ProductId,b.ProdType,b.OfferUId) as a,TrnPartyOrderDetail as b";
                        Sql = Sql + " Where a.OrderNo=b.OrderNo And a.ProductId=b.ProductCode And a.ProdType=b.ProdType AND a.OfferUId=b.OfferUId";
                        Sql = Sql + " ;Update TrnPartyOrderDetail Set RemQty=Qty-DispatchQty Where OrderNo='" + objPartyDispatchOrder.OrderNo + "' AND ActiveStatus='Y'";
                        Sql = Sql + " ;Update TrnPartyOrderDetail Set Status=Case When RemQty<=0 Then 'C' Else 'P' End Where OrderNo='" + objPartyDispatchOrder.OrderNo + "' AND ActiveStatus='Y'";
                        Sql = Sql + " ;Update TrnPartyOrderMain Set TotalDiscount=a.TotalDiscount,TotalDispQty=a.TotalDispQty,TotalAmount=a.TotalAmount,";
                        Sql = Sql + " TotalTaxAmt=a.TotalTaxAmt,RndOff=Round(a.TotalAmount+a.TotalTaxAmt,0)-Round(a.TotalAmount+a.TotalTaxAmt,2),NetPayable=Round(a.NetPayable,0)";
                        Sql = Sql + " FROM (";
                        Sql = Sql + " Select FSessId,OrderNo,IsNull(SUM(Discount),0) as TotalDiscount,IsNull(SUM(TotalQty),0) as TotalDispQty,";
                        Sql = Sql + " IsNull(SUM(Amount),0) as TotalAmount,";
                        Sql = Sql + " IsNull(SUM(TaxAmount),0)+IsNull(SUM(STaxAmount),0) as TotalTaxAmt,";
                        Sql = Sql + " IsNull(SUM(NetPayable),0) as NetPayable";
                        Sql = Sql + " FROM TrnBillMain Where OrderType='S'";
                        Sql = Sql + " Group By FSessId,OrderNo) as a,TrnPartyOrderMain as b ";
                        Sql = Sql + " Where a.OrderNo=b.OrderNo And b.OrderNo='" + objPartyDispatchOrder.OrderNo + "'";
                        Sql = Sql + " ;Update TrnPartyOrderMain Set TotalRemQty=TotalOrdQty-TotalDispQty Where OrderNo='" + objPartyDispatchOrder.OrderNo + "' AND ActiveStatus='Y'";
                        Sql = Sql + " ;Update TrnPartyOrderMain Set Status=Case When TotalRemQty<=0 Then 'C' Else 'P' End Where OrderNo='" + objPartyDispatchOrder.OrderNo + "' AND ActiveStatus='Y'";
                        Sql = Sql + ";update TrnBillMain set OrderMethod='" + objPartyDispatchOrder.OrderMethod + "' where OrderNo='" + objPartyDispatchOrder.OrderNo + "'";
                        connection.Execute(Sql);
                        objResponse.ResponseMessage = "Saved Successfully!";
                        objResponse.ResponseStatus = "OK";
                    }
                    else
                    {
                        objResponse.ResponseMessage = "Something went wrong!";
                        objResponse.ResponseStatus = "FAILED";
                    }

                }

            }
            catch (Exception ex)
            {
                objResponse.ResponseMessage = "Something went wrong!";
                objResponse.ResponseStatus = "FAILED";
            }
            return objResponse;
        }
        public async Task<DistributorBillModel> getInvoice(string BillNo, string CurrentPartyCode, string id)
        {
            DistributorBillModel objDistributorModel = new DistributorBillModel();
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    decimal? TotCGSTTaxPer = 0;
                    decimal? TotCGSTTaxAmt = 0;
                    decimal? TotSGSTTaxPer = 0;
                    decimal? TotSGSTTaxAmt = 0;
                    decimal? TotalTaxAmt = 0;
                    decimal? TotalTaxPer = 0;
                    objDistributorModel.objListProduct = new List<ProductModel>();

                    string query = @"
                                    SELECT 
                                        M.PackSize AS Size,
                                        r.FCode AS IdNo,
                                        r.ReceiverMNo AS Mobileno,
                                        r.PartyName AS PartyName,
                                        t.ProductId AS ProductCodeStr,
                                        t.ProductName AS ProductName,
                                        t.Barcode AS Barcode,
                                        t.BatchNo AS BatchNo,
                                        t.PVValue AS PVValue,
                                        t.Rate AS Rate,
                                        t.MRP AS MRP,
                                        CASE WHEN t.ProdType = 'F' THEN t.DP + (t.Discount / t.Qty) ELSE t.DP END AS DP,
                                        t.Qty AS Quantity,
                                        t.FreeQty AS FreeQty,
                                        t.NetAmount AS Amount,
                                        t.BvValue AS BVValue,
                                        t.CGST AS CGST,
                                        r.RndOff AS Roundoff,
                                        t.SGST AS SGST,
                                        t.CGSTAmt AS CGSTAmount,
                                        t.SGSTAmt AS SGSTAmount,
                                        r.NetPayable AS TotalNetPayable,
                                        r.BillDate AS BillDate,
                                        t.TaxAmount AS TaxAmt,
                                        t.Tax AS TaxPer,
                                        r.SoldBy AS BillSoldBy,
                                        r.OrderType AS OrderType,
                                        r.BvValue AS TotalBV,
                                        r.TotalQty AS TotalQty,
                                        t.TaxType AS TaxType,
                                        t.ProdType AS ProductType,
                                        M.HSNCode AS HSNCode,
                                        r.BillType AS billType,
                                        r.DcNo AS VoucherNo,
                                        r.CashReward AS VoucherAmt,
                                        r.VDiscountAmt + r.DiscountAmt AS VDiscountAmt,
                                        r.Remarks AS Remarks,
                                        t.OfferUID AS OfferUID,
                                        r.FType AS Ftype,
                                        r.UserName AS UserName,
                                        r.DocWeight AS TotalWeight,
                                        r.OrderNo AS OrderNo,
                                        t.Discount as DiscAmt,
                                        M.UOM,
                                        r.UserBillNo
                                        FROM TrnBillMain r
                                        INNER JOIN TrnBillDetails t ON r.BillNo = t.BillNo
                                        INNER JOIN M_ProductMaster M ON t.ProductId = M.ProdId
                                        WHERE r.BillNo = @BillNo 
                                        AND (@Id = 'F' OR r.FCode = @Id);
                                        ";

                    var parameters = new { BillNo = BillNo, Id = id };
                    objDistributorModel.objListProduct = (await connection.QueryAsync<ProductModel>(query, parameters)).ToList();
                    if (objDistributorModel.objListProduct.Count > 0)
                    {
                        string sqlpaymode = @"
        SELECT 
            r.PayMode,
            r.CardNo,
            r.BillAmt,
            r.ChqDDNo,
            r.ChqDDDate,
            r.Amount
        FROM TrnPayModeDetail r
        INNER JOIN TrnBillMain b ON r.BillNo = b.BillNo
        WHERE b.BillNo = @BillNo and r.ActiveStatus='Y'";

                        var resultpaymode = connection.Query<PaymentModeDetail>(sqlpaymode, new { BillNo }).ToList();
                        objDistributorModel.objPaymentMode = resultpaymode;
                        objDistributorModel.UserBillNo = objDistributorModel.objListProduct[0].UserBillNo;
                        string PartyCode = objDistributorModel.objListProduct[0].IdNo;
                        if (objDistributorModel.objListProduct[0].billType == "S")
                        {
                            string queryUserMaster = @"
                                             SELECT TOP 1 UserName GetCustInfo

                                             FROM Inv_M_UserMaster 
                                             WHERE BranchCode = @PartyCode;
                                             ";

                            var parametersUserMaster = new { PartyCode = PartyCode };
                            objDistributorModel.Username = (await connection.QueryFirstOrDefaultAsync<string>(queryUserMaster, parametersUserMaster));
                        }
                        foreach (var obj in objDistributorModel.objListProduct)
                        {
                            obj.DP = Math.Round(obj.DP ?? 0, 2);
                            //get expriry batch expirydate
                            // Fetch expiry date using Dapper
                            string queryExpDate = @"
                                                       SELECT ExpDate 
                                                       FROM M_BatchMaster 
                                                       WHERE BatchNo = @BatchNo;
                                                      ";
                            var parametersExpDate = new { BatchNo = obj.BatchNo };

                            var expdate = (await connection.QueryFirstOrDefaultAsync<DateTime?>(queryExpDate, parametersExpDate));
                            obj.ExpDate = (DateTime)expdate;
                        }
                        objDistributorModel.IsSequneceproduct = 0;
                        var offerID = objDistributorModel.objListProduct[0].OfferUID;

                        string queryOffer = @"
                                                            SELECT OfferID, StartProduct, OfferName 
                                                            FROM M_OtherOffers 
                                                            WHERE OfferID = @OfferID;
                                                            ";

                        var parametersOffer = new { OfferID = offerID };

                        var offer = (await connection.QueryFirstOrDefaultAsync<M_OtherOffers>(queryOffer, parametersOffer));

                        if (offer != null)
                        {
                            objDistributorModel.IsSequneceproduct = offer.StartProduct;
                            objDistributorModel.OfferName = offer.OfferName;
                        }
                        else
                        {
                            string queryOffer1 = @"
                                                      SELECT OfferName 
                                                       FROM M_Offers 
                                                       WHERE AID = @OfferID;
                                                       ";

                            var parametersqueryOffer1 = new { OfferID = offerID };

                            string offerName = (await connection.QueryFirstOrDefaultAsync<string>(queryOffer1, parametersqueryOffer1));

                            if (!string.IsNullOrEmpty(offerName))
                            {
                                objDistributorModel.OfferName = offerName;
                            }
                            else
                            {
                                objDistributorModel.OfferName = "PC Discounts";
                            }
                        }
                        string sql = @"
                                             SELECT 
                                             ROW_NUMBER() OVER(PARTITION BY OrderNo ORDER BY BillDate) AS SNo, 
                                             * 
                                             FROM TrnBillMain 
                                             WHERE OrderNo = @OrderNo 
                                             ORDER BY BillDate;
                                             ";
                        var results = (await connection.QueryAsync(sql, new { OrderNo = objDistributorModel.objListProduct[0].OrderNo })).ToList();
                        // Filter results based on UserBillNo
                        var matchingRow = results.FirstOrDefault(row => row.UserBillNo == BillNo);

                        if (matchingRow != null)
                        {
                            objDistributorModel.OrderNo = $"{objDistributorModel.objListProduct[0].OrderNo}-{matchingRow.SNo}";
                        }

                        string queryBillType = @"
                                                           SELECT BillType 
                                                           FROM TrnBillMain 
                                                           WHERE UserBillNo = @BillNo;
                                                            ";
                        var parametersBillType = new { BillNo = BillNo };
                        objDistributorModel.BillType = (await connection.QueryFirstOrDefaultAsync<string>(queryBillType, parametersBillType));
                        if (objDistributorModel.BillType == null)
                        {
                            queryBillType = @"SELECT BillType 
                                                           FROM TrnBillMain 
                                                           WHERE BillNo = @BillNo;
                                                            ";
                            parametersBillType = new { BillNo = BillNo };
                            objDistributorModel.BillType = (await connection.QueryFirstOrDefaultAsync<string>(queryBillType, parametersBillType));
                        }

                        string PartyInvoicequery = @"SELECT TOP 1 OrderType FROM TrnBillMain WHERE BillNo = @BillNo";
                        objDistributorModel.PartyInvoice = connection.QueryFirstOrDefault<string>(PartyInvoicequery, new { BillNo = BillNo });

                        objDistributorModel.objListProduct = objDistributorModel.objListProduct.OrderByDescending(m => m.ProductType).ThenByDescending(m => m.Rate).ToList();
                        decimal? TotalNetAmount = 0;
                        string OrderType = objDistributorModel.objListProduct[0].OrderType;

                        objDistributorModel.objTaxSummary = new List<TaxSummary>();

                        if (OrderType == "T")
                            objDistributorModel.objListProduct = objDistributorModel.objListProduct.Where(m => m.Amount > 0).ToList();

                        objDistributorModel.objTaxSummary = objDistributorModel.objListProduct

                    .GroupBy(m => new
                    {
                        m.TaxPer,
                        m.CGST,
                        m.SGST,

                    }).Select(m => new TaxSummary
                    {
                        SumTaxPer = m.Key.TaxPer ?? 0,
                        SumCGSTPer = m.Key.CGST,
                        SumSGSTPer = m.Key.SGST,
                        SumTaxAmt = m.Sum(r => r.TaxAmt) ?? 0,
                        SumCGSTAmt = m.Sum(r => r.CGSTAmount),
                        SumSGSTAmt = m.Sum(r => r.SGSTAmount),
                        SumAmount = m.Sum(r => r.Amount),
                        TotalTaxAmount = m.Sum(p => p.CGSTAmount + p.SGSTAmount + p.TaxAmt) ?? 0,
                        SumNetPayableAmount = m.Sum(p => p.CGSTAmount + p.SGSTAmount + p.TaxAmt + p.Amount) ?? 0
                    }).ToList();

                        objDistributorModel.objTaxSummary = objDistributorModel.objTaxSummary.Where(m => m.SumNetPayableAmount > 0).ToList();

                        decimal TotalNetPayableTobill = 0;
                        TotalNetPayableTobill = objDistributorModel.objTaxSummary.Sum(m => m.SumNetPayableAmount);
                        TotalTaxAmt = objDistributorModel.objTaxSummary.Sum(m => m.TotalTaxAmount);

                        string queryresult = @"
                                                   SELECT TOP 1 * 
                                                   FROM M_CompanyMaster 
                                                   WHERE ActiveStatus = @ActiveStatus;
                                                   ";

                        var parametersresult = new { ActiveStatus = "Y" };

                        var result = (await connection.QueryFirstOrDefaultAsync<M_CompanyMaster>(queryresult, parametersresult));
                        if (result != null)
                        {
                            objDistributorModel.CompCity = result.CompCity;
                            string SoldBy = objDistributorModel.objListProduct[0].BillSoldBy;

                            string queryLedger = @"
                                                       SELECT TOP 1 * 
                                                       FROM M_LedgerMaster 
                                                       WHERE PartyCode = @SoldBy;
                                                        ";
                            var parametersLedger = new { SoldBy = SoldBy };
                            var resultDetails = (await connection.QueryFirstOrDefaultAsync<M_LedgerMaster>(queryLedger, parametersLedger));

                            if (resultDetails != null)
                            {
                                objDistributorModel.GSTNo = resultDetails.TinNo;
                                objDistributorModel.SoldByName = resultDetails.PartyName;
                                objDistributorModel.SoldByAddress = resultDetails.Address1;
                                objDistributorModel.SoldByCity = resultDetails.CityName;
                                objDistributorModel.IsGSTRegistered = resultDetails.NewFld3;
                                objDistributorModel.PANNo = resultDetails.PanNo;
                                objDistributorModel.CINNo = resultDetails.NewFld1;
                            }
                            objDistributorModel.BillNo = BillNo;
                            objDistributorModel.BillDate = objDistributorModel.objListProduct[0].BillDate.Date;
                            objDistributorModel.CompanyName = result.CompName;
                            objDistributorModel.CompanyAdd = result.CompAdd;

                            objDistributorModel.objCustomer = new CustomerDetail();
                            var Fcode = objDistributorModel.objListProduct[0].IdNo;

                            string queryCustomerResult = @"
                                                               SELECT TOP 1 * 
                                                               FROM M_LedgerMaster 
                                                               WHERE PartyCode = @FCode;
                                                                ";
                            var parametersCustomerResult = new { FCode = Fcode };
                            var CustomerResult = (await connection.QueryFirstOrDefaultAsync<M_LedgerMaster>(queryCustomerResult, parametersCustomerResult));
                            if (CustomerResult != null)
                            {
                                objDistributorModel.objCustomer.IdNo = CustomerResult.PartyCode;
                                objDistributorModel.objCustomer.Name = CustomerResult.PartyName;
                                objDistributorModel.objCustomer.Address = CustomerResult.Address1;
                                objDistributorModel.objCustomer.MobileNo = CustomerResult.MobileNo.ToString();
                                objDistributorModel.objCustomer.GSTNo = CustomerResult.TinNo;
                                objDistributorModel.objCustomer.PANNo = CustomerResult.PanNo;
                                objDistributorModel.objCustomer.CityName = CustomerResult.CityName;
                                objDistributorModel.objCustomer.StateCode = CustomerResult.StateCode;
                                objDistributorModel.objCustomer.CardNo = "Shoppe Code";
                                objDistributorModel.objCustomer.CustomerType = "Shoppe Name";
                                objDistributorModel.objCustomer.IsRegisteredCustomer = false;
                            }
                            else
                            {
                                if (objDistributorModel.BillType != "J" && objDistributorModel.BillType != "X")
                                {
                                    objDistributorModel.objCustomer = await GetCustInfo(objDistributorModel.objListProduct[0].IdNo);
                                    objDistributorModel.objCustomer.CityName = objDistributorModel.objCustomer.CityName + "[" + objDistributorModel.objCustomer.StateName + "]";
                                }
                                else
                                {
                                    objDistributorModel.objCustomer = await GetSJPCustInfo(objDistributorModel.objListProduct[0].IdNo);

                                    string storedProcedure = "sp_GetNoOfJackpotBill";
                                    var parametersJackpotBill = new { IdNo = objDistributorModel.objListProduct[0].IdNo };

                                    // Execute the stored procedure and fetch the result
                                    var NumberOfbill = connection.QueryFirstOrDefault<int>(
                                  storedProcedure,
                                  parameters,
                                  commandType: CommandType.StoredProcedure
                                   );

                                    objDistributorModel.NumberOfBill = NumberOfbill;
                                }
                                if (objDistributorModel.objCustomer.IdNo == "Record does not exists!")
                                {
                                    objDistributorModel.objCustomer = new CustomerDetail();
                                    objDistributorModel.objCustomer.IdNo = objDistributorModel.objListProduct[0].IdNo;
                                    objDistributorModel.objCustomer.Name = objDistributorModel.objListProduct[0].PartyName;
                                    objDistributorModel.objCustomer.Address = "";
                                    objDistributorModel.objCustomer.CityName = objDistributorModel.objCustomer.CityName + "[" + objDistributorModel.objCustomer.StateName + "]";
                                    objDistributorModel.objCustomer.MobileNo = objDistributorModel.objListProduct[0].Mobileno;
                                    objDistributorModel.objCustomer.PANNo = "";
                                    objDistributorModel.objCustomer.CardNo = "Customer Name";
                                    objDistributorModel.objCustomer.CustomerType = "Customer Name";
                                    objDistributorModel.objCustomer.IsRegisteredCustomer = true;
                                }
                                else
                                {
                                    objDistributorModel.objCustomer.CardNo = "PC ID";
                                    objDistributorModel.objCustomer.CustomerType = "PC Name";
                                    objDistributorModel.objCustomer.IsRegisteredCustomer = false;
                                }
                            }
                            objDistributorModel.StateGSTName = await GetStateGstName(objDistributorModel.objCustomer.StateCode);
                            objDistributorModel.objProduct = new ProductModel();
                            objDistributorModel.objProduct.TotalTaxPer = TotalTaxPer ?? 0;
                            objDistributorModel.objProduct.TotalTaxAmount = TotalTaxAmt ?? 0;
                            objDistributorModel.objProduct.TotalCGSTPer = TotCGSTTaxPer ?? 0;
                            objDistributorModel.objProduct.TotalCGSTAmt = TotCGSTTaxAmt ?? 0;
                            objDistributorModel.objProduct.TotalSGSTPer = TotSGSTTaxPer ?? 0;
                            objDistributorModel.objProduct.TotalSGSTAmt = TotSGSTTaxAmt ?? 0;
                            //objDistributorModel.objProduct.TotalNetPayable = objDistributorModel.objListProduct[0].TotalNetPayable;
                            objDistributorModel.objProduct.TotalNetPayable = TotalNetPayableTobill;
                            objDistributorModel.objProduct.TotalAmount = TotalNetAmount ?? 0;
                            objDistributorModel.Username = objDistributorModel.objListProduct[0].UserName;
                            objDistributorModel.objProduct.Roundoff = objDistributorModel.objListProduct[0].Roundoff;
                            objDistributorModel.objProduct.TotalAmount = objDistributorModel.objListProduct[0].TotalAmount;
                            objDistributorModel.objProduct.TotalBV = objDistributorModel.objListProduct[0].TotalBV;
                            objDistributorModel.objProduct.TotalQty = objDistributorModel.objListProduct[0].TotalQty;
                            objDistributorModel.objProduct.TotalWeight = objDistributorModel.objListProduct[0].TotalWeight;

                            if (objDistributorModel.BillType == "B")
                            {
                                string querybillno = @"
                                                           SELECT TOP 1 BillNo 
                                                           FROM TrnBillMain 
                                                           WHERE BillNo = @UserBillNo";

                                var billno = connection.QueryFirstOrDefault<string>(
                                     querybillno,
                                     new { UserBillNo = objDistributorModel.BillNo }
                                 );
                                decimal adjustmonut = 0;
                                if (billno != null)
                                {
                                    FPVoucher Fpvoucher = new FPVoucher();
                                    Fpvoucher.Amount = 0;
                                    string queryFpvoucher = @"     SELECT  
                                                                   AdjustAmount  
                                                                   FROM FPVoucherUsed 
                                                                   WHERE BillNo = @BillNo";

                                    var fpVoucherData = (await connection.QueryFirstOrDefaultAsync(
                                          queryFpvoucher,
                                          new { BillNo = billno }
                                      ));
                                    if (fpVoucherData != null)
                                    {
                                        Fpvoucher.Amount = Convert.ToDecimal(fpVoucherData.AdjustAmount);
                                        //objDistributorModel.FpVoucher = fpVoucherData.code;
                                    }
                                    string querycouponamt = @"
                                                                  SELECT TOP 1 
                                                                      Amount, 
                                                                      Code 
                                                                  FROM coupon 
                                                                  WHERE BillNo = @BillNo and IdNo=@IdNo";

                                    var couponamt = (await connection.QueryFirstOrDefaultAsync(
                                         querycouponamt,
                                         new { BillNo = billno, IdNo = objDistributorModel.objCustomer.IdNo }
                                     ));
                                    Coupon Coupon = new Coupon();
                                    Coupon.Amount = 0;
                                    if (couponamt != null)
                                    {
                                        Coupon.Amount = Convert.ToDecimal(couponamt.Amount);
                                        objDistributorModel.CouponCode = couponamt.Code;
                                    }
                                    objDistributorModel.CouponAmt = Coupon.Amount;
                                    objDistributorModel.FpVoucherAmt = Fpvoucher.Amount;
                                    //if (Coupon.Amount != 0 && Fpvoucher.Amount == 0)
                                    //{
                                    //    if (TotalNetPayableTobill <= Coupon.Amount)
                                    //    {
                                    //        objDistributorModel.CouponAmt = TotalNetPayableTobill;
                                    //    }
                                    //    else
                                    //    {
                                    //        adjustmonut = TotalNetPayableTobill - Convert.ToDecimal(Coupon.Amount);
                                    //        objDistributorModel.CouponAmt = TotalNetPayableTobill - Convert.ToDecimal(Coupon.Amount);
                                    //    }
                                    //}
                                    //else if (Coupon.Amount == 0 && Fpvoucher.Amount != 0)
                                    //{
                                    //    if (TotalNetPayableTobill <= Fpvoucher.Amount)
                                    //    {
                                    //        objDistributorModel.FpVoucherAmt = TotalNetPayableTobill;
                                    //    }
                                    //    else
                                    //    {
                                    //        adjustmonut = TotalNetPayableTobill - Convert.ToDecimal(Coupon.Amount);
                                    //        objDistributorModel.FpVoucherAmt = TotalNetPayableTobill - Convert.ToDecimal(Fpvoucher.Amount);
                                    //    }
                                    //}
                                    //else if (Coupon.Amount != 0 && Fpvoucher.Amount != 0)
                                    //{
                                    //    decimal temptot = Convert.ToDecimal(Fpvoucher.Amount) + Convert.ToDecimal(Coupon.Amount);
                                    //    if (TotalNetPayableTobill <= temptot)
                                    //    {
                                    //        objDistributorModel.FpVoucherAmt = TotalNetPayableTobill - Convert.ToDecimal(Coupon.Amount);
                                    //        objDistributorModel.CouponAmt = Convert.ToDecimal(Coupon.Amount);
                                    //    }
                                    //    else
                                    //    {
                                    //        adjustmonut = TotalNetPayableTobill - Convert.ToDecimal(temptot);
                                    //        objDistributorModel.FpVoucherAmt = Convert.ToDecimal(Fpvoucher.Amount);
                                    //        objDistributorModel.CouponAmt = Convert.ToDecimal(Coupon.Amount);
                                    //    }
                                    //}
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {

            }
            return objDistributorModel;
        }

        public async Task<CustomerDetail> GetSJPCustInfo(string IdNo)
        {
            CustomerDetail objCustomerDetail = new CustomerDetail();
            if (!(string.IsNullOrEmpty(IdNo)))
            {
                try
                {
                    using (var connection = _context.CreateConnection())
                    {
                        string query = "SELECT *,IDno as IdNo,MemberName as Name,ActiveStatus as IsActive, Mobl as MobileNo FROM M_JackpotMaster WHERE IDno = @IdNo";
                        var customerDetail = (await connection.QueryFirstOrDefaultAsync<CustomerDetail>(query, new { IdNo = IdNo }));

                        if (customerDetail != null)
                        {
                            objCustomerDetail.IdNo = customerDetail.IdNo != null ? customerDetail.IdNo.ToString() : "";
                            objCustomerDetail.Name = customerDetail.Name != null ? customerDetail.Name.ToString() : "";
                            objCustomerDetail.FormNo = customerDetail.FormNo != null ? decimal.Parse(customerDetail.FormNo.ToString()) : 0;
                            objCustomerDetail.IsActive = customerDetail.IsActive.ToString() == "Y" ? true : false;
                            objCustomerDetail.MobileNo = customerDetail.MobileNo.ToString();
                        }
                        else
                        {
                            objCustomerDetail = new CustomerDetail();
                            objCustomerDetail.IdNo = "Record does not exists!";
                            objCustomerDetail.Name = "";
                        }
                        objCustomerDetail.MinRepurch = 0;

                        objCustomerDetail.InvoiceType = new List<string>();
                        objCustomerDetail.InvoiceType.Add("SJP Customer Invoice,J");
                        objCustomerDetail.IsBillOnMrp = true;
                    }
                }
                catch (Exception e)
                {
                    objCustomerDetail = new CustomerDetail();
                    objCustomerDetail.IdNo = "Something went wrong!";
                    objCustomerDetail.Name = "";
                }
            }
            return objCustomerDetail;
        }

        public async Task<string> GetStateGstName(decimal StateCode)
        {
            string statename = "";
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    string query = @"
                SELECT 
                    StateName + ' (' + 
                    CASE 
                        WHEN LEN(CAST(DivisionCode AS VARCHAR(5))) = 1 THEN '0' + CAST(DivisionCode AS VARCHAR(5)) 
                        ELSE CAST(DivisionCode AS VARCHAR(5)) 
                    END + ')' AS S
                FROM M_StateDivMaster 
                WHERE RowStatus = 'Y' 
                  AND StateCode > 0 
                  AND StateCode = @StateCode";

                    statename = (await connection.QueryFirstOrDefaultAsync<string>(
                        query,
                        new { StateCode }
                    ));
                }

            }
            catch (Exception)
            {

                throw;
            }
            return statename;
        }

        public async Task<FPVoucher> GetCheckFpWallet(string Idno)
        {
            FPVoucher fPVoucher = new FPVoucher();
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    var query = @"exec sp_GetFPBalance @Idno";
                    fPVoucher = await connection.QueryFirstOrDefaultAsync<FPVoucher>(query, new
                    {
                        Idno = @Idno
                    });
                }
            }
            catch (Exception ex)
            {

            }
            return fPVoucher;
        }

        public async Task<FPVoucherEligibilityResult> CheckFPVoucherEligibilityAsync(string idno)
        {
            FPVoucherEligibilityResult result = new FPVoucherEligibilityResult();
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    var query = @"exec CheckFPVoucherEligibility @IDno";
                    result = await connection.QueryFirstOrDefaultAsync<FPVoucherEligibilityResult>(query, new
                    {
                        Idno = idno
                    });
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<List<string>> GetAvailStockProductNamesOnly(string StockforParty)
        {
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {

                    var query = @"
                    SELECT DISTINCT pm.ProductName
                    FROM M_ProductMaster pm
                   INNER JOIN Im_CurrentStock cs ON cs.ProdId = pm.ProdId
                   WHERE pm.ActiveStatus = 'Y'
                   AND pm.IsBillingAllowed = 'Y'
                   AND pm.IsCardIssue = 'N'
                   AND cs.FCode = @StockforParty
                   GROUP BY pm.ProductName, cs.ProdId
                  HAVING SUM(cs.Qty) > 0;";

                    var productNames = (await connection.QueryAsync<string>(query, new { StockforParty })).ToList();

                    return productNames;
                }
            }
            catch (Exception ex)
            {
                // Log error
                return new List<string>();
            }
        }

        public async Task<List<PartyModel>> GetAllPartyNew(string LoginPartyCode, decimal LoginStateCode, bool NeedWallet)
        {
            List<PartyModel> objpartyList = new List<PartyModel>();

            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {

                    // -----------------------------------------
                    // GET USER INFO (IsAdmin, GroupId, SameLevelBilling)
                    // -----------------------------------------
                    string userQuery = @"
                SELECT TOP 1 
                    r.IsAdmin,
                    s.GroupId,
                    s.RecvdCForm AS SameLevelBilling
                FROM Inv_M_UserMaster r
                INNER JOIN M_LedgerMaster s ON r.BranchCode = s.PartyCode
                WHERE s.PartyCode = @LoginPartyCode";

                    var userInfo = await connection.QueryFirstOrDefaultAsync(userQuery, new { LoginPartyCode });

                    bool IsAdmin = false;
                    bool IsSoldByHo = false;
                    decimal LoginGroupId = 0;
                    string SameLevelBilling = "";

                    if (userInfo != null)
                    {
                        LoginGroupId = userInfo.GroupId;

                        if (LoginGroupId == 0)
                        {
                            IsSoldByHo = true;
                        }
                        else
                        {
                            IsSoldByHo = false;
                        }
                        IsAdmin = userInfo.IsAdmin == "Y" ? true : false;
                        //  IsSoldByHo = (LoginGroupId == 0);
                        // IsAdmin = (userInfo.IsAdmin == "Y");
                        SameLevelBilling = userInfo.SameLevelBilling;
                   }

                    // -----------------------------------------
                    // GET PARTY LIST WITH OR WITHOUT WALLET
                    // -----------------------------------------
                    if (NeedWallet)
                    {
                        string queryWithWallet = @"
                    SELECT 
                        p.PartyCode,
                        p.ParentPartyCode,
                        p.PartyName,
                        p.StateCode,
                        p.GroupId,
                        p.UserPartyCode,
                        p.Address1,
                        p.TinNo AS GSTIN,
                        (w.Balance) AS CreditLimit,

                        -- PromoBalance = SUM(CrAmount) - SUM(DrAmount)
                        (
                            ISNULL((SELECT SUM(Amount) FROM TrnVoucher WHERE VType='X' AND CrTo = p.PartyCode), 0) -
                            ISNULL((SELECT SUM(Amount) FROM TrnVoucher WHERE VType='X' AND DrTo = p.PartyCode), 0)
                        ) AS PromoBalance

                    FROM M_LedgerMaster p
                    INNER JOIN V#PartyBalance w ON p.PartyCode = w.PartyCode
                    WHERE p.ActiveStatus='Y' AND p.PartyCode <> @LoginPartyCode
                    ORDER BY p.PartyName";

                        objpartyList = (await connection.QueryAsync<PartyModel>(queryWithWallet, new { LoginPartyCode })).ToList();
                    }
                    else
                    {
                        string queryNoWallet = @"
                    SELECT 
                        p.PartyCode,
                        p.ParentPartyCode,
                        p.PartyName,
                        p.StateCode,
                        p.GroupId,
                        p.UserPartyCode,
                        p.Address1,
                        0 AS CreditLimit,
                        p.TinNo AS GSTIN,
                        0 AS PromoBalance
                    FROM M_LedgerMaster p
                    WHERE p.ActiveStatus='Y' AND p.PartyCode <> @LoginPartyCode
                    ORDER BY p.PartyName";

                        objpartyList = (await connection.QueryAsync<PartyModel>(queryNoWallet, new { LoginPartyCode })).ToList();
                    }

                    // -----------------------------------------
                    // FILTER LEVEL WISE BILLING
                    // -----------------------------------------
                    if (!IsSoldByHo)
                    {
                        objpartyList = objpartyList
                            .Where(m =>
                                m.PartyCode != LoginPartyCode &&
                                (
                                    (NeedWallet && LoginGroupId == 1 &&
                                        (
                                            (m.StateCode == LoginStateCode && m.GroupId > LoginGroupId) ||
                                            (SameLevelBilling == "Y" && m.GroupId >= LoginGroupId) ||
                                            m.ParentPartyCode == LoginPartyCode ||
                                            m.GroupId == 0
                                        )
                                    ) ||
                                    (!(NeedWallet && LoginGroupId == 1) &&
                                        (
                                            (m.StateCode == LoginStateCode && m.GroupId > LoginGroupId) ||
                                            (SameLevelBilling == "Y" && m.GroupId >= LoginGroupId) ||
                                            m.ParentPartyCode == LoginPartyCode
                                        )
                                    )
                                )
                            ).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO: Add logging
            }

            return objpartyList;
        }

        public async Task<List<string>> GetAllBarcode()
        {
            List<string> barcodes = new List<string>();

            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {

                    string query = @"
                         SELECT Barcode
                        FROM M_ProductMaster
                        WHERE ActiveStatus = 'Y'
                     AND IsBillingAllowed = 'Y'";

                    barcodes = (await connection.QueryAsync<string>(query)).ToList();
                }
            }
            catch (Exception ex)
            {
                // TODO: Add error logging
            }

            return barcodes;
        }

        public async Task<List<StateModel>> GetStateList()
        {
            List<StateModel> stateList = new List<StateModel>();

            try
            {
                using (var connection = _context.CreateConnection())
                {
                    connection.Open();

                    // Get list of states
                    string stateQuery = @"
                SELECT StateCode, StateName
                FROM M_StateDivMaster
                WHERE RowStatus = 'Y' AND ActiveStatus = 'Y'";

                    var states = (await connection.QueryAsync<StateModel>(stateQuery)).ToList();

                    // Get Company details
                    using (var connectioninv = _context.CreateLiveconnInv())
                    {
                        string companyQuery = @"
                        SELECT TOP 1 CompState
                       FROM M_CompanyMaster
                      WHERE ActiveStatus = 'Y'";

                        var compState = (await connectioninv.QueryFirstOrDefaultAsync<decimal?>(companyQuery)) ?? 0;

                        // Assign IsCompanyState & add to final list
                        foreach (var state in states)
                        {
                            state.IsCompanyState = (state.StateCode == compState);
                            stateList.Add(state);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                // TODO: error logging
            }

            return stateList;
        }

        public async Task<List<string>> GetAutocompleteProductNames(string InvType)
        {
            List<string> productNames = new List<string>();

            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    connection.Open();

                    string baseQuery = @"
                SELECT ProductName, IsCardIssue
                FROM M_ProductMaster
                WHERE ActiveStatus = 'Y'
                  AND IsBillingAllowed = 'Y'";

                    var products = (await connection.QueryAsync<dynamic>(baseQuery)).ToList();

                    if (InvType == "Purchase")
                    {
                        // Purchase
                        productNames = products
                                        .Select(p => (string)p.ProductName)
                                        .ToList();
                    }
                    else if (InvType == "Partybill")
                    {
                        // Partybill logic (card issue removed)
                        productNames = products
                                        .Where(p => p.IsCardIssue == "N")
                                        .Select(p => (string)p.ProductName)
                                        .ToList();
                    }
                    else
                    {
                        // All Others : not purchase, not partybill
                        productNames = products
                                        .Where(p => p.IsCardIssue == "N")
                                        .Select(p => (string)p.ProductName)
                                        .ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                // TODO: Log exception
            }

            return productNames;
        }

        public async Task<ResponseDetail> CheckBillCustomer(string mobile)
        {
            ResponseDetail objResponse = new ResponseDetail
            {
                ResponseMessage = "Not Exist",
                ResponseStatus = "OK"
            };

            try
            {
                string query = $"SELECT IdNo FROM M_MemberMaster WHERE Mobl = @Mobile";

                using (var connection = _context.CreateConnection())
                {
                    var result = (await connection.QueryFirstOrDefaultAsync<string>(query, new { Mobile = mobile }));

                    if (!string.IsNullOrEmpty(result))
                    {
                        objResponse.ResponseMessage = result;
                        objResponse.ResponseStatus = "OK";
                    }
                }
            }
            catch (Exception ex)
            {
                objResponse.ResponseMessage = ex.Message;
                objResponse.ResponseStatus = "FAILED";
            }

            return objResponse;
        }
        public async Task<DistributorBillModel> GetSoldBy(string BillNo, string id)
        {
            DistributorBillModel objDistributorModel = new DistributorBillModel();
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {

                    string query = @"
                     SELECT 
                     r.SoldBy,
                     r.BillNo
                     FROM TrnBillMain r
                     WHERE r.UserBillNo = @BillNo 
                     AND (@Id = 'F' OR r.FCode = @Id);
                    ";

                    var parameters = new { BillNo = BillNo, Id = id };
                    objDistributorModel = (await connection.QueryAsync<DistributorBillModel>(query, parameters)).FirstOrDefault();
                }
            }
            catch
            {

            }
            return objDistributorModel;
        }

    }
}
