using System;
using NUnit.Framework;
using LonelyPlanet.Model;

namespace Tests
{
    [TestFixture]
    class MapTest
    {
        [Test]
        public void GetChunksOfDefaultBiome()
        {
            var map = new Map();
            var defaultWidth = map.Width;
            for (var x = 0; x < defaultWidth; x++)
            {
                Assert.NotNull(map.GetChunk(x) != null);
                Assert.AreEqual(defaultWidth, map.Width);
            }
        }

        [Test]
        public void GetBiomeByDefaultBiomeCoordinates()
        {
            var map = new Map();
            var defaultWidth = map.Width;
            for(var x = 0; x < defaultWidth; x++)
            {
                Assert.NotNull(map.GetBiomeByX(x));
                Assert.AreEqual(defaultWidth, map.Width);
            }
        }

        [Test]
        public void GenerateNewBiome()
        {
            var map = new Map();
            var currentWidth = map.Width;
            map.GenerateNextBiome(Direction.Right);
            Assert.IsTrue(map.Width > currentWidth);
            currentWidth = map.Width;
            map.GenerateNextBiome(Direction.Left);
            Assert.IsTrue(map.Width > currentWidth);
        }

        [Test]
        public void GenerateNewBiomeByCallingGetFunction()
        {
            var map = new Map();
            var currentWidth = map.Width;
            Assert.NotNull(map.GetBiomeByX(currentWidth + 1));
            Assert.IsTrue(map.Width > currentWidth);
            currentWidth = map.Width;
            Assert.NotNull(map.GetBiomeByX(-1));
            Assert.IsTrue(map.Width > currentWidth);
        }
    }
}
