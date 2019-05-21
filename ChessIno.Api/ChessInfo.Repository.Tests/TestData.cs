using System;
using ChessInfo.Domain;
using FizzWare.NBuilder;

namespace ChessInfo.Repository.Tests
{
    internal static class TestData
    {
        public const string John = "John";
        public const string Doe = "Doe";
        public const string Vincent = "Vincent";
        public const string NonExistentName = "NonExistentName";
        public const string A01 = "A01";
        public const string B99 = "B99";

        public static Game CreateGame(string openingClassification = A01)
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

        public static Player AddWhitePlayer()
        {
            var whitePlayer = CreateWhitePlayer();
            SavePlayer(whitePlayer);
            return whitePlayer;
        }

        public static Player AddWBlackPlayer()
        {
            var blackPlayer = CreateBlackPlayer();
            SavePlayer(blackPlayer);
            return blackPlayer;
        }

        private static void SavePlayer(Player player)
        {
            var repositoryTests = new RepositoryTests();
            using (var repository = new PlayersRepository(repositoryTests.GetContext()))
            {
                repository.AddPlayer(player);
            }
        }

        private static Player CreateWhitePlayer()
        {
            return Builder<Player>.CreateNew()
                .With(p => p.PlayerId = 0)
                .With(p => p.FirstName = $"{John}{DateTime.Now.Ticks.ToString().Substring(0, 3)}")
                .With(p => p.LastName = $"{Doe}{DateTime.Now.Ticks.ToString().Substring(0, 3)}")
                .With(p => p.Rating = 1100)
                .Build();
        }

        private static Player CreateBlackPlayer()
        {
            return Builder<Player>.CreateNew()
                .With(p => p.PlayerId = 0)
                .With(p => p.FirstName = $"{Vincent}{DateTime.Now.Ticks.ToString().Substring(0, 3)}")
                .With(p => p.LastName = $"{John}{DateTime.Now.Ticks.ToString().Substring(0, 3)}")
                .With(p => p.Rating = 1200)
                .Build();
        }
    }
}
