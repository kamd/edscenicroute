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
            Console.WriteLine("Hello world");
            Console.WriteLine(EDScenicRouteCore.Class1.TestString);
            Console.WriteLine();
            var data = new EDScenicRouteCore.DataUpdates.EDSMDownloader().DownloadPOIInfoAsJSON().Result;
            Console.WriteLine(data);
            Console.ReadKey();
        }
    }
}
