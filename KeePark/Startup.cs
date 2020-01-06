using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using KeePark.Models;
using KeePark.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace KeePark
{
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddRazorPagesOptions(options =>
                {
                    options.AllowAreas = true;
                    options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
                    options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout");
                });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });


            services.AddAuthorization();

            services.AddDbContext<KeeParkContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("KeeParkContext")));

            services.AddDbContext<IdentityContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("IdentityContext")));

            services.AddIdentity<GeneralUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders()
                .AddDefaultUI();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithRedirects("/Error/{0}");
                
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseCookiePolicy();


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            CreateRoles(serviceProvider);

        }

        private void CreateRoles(IServiceProvider serviceProvider)
        {

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<GeneralUser>>();
            Task<IdentityResult> roleResult;
            string email = "keepark@keepark.com";
            string userEmail = "peter@peter.com";
            GeneralUser user;
            GeneralUser gUser;

            //Check that there is an Administrator role and create if not
            //   try
            //  {
            Task<bool> hasAdminRole = roleManager.RoleExistsAsync("Administrator");
                hasAdminRole.Wait();

                if (!hasAdminRole.Result)
                {
                    roleResult = roleManager.CreateAsync(new IdentityRole("Administrator"));
                    roleResult.Wait();
                }

                //Check if the admin user exists and create it if not
                //Add to the Administrator role

                Task<GeneralUser> testUser = userManager.FindByEmailAsync(email);
                testUser.Wait();
                user = testUser.Result;

                if (user == null)
                {
                    user = CreateAdminAsync(email, userManager).Result;
                }
                if (!userManager.IsInRoleAsync(user, "Administrator").Result)
                {
                    Task<IdentityResult> newUserRole = userManager.AddToRoleAsync(user, "Administrator");
                    newUserRole.Wait();
                }

                 Task<GeneralUser> tUser = userManager.FindByEmailAsync(userEmail);
                 tUser.Wait();
                 gUser = testUser.Result;
                  if (gUser == null)
                  {
                     gUser = CreateUserAsync(userEmail, userManager).Result;
                   }


            //   }
            // catch (Exception ex)
            // {

            // }

        }

        private async Task<GeneralUser> CreateAdminAsync(string email, UserManager<GeneralUser> userManager)
        {
            KeePark.Areas.Identity.Pages.Account.RegisterModel.InputModel model = new KeePark.Areas.Identity.Pages.Account.RegisterModel.InputModel
            {
                UID = "111111111",
                FirstName = "Admin",
                LastName = "Admin",
                PhoneNumber = "0524897653",
                CarNumber = "7777777",
                CarType = "Admin",
                CreditCard = "12341234123456",
                Balance = 0,
                ConfirmPassword = "Ad7&Ad",
                Password = "Ad7&Ad",
                Email = email,
                Address = "Rishon Lezion"
            };
            var user = new GeneralUser { UserName = model.Email, Email = model.Email, Address = model.Address, Balance = model.Balance, CarNumber = model.CarNumber, CarType = model.CarType, CreditCard = model.CreditCard, FirstName = model.FirstName, LastName = model.LastName, UID = model.UID };
            //var s=new KeePark.
            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return user;
            }
            return null;

        }

        private async Task<GeneralUser> CreateUserAsync(string email, UserManager<GeneralUser> userManager)
        {
            KeePark.Areas.Identity.Pages.Account.RegisterModel.InputModel model = new KeePark.Areas.Identity.Pages.Account.RegisterModel.InputModel
            {
                UID = "326680978",
                FirstName = "Peter",
                LastName = "Parker",
                PhoneNumber = "0547654332",
                CarNumber = "2367892",
                CarType = "bmw",
                CreditCard = "23466783",
                Balance = 900,
                ConfirmPassword = "Ad7&Ad",
                Password = "Ad7&Ad",
                Email = email,
                Address = "New York"
            };

            var user = new GeneralUser { UserName = model.Email, Email = model.Email, Address = model.Address, Balance = model.Balance, CarNumber = model.CarNumber, CarType = model.CarType, CreditCard = model.CreditCard, FirstName = model.FirstName, LastName = model.LastName, UID = model.UID, History= "5,5,5,6,8,8,8,10" };
            //var s=new KeePark.
            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return user;
            }
            return null;

        }






    }
}