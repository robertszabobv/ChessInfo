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
        //private const string PlayersRelativeUrl = "players";
        private static readonly string ServiceBaseUrl;

        static ChessInfoHttpClient()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            ServiceBaseUrl = configuration.GetSection("ChessInfoApi")["Url"];
        }
        
        public static IEnumerable<Player> SendHttpGetPlayers(HttpClient client, string relativeUrl)
        {
            var getPlayersUri = new Uri($"{ServiceBaseUrl}/{relativeUrl}");
            var response = client.GetAsync(getPlayersUri).Result;
            response.EnsureSuccessStatusCode();
            return ReadContentAs<IEnumerable<Player>>(response);
        }

        public static void SendHttpDeletePlayer(HttpClient client, Uri newPlayerUri)
        {
            var response = client.DeleteAsync(newPlayerUri).Result;
            response.EnsureSuccessStatusCode();
        }

        public static Player SendHttpGetPlayer(HttpClient client, Uri newPlayerUri)
        {
            var response = client.GetAsync(newPlayerUri).Result;
            return ReadContentAs<Player>(response);
        }

        public static T ReadContentAs<T>(HttpResponseMessage response)
        {
            string responseBody = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(responseBody);
        }

        public static Uri SendHttpPostToCreateNewDummyPlayer(HttpClient client, Player player, string relativeUrl)
        {            
            var createPlayerUri = new Uri($"{ServiceBaseUrl}/{relativeUrl}");
            StringContent playerHttpContent = CreateHttpContentFrom(player);
            var response = client.PostAsync(createPlayerUri, playerHttpContent).Result;
            return response.Headers.Location;
        }

        public static async Task SendHttpPutToCUpdatePlayer(HttpClient client, Player player, string relativeUrl)
        {
            var playerUri = new Uri($"{ServiceBaseUrl}/{relativeUrl}");
            StringContent playerHttpContent = CreateHttpContentFrom(player);
            await client.PutAsync(playerUri, playerHttpContent);
        }

        private static StringContent CreateHttpContentFrom(Player player)
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
