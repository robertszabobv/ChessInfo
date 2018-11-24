using ChessInfo.Domain;
using Microsoft.EntityFrameworkCore;

namespace ChessInfo.Repository
{
    public class ChessInfoContext : DbContext
    {
        public ChessInfoContext(DbContextOptions<ChessInfoContext> options) : base(options)
        {}

        public DbSet<Player> Players { get ; set; }      
    }
}
