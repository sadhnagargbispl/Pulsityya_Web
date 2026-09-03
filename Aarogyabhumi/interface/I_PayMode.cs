using Shopinv.Entity;
using System.Collections.Generic;
using System.Data;

namespace Shopinv.Interface
{
    public interface I_PayMode
    {
        IEnumerable<E_Payment> GetBalence(string walletType, string FormNo);
        string TRNVoucherDebit(string FormNo, string Amount, string walletType, string IdNo, string OrderID);
        DataSet GetPendinOrderDetail(string IDno, string formno);
        DataSet GetpaygatwayOrderDetail(string TxnID);
        DataSet SaveOnlineres(string orderid, string res);

    }
}
