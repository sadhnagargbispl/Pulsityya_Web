using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VitaFlow.Domain.Entities
{
    /// <summary>
    /// Row of VoucherType where IsWr = 1 i.e. the wallet(s) the company actually uses.
    /// </summary>
    public class WalletTypeMaster
    {
        public int Id { get; set; }
        public string Vtype { get; set; }
        public string Voucher_Discrption { get; set; }

        /// <summary>
        /// Order method used on checkout for this wallet ("BV" / "PV").
        /// </summary>
        public string OrderMethod
        {
            get
            {
                string code = (Vtype ?? "").Trim().ToUpper();
                return (code == "B" || code == "Z") ? "BV" : "PV";
            }
        }
    }
}
