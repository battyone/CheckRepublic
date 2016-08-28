using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using idunno.Authentication;
using Knapcode.CheckRepublic.Logic.Business;
using Knapcode.CheckRepublic.Logic.Entities;
using Knapcode.CheckRepublic.Logic.Runner;
using Knapcode.CheckRepublic.Logic.Runner.Checks;
using Knapcode.CheckRepublic.Logic.Runner.Utilities;
using Knapcode.CheckRepublic.Website.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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

            services.AddTransient<ICheckService, CheckService>();
            services.AddTransient<ICheckBatchService, CheckBatchService>();

            services.AddTransient<ICheckRunner, CheckRunner>();
            services.AddTransient<ICheckBatchRunner, CheckBatchRunner>();
            services.AddTransient<ICheckPersister, CheckPersister>();
            services.AddTransient<IHttpCheck, HttpCheck>();
            services.AddTransient<ICheckFactory, ServiceProviderCheckFactory>();
            services.AddTransient<ICheckRunnerService, CheckRunnerService>();
            services.AddTransient<IHeartGroupService, HeartGroupService>();
            services.AddTransient<IHeartbeatService, HeartbeatService>();

            services.AddTransient<ICheck, BlogUpCheck>();
            services.AddTransient<ICheck, ConcertoUpCheck>();
            services.AddTransient<ICheck, NuGetToolsUpCheck>();
            services.AddTransient<ICheck, UserAgentReportUpCheck>();
            services.AddTransient<ICheck, WintalloUpCheck>();

            services.AddOptions();
            services.Configure<WebsiteOptions>(Configuration);

            services
                .AddAuthorization(options =>
                {
                    options.AddPolicy(
                        AuthorizationConstants.ReadPolicy,
                        policy => policy.AddRequirements(
                            new RolesAuthorizationRequirement(new[]
                            {
                                AuthorizationConstants.ReaderRole,
                                AuthorizationConstants.WriterRole
                            })));

                    options.AddPolicy(
                        AuthorizationConstants.WritePolicy,
                        policy => policy.AddRequirements(
                            new RolesAuthorizationRequirement(new[]
                            {
                                AuthorizationConstants.WriterRole
                            })));
                });

            services
                .AddMvc(options =>
                {
                    var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();

                    options.Filters.Add(new AuthorizeFilter(policy));
                })
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

            app.UseBasicAuthentication(new BasicAuthenticationOptions
            {
                Realm = env.ApplicationName,
                Events = new BasicAuthenticationEvents
                {
                    OnValidateCredentials = context =>
                    {
                        var options = ServiceProvider.GetService<IOptions<WebsiteOptions>>();

                        var claims = new List<Claim>();

                        var readPassword = options.Value.ReadPassword;
                        if (string.IsNullOrWhiteSpace(readPassword) || context.Password == readPassword)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, AuthorizationConstants.ReaderRole));
                        }

                        var writePassword = options.Value.WritePassword;
                        if (string.IsNullOrWhiteSpace(writePassword) || context.Password == writePassword)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, AuthorizationConstants.WriterRole));
                        }

                        context.Ticket = new AuthenticationTicket(
                            new ClaimsPrincipal(new ClaimsIdentity(claims, context.Options.AuthenticationScheme)),
                            new AuthenticationProperties(),
                            context.Options.AuthenticationScheme);

                        return Task.FromResult(0);
                    }
                }
            });

            app.UseMvc();

            // Perform any database migration, if needed. This code also initalizes the database.
            using (var checkContext = ServiceProvider.GetService<CheckContext>())
            {
                checkContext.Database.Migrate();
            }
        }
    }
}
