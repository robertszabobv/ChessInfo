using Microsoft.EntityFrameworkCore;

namespace ChessInfo.Repository.Tests
{
    public class RepositoryTests
    {
        public ChessInfoContext GetContext()
        {
            return CreateInMemoryContext();
        }

        private ChessInfoContext CreateInMemoryContext()
        {
            var dbOptions = new DbContextOptionsBuilder<ChessInfoContext>()
                .UseInMemoryDatabase("ChessInfoTest")
                .Options;
            return new ChessInfoContext(dbOptions);
        }
    }
}