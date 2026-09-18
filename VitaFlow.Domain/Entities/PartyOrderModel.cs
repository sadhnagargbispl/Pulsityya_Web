using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VitaFlow.Domain.Entities
{
    public class PartyOrderModel
    {
        public DateTime OrderDate { get; set; }
        public string OrderDateStr { get; set; }
        public string OrderNo { get; set; }
        public string OrderBy { get; set; }
        public string OrderTo { get; set; }
        public string Remarks { get; set; }
        public ProductModel objProduct { get; set; }
        public List<ProductModel> objListProduct { get; set; }
        public string objProductListStr { get; set; }
        public List<TaxSummary> objTaxSummary { get; set; }
        public User LoginUser { get; set; }
        public decimal PartyWalletBalance { get; set; }
        public string PartyCode { get; set; }
        public string PartyName { get; set; }
        public decimal OrderAmt { get; set; }
        public string ChNo { get; set; }
        public DateTime ChDate { get; set; }
        public decimal ChAmt { get; set; }
        public string BankName { get; set; }
        public decimal WalletAmt { get; set; }
        public string DispStatus { get; set; }
        public string ProductName { get; set; }
        public decimal PV { get; set; }
        public decimal BV { get; set; }
        public decimal TotalNetPayable { get; set; }
        public decimal OrderedOty { get; set; }
        public string ImagePath { get; set; }
        public decimal DP { get; set; }
        public string OrderMethod { get; set; }
        public int TotalOrdQty { get; set; }
        public int TotalRemQty { get; set; }
        public int TotalDispQty { get; set; } 
    }
    public class PartyOrderList
    {
        public List<PartyOrderModel> GetPartyOrderlist { get; set; }
    }
}
