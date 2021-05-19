using System;
using NUnit.Framework;
using LonelyPlanet.Model;

namespace Tests
{
    [TestFixture]
    public class InventoryTest
    {
        private Random rand;

        [SetUp]
        public void SetUp()
        {
            rand = new Random();
        }

        [Test]
        public void InitialiseWithTrueArgs()
        {
            var inventory = new Inventory(5, 10, 10);
            Assert.Pass();
        }

        [Test]
        public void InitialiseWithWrongArgs()
        {
            Assert.Catch(typeof(ArgumentException), ()=>new Inventory(100500, 1, 1));
        }

        [Test]
        public void AddItemInEmpty()
        {
            var inventory = new Inventory(5, 10, 10);
            Assert.IsTrue(inventory.AddItem(new GoldItem()));
            Assert.IsFalse(inventory.IsCellEmpty(0));
            var cell = inventory.GetCell(0);
            Assert.AreEqual("Gold", cell.Item.Name);
            Assert.AreEqual(1, cell.Count);
        }

        [Test]
        public void AddTwoEqualItemsInEmpty()
        {
            var inventory = new Inventory(5, 10, 0);
            Assert.IsTrue(inventory.AddItem(new GoldItem()));
            Assert.IsTrue(inventory.AddItem(new GoldItem()));
            Assert.AreEqual(2, inventory.GetCell(0).Count);
        }

        [Test]
        public void AddItemInCellByIndex()
        {
            var inventory = new Inventory(5, 10, 0);
            Assert.IsTrue(inventory.AddItem(new GoldItem(), cell: 3));
            Assert.IsTrue(inventory.IsCellEmpty(0));
            Assert.IsFalse(inventory.IsCellEmpty(3));
            Assert.AreEqual("Gold", inventory.GetCell(3).Item.Name);
        }

        [Test]
        public void AddTwoEqualItemsInOneCell()
        {
            var inventory = new Inventory(5, 10, 0);
            Assert.IsTrue(inventory.AddItem(new GoldItem(), 2));
            Assert.IsTrue(inventory.AddItem(new GoldItem(), 2));
            Assert.AreEqual(2, inventory.GetCell(2).Count);
        }

        [Test]
        public void AddItemInFull()
        {
            var inventory = new Inventory(1, 1, 0);
            inventory.AddItem(new GoldItem());
            Assert.IsFalse(inventory.AddItem(new RockItem()));
        }

        [Test]
        public void AddItemInBusyCell()
        {
            var inventory = new Inventory(5, 10, 0);
            inventory.AddItem(new GoldItem(), cell: 2);
            Assert.IsFalse(inventory.AddItem(new RockItem(), 2));
            Assert.AreEqual("Gold", inventory.GetCell(2).Item.Name);
            Assert.AreEqual(1, inventory.GetCell(2).Count);
        }

        [Test]
        public void EmptyCell()
        {
            var inventory = new Inventory(5, 10, 10);
            var cellIndex = rand.Next(0, 10);
            Assert.IsTrue(inventory.IsCellEmpty(cellIndex));
        }
    }
}
