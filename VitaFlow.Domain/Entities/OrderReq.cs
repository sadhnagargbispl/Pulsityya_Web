using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VitaFlow.Domain.Entities
{
    public class OrderReq
    {
        public List<M_CartDetails> Orderdetail { get; set; }
        public decimal NetAmount { get; set; }
        public string OrderBy { get; set; }
        public string OrderTo { get; set; }
        public string OrderNo { get; set; }
        public bool IsP { get; set; }
        public PayDetails PayDetails { get; set; }
        public string SelectedInvoiceType { get; set; }
        public decimal AmountByPaytm { get; set; }
        public string PaytmTransactionId { get; set; }
        public decimal UserId { get; set; }
        public string UserName { get; set; }
        public int GroupId { get; set; }
        public string PartyName { get; set; }
        public int TotalQty { get; set; }
        public decimal TotalTaxAmount { get; set; }
        public decimal Roundoff { get; set; }
        public decimal TotalNetPayable { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalBV { get; set; }
        public decimal TotalRP { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalPV { get; set; }
        public string OrderMethod { get; set; }
        public string wallettype { get; set; }
        public decimal Promobalance { get; set; } 
    }
}
