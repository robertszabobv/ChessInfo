using System;
using System.Collections.Generic;
using System.Linq;
using FizzWare.NBuilder;
using NUnit.Framework;

namespace ChessInfo.Api.IntegrationTests
{
    [TestFixture]
    [Category("Integration tests")]
    public class GamesTests
    {
        private const string OpeningClassificationInitial = "A12";
        private const string OpeningClassificationUpdated = "C30";
        private const string GamesRelativeUrl = "games";

        [Test]
        public void CRUD_Succeeds()
        {
            Game game = CreateDummyGame();
            Uri newGameUri = ChessInfoHttpClient.SendHttpPostToCreateNew(game, GamesRelativeUrl);
            GameDto gameLoaded = ChessInfoHttpClient.SendHttpGetFor<GameDto>(newGameUri);
            Assert.IsTrue(IsFilledWithExpectedValues(gameLoaded));

            game.GameId = gameLoaded.GameId;
            game.OpeningClassification = OpeningClassificationUpdated;
            ChessInfoHttpClient.SendHttpPutToUpdate(game, GamesRelativeUrl).Wait();
            GameDto gameUpdated = ChessInfoHttpClient.SendHttpGetFor<GameDto>(newGameUri);

            Assert.AreEqual(OpeningClassificationUpdated, gameUpdated.OpeningClassification);

            ChessInfoHttpClient.SendHttpDelete(newGameUri);
            var gameAfterDelete = ChessInfoHttpClient.SendHttpGetFor<GameDto>(newGameUri);
            Assert.IsNull(gameAfterDelete);
        }

        private bool IsFilledWithExpectedValues(GameDto dto)
        {
            return dto.GameId > 0
                   && !string.IsNullOrWhiteSpace(dto.WhitePlayer)
                   && !string.IsNullOrWhiteSpace(dto.BlackPlayer)
                   && dto.GameDate == DateTime.Today
                   && dto.OpeningClassification == OpeningClassificationInitial
                   && dto.Result == "1-0";
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
                .With(g => g.OpeningClassification = OpeningClassificationInitial)
                .With(g => g.GameResult = 1)
                .Build();
        }
    }
}
