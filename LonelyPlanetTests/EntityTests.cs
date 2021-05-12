using System;
using NUnit.Framework;
using LonelyPlanet.Model;

namespace LonelyPlanetTests
{
    [TestFixture]
    class EntityTests
    {
        [Test]
        public void Hit()
        {
            var entity = new Entity("test", new Coordinate(0, 0), 100.0, new Map());
            entity.Hit(10);
            Assert.Equals(10, entity.Health);
            Assert.IsFalse(entity.IsDead);
        }

        [Test]
        public void HitWithALotOfDamage()
        {
            var entity = new Entity("test", new Coordinate(0, 0), 100.0, new Map());
            entity.Hit(100500);
            Assert.Equals(0, entity.Health);
            Assert.IsTrue(entity.IsDead);
        }

        [Test]
        public void Heal()
        {
            var entity = new Entity("test", new Coordinate(0, 0), 100.0, new Map());
            entity.Heal(10);
            Assert.Equals(110, entity.Health);
            Assert.IsFalse(entity.IsDead);
        }

        [Test]
        public void HealDeadEntity()
        {
            var entity = new Entity("test", new Coordinate(0, 0), 100.0, new Map());
            entity.Hit(100500);
            entity.Heal(10);
            Assert.IsTrue(entity.IsDead);
        }

        [Test]
        public void Revive()
        {
            var entity = new Entity("test", new Coordinate(0, 0), 100.0, new Map());
            entity.Hit(110);
            entity.Heal(120);
            Assert.IsFalse(entity.IsDead);
        }
    }
}
