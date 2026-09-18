using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VitaFlow.Domain.Entities
{
    public class Product
    {
        public int ProdId { get; set; }
        public decimal PV { get; set; }
        public string ImagePath { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public decimal BV { get; set; }
        public decimal MRP { get; set; }
        public decimal Discount { get; set; }
        public decimal Qty { get; set; }
        public string Liner { get; set; }
        public string IsForPC { get; set; }
        public string IsFlexible { get; set; }
        public string HotSell { get; set; }
        public string spcloffer { get; set; }
        public decimal Weight { get; set; }
        public decimal Gst { get; set; }
        public decimal StockQTY { get; set; }
        public string BatchNo { get; set; }
        public List<ProductBatchModel> BatchDetail { get; set; }
        public int SubCatId { get; set; }
    }

    public class M_product
    {
        public List<Product> ProductsList { get; set; }
        public List<M_SubCatMaster> SubCategoriesList { get; set; }

    }
    public class M_SubCatMaster
    {
        public int SubCatId { get; set; }
        public string SubCatName { get; set; }
        public string ActiveStatus { get; set; }
    }
    public class clientProduct
    {
        public string PartyCode { get; set; }
        public string PartyName { get; set; }
    }
}
