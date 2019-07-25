using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace SSO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration configuration;
        public LoginController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        [HttpPost]
        public async Task<ActionResult> RequestToken(LoginRequestParam model)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict["client_id"] = model.ClientId;
            dict["client_secret"] = "secret";
            dict["grant_type"] = "password";
            dict["username"] = model.UserName;
            dict["password"] = model.Password;

            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:44379");
            var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "clientApi",
                ClientSecret = "secret",
                UserName = model.UserName,
                Password = model.Password,
            });
            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return StatusCode(Convert.ToInt32(tokenResponse.HttpStatusCode));
            }
            Console.WriteLine(tokenResponse.Json);
            return Content(tokenResponse.AccessToken, "application/json");
        }
    }

    public class LoginRequestParam
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string GrantType { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}