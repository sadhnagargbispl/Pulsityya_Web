using Shopinv.Models;
using Shopinv.Entity;
using System.Collections.Generic;
using System.Data;
using System.Security.Policy;

namespace Shopinv.Interface
{
    public interface I_Product
    {
        IEnumerable<E_Product> DDLProductList();
        IEnumerable<E_Product> FliterColorSize(string Action, string Prm);
        IEnumerable<E_Product> GetProductByPriceFilter(string MinPrice, string MaxPrice);
        IEnumerable<E_Product> GetProductByBVFilter(string MinBv, string MaxBv);
        IEnumerable<E_Product> GetSpecialProduct();
        IEnumerable<E_Product> GetFeaturedProduct();
        IEnumerable<E_Product> GetTopSellerProduct();
        IEnumerable<E_Product> DealsOfTheWeek();
        IEnumerable<E_ProductDetail> ProductDetail(string ProdId);
        DataSet GetProdAvailable(string ProdId);
        DataSet UpdateCartDetail(string Userid, string idno);
        DataSet UpdateClearCart(string Userid, string idno);
        IEnumerable<E_CartDetails> Cartdetailsftchouter(string userid);//string Unqid,
        IEnumerable<E_CartDetails> Cartdetailsftch(string userid);//string Unqid,
        IEnumerable<E_CartDetails> updateQuantity(string productId, string Quant, string userid);
        IEnumerable<E_CartDetails> updateQuantityouter(string productId, string Quant, string userid);
        string SaveProd(string Action, string ProdId, string ProdName, string Image, string Price, string Bv, string Qty, string myIP, string UnqiueId, string userid, string PV, string Weight, string Color, string SIZE);
        string SaveProdouter(string Action, string ProdId, string ProdName, string Image, string Price, string Bv, string Qty, string myIP, string UnqiueId, string userid, string PV, string Weight, string Color, string SIZE);

        string deleteProd(string Action, string Id, string ProdId, string ProdName, string imagePath, string qty, string Price, string bv, string IpAddress, string posted, string uniqueId, string userid);
        //DataSet SaveStock(DataTable Stock, string myIP, string uniqueId,string userid);
        DataSet SaveCheckOutOrder(DataTable Stock, string myIP, string uniqueId, string userid, string IDNo, string FormNo);
        DataSet InsertOrderDetail(string regXML);
        DataSet InserttblTrnOrderWeb(string regXML, decimal Formno, decimal Amount, decimal ordertransId, string orderType, string Idno, string Narration, string BillNo);
        DataSet GetKitActivation(string Idno, string Formno, string PV, string BV);
        DataSet GetAllparty();
        DataSet GetDeliveryCenterAddress(string PartyCode);
        DataSet GetProductPrizeList();
        DataSet SavepaymentgetwayTemp(string idNo, string FormNo, string Txnid, string firstname, string email, string phone, string userid, string ShopType, string PartyCode, string regXML, string transid, string Amount, string Status, string DecliveryId, string CourierCharge);
        DataSet GetTransId(string Orderid);
        DataSet UpdateStatus(string Status, string Orderid);
        DataSet UpdatePaymentOrderId(string orderid, string txnid);
        DataSet CheckStatus(string idNo);
        DataSet SavePGCashFreeTemp(string idNo, string FormNo, string TxnId, string Email,
              string MobileNo, string userid, string XML, decimal Amountdec, string output,
              string Url, string response, string Flag, string Api, string DeliveryID,
              string CourierChatge, string PartyCode, string Paymentimg, string Specialremark);
        DataSet SavePGCashFreeTempApi(string idNo, string FormNo, string TxnId, string Email, string MobileNo, string userid, string XML, decimal Amountdec, string output, string Url, string response, string Flag, string Api, string DeliveryID, string CourierChatge);
        DataSet GetTransIdApi(string Orderid);
        DataSet getCourierCharge();
        IEnumerable<E_ProductDetail> SearchProductname(string Terms);
        DataSet Get_ColorSizeimgaeBYid(string Action, string ProdId, string colorid, string Colorname);
        DataSet Get_ProductWiseStock(string productid);
        DataSet Get_ProductWiseStockWithPartyCode(string productid, string Partycode);
        DataSet GetCompanydetail();
        DataTable GetIDStatus(string IDno);

        DataTable GetOrderStatus(string formno, string orderid);
        DataSet GetCoupon(string Formno);
        DataTable CouponDetail(string formno, string Couponno, decimal TotalAmount, decimal totalbv);
        DataTable SaveAarogyaidactivationLog(string idno, string Request, string Response);
        bool SaveTransactionOrder(int Orderno);
        List<M_Franchisetype> GetFranchisetype();
        List<M_Franchiselist> GetFranchiseList(int FranchiseType);
        List<M_GetWallettype> GetWallettype();
        List<WalletUseDetail> GetWallettypeBalance(string Formno, string WalletType);
        List<AllWalletDetail> GetAllWalletDetail(string Formno, string WalletType);
        List<M_Level> GetLevel(string Formno, string Type);
        List<ReferalDownlinein> GetReferalDownlineinfonew(string Formno);
        DataSet sp_GetLevelDetail(string MLevel, string Legno, string ActiveStatus, string @FormNo, int PageIndex, int PageSize);
        DataSet GetLevelIncome(int FormNo);
        IEnumerable<E_ProductReview> GetProductReview(int Productcode);
        DataSet SaveReview(string ReviewMessage, string ReviewRating, string ReviewName, string Productcode, int Formno);
        DataSet SaveShoppingWishlist(int UserID, int ProductID);
        DataSet CheckProductwiseWishlist(int UserID, int ProductID);
        IEnumerable<E_CartDetails> CheckUserwiseWishlist(int UserID);
        DataSet CheckTxno(string Txno);
        IEnumerable<E_ProductReview> GetTopProductReview();
        DataSet CheckSponsor(string id);
        DataSet SaveMember(RegisterUser req);
        List<BankList> GetbankLists();
        List<KycTypeMaster> kycTypeMasters();
        DataSet Checkordercount(int Formno);
        DataSet CheckKitOnPurchase(int Formno, decimal TotalAmount);
        DataSet SaveRazarpayTemp(string idNo, string FormNo, string TxnId, string Email,
        string MobileNo, string userid, string XML, decimal Amountdec, string output,
        string Url, string response, string Flag, string Api, string DeliveryID,
        string CourierChatge, string PartyCode, string Paymentimg, string PgTxnid, string Bvapiurl, string Mememode);
        DataSet GetOrderbyPgTxnid(string PgTxnid);
        DataSet RejectPGOrder(string OrderNo, string RejectReason);
        DataSet UpdateKitOnPurchaseUpdate(int Formno, decimal TotalAmount, string BillType);
        DataSet GetWholeIncomeRange(int Formno, decimal PV);
        DataSet SaveRazarpayTemp(string idNo, string FormNo, string TxnId, string Email,
       string MobileNo, string userid, string XML, decimal Amountdec, string output,
       string Url, string response, string Flag, string Api, string DeliveryID,
       string CourierChatge, string PartyCode, string Paymentimg, string PgTxnid);
        DataSet UpdateStatus(string Status, string Orderid, string transid, string razorpay_payment_id);
        IEnumerable<E_CartDetails> GetTempAddtocart(string TempOrderDataPGID);

    }
}
