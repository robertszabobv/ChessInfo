using System;
using ChessInfo.Domain;
using FizzWare.NBuilder;
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
            var game = CreateGame();
            using (var repository = new GamesRepository())
            {
                Assert.DoesNotThrow(() => repository.AddGame(game));
            }
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
                .With(p => p.FirstName = $"white{DateTime.Now.Ticks}")
                .With(p => p.LastName = $"white{DateTime.Now.Ticks}")
                .With(p => p.Rating = 1100)
                .Build();
        }

        private Player CreateBlackPlayer()
        {
            return Builder<Player>.CreateNew()
                .With(p => p.PlayerId = 0)
                .With(p => p.FirstName = $"black{DateTime.Now.Ticks}")
                .With(p => p.LastName = $"black{DateTime.Now.Ticks}")
                .With(p => p.Rating = 1200)
                .Build();
        }
    }
}
