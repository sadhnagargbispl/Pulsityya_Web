using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VitaFlow.Domain.Entities
{
    public partial class M_PayModeMaster
    {
        public decimal PId { get; set; }
        public string PayMode { get; set; }
        public string ActiveStatus { get; set; }
        public System.DateTime RecTimeStamp { get; set; }
        public string Remarks { get; set; }
        public string Prefix { get; set; }
        public string IsFor { get; set; }
        public string IsShow { get; set; }
    }
}
