using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml;
using VitaFlow.Domain.CommonDTO;
using VitaFlow.Domain.Entities;
using VitaFlow.Domain.Interface;
using VitaFlow.Infrastructure.DapperContext;
using static Dapper.SqlMapper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace VitaFlow.Infrastructure.Repository
{
    public class ProductRepository : I_Product
    {
        private readonly DapperDbContext _context;
        public ProductRepository(DapperDbContext dapperContext)
        {
            _context = dapperContext;
        }

        public async Task<List<Product>> Productlist(string FCode)
        {
            List<Product> obj = new List<Product>();
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    var storedProcedureName = "GetProducts_Update";
                    var values = new { @FCode = FCode };
                    obj = (await connection.QueryAsync<Product>(storedProcedureName, values, commandType: CommandType.StoredProcedure)).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return obj;
        }
        public async Task<string> AddtoCart(M_CartDetails req)
        {
            string Str = string.Empty;
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    var storedProcedureName = "sp_AddtoCartnew1";
                    var values = new
                    {
                        @Action = req.Action,
                        @productname = req.ProdName,
                        @ProdId = req.ProdId,
                        @Imgpath = req.imagePath,
                        @price = req.Price,
                        @bv = req.bv,
                        @qty = req.qty,
                        @IPAddress = req.IpAddress,
                        @UnqiueId = req.UnqiueId,
                        @userId = req.userid,
                        @Weight = req.Weight,
                        @PV = req.PV,
                        @FCode = req.FCode,
                        @BatchNo = req.BatchNo


                    };
                    Str = (await connection.QueryAsync<string>(storedProcedureName, values, commandType: CommandType.StoredProcedure)).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

            }
            return Str;
        }

        public async Task<List<M_CartDetails>> CartDetails(M_CartDetails req)
        {
            List<M_CartDetails> list = new List<M_CartDetails>();
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    var storedProcedureName = "sp_AddtoCartnew1";
                    var values = new
                    {
                        @Action = req.Action,
                        @userId = req.userid,
                        @FCode = req.FCode,
                        @ParentPartyCode = req.ParentPartycode
                    };
                    list = (await connection.QueryAsync<M_CartDetails>(storedProcedureName, values, commandType: CommandType.StoredProcedure)).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return list;
        }

        public async Task<string> GetCartTotal(M_CartDetails req)
        {
            string Rstr = string.Empty;
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    var storedProcedureName = "sp_AddtoCartnew1";
                    var values = new
                    {
                        @Action = req.Action,
                        @userId = req.userid,
                    };
                    Rstr = (await connection.QueryAsync<string>(storedProcedureName, values, commandType: CommandType.StoredProcedure)).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

            }
            return Rstr;
        }
        public async Task<string> Updatecart(M_CartDetails req)
        {
            string Rstr = string.Empty;
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    var storedProcedureName = "sp_AddtoCartnew1";
                    var values = new
                    {
                        @Action = req.Action,
                        @userId = req.userid,
                        @ProdId = req.ProdId,
                        @qty = req.qty
                    };
                    Rstr = (await connection.QueryAsync<string>(storedProcedureName, values, commandType: CommandType.StoredProcedure)).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

            }
            return Rstr;
        }

        public async Task<string> DeleteCart(M_CartDetails req)
        {
            string Rstr = string.Empty;
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    var storedProcedureName = "sp_AddtoCartnew1";
                    var values = new
                    {
                        @Action = req.Action,
                        @Id = req.id
                    };
                    Rstr = (await connection.QueryAsync<string>(storedProcedureName, values, commandType: CommandType.StoredProcedure)).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

            }
            return Rstr;
        }

        public async Task<ShippingDetail> GetShippingDetail(ShippingDetail req)
        {
            ShippingDetail obj = new ShippingDetail();
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    var storedProcedureName = "Sp_GetShippingDetail";
                    var values = new
                    {
                        @userId = req.Userid,
                    };
                    obj = (await connection.QueryAsync<ShippingDetail>(storedProcedureName, values, commandType: CommandType.StoredProcedure)).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

            }
            return obj;
        }

        public async Task<List<M_City>> GetCityList(string Statecode)
        {
            List<M_City> obj = new List<M_City>();
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    var storedProcedureName = "Sp_GetCityList";
                    var values = new
                    {
                        @Statecode = Statecode,
                    };
                    obj = (await connection.QueryAsync<M_City>(storedProcedureName, values, commandType: CommandType.StoredProcedure)).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return obj;
        }
        public async Task<string> SaveFranchiseShippingDetail(ShippingDetail req)
        {
            string Rstr = "";
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    var storedProcedureName = "Sp_SaveFranchiseShippingDetail";
                    var values = new
                    {
                        @Userid = req.Userid,
                        @BusinessName = req.BusinessName,
                        @CustomerName = req.CustomerName,
                        @Address1 = req.Address1,
                        @Address2 = req.Address2,
                        @StateCode = req.StateCode,
                        @CityCode = req.CityCode,
                        @Pincode = req.Pincode,
                        @MobileNo = req.MobileNo,
                        @Email = req.Email
                    };
                    Rstr = (await connection.QueryAsync<string>(storedProcedureName, values, commandType: CommandType.StoredProcedure)).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

            }
            return Rstr;
        }

        public async Task<ResponseDetail> SaveDistributorBill(DistributorBillModel req)
        {
            ResponseDetail objResponse = new ResponseDetail();
            try
            {
                decimal maxUserSBillNo = 0;
                decimal? SessId = 0;
                string billPrefix = "";
                decimal maxSbillNo = 0;
                decimal? FsessId = 0;
                string UserBillNo = "";
                string version = "";
                SqlTransaction objTrans = null;
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

            }
            catch (Exception ex)
            {

            }
            return objResponse;
        }
        public async Task<M_CompanyMaster> GetCompanyDetail()
        {
            M_CompanyMaster obj = new M_CompanyMaster();
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    var sql = "select * from M_CompanyMaster";
                    obj = (await connection.QueryAsync<M_CompanyMaster>(sql, commandType: CommandType.Text)).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {

            }
            return obj;
        }
        public async Task<ResponseDetail> SavePartyOrderDetails(OrderReq req)
        {
            ResponseDetail objResponse = new ResponseDetail();
            objResponse.ResponseMessage = "Something went wrong!";
            objResponse.ResponseStatus = "FAILED";
            try
            {
                bool IsWalletEntry = false;
                decimal WalletBalance = 0;
                decimal? SessId = 0;
                decimal? FsessId = 0;
                string version = "";
                decimal maxSbillNo = 0;
                string billPrefix = "";
                DateTime BillDate = DateTime.Now.Date;
                TrnPayModeDetail objDtPayModeDetail = new TrnPayModeDetail();
                List<string> Paymode = new List<string>();
                List<string> PayPrefix = new List<string>();
                List<TrnPayModeDetail> objDTListPayMode = new List<TrnPayModeDetail>();

                using (var connection = _context.CreateLiveconnInv())
                {
                    try
                    {
                        var sql = "Select Max(SessID) as MaxSessId from M_SessnMaster";
                        SessId = (await connection.QueryAsync<decimal>(sql, commandType: CommandType.Text)).FirstOrDefault();

                        var FsessIdsql = "select  FSessId from M_FiscalMaster where ActiveStatus='Y'";
                        FsessId = (await connection.QueryAsync<decimal>(FsessIdsql, commandType: CommandType.Text)).FirstOrDefault();

                        var billPrefixsql = "select BillPrefix from M_ConfigMaster";
                        billPrefix = (await connection.QueryAsync<string>(billPrefixsql, commandType: CommandType.Text)).FirstOrDefault();

                        var versionsql = "select VersionNo from M_NewHOVersionInfo";
                        version = (await connection.QueryAsync<string>(versionsql, commandType: CommandType.Text)).FirstOrDefault();

                        //decimal totalwalletamount = req.NetAmount - req.Promobalance;
                        decimal totalwalletamount = req.NetAmount;
                        var query = "INSERT INTO TrnVoucher(VoucherNo,VoucherDate,DrTo,Crto,Amount,Narration,Refno,VType,BType,AccDocType,SessID,FSessID) " +
                                    "Select CASE WHEN Max(VoucherNo) is NULL THEN 1 ELSE Max(VoucherNo)+1 END ,Cast(Convert(varchar,Getdate(),106) as Datetime),'" + req.OrderBy + "','" + (req.OrderTo).Trim() + "','" + decimal.Parse(totalwalletamount.ToString()) + "','Order " + (req.OrderNo).Trim() + " generated for product.','" + (req.OrderNo).Trim() + "','" + req.wallettype + "','O','Order Generated','" + SessId + "','" + FsessId + "' FROM TrnVoucher;";
                        var rowsAffected = await connection.ExecuteAsync(query);
                        if (rowsAffected > 0)
                        {
                            EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Wallet;
                            string value = EnumPayModes.GetEnumDescription(enumVar);
                            PayPrefix.Add(value);
                            //objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = objModel.objProduct.TotalNetPayable, SoldBy = objModel.objCustomer.UserDetails.PartyCode, BillDate = DateTime.Now.Date, BillType = objModel.objCustomer.SelectedInvoiceType, BillNo = billPrefix + "/" + objModel.objCustomer.UserDetails.PartyCode + "/" + maxSbillNo, PayPrefix = value, Amount = objModel.objProduct.PayDetails.AmountByWallet, BankCode = 0, BankName = "", AcNo = "", IFSCode = "", Narration = "", DUserId = 0, DRecTimeStamp = null, ChqDDNo = "", ChqDDDate = null, CardNo = objModel.objCustomer.CardNo, ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = objModel.objCustomer.UserDetails.UserId, Version = version, UserName = objModel.objCustomer.UserDetails.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                            ////insert entry into couponsalesdetails for wallet
                            IsWalletEntry = true;
                        }
                        else
                        {
                            objResponse.ResponseStatus = "FAILED";
                            objResponse.ResponseMessage = "Something went wrong";
                            objResponse.StatusCode = 101;
                            return objResponse;
                        }

                        if(req.Promobalance>0)
                        {
                            var promobalquery = "INSERT INTO TrnVoucher(VoucherNo,VoucherDate,DrTo,Crto,Amount,Narration,Refno,VType,BType,AccDocType,SessID,FSessID) " +
                                   "Select CASE WHEN Max(VoucherNo) is NULL THEN 1 ELSE Max(VoucherNo)+1 END ,Cast(Convert(varchar,Getdate(),106) as Datetime),'" + req.OrderBy + "','" + (req.OrderTo).Trim() + "','" + decimal.Parse(req.Promobalance.ToString()) + "','Order " + (req.OrderNo).Trim() + " generated for product.','" + (req.OrderNo).Trim() + "','X','O','Order Generated','" + SessId + "','" + FsessId + "' FROM TrnVoucher;";
                            var promorowsAffected = await connection.ExecuteAsync(promobalquery);
                            if (promorowsAffected > 0)
                            {
                                EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Voucher;
                                string value = EnumPayModes.GetEnumDescription(enumVar);
                                PayPrefix.Add(value);
                            }
                            else
                            {
                                objResponse.ResponseStatus = "FAILED";
                                objResponse.ResponseMessage = "Something went wrong";
                                objResponse.StatusCode = 101;
                                return objResponse;
                            }
                        }
                        
                        string maxSBillNosql = @"
                                          SELECT ISNULL(MAX(SBillNo), 0)
                                          FROM TrnBillMain";

                        maxSbillNo = await connection.QuerySingleOrDefaultAsync<int>(maxSBillNosql);
                        maxSbillNo = maxSbillNo + 1;
                        if (req.IsP)
                        {
                            EnumPayModes.PayModes enumVar = EnumPayModes.PayModes.Paytm;
                            string value = EnumPayModes.GetEnumDescription(enumVar);
                            PayPrefix.Add(value);
                            objDTListPayMode.Add(new TrnPayModeDetail { BillAmt = req.NetAmount, SoldBy = req.OrderBy, BillDate = DateTime.Now.Date, BillType = req.SelectedInvoiceType, BillNo = billPrefix + "/" + req.OrderBy + "/" + maxSbillNo, PayPrefix = value, Amount = req.AmountByPaytm, BankCode = 0, BankName = "", AcNo = "", IFSCode = "", Narration = "", DUserId = 0, DRecTimeStamp = null, ChqDDNo = req.PaytmTransactionId, ChqDDDate = DateTime.Now, CardNo = "", ActiveStatus = "Y", RecTimeStamp = DateTime.Now, UserId = req.UserId, Version = version, UserName = req.UserName, FSessId = FsessId ?? 0, SBillNo = maxSbillNo });
                        }
                        if (PayPrefix.Count > 0)
                        {
                            string PayPrefixsql = @"
                                             SELECT PayMode 
                                             FROM M_PayModeMaster 
                                             WHERE Prefix IN @Prefixes";

                            // Execute the query and retrieve the results
                            Paymode = connection.Query<string>(sql, new { Prefixes = PayPrefix }).ToList();

                        }
                        string SoldByCode = "";
                        List<TrnBillData> tempTableList = new List<TrnBillData>();
                        List<ProductModel> objListProductModel = new List<ProductModel>();
                        int TrnPartyOrderrowsAffected = 0;
                        foreach (var obj in req.Orderdetail)
                        {
                            //objListProductModel.Add(obj);
                            TrnPartyOrderDetail objDTBillData = new TrnPartyOrderDetail();
                            objDTBillData.OrderNo = req.OrderNo;
                            objDTBillData.OrderTo = req.OrderTo;
                            objDTBillData.OrderBy = req.OrderBy;
                            var splitValues = req.OrderNo.Split('/');
                            objDTBillData.SOrderNo = decimal.Parse(splitValues[2]);
                            objDTBillData.PLNo = 0;
                            objDTBillData.PLDate = DateTime.Now;
                            objDTBillData.ProductCode = obj.ProdCode.ToString();
                            objDTBillData.ProductName = obj.ProdName;
                            objDTBillData.Qty = obj.qty;
                            objDTBillData.DispatchQty = 0;
                            objDTBillData.RemQty = obj.qty;
                            objDTBillData.Weight = 0;
                            objDTBillData.Carton = "";
                            objDTBillData.MonoCarton = "";
                            objDTBillData.MRP = obj.MRP;
                            objDTBillData.DP = obj.DP;
                            objDTBillData.Rate = obj.Rate;
                            objDTBillData.Amount = obj.Amount;
                            objDTBillData.NetWeight = 0;
                            objDTBillData.DispatchAmt = 0;
                            objDTBillData.DispWeight = 0;
                            objDTBillData.ProdStatus = "O";
                            objDTBillData.PLGen = "N";
                            objDTBillData.OrdType = "O";
                            objDTBillData.Status = "P";
                            objDTBillData.CardStatus = "";
                            objDTBillData.ActiveStatus = "Y";
                            objDTBillData.Version = version;
                            objDTBillData.UserId = req.UserId;
                            objDTBillData.RecTimeStamp = DateTime.Now;
                            objDTBillData.PLUser = "0";
                            objDTBillData.PLUser = "";
                            objDTBillData.PLRecTimeStamp = DateTime.Now;
                            objDTBillData.Transporter = "";
                            objDTBillData.LRNo = "";
                            objDTBillData.LRDate = DateTime.Now;
                            objDTBillData.Fld1 = "";
                            objDTBillData.Fld2 = "";
                            objDTBillData.Fld3 = "";
                            //objDTBillData.BatchNo = obj.BatchNo;
                            //objDTBillData.Barcode = obj.Barcode;
                            objDTBillData.BatchNo = "";
                            objDTBillData.Barcode = "";
                            objDTBillData.PLQty = 0;
                            objDTBillData.PLDispQty = 0;
                            objDTBillData.PLRemQty = 0;
                            objDTBillData.PLStatus = "P";
                            objDTBillData.MID = 0;
                            objDTBillData.DiscPer = obj.DiscPer;
                            objDTBillData.Discount = obj.DiscAmt;
                            objDTBillData.FSessId = FsessId ?? 0;
                            objDTBillData.IsKit = "N";
                            objDTBillData.ProdType = "P";
                            objDTBillData.BV = obj.bv;
                            objDTBillData.BVValue = obj.BVValue;
                            objDTBillData.RP = obj.RP;
                            objDTBillData.RPValue = obj.RPValue;
                            objDTBillData.OfferUId = 0;
                            objDTBillData.VAT = 0;
                            objDTBillData.TaxAmount = obj.TaxAmt;
                            objDTBillData.Tax = obj.TaxPer;
                            objDTBillData.PV = obj.PV;
                            objDTBillData.PVValue = obj.PVValue;
                            //objDTBillData.CGST = obj.TaxPer / 2 ?? 0;
                            //objDTBillData.CGSTAmt = obj.TaxAmt / 2 ?? 0;
                            //objDTBillData.SGST = obj.TaxPer / 2 ?? 0;
                            //objDTBillData.SGSTAmt = obj.TaxAmt / 2 ?? 0;
                            objDTBillData.TaxType = "I";

                            //entity.TrnPartyOrderDetails.Add(objDTBillData);
                            var TrnPartyOrder = @"insert into TrnPartyOrderDetail(OrderNo,SOrderNo,PLNo,PLDate,OrderBy,ProductCode,ProductName,Qty,DispatchQty,RemQty,Weight,Carton,MonoCarton,
                                             MRP,DP,Rate,Tax,TaxType,Amount,TaxAmount,NetWeight,DispatchAmt,DispWeight,ProdStatus,PLGen,OrdType,Status,CardStatus,ActiveStatus,Version,UserId,
                                             RecTimeStamp,PLUserId,PLUser,PLRecTimeStamp,Transporter,LRNo,LRDate,Fld1,Fld2,Fld3,BatchNo,Barcode,PLQty,PLDispQty,PLRemQty,PLStatus,
                                             OrderTo,MID,DiscPer,FSessId,Discount,IsKit,ProdType,BV,BVValue,RP,RPValue,OfferUId,VAT,PV,PVValue)
                                             Values(@OrderNo,@SOrderNo,@PLNo,@PLDate,@OrderBy,@ProductCode,@ProductName,@Qty,@DispatchQty,@RemQty,@Weight,@Carton,@MonoCarton,
                                             @MRP,@DP,@Rate,@Tax,@TaxType,@Amount,@TaxAmount,@NetWeight,@DispatchAmt,@DispWeight,@ProdStatus,@PLGen,@OrdType,@Status,@CardStatus,@ActiveStatus,@Version,@UserId,
                                             Getdate(),@PLUserId,@PLUser,@PLRecTimeStamp,@Transporter,@LRNo,@LRDate,@Fld1,@Fld2,@Fld3,@BatchNo,@Barcode,@PLQty,@PLDispQty,@PLRemQty,@PLStatus,
                                             @OrderTo,@MID,@DiscPer,@FSessId,@Discount,@IsKit,@ProdType,@BV,@BVValue,@RP,@RPValue,@OfferUId,@VAT,@PV,@PVValue)";
                            TrnPartyOrderrowsAffected = await connection.ExecuteAsync(TrnPartyOrder, objDTBillData);
                        }

                        var BillMainrowsAffected = 0;

                        if (TrnPartyOrderrowsAffected > 0)
                        {
                            TrnPartyOrderMain objDTBillMain = new TrnPartyOrderMain();
                            objDTBillMain.OrderBy = req.OrderBy;
                            objDTBillMain.OrderTo = req.OrderTo;
                            objDTBillMain.OrderDate = BillDate.Date;
                            objDTBillMain.GroupId = req.GroupId;
                            objDTBillMain.PGroupId = 0;
                            var splitValues = req.OrderNo.Split('/');
                            objDTBillMain.SOrderNo = decimal.Parse(splitValues[2]);
                            objDTBillMain.OrderNo = req.OrderNo;
                            objDTBillMain.PLNo = 0;
                            objDTBillMain.PLDate = DateTime.Now;
                            objDTBillMain.BillNo = "";
                            objDTBillMain.BillDate = BillDate.Date;
                            objDTBillMain.PartyName = req.PartyName;
                            objDTBillMain.RefNo = "";
                            objDTBillMain.Paymode = Paymode[0];
                            objDTBillMain.chNo = 0;
                            //objPartyOrderModel.objProduct.PayDetails.ChequeDate = DateTime.Now;

                            objDTBillMain.ChDate = DateTime.Now;
                            objDTBillMain.ChAmt = 0;
                            objDTBillMain.BankName = "";
                            objDTBillMain.TotalWeight = req.NetAmount;
                            objDTBillMain.TotalOrdQty = req.TotalQty;
                            objDTBillMain.TotalDispQty = 0;
                            objDTBillMain.TotalRemQty = req.TotalQty;
                            objDTBillMain.TotalAmount = req.TotalAmount;
                            objDTBillMain.TotalTaxAmt = req.TotalTaxAmount;
                            objDTBillMain.RndOff = req.Roundoff;
                            objDTBillMain.NetPayable = req.NetAmount;
                            objDTBillMain.LastPLDate = DateTime.Now;
                            objDTBillMain.Remarks = "";
                            objDTBillMain.OType = "O";
                            objDTBillMain.PLUserId = 0;
                            objDTBillMain.PLUser = "";
                            objDTBillMain.PLRecTimeStamp = DateTime.Now;
                            objDTBillMain.IsModify = "N";
                            objDTBillMain.PLStatus = "P";
                            objDTBillMain.MID = 0;
                            objDTBillMain.Status = "P";
                            objDTBillMain.ActiveStatus = "Y";
                            objDTBillMain.Version = version;
                            objDTBillMain.UserId = req.UserId;
                            objDTBillMain.RecTimeStamp = DateTime.Now;
                            objDTBillMain.FSessId = FsessId ?? 0;
                            objDTBillMain.UserName = req.UserName;
                            objDTBillMain.IsConfirm = "N";
                            objDTBillMain.ConfDate = DateTime.Now;
                            objDTBillMain.ConfUserID = 0;
                            objDTBillMain.TotalDiscount = req.TotalDiscount;
                            objDTBillMain.BankCode = 0;
                            objDTBillMain.TotalBV = req.TotalBV;
                            objDTBillMain.TotalRP = req.TotalRP;
                            objDTBillMain.TotalPV = req.TotalPV;
                            objDTBillMain.UID = "";
                            objDTBillMain.OrderAmount = req.TotalNetPayable;
                            objDTBillMain.OrderMethod = req.OrderMethod;
                            if (req.Promobalance > 0)
                            {
                                objDTBillMain.Ispromobaluse = "Y";
                            }
                            else
                            {
                                objDTBillMain.Ispromobaluse = "N";
                            }
                            var objDTBillMainsql = @"insert into TrnPartyOrderMain(GroupId,PGroupId,SOrderNo,OrderNo,PLNo,PLDate,BillNo,BillDate,
                                 OrderBy,PartyName,OrderTo,OrderDate,RefNo,Paymode,chNo,ChDate,ChAmt,BankName,TotalWeight,TotalOrdQty,TotalDispQty,
                                 TotalRemQty,TotalAmount,TotalTaxAmt,RndOff,NetPayable,LastPLDate,Remarks,OType,PLUserId,PLRecTimeStamp,PLUser,IsModify,
                                 PLStatus,MID,Status,ActiveStatus,Version,UserId,RecTimeStamp,FSessId,UserName,IsConfirm,ConfDate,ConfUserID,TotalDiscount,
                                 BankCode,TotalBV,TotalRP,UID,OrderAmount,TotalPV,OrderMethod,Ispromobaluse)
                                 values(@GroupId,@PGroupId,@SOrderNo,@OrderNo,@PLNo,@PLDate,@BillNo,@BillDate,
                                 @OrderBy,@PartyName,@OrderTo,@OrderDate,@RefNo,@Paymode,@chNo,@ChDate,@ChAmt,@BankName,@TotalWeight,@TotalOrdQty,@TotalDispQty,
                                 @TotalRemQty,@TotalAmount,@TotalTaxAmt,@RndOff,@NetPayable,@LastPLDate,@Remarks,@OType,@PLUserId,@PLRecTimeStamp,@PLUser,@IsModify,
                                 @PLStatus,@MID,@Status,@ActiveStatus,@Version,@UserId,@RecTimeStamp,@FSessId,@UserName,@IsConfirm,@ConfDate,@ConfUserID,@TotalDiscount,
                                 @BankCode,@TotalBV,@TotalRP,@UID,@OrderAmount,@TotalPV,@OrderMethod,@Ispromobaluse)";
                            BillMainrowsAffected = await connection.ExecuteAsync(objDTBillMainsql, objDTBillMain);
                        }
                        if (BillMainrowsAffected > 0)
                        {
                            foreach (var item in req.Orderdetail)
                            {
                                var storedaddProcedureName = "sp_AddtoCartnew1";
                                var valuescart = new
                                {
                                    @Action = "deleteCartDeatils",
                                    @Id = item.id
                                };
                                var Rstr = (await connection.QueryAsync<string>(storedaddProcedureName, valuescart, commandType: CommandType.StoredProcedure)).FirstOrDefault();
                            }
                            var storedProcedureName = "SaveFirstOrder";
                            var values = new
                            {
                                @Userid = req.UserId,
                                @OrderNo = req.OrderNo
                            };
                            int i = await connection.ExecuteAsync(storedProcedureName, values, commandType: CommandType.StoredProcedure);
                            objResponse.ResponseMessage = "Saved Successfully!";
                            objResponse.ResponseStatus = "OK";
                            objResponse.StatusCode = 201;
                        }
                        else
                        {
                            objResponse.ResponseMessage = "Order not save";
                            objResponse.ResponseStatus = "Failed";
                            objResponse.StatusCode = 101;
                        }
                    }
                    catch
                    {
                        connection.BeginTransaction().Rollback();
                    }

                }
            }
            catch (Exception ex)
            {
                objResponse.ResponseMessage = "Order not save";
                objResponse.ResponseStatus = "Failed";
                objResponse.StatusCode = 101;
            }
            return objResponse;
        }
        public async Task<decimal> GetPartyWalletBalance(string LoginPartyCode, string vtype)
        {
            decimal WalletBalance = 0;
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    var sql = "Select CrAmt-DrAmt as AcBalance FROM (Select ISNULL(SUM(Amount),0) as CrAmt FROM TrnVoucher WHERE Vtype='" + vtype + "' AND Crto='" + LoginPartyCode + "') a," +
                                "(Select ISNULL(SUM(Amount),0) as DrAmt FROM TrnVoucher WHERE Vtype='" + vtype + "' AND Drto='" + LoginPartyCode + "') b";
                    WalletBalance = (await connection.QueryAsync<decimal>(sql, commandType: CommandType.Text)).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

            }
            return WalletBalance;
        }

        public async Task<int> GetOrderCount(string Fcode)
        {
            int Rstr = 0;
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    var storedProcedureName = "Sp_GetOrderCount";
                    var values = new
                    {
                        @Fcode = Fcode
                    };
                    Rstr = (await connection.QueryAsync<int>(storedProcedureName, values, commandType: CommandType.StoredProcedure)).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

            }
            return Rstr;
        }
        public async Task<string> GetorderMethodSelection(int Userid)
        {
            string Rstr = "";
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    var storedProcedureName = "Sp_GetorderMethodSelection";
                    var values = new
                    {
                        @Userid = Userid
                    };
                    Rstr = (await connection.QueryAsync<string>(storedProcedureName, values, commandType: CommandType.StoredProcedure)).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

            }
            return Rstr;
        }
        public async Task<string> GetOrderNo(string LoginPartyCode)
        {
            string OrderNo = "ORD/";
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    string sql = @"
                                SELECT COALESCE(MAX(CAST(SOrderNo AS INT)), 10000)
                                FROM TrnPartyOrderMain
                               WHERE ActiveStatus = 'Y' AND OrderBy = @LoginPartyCode;";

                    int maxOrderNo = (await connection.QuerySingleOrDefaultAsync<int>(sql, new { LoginPartyCode }));
                    maxOrderNo = maxOrderNo + 1;
                    OrderNo = OrderNo + LoginPartyCode + "/" + maxOrderNo;
                }
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., log the error)
            }
            return OrderNo;
        }

        public async Task<string> SaveOrderMethod(int userid, string Ordermethod)
        {
            string Rstr = "";
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    string sqlprocedure = "SaveOrderMethod";

                    var values = new
                    {
                        @Userid = userid,
                        @OrderMethod = Ordermethod
                    };
                    Rstr = (await connection.QueryAsync<string>(sqlprocedure, values, commandType: CommandType.StoredProcedure)).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

            }
            return Rstr;
        }

        public async Task<int> CheckPackageSelection(string Partycode)
        {
            int Rstr = 0;
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    string sqlprocedure = "Sp_CheckPackageSelection";
                    var values = new
                    {
                        @PartyCode = Partycode
                    };
                    Rstr = (await connection.QueryAsync<int>(sqlprocedure, values, commandType: CommandType.StoredProcedure)).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

            }
            return Rstr;
        }
        public async Task<int> SaveFranchisepackage(string Fcode, int Packageid)
        {
            int rowsAffected = 0;
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    string sqlprocedure = "Sp_SaveFranchisepackage";
                    var values = new
                    {
                        @PackageId = Packageid,
                        @PartyCode = Fcode
                    };
                    rowsAffected = await connection.ExecuteAsync(sqlprocedure, values, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {

            }
            return rowsAffected;
        }

        public async Task<PackageMasterDetail> GetPartyPackageAmount(string Partycode)
        {
            PackageMasterDetail obj = new PackageMasterDetail();
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    var storedProcedureName = "Sp_GetPartyPackageAmount";
                    var values = new
                    {
                        @Partycode = Partycode
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
        public async Task<List<M_SubCatMaster>> GetSubCategories()
        {
            List<M_SubCatMaster> list = new List<M_SubCatMaster>();
            try
            {
                using (var connection = _context.CreateLiveconnInv())
                {
                    var storedProcedureName = "Sp_GetSubCategories";
                    var res = await connection.QueryAsync<M_SubCatMaster>(
                        storedProcedureName,
                        commandType: CommandType.StoredProcedure);

                    if (res != null)
                    {
                        list = res.ToList();
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return list;
        }
    }
}
