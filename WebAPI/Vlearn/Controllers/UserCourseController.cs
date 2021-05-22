using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using VShop.Models;

namespace VShop.Controllers
{
    // Controller to Add User to Many Courses and added deatails of the users.
    [Route("api/[controller]")]
    [ApiController]
    public class UserCourseController : ControllerBase
    {
        private readonly AuthenticationContext _entity;
        private UserManager<ApplicationUser> _userManager;

        // Constructor to access the Entity Framework and Identity Framework.
        public UserCourseController(AuthenticationContext entity, UserManager<ApplicationUser> userManager)
        {
            // Assigning the instances of the  constructor to the private properties 
            _userManager = userManager;
            _entity = entity;

        }

        // Method to Add (or Enroll) Course to the Current User.
        // POST : /api/UserCourse/AddUserCourse
        [HttpPost]
        [Route("AddUserCourse")]
        [Authorize(Roles = "Customer,Admin")]
        public async Task<Object> AddUsersCourse(Course tmpcourse)
        {
            try
            {
                var userId = User.Claims.First(result => result.Type == "UserID").Value;
                Course data = _entity.Courses.FirstOrDefault(Course => Course.Title == tmpcourse.Title);
                _entity.ApplicationUsers.Include("Courses").FirstOrDefault(res => res.Id == userId).Courses.Add(data);
                //  To make edit options  _entity.Entry(Update).State = EntityState.Modified; 
                _entity.SaveChanges();
                return Ok("Added Successfully");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // Method to get the deatils of the Enrolled users courses.
        // Get : /api/UserCourse/UserCourses
        [HttpGet]
        [Route("UserCourses")]
        [Authorize(Roles = "Customer,Admin")]
        public async Task<Object> UsersCourses()
        {
            try
            {
                var userId = User.Claims.First(result => result.Type == "UserID").Value;
                var RawData = _entity.ApplicationUsers.Include("Courses").FirstOrDefault(res => res.Id == userId).Courses;

                foreach (var currentUserCourse in RawData)
                {
                    currentUserCourse.ApplicationUsers = null;

                }

                return Ok(RawData);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
           








    }
}
