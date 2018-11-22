using System.Diagnostics.CodeAnalysis;
using ChessInfo.Api;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ChessIno.Api
{
    
    public class Program
    {
        [ExcludeFromCodeCoverage]
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        [ExcludeFromCodeCoverage]
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
