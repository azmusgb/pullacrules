using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FwdProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = ConfigureServices();

            var pullACRules = serviceProvider.GetService<PullACRules>();

            if (pullACRules == null)
            {
                Console.WriteLine("Error: Unable to initialize PullACRules service.");
                return;
            }

            try
            {
                if (args.Length == 0)
                {
                    Console.WriteLine("No arguments provided.");
                    Console.Write("Please enter the path to the FWD file: ");
                    var inputPath = Console.ReadLine();

                    if (string.IsNullOrEmpty(inputPath) || !System.IO.File.Exists(inputPath))
                    {
                        Console.WriteLine("Error: Invalid file path provided. Exiting.");
                        return;
                    }

                    args = new[] { inputPath };
                }

                Console.WriteLine("Starting processing...");
                pullACRules.Process(args);
                Console.WriteLine("Processing completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                Console.WriteLine("Processing aborted. Check the logs for more details.");
            }
        }

        private static ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddLogging(configure =>
                {
                    configure.AddConsole();
                    configure.AddFile("Logs/fwdprocessor-{Date}.log"); // File-based logging
                    configure.SetMinimumLevel(LogLevel.Debug);
                })
                .AddSingleton<PullACRules>()
                .BuildServiceProvider();
        }
    }
}
