using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VitaFlow.Domain.Entities
{
    public class ShippingDetail
    {
        public int Id { get; set; }
        public int Userid { get; set; }
        public string BusinessName { get; set; }
        public string CustomerName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public int StateCode { get; set; }
        public int CityCode { get; set; }
        public string Pincode { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public List<StateModel> Statelist { get; set; }
        public List<M_City> CityList { get; set; }
        public List<M_CartDetails> CartList { get; set; }
        public decimal TotalAmont { get; set; }
        public string OrderMethod { get; set; }
        public string IsFirstOrder { get; set; }
    }
}
