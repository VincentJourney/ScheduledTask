using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AS.QuartZ;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AS.ScheduledTask
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                    logging.AddFile();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    QuartzStartup.Start();
                    webBuilder.UseStartup<Startup>();
                });
    }
}
