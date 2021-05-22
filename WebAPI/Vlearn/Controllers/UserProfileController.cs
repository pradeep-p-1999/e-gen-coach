using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VShop.Models;

namespace VShop.Controllers
{
    // Controller to Make CRUD operation of the User Profile Details. 
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private readonly AuthenticationContext _entity;

        // Default Cover Image for the profile.
        static string profileimagename = "default.png";
       

        // Constructor to access the Entity framework and Identy framework.
        public UserProfileController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AuthenticationContext entity)
        {
            // Assigning the instances of the  constructor to the private properties 
            _userManager = userManager;
            _signInManager = signInManager;
            _entity = entity;
        }

        // Method to Get the User details of the current user.
        // GET : /api/UserProfiles
        [HttpGet]
        [Authorize(Roles = "Customer,Admin")]
        public async Task<Object> GetUserProfile()
        {
            try
            {
                string userId = User.Claims.First(result => result.Type == "UserID").Value;
                var user = await _userManager.FindByIdAsync(userId);
                return new
                {
                    user.FullName,
                    user.Email,
                    user.UserName,
                    user.ImageFileName
                };
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
        // Method to update User Profile Image.
        // POST : /api/UserProfile/AddProfileImage
        [HttpPost]
        [Route("AddProfileImage")]
        [Authorize(Roles = "Customer,Admin")]
        public async Task<Object> AddCourseImage([FromForm]ApplicationUser receiveduser)
        {
            try
            {
                var postedFile = receiveduser.Profileimage;
                if (postedFile.Length > 0)
                {
                    using (var fileStream = new FileStream("~/Angular/src/assets/img/Users/" + postedFile.FileName, FileMode.Create))
                    {
                        postedFile.CopyTo(fileStream);
                    }
                    profileimagename = postedFile.FileName;
                }
                return Ok(new { status = true, message = "Image Posted Successfully" });
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Method to update User Profile Details.
        // PUT : /api/UserProfiles/Update
        [HttpPut]
        [Route("Update")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<Object> UpdateCourse(ApplicationUser modelprofile)
        {            
            var foundprofile = _entity.ApplicationUsers.Where(profile => profile.UserName == modelprofile.UserName).FirstOrDefault(); ;
            if (foundprofile == null)
            {
                return BadRequest("User not Exists");
            }
            else
            {
                try
                {                    
                    _entity.Entry(foundprofile).State = EntityState.Modified;
                    foundprofile.Email = modelprofile.Email;
                    foundprofile.FullName = modelprofile.FullName;
                    if(profileimagename != "default.png")
                    {
                        foundprofile.ImageFileName = profileimagename;
                    }
                    _entity.SaveChanges();
                    profileimagename = "default.png";
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
                return Ok("profile was updated successfully");
            }
            return BadRequest("Something went Wrong");
        }


    }
}
