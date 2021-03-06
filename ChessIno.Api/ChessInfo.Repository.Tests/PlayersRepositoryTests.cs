﻿using System;
using System.Linq;
using ChessInfo.Domain;
using FizzWare.NBuilder;
using NUnit.Framework;

namespace ChessInfo.Repository.Tests
{
    [TestFixture]
    [Category("Integration tests")]
    public class PlayersRepositoryTests
    {
        private readonly RepositoryTests _repositoryTests = new RepositoryTests();

        [Test]
        public void AddPlayer_Succeeds()
        {
            using (var repository = new PlayersRepository(_repositoryTests.GetContext()))
            {
                Assert.DoesNotThrow(() => repository.AddPlayer(CreateNewDummyPlayer()));
            }                
        }

        [Test]
        public void GetById_ReturnsPlayerBySuppliedPlayerId()
        {
            var player = CreateNewDummyPlayer();
            AddPlayer(player);
            using (var repository = new PlayersRepository(_repositoryTests.GetContext()))
            {
                var loadedPlayer = repository.GetById(player.PlayerId);

                Assert.AreEqual(player.PlayerId, loadedPlayer.PlayerId);
            }
        }

        [Test]
        public void GetById_ReturnsNullWhenNotFound()
        {
            const int nonExistentPlayerId = -1;
            using (var repository = new PlayersRepository(_repositoryTests.GetContext()))
            {
                var loadedPlayer = repository.GetById(nonExistentPlayerId);

                Assert.IsNull(loadedPlayer);
            }
        }

        [Test]
        public void GetPlayers_ReturnsPlayers()
        {
            using (var repository = new PlayersRepository(_repositoryTests.GetContext()))
            {
                var loadedPlayers = repository.GetPlayers();

                Assert.IsNotNull(loadedPlayers);
            }
        }

        [Test]
        public void GetPlayersByLastName_ReturnsPlayersWithSpecifiedLastNameOnly()
        {
            string lastName = DateTime.Now.DayOfWeek.ToString();
            using (var repository = new PlayersRepository(_repositoryTests.GetContext()))
            {
                repository.AddPlayer(CreatePlayerWithLastName(lastName));
                var loadedPlayers = repository.GetPlayers(lastName);

                Assert.IsTrue(loadedPlayers.All(p => p.LastName.StartsWith(lastName)));
            }
        }

        [Test]
        public void GetPlayersByLastName_ReturnsPlayersHavingLastNameStartingWithSearchedString()
        {
            string lastName = DateTime.Now.DayOfWeek.ToString();
            string lastNameToFind = lastName.Substring(0, 3);
            using (var repository = new PlayersRepository(_repositoryTests.GetContext()))
            {
                repository.AddPlayer(CreatePlayerWithLastName(lastName));
                var loadedPlayers = repository.GetPlayers(lastNameToFind);

                Assert.IsTrue(loadedPlayers.All(p => p.LastName.StartsWith(lastNameToFind)));
            }
        }

        [Test]
        public void GetPlayersByLastName_ReturnsPlayersHavingLastNameStartingWithSearchedStringInLowerCase()
        {
            string lastName = DateTime.Now.DayOfWeek.ToString();
            string lastNameToFind = lastName.Substring(0, 3).ToLower();
            using (var repository = new PlayersRepository(_repositoryTests.GetContext()))
            {
                repository.AddPlayer(CreatePlayerWithLastName(lastName));
                var loadedPlayers = repository.GetPlayers(lastNameToFind);

                Assert.IsTrue(loadedPlayers.All(p => p.LastName.StartsWith(lastNameToFind, StringComparison.OrdinalIgnoreCase)));
            }
        }

        [Test]
        public void DeletePlayer_DeletesPlayer()
        {
            string lastName = DateTime.Now.DayOfWeek.ToString();
            using (var repository = new PlayersRepository(_repositoryTests.GetContext()))
            {
                repository.AddPlayer(CreatePlayerWithLastName(lastName));
                int playerId = repository.GetPlayers(lastName).First().PlayerId;
                repository.DeletePlayer(playerId);
                Player playerReloaded = repository.GetById(playerId);

                Assert.IsNull(playerReloaded);
            }
        }

        [Test]
        public void UpdatePlayer_UpdatesPlayer()
        {
            string lastName = DateTime.Now.DayOfWeek.ToString();
            using (var repository = new PlayersRepository(_repositoryTests.GetContext()))
            {
                repository.AddPlayer(CreatePlayerWithLastName(lastName));
                Player playerToUpdate = repository.GetPlayers(lastName).First();
                playerToUpdate.FirstName = "updated fn";
                playerToUpdate.LastName = "updated ln";
                playerToUpdate.Rating = 3000;

                bool updatePerformed = repository.Update(playerToUpdate);

                Assert.IsTrue(updatePerformed);

                Player playerReloaded = repository.GetById(playerToUpdate.PlayerId);

                Assert.IsTrue(
                    playerReloaded.FirstName == "updated fn"
                    && playerReloaded.LastName == "updated ln"
                    && playerReloaded.Rating == 3000);
            }
        }

        [Test]
        public void UpdatePlayer_Returns_False_WhenNoUpdatePerformed()
        {
            bool updatePerformed;
            var nonExistentPlayer = CreateNewDummyPlayer();
            using (var repository = new PlayersRepository(_repositoryTests.GetContext()))
            {
                updatePerformed = repository.Update(nonExistentPlayer);
            }

            Assert.IsFalse(updatePerformed);
        }

        [Test]
        public void DeletePlayer_FailsWhenPlayerHasAnyGame()
        {
            var game = TestData.CreateGame();
            using (var gamesRepository = new GamesRepository(_repositoryTests.GetContext()))
            {
                gamesRepository.AddGame(game);
            }
            using (var repository = new PlayersRepository(_repositoryTests.GetContext()))
            {
                bool isPlayerDeleted = repository.DeletePlayer(game.WhitePlayerId);

                Assert.IsFalse(isPlayerDeleted);
            }
        }

        private void AddPlayer(Player player)
        {
            using (var repository = new PlayersRepository(_repositoryTests.GetContext()))
            {
                repository.AddPlayer(player);                
            }
        }

        private Player CreatePlayerWithLastName(string lastName)
        {
            return Builder<Player>.CreateNew()
                .With(p => p.PlayerId = 0)
                .With(p => p.LastName = lastName)
                .Build();
        }

        private Player CreateNewDummyPlayer()
        {
            return Builder<Player>.CreateNew()
                .With(p => p.PlayerId = 0)
                .Build();
        }
    }
}
