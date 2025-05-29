using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Data;
using Web;
using Project.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Web.Controllers;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Http.Features;
using System.Text.Json.Serialization;
using static Data.GeneralEnums;
using CommonClasses;
using Microsoft.AspNetCore.Mvc;
using DinkToPdf.Contracts;
using DinkToPdf;
using static Data.RoleEnums;
using System.Linq;
using Data.DataContexts;


namespace DotNet5Template
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the padding.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null; // Ensure property names match
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); // If you have enum values

            });

            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = 1000000000;
            });

            services.Configure<MvcOptions>(options =>
            {
                options.MaxModelBindingCollectionSize = 1000000000;
            });

            services.Configure<FormOptions>(options =>
            {
                // Set the limit to 128 MB
                options.MultipartBodyLengthLimit = 1000000000;
                options.ValueCountLimit = 50000;
            });

            services.AddSingleton<IConverter, SynchronizedConverter>(
             provider => new SynchronizedConverter(new PdfTools())
            );

            //test
            services.AddDbContext<CMGContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("CMG"), sql => sql.CommandTimeout(300)));
            services.AddDbContext<MSContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("MS"), sql => sql.CommandTimeout(300)));
            services.AddDbContext<ADHEREContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ADHERE"), sql => sql.CommandTimeout(300)));
            services.AddDbContext<ACMEDIAContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ACMEDIA"), sql => sql.CommandTimeout(300)));
            services.AddDbContext<PROMKTContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("PROMKT"), sql => sql.CommandTimeout(300)));
            services.AddDbContext<CMContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("CM"), sql => sql.CommandTimeout(300)));

            services.AddDbContext<TargetDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("TargetDb"), sql => sql.CommandTimeout(300)));

            //test

            /* Helpers */
            GoogleAPI.Initialize();
            MicrosoftAPI.Initialize();
            services.AddScoped<IViewRenderService, ViewRenderService>();
            services.AddScoped<RazorViewToStringRenderer, RazorViewToStringRenderer>();
            services.AddScoped<ExceptionLogger, ExceptionLogger>();
            services.AddScoped<Email, Email>();

            /* Template */
            services.AddScoped<TemplateDataLibrary, TemplateDataLibrary>();

            /* Exceptions & Audit*/
            services.AddScoped<ExceptionLogDataLibrary, ExceptionLogDataLibrary>();

            /* Company */
            services.AddScoped<CompanyDataLibrary, CompanyDataLibrary>();
            services.AddScoped<CompanyRolesDataLibrary, CompanyRolesDataLibrary>();
            services.AddScoped<CompanyIntegrationDataLibrary, CompanyIntegrationDataLibrary>();
            services.AddScoped<EmailDataLibrary, EmailDataLibrary>();

            /* System Admin */

            /* Users */
            services.AddScoped<UserDataLibrary, UserDataLibrary>();
            services.AddScoped<StaffGroupDataLibrary, StaffGroupDataLibrary>();

            //Contact
            services.AddScoped<DocumentDataLibrary, DocumentDataLibrary>();

            //Customers
            services.AddScoped<CustomerDataLibrary, CustomerDataLibrary>();
         
            //Levels
            services.AddScoped<DegreeLevelDataLibrary, DegreeLevelDataLibrary>();

            //AreaOfInterest
            services.AddScoped<AreaOfInterestDataLibrary, AreaOfInterestDataLibrary>();



            //Sources
            services.AddScoped<SourcesDataLibrary, SourcesDataLibrary>();
            services.AddScoped<AllocationDataLibrary, AllocationDataLibrary>();
            services.AddScoped<SchoolsDataLibrary, SchoolsDataLibrary>();
            services.AddScoped<CampusDataLibrary, CampusDataLibrary>();
            services.AddScoped<GroupDataLibrary, GroupDataLibrary>();

            //NewDataLibrary

            services.AddDbContextPool<DataContext>(options => options.UseSqlServer(CommonClasses.Environment.DBConnection()));
            //test
            services.AddTransient<MigrationService>();
            //test


            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
          .AddEntityFrameworkStores<Data.DataContext>();

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("Admin", policy => policy.RequireClaim("Admin"));
            //    options.AddPolicy("Office", policy => policy.RequireClaim("Office"));
            //});


            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireClaim("Admin"));

                foreach (var permission in Enum.GetValues(typeof(Permission)).Cast<Permission>())
                {
                    string permissionName = permission.ToString();
                    string policyName = $"AdminOr{permissionName}";

                    options.AddPolicy(policyName, policy =>
                        policy.RequireAssertion(context =>
                            context.User.HasClaim(c => c.Type.Contains(permissionName)) ||
                            context.User.HasClaim(c => c.Type.Contains("SystemAdmin")) ||
                            context.User.HasClaim(c => c.Type.Contains("Admin"))
                        ));
                }

                options.AddPolicy("SystemAdmin", policy =>
                    policy.RequireAssertion(context =>
                        context.User.HasClaim(c => c.Type.Contains("SystemAdmin"))
                    ));
            });

            services.AddSession();

            services.AddRazorPages();

            services.AddSignalR();

            services.AddMvc(option => option.EnableEndpointRouting = false);

            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            services.Configure<RazorViewEngineOptions>(o =>
            {
                o.ViewLocationFormats.Clear();
                o.ViewLocationFormats.Add("/Pages/{1}/{0}" + RazorViewEngine.ViewExtension);
                o.ViewLocationFormats.Add("/Website/{1}/{0}" + RazorViewEngine.ViewExtension);
                o.ViewLocationFormats.Add("/Pages/Templates/{1}/{0}" + RazorViewEngine.ViewExtension);
                o.ViewLocationFormats.Add("/Pages/CompanyAdmin/{1}/{0}" + RazorViewEngine.ViewExtension);
                o.ViewLocationFormats.Add("/Pages/SystemAdmin/{1}/{0}" + RazorViewEngine.ViewExtension);
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            //test
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var migrationService = scope.ServiceProvider.GetRequiredService<MigrationService>();

                // Run your migrations
                //migrationService.MigrateClients();//done
                //migrationService.MigrateSchools();//done
                //migrationService.MigrateOffers();//done
                //migrationService.MigrateStates();//done
                //migrationService.MigratePostalCodes();//done
                //migrationService.MigrateCampuses();//done
                //migrationService.MigrateLevels();//done
                //migrationService.MigratePrograms();//done
                //migrationService.MigrateDegreePrograms();//done
                //migrationService.MigrateCampusDegrees();//done
                //migrationService.MigrateSources();//done
                //migrationService.MigrateAllocations();//done
                //migrationService.MigrateCampusPostalCodes();//remain
                //migrationService.MigrateDownSellOffers();//done             
                //migrationService.MigrateDownSellOfferPostalCodes();//remain
                //migrationService.MigrateMasterSchools();//done
                //migrationService.MigrateMasterSchoolMappings();//done
                //migrationService.MigrateAreas();//done
                //migrationService.MigrateProgramAreas();//done
                //migrationService.MigrateInterests();//done
                //migrationService.MigrateProgramInterests();//done
                //migrationService.MigrateGroups();//done
                //migrationService.MigrateSchoolGroups();//done
                //migrationService.MigrateExtraRequiredEducation();//done
                //migrationService.MigrateLeadPosts();//done
                //migrationService.MigrateOfferTargeting();//done
                //migrationService.MigratePingCache();//done
                //migrationService.MigratePortalTargeting();//done
                //migrationService.MigrateSearchPortals();//done
                //migrationService.MigrateConfigEducationLevels();//done



            }
            //test
            app.UseExceptionHandler("/Error");
            app.UseHsts();


            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();  // Enable authorization middleware


            app.UseEndpoints(endpoints =>
            {
                // API route configuration using attribute routing
                endpoints.MapControllers(); // This will map attribute-routed controllers, typically API controllers.
                endpoints.MapHub<NotificationHub>("/notificationHub");

                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Index}/{id?}");


                // MVC route configuration

            });
        }

    }
}
