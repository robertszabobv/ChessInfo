using System;
using System.Collections.Generic;
using System.Linq;
using FizzWare.NBuilder;
using NUnit.Framework;

namespace ChessInfo.Api.IntegrationTests
{
    [TestFixture]
    public class GamesTests
    {

        private const string GamesRelativeUrl = "games";

        [Test]
        public void CRUD_Succeeds()
        {
            Game game = CreateDummyGame();
            Uri newGameUri = ChessInfoHttpClient.SendHttpPostToCreateNew(game, GamesRelativeUrl);
            Game gameReloaded = ChessInfoHttpClient.SendHttpGetFor<Game>(newGameUri);
            Assert.IsTrue(gameReloaded.GameId > 0);
        }

        private Game CreateDummyGame()
        {
            IEnumerable<Player> allPlayers = ChessInfoHttpClient.SendHttpGetFor<Player>("players").ToList();
            var whitePlayer = allPlayers.First();
            var blackPlayer = allPlayers.Last();
            return Builder<Game>.CreateNew()
                .With(g => g.GameId = 0)
                .With(g => g.WhitePlayerId = whitePlayer.PlayerId)
                .With(g => g.BlackPlayerId = blackPlayer.PlayerId)
                .With(g => g.GameDate == DateTime.Now)
                .With(g => g.OpeningClassification = "A12")
                .With(g => g.GameResult = 1)
                .Build();
        }
    }
}
