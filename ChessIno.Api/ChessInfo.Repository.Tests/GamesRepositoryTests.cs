using System;
using System.Collections.Generic;
using System.Linq;
using ChessInfo.Domain;
using FizzWare.NBuilder;
using NUnit.Framework;

namespace ChessInfo.Repository.Tests
{
    [TestFixture]
    [Category("Integration tests")]
    public class GamesRepositoryTests
    {
        private const string John = "John";
        private const string Doe = "Doe";
        private const string Vincent = "Vincent";
        private const string NonExistentName = "NonExistentName";
        private const string A01 = "A01";
        private const string B99 = "B99";


        [Test]
        public void CreateGame_Succeeds()
        {
            var game = CreateGame(A01);
            using (var repository = new GamesRepository())
            {
                Assert.DoesNotThrow(() => repository.AddGame(game));
            }
        }

        [Test]
        public void GetById_ReturnsGameById()
        {
            var game = CreateGame(B99);
            using (var repository = new GamesRepository())
            {
                repository.AddGame(game);
                var gameLoaded = repository.GetById(game.GameId);

                Assert.IsNotNull(gameLoaded);
            }
        }

        [Test]
        public void GetGames_WithNoFilter_ReturnsAllGames()
        {
            var game = CreateGame(A01);
            using (var repository = new GamesRepository())
            {
                repository.AddGame(game);
                IEnumerable<Game> gamesLoaded = repository.GetGames();

                Assert.IsNotEmpty(gamesLoaded);
            }
        }

        [Test]
        public void GetGames_ByPlayerLasName_ReturnsGamesWherePlayerParticipatedAsBlack()
        {
            var game = CreateGame(B99);
            using (var repository = new GamesRepository())
            {
                repository.AddGame(game);
                IEnumerable<Game> gamesLoaded = repository.GetGames(playerLastName: John);

                Assert.IsTrue(IsBlackOrWhitePlayerInAllGameByLastName(gamesLoaded, John));
            }            
        }

        [Test]
        public void GetGames_ByPlayerLasName_ReturnsGamesWherePlayerParticipatedAsWhite()
        {
            var game = CreateGame(A01);
            using (var repository = new GamesRepository())
            {
                repository.AddGame(game);
                IEnumerable<Game> gamesLoaded = repository.GetGames(playerLastName: Doe);

                Assert.IsTrue(IsBlackOrWhitePlayerInAllGameByLastName(gamesLoaded, Doe));
            }
        }

        [Test]
        public void GetGames_ByNonExistentPlayerLasName_ReturnsEmpty()
        {
            using (var repository = new GamesRepository())
            {
                IEnumerable<Game> gamesLoaded = repository.GetGames(playerLastName: NonExistentName);

                Assert.IsEmpty(gamesLoaded);
            }
        }

        [Test]
        public void GetGames_ByPlayerFirstName_ReturnsEmpty()
        {
            var game = CreateGame(B99);
            using (var repository = new GamesRepository())
            {
                repository.AddGame(game);
                IEnumerable<Game> gamesLoaded = repository.GetGames(playerLastName: Vincent);

                Assert.IsEmpty(gamesLoaded);
            }
        }

        [Test]
        public void GetGamesBy_OpeningClassification_ReturnsGamesStartingWithSearchingValue()
        {
            const string searchingFor = "B";
            var b99Game = CreateGame(B99);
            var a01Game = CreateGame(A01);
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
            var b99Game = CreateGame(B99);
            var a01Game = CreateGame(A01);
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
            var b99Game = CreateGame(B99);
            var a01Game = CreateGame(A01);
            using (var repository = new GamesRepository())
            {
                repository.AddGame(b99Game);
                repository.AddGame(a01Game);
                IEnumerable<Game> gamesLoaded = repository.GetGames(playerLastName: Doe, openingClassification: searchedOpeningClassification);

                Assert.IsTrue(gamesLoaded.All(
                    g => (IsWhiteOrBlackPlayerLastNameMatchingSearchedValue(g, Doe))
                        && g.OpeningClassification.StartsWith(searchedOpeningClassification)));
            }
        }

        private bool IsWhiteOrBlackPlayerLastNameMatchingSearchedValue(Game game, string searchedLastName)
        {
            return game.WhitePlayer.LastName.StartsWith(Doe) || game.BlackPlayer.LastName.StartsWith(Doe);
        }

        private bool IsBlackOrWhitePlayerInAllGameByLastName(IEnumerable<Game> games, string playerLastName)
        {
            return games.All(g => g.WhitePlayer.LastName.StartsWith(playerLastName) || g.BlackPlayer.LastName.StartsWith(playerLastName));
        }

        private Game CreateGame(string openingClassification)
        {
            var whitePlayer = AddWhitePlayer();
            var blackPlayer = AddWBlackPlayer();
            return Builder<Game>.CreateNew()
                .With(g => g.GameId = 0)
                .With(g => g.WhitePlayerId = whitePlayer.PlayerId)
                .With(g => g.BlackPlayerId = blackPlayer.PlayerId)
                .With(g => g.GameDate = DateTime.Now)
                .With(g => g.ResultDetail = new GameResultDetail(GameResultTypes.WhiteWins))
                .With(g => g.OpeningClassification = openingClassification)
                .Build();
        }

        private Player AddWhitePlayer()
        {
            var whitePlayer = CreateWhitePlayer();
            SavePlayer(whitePlayer);
            return whitePlayer;
        }

        private Player AddWBlackPlayer()
        {
            var blackPlayer = CreateBlackPlayer();
            SavePlayer(blackPlayer);
            return blackPlayer;
        }

        private void SavePlayer(Player player)
        {
            using (var repository = new PlayersRepository())
            {
                repository.AddPlayer(player);
            }            
        }

        private Player CreateWhitePlayer()
        {
            return Builder<Player>.CreateNew()
                .With(p => p.PlayerId = 0)
                .With(p => p.FirstName = $"{John}{DateTime.Now.Ticks}")
                .With(p => p.LastName = $"{Doe}{DateTime.Now.Ticks}")
                .With(p => p.Rating = 1100)
                .Build();
        }

        private Player CreateBlackPlayer()
        {
            return Builder<Player>.CreateNew()
                .With(p => p.PlayerId = 0)
                .With(p => p.FirstName = $"{Vincent}{DateTime.Now.Ticks}")
                .With(p => p.LastName = $"{John}{DateTime.Now.Ticks}")
                .With(p => p.Rating = 1200)
                .Build();
        }
    }
}
