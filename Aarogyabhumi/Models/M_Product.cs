using Shopinv.Entity;
using System.Collections.Generic;

namespace Shopinv.Models
{
    public class M_Product
    {
        public E_Product product { get; set; }
        public IEnumerable<E_Product> ProductList { get; set; }
        public IEnumerable<E_Category> DDLCategory { get; set; }
        public IEnumerable<E_CartDetails> CartDetail { get; set; }
        
    }
}