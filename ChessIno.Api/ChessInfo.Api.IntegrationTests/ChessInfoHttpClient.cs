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
        
        public static IEnumerable<T> SendHttpGetPlayers<T>(HttpClient client, string relativeUrl)
        {
            var getPlayersUri = new Uri($"{ServiceBaseUrl}/{relativeUrl}");
            var response = client.GetAsync(getPlayersUri).Result;
            response.EnsureSuccessStatusCode();
            return ReadContentAs<IEnumerable<T>>(response);
        }

        public static void SendHttpDeletePlayer(HttpClient client, Uri newPlayerUri)
        {
            var response = client.DeleteAsync(newPlayerUri).Result;
            response.EnsureSuccessStatusCode();
        }

        public static T SendHttpGetPlayer<T>(HttpClient client, Uri newPlayerUri)
        {
            var response = client.GetAsync(newPlayerUri).Result;
            return ReadContentAs<T>(response);
        }

        public static T ReadContentAs<T>(HttpResponseMessage response)
        {
            string responseBody = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(responseBody);
        }

        public static Uri SendHttpPostToCreateNewDummyPlayer<T>(HttpClient client, T player, string relativeUrl)
        {            
            var createPlayerUri = new Uri($"{ServiceBaseUrl}/{relativeUrl}");
            StringContent playerHttpContent = CreateHttpContentFrom(player);
            var response = client.PostAsync(createPlayerUri, playerHttpContent).Result;
            return response.Headers.Location;
        }

        public static async Task SendHttpPutToCUpdatePlayer<T>(HttpClient client, T player, string relativeUrl)
        {
            var playerUri = new Uri($"{ServiceBaseUrl}/{relativeUrl}");
            StringContent playerHttpContent = CreateHttpContentFrom(player);
            await client.PutAsync(playerUri, playerHttpContent);
        }

        private static StringContent CreateHttpContentFrom<T>(T player)
        {
            string playerJson = JsonConvert.SerializeObject(player);
            var playerHttpContent = new StringContent(playerJson, Encoding.UTF8, "application/json");
            return playerHttpContent;
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
