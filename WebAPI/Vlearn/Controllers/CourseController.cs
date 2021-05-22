using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VShop.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace VShop.Controllers
{

    // Controller to make CRUD operation for the course's with Role based access.
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly AuthenticationContext _entity;

        // Default Cover Image for the Course's.
        static string coverimagename="default.jpeg";

        // Constructor to access the Entity framework.
        public CourseController(AuthenticationContext entity)
        {
            // Assigning the instances of the constructor to the private properties.
            _entity = entity;
        }

        // Method to Get Course's cover image and make a copy of the image locally.
        // POST : /api/Course/AddCourseImage
        [HttpPost]
        [Route("AddCourseImage")]
        [Authorize(Roles = "Admin")]
        public async Task<Object> AddCourseImage([FromForm]Course receivedCourse)
        {
            var postedFile = receivedCourse.CoverImage;
            if (postedFile.Length > 0)
            {
                using (var fileStream = new FileStream("~/Angular/src/assets/img/Courses/" + postedFile.FileName, FileMode.Create))
                {
                    postedFile.CopyTo(fileStream);
                }
                coverimagename = postedFile.FileName;

            }
            return Ok(new { status = true, message = "Image Posted Successfully" });
        }

        // Method to Add Course to database with entity by admin.
        // POST : /api/Course/Add
        [HttpPost]
        [Route("Add")]
        [Authorize(Roles = "Admin")]
        public async Task<Object> AddCourse(Course modelCourse)
        {
            try
            {
                Course data = new Course
                {
                    Category = modelCourse.Category,
                    Duration = modelCourse.Duration,
                    Price = modelCourse.Price,
                    Title = modelCourse.Title,
                    Description = modelCourse.Description,
                    UplodedDate = DateTime.Now,
                    PictureFileName = coverimagename
                };
                var findcourse = _entity.Courses.Where(Course => Course.Title == data.Title).FirstOrDefault();
                if (findcourse == null)
                {
                    _entity.Courses.Add(data);
                    _entity.SaveChanges();
                    return Ok("Course was succesfully added ");
                }
                else
                {
                    return BadRequest("Course was already exists ");
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }     
        }

        // Method to get all the course details
        //GET : /api/Course/ViewAll
        [HttpGet]
        [Route("ViewAll")]
        public async Task<Object> ViewCourses()
        {
            var data = _entity.Courses;
            return Ok(data);
        }

        // Method to get details of the courses based on their Category names.
        //GET : /api/Course/FindByCategory
        [HttpGet]
        [Route("FindByCategory")]
        public async Task<Object> FindCoursesByCategory(string courseCategory)
        {
            var data = _entity.Courses.Where(Course => Course.Category == courseCategory);
            return Ok(data);
        }

        // Method to Update the Course Details by admin.
        // PUT : /api/Course/Update
        [HttpPut]
        [Route("Update")]
        [Authorize(Roles = "Admin")]
        public async Task<Object> UpdateCourse(Course modelCourse)
        {
            var foundCourse = _entity.Courses.Where(Course => Course.Title == modelCourse.Title).FirstOrDefault();
            if (foundCourse == null)
            {
                return BadRequest("Course not Exists");
            }
            else
            {
                try
                {
                    _entity.Entry(foundCourse).State = EntityState.Modified;
                    foundCourse.Category = modelCourse.Category;
                    foundCourse.Duration = modelCourse.Duration;
                    foundCourse.Price = modelCourse.Price;
                    foundCourse.Description = modelCourse.Description;
                    foundCourse.UplodedDate = DateTime.Now;                    
                    _entity.SaveChanges();
                    return Ok("Course was updated successfully");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest("Something went Wrong");
        }

        // Method to Delete a Course Based on its Id by admin.
        // DELETE : /api/Course/Delete
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<Object> DeleteCourse(int id)
        {
            var foundCourse = _entity.Courses.Where(Course => Course.Id == id).FirstOrDefault();
            if (foundCourse == null)
            {
                return BadRequest("Course not Exists");
            }
            else
            {
                try
                {
                    _entity.Courses.Remove(foundCourse);
                    _entity.SaveChanges();
                    return Ok("Course was updated successfully");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest("Something went Wrong");
        }


    }
}
