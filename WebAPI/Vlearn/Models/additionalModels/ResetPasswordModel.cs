using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VShop.Models.additionalModels
{
    // Model helps to Reset password who forgots Password.
    public class ResetPasswordModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }
       
        public string Token { get; set; }
    }
}
