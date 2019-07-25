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
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Cache.CacheManager;
using Ocelot.Provider.Polly;
using IdentityServer4.AccessTokenValidation;

namespace OcelotGetway
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

            #region IdentityServerAuthenticationOptions => need to refactor
            Action<IdentityServerAuthenticationOptions> isaOptClient = option =>
            {
                option.Authority = "https://localhost:44379";
                option.ApiName = "WebApiA";
                option.RequireHttpsMetadata = true;
                option.SupportedTokens = SupportedTokens.Both;
                //option.ApiSecret = Configuration["IdentityService:ApiSecrets:clientservice"];
            };

            Action<IdentityServerAuthenticationOptions> isaOptProduct = option =>
            {
                option.Authority = "https://localhost:44379";
                option.ApiName = "WebApiB";
                option.RequireHttpsMetadata = true;
                option.SupportedTokens = SupportedTokens.Both;
                //option.ApiSecret = Configuration["IdentityService:ApiSecrets:productservice"];
            };
            #endregion

            services.AddAuthentication()
                .AddIdentityServerAuthentication("WebApiAKey", isaOptClient)
                .AddIdentityServerAuthentication("WebApiBKey", isaOptProduct);


            services.AddOcelot(new ConfigurationBuilder().AddJsonFile("configuration.json").Build())
                .AddConsul().AddPolly().AddCacheManager(x => x.WithDictionaryHandle());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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

            app.UseOcelot().Wait();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
