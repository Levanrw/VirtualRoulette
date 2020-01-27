using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace VirtualRoulette.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
          return  WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((c) => c.AddXmlFile("Web.xml", false, true))
                .UseStartup<Startup>();
            
        }
    }
}
