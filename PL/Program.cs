using BLL.Interfaces;
using BLL.Repositries;
using DAL.Data;
using DAL.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PL.Helper;
using PL.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace PL
{
    public class Program
    {
		//Entry Point
        public static void Main(string[] args)
        {
            var Builder = WebApplication.CreateBuilder(args);

			#region Configure Services That's Allow Dependency Injection
			Builder.Services.AddControllersWithViews();

			Builder.Services.AddDbContext<AppDbContext>(options =>
			{
				options.UseSqlServer(Builder.Configuration.GetConnectionString("DefaultConnection"));
			}); //Allow DI 

			//services.AddAutoMapper(typeof(MappingProfiles));
			Builder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));

			//services.AddScoped<IDepartmentRepositry,DepartmentRepositry>();
			//services.AddScoped<IEmployeeRepositry,EmployeeRepositry>();
			Builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

			//services.AddScoped();     //Lifetime Per Request - After Request Object be UnReachable
			//services.AddTransient();  //Lifetime Per Operation
			//services.AddSingleton();  //Lifetime Per Application

			Builder.Services.AddScoped<IScopedService, ScopedService>();          //Per Request
			Builder.Services.AddTransient<ITransientService, TransientService>(); //Per Operation
			Builder.Services.AddSingleton<ISingeltonService, SingletonService>(); //Per App

			//services.AddScoped<UserManager<ApplicationUser>>();
			//services.AddScoped<SignInManager<ApplicationUser>>();

			Builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
					.AddEntityFrameworkStores<AppDbContext>()
					.AddDefaultTokenProviders();

			Builder.Services.ConfigureApplicationCookie(config =>
			{
				config.LoginPath = "/Account/SignIn";
			});
			#endregion

			var app = Builder.Build();

			#region Configure HTTP Request Middelwares

			if (app.Environment.IsDevelopment())
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
			#endregion

			app.Run();

		}

        
    }
}
