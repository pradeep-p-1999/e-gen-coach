using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VShop.Models
{
    // User's Data Model in Registration process and other user Details process.
    public class ApplicationUserModel
    {       
     
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string FullName { get; set; }

        public string Role { get; set; }       


    }
}
