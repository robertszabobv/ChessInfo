using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ChessInfo.Api.IntegrationTests
{
    internal static class ChessInfoHttpClient
    {
        private static readonly string ServiceBaseUrl;

        static ChessInfoHttpClient()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            ServiceBaseUrl = configuration.GetSection("ChessInfoApi")["Url"];
        }
        
        public static IEnumerable<T> SendHttpGetFor<T>(HttpClient client, string relativeUrl)
        {
            var getElementsUri = new Uri($"{ServiceBaseUrl}/{relativeUrl}");
            var response = client.GetAsync(getElementsUri).Result;
            response.EnsureSuccessStatusCode();
            return ReadContentAs<IEnumerable<T>>(response);
        }

        public static void SendHttpDelete(HttpClient client, Uri deleteUri)
        {
            var response = client.DeleteAsync(deleteUri).Result;
            response.EnsureSuccessStatusCode();
        }

        public static T SendHttpGetFor<T>(HttpClient client, Uri newElementUri)
        {
            var response = client.GetAsync(newElementUri).Result;
            return ReadContentAs<T>(response);
        }

        public static T ReadContentAs<T>(HttpResponseMessage response)
        {
            string responseBody = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(responseBody);
        }

        public static Uri SendHttpPostToCreateNew<T>(HttpClient client, T element, string relativeUrl)
        {            
            var createUri = new Uri($"{ServiceBaseUrl}/{relativeUrl}");
            StringContent httpContent = CreateHttpContentFrom(element);
            var response = client.PostAsync(createUri, httpContent).Result;
            return response.Headers.Location;
        }

        public static async Task SendHttpPutToUpdate<T>(HttpClient client, T element, string relativeUrl)
        {
            var putUri = new Uri($"{ServiceBaseUrl}/{relativeUrl}");
            StringContent httpContent = CreateHttpContentFrom(element);
            await client.PutAsync(putUri, httpContent);
        }

        private static StringContent CreateHttpContentFrom<T>(T element)
        {
            string elementJson = JsonConvert.SerializeObject(element);
            var httpContent = new StringContent(elementJson, Encoding.UTF8, "application/json");
            return httpContent;
        }

        public static HttpClient CreateHttpClient()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(ServiceBaseUrl)
            };
            return client;
        }
    }
}
