using Shopinv.Entity;
using Shopinv.Interface;
using Shopinv.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace Shopinv.Repoistory
{
    public class R_OrderReport : I_OrderReport
    {
        public IEnumerable<E_OrderReport> GetOrderdetail(string UserId, string FormNo)
        //public DataSet  GetOrderdetail(string UserId, string FormNo)
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            DataTable dt;
            DataSet ds = null;
            //hst.Add("Action", "GetOrderSummery");
            hst.Add("Action", "GetOrderSummeryDetail");
            hst.Add("userId", UserId);
            hst.Add("FormNo", FormNo);
            dt = blldb.GetDataTable("Sp_GetOrderHistoryNew_test", CommandType.StoredProcedure, hst);
            // ds = blldb.GetDataSet("SaveOrderDetail", CommandType.StoredProcedure, hst);
            //return ds;
            IEnumerable<E_OrderReport> lst = DbOperation.ConvertDataTable<E_OrderReport>(dt);
            return lst;
        }

        public IEnumerable<E_OrderReport> GetGrdOrderNoDetail(string OrderId, string userid)
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            DataTable dt;
            hst.Add("Action", "GetOrderNoDetail");
            hst.Add("userId", userid);
            hst.Add("OrderNo", OrderId);
            dt = blldb.GetDataTable("Sp_GetOrderHistory1", CommandType.StoredProcedure, hst);
            IEnumerable<E_OrderReport> lst = DbOperation.ConvertDataTable<E_OrderReport>(dt);
            return lst;
        }

        public IEnumerable<E_PendingTrans> GetPendingOrder()
        {
            DataTable dsReturn = new DataTable();

            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            DataTable dt; ;
            hst.Add("Action", "GetPendingOrder");
            //hst.Add("OrderNo", OrderId);
            //hst.Add("userId", userid);
            dsReturn = blldb.GetDataTable("Sp_SavepaymentgetwayTemp1", CommandType.StoredProcedure, hst);
            IEnumerable<E_PendingTrans> lst = DbOperation.ConvertDataTable<E_PendingTrans>(dsReturn);
            return lst;
        }

        public DataSet GetPendingOrderDetail(string txnid, string Formno)
        {
            DataSet dsreturn = new DataSet();
            try
            {
                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();
                DataSet ds;
                hst.Add("Action", "pendingOrder");
                hst.Add("txnid", txnid);
                hst.Add("Formno", Formno);
                ds = blldb.GetDataSet("sp_PendingOrder", CommandType.StoredProcedure, hst);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dsreturn = ds;
                }
            }
            catch (Exception ex)
            {

            }
            return dsreturn;
        }

        public DataSet UpdateNewTxnid(string Formno, string txnid, string newtxnid, string newtransid)
        {
            DataSet dsreturn = new DataSet();
            try
            {
                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();
                DataSet ds;
                hst.Add("Action", "Updatetxnid");
                hst.Add("Formno", Formno);
                hst.Add("txnid", txnid);
                hst.Add("newtxnid", newtxnid);
                hst.Add("newtransid", newtransid);
                ds = blldb.GetDataSet("sp_PendingOrder", CommandType.StoredProcedure, hst);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dsreturn = ds;
                }
            }
            catch (Exception ex)
            {

            }
            return dsreturn;
        }

        public IEnumerable<E_CouponDetail> GetCoupondetailNew(string UserId, string FormNo)
        //public DataSet  GetOrderdetail(string UserId, string FormNo)
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            DataTable dt;
            DataSet ds = null;
            hst.Add("userId", FormNo);
            dt = blldb.GetDataTable("Sp_GetCouponDetail", CommandType.StoredProcedure, hst);
            // ds = blldb.GetDataSet("SaveOrderDetail", CommandType.StoredProcedure, hst);
            //return ds;
            IEnumerable<E_CouponDetail> lst = DbOperation.ConvertDataTable<E_CouponDetail>(dt);
            return lst;
        }

        public IEnumerable<E_OrderReport> GetOfflineOrderdetail(string Formno)
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            DataTable dt;
            hst.Add("FormNo", Formno);
            dt = blldb.GetDataTable("Sp_GetOfflineOrders", CommandType.StoredProcedure, hst);
            IEnumerable<E_OrderReport> lst = DbOperation.ConvertDataTable<E_OrderReport>(dt);
            return lst;
        }

        public IEnumerable<E_OrderReport> Showofflineorderdetail(string OrderId, string userid)
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            DataTable dt;
            hst.Add("id", OrderId);
            dt = blldb.GetDataTable("Sp_GetOfflineOrdersDetail", CommandType.StoredProcedure, hst);
            IEnumerable<E_OrderReport> lst = DbOperation.ConvertDataTable<E_OrderReport>(dt);
            return lst;
        }
    }
}