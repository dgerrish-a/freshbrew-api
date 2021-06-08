using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.RollingFileAlternate;
using System.Net;

namespace Riverbed.Test.FreshBrewApi
{
    public class Program
    {
        static Serilog.ILogger log = null;
        public static void Main(string[] args)
        {             
            try
            {
                //setup serilog to read from appsettings.json file
                var configuration = new ConfigurationBuilder()
                               .SetBasePath(Environment.CurrentDirectory)
                               .AddJsonFile("appsettings.json")
                               .Build();
                log = new LoggerConfiguration()
                      .ReadFrom.Configuration(configuration)
                      .MinimumLevel.Debug()
                      .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                      .Enrich.FromLogContext()
                      .CreateLogger();

                Log.Information("Starting FreshBrewApi");
                BuildWebHost(args).Run();                
               
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
               
            }
            finally
            {
                Log.CloseAndFlush();
            }
           
        }
        public static IWebHost BuildWebHost(string[] args)
        {

            int port = 9003;
            if (args.Length != 0)
            {
                try
                {
                    // arg will be of form: /port:value=5002
                    Console.WriteLine("Argument passed:" + args[0]);
                    string arg1 = args[0].ToLower();
                    if (arg1.StartsWith("/port"))
                    {
                        string[] tokens = arg1.Split(":");
                        string value = tokens[1];
                        if (tokens.Length == 2 && value.StartsWith("value"))
                        {
                            tokens = value.Split("=");
                            if (tokens.Length == 2)
                            {
                                port = Convert.ToInt32(tokens[1]); //index=1 will have port number
                                Console.WriteLine("Port number from command line:" + port);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Application uses default port of 9003. To override default port, use /port:value=<number>");

                    Console.WriteLine("Exception:" + ex.ToString());
                }
            }

            return WebHost.CreateDefaultBuilder(args)
                 .UseStartup<Startup>()
                 .UseKestrel(options =>
                 {
                     Console.WriteLine("Application's port:" + port);
                     options.Listen(IPAddress.Loopback, port);

                 })
                 .UseSerilog(log)
                 .Build();
        }
        /*
       public static IWebHost BuildWebHost(string[] args) =>
           WebHost.CreateDefaultBuilder(args)
               .UseStartup<Startup>()
               .UseSerilog(log)
               .UseKestrel(options =>
               {
                   options.Listen(IPAddress.Loopback, 5001);

               })
               .Build(); */
        }                

    }
