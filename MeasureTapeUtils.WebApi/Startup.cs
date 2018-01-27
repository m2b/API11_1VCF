using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ITVizion.Tools.Core.Logging;
using System.Reflection;
using APIVCF;

namespace MeasureTapeUtils.WebApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            // Add TapeUtilsApi service
            services.AddScoped<ITapeMeasureAPI,TapeMeasureAPI>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            // Log4Net logging
            loggerFactory.AddLog4Net(typeof(Controllers.TapeUtilsController).GetTypeInfo().Assembly);
            loggerFactory.AddDebug();

            // Use middleware to return excepton messages
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            // Routes are defined in controller methods
            app.UseMvc();
        }
    }
}
