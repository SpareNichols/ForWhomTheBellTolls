using DbUp;
using System;
using System.Linq;
using System.Reflection;

namespace BellScheduleManager.DatabaseSetup
{
    class Program
    {
        static int Main(string[] args)
        {
            var connectionString =
        args.FirstOrDefault()
        ?? "Server=(local); Database=ForWhomTheBellTolls_Local; Trusted_connection=true";

            EnsureDatabase.For.SqlDatabase(connectionString);

            var upgrader =
                DeployChanges.To
                    .SqlDatabase(connectionString)
                    .WithScriptsFromFileSystem(".\\Structure")
                    .WithScriptsFromFileSystem(".\\TestData")
                    .LogToConsole()
                    .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
#if DEBUG
                Console.ReadLine();
#endif
                return -1;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();
            return 0;
        }
    }
}
