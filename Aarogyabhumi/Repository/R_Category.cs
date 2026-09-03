using Shopinv.Entity;
using Shopinv.Interface;
using Shopinv.Models;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace Shopinv.Repoistory
{
    public class R_Category : I_Category
    {
        public IEnumerable<E_Category> DDLCategory()
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            DataTable ds;
            hst.Add("Action", "CategoryList");
            ds = blldb.GetDataTable("Category", CommandType.StoredProcedure, hst);
            IEnumerable<E_Category> lst = DbOperation.ConvertDataTable<E_Category>(ds);
            return lst;
        }

        public IEnumerable<E_SubCategory> DDLSubCategory(string CatId)
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            DataTable ds;
            hst.Add("Action", "SubCategory");
            hst.Add("CatId", CatId);
            ds = blldb.GetDataTable("Category", CommandType.StoredProcedure, hst);
            IEnumerable<E_SubCategory> lst = DbOperation.ConvertDataTable<E_SubCategory>(ds);
            return lst;

        }


        public IEnumerable<E_Product> SrchProduct(string CatName)
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            DataTable ds;
            hst.Add("Action", "SrchProduct");

            hst.Add("prodName", CatName);
            ds = blldb.GetDataTable("SearchProduct", CommandType.StoredProcedure, hst);
            IEnumerable<E_Product> lst = DbOperation.ConvertDataTable<E_Product>(ds);

            return lst;
        }

        public IEnumerable<E_GetColor> getColors(string prodName)
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hh = new Hashtable();
            hh.Add("prodName", prodName);
            DataTable dt = new DataTable();
            dt = blldb.GetDataTable("Sp_getColor", CommandType.StoredProcedure, hh);
            IEnumerable<E_GetColor> lst = DbOperation.ConvertDataTable<E_GetColor>(dt);
            return lst;
        }

        public IEnumerable<E_SizeMaster> GetSize(string prodName)
        {
            BLLDBOperations bl = new BLLDBOperations();
            DataTable dt = new DataTable();
            Hashtable hh = new Hashtable();
            hh.Add("prodName", prodName);
            dt = bl.GetDataTable("sp_getSize", CommandType.StoredProcedure, hh);
            IEnumerable<E_SizeMaster> lat = DbOperation.ConvertDataTable<E_SizeMaster>(dt);
            return lat;
        }
    }
}