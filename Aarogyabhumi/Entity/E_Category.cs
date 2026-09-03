using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopinv.Entity
{ 
    public class E_Category
    {

        public decimal CatId { get; set; }
        public string CatName { get; set; }
        public string CatImage { get; set; }
        public IEnumerable<E_SubCategory> subCategory { get; set; }


    }

    public class E_SubCategory
    {
        public decimal SubCatId { get; set; }
        public string SubCatName { get; set; }
    }

    public class E_GetColor
    {
        public decimal Id { get; set; }
        public string ColorName { get; set; }
        public string ColorCode { get; set; }
    }

    public class E_SizeMaster
    {
        public decimal Id { get; set; }
        public string Size { get; set; }
    }

}