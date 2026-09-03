using Shopinv.Entity;
using System.Collections.Generic;
using System.Data;

namespace Shopinv.Interface
{
    public interface I_CouponReport
    {
        IEnumerable<E_CouponDetail>  /*DataSet*/ GetCoupondetail(string UserId, string Formno);
        }
}
