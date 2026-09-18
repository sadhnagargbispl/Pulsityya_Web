namespace VitaFlow.Domain.Entities
{
    public class SaleRegister
    {
        public string BillNo { get; set; }

        public string UserBillNo { get; set; }
        public string Billdate { get; set; }
        public string Code { get; set; }
        public string PartyCode { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string ProdID { get; set; }
        public string ProductName { get; set; }

        public string HSNCode { get; set; }
        public string PartyName { get; set; }

        public string GSTIN { get; set; }

        public decimal Tax { get; set; }
        public decimal ExemptSale { get; set; }

        public decimal ExemtTax { get; set; }
        public decimal Discount { get; set; }
        public decimal Basic3 { get; set; }

        public decimal IGST3 { get; set; }
        public decimal CGST15 { get; set; }
        public decimal SGST15 { get; set; }
        public decimal Basic5 { get; set; }
        public decimal IGST5 { get; set; }
        public decimal CGST25 { get; set; }
        public decimal SGST25 { get; set; }
        public decimal Basic12 { get; set; }
        public decimal IGST12 { get; set; }
        public decimal CGST6 { get; set; }
        public decimal SGST6 { get; set; }
        public decimal Basic18 { get; set; }
        public decimal IGST18 { get; set; }
        public decimal CGST9 { get; set; }
        public decimal SGST9 { get; set; }
        public decimal Basic28 { get; set; }
        public decimal IGST28 { get; set; }
        public decimal CGST14 { get; set; }
        public decimal SGST14 { get; set; }
        public decimal TotalBasicAmt { get; set; }
        public decimal TotalIGST { get; set; }
        public decimal TotalCGST { get; set; }
        public decimal TotalSGST { get; set; }

        public decimal RoundOff { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal IGSTAmt { get; set; }
        public decimal CGSTAmt { get; set; }
        public decimal SGSTAmt { get; set; }
        public decimal Qty { get; set; }
        public decimal BasicAmount { get; set; }
        public string Statename { get; set; }
       
    }
}
