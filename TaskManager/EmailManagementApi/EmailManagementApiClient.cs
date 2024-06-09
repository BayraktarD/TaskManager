using Newtonsoft.Json;
using NuGet.Common;
using System.Net.Http.Headers;
using System.Text;
using TaskManager.EmailManagementApi.Entities;
using static Humanizer.On;

namespace TaskManager.Services.API
{
    public class EmailManagementApiClient
    {
       // private static readonly HttpClient client = new HttpClient();

        private readonly string emailManagementApiKey = "cf13f65f-4ee6-4726-9e30-aa1992f93815";
        private readonly string ApiUrl = "https://localhost:7120/";

        public EmailManagementApiClient()
        {
            //client.BaseAddress = new Uri("https://localhost:7120/"); 
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private async Task<string> GetAsync(string url, string key)
        {
            var result = "";
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                if (key.Length > 0)
                {
                    client.DefaultRequestHeaders.Add("authenticationKey", key);
                }

                using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false))
                {
                    var content = await response.Content.ReadAsStringAsync();
                    result = content;
                }
            }

            return result;
        }

        public  EmailManagementApiResponse SendEmailThroughEmailManagementApi(ClientAppEmail clientAppEmail)
        {
            EmailManagementApiResponse emailManagementApiResponse = new EmailManagementApiResponse();

            if (!String.IsNullOrEmpty(clientAppEmail.EmailManagementApiKey))
            {
                string token = GetEmailManagementApiToken(clientAppEmail.EmailManagementApiKey);


                var json = JsonConvert.SerializeObject(clientAppEmail);
                var responseJson = PostAsync(json, ApiUrl + "api/EmailManagement/SendEmailAsync", token).Result;
                emailManagementApiResponse = JsonConvert.DeserializeObject<EmailManagementApiResponse>(responseJson);
            }

            return emailManagementApiResponse;
        }

        private string GetEmailManagementApiToken(string emailManagementApiKey)
        {
            var responseJson = GetAsync(ApiUrl + "ClaimsLogin", emailManagementApiKey).Result;
            return responseJson;
        }

        public async Task<string> PostAsync(string json, string url, string token = "", string authenticationKey = "")
        {
            var result = "";
            var handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
            using (var client = new HttpClient(handler))
            using (var request = new HttpRequestMessage(HttpMethod.Post, url))
            {
                client.Timeout = new TimeSpan(0, 4, 0);
                if (token.Length > 0)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                if (authenticationKey.Length > 0)
                {
                    client.DefaultRequestHeaders.Add("authenticationKey", authenticationKey);
                }

                using (var stringContent = new StringContent(json, Encoding.UTF8, "application/json"))
                {
                    request.Content = stringContent;
                    using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false))
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        result = content;
                    }
                }
            }
            return result;

        }

        //public static async Task<T> GetAsync<T>(string endpoint)
        //{
        //    HttpResponseMessage response = await client.GetAsync(endpoint);
        //    response.EnsureSuccessStatusCode();

        //    string responseData = await response.Content.ReadAsStringAsync();
        //    return JsonConvert.DeserializeObject<T>(responseData);
        //}

        //public static async Task<T> PostAsync<T>(string endpoint, object data)
        //{
        //    string json = JsonConvert.SerializeObject(data);
        //    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");


        //    HttpResponseMessage response = await client.PostAsync(endpoint, content);
        //    response.EnsureSuccessStatusCode();

        //    string responseData = await response.Content.ReadAsStringAsync();
        //    return JsonConvert.DeserializeObject<T>(responseData);
        //}
    }
}
