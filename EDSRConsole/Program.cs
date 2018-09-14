using EDScenicRouteCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Console;

namespace EDSRConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("hostparams.json", optional: true)
                .Build();
            
            var galaxy = new Galaxy(config, new ConsoleLogger("Console", (s, level) => true, true));
            galaxy.Initialise(CancellationToken.None).Wait();

            if (args.Contains("--updatesystems"))
            {
                Console.WriteLine(DateTime.Now);
                galaxy.UpdateSystemsFromFile(args[1], CancellationToken.None);
                Console.WriteLine(DateTime.Now);
            }


            
            Console.WriteLine("System you are travelling from:");
            var fromName = Console.ReadLine();
            Console.WriteLine("System you are travelling to:");
            var toName = Console.ReadLine();
            Console.WriteLine("Acceptable extra distance to travel (in lightyears):");
            try
            {
                if (float.TryParse(Console.ReadLine(), out var extraDistance))
                {
                    if (extraDistance > 500f)
                    {
                        Console.WriteLine("Extra distance should be 500Ly or less.");
                        return;
                    }
                    var results = galaxy.GenerateSuggestions(fromName, toName, extraDistance).Result;
                    Console.WriteLine($"Direct distance: {results.StraightLineDistance} Ly");
                    if (!results.Suggestions.Any())
                    {
                        Console.WriteLine("There's nowhere else interesting near your route.");
                        return;
                    }
                    
                    Console.WriteLine("On your way, why not visit...");
                    foreach(var s in results.Suggestions)
                    {
                        Console.WriteLine(s);
                    }
                    
                } else
                {
                    Console.WriteLine("That wasn't a valid number for distance.");
                }
            }
            finally
            {
                galaxy.SaveSystems();
                Console.ReadKey();
            }
            
        }

    }
}
