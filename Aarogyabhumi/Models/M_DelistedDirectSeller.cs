using PagedList;
using Shopinv.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopinv.Models
{
    public class M_DelistedDirectSeller
    {
        public IPagedList<E_DelistedDirectSeller> DirectsellerReport { get; set; }
    }
}