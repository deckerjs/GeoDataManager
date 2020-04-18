using System;
using System.IO;
using GeoStoreAPI.DataAccess;
using GeoStoreAPI.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;
using GeoStoreAPI.Services;
using GeoStoreAPI.Repositories;
using Microsoft.OpenApi.Models;
using GeoDataModels.Models;
using DataTransformUtilities.Transformers;
using Newtonsoft.Json.Serialization;

namespace geostoreapi
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        private IWebHostEnvironment Environment {get;}

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentityServerServices(Configuration,Environment);

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", p =>
                {
                    p.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            services.AddScoped<IFileDataAccess<GeoData>>(x => new FileDataAccess<GeoData>(Path.Combine(Directory.GetCurrentDirectory(), FileDataAccess<GeoData>.BASE_DIR,GeoDataFileDataAccess.GEODATA)));
            services.AddScoped<IGeoDataAccess, GeoDataFileDataAccess>();
            services.AddScoped<IGeoDataRepository, GeoDataRepository>();
            services.AddScoped<IGPXTransform, GPXTransform>();
            services.AddScoped<IQueryStringFilterBuilderService, QueryStringFilterBuilderService>();
        
            services.Configure<AppOptions>(Configuration.GetSection("AppOptions"));
            services.AddScoped<AppOptions>(x => x.GetService<IOptions<AppOptions>>().Value);

            services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "GeoStoreAPI", Version = "v1" });
                });

            services.AddControllers()
                .AddNewtonsoftJson(o=> o.UseMemberCasing())
                .AddXmlSerializerFormatters()
                .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);            


            services.AddHealthChecks().AddCheck<APIHealthCheck>("apiCheck");
            services.AddHttpContextAccessor();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseCors("AllowAll");
            app.UseIdentityServer();
            
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
                {
                    endpoints.MapHealthChecks("/health");
                    endpoints.MapControllers();
                });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "GeoStoreAPI V1");
            });
            
            app.SeedUserData(serviceProvider);
        }

    }
}