using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VitaFlow.Domain.Entities
{
    public class Login
    {
        [Required(ErrorMessage = "Please enter username")]
        public string? Username { get; set; }
        [Required(ErrorMessage = "Please enter password")]
        public string? Password { get; set; }
    }

    
}
