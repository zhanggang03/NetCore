using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace WebMVC1
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";   //使用Cookies认证
                options.DefaultChallengeScheme = "oidc";  //使用oidc
            })
               .AddCookie("Cookies",
               options =>
               {

                   options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                   options.Cookie.Name = "mvc1";

               })   //配置Cookies认证
               .AddOpenIdConnect("oidc", options =>    //配置oidc
               {
                   options.SignInScheme = "Cookies";
                   options.Authority = "https://localhost:44379";
                   options.RequireHttpsMetadata = true;
                   options.ClientId = "mvc1";
                   options.ResponseType = "id_token code";
                   options.GetClaimsFromUserInfoEndpoint = true;
                   //options.SignedOutCallbackPath = "/signout-callback-oidc";  //调用idrs4执行事件
                   options.ClientSecret = "secret";
                   options.SaveTokens = true;
                   options.Scope.Clear();
                   options.Scope.Add("openid");
                   options.Scope.Add("profile");
                   options.Scope.Add("MsCoreApi");
                   options.Scope.Add("roles");   //定义使用范围

                   options.Events = new OpenIdConnectEvents
                   {
                       OnRedirectToIdentityProvider = OnRedirectToIdentityProvider,
                       OnSignedOutCallbackRedirect = OnSignedOutCallbackRedirect,
                       OnRemoteSignOut = OnRemoteSignOut,
                       OnRemoteFailure = OnRemoteFailure,
                       OnAuthenticationFailed = OnAuthenticationFailed,
                       OnRedirectToIdentityProviderForSignOut = OnRedirectToIdentityProviderForSignOut,
                       OnAuthorizationCodeReceived = OnAuthorizationCodeReceived,
                       OnMessageReceived = OnMessageReceived,
                       OnTicketReceived = OnTicketReceived,
                       OnTokenResponseReceived = OnTokenResponseReceived,
                       OnTokenValidated = OnTokenValidated,
                       OnUserInformationReceived = OnUserInformationReceived
                   };

                   options.ClaimActions.MapUniqueJsonKey("role", "role");
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       NameClaimType = JwtClaimTypes.GivenName,
                       RoleClaimType = JwtClaimTypes.Role
                   };
               });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        #region  Events事件
        private static Task OnRedirectToIdentityProvider(RedirectContext context)
        {
            if (context.HttpContext.Items.ContainsKey("idp"))
            {
                var idp = context.HttpContext.Items["idp"];
                context.ProtocolMessage.AcrValues = "idp:" + idp;
            }

            return Task.FromResult(0);
        }

        private static Task OnSignedOutCallbackRedirect(RemoteSignOutContext context)
        {
            return Task.FromResult(0);
        }

        private static Task OnRemoteSignOut(RemoteSignOutContext context)
        {
            return Task.FromResult(0);
        }

        private static Task OnRemoteFailure(RemoteFailureContext context)
        {
            return Task.FromResult(0);
        }

        private static Task OnAuthenticationFailed(AuthenticationFailedContext context)
        {
            return Task.FromResult(0);
        }

        private static Task OnRedirectToIdentityProviderForSignOut(RedirectContext context)
        {
            return Task.FromResult(0);
        }
        private static Task OnAuthorizationCodeReceived(AuthorizationCodeReceivedContext context)
        {
            return Task.FromResult(0);
        }
        private static Task OnMessageReceived(MessageReceivedContext context)
        {
            return Task.FromResult(0);
        }

        private static Task OnTicketReceived(TicketReceivedContext context)
        {
            return Task.FromResult(0);
        }

        private static Task OnTokenResponseReceived(TokenResponseReceivedContext context)
        {
            return Task.FromResult(0);
        }

        private static Task OnTokenValidated(TokenValidatedContext context)
        {
            return Task.FromResult(0);
        }

        private static Task OnUserInformationReceived(UserInformationReceivedContext context)
        {
            return Task.FromResult(0);
        }
        #endregion

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.Run(async context =>
            {
                int i = 1;
            });
        }
    }
}
