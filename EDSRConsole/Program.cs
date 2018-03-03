using EDScenicRouteCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDSRConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var galaxy = new Galaxy();
            galaxy.Initialise().Wait();
            Console.WriteLine("System you are travelling from:");
            var fromName = Console.ReadLine();
            Console.WriteLine("System you are travelling to:");
            var toName = Console.ReadLine();
            Console.WriteLine("Acceptable extra distance to travel (in lightyears):");
            if (float.TryParse(Console.ReadLine(), out var extraDistance))
            {
                (var distance, var suggestions) = galaxy.GenerateSuggestions(fromName, toName, extraDistance).Result;
                Console.WriteLine($"Direct distance: {distance} Ly");
                if (suggestions.Count() > 0)
                {
                    Console.WriteLine("On your way, why not visit...");
                    foreach(var s in suggestions)
                    {
                        Console.WriteLine(s);
                    }
                } else
                {
                    Console.WriteLine("There's nowhere else interesting near your route.");
                }
                
            } else
            {
                Console.WriteLine("That wasn't a valid number for distance.");
            }

            galaxy.SaveSystems();
            Console.ReadKey();
        }
    }
}
