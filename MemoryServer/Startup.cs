using System;
using System.Globalization;
using MemoryCore.DbModels;
using MemoryServer.Core.Business;
using MemoryServer.Core.Business.Impl;
using MemoryServer.Core.Database;
using MemoryServer.Core.Database.Impl;
using MemoryServer.Core.Database.Repositories;
using MemoryServer.Core.Database.Repositories.Impl;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;

namespace MemoryServer
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

            if (env.IsDevelopment()) builder.AddUserSecrets<Startup>();

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddOptions();
            services.Configure<Config>(Configuration.Bind);
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddDbContext<MemoryContext>(
                options => options.UseNpgsql(Configuration["database"], b => b.MigrationsAssembly("MemoryServer")));

            services.AddIdentity<User, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<MemoryContext>()
                .AddDefaultTokenProviders();

            /** Other **/
            services.AddSingleton<IScheduler, BasicScheduler>();
            services.AddScoped<ITransactionFactory<MemoryContext>, TransactionFactory<MemoryContext>>();
            
            /** Data Storage **/
            services.AddScoped<IAssignmentRepository, AssignmentRepository>();
            services.AddScoped<ILanguageRepository, LanguageRepository>();
            services.AddScoped<ILessonRepository, LessonRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IReviewStore, ReviewStore>();
            services.AddScoped<ILessonReviewStore, LessonReviewStore>();
            
            /** Business Logic **/
            services.AddScoped<ILessonService, LessonService>();
            services.AddScoped<ILanguageService, LanguageService>();
            services.AddScoped<IAutocompleteService, AutocompleteService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IThemeService, ThemeService>();

            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = Configuration["redisHost"];
                options.InstanceName = "ReviewStore";
            });
            
            // Add framework services.
            services.AddMvc();

            services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromDays(150);
                options.LoginPath = "/api/auth/login";
                options.LogoutPath = "/api/auth/logout";
            });
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;

                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler("/Home/Error");

            app.UseAuthentication();

            app.UseMvcWithDefaultRoute();
        }
    }
}
