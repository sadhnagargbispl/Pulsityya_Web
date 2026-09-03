using System;
using System.Collections;
using System.Data;
using Shopinv.Interface;
using Shopinv.Models;

namespace Shopinv.Repository
{
    public class R_Package:I_Package
    {
      public DataSet savepackage(string Kitid, string formno)
        {
            DataSet dt= new DataSet();
            try
            {
                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();
             
                hst.Add("Action", "Savepackage");
                hst.Add("Kitid", Kitid);
                hst.Add("FormNo", formno);
                dt = blldb.GetDataSet("Sp_Savepackage", CommandType.StoredProcedure, hst);
            }
             catch(Exception ex)
            {

            }
            return dt;
        }

        public DataSet getPackage( string formno)
        {
            DataSet dt = new DataSet();
            try
            {
                BLLDBOperations blldb = new BLLDBOperations();
                Hashtable hst = new Hashtable();

                hst.Add("Action", "Getpackage");
                hst.Add("FormNo", formno);
                dt = blldb.GetDataSet("Sp_Savepackage", CommandType.StoredProcedure, hst);
            }
            catch (Exception ex)
            {

            }
            return dt;
        }
    }
}