using ChessInfo.Business;
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

        private void AddPlayer(Player player)
        {
            using (var repository = new PlayersRepository())
            {
                repository.AddPlayer(player);                
            }
        }

        private Player CreateNewDummyPlayer()
        {
            return Builder<Player>.CreateNew()
                .With(p => p.PlayerId = 0)
                .Build();
        }
    }
}
