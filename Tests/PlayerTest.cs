using System;
using NUnit.Framework;
using LonelyPlanet.Model;
using System.Drawing;

namespace Tests
{
    [TestFixture]
    class PlayerTest
    {
        [Test]
        public void DestroyBlock()
        {
            var map = new Map();
            var player = new Player("Test", new Coordinate(0, 50), 80, map);
            player.ManipulateBlock(new Point(0, 50));
            Assert.AreEqual("Air", map.GetChunk(0)[50].Name);
        }

        [Test]
        public void PlaceBlock()
        {
            var map = new Map();
            var player = new Player("Test", new Coordinate(0, 120), 80, map);
            player.Inventory.AddItem(new RockItem());
            player.ManipulateBlock(new Point(0, 120));
            Assert.AreEqual("Rock", map.GetChunk(0)[120].Name);
        }

        [Test]
        public void PlaceNotPlaceableBlock()
        {
            var map = new Map();
            var player = new Player("Test", new Coordinate(0, 120), 80, map);
            player.Inventory.AddItem(new GoldItem());
            player.ManipulateBlock(new Point(0, 120));
            Assert.AreEqual("Air", map.GetChunk(0)[120].Name);
        }

        //This method also testing Inventory class
        [Test]
        public void DestroyBlockWithFullInventory()
        {
            var map = new Map();
            var player = new Player("Test", new Coordinate(0, 50), 80, map);
            for(var i = 0; i < player.Inventory.SolidMaxAmount; i++)
                player.Inventory.AddItem(new GoldItem(), i);
            player.ManipulateBlock(new Point(0, 50));
            for(var i = 0; i < player.Inventory.SolidMaxAmount; i++)
            {
                if (player.Inventory.GetCell(i).Item.Name != "Gold")
                    Assert.Fail();
            }
        }
    }
}
