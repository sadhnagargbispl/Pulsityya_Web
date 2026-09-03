using Shopinv.Entity;
using Shopinv.Interface;
using Shopinv.Models;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace Shopinv.Repoistory
{
    public class R_Banner:I_Banner
    {
        public IEnumerable<E_Banner> ShowBannerList(string BannerCatId)
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            DataTable dt;
            hst.Add("Action", "DisplayBanner");
            hst.Add("BannerCatId", 1);
            dt = blldb.GetDataTable("ShowBanner", CommandType.StoredProcedure, hst);
            IEnumerable<E_Banner> lst = DbOperation.ConvertDataTable<E_Banner>(dt);
            return lst;
        }
        public IEnumerable<E_Banner> Showpopup(string BannerCatId)
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            DataTable dt;
            hst.Add("Action", "DisplayBanner");
            hst.Add("BannerCatId", 1);
            dt = blldb.GetDataTable("ShowPopupList", CommandType.StoredProcedure, hst);
            IEnumerable<E_Banner> lst = DbOperation.ConvertDataTable<E_Banner>(dt);
            return lst;

        }
        public IEnumerable<E_Banner> NewYearBanner(string BannerCatId)
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            DataTable dt;
            hst.Add("Action", "DisplayBanner");
            hst.Add("BannerCatId", 2);
            dt = blldb.GetDataTable("ShowBanner", CommandType.StoredProcedure, hst);
            IEnumerable<E_Banner> lst = DbOperation.ConvertDataTable<E_Banner>(dt);
            return lst;

        }
        public IEnumerable<E_Banner> SeasonSaleBanner(string BannerCatId)
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            DataTable dt;
            hst.Add("Action", "DisplayBanner");
            hst.Add("BannerCatId", 3);
            dt = blldb.GetDataTable("ShowBanner", CommandType.StoredProcedure, hst);
            IEnumerable<E_Banner> lst = DbOperation.ConvertDataTable<E_Banner>(dt);
            return lst;

        }
        public IEnumerable<E_Banner> NewTrendBanner(string BannerCatId)
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            DataTable dt;
            hst.Add("Action", "DisplayBanner");
            hst.Add("BannerCatId", 4);
            dt = blldb.GetDataTable("ShowBanner", CommandType.StoredProcedure, hst);
            IEnumerable<E_Banner> lst = DbOperation.ConvertDataTable<E_Banner>(dt);
            return lst;

        }
        public IEnumerable<E_Banner> SaleUpToBanner(string BannerCatId)
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            DataTable dt;
            hst.Add("Action", "DisplayBanner");
            hst.Add("BannerCatId", 5);
            dt = blldb.GetDataTable("ShowBanner", CommandType.StoredProcedure, hst);
            IEnumerable<E_Banner> lst = DbOperation.ConvertDataTable<E_Banner>(dt);
            return lst;

        }

         public IEnumerable<E_RankAchiver> GetRankActiver()
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            DataTable dt;
            dt = blldb.GetDataTable("sp_GetRankAchievers", CommandType.StoredProcedure);
            IEnumerable<E_RankAchiver> lst = DbOperation.ConvertDataTable<E_RankAchiver>(dt);
            return lst;
        }
    }
}