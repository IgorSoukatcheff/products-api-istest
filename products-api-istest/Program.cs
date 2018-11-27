using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Common;
using NLog.Web;

namespace products_api_istest
{
    public class Program
    {
        public static void Main(string[] args)
        {
           
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("azurenlog.config").GetCurrentClassLogger();
            try
            {
                InternalLogger.LogFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "nlog-internals.txt");
                logger.Debug("init main");
                BuildWebHost(args).Run();
            }
            catch (Exception exception)
            {
                //NLog: catch setup errors
                logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                })
                .UseNLog()
                .UseContentRoot(InExecutionDirectory())
                .UseKestrel()
                .UseIISIntegration()                              
                .Build();

        public static string InExecutionDirectory(string filename = "") => Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), filename);
    }

}

