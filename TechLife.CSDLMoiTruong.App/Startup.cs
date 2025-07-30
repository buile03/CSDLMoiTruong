using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TechLife.CSDLMoiTruong.App.ApiClients;
using TechLife.CSDLMoiTruong.App.Extensions;
using TechLife.CSDLMoiTruong.App.Extensions.Authorizations;
using TechLife.CSDLMoiTruong.App.Extensions.FileLogger;
using TechLife.CSDLMoiTruong.Common;
using TechLife.CSDLMoiTruong.Data.EF;
using TechLife.CSDLMoiTruong.Service;

namespace TechLife.CSDLMoiTruong.App
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
            services.AddHttpClient();

            //services.AddAntiforgery(o => o.SuppressXFrameOptionsHeader = true);
            services.AddAntiforgery(options =>
            {
                options.SuppressXFrameOptionsHeader = true;
            });
            services.AddControllersWithViews();

            services.AddMvc().AddSessionStateTempDataProvider();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            });

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString(SystemConstants.MainConnectionString)), ServiceLifetime.Transient);

            services.Configure<IdentityOptions>(options =>
            {
                // Default Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = ".NhuanBut.Identity";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);

                options.LoginPath = new PathString($"{Configuration["Login:Url"]}");
                options.AccessDeniedPath = new PathString("/AccessDenied");
                options.SlidingExpiration = true;
            });
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            services.AddAuthentication(options =>
            {
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.LoginPath = new PathString($"{Configuration["Login:Url"]}");
                options.AccessDeniedPath = "/AccessDenied";
                options.Events.OnRedirectToLogin = context =>
                {
                    if (context.Request.Path.StartsWithSegments("/api")
                            && context.Response.StatusCode == StatusCodes.Status200OK)
                    {
                        context.Response.Clear();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return Task.CompletedTask;
                    }
                    context.Response.Redirect(context.RedirectUri);
                    return Task.CompletedTask;
                };
            });
            services.AddInfrastructure();
            services.AddHttpContextAccessor();

            services.AddTransient(typeof(ILogger<>), (typeof(Logger<>)));
            services.AddTransient<IAuthorizationHandler, AuthorizationHandler>();
            services.AddTransient<IUserApiClient, UserApiClient>();
            


            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 200 * 1024 * 1024; // 200 MB
            });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("*")
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials(); // nếu dùng cookie
                });
            });

            IMvcBuilder builder = services.AddRazorPages();

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

#if DEBUG
            if (environment == Environments.Development)
            {
                builder.AddRazorRuntimeCompilation();
            }
#endif
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, AppDbContext dataContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();

                dataContext.Database.Migrate();
                loggerFactory.AddFile(Directory.GetCurrentDirectory());

                app.Use(async (context, next) =>
                {
                    await next();
                    if (context.Response.StatusCode == 404)
                    {
                        context.Request.Path = "/NotFound";
                        await next();
                    }
                });
                app.Use(async (context, next) =>
                {
                    await next();
                    if (context.Response.StatusCode == 400)
                    {
                        context.Request.Path = "/NotFound";
                        await next();
                    }
                });
                app.Use(async (context, next) =>
                {
                    await next();
                    if (context.Response.StatusCode == 500)
                    {
                        context.Request.Path = "/Error";
                        await next();
                    }
                });
                app.Use(async (context, next) =>
                {
                    if (context.Request.Path.Value.EndsWith(".txt"))
                    {
                        context.Response.StatusCode = 403; // Forbidden
                        context.Request.Path = "/AccessDenied";
                        return;
                    }
                    await next();
                });
            }

            app.UseCookiePolicy();
            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=31536000");
                }
            });
            app.UseRouting();
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

            });
        }
    }
}