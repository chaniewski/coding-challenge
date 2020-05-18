using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace ConstructionLine.CodingChallenge.Tests
{
    [TestFixture]
    public class SearchEngineTests : SearchEngineTestsBase
    {
        [Test]
        public void Test()
        {
            var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
            };

            var searchEngine = new SearchEngine(shirts);

            // Slightly improved the usability of the API by moving the ownership of collections into SearchOption class
            // This way it's not necessary to create the Colors and Sizes collection by the caller, just provide the values.
            var searchOptions = new SearchOptions
            {
                Colors = { Color.Red },
                Sizes = { Size.Small }
            };

            var results = searchEngine.Search(searchOptions);

            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(shirts, searchOptions, results.SizeCounts);
            AssertColorCounts(shirts, searchOptions, results.ColorCounts);
        }

        [Test]
        public void Search_OnlyByColor_ReturnsCorrectResult()
        {
            var shirts = CreateTestData();
            var searchEngine = new SearchEngine(shirts);
            var searchOptions = new SearchOptions
            {
                Colors = { Color.Red }
            };

            var results = searchEngine.Search(searchOptions);

            // NOTE: Normally, I prefer the more declarative way of making asserts,
            // but I understand the need of validating the results with code for this coding exercise.
            // Please treat the assertions in this test as an indication of my style, I'll revert
            // back to verification by provided routines in the base class for the remaining tests for brevity.
            Assert.That(results.Shirts, Has.Exactly(3).Items);
            Assert.That(results.Shirts.Select(s => s.Color), Is.All.EqualTo(Color.Red));
            Assert.That(results.Shirts.Select(s => s.Size), Is.EquivalentTo(Size.All));

            Assert.That(results.SizeCounts.Select(s => s.Size), Is.EquivalentTo(Size.All));
            Assert.That(results.SizeCounts.Select(s => s.Count), Has.All.EqualTo(1));

            Assert.That(results.ColorCounts.Select(s => s.Color), Is.EquivalentTo(Color.All));
            Assert.That(results.ColorCounts.Where(c => c.Color == Color.Red).Single().Count, Is.EqualTo(3));
            Assert.That(results.ColorCounts.Where(c => c.Color != Color.Red).Select(c => c.Count), Is.All.EqualTo(0));
        }

        [Test]
        public void Search_OnlyBySize_ReturnsCorrectResult()
        {
            var shirts = CreateTestData();
            var searchEngine = new SearchEngine(shirts);
            var searchOptions = new SearchOptions
            {
                Sizes = { Size.Large }
            };

            var results = searchEngine.Search(searchOptions);

            Assert.That(results.Shirts, Has.Exactly(5).Items);
            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(shirts.ToList(), searchOptions, results.SizeCounts);
            AssertColorCounts(shirts.ToList(), searchOptions, results.ColorCounts);
        }

        [Test]
        public void Search_ByColorAndSize_ReturnsCorrectResult()
        {
            var shirts = CreateTestData();
            var searchEngine = new SearchEngine(shirts);
            var searchOptions = new SearchOptions
            {
                Sizes = { Size.Large },
                Colors = { Color.White }
            };

            var results = searchEngine.Search(searchOptions);

            Assert.That(results.Shirts, Has.Exactly(1).Items);
            Assert.That(results.Shirts.Single().Name, Is.EqualTo("White - Large"));
            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(shirts.ToList(), searchOptions, results.SizeCounts);
            AssertColorCounts(shirts.ToList(), searchOptions, results.ColorCounts);
        }

        [Test]
        public void Search_NoParams_ReturnsAllShirts()
        {
            var shirts = CreateTestData();
            var searchEngine = new SearchEngine(shirts);
            var searchOptions = new SearchOptions();

            var results = searchEngine.Search(searchOptions);

            Assert.That(results.Shirts, Has.Exactly(15).Items);
            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(shirts.ToList(), searchOptions, results.SizeCounts);
            AssertColorCounts(shirts.ToList(), searchOptions, results.ColorCounts);
        }

        private IEnumerable<Shirt> CreateTestData()
        {
            // Let's return a lazy evaluated input, just for the demonstration that the concept works
            return Color.All.SelectMany(c => Size.All.Select(s =>
                new Shirt(Guid.NewGuid(), $"{c.Name} - {s.Name}", s, c)));
        }
    }
}
