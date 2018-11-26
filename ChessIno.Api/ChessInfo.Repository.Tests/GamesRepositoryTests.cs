using System;
using System.Collections.Generic;
using System.Linq;
using ChessInfo.Domain;
using NUnit.Framework;

namespace ChessInfo.Repository.Tests
{
    [TestFixture]
    [Category("Integration tests")]
    public class GamesRepositoryTests
    {       
        [Test]
        public void CreateGame_Succeeds()
        {
            var game = TestData.CreateGame(TestData.A01);
            using (var repository = new GamesRepository())
            {
                Assert.DoesNotThrow(() => repository.AddGame(game));
            }
        }

        [Test]
        public void GetById_ReturnsGameByIdWithPlayersAndResultDetail()
        {
            var game = TestData.CreateGame(TestData.B99);
            using (var repository = new GamesRepository())
            {
                repository.AddGame(game);
                var gameLoaded = repository.GetById(game.GameId);

                Assert.IsNotNull(gameLoaded);
                Assert.IsNotNull(gameLoaded.WhitePlayer);
                Assert.IsNotNull(gameLoaded.BlackPlayer);
                Assert.IsNotNull(gameLoaded.ResultDetail);
            }
        }

        [Test]
        public void GetGames_WithNoFilter_ReturnsAllGamesWithPlayers()
        {
            var game = TestData.CreateGame(TestData.A01);
            using (var repository = new GamesRepository())
            {
                repository.AddGame(game);
                IEnumerable<Game> gamesLoaded = repository.GetGames().ToList();

                Assert.IsNotEmpty(gamesLoaded);
                Assert.IsTrue(gamesLoaded.All(g => g.WhitePlayer != null && g.BlackPlayer != null));
            }
        }

        [Test]
        public void GetGames_ByPlayerLasName_ReturnsGamesWherePlayerParticipatedAsBlack()
        {
            var game = TestData.CreateGame(TestData.B99);
            using (var repository = new GamesRepository())
            {
                repository.AddGame(game);
                IEnumerable<Game> gamesLoaded = repository.GetGames(playerLastName: TestData.John);

                Assert.IsTrue(IsBlackOrWhitePlayerInAllGameByLastName(gamesLoaded, TestData.John));
            }            
        }

        [Test]
        public void GetGames_ByPlayerLasName_ReturnsGamesWherePlayerParticipatedAsWhite()
        {
            var game = TestData.CreateGame(TestData.A01);
            using (var repository = new GamesRepository())
            {
                repository.AddGame(game);
                IEnumerable<Game> gamesLoaded = repository.GetGames(playerLastName: TestData.Doe);

                Assert.IsTrue(IsBlackOrWhitePlayerInAllGameByLastName(gamesLoaded, TestData.Doe));
            }
        }

        [Test]
        public void GetGames_ByNonExistentPlayerLasName_ReturnsEmpty()
        {
            using (var repository = new GamesRepository())
            {
                IEnumerable<Game> gamesLoaded = repository.GetGames(playerLastName: TestData.NonExistentName);

                Assert.IsEmpty(gamesLoaded);
            }
        }

        [Test]
        public void GetGames_ByPlayerFirstName_ReturnsEmpty()
        {
            var game = TestData.CreateGame(TestData.B99);
            using (var repository = new GamesRepository())
            {
                repository.AddGame(game);
                IEnumerable<Game> gamesLoaded = repository.GetGames(playerLastName: TestData.Vincent);

                Assert.IsEmpty(gamesLoaded);
            }
        }

        [Test]
        public void GetGamesBy_OpeningClassification_ReturnsGamesStartingWithSearchingValue()
        {
            const string searchingFor = "B";
            var b99Game = TestData.CreateGame(TestData.B99);
            var a01Game = TestData.CreateGame(TestData.A01);
            using (var repository = new GamesRepository())
            {
                repository.AddGame(b99Game);
                repository.AddGame(a01Game);
                IEnumerable<Game> gamesLoaded = repository.GetGames(playerLastName: null, openingClassification: searchingFor);

                Assert.IsTrue(gamesLoaded.All(g => g.OpeningClassification.StartsWith(searchingFor)));
            }
        }

        [Test]
        public void GetGamesBy_OpeningClassification_ReturnsEmptyForLowerCase()
        {
            const string searchingFor = "b";
            var b99Game = TestData.CreateGame(TestData.B99);
            var a01Game = TestData.CreateGame(TestData.A01);
            using (var repository = new GamesRepository())
            {
                repository.AddGame(b99Game);
                repository.AddGame(a01Game);
                IEnumerable<Game> gamesLoaded = repository.GetGames(playerLastName: null, openingClassification: searchingFor);

                Assert.IsEmpty(gamesLoaded);
            }
        }

        [Test]
        public void GetGamesBy_LastNameAndOpeningClassification_ReturnsGamesMatchingSpecificPlayerAndOpeningClassification()
        {
            const string searchedOpeningClassification = "A0";
            var b99Game = TestData.CreateGame(TestData.B99);
            var a01Game = TestData.CreateGame(TestData.A01);
            using (var repository = new GamesRepository())
            {
                repository.AddGame(b99Game);
                repository.AddGame(a01Game);
                IEnumerable<Game> gamesLoaded = repository.GetGames(playerLastName: TestData.Doe, openingClassification: searchedOpeningClassification);

                Assert.IsTrue(gamesLoaded.All(
                    g => (IsWhiteOrBlackPlayerLastNameMatchingSearchedValue(g))
                        && g.OpeningClassification.StartsWith(searchedOpeningClassification)));
            }
        }

        [Test]
        public void UpdateGame_UpdatesGame()
        {
            const string d10 = "D10";
            var b99Game = TestData.CreateGame(TestData.B99);
            using (var repository = new GamesRepository())
            {
                repository.AddGame(b99Game);
                var newWhitePlayer = TestData.AddWhitePlayer();
                var newBlackPlayer = TestData.AddWBlackPlayer();
                b99Game.WhitePlayerId = newWhitePlayer.PlayerId;
                b99Game.BlackPlayerId = newBlackPlayer.PlayerId;
                b99Game.GameDate = DateTime.Today.AddYears(-1);
                b99Game.ResultDetail = new GameResultDetail(GameResultTypes.Draw);
                b99Game.OpeningClassification = d10;
                bool isUpdated = repository.Update(b99Game);
                repository.GetById(b99Game.BlackPlayerId);

                Assert.IsTrue(isUpdated
                      && b99Game.WhitePlayerId == newWhitePlayer.PlayerId
                      && b99Game.BlackPlayerId == newBlackPlayer.PlayerId
                      && b99Game.GameDate == DateTime.Today.AddYears(-1)
                      && b99Game.GameResult == (int)GameResultTypes.Draw
                      && b99Game.OpeningClassification == d10);
            }
        }

        [Test]
        public void UpdateGame_ReturnsFalse_WhenUpdatingANonExistingGame()
        {
            var dummyGame = TestData.CreateGame(TestData.A01);
            dummyGame.GameId = -1;
            using (var repository = new GamesRepository())
            {
                bool isUpdated = repository.Update(dummyGame);

                Assert.IsFalse(isUpdated);
            }            
        }

        [Test]
        public void DeleteGame_DeletesGame()
        {
            var b99Game = TestData.CreateGame(TestData.B99);
            using (var repository = new GamesRepository())
            {
                repository.AddGame(b99Game);
                repository.DeleteGame(b99Game.GameId);
                var gameReloaded = repository.GetById(b99Game.GameId);

                Assert.IsNull(gameReloaded);
            }
        }
        
        private bool IsWhiteOrBlackPlayerLastNameMatchingSearchedValue(Game game)
        {
            return game.WhitePlayer.LastName.StartsWith(TestData.Doe) || game.BlackPlayer.LastName.StartsWith(TestData.Doe);
        }

        private bool IsBlackOrWhitePlayerInAllGameByLastName(IEnumerable<Game> games, string playerLastName)
        {
            return games.All(g => g.WhitePlayer.LastName.StartsWith(playerLastName) || g.BlackPlayer.LastName.StartsWith(playerLastName));
        }        
    }
}
