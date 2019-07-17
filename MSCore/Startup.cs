using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Model;
using ViewModelMap;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Swashbuckle.AspNetCore.Swagger;
using System.Reflection;
using System.IO;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using MSCore.API;

namespace MSCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            RegisterMappings.Initialize();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region EF 配置新增
            string Conndb = Configuration.GetConnectionString("SqlServer");
            //配置EF的服务注册
            services.AddDbContext<DataContext>(options =>
            {
                options.UseLazyLoadingProxies().UseSqlServer(Conndb, //读取配置文件中的链接字符串
                    b => b.UseRowNumberForPaging());  //配置分页 使用旧方式
            });
            #endregion

            //注册IdentityServer 要用IdentityServer4.AccessTokenValidation包
            //services.AddAuthentication(config => {
            //    config.DefaultScheme = "Bearer"; //这个是access_token的类型，获取access_token的时候返回参数中的token_type一致
            //}).AddIdentityServerAuthentication(option => {
            //    option.ApiName = "MsCoreApi"; //资源名称，认证服务注册的资源列表名称一致，
            //    option.Authority = "https://localhost:44379"; //认证服务的url
            //    option.RequireHttpsMetadata = true; //是否启用https
            //});

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://localhost:44379";
                    options.RequireHttpsMetadata = true;
                    options.Audience = "MsCoreApi";
                });

            services.AddApiVersioning(option => {
                option.AssumeDefaultVersionWhenUnspecified = true;
                option.ReportApiVersions = false;
            }).AddVersionedApiExplorer(option => {
                option.GroupNameFormat = "'v'VVV";
                option.AssumeDefaultVersionWhenUnspecified = true;
            });
            var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
            services.AddSwaggerGen(gen =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {

                    gen.SwaggerDoc(description.GroupName, new Info
                    {
                        Version = description.ApiVersion.ToString(),
                        Title = $"微服务接口 v{description.ApiVersion}",
                        Description = "切换版本请点右上角版本切换",
                        Contact = new Contact
                        {
                            Name = "ZG",
                            Email = string.Empty,
                        },
                    });
                }
                //var xmlFile = $"ServicesApi.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //gen.IncludeXmlComments(xmlPath);

                //var xmlModelFile = "ServicesDomain.xml";
                //var xmlMolelPath = Path.Combine(AppContext.BaseDirectory, xmlModelFile);
                //gen.IncludeXmlComments(xmlMolelPath);

                //接入identityserver
                gen.AddSecurityDefinition("oauth2", new OAuth2Scheme
                {
                    Flow = "implicit", //只需要通过浏览器获取令牌，适用于Swagger
                    AuthorizationUrl = "https://localhost:44379/connect/authorize",//获取登录授权接口  
                    Scopes = new Dictionary<string, string>
                    {
                        { "MsCoreApi", "同意 MsCoreApi 的访问权限" }//指定客户端请求的api作用域。 如果为空，则客户端无法访问
                    }
                });
                gen.OperationFilter<AuthorizeCheckOperationFilter>(); // 添加IdentityServer4认证过滤
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, DataContext dataContext
            , IApiVersionDescriptionProvider provider)
        {
            //new DbInitializer().InitializeAsync(dataContext);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            //配置Authentication中间件
            app.UseAuthentication();

            //配置Swagger中间件
            app.UseSwagger();
            // 使中间件能够服务于轻量级用户界面（HTML、JS、CSS等），并指定SWAGJER JSON端点
            app.UseSwaggerUI(ui =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    ui.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"{description.GroupName} CoreAPI");
                }
                //要在应用程序的根处提供Swagger UI ，请将该RoutePrefix属性设置为空字符串
                ui.RoutePrefix = string.Empty;
                ui.OAuthClientId("core_swagger");
                ui.OAuthAppName("Core API 服务");
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }

    public class DbInitializer
    {
        public async Task InitializeAsync(DataContext context)
        {
            //var migrations = await context.Database.GetPendingMigrationsAsync();//获取未应用的Migrations，不必要，MigrateAsync方法会自动处理
            await context.Database.MigrateAsync();//根据Migrations修改/创建数据库
        }
    }
}
