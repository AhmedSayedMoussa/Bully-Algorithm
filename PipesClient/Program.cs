using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace PipeClient
{
    internal static class Program
    {
        public static IConfiguration Configuration;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            Configuration = builder.Build();

            ApplicationConfiguration.Initialize();
            Application.Run(new PipeClientView());
        }
    }
}




/*
 * - add logger
 * - add depenedency Injection
 * - bind Mysettings class into appsettings
 * - pass object and parse it right and take decision based on it
 *
 *
 */