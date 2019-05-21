using System.Linq;
using ChessInfo.Domain;
using Microsoft.EntityFrameworkCore;

namespace ChessInfo.Repository
{
    public class ChessInfoContext : DbContext
    {
        public ChessInfoContext(DbContextOptions<ChessInfoContext> options) : base(options)
        {
            //Database.Migrate();
        }

        public DbSet<Player> Players { get ; set; }
        public DbSet<Game> Games { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        //    {
        //        relationship.DeleteBehavior = DeleteBehavior.Restrict;
        //    }

        //    base.OnModelCreating(modelBuilder);
        //}
    }
}
