using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopinv.Entity
{
    public class E_ProdPrizeList
    {    public int Rn { get; set; }
         public string Name { get; set; }
         public string ProductName { get; set; }
         public string ProdCode { get; set; }
         public string CatId { get; set; }
         public decimal Prize { get; set; }
         public decimal MRP { get; set; }
         public decimal EP { get; set; }
         public decimal PV { get; set; }
    }
}