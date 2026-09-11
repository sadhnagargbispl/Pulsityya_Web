using System;
using System.Collections;
using System.Data;
using Shopinv.Interface;
using Shopinv.Models;

namespace Shopinv.Repository
{
    public class HubbleSSORepository : IHubbleSSORepository
    {
        private readonly BLLDBOperations _blldb;

        public HubbleSSORepository()
        {
            _blldb = new BLLDBOperations();
        }

        // ----------------------------------------------------
        // SAVE TOKEN (SSO)
        // ----------------------------------------------------
        public void SaveToken(string token, string userId)
        {
            try
            {
                Hashtable hst = new Hashtable();

                hst.Add("Action", "SaveHubbleToken");
                hst.Add("Token", token);
                hst.Add("UserId", userId);

                _blldb.GetDataTable(
                    "sp_HubbleSSO",
                    CommandType.StoredProcedure,
                    hst);
            }
            catch (Exception ex)
            {
                throw new Exception("SaveToken Error: " + ex.Message);
            }
        }


        // ----------------------------------------------------
        // GET USER BY TOKEN (SSO AUTH)
        // ----------------------------------------------------
        public HubbleSSOUserModel GetUserByToken(string token)
        {
            try
            {
                Hashtable hst = new Hashtable();

                hst.Add("Action", "GetUserByToken");
                hst.Add("Token", token);

                DataTable dt = _blldb.GetDataTable(
                    "sp_HubbleSSO",
                    CommandType.StoredProcedure,
                    hst);

                if (dt == null || dt.Rows.Count == 0)
                    return null;

                DataRow row = dt.Rows[0];

                return new HubbleSSOUserModel
                {
                    UserId = row["UserId"].ToString(),
                    Email = row["Email"].ToString(),
                    FirstName = row["FirstName"].ToString(),
                    LastName = row["LastName"].ToString(),
                    PhoneNumber = row["PhoneNumber"].ToString()
                };
            }
            catch (Exception ex)
            {
                throw new Exception("GetUserByToken Error: " + ex.Message);
            }
        }


        // ----------------------------------------------------
        // GET USER BALANCE (Coins Balance API)
        // ----------------------------------------------------
        public UserModel GetUserByUserId(string userId)
        {
            try
            {
                Hashtable hst = new Hashtable();

                hst.Add("Action", "GetCoinBalance");
                hst.Add("UserId", userId);

                DataTable dt = _blldb.GetDataTable(
                    "sp_HubbleSSO",
                    CommandType.StoredProcedure,
                    hst);

                if (dt == null || dt.Rows.Count == 0)
                    return null;

                DataRow row = dt.Rows[0];

                return new UserModel
                {
                    UserId = row["UserId"].ToString(),
                    CoinBalance = Convert.ToDecimal(row["CoinBalance"])
                };
            }
            catch (Exception ex)
            {
                throw new Exception("GetUserByUserId Error: " + ex.Message);
            }
        }


        // ----------------------------------------------------
        // DEBIT COINS API
        // ----------------------------------------------------
        public DataTable DebitCoins(
    string userId,
    decimal coins,
    string referenceId,
    string note)
        {
            try
            {
                Hashtable hst = new Hashtable();

                hst.Add("Action", "DebitCoins");
                hst.Add("UserId", userId);
                hst.Add("Coins", coins);
                hst.Add("ReferenceId", referenceId);
                hst.Add("Note", note);

                return _blldb.GetDataTable(
                    "sp_HubbleDebitCoins",
                    CommandType.StoredProcedure,
                    hst);
            }
            catch (Exception ex)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Status");
                dt.Columns.Add("TransactionId");
                dt.Columns.Add("Balance");
                dt.Columns.Add("ReferenceId");

                dt.Rows.Add("FAILED", "", 0, "");

                return dt;
            }
        }
        public DataTable ReverseCoins(
    string userId,
    string referenceId,
    string note)
        {
            try
            {
                Hashtable hst = new Hashtable();

                hst.Add("Action", "ReverseCoins");
                hst.Add("UserId", userId);
                hst.Add("ReferenceId", referenceId);
                hst.Add("Note", note);

                return _blldb.GetDataTable(
                    "sp_HubbleDebitCoins",
                    CommandType.StoredProcedure,
                    hst);
            }
            catch (Exception ex)
            {
                DataTable dt = new DataTable();

                dt.Columns.Add("Status");
                dt.Columns.Add("TransactionId");
                dt.Columns.Add("Balance");
                dt.Columns.Add("ReferenceId");

                dt.Rows.Add("FAILED", "", 0, "");

                return dt;
            }
        }
        //public object DebitCoins(
        //    string userId,
        //    decimal coins,
        //    string referenceId,
        //    string note)
        //{
        //    try
        //    {
        //        Hashtable hst = new Hashtable();

        //        hst.Add("Action", "DebitCoins");
        //        hst.Add("UserId", userId);
        //        hst.Add("Coins", coins);
        //        hst.Add("ReferenceId", referenceId);
        //        hst.Add("Note", note);

        //        return _blldb.GetDataTable(
        //            "sp_HubbleDebitCoins",
        //            CommandType.StoredProcedure,
        //            hst);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new
        //        {
        //            status = "FAILED",
        //            message = ex.Message
        //        };
        //    }
        //}


        // ----------------------------------------------------
        // REVERSE COINS API
        // ----------------------------------------------------
        //public DataTable ReverseCoins(
        //    string userId,
        //    string referenceId,
        //    string note)
        //{
        //    try
        //    {
        //        Hashtable hst = new Hashtable();

        //        hst.Add("Action", "ReverseCoins");
        //        hst.Add("UserId", userId);
        //        hst.Add("ReferenceId", referenceId);
        //        hst.Add("Note", note);

        //        return _blldb.GetDataTable(
        //            "sp_HubbleDebitCoins",
        //            CommandType.StoredProcedure,
        //            hst);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new
        //        {
        //            status = "FAILED",
        //            message = ex.Message
        //        };
        //    }
        //}


        // ----------------------------------------------------
        // MARK TOKEN USED
        // ----------------------------------------------------
        public void MarkTokenUsed(string token)
        {
            try
            {
                Hashtable hst = new Hashtable();

                hst.Add("Action", "MarkTokenUsed");
                hst.Add("Token", token);

                _blldb.GetDataTable(
                    "sp_HubbleSSO",
                    CommandType.StoredProcedure,
                    hst);
            }
            catch (Exception ex)
            {
                throw new Exception("MarkTokenUsed Error: " + ex.Message);
            }
        }
    }
}