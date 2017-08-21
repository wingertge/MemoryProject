using System;
using JetBrains.Annotations;
using MemoryCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MemoryClient.Web
{
    public class Startup
    {
        public Startup([NotNull] IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry(Configuration["AppInsightsKey"]);

            services.AddTransient<IApiAccess, ApiAccess>();
            services.AddSingleton(Configuration);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if(Configuration["runProxy"] == "true") //using nginx in production
            {
                app.MapWhen(IsApiCall, builder => builder.RunProxy(new ProxyOptions
                {
                    Host = Configuration["ApiHost"],
                    Port = Configuration["ApiPort"],
                    Scheme = Configuration["ApiProtocol"]
                }));
            }

            app.UseStaticFiles();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }

        private static bool IsApiCall([NotNull] HttpContext context)
        {
            return context.Request.Path.Value.StartsWith(@"/api/", StringComparison.OrdinalIgnoreCase);
        }
    }
}
