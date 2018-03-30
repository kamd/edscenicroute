using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using EDScenicRouteCore;
using EDScenicRouteCore.Data;
using NUnit.Framework;

namespace EDScenicRouteTest
{
    [TestFixture]
    public class ScenicSuggestionCalculatorTest
    {
        [Test]
        public void TestCalculate()
        {
            var poi1 = new GalacticPOI()
            {
                Coordinates = new Vector3(6, 5, 5),
                DistanceFromSol = 250,
                Id = "POI1",
                Name = "Exciting POI",
                Type = "Nebula"
            };

            var poi2 = new GalacticPOI()
            {
                Coordinates = new Vector3(5, 5, 20),
                DistanceFromSol = 300,
                Id = "POI2",
                Name = "Farawayland",
                Type = "Nebula"
            };

            var poi3 = new GalacticPOI()
            {
                Coordinates = new Vector3(3, 5, 3),
                DistanceFromSol = 220,
                Id = "POI3",
                Name = "On the way POI",
                Type = "Nebula"
            };

            var pois = new List<GalacticPOI>() {poi1, poi2, poi3};

            var system1 = new GalacticSystem() {Coordinates = new Vector3(1, 1, 1), Name = "Home System"};
            var system2 = new GalacticSystem() {Coordinates = new Vector3(10, 10, 10), Name = "Destination System"};
            var systems = new List<GalacticSystem>() {system1, system2};

            var calculator = new ScenicSuggestionCalculator(pois, systems);

            (var originalDistance, var suggestions) = calculator.GenerateSuggestions(system1, system2, 15f);
            Assert.AreEqual(
                new List<ScenicSuggestion>() {new ScenicSuggestion(poi1, 0.08541584f), new ScenicSuggestion(poi3, 0.4010582f) },
                suggestions);
            Assert.AreEqual(15.5884571f, originalDistance);

            (originalDistance, suggestions) = calculator.GenerateSuggestions(system1, system2, 0.2f);
            Assert.AreEqual(
                new List<ScenicSuggestion>() { new ScenicSuggestion(poi1, 0.08541584f) }, suggestions);
            Assert.AreEqual(15.5884571f, originalDistance);

            (originalDistance, suggestions) = calculator.GenerateSuggestions(system1, system2, 0f);
            Assert.AreEqual(
                new List<ScenicSuggestion>(), suggestions);
            Assert.AreEqual(15.5884571f, originalDistance);

            (originalDistance, suggestions) = calculator.GenerateSuggestions(system1, system2, 20f);
            Assert.AreEqual(
                new List<ScenicSuggestion>()
                {
                    new ScenicSuggestion(poi1, 0.08541584f),
                    new ScenicSuggestion(poi2, 16.4832211f),
                    new ScenicSuggestion(poi3, 0.4010582f)
                }, 
                suggestions);
            Assert.AreEqual(15.5884571f, originalDistance);
        }
    }
}
