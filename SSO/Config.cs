using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Test;
using IdentityServer4.Models;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4;

namespace SSO
{
    public class Config
    {
        public static List<TestUser> GetUser()
        {
            return new List<TestUser>
            {
                    new TestUser
                    {
                        SubjectId = "1",
                        Username = "ZG",
                        Password = "password",
                        Claims = new List<Claim>(){new Claim(JwtClaimTypes.Role,"superadmin") }
                    },
                    new TestUser
                    {
                        SubjectId = "2",
                        Username = "WG",
                        Password = "password",
                        Claims = new List<Claim>
                    {
                        new Claim("name", "Bob"),
                        new Claim("website", "https://bob.com")
                    },
                   }
            };
        }

        public static IEnumerable<Client> GetClient()
        {

            /*
             * 作为OAuth2.0客户端，需要注意一下几点：
四种授权模式：

1. 授权码（认证码）模式 （Authorization code) response_type=code

2. 简化（隐形）模式 (Impilict） response_type=token

3. 用户名密码模式 (Resource Owner Password Credential) grant_type=password

4. 客户端模式 (Client Credential) grant_type=client_credential
             * */

            return new List<Client>
            {
               new Client
                {
                    //客户端id自定义
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials, ////设置模式，客户端模式
                    // 加密验证
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())
                    },
                    // client可以访问的范围，在GetScopes方法定义的名字。
                    AllowedScopes = new List<string>
                    {
                        "MsCoreApi"
                    }
                },
               new Client
                {
                    //客户端id自定义
                    ClientId = "clientApi",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    // 加密验证
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())
                    },
                    // client可以访问的范围，在GetScopes方法定义的名字。
                    AllowedScopes = new List<string>
                    {
                        "MsCoreApi","WebApiA","WebApiB"
                    }
                },
               new Client
                {
                    ClientId = "core_swagger",//客服端名称
                    AllowedGrantTypes = GrantTypes.Implicit,//指定允许的授权类型（AuthorizationCode，Implicit，Hybrid，ResourceOwner，ClientCredentials的合法组合）。
                    AllowAccessTokensViaBrowser = true,//是否通过浏览器为此客户端传输访问令牌
                    RedirectUris =
                    {
                        "https://localhost:44312/oauth2-redirect.html"
                    },
                    AllowedScopes = {
                       "MsCoreApi"
                   }//指定客户端请求的api作用域。 如果为空，则客户端无法访问
                },
                new Client
                {
                    ClientId = "mvc",
                    AllowedGrantTypes = GrantTypes.Implicit,  //模式：隐式模式
                    ClientSecrets =   //私钥
                    {
                        new Secret("secret".Sha256())
                    },
                    RedirectUris = { "https://localhost:44346/signin-oidc" },   //跳转登录到的客户端的地址
                    PostLogoutRedirectUris = { "https://localhost:44346/signout-callback-oidc" },  //跳转登出到的客户端的地址
                    AllowedScopes =  //运行访问的资源
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                    },
                    FrontChannelLogoutUri = "https://localhost:44346/signout-oidc",  //调用Client执行事件
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowOfflineAccess = true,
                    RequireConsent = false,      //是否需要用户点击确认进行跳转
                    AllowAccessTokensViaBrowser = true // can return access_token to this client
                },
                new Client
                {
                    ClientId = "mvc1",
                    AllowedGrantTypes = GrantTypes.Hybrid,  //模式：隐式模式
                    ClientSecrets =   //私钥
                    {
                        new Secret("secret".Sha256())
                    },
                    RedirectUris = { "https://localhost:44365/signin-oidc" },   //跳转登录到的客户端的地址
                    PostLogoutRedirectUris = { "https://localhost:44365/signout-callback-oidc" },  //跳转登出到的客户端的地址
                    AllowedScopes =  //运行访问的资源
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "MsCoreApi",
                        "roles",    //User 的Claims中含有Role字段
                    },
                    FrontChannelLogoutUri = "https://localhost:44365/signout-oidc",  //调用Client执行事件
                    //BackChannelLogoutUri = "https://localhost:44365/signout-oidc",
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowOfflineAccess = true,
                    RequireConsent = false,      //是否需要用户点击确认进行跳转
                    AllowAccessTokensViaBrowser = true, // can return access_token to this client
                },
                new Client
                {
                    ClientId = "js",
                    ClientName = "JavaScript Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { "http://localhost:5003/callback.html" },
                    PostLogoutRedirectUris = { "http://localhost:5003/index.html" },
                    AllowedCorsOrigins = { "http://localhost:5003" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "MsCoreApi"
                    },
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource("roles","role",new List<string>{ "role"})   //自定义IdentityResource
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("MsCoreApi", "MyAPI")
                {
                    ApiSecrets =                     {
                        new Secret("secret".Sha256())
                    },
                    UserClaims = new [] { "role" }   //在JWT Token中包含Role 资源
                },

                new ApiResource("WebApiA", "WebApiA Service"),
                new ApiResource("WebApiB", "WebApiB Service"),
            };
        }

    }
}
