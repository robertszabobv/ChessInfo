using System;

namespace ChessInfo.Repository
{
    public abstract class BaseRepository : IDisposable
    {
        protected ChessInfoContext Context { get; }

        protected BaseRepository(ChessInfoContext context)
        {
            Context = context;
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}
