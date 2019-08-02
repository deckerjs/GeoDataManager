using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GeoStoreAPI.DataAccess;
using GeoStoreAPI.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using GeoStoreAPI.Services;

namespace geostoreapi
{
    public class Startup
    {
        public IHostingEnvironment Environment { get; set; }
        public Startup(IHostingEnvironment environment, IConfiguration configuration)
        {
            this.Environment = environment;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            .AddJsonOptions(x => x.UseMemberCasing());
            
            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            });

            builder.AddInMemoryIdentityResources(IDSConfig.GetIdentityResources());
            builder.AddInMemoryApiResources(IDSConfig.GetApis());
            builder.AddInMemoryClients(IDSConfig.GetClients(Configuration));
            builder.AddCustomUserStore();

            if (Environment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                //todo: get cert for signing
                builder.AddDeveloperSigningCredential();
                //.AddSigningCredential(new X509Certificate2(Path.Combine(".", "certs", "IdentityServer4Auth.pfx")))
            }

            //todo: add google authentication
            //services.AddAuthentication()
            //.AddGoogle(options =>
            //{
            //    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
            //    // register your IdentityServer with Google at https://console.developers.google.com
            //    // enable the Google+ API
            //    // set the redirect URI to http://localhost:5000/signin-google
            //    options.ClientId = "copy client ID from Google here";
            //    options.ClientSecret = "copy client secret from Google here";
            //});

            var authUrl = Configuration.GetValue<string>("AuthURL");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = authUrl;
                options.Audience = "geomgrapi";
                options.RequireHttpsMetadata = false;
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", p =>
                {
                    p.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            services.AddSingleton<IFileDataAccess<GeoData>>(x => new FileDataAccess<GeoData>(Path.Combine(Directory.GetCurrentDirectory(), "Data")));
            services.AddSingleton<IFileDataAccess<AppUser>>(x => new FileDataAccess<AppUser>(Path.Combine(Directory.GetCurrentDirectory(), "UserData")));            
            services.AddSingleton<IGeoDataAccess, GeoDataAccess>();
            services.AddSingleton<IUserDataAccess, UserDataAccess>();                        
            services.AddSingleton<IGeoDataRepository, GeoDataRepository>();
            
            services.AddScoped<IUserIdentificationService, UserIdentificationService>();

            services.AddHttpContextAccessor();
        }
                
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseCors("AllowAll");            
            app.UseIdentityServer();
            app.UseMvc();
        }

        //todo: configure encryption
        // private void ConfigureEncryption(IServiceCollection services)
        // {
        //     string azVaultKeyID = Configuration.GetSection("DataProtectionSettings:azVaultKeyID").Value;
        //     string appClientID = Configuration.GetSection("DataProtectionSettings:appClientID").Value;
        //     string appClientSecret = Configuration.GetSection("DataProtectionSettings:appClientSecret").Value;
        //     string storageURI = Configuration.GetSection("DataProtectionSettings:storageURI").Value;
        //     string storageAccount = Configuration.GetSection("DataProtectionSettings:storageAccount").Value;
        //     string storageAccessKey = Configuration.GetSection("DataProtectionSettings:storageAccessKey").Value;
        //     string dataProtectionAppName = Configuration.GetSection("DataProtectionSettings:dataProtectionAppName").Value;
        //     string dataProtectionKeyfileName = Configuration.GetSection("DataProtectionSettings:dataProtectionKeyfileName").Value;
        //     var container = new CloudBlobContainer(
        //         new Uri(storageURI),
        //         new StorageCredentials(storageAccount, storageAccessKey));
        //     services.AddDataProtection()
        //         .SetApplicationName(dataProtectionAppName)
        //         .PersistKeysToAzureBlobStorage(container, dataProtectionKeyfileName)
        //         .ProtectKeysWithAzureKeyVault(azVaultKeyID, appClientID, appClientSecret);
        // }

    }
}