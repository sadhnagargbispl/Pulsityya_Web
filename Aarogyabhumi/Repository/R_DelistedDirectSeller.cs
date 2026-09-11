using Shopinv.Entity;
using Shopinv.Interface;
using Shopinv.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace Shopinv.Repoistory
{
    public class R_DelistedDirectSeller : I_DelistedDirectSeller
    {
        public IEnumerable<E_DelistedDirectSeller> GetOrderdetail()
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            DataTable dt;
            hst.Add("Action", "blockid");
            dt = blldb.GetDataTable("Sp_GetDelistedDirectSeller", CommandType.StoredProcedure, hst);
            IEnumerable<E_DelistedDirectSeller> lst = DbOperation.ConvertDataTable<E_DelistedDirectSeller>(dt);
            return lst;
        }
        public IEnumerable<E_DelistedDirectSeller> GetRegisterOrderdetail()
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            DataTable dt;
            hst.Add("Action", "notblockid");
            dt = blldb.GetDataTable("Sp_GetDelistedDirectSeller", CommandType.StoredProcedure, hst);
            IEnumerable<E_DelistedDirectSeller> lst = DbOperation.ConvertDataTable<E_DelistedDirectSeller>(dt);
            return lst;
        }
    }
}