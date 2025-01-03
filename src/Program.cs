using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace YourNamespace
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
                pullACRules.Process(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        private static ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddLogging(configure =>
                {
                    configure.AddConsole();
                    configure.AddFile("Logs/fwdprocessor-{Date}.log");
                })
                .AddSingleton<PullACRules>()
                .BuildServiceProvider();
        }
    }
}
