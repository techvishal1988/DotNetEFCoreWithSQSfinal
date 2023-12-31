namespace Geography.Service
{
    using Amazon.SQS.Model;
    using Framework.Business.ServiceProvider.Queue;
    using Framework.Business.ServiceProvider.Storage;
    using Framework.Configuration.Models;
    using Framework.Service;
    using Framework.Service.Extension;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="Startup" />.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration<see cref="IConfiguration"/>.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the Configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// The ConfigureServices.
        /// </summary>
        /// <param name="services">The services<see cref="IServiceCollection"/>.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped(typeof(IStorageManager<AmazonS3ConfigurationOptions>), typeof(StorageManager));
            services.AddScoped(typeof(IQueueManager<AmazonSQSConfigurationOptions, List<Message>>), typeof(QueueManager));
            //services.AddControllersWithViews().AddNewtonsoftJson();
            services.AddControllersWithViews();
            services.ConfigureClientServices();
            services.ConfigureSwagger();
        }

        /// <summary>
        /// The Configure.
        /// </summary>
        /// <param name="app">The app<see cref="IApplicationBuilder"/>.</param>
        /// <param name="env">The env<see cref="IWebHostEnvironment"/>.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.AddProblemDetailsSupport();

            app.UseSwagger(new[]
                   {
                      new SwaggerConfigurationModel(ApiConstants.ApiVersion, ApiConstants.ApiName, true),
                      new SwaggerConfigurationModel(ApiConstants.JobsApiVersion, ApiConstants.JobsApiName, false)
                    });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
