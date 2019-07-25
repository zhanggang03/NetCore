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
                    //��������cookie���ѷ���idr4.SessionId=8660972474e55224ff37f7421c79a530 ʵ����cookie��¼������session������
                    CheckSessionCookieName = "idr4.SessionId", // CookieAuthenticationDefaults.AuthenticationScheme,//���ڼ��Ự�˵��cookie������
                    CookieLifetime = new TimeSpan(1, 0, 0),//�����֤Cookie�����ڣ�����ʹ��IdentityServer�ṩ��Cookie�������ʱ��Ч��
                    CookieSlidingExpiration = true,//ָ��cookie�Ƿ�Ӧ�û���������ʹ��IdentityServer�ṩ��cookie�������ʱ��Ч��
                    RequireAuthenticatedUserForSignOutMessage = true //ָʾ�Ƿ������û����������֤���ܽ��ܲ����Խ����Ự�˵㡣Ĭ��Ϊfalse
                };
                //��¼� ���������Ƿ�Ӧ�ý���Щ�¼��ύ��ע����¼�������
                idroptions.Events = new IdentityServer4.Configuration.EventsOptions
                {
                    RaiseErrorEvents = true,
                    RaiseFailureEvents = true,
                    RaiseSuccessEvents = true,
                    RaiseInformationEvents = true
                };
                idroptions.UserInteraction = new IdentityServer4.Configuration.UserInteractionOptions
                {

                    LoginUrl = "/Account/Login",//���ر�����¼��ַ  
                    LogoutUrl = "/Account/Logout",//���ر����˳���ַ 
                    ConsentUrl = "/Consent/Index",//���ر���������Ȩͬ��ҳ���ַ
                    ErrorUrl = "/Error/Index", //���ر�������ҳ���ַ
                    LoginReturnUrlParameter = "returnUrl",//���ر������ô��ݸ���¼ҳ��ķ���URL���������ơ�Ĭ��ΪreturnUrl 
                    LogoutIdParameter = "logoutId", //���ر������ô��ݸ�ע��ҳ���ע����ϢID���������ơ�ȱʡΪlogoutId 
                    ConsentReturnUrlParameter = "returnUrl", //���ر������ô��ݸ�ͬ��ҳ��ķ���URL���������ơ�Ĭ��ΪreturnUrl
                    ErrorIdParameter = "errorId", //���ر������ô��ݸ�����ҳ��Ĵ�����ϢID���������ơ�ȱʡΪerrorId
                    CustomRedirectReturnUrlParameter = "returnUrl", //���ر������ô���Ȩ�˵㴫�ݸ��Զ����ض���ķ���URL���������ơ�Ĭ��ΪreturnUrl
                    CookieMessageThreshold = 5 //���ر��������������Cookie�Ĵ�С�����ƣ�����Cookies���������ƣ���Ч�ı�֤��������򿪶��ѡ���һ��������Cookies���ƾͻ������ǰ��Cookiesֵ
                };
                //�����������  �������������Ч�� ����ÿ�δ����ݿ��ѯ
                //idroptions.Caching = new IdentityServer4.Configuration.CachingOptions
                //{
                //    ClientStoreExpiration = new TimeSpan(1, 0, 0),//����Client�ͻ��˴洢���صĿͻ������õ����ݻ������Чʱ�� 
                //    ResourceStoreExpiration = new TimeSpan(1, 0, 0),// ���ô���Դ�洢���ص���ݺ�API��Դ���õĻ������ʱ��
                //    CorsExpiration = new TimeSpan(1, 0, 0)  //���ô���Դ�洢�Ŀ����������ݵĻ���ʱ��
                //};
            })
                .AddDeveloperSigningCredential()
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClient())
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddResourceOwnerValidator<CustomResourceOwnerPasswordValidator>();   //��ʹ��Password��֤�У��������TestUser�û������붨�����CustomResourceOwnerPasswordValidator
                //.AddTestUsers(Config.GetUser());    //ע�����ڸ���������֤�У�����ʹ�����ݿ�־û�������֤���������ʹ��TestUser������֤
            //�����ݿ���������
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
            //    options.EnableTokenCleanup = false;//�Ƿ�����ݿ�����������ݣ�Ĭ��Ϊfalse
            //    options.TokenCleanupInterval = 300;//���ƹ���ʱ�䣬Ĭ��Ϊ3600�룬һ��Сʱ
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

            //��IddiTyServer��ӵ��ܵ��С�
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
