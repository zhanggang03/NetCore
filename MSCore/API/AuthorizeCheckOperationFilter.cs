using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSCore.API
{
    /// <summary>
    /// IdentityServer4授权响应操作的过滤器
    /// </summary>
    public class AuthorizeCheckOperationFilter : IOperationFilter   
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            //获取是否添加登录特性
            var authAttributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
             .Union(context.MethodInfo.GetCustomAttributes(true))
             .OfType<AuthorizeAttribute>().Any();

            if (authAttributes)
            {
                operation.Responses.Add("401", new Response { Description = "暂无访问权限" });
                operation.Responses.Add("403", new Response { Description = "禁止访问" });
                //给api添加锁的标注
                operation.Security = new List<IDictionary<string, IEnumerable<string>>>
                {
                    new Dictionary<string, IEnumerable<string>> {{"oauth2", new[] { "core_swagger" } }}
                };
            }
        }
    }
}
