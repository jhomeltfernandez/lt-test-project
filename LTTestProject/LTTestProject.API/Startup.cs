using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AutoMapper;
using LTTestProject.Models;
using LTTestProject.DataAccess;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Swagger;
using LTTestProject.Business.Handlers.Queries;
using MediatR;
using LTTestProject.Business.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using LTTestProject.DataAccess.Seeder;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using System.Reflection;
using IdentityServer4.Validation;
using IdentityServer4.Services;

namespace LTTestProject.API
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSingleton(sp => Mappings.CreateConfiguration().CreateMapper());

            services.AddMediatR(GetType().Assembly, typeof(GetCategoryHandler).Assembly);
            services.AddScoped<IFileService, FileService>();

            string connectionString = Configuration.GetSection("DataConnection").GetValue<string>("SqlConnection");
            services.AddDbContext<LTTestProjectDbContext>(options => options.UseSqlServer(connectionString, b=>b.MigrationsAssembly("LTTestProject.DataAccess")));
            services.AddDbContext<LTTestProjectIdentityDbContext>(options => options.UseSqlServer(connectionString, b => b.MigrationsAssembly("LTTestProject.DataAccess")));

            services.AddIdentity<IdentityUser, IdentityRole>(config =>
            {
                config.SignIn.RequireConfirmedEmail = true;
                config.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<LTTestProjectIdentityDbContext>().AddDefaultTokenProviders();

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Info { Title = "LTTestProject API", Version = "v1" });
                c.AddSecurityDefinition("Bearer",
                    new ApiKeyScheme
                    {
                        In = "header",
                        Description = "Please enter into field the word 'Bearer' following by space and JWT",
                        Name = "Authorization",
                        Type = "apiKey"
                    });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> {
                    { "Bearer", Enumerable.Empty<string>() },
                });

            });

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                //.AddInMemoryApiResources(Config.GetApiResources())
                //.AddInMemoryClients(Config.GetClients())
                //.AddProfileService<ProfileService>()
                //.AddTestUsers(Config.GetTestUsers());
                // this adds the config data from DB (clients, resources)
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = b =>
                        b.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly(typeof(LTTestProjectDbContext).GetTypeInfo().Assembly.GetName().Name));
                })
                // this adds the operational data from DB (codes, tokens, consents)
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b =>
                        b.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly(typeof(LTTestProjectDbContext).GetTypeInfo().Assembly.GetName().Name));

                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;
                });

            services.AddAuthentication(o => {
                o.DefaultScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                o.DefaultAuthenticateScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
            }).AddIdentityServerAuthentication(options =>
            {
                options.Authority = Configuration.GetSection("Security").GetValue<string>("Authority"); 
                options.ApiName = Configuration.GetSection("Security").GetValue<string>("ApiName");
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("myPolicy", builder =>
                {
                    builder.RequireScope(Configuration.GetSection("Security").GetValue<string>("ApiName"));
                });
            });

            services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
            services.AddTransient<IProfileService, ProfileService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, UserManager<IdentityUser> userManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //Initialize Identity Server 4 Entities
            InitializeID4Database(app);

            ApplicationUserSeeder.SeedUsers(userManager);

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "LTTestProject API");
                c.RoutePrefix = string.Empty;  // Set Swagger UI at apps root
            });

            app.UseIdentityServer();
            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseMvc();
        }

        private void InitializeID4Database(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();

                if (!context.Clients.Any())
                {
                    foreach (var client in Config.GetClients())
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in Config.GetIdentityResources())
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in Config.GetApiResources())
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}
