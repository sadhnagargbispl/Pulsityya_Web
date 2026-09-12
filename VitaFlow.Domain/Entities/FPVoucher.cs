using System;

namespace VitaFlow.Domain.Entities
{
    public class FPVoucher
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string IdNo { get; set; }
        public bool Isuse { get; set; }
        public decimal Amount { get; set; }
        public decimal credit { get; set; }
        public decimal debit { get; set; }
        public decimal balance { get; set; }
        public decimal TotalBalance { get; set; }


    }

    public class FPVoucherEligibilityResult
    {
        public string EligibilityStatus { get; set; }
        public string Reason { get; set; }
    }

    public class FPVucherWallet
    {
        public int id { get; set; }
        public string Idno { get; set; }
        public string Remark { get; set; }
        public decimal credit { get; set; }
        public decimal debit { get; set; }
        public string Vtype { get; set; }

        public string VoucherNo { get; set; }
    }
}
