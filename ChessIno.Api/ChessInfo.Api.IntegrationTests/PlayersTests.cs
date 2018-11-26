﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using FizzWare.NBuilder;
using NUnit.Framework;

namespace ChessInfo.Api.IntegrationTests
{
    [TestFixture]
    [Category("Integration tests")]
    public class PlayersTests
    {
        private const string PlayersRelativeUrl = "players";
        const string UpdatedLastName = "updated by http put";
    
        [Test]
        public void CRUD_Succeeds()
        {
            HttpClient client = ChessInfoHttpClient.CreateHttpClient();
            Player player = CreateNewDummyPlayer();
            Uri newPlayerUri = ChessInfoHttpClient.SendHttpPostToCreateNew(client, player, PlayersRelativeUrl);
            Player playerLoaded = ChessInfoHttpClient.SendHttpGetFor<Player>(client, newPlayerUri);
            Assert.IsTrue(playerLoaded.PlayerId > 0);

            playerLoaded.LastName = UpdatedLastName;
            ChessInfoHttpClient.SendHttpPutToUpdate(client, playerLoaded, PlayersRelativeUrl).Wait();
            Player playerUpdated = ChessInfoHttpClient.SendHttpGetFor<Player>(client, newPlayerUri);
            
            Assert.IsTrue(playerUpdated.LastName == UpdatedLastName);

            ChessInfoHttpClient.SendHttpDelete(client, newPlayerUri);
            Player playerAfterDelete = ChessInfoHttpClient.SendHttpGetFor<Player>(client, newPlayerUri);
            Assert.IsNull(playerAfterDelete);
        }

        [Test]
        public void GetPlayers_ReturnsPlayers()
        {
            HttpClient client = ChessInfoHttpClient.CreateHttpClient();
            Player player = CreateNewDummyPlayer();
            ChessInfoHttpClient.SendHttpPostToCreateNew(client, player, PlayersRelativeUrl);
            IEnumerable<Player> players = ChessInfoHttpClient.SendHttpGetFor<Player>(client, PlayersRelativeUrl);

            Assert.IsNotNull(players);
            Assert.IsNotEmpty(players);
        }       

        private Player CreateNewDummyPlayer()
        {
            return Builder<Player>.CreateNew()
                .With(p => p.PlayerId = 0)
                .Build();
        }
    }
}
