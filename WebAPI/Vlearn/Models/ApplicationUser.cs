using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VShop.Models
{
    // Custome model from the Identity Model to add User and link their course's.
    public class ApplicationUser : IdentityUser
    {       

        [Column(TypeName="nvarchar(150)")]

        public string FullName { get; set; }

        public string ImageFileName { get; set; }

        public IList<Course> Courses{ get; set; }

        public IFormFile Profileimage { get; set; }
    }
}
