using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VShop.Models
{
    // Model to take Course's and their stucture and details.
    public class Course
        {
        public int Id { get; set; }

        public string Category { get; set; }

        public string Duration { get; set; }

        public decimal Price { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime UplodedDate { get; set; }

        public string PictureFileName { get; set; }

        public IList<ApplicationUser> ApplicationUsers { get; set; }

        public IFormFile CoverImage { get; set; }

    }
}
