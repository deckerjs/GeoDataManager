using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataTransformUtilities.Transformers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DataConversionAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            IMvcBuilder builder = services.AddMvc();
            builder.SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            builder.AddXmlSerializerFormatters();

            services.AddScoped<IGPXTransform, GPXTransform>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {

            }

            app.UseMvc();
            
        }
    }
}
