using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VitaFlow.Domain.Entities
{
    public  class M_Registration
    {
        [Required(ErrorMessage = "select Franchise")]
        public decimal GroupId { get; set; }
        [Required(ErrorMessage = "Select State")]
        public decimal StateCode { get; set; }
        [Required(ErrorMessage = "PartyCode Required!")]
        public string? PartyCode { get; set; }
        [Required(ErrorMessage = "Name Required!")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Address Required!")]
        public string? Address { get; set; }
        [Required(ErrorMessage = "PinCode Required!")]
        public string? PinCode { get; set; }
        
        [Required(ErrorMessage = "Phone Number Required!")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
                   ErrorMessage = "Entered mobile format is not valid.")]
        [StringLength(10, ErrorMessage = "MobileNo length can't be more than 10.")]
        public string? MobileNo { get; set; }
        public string GST { get; set; }
        [Required (ErrorMessage = "Password Required!")]
        public string? Password { get; set; }
        [Required(ErrorMessage = "City Required!")]
        public string? City { get; set; }
        public string ParentPartyCode { get; set; }
        public string ParentpartyName { get; set; }
        public List<GroupModel> GroupList { get; set; } 
        public List<StateModel> Statelist { get; set; }
    }
}
