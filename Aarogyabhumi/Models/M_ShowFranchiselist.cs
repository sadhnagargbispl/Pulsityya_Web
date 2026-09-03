using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopinv.Models
{
    public class M_ShowFranchiselist
    {
        public List<M_Franchisetype> FranchiseType { get; set; }
        public List<M_Franchiselist> Franchiselist { get; set; }
    }
}