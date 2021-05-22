using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using VShop.Models;
using VShop.Models.additionalModels;

namespace VShop.Controllers
{
    // Application user account controls are used in this controller.
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;

        // Deafault profile image for the Users.
        static string profileimagename = "default.png";

        // Constructor to access the Identity UserManager.
        public ApplicationUserController(UserManager<ApplicationUser> userManager , SignInManager<ApplicationUser> signInManager )
        {
            // Assigning the instances of the  constructor to the private properties.
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // Application User Account details Registers or added to the database with identity framework.
        // POST : /api/ApplicationUser/Register
        [HttpPost]
        [Route("Register")]
        public async Task<Object> PostApplicationUser(ApplicationUserModel model)
        {
            // Defaultly role of the registering users are Customer 
            model.Role = "Customer";
            var applicationUser = new ApplicationUser()
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName,
                ImageFileName = profileimagename
            };

            // If any error occurs in registering this block will trow badrequest with exception message
            try
            {
                var result = await _userManager.CreateAsync(applicationUser, model.Password);
                await _userManager.AddToRoleAsync(applicationUser, model.Role);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Application Users login controll.
        //POST :/api/ApplicationUser/Login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            // Check the user name and password.
            if(user !=null && await _userManager.CheckPasswordAsync(user,model.Password))
            {
                //Get the roles assigned to the user
                var role = await _userManager.GetRolesAsync(user);
                IdentityOptions _options = new IdentityOptions();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[] {

                        new Claim("UserID", user.Id.ToString()),
                        new Claim(_options.ClaimsIdentity.RoleClaimType,role.FirstOrDefault())

                    }),
                    Expires = DateTime.UtcNow.AddHours(5),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("1234567890123456")), SecurityAlgorithms.HmacSha256Signature)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                return Ok(new { token });
            }
            else
            {
                return BadRequest(new { message = "Username or Password is incorrect." });
            }
        }

        // Users forgot password control to send mail to the registered mail.
        //POST :/api/ApplicationUser/Forgotpassword
        [HttpPost]
        [Route("Forgotpassword")]
        public async Task<IActionResult> Forgotpassword(ApplicationUser userdata)
        {
            var user = await _userManager.FindByEmailAsync(userdata.Email);
            // If cant find user with the provided mail id it returns Badrequest.
            if (user == null)
            {
                return BadRequest("The mail id dosen't Exist");
            }
            else
            {                
                String token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(token);
                var encodeToken = System.Convert.ToBase64String(plainTextBytes);



                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("mailid");
                mail.To.Add(user.Email);
                mail.Subject = "Forgot Password";
                mail.Body = $"Click <a href = 'http://localhost:4200/resetpassword?token={encodeToken}'> here </a> to Rest your password";
                mail.IsBodyHtml = true;
                SmtpServer.Port = 25;
                SmtpServer.Credentials = new System.Net.NetworkCredential("mailid", "password");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);

                return Ok("Please Check your Email To restPassword"+token);
            }
        }

        // User Reset Password to change the users Password in database.
        //POST :/api/ApplicationUser/ResetPassword
        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> RestPassword(ResetPasswordModel userchangepassword)
        {

                var base64EncodedBytes = System.Convert.FromBase64String(userchangepassword.Token);
                var decrptToken = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            

            if (userchangepassword.Token == null)
            {
                return BadRequest("Invalid user");
            }
            else
            {
                var user = await _userManager.FindByEmailAsync(userchangepassword.Email);
                if (user == null)
                {
                    return BadRequest("The User with specified email id dosen't Exist");
                }
                else
                {
                    var changepassword = await _userManager.ResetPasswordAsync(user,decrptToken, userchangepassword.password);
                        if (changepassword.Succeeded)
                        {
                            return Ok("The password has been changed succesfully");
                        }
                        else
                        {
                            return BadRequest("something went wrong");
                        }
                   
                }
            }

        }








    }
}
