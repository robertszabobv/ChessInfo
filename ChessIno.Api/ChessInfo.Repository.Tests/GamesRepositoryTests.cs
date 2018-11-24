﻿using System;
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


        [Test]
        public void CreateGame_Succeeds()
        {
            var game = CreateGame();
            using (var repository = new GamesRepository())
            {
                Assert.DoesNotThrow(() => repository.AddGame(game));
            }
        }

        [Test]
        public void GetById_ReturnsGameById()
        {
            var game = CreateGame();
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
            var game = CreateGame();
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
            var game = CreateGame();
            using (var repository = new GamesRepository())
            {
                repository.AddGame(game);
                IEnumerable<Game> gamesLoaded = repository.GetGames(playerLastName: John);

                Assert.IsTrue(IsBlackOrWhitePlayerInAllGameByLastName(gamesLoaded));
            }            
        }

        [Test]
        public void GetGames_ByPlayerLasName_ReturnsGamesWherePlayerParticipatedAsWhite()
        {
            var game = CreateGame();
            using (var repository = new GamesRepository())
            {
                repository.AddGame(game);
                IEnumerable<Game> gamesLoaded = repository.GetGames(playerLastName: Doe);

                Assert.IsTrue(IsBlackOrWhitePlayerInAllGameByLastName(gamesLoaded));
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

        private bool IsBlackOrWhitePlayerInAllGameByLastName(IEnumerable<Game> games)
        {
            return games.All(g => g.WhitePlayer.LastName.StartsWith(Doe) || g.BlackPlayer.LastName.StartsWith(Doe));
        }

        private Game CreateGame()
        {
            var whitePlayer = AddWhitePlayer();
            var blackPlayer = AddWBlackPlayer();
            return Builder<Game>.CreateNew()
                .With(g => g.GameId = 0)
                .With(g => g.WhitePlayerId = whitePlayer.PlayerId)
                .With(g => g.BlackPlayerId = blackPlayer.PlayerId)
                .With(g => g.GameDate = DateTime.Now)
                .With(g => g.ResultDetail = new GameResultDetail(GameResultTypes.WhiteWins))
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
            var whitePlayer = CreateBlackPlayer();
            SavePlayer(whitePlayer);
            return whitePlayer;
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
