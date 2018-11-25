using System;
using System.Collections.Generic;
using ChessInfo.Api.Controllers;
using ChessInfo.Domain;
using FizzWare.NBuilder;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace ChessInfo.Api.Tests.Controllers
{
    [TestFixture]
    public class GamesControllerTests
    {
        [Test]
        public void CreateGame_Returns_400_WhenGameIsNull()
        {
            var controller = new GamesController(gamesRepository: null);
            IActionResult result = controller.CreateGame(null);

            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public void CreateGame_Returns_400_WhenWhitePlayerIdIsNotSet()
        {
            var controller = new GamesController(gamesRepository: null);
            var game = Builder<Game>.CreateNew()
                .With(g => g.WhitePlayerId = 0)
                .With(g => g.BlackPlayerId = 1)
                .With(g => g.GameDate == DateTime.Now)
                .With(g => g.OpeningClassification = "B12")
                .With(g => g.GameResult = 1)
                .Build();
            IActionResult result = controller.CreateGame(game);

            Assert.IsInstanceOf<BadRequestResult>(result);

        }

        [Test]
        public void CreateGame_Returns_400_WhenBlackPlayerIdIsNotSet()
        {
            var controller = new GamesController(gamesRepository: null);
            var game = Builder<Game>.CreateNew()
                .With(g => g.WhitePlayerId = 1)
                .With(g => g.BlackPlayerId = 0)
                .With(g => g.GameDate == DateTime.Now)
                .With(g => g.OpeningClassification = "B12")
                .With(g => g.GameResult = 1)
                .Build();
            IActionResult result = controller.CreateGame(game);

            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        
        [Test]
        public void CreateGame_Returns_400_WhenOpeningClassificationIsNotSet()
        {
            var controller = new GamesController(gamesRepository: null);
            var game = Builder<Game>.CreateNew()
                .With(g => g.WhitePlayerId = 1)
                .With(g => g.BlackPlayerId = 2)
                .With(g => g.GameDate == DateTime.Now)
                .With(g => g.OpeningClassification = null)
                .With(g => g.GameResult = 1)
                .Build();
            IActionResult result = controller.CreateGame(game);

            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public void CreateGame_Returns_400_WhenOpeningClassificationIsEmpty()
        {
            var controller = new GamesController(gamesRepository: null);
            var game = Builder<Game>.CreateNew()
                .With(g => g.WhitePlayerId = 1)
                .With(g => g.BlackPlayerId = 2)
                .With(g => g.GameDate == DateTime.Now)
                .With(g => g.OpeningClassification = string.Empty)
                .With(g => g.GameResult = 1)
                .Build();
            IActionResult result = controller.CreateGame(game);

            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public void CreateGame_Returns_400_WhenOpeningClassificationIsNotInCorrectFormat()
        {
            var controller = new GamesController(gamesRepository: null);
            var game = Builder<Game>.CreateNew()
                .With(g => g.WhitePlayerId = 1)
                .With(g => g.BlackPlayerId = 2)
                .With(g => g.GameDate == DateTime.Now)
                .With(g => g.OpeningClassification = "foo")
                .With(g => g.GameResult = 1)
                .Build();
            IActionResult result = controller.CreateGame(game);

            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public void CreateGame_Returns_400_WhenOpeningClassificationIsInCorrectFormatButLowercase()
        {
            var controller = new GamesController(gamesRepository: null);
            var game = Builder<Game>.CreateNew()
                .With(g => g.WhitePlayerId = 1)
                .With(g => g.BlackPlayerId = 2)
                .With(g => g.GameDate == DateTime.Now)
                .With(g => g.OpeningClassification = "a12")
                .With(g => g.GameResult = 1)
                .Build();
            IActionResult result = controller.CreateGame(game);

            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public void CreateGame_Returns_400_WhenWhitePlayerIsTheSameAsBlackPlayer()
        {
            var controller = new GamesController(gamesRepository: null);
            var game = Builder<Game>.CreateNew()
                .With(g => g.WhitePlayerId = 1)
                .With(g => g.BlackPlayerId = 1)
                .With(g => g.GameDate == DateTime.Now)
                .With(g => g.OpeningClassification = "A12")
                .With(g => g.GameResult = 1)
                .Build();
            IActionResult result = controller.CreateGame(game);

            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public void CreateGame_Returns_400_WhenGameResultIsZero()
        {
            var controller = new GamesController(gamesRepository: null);
            var game = Builder<Game>.CreateNew()
                .With(g => g.WhitePlayerId = 1)
                .With(g => g.BlackPlayerId = 1)
                .With(g => g.GameDate == DateTime.Now)
                .With(g => g.OpeningClassification = "A12")
                .With(g => g.GameResult = 0)
                .Build();
            IActionResult result = controller.CreateGame(game);

            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public void CreateGame_Returns_400_WhenGameResultGreaterThenThree()
        {
            var controller = new GamesController(gamesRepository: null);
            var game = Builder<Game>.CreateNew()
                .With(g => g.WhitePlayerId = 1)
                .With(g => g.BlackPlayerId = 1)
                .With(g => g.GameDate == DateTime.Now)
                .With(g => g.OpeningClassification = "A12")
                .With(g => g.GameResult = 4)
                .Build();
            IActionResult result = controller.CreateGame(game);

            Assert.IsInstanceOf<BadRequestResult>(result);
        }
        
        [Test]
        public void CreateGame_Returns_201_WhenGameIsValid()
        {
            var controller = new GamesController(new FakeGamesRepository());
            var game = Builder<Game>.CreateNew()
                .With(g => g.WhitePlayerId = 1)
                .With(g => g.BlackPlayerId = 2)
                .With(g => g.GameDate == DateTime.Now)
                .With(g => g.OpeningClassification = "A12")
                .With(g => g.GameResult = 1)
                .Build();
            IActionResult result = controller.CreateGame(game);

            Assert.IsInstanceOf<CreatedAtRouteResult>(result);
        }

        [Test]
        public void FilterGamesByPlayerLastName_Returns_404_WhenNoMatchingGamesFound()
        {
            const string nonExistentLastName = "foo";
            var repositoryMock = new Mock<IGamesRepository>();
            repositoryMock.Setup(r => r.GetGames(nonExistentLastName, null)).Returns(new List<Game>());
            var controller = new GamesController(repositoryMock.Object);
            IActionResult result = controller.GetGames(nonExistentLastName);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void FilterGamesByPlayerLastName_Returns_200_WhenNoMatchingGamesFound()
        {
            const string playerLastName = "foo";
            var repositoryMock = new Mock<IGamesRepository>();
            repositoryMock.Setup(r => r.GetGames(playerLastName, null)).Returns(new[] { CreateDummyGame(), CreateDummyGame() });
            var controller = new GamesController(repositoryMock.Object);
            IActionResult result = controller.GetGames(playerLastName);

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        private Game CreateDummyGame()
        {
            return Builder<Game>.CreateNew()
                .With(g => g.WhitePlayerId = 1)
                .With(g => g.BlackPlayerId = 2)
                .With(g => g.GameDate == DateTime.Now)
                .With(g => g.OpeningClassification = "A12")
                .With(g => g.GameResult = 1)
                .Build();
        }
    }
}
