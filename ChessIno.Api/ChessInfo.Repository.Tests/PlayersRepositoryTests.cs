using System;
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
        [Test]
        public void AddPlayer_Succeeds()
        {
            using (var repository = new PlayersRepository())
            {
                Assert.DoesNotThrow(() => repository.AddPlayer(CreateNewDummyPlayer()));
            }                
        }

        [Test]
        public void GetById_ReturnsPlayerBySuppliedPlayerId()
        {
            var player = CreateNewDummyPlayer();
            AddPlayer(player);
            using (var repository = new PlayersRepository())
            {
                var loadedPlayer = repository.GetById(player.PlayerId);

                Assert.AreEqual(player.PlayerId, loadedPlayer.PlayerId);
            }
        }

        [Test]
        public void GetById_ReturnsNullWhenNotFound()
        {
            const int nonExistentPlayerId = -1;
            using (var repository = new PlayersRepository())
            {
                var loadedPlayer = repository.GetById(nonExistentPlayerId);

                Assert.IsNull(loadedPlayer);
            }
        }

        [Test]
        public void GetPlayers_ReturnsPlayers()
        {
            using (var repository = new PlayersRepository())
            {
                var loadedPlayers = repository.GetPlayers();

                Assert.IsNotNull(loadedPlayers);
            }
        }

        [Test]
        public void GetPlayersByLastName_ReturnsPlayersWithSpecifiedLastNameOnly()
        {
            string lastName = DateTime.Now.DayOfWeek.ToString();
            using (var repository = new PlayersRepository())
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
            using (var repository = new PlayersRepository())
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
            using (var repository = new PlayersRepository())
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
            using (var repository = new PlayersRepository())
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
            using (var repository = new PlayersRepository())
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
            using (var repository = new PlayersRepository())
            {
                updatePerformed = repository.Update(nonExistentPlayer);
            }

            Assert.IsFalse(updatePerformed);
        }

        [Test]
        public void DeletePlayer_FailsWhenPlayerHasAnyGame()
        {

        }

        private void AddPlayer(Player player)
        {
            using (var repository = new PlayersRepository())
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
