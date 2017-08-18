using System;
using Google.Apis.Auth.OAuth2;
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

namespace MemoryServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<Config>(Configuration.Bind);
            services.AddDbContextPool<MemoryContext>(
                options => options.UseNpgsql(Configuration["database"], b => b.MigrationsAssembly("MemoryServer")));

            services.AddIdentity<User, DummyRole>()
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

            if (Configuration["useCloudStorage"] == "true")
            {
                services.AddDistributedCloudStorageCache(options =>
                {
                    options.BucketName = Configuration["storageBucketName"];
                    options.ProjectName = Configuration["projectId"];
                    options.Credentials = GoogleCredential.GetApplicationDefault();
                });
            }
            else
            {
                services.AddDistributedRedisCache(options =>
                {
                    options.Configuration = Configuration["redisHost"];
                    options.InstanceName = "ReviewStore";
                });
            }

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
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseAuthentication();

            app.UseMvcWithDefaultRoute();
        }
    }
}
