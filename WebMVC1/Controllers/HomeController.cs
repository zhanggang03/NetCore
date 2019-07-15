using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebMVC1.Models;

namespace WebMVC1.Controllers
{
    //[Authorize]
    [Authorize(Roles = "admin")]
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            //获取用户信息
            var claimIdentity = (ClaimsIdentity)HttpContext.User.Identity;
            var claimsPrincipal = claimIdentity.Claims as List<Claim>;

            //获取用户token
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var client = new HttpClient();
            client.SetBearerToken(accessToken);
            //访问之前定义好的Api项目的方法
            var content = await client.GetStringAsync("https://localhost:44312/api/Identity");

            return View();
        }

        public async void Logout()    //返回类型必须是Void值
        {
            //await HttpContext.SignOutAsync("Cookies");   //删除本地Cookies
            await HttpContext.SignOutAsync("oidc");      //请求删除idr4服务器的Cookies
        }


        [AllowAnonymous]
        public async Task<IActionResult> FrontChannelLogout(string sid)
        {
            //if (User.Identity.IsAuthenticated)
            //{
            //    var currentSid = User.FindFirst("sid")?.Value ?? "";
            //    if (string.Equals(currentSid, sid, StringComparison.Ordinal))
            //{
            //    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //}
            // }
            //return NoContent();
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");
            return NoContent();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
