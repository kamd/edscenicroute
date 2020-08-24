using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using EDScenicRouteWeb.Server.Models;
using EDScenicRouteWeb.Server.Services;
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
                X = 6,
                Y = 5,
                Z = 5,
                DistanceFromSol = 250,
                Id = 1,
                Name = "Exciting POI",
                Type = GalacticPOIType.nebula
            };

            var poi2 = new GalacticPOI()
            {
                X = 5,
                Y = 5,
                Z = 20,
                DistanceFromSol = 300,
                Id = 2,
                Name = "Farawayland",
                Type = GalacticPOIType.nebula
            };

            var poi3 = new GalacticPOI()
            {
                X = 3,
                Y = 5,
                Z = 3,
                DistanceFromSol = 220,
                Id = 3,
                Name = "On the way POI",
                Type = GalacticPOIType.nebula
            };

            var pois = new List<GalacticPOI>() {poi1, poi2, poi3};

            var system1 = new GalacticSystem() {X = 1, Y = 1, Z = 1, Name = "Home System"};
            var system2 = new GalacticSystem() {X = 10, Y = 10, Z = 10, Name = "Destination System"};
            var systems = new List<GalacticSystem>() {system1, system2};

            var calculator = new ScenicSuggestionCalculator(pois.AsQueryable(), systems.AsQueryable(), 5f);

            var results = calculator.GenerateSuggestions(system1, system2, 15f);
            Assert.AreEqual(
                new List<ScenicSuggestion>() {new ScenicSuggestion(poi1, 0.08541584f, 48.1481476f), new ScenicSuggestion(poi3, 0.4010582f, 29.62963f) },
                results.Suggestions);
            Assert.AreEqual(15.5884571f, results.StraightLineDistance);

            results = calculator.GenerateSuggestions(system1, system2, 0.2f);
            Assert.AreEqual(
                new List<ScenicSuggestion>() { new ScenicSuggestion(poi1, 0.08541584f, 48.1481476f) }, results.Suggestions);
            Assert.AreEqual(15.5884571f, results.StraightLineDistance);

            results = calculator.GenerateSuggestions(system1, system2, 0f);
            Assert.AreEqual(
                new List<ScenicSuggestion>(), results.Suggestions);
            Assert.AreEqual(15.5884571f, results.StraightLineDistance);

            results = calculator.GenerateSuggestions(system1, system2, 20f);
            Assert.AreEqual(
                new List<ScenicSuggestion>()
                {
                    new ScenicSuggestion(poi1, 0.08541584f, 48.1481476f),
                    new ScenicSuggestion(poi3, 0.4010582f, 29.62963f),
                    new ScenicSuggestion(poi2, 16.4832211f, 100f)
                },
                results.Suggestions);
            Assert.AreEqual(15.5884571f, results.StraightLineDistance);
        }
    }
}
