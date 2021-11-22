using Awesomek8.Core;
using k8s;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Awesomek8
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
            services.AddControllers();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc($"v1", new OpenApiInfo
                {
                    Title = $"Awesomek8 Api",
                    Version = $"v1"
                });
            });

            services.AddScoped<IKubernetesClient, KubernetesClient>(services =>
            {
                KubernetesClientConfiguration config = KubernetesClientConfiguration.BuildDefaultConfig();
                Kubernetes client = new Kubernetes(config);
                ILogger<KubernetesClient> logger = services.GetService<ILogger<KubernetesClient>>();
                return new KubernetesClient(client, logger);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage()
               .UseSwagger(options =>
               {
                   options.RouteTemplate = "/swagger/{documentName}/swagger.{json|yaml}";
               })
               .UseSwaggerUI(options =>
               {
                   options.EnableFilter();
                   options.ShowExtensions();
                   options.SwaggerEndpoint($"/swagger/v1/swagger.json", $"Awesomek8 Docs");
               });

            app.UseExceptionHandler("/Error");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
