using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Project.BLL.Interfaces;
using Project.BLL.Repositories;
using Project.DAL.Data;
using Project.DAL.Models;
using Project.PL.Extensions;
using Project.PL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.PL
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
            services.AddControllersWithViews();

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            //ApplicationServicesExtension.AddApplicationServices(services);
            services.AddApplicationServices();

            services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));

            services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.Password.RequiredUniqueChars = 2;
                config.Password.RequireDigit = true;
                config.Password.RequireNonAlphanumeric = true;
                config.Password.RequiredLength = 5;
                config.Password.RequireLowercase = true;
                config.Password.RequireUppercase = true;
                config.User.RequireUniqueEmail = true;
                config.Lockout.MaxFailedAccessAttempts = 4;


            } )
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Account/SignIn";
            });

           

            //services.AddAuthentication("Cookies")
            //    .AddCookie("Default", config =>
            //    {
            //        config.LoginPath = "/Account/SignIn";
            //        config.AccessDeniedPath = "/Home/Error";
            //    });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
