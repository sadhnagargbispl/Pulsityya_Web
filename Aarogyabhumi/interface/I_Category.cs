using Shopinv.Entity;
using System.Collections.Generic;

namespace Shopinv.Interface
{
    public interface I_Category
    {
        IEnumerable<E_Category> DDLCategory();
        IEnumerable<E_SubCategory> DDLSubCategory(string CatId);
        IEnumerable<E_Product> SrchProduct(string CatName);
        IEnumerable<E_GetColor> getColors(string prodName);
        IEnumerable<E_SizeMaster> GetSize(string prodName); 
    }
}
