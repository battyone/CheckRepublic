using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using ZNetCS.AspNetCore.Authentication.Basic;
using ZNetCS.AspNetCore.Authentication.Basic.Events;
using Knapcode.CheckRepublic.Logic.Business;
using Knapcode.CheckRepublic.Logic.Business.Mappers;
using Knapcode.CheckRepublic.Logic.Business.Models;
using Knapcode.CheckRepublic.Logic.Entities;
using Knapcode.CheckRepublic.Logic.Runner;
using Knapcode.CheckRepublic.Logic.Runner.Checks;
using Knapcode.CheckRepublic.Logic.Runner.Utilities;
using Knapcode.CheckRepublic.Website.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ISystemClock = Knapcode.CheckRepublic.Logic.Utilities.ISystemClock;
using SystemClock = Knapcode.CheckRepublic.Logic.Utilities.SystemClock;
using Microsoft.AspNetCore.Authentication;

namespace Knapcode.CheckRepublic.Website
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            HostingEnvironment = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IHostingEnvironment HostingEnvironment { get; }

        public IConfigurationRoot Configuration { get; }

        public IServiceProvider ServiceProvider { get; private set; }
        
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services
                .AddDbContext<CheckContext>(options =>
                {
                    var path = Path.Combine(HostingEnvironment.ContentRootPath, "CheckRepublic.sqlite3");
                    options.UseSqlite($"Filename={path}");
                });

            services.AddTransient<ICheckBatchRunner, CheckBatchRunner>();
            services.AddTransient<ICheckBatchService, CheckBatchService>();
            services.AddTransient<ICheckFactory, ServiceProviderCheckFactory>();
            services.AddTransient<ICheckNotificationService, CheckNotificationService>();
            services.AddTransient<ICheckPersister, CheckPersister>();
            services.AddTransient<ICheckResultService, CheckResultService>();
            services.AddTransient<ICheckRunner, CheckRunner>();
            services.AddTransient<ICheckRunnerService, CheckRunnerService>();
            services.AddTransient<ICheckService, CheckService>();
            services.AddTransient<IEntityMapper, EntityMapper>();
            services.AddTransient<IHealthService, HealthService>();
            services.AddTransient<IHeartbeatService, HeartbeatService>();
            services.AddTransient<IHeartGroupService, HeartGroupService>();
            services.AddTransient<IMigrationService, MigrationService>();
            services.AddTransient<INotificationRunnerService, NotificationRunnerService>();
            services.AddTransient<IRunnerMapper, RunnerMapper>();
            services.AddTransient<ISystemClock, SystemClock>();
            
            services.AddTransient<IHeartbeatCheck, HeartbeatCheck>();
            services.AddTransient<IHttpJTokenCheck, HttpJTokenCheck>();
            services.AddTransient<IHttpResponseStreamCheck, HttpResponseStreamCheck>();
            services.AddTransient<IHttpSubstringCheck, HttpSubstringCheck>();

            services.AddTransient<ICheck, BlogUpCheck>();
            services.AddTransient<ICheck, ConnectorRideLatestJsonCheck>();
            services.AddTransient<ICheck, ConnectorRideScrapeStatusCheck>();
            services.AddTransient<ICheck, NuGetToolsUpCheck>();
            services.AddTransient<ICheck, PoGoNotificationsHeartbeatCheck>();
            services.AddTransient<ICheck, UserAgentReportDatabaseStatusCheck>();
            services.AddTransient<ICheck, UserAgentReportUpCheck>();
            services.AddTransient<ICheck, WintalloUpCheck>();

            services.AddOptions();
            services.Configure<WebsiteOptions>(Configuration);
            services.Configure<GroupMeOptions>(Configuration.GetSection("GroupMe"));

            if (string.IsNullOrEmpty(Configuration.GetValue<string>("GroupMe:AccessToken")) ||
                string.IsNullOrEmpty(Configuration.GetValue<string>("GroupMe:BotId")))
            {
                services.AddTransient<INotificationSender, LoggerNotificationSender>();
            }
            else
            {
                services.AddTransient<INotificationSender, GroupMeNotificationSender>();
            }

            services.AddSingleton<IAuthorizationHandler, AnonymousHandler>();

            services
                .AddAuthorization(options =>
                {
                    var hasRead = !string.IsNullOrEmpty(Configuration.GetValue<string>("ReadPassword"));
                    var hasWrite = !string.IsNullOrEmpty(Configuration.GetValue<string>("WritePassword"));

                    var readRequirements = new List<string>();
                    var writeRequirements = new List<string>();

                    if (hasRead)
                    {
                        readRequirements.Add(AuthorizationConstants.ReaderRole);
                    }
                    
                    if (hasWrite)
                    {
                        readRequirements.Add(AuthorizationConstants.WriterRole);
                        writeRequirements.Add(AuthorizationConstants.WriterRole);
                    }

                    if (hasRead)
                    {
                        options.AddPolicy(
                           AuthorizationConstants.ReadPolicy,
                           policy => policy
                               .RequireAuthenticatedUser()
                               .AddRequirements(new RolesAuthorizationRequirement(readRequirements)));
                    }
                    else
                    {
                        options.AddPolicy(
                           AuthorizationConstants.ReadPolicy,
                           policy => policy
                                .AddRequirements(new AnonymousRequirement()));
                    }

                    if (hasWrite)
                    {
                        options.AddPolicy(
                           AuthorizationConstants.WritePolicy,
                           policy => policy
                               .RequireAuthenticatedUser()
                               .AddRequirements(new RolesAuthorizationRequirement(writeRequirements)));
                    }
                    else
                    {
                        options.AddPolicy(
                           AuthorizationConstants.WritePolicy,
                           policy => policy
                                .AddRequirements(new AnonymousRequirement()));
                    }
                });

            services
                .AddAuthentication(BasicAuthenticationDefaults.AuthenticationScheme)
                .AddBasicAuthentication(options =>
                {
                    options.Realm = HostingEnvironment.ApplicationName;
                    options.Events = new BasicAuthenticationEvents
                    {
                        OnValidatePrincipal = context =>
                        {
                            var websiteOptions = ServiceProvider.GetService<IOptions<WebsiteOptions>>();

                            var claims = new List<Claim>();

                            var readPassword = websiteOptions.Value.ReadPassword;
                            if (string.IsNullOrWhiteSpace(readPassword) || context.Password == readPassword)
                            {
                                claims.Add(new Claim(ClaimTypes.Role, AuthorizationConstants.ReaderRole));
                            }

                            var writePassword = websiteOptions.Value.WritePassword;
                            if (string.IsNullOrWhiteSpace(writePassword) || context.Password == writePassword)
                            {
                                claims.Add(new Claim(ClaimTypes.Role, AuthorizationConstants.WriterRole));
                            }

                            var ticket = new AuthenticationTicket(
                                new ClaimsPrincipal(new ClaimsIdentity(claims, BasicAuthenticationDefaults.AuthenticationScheme)),
                                new AuthenticationProperties(),
                                BasicAuthenticationDefaults.AuthenticationScheme);

                            return Task.FromResult(AuthenticateResult.Success(ticket));
                        }
                    };
                });

            services
                .AddMvc(options => { })
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });

            ServiceProvider = services.BuildServiceProvider();

            return ServiceProvider;
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Status}/{action=LatestCheckBatch}/{id?}");
            });

            // Perform any database migration, if needed. This code also initalizes the database.
            var migrationService = ServiceProvider.GetService<IMigrationService>();
            migrationService.Migrate();
        }
    }
}
