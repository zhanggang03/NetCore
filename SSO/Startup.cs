using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SSO.Validator;

namespace SSO
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
            const string connectionString = @"Data Source=192.168.5.148\sql;Initial Catalog=IdentityServer4;User ID=sa;Password=123456;";
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddIdentityServer(idroptions =>
            {
                idroptions.Authentication = new IdentityServer4.Configuration.AuthenticationOptions
                {
                    //监控浏览器cookie不难发现idr4.SessionId=8660972474e55224ff37f7421c79a530 实际是cookie记录服务器session的名称
                    CheckSessionCookieName = "idr4.SessionId", // CookieAuthenticationDefaults.AuthenticationScheme,//用于检查会话端点的cookie的名称
                    CookieLifetime = new TimeSpan(1, 0, 0),//身份验证Cookie生存期（仅在使用IdentityServer提供的Cookie处理程序时有效）
                    CookieSlidingExpiration = true,//指定cookie是否应该滑动（仅在使用IdentityServer提供的cookie处理程序时有效）
                    RequireAuthenticatedUserForSignOutMessage = true //指示是否必须对用户进行身份验证才能接受参数以结束会话端点。默认为false
                };
                //活动事件 允许配置是否应该将哪些事件提交给注册的事件接收器
                idroptions.Events = new IdentityServer4.Configuration.EventsOptions
                {
                    RaiseErrorEvents = true,
                    RaiseFailureEvents = true,
                    RaiseSuccessEvents = true,
                    RaiseInformationEvents = true
                };
                idroptions.UserInteraction = new IdentityServer4.Configuration.UserInteractionOptions
                {

                    LoginUrl = "/Account/Login",//【必备】登录地址  
                    LogoutUrl = "/Account/Logout",//【必备】退出地址 
                    ConsentUrl = "/Consent/Index",//【必备】允许授权同意页面地址
                    ErrorUrl = "/Error/Index", //【必备】错误页面地址
                    LoginReturnUrlParameter = "returnUrl",//【必备】设置传递给登录页面的返回URL参数的名称。默认为returnUrl 
                    LogoutIdParameter = "logoutId", //【必备】设置传递给注销页面的注销消息ID参数的名称。缺省为logoutId 
                    ConsentReturnUrlParameter = "returnUrl", //【必备】设置传递给同意页面的返回URL参数的名称。默认为returnUrl
                    ErrorIdParameter = "errorId", //【必备】设置传递给错误页面的错误消息ID参数的名称。缺省为errorId
                    CustomRedirectReturnUrlParameter = "returnUrl", //【必备】设置从授权端点传递给自定义重定向的返回URL参数的名称。默认为returnUrl
                    CookieMessageThreshold = 5 //【必备】由于浏览器对Cookie的大小有限制，设置Cookies数量的限制，有效的保证了浏览器打开多个选项卡，一旦超出了Cookies限制就会清除以前的Cookies值
                };
                //缓存参数处理  缓存起来提高了效率 不用每次从数据库查询
                //idroptions.Caching = new IdentityServer4.Configuration.CachingOptions
                //{
                //    ClientStoreExpiration = new TimeSpan(1, 0, 0),//设置Client客户端存储加载的客户端配置的数据缓存的有效时间 
                //    ResourceStoreExpiration = new TimeSpan(1, 0, 0),// 设置从资源存储加载的身份和API资源配置的缓存持续时间
                //    CorsExpiration = new TimeSpan(1, 0, 0)  //设置从资源存储的跨域请求数据的缓存时间
                //};
            })
                .AddDeveloperSigningCredential()
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClient())
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddResourceOwnerValidator<CustomResourceOwnerPasswordValidator>();   //在使用Password验证中，如果不用TestUser用户，必须定义此项CustomResourceOwnerPasswordValidator
                //.AddTestUsers(Config.GetUser());    //注意此项，在各种类型验证中，可以使用数据库持久化数据验证，否则必须使用TestUser数据验证
            //从数据库增加数据
            //.AddConfigurationStore(options =>
            //{
            //     options.ConfigureDbContext = builder =>
            //        builder.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
            //})
            //// this adds the operational data from DB (codes, tokens, consents)
            //.AddOperationalStore(options =>
            //{
            //    options.ConfigureDbContext = builder =>
            //        builder.UseSqlServer(connectionString,
            //            sql => sql.MigrationsAssembly(migrationsAssembly));

            //    // this enables automatic token cleanup. this is optional.
            //    options.EnableTokenCleanup = false;//是否从数据库清楚令牌数据，默认为false
            //    options.TokenCleanupInterval = 300;//令牌过期时间，默认为3600秒，一个小时
            //});

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //InitializeDatabase(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            //将IddiTyServer添加到管道中。
            app.UseIdentityServer();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Account}/{action=Login}/{id?}");
            });
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                if (!context.Clients.Any())
                {
                    foreach (var client in Config.GetClient())
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
