using Shopinv.Entity;
using System.Collections.Generic;
using System.Data;

namespace Shopinv.Interface
{
    public interface I_DelistedDirectSeller
    {
        IEnumerable<E_DelistedDirectSeller> GetOrderdetail();
        IEnumerable<E_DelistedDirectSeller> GetRegisterOrderdetail();
    }
}
