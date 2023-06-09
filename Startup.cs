﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Projet2_EasyFid.Models;
using Projet2_EasyFid.Data;
using Microsoft.Extensions.FileSystemGlobbing.Internal;

namespace Projet2_EasyFid
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {


            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Login/Index";

            });
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Initialisation de la BDD pour la phase de test, à supprimer pour la phase prod
            using (BddContext ctx = new BddContext())
            {
                ctx.InitialiseDb();
            }


            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // Première route visible avec l'adresse url vide
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Index}/{id?}");

                // Route de la partie Salarie (/salarie)
                endpoints.MapControllerRoute(
                    name: "adminHome",
                    pattern: "{controller=Salarie}/{action=Index}/{id?}");

                // Route de la partie Admin (/admin)
                endpoints.MapControllerRoute(
                    name: "adminHome",
                    pattern: "{controller=Admin}/{action=Index}/{id?}");
            });
        }
    }
}

