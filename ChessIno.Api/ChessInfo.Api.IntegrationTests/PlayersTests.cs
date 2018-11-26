using System;
using System.Collections.Generic;
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
            Player player = CreateNewDummyPlayer();
            Uri newPlayerUri = ChessInfoHttpClient.SendHttpPostToCreateNew(player, PlayersRelativeUrl);
            Player playerLoaded = ChessInfoHttpClient.SendHttpGetFor<Player>(newPlayerUri);
            Assert.IsTrue(playerLoaded.PlayerId > 0);

            playerLoaded.LastName = UpdatedLastName;
            ChessInfoHttpClient.SendHttpPutToUpdate(playerLoaded, PlayersRelativeUrl).Wait();
            Player playerUpdated = ChessInfoHttpClient.SendHttpGetFor<Player>(newPlayerUri);
            
            Assert.IsTrue(playerUpdated.LastName == UpdatedLastName);

            ChessInfoHttpClient.SendHttpDelete(newPlayerUri);
            Player playerAfterDelete = ChessInfoHttpClient.SendHttpGetFor<Player>(newPlayerUri);
            Assert.IsNull(playerAfterDelete);
        }

        [Test]
        public void GetPlayers_ReturnsPlayers()
        {
            Player player = CreateNewDummyPlayer();
            ChessInfoHttpClient.SendHttpPostToCreateNew(player, PlayersRelativeUrl);
            IEnumerable<Player> players = ChessInfoHttpClient.SendHttpGetFor<Player>(PlayersRelativeUrl);

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
