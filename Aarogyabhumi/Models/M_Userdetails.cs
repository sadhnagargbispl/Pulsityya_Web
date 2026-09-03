using Shopinv.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shopinv.Models
{
    public class M_Userdetails
    {

        public IEnumerable<E_UserDetail> DDLUserDetails { get; set; }
        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail id is not valid")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}