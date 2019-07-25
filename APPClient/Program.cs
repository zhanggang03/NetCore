using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace APPClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                // 从元数据中发现终结点,查找IdentityServer
                var client = new HttpClient();
                var disco = await client.GetDiscoveryDocumentAsync("https://localhost:44379");
                if (disco.IsError)
                {
                    Console.WriteLine(disco.Error);
                    return;
                }

                //向IdentityServer请求令牌
                var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                {
                    Address = disco.TokenEndpoint,
                    ClientId = "client",
                    ClientSecret = "secret",
                    //Scope = "MsCoreApi"
                });
                if (tokenResponse.IsError)
                {
                    Console.WriteLine(tokenResponse.Error);
                    return;
                }
                Console.WriteLine(tokenResponse.Json);


                //访问Api
                //把令牌添加进请求
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",tokenResponse.AccessToken);
                //client.SetBearerToken(tokenResponse.AccessToken);
                client.SetToken("Bearer", tokenResponse.AccessToken);
                var response = await client.GetAsync("https://localhost:44312/api/Identity");
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine(response.StatusCode);
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(JArray.Parse(content));
                }
            });
            Console.ReadLine();
        }
    }
}
