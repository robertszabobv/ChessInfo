﻿using System.Collections.Generic;
using ChessInfo.Api.Controllers;
using ChessInfo.Business;
using FizzWare.NBuilder;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ChessInfo.Api.Tests.Controllers
{
    [TestFixture]
    public class PlayersControllerTests
    {
        [Test]
        public void CreatePlayer_Returns_400_WhenPlayerIsNull()
        {
            var controller = new PlayersController(new FakePlayersRepository());
            IActionResult result = controller.CreatePlayer(player: null);

            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public void CreatePlayer_Returns_201_WhenPlayerIsCreated()
        {
            var controller = new PlayersController(new FakePlayersRepository());
            IActionResult result = controller.CreatePlayer(CreateNewDummyPlayer());

            Assert.IsInstanceOf<CreatedAtRouteResult>(result);
        }

        [Test]
        public void CreatePlayer_Returns_TheCreatedPlayer()
        {
            var controller = new PlayersController(new FakePlayersRepository());
            IActionResult result = controller.CreatePlayer(CreateNewDummyPlayer());

            Assert.IsInstanceOf<Player>(((CreatedAtRouteResult)result).Value); 
        }

        [Test]
        public void CreatePlayer_SavesPlayer()
        {
            IPlayersRepository repository = new FakePlayersRepository();
            var controller = new PlayersController(repository);
            IActionResult createPlayerResult = controller.CreatePlayer(CreateNewDummyPlayer());
            Player createdPlayer = (Player)((CreatedAtRouteResult) createPlayerResult).Value;
            var loadedPlayer = repository.GetById(createdPlayer.PlayerId);
            
            Assert.IsTrue(AreTheSamePlayers(loadedPlayer, createdPlayer));
        }

        [Test]
        public void CreatingPlayer_WithEmptyFirstName_Returns_400()
        {
            var controller = new PlayersController(playersRepository: null);
            IActionResult result = controller.CreatePlayer(CreateNewPlayerWithEmptyFirstName());

            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public void CreatingPlayer_WithNullLastName_Returns_400()
        {
            var controller = new PlayersController(playersRepository: null);
            IActionResult result = controller.CreatePlayer(CreateNewPlayerWithNullLastName());

            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public void CreatingPlayer_WithRatingZero_Returns_400()
        {
            var controller = new PlayersController(playersRepository: null);
            IActionResult result = controller.CreatePlayer(CreateNewPlayerWithZeroRating());

            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public void CreatingPlayer_WithNegativeRating_Returns_400()
        {
            var controller = new PlayersController(playersRepository: null);
            IActionResult result = controller.CreatePlayer(CreateNewPlayerWithNegativeRating());

            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public void GetPlayer_Returns_404_WhenPlayerIsNotFound()
        {
            const int nonExistingPlayerId = -1;
            var repositoryMock = new Mock<IPlayersRepository>();
            repositoryMock.Setup(r => r.GetById(nonExistingPlayerId)).Returns((Player)null);
            var controller = new PlayersController(repositoryMock.Object);
            IActionResult result = controller.GetPlayer(nonExistingPlayerId);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void GetPlayer_Returns_200_WhenPlayerFound()
        {
            const int evilPlayerId = 666;
            var repositoryMock = new Mock<IPlayersRepository>();
            repositoryMock.Setup(r => r.GetById(evilPlayerId)).Returns(new Player());
            var controller = new PlayersController(repositoryMock.Object);
            IActionResult result = controller.GetPlayer(evilPlayerId);

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void GetPlayer_Returns_PlayerById_WhenPlayerFound()
        {
            const int idOfPlayerToFind = 666;
            var repositoryMock = new Mock<IPlayersRepository>();
            repositoryMock.Setup(r => r.GetById(idOfPlayerToFind)).Returns(new Player{PlayerId = idOfPlayerToFind});
            var controller = new PlayersController(repositoryMock.Object);
            IActionResult result = controller.GetPlayer(idOfPlayerToFind);

            var player = (Player)((OkObjectResult)result).Value;
            Assert.AreEqual(idOfPlayerToFind, player.PlayerId);
        }

        [Test]
        public void GetPlayers_Returns_404_WhenNoPlayerFound()
        {
            var repositoryMock = new Mock<IPlayersRepository>();
            repositoryMock.Setup(r => r.GetPlayers()).Returns((IEnumerable<Player>) null);
            var controller = new PlayersController(repositoryMock.Object);
            IActionResult result = controller.GetPlayers();

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        private bool AreTheSamePlayers(Player expected, Player actual)
        {
            return expected.FirstName.Equals(actual.FirstName)
                   && expected.LastName.Equals(actual.LastName)
                   && expected.Rating.Equals(actual.Rating);
        }

        private Player CreateNewDummyPlayer()
        {
            return Builder<Player>.CreateNew()
                .With(p => p.PlayerId = 0)
                .Build();
        }

        private Player CreateNewPlayerWithEmptyFirstName()
        {
            return Builder<Player>.CreateNew()
                .With(p => p.PlayerId = 0)
                .With(p => p.FirstName = string.Empty)
                .Build();
        }

        private Player CreateNewPlayerWithNullLastName()
        {
            return Builder<Player>.CreateNew()
                .With(p => p.PlayerId = 0)
                .With(p => p.LastName = null)
                .Build();
        }

        private Player CreateNewPlayerWithZeroRating()
        {
            return Builder<Player>.CreateNew()
                .With(p => p.PlayerId = 0)
                .With(p => p.Rating = 0)
                .Build();
        }

        private Player CreateNewPlayerWithNegativeRating()
        {
            return Builder<Player>.CreateNew()
                .With(p => p.PlayerId = 0)
                .With(p => p.Rating = -1)
                .Build();
        }
    }
}