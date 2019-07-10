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

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, DataContext dataContext)
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
