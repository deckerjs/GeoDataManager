using System.IO;
using GeoStoreAPI.DataAccess;
using GeoStoreAPI.Models;
using GeoStoreAPI.Repositories;
using GeoStoreAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CustomIdentityServerBuilderExtensions
    {
        private const string IDSOPTIONS = "IDSOptions";
        private const string AUTH_URL = "AuthURL";
        private const string GEOMGRAPI = "geomgrapi";

        public static IServiceCollection AddIdentityServerServices(this IServiceCollection services,IConfiguration configuration, IWebHostEnvironment env){

            IDSOptions idsOptions = configuration.GetSection(IDSOPTIONS).Get<IDSOptions>();

            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                if (!string.IsNullOrEmpty(idsOptions.PublicOrigin))
                {
                    options.PublicOrigin = idsOptions.PublicOrigin;
                }
                if (!string.IsNullOrEmpty(idsOptions.IssuerUri))
                {
                    options.IssuerUri = idsOptions.IssuerUri;
                }
            });

            builder.AddInMemoryIdentityResources(IDSConfig.GetIdentityResources());
            builder.AddInMemoryApiResources(IDSConfig.GetApis());
            builder.AddInMemoryClients(IDSConfig.GetClients(configuration));

            if (env.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                //todo: get cert for signing
                builder.AddDeveloperSigningCredential();
                //.AddSigningCredential(new X509Certificate2(Path.Combine(".", "certs", "IdentityServer4Auth.pfx")))
            }

            builder.AddIdentityServices();

            var authUrl = configuration.GetValue<string>(AUTH_URL);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = authUrl;
                options.Audience = GEOMGRAPI;
                options.RequireHttpsMetadata = false;
            });

            return services;
        }

        public static IIdentityServerBuilder AddIdentityServices(this IIdentityServerBuilder builder)
        {
            builder.AddProfileService<CustomProfileService>();
            builder.AddResourceOwnerValidator<CustomResourceOwnerPasswordValidator>();
            builder.Services.AddScoped<IUserIdentificationService, UserIdentificationService>();

            return builder;
        }
    }
}