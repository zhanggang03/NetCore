using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace APPClientPassword
{
    public class LoginRequestParam
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string GrantType { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    class Program
    {
        static readonly HttpClient Client = new HttpClient();
        static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                LoginRequestParam param = new LoginRequestParam();
                param.UserName = "AAA";
                param.Password = "123";
                var tokenResponse = await PostAsync("https://localhost:44397/api/identityservice/Login", param);  //ocelot 网关地址

                //访问Api
                //把令牌添加进请求
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",tokenResponse.AccessToken);
                //client.SetBearerToken(tokenResponse.AccessToken);
                Client.SetToken("Bearer", tokenResponse);
                var responseA = await Client.GetAsync("https://localhost:44397/webapia/values");
                if (!responseA.IsSuccessStatusCode)
                {
                    Console.WriteLine(responseA.StatusCode);
                }
                else
                {
                    var content = await responseA.Content.ReadAsStringAsync();
                    Console.WriteLine(JArray.Parse(content));
                }

                Client.SetToken("Bearer", tokenResponse);
                var responseB = await Client.GetAsync("https://localhost:44397/webapib/values");
                if (!responseB.IsSuccessStatusCode)
                {
                    Console.WriteLine(responseB.StatusCode);
                }
                else
                {
                    var content = await responseB.Content.ReadAsStringAsync();
                    Console.WriteLine(JArray.Parse(content));
                }
            });
            Console.ReadLine();
        }

        public static async Task<T> PostAsync<T>(string url, object data) where T : class, new()
        {
            try
            {
                string content = JsonConvert.SerializeObject(data);
                var buffer = Encoding.UTF8.GetBytes(content);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = await Client.PostAsync(url, byteContent).ConfigureAwait(false);
                string result = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    //logger.Error($"GetAsync End, url:{url}, HttpStatusCode:{response.StatusCode}, result:{result}");
                    return new T();
                }
                //logger.Info($"GetAsync End, url:{url}, result:{result}");
                return JsonConvert.DeserializeObject<T>(result);
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                    throw new System.Exception($"response :{responseContent}", ex);
                }
                throw;
            }
        }

        public static async Task<string> PostAsync(string url, object data)
        {
            try
            {
                string content = JsonConvert.SerializeObject(data);
                var buffer = Encoding.UTF8.GetBytes(content);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = await Client.PostAsync(url, byteContent).ConfigureAwait(false);
                string result = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    //logger.Error($"GetAsync End, url:{url}, HttpStatusCode:{response.StatusCode}, result:{result}");
                    return string.Empty;
                }
                //logger.Info($"GetAsync End, url:{url}, result:{result}");
                return result;
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                    throw new System.Exception($"response :{responseContent}", ex);
                }
                throw;
            }
        }

        //public async Task<string> GetAsync(string url, object data)
        //{
        //    try
        //    {
        //        string requestUrl = $"{url}?{GetQueryString(data)}";
        //        //logger.Info($"GetAsync Start, requestUrl:{requestUrl}");
        //        var response = await Client.GetAsync(requestUrl).ConfigureAwait(false);
        //        string result = await response.Content.ReadAsStringAsync();
        //        //logger.Info($"GetAsync End, requestUrl:{requestUrl}, HttpStatusCode:{response.StatusCode}, result:{result}");
        //        return result;
        //    }
        //    catch (WebException ex)
        //    {
        //        if (ex.Response != null)
        //        {
        //            string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
        //            throw new Exception($"Response :{responseContent}", ex);
        //        }
        //        throw;
        //    }
        //}
        //private static string GetQueryString(object obj)
        //{
        //    var properties = from p in obj.GetType().GetProperties()
        //                     where p.GetValue(obj, null) != null
        //                     select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());
        //    return String.Join("&", properties.ToArray());
        //}
    }
}
