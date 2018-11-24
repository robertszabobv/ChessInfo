using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ChessInfo.Repository
{
    public abstract class BaseRepository : IDisposable
    {
        protected ChessInfoContext Context { get; }

        protected BaseRepository()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            var optionsBuilder = new DbContextOptionsBuilder<ChessInfoContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("ChessInfoContext"));
            Context = new ChessInfoContext(optionsBuilder.Options);

            Context.Database.Migrate();
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}
