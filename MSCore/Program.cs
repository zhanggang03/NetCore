using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Unity;
using Unity.Microsoft.DependencyInjection;

namespace MSCore
{
    public class Program
    {
        private static IUnityContainer _container;
        public static void Main(string[] args)
        {
            _container = new UnityContainer();
            ConfigureContainer(_container);
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUnityServiceProvider(_container)
                .UseStartup<Startup>();

        public static void ConfigureContainer(IUnityContainer _container)
        {
            //注册对象关系

            //注册控制器
            _container.RegisterType<Controllers.UserController>();

            //注册接口
            _container.RegisterType<DAO.IDeptDao, DAO.DeptDao>();
            _container.RegisterType<DAO.IUserDao, DAO.UserDao>();

            _container.RegisterType<BLL.IDeptBll, BLL.DeptBll>();
            _container.RegisterType<BLL.IUserBll, BLL.UserBll>();
        }
    }
}
