using System.ComponentModel.DataAnnotations;

namespace Shopinv.Models
{
    public class LoginLPlus
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        

    }
}