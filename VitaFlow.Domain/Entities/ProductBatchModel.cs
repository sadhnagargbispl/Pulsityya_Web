using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VitaFlow.Domain.Entities
{
    public class ProductBatchModel
    {
        public string ProdId { get; set; }

        public string BatchNo { get; set; }
        public string Batchcode { get; set; }
        public string Barcode { get; set; }
        public bool IsExpirable { get; set; }
        public DateTime ExpDate { get; set; }
        public decimal? MRP { get; set; }
        public decimal Weight { get; set; }
        public decimal? RP { get; set; }
        public decimal? RPValue { get; set; }
        public decimal? DP { get; set; }
        public decimal StockAvailable { get; set; }
        public decimal? BV { get; set; }
        public decimal? CV { get; set; }
        public decimal? PV { get; set; }
        public decimal? DiscPer { get; set; }
        public decimal? DiscAmt { get; set; }
        public int IsCommissionAdd { get; set; }
        public int IsDiscountAdd { get; set; }
        public string TaxType { get; set; }
        public decimal? Rate { get; set; }
        public decimal? CommissionPer { get; set; }
        public string IsAvailableForOffer { get; set; }
        public string IsAvailableForBilling { get; set; }
        public decimal TotalDiscPer { get; set; }
        public decimal? TaxPer { get; set; }
        public string ProductName { get; set; }
        public string ProductCodeStr { get; set; }
        public int ProdCode { get; set; }
        public OfferProducts offerDetail { get; set; }

    }
}
