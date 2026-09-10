using Shopinv.Models;
using Shopinv.Entity;
using Shopinv.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;

namespace Shopinv.Repoistory
{
    public class R_Product : I_Product
    {
        string MLMSql = ConfigurationManager.ConnectionStrings["sqlMLMConn"].ToString();

        public IEnumerable<E_Product> FliterColorSize(string Action, string Prm)
        {
            IEnumerable<E_Product> lst = new List<E_Product>();
            try
            {
                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();
                DataTable dS;
                hst.Add("Action", Action);
                hst.Add("Prm", Prm);
                dS = blldb.GetDataTable("sp_FliterColorSize", CommandType.StoredProcedure, hst);
                lst = DbOperation.ConvertDataTable<E_Product>(dS);

            }
            catch (Exception ex)
            {

            }
            return lst;
        }
        public IEnumerable<E_Product> DDLProductList()
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            DataTable dt;
            hst.Add("Action", "ShowProductImage");
            dt = blldb.GetDataTable("ShowProduct", CommandType.StoredProcedure, hst);
            IEnumerable<E_Product> lst = DbOperation.ConvertDataTable<E_Product>(dt);
            return lst;

        }
        public IEnumerable<E_ProductDetail> ProductDetail(string ProdId)
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            DataTable dt;
            hst.Add("Action", "ShowImageDetail");
            hst.Add("ProdId", ProdId);
            dt = blldb.GetDataTable("ShowProduct", CommandType.StoredProcedure, hst);
            IEnumerable<E_ProductDetail> lst = DbOperation.ConvertDataTable<E_ProductDetail>(dt);
            return lst;

        }

        public DataSet UpdateCartDetail(string Userid, string idno)
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            DataSet dt;
            // hst.Add("Action", "srchCartDetails");
            // hst.Add("UnqiueId", Unqid);
            hst.Add("userId", Userid);
            hst.Add("idno", idno);
            dt = blldb.GetDataSet("Sp_UpdateCart", CommandType.StoredProcedure, hst);
            // IEnumerable<E_CartDetails> lst = DbOperation.ConvertDataTable<E_CartDetails>(dt);
            return dt;

        }
        public DataSet UpdateClearCart(string Userid, string idno)
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            DataSet dt;
            hst.Add("userId", Userid);
            hst.Add("idno", idno);
            dt = blldb.GetDataSet("Sp_ClearCart", CommandType.StoredProcedure, hst);
            return dt;

        }

        public IEnumerable<E_CartDetails> Cartdetailsftch(string userid)//string Unqid,
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            DataTable dt;
            hst.Add("Action", "srchCartDetails");
            // hst.Add("UnqiueId", Unqid);
            hst.Add("userId", userid);
            dt = blldb.GetDataTable("sp_AddtoCart", CommandType.StoredProcedure, hst);
            IEnumerable<E_CartDetails> lst = DbOperation.ConvertDataTable<E_CartDetails>(dt);
            return lst;

        }

        public IEnumerable<E_CartDetails> Cartdetailsftchouter(string userid)//string Unqid,
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            DataTable dt;
            hst.Add("Action", "srchCartDetails");
            // hst.Add("UnqiueId", Unqid);
            hst.Add("UnqiueId", userid);
            dt = blldb.GetDataTable("sp_AddtoCartouter", CommandType.StoredProcedure, hst);
            IEnumerable<E_CartDetails> lst = DbOperation.ConvertDataTable<E_CartDetails>(dt);
            return lst;

        }

        public string SaveProd(string Action, string ProdId, string ProdName, string Image, string Price, string Bv, string Qty, string myIP, string UnqiueId, string userid, string PV, string Weight, string Color, string SIZE)
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            DataTable dt;
            hst.Add("Action", "AddTocartDeatils");
            hst.Add("productname", ProdName);
            hst.Add("ProdId", ProdId);
            hst.Add("Imgpath", Image);
            hst.Add("price", Price);
            hst.Add("bv", Bv);
            hst.Add("qty", Qty);
            hst.Add("IPAddress", myIP);
            hst.Add("UnqiueId", UnqiueId);
            hst.Add("userId", userid);
            hst.Add("Weight", Weight);
            hst.Add("PV", PV);
            hst.Add("Color", Color);
            hst.Add("SIZE", SIZE);
            dt = blldb.GetDataTable("sp_AddtoCart", CommandType.StoredProcedure, hst);
            //dt = blldb.GetDataTable("sp_AddtoCartnewColor", CommandType.StoredProcedure, hst);
            return dt.Rows[0][0].ToString();

        }

        public string SaveProdouter(string Action, string ProdId, string ProdName, string Image, string Price, string Bv, string Qty, string myIP, string UnqiueId, string userid, string PV, string Weight, string Color, string SIZE)
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            DataTable dt;
            hst.Add("Action", "AddTocartDeatils");
            hst.Add("productname", ProdName);
            hst.Add("ProdId", ProdId);
            hst.Add("Imgpath", Image);
            hst.Add("price", Price);
            hst.Add("bv", Bv);
            hst.Add("qty", Qty);
            hst.Add("IPAddress", myIP);
            hst.Add("UnqiueId", UnqiueId);
            hst.Add("userId", userid);
            hst.Add("Weight", Weight);
            hst.Add("PV", PV);
            hst.Add("Color", Color);
            hst.Add("SIZE", SIZE);
            //dt = blldb.GetDataTable("sp_AddtoCart", CommandType.StoredProcedure, hst);
            dt = blldb.GetDataTable("sp_AddtoCartnew1", CommandType.StoredProcedure, hst);
            return dt.Rows[0][0].ToString();

        }

        public string deleteProd(string Action, string Id, string ProdId, string ProdName, string imagePath, string qty, string Price, string bv, string IpAddress, string posted, string UnqiueId, string userId)
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            DataTable dt;
            hst.Add("Action", "deleteCartDeatils");
            hst.Add("productname", ProdName);
            hst.Add("Id", Id);
            hst.Add("ProdId", ProdId);
            hst.Add("Imgpath", imagePath);
            hst.Add("price", Price);
            hst.Add("bv", bv);
            hst.Add("qty", qty);
            hst.Add("IPAddress", IpAddress);
            hst.Add("UnqiueId", UnqiueId);
            hst.Add("userId", userId);
            dt = blldb.GetDataTable("sp_AddtoCart", CommandType.StoredProcedure, hst);
            return dt.Rows[0][0].ToString();
        }

        //public DataSet SaveStock(DataTable Stock, string myIP, string uniqueId, string userId)
        //{
        //    BLLDBOperations blldb = new BLLDBOperations();
        //    Hashtable hst = new Hashtable();
        //    DataSet ds;
        //    hst.Add("Action", "SaveOrderItems");
        //    hst.Add("userId", userId);
        //    hst.Add("IPAddress", myIP);
        //    hst.Add("UnqiueId", uniqueId);
        //    hst.Add("Stock", Stock);
        //    //hst.Add("Qty", Qty);
        //    ds = blldb.GetDataSet("SaveOrderDetail", CommandType.StoredProcedure, hst);
        //    return ds;

        //}

        public DataSet SaveCheckOutOrder(DataTable Stock, string myIP, string uniqueId, string userId, string IdNo, string FormNo)
        {
            DataSet ds = null;
            try
            {
                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();
                //hst.Add("Action", "SaveOrderItems");
                hst.Add("Action", "SaveOrderItemsofShipping");
                hst.Add("userId", Convert.ToInt32(userId));
                hst.Add("IPAddress", myIP);
                hst.Add("UnqiueId", uniqueId);
                hst.Add("Stock", Stock);
                hst.Add("IdNo", IdNo);
                hst.Add("FormNo", FormNo);
                //hst.Add("Qty", Qty);
                ds = blldb.GetDataSet("SaveOrderDetail", CommandType.StoredProcedure, hst);
                return ds;
            }
            catch (Exception ex)
            {

            }
            return ds;
        }


        public IEnumerable<E_Product> GetSpecialProduct()
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            DataTable dt;
            hst.Add("Action", "ShowSpecialProduct");
            dt = blldb.GetDataTable("SearchProduct", CommandType.StoredProcedure, hst);
            IEnumerable<E_Product> lst = DbOperation.ConvertDataTable<E_Product>(dt);
            return lst;
        }

        public IEnumerable<E_Product> GetFeaturedProduct()
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            DataTable dt;
            hst.Add("Action", "ShowFeaturedProduct");
            dt = blldb.GetDataTable("SearchProduct", CommandType.StoredProcedure, hst);
            IEnumerable<E_Product> lst = DbOperation.ConvertDataTable<E_Product>(dt);
            return lst;
        }
        public IEnumerable<E_Product> GetTopSellerProduct()
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            DataTable dt;
            hst.Add("Action", "ShowTopSellerProduct");
            dt = blldb.GetDataTable("SearchProduct", CommandType.StoredProcedure, hst);
            IEnumerable<E_Product> lst = DbOperation.ConvertDataTable<E_Product>(dt);
            return lst;
        }

        public IEnumerable<E_Product> DealsOfTheWeek()
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            DataTable dt;
            hst.Add("Action", "ShowDealsofWeek");
            dt = blldb.GetDataTable("SearchProduct", CommandType.StoredProcedure, hst);
            IEnumerable<E_Product> lst = DbOperation.ConvertDataTable<E_Product>(dt);
            return lst;
        }
        public IEnumerable<E_CartDetails> updateQuantity(string productId, string Quant, string userid)
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            DataTable dt;
            hst.Add("Action", "updateQuantity");
            hst.Add("ProdId", productId);
            hst.Add("qty", Convert.ToDecimal(Quant));
            hst.Add("userId", userid);
            dt = blldb.GetDataTable("sp_AddtoCart", CommandType.StoredProcedure, hst);
            IEnumerable<E_CartDetails> lst = DbOperation.ConvertDataTable<E_CartDetails>(dt);
            return lst;
        }

        public IEnumerable<E_CartDetails> updateQuantityouter(string productId, string Quant, string userid)
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            DataTable dt;
            hst.Add("Action", "updateQuantity");
            hst.Add("ProdId", productId);
            hst.Add("qty", Convert.ToDecimal(Quant));
            hst.Add("UnqiueId", userid);
            dt = blldb.GetDataTable("sp_AddtoCartouter", CommandType.StoredProcedure, hst);
            IEnumerable<E_CartDetails> lst = DbOperation.ConvertDataTable<E_CartDetails>(dt);
            return lst;
        }
        public DataSet Get_ProductWiseStock(string productid)
        {
            DataSet reds = new DataSet();
            try
            {
                BLLDBOperations bl = new BLLDBOperations();
                Hashtable hs = new Hashtable();
                hs.Add("proid", productid);
                hs.Add("size", "");
                hs.Add("color", "");
                reds = bl.GetDataSet("Sp_ProductWiseStock", CommandType.StoredProcedure, hs);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }
            return reds;
        }
        public DataSet Get_ProductWiseStockWithPartyCode(string productid, string Partycode)
        {
            DataSet reds = new DataSet();
            try
            {
                BLLDBOperations bl = new BLLDBOperations();
                Hashtable hs = new Hashtable();
                hs.Add("proid", productid);
                hs.Add("size", "");
                hs.Add("color", "");
                hs.Add("partycode", Partycode);
                reds = bl.GetDataSet("Sp_ProductWiseStockWithPartCode", CommandType.StoredProcedure, hs);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }
            return reds;
        }
        public IEnumerable<E_Product> GetProductByPriceFilter(string MinPrice, string MaxPrice)
        {
            List<E_Product> srchFilter = new List<E_Product>();
            try
            {
                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();
                DataTable dt;
                hst.Add("Action", "SrchProductByPrice");
                hst.Add("MinPrice", MinPrice);
                hst.Add("MaxPrice", MaxPrice);
                dt = blldb.GetDataTable("SearchFilterType", CommandType.StoredProcedure, hst);
                IEnumerable<E_Product> lst = DbOperation.ConvertDataTable<E_Product>(dt);
                return lst;
            }
            catch (Exception ex)
            {

            }
            return srchFilter;
        }


        public IEnumerable<E_Product> GetProductByBVFilter(string MinBv, string MaxBv)
        {
            List<E_Product> srchFilter = new List<E_Product>();
            try
            {
                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();
                DataTable dt;
                hst.Add("Action", "SrchProductByEP");
                hst.Add("MinBv", MinBv);
                hst.Add("MaxBv", MaxBv);

                dt = blldb.GetDataTable("SearchFilterType", CommandType.StoredProcedure, hst);
                IEnumerable<E_Product> lst = DbOperation.ConvertDataTable<E_Product>(dt);
                return lst;
            }
            catch (Exception ex)
            {

            }
            return srchFilter;
        }

        public DataSet InsertOrderDetail(string regXML)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();
                DataTable dt;
                hst.Add("Action", "SaveOrder");
                hst.Add("regXML", regXML);
                dsReturn = blldb.GetDataSet("Sp_InsertShopingOrder", CommandType.StoredProcedure, hst);
                //dsReturn = blldb.GetDataSet("Sp_InsertShopingOrder", CommandType.StoredProcedure, hst);
                return dsReturn;
            }
            catch (Exception ex)
            {

            }
            return dsReturn;
        }


        public DataSet InserttblTrnOrderWeb(string regXML, decimal Formno, decimal Amount, decimal ordertransId, string orderType, string Idno, string Narration, string BillNo)
        {
            DataSet dsReturn = null;
            try
            {
                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();
                DataTable dt;
                hst.Add("Action", "saveTrnOrderWeb");
                hst.Add("Xmll", regXML);
                hst.Add("Formno", Formno);
                hst.Add("OrderTransId", ordertransId);
                hst.Add("orderType", orderType);
                hst.Add("Idno", Idno);
                hst.Add("Amount", Amount);
                hst.Add("Narration", Narration);
                hst.Add("Wallettype", "R");
                hst.Add("BillNo", BillNo);
                dsReturn = blldb.GetDataSet("Sp_tblTrnOrderWeb", CommandType.StoredProcedure, hst);
                return dsReturn;
            }
            catch (Exception ex)
            {
                var Exception = ex.Message;
            }
            return dsReturn;
        }

        public bool SaveTransactionOrder(int Orderno)
        {
            try
            {
                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();
                hst.Add("orderno", Orderno);

                int rowsAffected = blldb.ExecuteNonQuery("Sp_SaveTrnTransaction", CommandType.StoredProcedure, hst);

                return rowsAffected > 0; //  Return true only if row(s) inserted
            }
            catch (Exception ex)
            {
                // Optional: log exception
                return false;
            }
        }

        public DataSet GetKitActivation(string idNo, string FormNo, string PV, string BV)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();
                DataTable dt;
                hst.Add("Action", "GetIDActive");
                hst.Add("idNo", idNo);
                hst.Add("FormNo", FormNo);
                hst.Add("Pv", PV);
                hst.Add("BV", BV);
                //dsReturn = blldb.GetDataSet("Sp_GetIDActive", CommandType.StoredProcedure, hst);
                dsReturn = blldb.GetDataSet("Sp_GetIDActiveNew", CommandType.StoredProcedure, hst);
            }
            catch (Exception ex)
            {

            }
            return dsReturn;
        }

        public DataSet GetAllparty()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();
                DataTable dt;
                hst.Add("Action", "getPartyName");
                dsReturn = blldb.GetDataSet("Sp_GetAllPartyList", CommandType.StoredProcedure, hst);

            }
            catch (Exception ex)
            {

            }
            return dsReturn;
        }

        public DataSet GetDeliveryCenterAddress(string PartyCode)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();
                DataTable dt;
                hst.Add("Action", "getAddress");
                hst.Add("PartyCode", PartyCode);
                dsReturn = blldb.GetDataSet("Sp_GetAllPartyList", CommandType.StoredProcedure, hst);
            }
            catch (Exception ex)
            {

            }
            return dsReturn;
        }


        public DataSet getCourierCharge()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();
                DataTable dt;
                hst.Add("Action", "getCourierCharge");
                dsReturn = blldb.GetDataSet("Sp_getCourierCharge", CommandType.StoredProcedure, hst);
            }
            catch (Exception ex)
            {

            }
            return dsReturn;
        }



        public DataSet GetProductPrizeList()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();
                DataTable dt;
                hst.Add("Action", "getProdPrizeLst");

                dsReturn = blldb.GetDataSet("sp_productPrizeList", CommandType.StoredProcedure, hst);
            }
            catch (Exception ex)
            {

            }
            return dsReturn;
        }

        public DataSet SavepaymentgetwayTemp(string idNo, string FormNo, string Txnid, string firstname, string email, string phone, string userid, string ShopType, string PartyCode, string regXML, string transid, string Amount, string Status, string DecliveryId, string CourierCharge)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();
                DataTable dt;
                hst.Add("Action", "SavetempOrderData");
                hst.Add("idNo", idNo);
                hst.Add("FormNo", FormNo);
                hst.Add("Txnid", Txnid);
                hst.Add("firstname", firstname);
                hst.Add("email", email);
                hst.Add("phone", phone);
                hst.Add("userid", userid);
                hst.Add("ShopType", ShopType);
                hst.Add("PartyCode", PartyCode);
                hst.Add("regXML", regXML);
                hst.Add("transid", transid);
                hst.Add("Amount", Amount);
                hst.Add("Status", Status);
                hst.Add("DecliveryId", DecliveryId);
                hst.Add("CourierCharge", CourierCharge);

                dsReturn = blldb.GetDataSet("Sp_SavepaymentgetwayTemp1", CommandType.StoredProcedure, hst);
            }
            catch (Exception ex)
            {

            }
            return dsReturn;
        }


        //public DataSet SavePGCashFreeTemp(string idNo, string FormNo, string TxnId, string Email, string MobileNo, string userid, string regXML, decimal Amountdec, string request, string Url, string response, string Flag, string Api, string DecliveryId, string CourierCharge, string partyCode)
        //{
        //    DataSet dsReturn = new DataSet();
        //    try
        //    {

        //        BLLDBOperations blldb = new BLLDBOperations();
        //        Hashtable hst = new Hashtable();
        //        DataTable dt;

        //        hst.Add("Action", "SavePGCashFreeTemp");
        //        hst.Add("idNo", idNo);
        //        hst.Add("FormNo", FormNo);
        //        hst.Add("Txnid", TxnId);
        //        hst.Add("email", Email);
        //        hst.Add("phone", MobileNo);
        //        hst.Add("userid", userid);
        //        hst.Add("regXML", regXML);
        //        hst.Add("Amount", Amountdec);
        //        hst.Add("request", request);
        //        hst.Add("Url", Url);
        //        hst.Add("response", response);
        //        hst.Add("Flag", Flag);
        //        hst.Add("Api", Api);
        //        hst.Add("DecliveryId", DecliveryId);
        //        hst.Add("PartyCode", partyCode);
        //        hst.Add("CourierCharge", CourierCharge);
        //        dsReturn = blldb.GetDataSet("Sp_SavePGCashFreeTemp", CommandType.StoredProcedure, hst);
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return dsReturn;
        //}

        public DataSet SavePGCashFreeTemp(string idNo, string FormNo, string TxnId,
    string Email, string MobileNo, string userid, string regXML, decimal Amountdec,
    string request, string Url, string response, string Flag, string Api, string DecliveryId,
    string CourierCharge, string partyCode, string Paymentimg, string Specialremark)
        {
            DataSet dsReturn = new DataSet();
            try
            {

                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();
                DataTable dt;

                hst.Add("Action", "SavePGCashFreeTemp");
                hst.Add("idNo", idNo);
                hst.Add("FormNo", FormNo);
                hst.Add("Txnid", TxnId);
                hst.Add("email", Email);
                hst.Add("phone", MobileNo);
                hst.Add("userid", userid);
                hst.Add("regXML", regXML);
                hst.Add("Amount", Amountdec);
                hst.Add("request", request);
                hst.Add("Url", Url);
                hst.Add("response", response);
                hst.Add("Flag", Flag);
                hst.Add("Api", Api);
                hst.Add("DecliveryId", DecliveryId);
                hst.Add("PartyCode", partyCode);
                hst.Add("CourierCharge", CourierCharge);
                hst.Add("Paymentimg", Paymentimg);
                hst.Add("Specialremark", Specialremark);
                dsReturn = blldb.GetDataSet("Sp_SavePGCashFreeTemp", CommandType.StoredProcedure, hst);
            }
            catch (Exception ex)
            {

            }
            return dsReturn;
        }

        public DataSet SavePGCashFreeTempApi(string idNo, string FormNo, string TxnId, string Email, string MobileNo, string userid, string regXML, decimal Amountdec, string request, string Url, string response, string Flag, string Api, string DecliveryId, string CourierCharge)
        {
            DataSet dsReturn = new DataSet();
            try
            {

                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();
                DataTable dt;

                hst.Add("Action", "SavePGCashFreeTemp");
                hst.Add("idNo", idNo);
                hst.Add("FormNo", FormNo);
                hst.Add("Txnid", TxnId);
                hst.Add("email", Email);
                hst.Add("phone", MobileNo);
                hst.Add("userid", userid);
                hst.Add("regXML", regXML);
                hst.Add("Amount", Amountdec);
                hst.Add("request", request);
                hst.Add("Url", Url);
                hst.Add("response", response);
                hst.Add("Flag", Flag);
                hst.Add("Api", Api);
                hst.Add("DecliveryId", "");
                hst.Add("CourierCharge", 0);
                dsReturn = blldb.GetDataSet("Sp_SavePGCashFreeTempApi", CommandType.StoredProcedure, hst);
            }
            catch (Exception ex)
            {

            }
            return dsReturn;
        }

        public DataSet GetTransId(string Orderid)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();
                DataTable dt;
                hst.Add("Action", "GetTransId");
                hst.Add("OrderID", Orderid);

                dsReturn = blldb.GetDataSet("Sp_SavepaymentgetwayTemp", CommandType.StoredProcedure, hst);
            }
            catch (Exception ex)
            {

            }
            return dsReturn;
        }

        public DataSet GetTransIdApi(string Orderid)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();
                DataTable dt;
                hst.Add("Action", "GetTransIdAPI");
                hst.Add("OrderID", Orderid);

                dsReturn = blldb.GetDataSet("Sp_SavepaymentgetwayTemp", CommandType.StoredProcedure, hst);
            }
            catch (Exception ex)
            {

            }
            return dsReturn;
        }

        public DataSet UpdateStatus(string Status, string Orderid)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();
                DataTable dt;
                hst.Add("Action", "UpdateStatus");
                hst.Add("OrderID", Orderid);
                hst.Add("Status", Status);
                dsReturn = blldb.GetDataSet("Sp_SavepaymentgetwayTemp1", CommandType.StoredProcedure, hst);
            }
            catch (Exception ex)
            {

            }
            return dsReturn;
        }

        public DataSet UpdatePaymentOrderId(string orderid, string txnid)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();
                DataTable dt;
                hst.Add("Action", "UpdateOrderId");
                hst.Add("OrderID", orderid);
                hst.Add("txnid", txnid);
                dsReturn = blldb.GetDataSet("Sp_SavepaymentgetwayTemp1", CommandType.StoredProcedure, hst);
            }
            catch (Exception ex)
            {

            }
            return dsReturn;
        }

        public DataSet GetProdAvailable(string ProdId)
        {
            DataSet dsreturn = new DataSet();
            try
            {
                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();
                DataSet dS;
                hst.Add("Action", "GetProdcat");
                hst.Add("ProdId", ProdId);
                dS = blldb.GetDataSet("getAlsoAvailableProd", CommandType.StoredProcedure, hst);
                //IEnumerable<E_ProductDetail> lst = DbOperation.ConvertDataTable<E_ProductDetail>(dt);
                return dS;
            }
            catch (Exception ex)
            {

            }
            return dsreturn;
        }

        public DataSet CheckStatus(string idNo)
        {
            DataSet dsreturn = null;
            try
            {
                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();
                DataSet dS;
                hst.Add("Action", "CheckSts");
                hst.Add("Idno", idNo);
                dsreturn = blldb.GetDataSet("Sp_UpdateCheckActiveStatus", CommandType.StoredProcedure, hst);
                //dsreturn = blldb.GetDataSet("Sp_CheckActiveStatus", CommandType.StoredProcedure, hst);
                //IEnumerable<E_ProductDetail> lst = DbOperation.ConvertDataTable<E_ProductDetail>(dt);
                return dsreturn;
            }
            catch (Exception ex)
            {

            }
            return dsreturn;
        }

        public IEnumerable<E_ProductDetail> SearchProductname(string Terms)
        {
            IEnumerable<E_ProductDetail> lst = new List<E_ProductDetail>();
            try
            {
                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();
                DataTable dS;
                hst.Add("term", Terms);
                dS = blldb.GetDataTable("Sp_GetProductname", CommandType.StoredProcedure, hst);
                //hst.Add("Action", "ShowProductImage");
                //dS = blldb.GetDataTable("ShowProduct", CommandType.StoredProcedure, hst);
                lst = DbOperation.ConvertDataTable<E_ProductDetail>(dS);

            }
            catch (Exception ex)
            {

            }
            return lst;
        }
        public DataSet Get_ColorSizeimgaeBYid(string Action, string ProdId, string colorid, string Colorname)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();
                DataSet dS;
                hst.Add("Action", Action);
                hst.Add("id", ProdId);
                hst.Add("colorid", colorid);

                hst.Add("Colorname", Colorname);
                dS = blldb.GetDataSet("Sp_Get_ColorSizeimgaeBYidnew", CommandType.StoredProcedure, hst);
                if (dS.Tables.Count > 0)
                {
                    dsReturn = dS;
                }
            }
            catch (Exception ex)
            {

            }
            return dsReturn;
        }
        public DataSet GetCompanydetail()
        {
            DataSet dsReturn = new DataSet();
            try
            {
                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();
                DataSet dS;
                dS = blldb.GetDataSet("Sp_GetCompanydetail", CommandType.StoredProcedure, hst);
                if (dS.Tables.Count > 0)
                {
                    dsReturn = dS;
                }
            }
            catch (Exception ex)
            {

            }
            return dsReturn;
        }
        public DataTable GetOrderStatus(string formno, string orderid)
        {
            DataTable dsReturn = new DataTable();
            try
            {
                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();
                hst.Add("@Formno", formno);
                hst.Add("@OrderTransId", orderid);
                dsReturn = blldb.GetDataTable("sp_checkOrderStatus", CommandType.StoredProcedure, hst);
            }
            catch (Exception ex)
            {

            }
            return dsReturn;
        }

        public DataTable GetIDStatus(string IDno)
        {
            DataTable dsReturn = new DataTable();
            try
            {
                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();
                hst.Add("idno", IDno);
                dsReturn = blldb.GetDataTable("Sp_GetIDStatus", CommandType.StoredProcedure, hst);
            }
            catch (Exception ex)
            {

            }
            return dsReturn;
        }
        public DataTable CouponDetail(string Formno, string CouponNo, decimal totalamount, decimal totalbv)
        {

            DataTable dsReturn = new DataTable();
            try
            {
                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();
                hst.Add("Formno", Formno);
                hst.Add("CouponNo", CouponNo);
                hst.Add("TotalAmount", totalamount);
                hst.Add("TotalBv", totalbv);
                dsReturn = blldb.GetDataTable("sp_getCouponNew", CommandType.StoredProcedure, hst);

            }
            catch (Exception ex)
            {

            }
            return dsReturn;

        }
        public DataSet GetCoupon(string Formno)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();
                hst.Add("Fomrno", Formno);
                DataSet dS;
                dS = blldb.GetDataSet("Sp_GetCoupon", CommandType.StoredProcedure, hst);
                if (dS.Tables.Count > 0)
                {
                    dsReturn = dS;
                }
            }
            catch (Exception ex)
            {

            }
            return dsReturn;
        }

        public DataTable SaveAarogyaidactivationLog(string idno, string Request, string Response)
        {
            DataTable dsReturn = new DataTable();
            try
            {
                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();
                hst.Add("Request", Request);
                hst.Add("Response", Response);
                hst.Add("Idno", idno);
                dsReturn = blldb.GetDataTable("Sp_SaveAarogyaidactivationLog", CommandType.StoredProcedure, hst);
            }
            catch (Exception ex)
            {

            }
            return dsReturn;
        }


        public List<M_Franchisetype> GetFranchisetype()
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            DataTable dt = blldb.GetDataTable("Sp_GetallFranchisetype", CommandType.StoredProcedure, hst);

            List<M_Franchisetype> franchiseTypes = DbOperation.ConvertDataTable<M_Franchisetype>(dt);
            return franchiseTypes;
        }

        public List<M_Franchiselist> GetFranchiseList(int FranchiseType)
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            hst.Add("FranchiseType", FranchiseType);
            DataTable dt = blldb.GetDataTable("Sp_GetFranchiseList", CommandType.StoredProcedure, hst);

            List<M_Franchiselist> franchiseTypes = DbOperation.ConvertDataTable<M_Franchiselist>(dt);
            return franchiseTypes;
        }

        public List<M_GetWallettype> GetWallettype()
        {
            BLLDBOperations blldb = new BLLDBOperations(MLMSql);
            DataTable dt = blldb.GetDataTable("sp_GetWallettype", CommandType.StoredProcedure);
            List<M_GetWallettype> walltes = DbOperation.ConvertDataTable<M_GetWallettype>(dt);
            return walltes;
        }
        public List<WalletUseDetail> GetWallettypeBalance(string Formno, string WalletType)
        {
            BLLDBOperations blldb = new BLLDBOperations(MLMSql);
            DataTable dt = blldb.GetDataTable("Select * From dbo.ufnGetBalance('" + Formno + "','" + WalletType + "')", CommandType.Text);
            List<WalletUseDetail> walltes = DbOperation.ConvertDataTable<WalletUseDetail>(dt);
            return walltes;
        }

        public List<AllWalletDetail> GetAllWalletDetail(string Formno, string WalletType)
        {
            BLLDBOperations blldb = new BLLDBOperations(MLMSql);
            Hashtable hst = new Hashtable();
            hst.Add("FormNo", Formno);
            hst.Add("WalletType", WalletType);
            DataTable dt = blldb.GetDataTable("Sp_GetAllWalletDetail", CommandType.StoredProcedure, hst);
            List<AllWalletDetail> walletdetails = DbOperation.ConvertDataTable<AllWalletDetail>(dt);
            return walletdetails;
        }

        public List<M_Level> GetLevel(string Formno, string Type)
        {
            BLLDBOperations blldb = new BLLDBOperations(MLMSql);
            Hashtable hst = new Hashtable();
            hst.Add("FormNo", Formno);
            hst.Add("type", Type);
            DataTable dt = blldb.GetDataTable("sp_GetLevel ", CommandType.StoredProcedure, hst);
            //foreach (DataColumn column in dt.Columns)
            //{
            //    var dd = column.DataType;
            //}
            List<M_Level> level = DbOperation.ConvertDataTable<M_Level>(dt);
            return level;
        }
        public List<ReferalDownlinein> GetReferalDownlineinfonew(string Formno)
        {
            BLLDBOperations blldb = new BLLDBOperations(MLMSql);
            DataTable dt = blldb.GetDataTable("Select * from V#ReferalDownlineinfonew where Formno=" + Formno + " ", CommandType.Text);
            List<ReferalDownlinein> level = DbOperation.ConvertDataTable<ReferalDownlinein>(dt);
            return level;
        }

        public DataSet sp_GetLevelDetail(string MLevel, string Legno, string ActiveStatus, string @FormNo, int PageIndex, int PageSize)
        {
            BLLDBOperations blldb = new BLLDBOperations(MLMSql);
            Hashtable hst = new Hashtable();
            hst.Add("MLevel", MLevel);
            hst.Add("Legno", Legno);
            hst.Add("ActiveStatus", ActiveStatus);
            hst.Add("FormNo", FormNo);
            hst.Add("PageIndex", PageIndex);
            hst.Add("PageSize", PageSize);
            hst.Add("RecordCount", 0);
            DataSet dt = blldb.GetDataSet("sp_GetLevelDetail ", CommandType.StoredProcedure, hst);
            return dt;
        }

        public DataSet GetLevelIncome(int FormNo)
        {
            BLLDBOperations blldb = new BLLDBOperations(MLMSql);
            Hashtable hst = new Hashtable();
            hst.Add("FormNo", FormNo);
            DataSet dt = blldb.GetDataSet("sp_GetLevelIncome ", CommandType.StoredProcedure, hst);
            return dt;
        }
        public IEnumerable<E_ProductReview> GetProductReview(int Productcode)
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            DataTable dt;
            hst.Add("Productcode", Productcode);
            dt = blldb.GetDataTable("Sp_GetProductReview", CommandType.StoredProcedure, hst);
            IEnumerable<E_ProductReview> lst = DbOperation.ConvertDataTable<E_ProductReview>(dt);
            return lst;
        }

        public DataSet SaveReview(string ReviewMessage, string ReviewRating,
            string ReviewName, string Productcode, int Formno)
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            hst.Add("Formno", Formno);
            hst.Add("ReviewMessage", ReviewMessage);
            hst.Add("ReviewRating", ReviewRating);
            hst.Add("ReviewName", ReviewName);
            hst.Add("Productcode", Productcode);
            DataSet dt = blldb.GetDataSet("SP_SaveProductReview ", CommandType.StoredProcedure, hst);
            return dt;
        }
        public DataSet SaveShoppingWishlist(int UserID, int ProductID)
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            hst.Add("Action", "Save");
            hst.Add("UserID", UserID);
            hst.Add("ProductID", ProductID);
            DataSet dt = blldb.GetDataSet("Sp_SaveShoppingWishlist ", CommandType.StoredProcedure, hst);
            return dt;
        }

        public DataSet CheckProductwiseWishlist(int UserID, int ProductID)
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            hst.Add("Action", "CheckProductwise");
            hst.Add("UserID", UserID);
            hst.Add("ProductID", ProductID);
            DataSet dt = blldb.GetDataSet("Sp_SaveShoppingWishlist ", CommandType.StoredProcedure, hst);
            return dt;
        }

        public IEnumerable<E_CartDetails> CheckUserwiseWishlist(int UserID)
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            hst.Add("Action", "CheckUserwise");
            hst.Add("UserID", UserID);
            hst.Add("ProductID", 0);
            DataTable dt = blldb.GetDataTable("Sp_SaveShoppingWishlist", CommandType.StoredProcedure, hst);
            IEnumerable<E_CartDetails> lst = DbOperation.ConvertDataTable<E_CartDetails>(dt);
            return lst;
        }
        public DataSet CheckTxno(string Txno)
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            hst.Add("Txno", Txno);
            DataSet dt = blldb.GetDataSet("sp_CheckTxno", CommandType.StoredProcedure, hst);
            return dt;
        }
        public IEnumerable<E_ProductReview> GetTopProductReview()
        {
            BLLDBOperations blldb = new BLLDBOperations();
            DataTable dt;
            dt = blldb.GetDataTable("Sp_GetTopProductReview", CommandType.StoredProcedure);
            IEnumerable<E_ProductReview> lst = DbOperation.ConvertDataTable<E_ProductReview>(dt);
            return lst;
        }
        public DataSet CheckSponsor(string id)
        {
            BLLDBOperations blldb = new BLLDBOperations(MLMSql);
            Hashtable hst = new Hashtable();
            hst.Add("SponsorID", id);
            DataSet dt = blldb.GetDataSet("Sp_CheckSponsor", CommandType.StoredProcedure, hst);
            return dt;
        }
        public DataSet SaveMember(RegisterUser req)
        {
            BLLDBOperations blldb = new BLLDBOperations(MLMSql);
            Hashtable hst = new Hashtable();
            hst.Add("Sponsorformno", req.referralid);
            hst.Add("name", req.name);
            hst.Add("mobl", req.mobl);
            hst.Add("passw", req.Password);
            hst.Add("email", req.email);
            DataSet dt = blldb.GetDataSet("SV_SaveMember ", CommandType.StoredProcedure, hst);
            return dt;
        }

        public List<BankList> GetbankLists()
        {
            BLLDBOperations blldb = new BLLDBOperations(MLMSql);
            DataTable dt = blldb.GetDataTable("sp_GetBankMaster", CommandType.StoredProcedure);
            //foreach (DataColumn column in dt.Columns)
            //{
            //    var dd = column.DataType;
            //}
            List<BankList> banks = DbOperation.ConvertDataTable<BankList>(dt);
            return banks;
        }
        public List<KycTypeMaster> kycTypeMasters()
        {
            BLLDBOperations blldb = new BLLDBOperations(MLMSql);
            DataTable dt = blldb.GetDataTable("SELECT Id,IdType  FROM M_IdTypeMaster WHERE ACTIVESTATUS='Y'", CommandType.Text);
            List<KycTypeMaster> level = DbOperation.ConvertDataTable<KycTypeMaster>(dt);
            return level;
        }
        public DataSet Checkordercount(int Formno)
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            hst.Add("Formno", Formno);
            DataSet dt = blldb.GetDataSet("Sp_Checkordercount ", CommandType.StoredProcedure, hst);
            return dt;
        }
        public DataSet CheckKitOnPurchase(int Formno, decimal TotalAmount)
        {
            BLLDBOperations blldb = new BLLDBOperations(MLMSql);
            Hashtable hst = new Hashtable();
            hst.Add("UserID", Formno);
            hst.Add("CurrentOrderAmount", TotalAmount);
            DataSet dt = blldb.GetDataSet("sp_UpdateKitOnPurchase ", CommandType.StoredProcedure, hst);
            return dt;
        }
        public DataSet SaveRazarpayTemp(string idNo, string FormNo, string TxnId,
          string Email, string MobileNo, string userid, string regXML, decimal Amountdec,
          string request, string Url, string response, string Flag, string Api, string DecliveryId,
          string CourierCharge, string partyCode, string Paymentimg, string PgTxnid,string Bvapiurl,string Mememode)
        {
            DataSet dsReturn = new DataSet();
            try
            {

                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();
                DataTable dt;
                hst.Add("Action", "SavePGCashFreeTemp");
                hst.Add("idNo", idNo);
                hst.Add("FormNo", FormNo);
                hst.Add("Txnid", TxnId);
                hst.Add("email", Email);
                hst.Add("phone", MobileNo);
                hst.Add("userid", userid);
                hst.Add("regXML", regXML);
                hst.Add("Amount", Amountdec);
                hst.Add("request", request);
                hst.Add("Url", Url);
                hst.Add("response", response);
                hst.Add("Flag", Flag);
                hst.Add("Api", Api);
                hst.Add("DecliveryId", DecliveryId);
                hst.Add("PartyCode", partyCode);
                hst.Add("CourierCharge", CourierCharge);
                hst.Add("Bvapiurl", Bvapiurl); 
                hst.Add("Mememode", Mememode); 
                dsReturn = blldb.GetDataSet("Sp_SaveRazarpayTemp", CommandType.StoredProcedure, hst);
            }
            catch (Exception ex)
            {

            }
            return dsReturn;
        }

        public  DataSet GetOrderbyPgTxnid(string PgTxnid)
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            hst.Add("PgTxnid", PgTxnid);
            DataSet dt = blldb.GetDataSet("Sp_GetOrderbyPgTxnid", CommandType.StoredProcedure, hst);
            return dt;
        }
        public DataSet RejectPGOrder(string OrderNo, string RejectReason)
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            hst.Add("UtrNo", OrderNo);
            hst.Add("RejectReason", RejectReason);
            DataSet dt = blldb.GetDataSet("REJECTOFFLINEORDER", CommandType.StoredProcedure, hst);
            return dt;
        }

        public DataSet UpdateKitOnPurchaseUpdate(int Formno, decimal TotalAmount, string BillType)
        {
            BLLDBOperations blldb = new BLLDBOperations(MLMSql);
            Hashtable hst = new Hashtable();
            hst.Add("UserID", Formno);
            hst.Add("CurrentOrderAmount", TotalAmount);
            hst.Add("BillType", BillType);
            DataSet dt = blldb.GetDataSet("sp_UpdateKitOnPurchaseUpdate", CommandType.StoredProcedure, hst);
            return dt;
        }
    }
}