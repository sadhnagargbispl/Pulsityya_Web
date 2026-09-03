using Shopinv.Interface;
using Shopinv.Entity;
using Shopinv.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace Shopinv.Repoistory
{
    public class R_register : I_register 
    {

        public IEnumerable<M_Register> Register(string Action, string firstName, string LastName, string E_Mail, string Password)
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            DataTable dt;
            hst.Add("Action", "RegisterDetails");
            hst.Add("firstName", firstName);
            hst.Add("LastName", LastName);
            hst.Add("E_Mail", E_Mail);
            hst.Add("Password", Password);

            dt = blldb.GetDataTable("UserDetails", CommandType.StoredProcedure, hst);
            IEnumerable<M_Register> lst = DbOperation.ConvertDataTable<M_Register>(dt);
            return lst;
        }
        public DataSet SaveRegistration(E_sregistration obj)
        {
            DataSet ds = null;
            try
            {
                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();
                DataSet dt;

                hst.Add("name", obj.name);
                hst.Add("EMail", obj.email);
                hst.Add("Mobl", obj.mobl);
                hst.Add("Password", obj.password);
                dt = blldb.GetDataSet("Sp_registration", CommandType.StoredProcedure, hst);
                ds = dt;

            }
            catch (Exception ex)
            {

            }
            return ds;
        }
        public DataSet SaveOTP(string OTP, string Email)
        {
            DataSet ds = null;
            try
            {
                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();
                DataSet dt;

                hst.Add("OTP", OTP);
                hst.Add("Email", Email);

                dt = blldb.GetDataSet("Sp_OTP", CommandType.StoredProcedure, hst);
                ds = dt;

            }
            catch (Exception ex)
            {

            }
            return ds;
        }
        public DataSet GetOTP(string Email)
        {
            DataSet ds = null;
            try
            {
                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();
                DataSet dt;
                hst.Add("Email", Email);
                dt = blldb.GetDataSet("Sp_GetOTP", CommandType.StoredProcedure, hst);
                ds = dt;

            }
            catch (Exception ex)
            {

            }
            return ds;
        }

        public DataSet getemail(string Email)
        {
            DataSet ds = null;
            try
            {
                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();
                DataSet dt;
                hst.Add("Email", Email);
                dt = blldb.GetDataSet("Sp_getEmail", CommandType.StoredProcedure, hst);
                ds = dt;
            }
            catch (Exception ex)
            {

            }
            return ds;
        }

        public DataSet Getmobileno(string mobileno)
        {
            DataSet ds = null;
            try
            {
                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();
                DataSet dt;
                hst.Add("Mobileno", mobileno);
                dt = blldb.GetDataSet("Sp_getmobile", CommandType.StoredProcedure, hst);
                ds = dt;
            }
            catch (Exception ex)
            {

            }
            return ds;
        }

    }
}