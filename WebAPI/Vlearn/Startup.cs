using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;
using VShop.Models;

namespace VShop
{
    // Intial loadup where middleware and services added to app pipeline.
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Adding swagger to make request call's and other features.
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "E_gen_Coach", Version = "v1" });
            });
            
            // Add Controllers to the app.
            services.AddControllers();

            // Adding resourse sharing serrvice to the app to make data transfer for front end and back (cross origin resourse sharing).
            services.AddCors();

            // Adding JWT 
            var key = Encoding.UTF8.GetBytes(Configuration["ApplicationSettings:JWTSecret"].ToString());
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(option =>
            {
                option.RequireHttpsMetadata = false;
                option.SaveToken = false;
                option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });

            // Adding entity framework to theSql database.
            services.AddDbContext<AuthenticationContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection")));
            
            // Adding identity and role based system with entity framework to the app.
            services.AddDefaultIdentity<ApplicationUser>().AddRoles<IdentityRole>().AddEntityFrameworkStores<AuthenticationContext>();

            // Constomizing Password field.
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredLength = 4;   
            }           
            );       


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Udender development process swagger is called to the pipeline.
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "e_gen_coach"));
            }

            // Adds Authentication middleware to the pipeline.
            app.UseAuthentication();

            

            // Adds the endpoint routing middleware to the pipeline of the app.
            app.UseRouting();

            // This middleware Allows cross domain request.
            app.UseCors(builder =>
            builder.WithOrigins(Configuration["ApplicationSettings:ClientURL"].ToString())            
            .AllowAnyHeader()
            .AllowAnyMethod()            
            );
            app.UseAuthorization();

            // Uses Routing to enpoint specification for controller mapping to the app.
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
        }
    }
}
