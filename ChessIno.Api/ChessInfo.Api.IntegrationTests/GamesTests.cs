using System;
using System.Collections.Generic;
using System.Text;
using FizzWare.NBuilder;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace ChessInfo.Api.IntegrationTests
{
    [TestFixture]
    public class GamesTests
    {

        private const string PlayersRelativeUrl = "games";
        const string UpdatedLastName = "updated by http put";

        [Test]
        public void CRUD_Succeeds()
        {

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
