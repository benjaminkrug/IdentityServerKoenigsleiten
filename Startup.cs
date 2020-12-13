namespace IdentityServerKoenigsleiten
{
    using identityServer.Data;
    using IdentityServer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;

    public class Startup
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration config, IWebHostEnvironment env)
        {
            _config = config;
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = _config.GetConnectionString("DefaultConnection");

            services.AddDbContext<AppDbContext>(config =>
            {
                //config.UseSqlServer(connectionString); in postgresql ändern
                config.UseInMemoryDatabase("Memory");
            });

            //AddIdentity register the services
            services.AddIdentity<IdentityUser, IdentityRole>(config =>
            {
                config.Password.RequiredLength = 4;
                config.Password.RequireDigit = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "IdentityServer.Cookie";
                config.LoginPath = "/Auth/Login";
                config.LogoutPath = "/Auth/Logout";
            });

            var assembly = typeof(Startup).Assembly.GetName().Name;

            X509Certificate2 certificate;
            if (_env.IsDevelopment())
            {
                var file = Path.Combine(_env.ContentRootPath, "is_cert.pfx");
                certificate = new X509Certificate2(file, "password");
            }
            else
            {
                X509Store certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                X509Certificate2Collection certCollection = certStore.Certificates.Find(
                                            X509FindType.FindByThumbprint,
                                            "FE1A8F62DAB5AB8EC767A2A70178BA790A427546",
                                            false);
                // Get the first cert with the thumbprint
                certificate = (X509Certificate2)certCollection.OfType<X509Certificate>().FirstOrDefault();

            }

            services.AddIdentityServer()
                .AddAspNetIdentity<IdentityUser>()
                //.AddConfigurationStore(options =>
                //{
                //    options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
                //        sql => sql.MigrationsAssembly(assembly));
                //})
                //.AddOperationalStore(options =>
                //{
                //    options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
                //        sql => sql.MigrationsAssembly(assembly));
                //})
                .AddInMemoryApiResources(Configuration.GetApis())
                .AddInMemoryIdentityResources(Configuration.GetIdentityResources())
                .AddInMemoryClients(Configuration.GetClients())
                .AddSigningCredential(certificate);
                //.AddDeveloperSigningCredential();

            services.AddAuthentication();
            //    .AddFacebook(config =>
            //    {
            //        config.AppId = "389241792326539";
            //        config.AppSecret = "9a134b712c65508f531ff1dcf32e867c";
            //    });

            services.AddControllersWithViews();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
