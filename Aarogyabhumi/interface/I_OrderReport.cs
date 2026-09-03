using Shopinv.Entity;
using System.Collections.Generic;
using System.Data;

namespace Shopinv.Interface
{
    public interface I_OrderReport
    {
        IEnumerable<E_OrderReport>  /*DataSet*/ GetOrderdetail(string UserId, string Formno);
        IEnumerable<E_OrderReport> GetGrdOrderNoDetail(string OrderId, string userid);
        IEnumerable<E_PendingTrans> GetPendingOrder();
        DataSet GetPendingOrderDetail(string txnid, string Formno);
        DataSet UpdateNewTxnid(string Formno, string txnid, string newtxnid, string newtransid);
        IEnumerable<E_CouponDetail>  /*DataSet*/ GetCoupondetailNew(string UserId, string Formno);
        IEnumerable<E_OrderReport> GetOfflineOrderdetail(string Formno);
        IEnumerable<E_OrderReport> Showofflineorderdetail(string OrderId, string userid);

    }
}
