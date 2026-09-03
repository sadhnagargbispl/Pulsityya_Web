using Shopinv.Entity;
using System.Collections.Generic;

namespace Shopinv.Models
{
    public class M_Category
    {
        public E_Category Category { get; set; }
        public E_Payment srchPayment { get; set; }
        public IEnumerable<E_Payment> GetWalletBalence { get; set; }
        public IEnumerable<E_Category> DDLCategory { get; set; }
        public IEnumerable<E_SubCategory> DDLSubCategory { get; set; }
        public E_Product product { get; set; }
        public IEnumerable<E_Product> ProductList { get; set; }
        public IEnumerable<E_Product> SrchFilterProductList { get; set; }
        public IEnumerable<E_Product> SpecialProductList { get; set; }
        public IEnumerable<E_Product> FeaturedProduct { get; set; }
        public IEnumerable<E_Product> DealsOfTheWeek { get; set; }
        public IEnumerable<E_Product> TopSellerProduct { get; set; }
        public IEnumerable<E_Product> SearchProductList { get; set; }
        public IEnumerable<E_ProductDetail> ProductDetail { get; set; }
        public IEnumerable<E_CartDetails> CartDetail { get; set; }
        public IEnumerable<E_CartDetails> DeleteDetail { get; set; }
        public IEnumerable<E_CheckOut> CheckOutDetail { get; set; }
        public IEnumerable<E_OrderReport> OrderReport { get; set; }
        public IEnumerable<E_OrderReport> OrderNoDetail { get; set; }
        public IEnumerable<E_Banner> ShowBanner { get; set; }
        public IEnumerable<E_Banner> NewYearBanner { get; set; }
        public IEnumerable<E_Banner> SeasonSaleBanner { get; set; }
        public IEnumerable<E_Banner> NewTrendBanner { get; set; }
        public IEnumerable<E_Banner> SaleUpToBanner { get; set; }
        public E_RegisterUser PlacerOrder { get; set; }
        public E_RegisterUser otherdetailUser { get; set; }
        public IEnumerable<E_RegisterUser> RegisterUserDetails { get; set; }
        public IEnumerable<E_RegisterUser> userOtherDetail { get; set; }
        public IEnumerable<E_UserPoints> UserPoints { get; set; }
        public IEnumerable<StateList> DDLState { get; set; }
        public IEnumerable<Address> Addressdetail { get; set; }
        public IEnumerable<E_Product> RelatedProduct { get; set; }
        public IEnumerable<E_RankAchiver> GetrankAchiver { get; set; }
        public IEnumerable<E_ProdPrizeList> GetProductPrizeLst { get; set; }

        // public E_pendingLstTrans TransactionPendingLst { get; set; }
        public IEnumerable<E_pendingOrderDetail> PendingOrder { get; set; }
        public IEnumerable<E_PendingTrans> PendingTrans { get; set; }

        public List<E_PendingTrans> lstobjg { get; set; }
        public List<E_FranchiseName> GetFranchiseList { get; set; }
        public E_Grivancelstres objGrivances { get; set; }
        public IEnumerable<E_GetColor> GetColors { get; set; }  
        public IEnumerable<E_SizeMaster> GetSizes { get; set; } 
        public IEnumerable <E_CouponDetail> GetCouponDetailNew { get; set; }
        public IEnumerable<E_ProductReview> ProductReview { get; set; }
        public IEnumerable<E_OrderReport> OfflineOrderReport { get; set; }
        public IEnumerable<E_Banner> PopupBannerList { get; set; }
    }

   

}