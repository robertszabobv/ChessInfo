using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
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
        public void NewPlayerGetsCreated_And_ResponseHeaderUriCanBeUsedToLoadIt()
        {
            HttpClient client = CreateHttpClient();
            Uri newPlayerUri = SendHttpPostPlayer(client);
            Player playerLoaded = SendHttpGetPlayer(client, newPlayerUri);

            Assert.IsTrue(playerLoaded.PlayerId > 0);
        }

        [Test]
        public void GetPlayers_ReturnsPlayers()
        {
            HttpClient client = CreateHttpClient();
            SendHttpPostPlayer(client);
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

        private Player SendHttpGetPlayer(HttpClient client, Uri newPlayerUri)
        {
            var response = client.GetAsync(newPlayerUri).Result;
            response.EnsureSuccessStatusCode();
            return ReadContentAs<Player>(response);
        }

        private static T ReadContentAs<T>(HttpResponseMessage response)
        {
            string responseBody = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(responseBody);
        }

        private Uri SendHttpPostPlayer(HttpClient client)
        {
            Player player = CreateNewDummyPlayer();
            var createPlayerUri = new Uri($"{_serviceBaseUrl}/{PlayersRelativeUrl}");
            StringContent playerHttpContent = CreateHttpContentFrom(player);
            var response = client.PostAsync(createPlayerUri, playerHttpContent).Result;
            return response.Headers.Location;
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
