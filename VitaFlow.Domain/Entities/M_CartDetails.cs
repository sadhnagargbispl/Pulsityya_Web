using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VitaFlow.Domain.Entities
{
    public class CartDetails
    {
        public List<M_CartDetails> CartList { get; set; }
        public decimal TotalAmont { get; set; }

    }
    public class M_CartDetails
    {
        public int id { get; set; }
        public string ProdId { get; set; }
        public string ProdName { get; set; }
        public string imagePath { get; set; }
        public string IpAddress { get; set; }
        public decimal Price { get; set; }
        public decimal BunchQty { get; set; }
        public decimal bv { get; set; }
        public decimal PV { get; set; }
        public decimal qty { get; set; }
        public string PartyCode { get; set; }
        public decimal Weight { get; set; }
        public decimal Gst { get; set; }
        public decimal prodprice { get; set; }
        public decimal gstAmt { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string UnqiueId { get; set; }
        public string userid { get; set; }
        public string FCode { get; set; }
        public string Action { get; set; }
        public int CatId { get; set; }
        public string CatName { get; set; }
        public string Barcode { get; set; }
        public string BatchNo { get; set; }
        public decimal DP { get; set; }
        public decimal DPValue { get; set; }
        public decimal DP1 { get; set; } 
        public decimal RP { get; set; }
        public decimal DiscPer { get; set; }
        public decimal DiscAmt { get; set; }
        public string ProdCode { get; set; }
        public string ProductCodeStr { get; set; }
        public decimal TaxPer { get; set; }
        public int ProdStateCode { get; set; }
        public decimal CV { get; set; }
        public decimal RPValue { get; set; }
        public decimal CVValue { get; set; }
        public decimal BVValue { get; set; }
        public decimal PVValue { get; set; }
        public bool IsExpirable { get; set; }
        public DateTime ExpDate { get; set; }
        public string TaxType { get; set; }
        public decimal Rate { get; set; }
        public decimal CommissionPer { get; set; }
        public int SubCatId { get; set; }
        public string IsAvailableForOffer { get; set; }
        public string IsAvailableForBilling { get; set; }
        public decimal TotalDiscPer { get; set; }
        public int IsDiscountAdd { get; set; }  
        public int IsCommissionAdd { get; set; }
        public decimal MRP { get; set; }
        public decimal AvailStock { get; set; }
        public bool IsFirstOrder { get; set; }
        public  bool IsPVorder { get; set; }
        public  bool IsBVorder { get; set; }
        public string OrderTo { get; set; }
        public decimal NetAmount { get; set; }
        public decimal AmountByPaytm { get; set; }
        public decimal TaxAmt { get; set; }
        public decimal Amount { get; set; }
        public decimal TotalAmt { get; set; }
        public decimal CommssnAmt { get; set; }
        public decimal TotalProductamount { get; set; }
        public string ParentPartycode { get; set; }
    }
}
