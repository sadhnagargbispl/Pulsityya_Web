using Shopinv.Entity;
using Shopinv.Interface;
using Shopinv.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace Shopinv.Repoistory
{
    public class R_PayMode :I_PayMode
    {
        public IEnumerable<E_Payment> GetBalence(string walletType, string FormNo)
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            DataTable dt;
            hst.Add("Action", "GetBalence");
            hst.Add("walletType", walletType);
            hst.Add("FormNo", FormNo);
            dt = blldb.GetDataTable("sp_GetBalence", CommandType.StoredProcedure, hst);
            IEnumerable<E_Payment> lst = DbOperation.ConvertDataTable<E_Payment>(dt);
            return lst;
        }

        public string TRNVoucherDebit(string FormNo, string Amount, string walletType, string IdNo, string OrderID)
        {
            string s = "";
            try
            {
                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();
                DataTable dt;
                hst.Add("Action", "SaveInTRNVoucher");
                hst.Add("WalletType", walletType);
                hst.Add("FormNo", FormNo);
                hst.Add("Amount", Amount);
                hst.Add("IdNo", IdNo);
                hst.Add("RefNo", OrderID);
                dt = blldb.GetDataTable("SaveOrderDetail1", CommandType.StoredProcedure, hst);
                return dt.Rows[0][0].ToString();
            }
             catch(Exception ex)
            {

            }
            return s;
        }

         public DataSet GetPendinOrderDetail(string IDno, string formno)
        {
            DataSet dsreturn = null;
            try
            {
                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();
                DataSet dt;
                hst.Add("Action", "GetPendinOrderDetail");
                hst.Add("IDno", IDno);
                hst.Add("FormNo", formno);
                dt = blldb.GetDataSet("Sp_GetPendinOrderDetail", CommandType.StoredProcedure, hst);
                dsreturn = dt;


            }
             catch(Exception ex)
            {

            }
            return dsreturn;
        }

         public  DataSet GetpaygatwayOrderDetail(string TxnID)
        {
            DataSet dsreturn = null;
            try
            {
                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();
                DataSet dt;
                hst.Add("Action", "GetpaygatwayOrderDetail");
                hst.Add("TxnID", TxnID);
                dt = blldb.GetDataSet("Sp_GetPendinOrderDetail", CommandType.StoredProcedure, hst);
                dsreturn = dt;

            }
            catch (Exception ex)
            {

            }
            return dsreturn;
        }


        public DataSet SaveOnlineres(string orderid, string res)
        {
            DataSet dsreturn = null;
            try
            {
                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();
                DataSet dt;
                hst.Add("Response", res);
                hst.Add("Orderid", orderid);
                dt = blldb.GetDataSet("sp_saveresponse", CommandType.StoredProcedure, hst);
                dsreturn = dt;
            }
             catch(Exception ex)
            {

            }
            return dsreturn;
        }

    }
}