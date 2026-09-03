using Shopinv.Entity;
using System.Collections.Generic;

namespace Shopinv.Interface
{
    public interface I_Banner
    {
        IEnumerable<E_Banner> ShowBannerList(string BannerCatId);
        IEnumerable<E_Banner> Showpopup(string BannerCatId);
        IEnumerable<E_Banner> NewYearBanner(string BannerCatId);
        IEnumerable<E_Banner> SeasonSaleBanner(string BannerCatId);
        IEnumerable<E_Banner> NewTrendBanner(string BannerCatId);
        IEnumerable<E_Banner> SaleUpToBanner(string BannerCatId);
        IEnumerable<E_RankAchiver> GetRankActiver();
    }
}
