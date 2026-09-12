using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VitaFlow.Domain.Entities
{
    public class StateModel
    {
        public decimal StateCode { get; set; }
        public string StateName { get; set; }
        public bool IsCompanyState { get; set; }
    }
}
