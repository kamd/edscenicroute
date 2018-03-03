using EDScenicRouteCore.DataUpdates;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDScenicRouteTest.Tests.DataUpdates
{
    [TestFixture]
    public class EDSMSystemEnquirerTest
    {
        [Test]
        public void TestGetGende()
        {
            var enq = new EDSMSystemEnquirer();
            var system = enq.GetSystemAsync("Gende").Result;
            Assert.AreEqual("Gende", system.Name);
            Assert.AreEqual("<93.28125, -117.75, -1.46875>", system.Coordinates.ToString());
            Console.WriteLine(system.Coordinates);
        }

    }
}
