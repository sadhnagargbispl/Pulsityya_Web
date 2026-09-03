using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Shopinv.Models
{
    public class M_WalletDetail
    {
       public List<WalletUseDetail> walletUseDetail { get; set; }
        public List<AllWalletDetail> allWalletDetails { get; set; }
        public List<M_GetWallettype> m_GetWallettypes { get; set; }
    }

    public class AllWalletDetail
    {
        public Int64 SNo { get; set; }
        public string Date { get; set; }
        public decimal VoucherNo { get; set; }
        public string Remark { get; set; }
        public decimal Deposit { get; set; }
        public decimal used { get; set; }
    }

    public class WalletUseDetail
    {
        public decimal Credit { get; set; }
        public decimal Debit { get; set; }
        public decimal Balance { get; set; }
    }
}