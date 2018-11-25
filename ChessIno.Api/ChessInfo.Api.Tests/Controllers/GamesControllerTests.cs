using System;
using System.Collections.Generic;
using System.Text;
using ChessInfo.Api.Controllers;
using ChessInfo.Domain;
using FizzWare.NBuilder;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace ChessInfo.Api.Tests.Controllers
{
    [TestFixture]
    public class GamesControllerTests
    {
        [Test]
        public void CreateGame_Returns_400_When_WhitePlayerIdIsNotSet()
        {
            var controller = new GamesController(gamesRepository: null);
            var game = Builder<Game>.CreateNew()
                .With(g => g.WhitePlayerId = 0)
                .Build();
            IActionResult result = controller.CreateGame(game);

            Assert.IsInstanceOf<BadRequestResult>(result);

        }
    }
}
