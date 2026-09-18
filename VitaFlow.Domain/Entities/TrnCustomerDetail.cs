using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VitaFlow.Domain.Entities
{
    public class TrnCustomerDetail
    {
        public decimal AId { get; set; }
        public string CustomerName { get; set; }
        public string MobileNo { get; set; }
        public string BillNo { get; set; }
        public string ActiveStatus { get; set; }
        public string Address1 { get; set; }
    }
}
