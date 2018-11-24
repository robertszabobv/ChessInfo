using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NUnit.Framework;

namespace ChessInfo.Api.IntegrationTests
{    
    [TestFixture]
    [Category("Integration tests")]
    public class PlayersTests
    {
        private static string _serviceBaseUrl;
        private const string PlayersRelativeUrl = "players";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            _serviceBaseUrl = configuration.GetSection("ChessInfoApi")["Url"];
        }

        [Test]
        public void CRUD_Succeeds()
        {
            HttpClient client = CreateHttpClient();
            Uri newPlayerUri = SendHttpPostToCreateNewDummyPlayer(client);
            Player playerLoaded = SendHttpGetPlayer(client, newPlayerUri);
            Assert.IsTrue(playerLoaded.PlayerId > 0);

            var playersUri = new Uri($"{_serviceBaseUrl}/{PlayersRelativeUrl}");
            SendHttpPutToCUpdatePlayer(client, playersUri, playerLoaded).Wait();
            Player playerUpdated = SendHttpGetPlayer(client, newPlayerUri);
            Assert.IsTrue(playerUpdated.LastName == "updated by http put");

            SendHttpDeletePlayer(client, newPlayerUri);
            Player playerAfterDelete = SendHttpGetPlayer(client, newPlayerUri);
            Assert.IsNull(playerAfterDelete);
        }

        [Test]
        public void GetPlayers_ReturnsPlayers()
        {
            HttpClient client = CreateHttpClient();
            SendHttpPostToCreateNewDummyPlayer(client);
            IEnumerable<Player> players = SendHttpGetPlayers(client);

            Assert.IsNotNull(players);
            Assert.IsNotEmpty(players);
        }


        private IEnumerable<Player> SendHttpGetPlayers(HttpClient client)
        {
            var getPlayersUri = new Uri($"{_serviceBaseUrl}/{PlayersRelativeUrl}");
            var response = client.GetAsync(getPlayersUri).Result;
            response.EnsureSuccessStatusCode();
            return ReadContentAs<IEnumerable<Player>>(response);
        }

        private void SendHttpDeletePlayer(HttpClient client, Uri newPlayerUri)
        {
            var response = client.DeleteAsync(newPlayerUri).Result;
            response.EnsureSuccessStatusCode();
        }

        private Player SendHttpGetPlayer(HttpClient client, Uri newPlayerUri)
        {
            var response = client.GetAsync(newPlayerUri).Result;
            return ReadContentAs<Player>(response);
        }

        private static T ReadContentAs<T>(HttpResponseMessage response)
        {
            string responseBody = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(responseBody);
        }

        private Uri SendHttpPostToCreateNewDummyPlayer(HttpClient client)
        {
            Player player = CreateNewDummyPlayer();
            var createPlayerUri = new Uri($"{_serviceBaseUrl}/{PlayersRelativeUrl}");
            StringContent playerHttpContent = CreateHttpContentFrom(player);
            var response = client.PostAsync(createPlayerUri, playerHttpContent).Result;
            return response.Headers.Location;
        }

        private async Task SendHttpPutToCUpdatePlayer(HttpClient client, Uri playerUri, Player player)
        {
            player.LastName = "updated by http put";
            StringContent playerHttpContent = CreateHttpContentFrom(player);
            await client.PutAsync(playerUri, playerHttpContent);
        }

        private static StringContent CreateHttpContentFrom(Player player)
        {
            string playerJson = JsonConvert.SerializeObject(player);
            var playerHttpContent = new StringContent(playerJson, Encoding.UTF8, "application/json");
            return playerHttpContent;
        }

        private static HttpClient CreateHttpClient()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(_serviceBaseUrl)
            };
            return client;
        }

        private Player CreateNewDummyPlayer()
        {
            return Builder<Player>.CreateNew()
                .With(p => p.PlayerId = 0)
                .Build();
        }
    }
}
