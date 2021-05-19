using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using LonelyPlanet.Model;

namespace Tests
{
    [TestFixture]
    public class EntityTest
    {
        private Map map;

        [SetUp]
        public void SetUp()
        {
            map = new Map();
        }

        [Test]
        public void Heal()
        {
            var entity = new Entity("Test", new Coordinate(0, 0), 100, map);
            var currentHealth = entity.Health;
            entity.Heal(10);
            Assert.AreEqual(currentHealth + 10, entity.Health);
        }

        [Test]
        public void Hit()
        {
            var entity = new Entity("Test", new Coordinate(0, 0), 100, map);
            var currentHealth = entity.Health;
            entity.Hit(5);
            Assert.AreEqual(currentHealth - 5, entity.Health);
        }

        [Test]
        public void DoMoreDamageThenHealth()
        {
            var entity = new Entity("Test", new Coordinate(0, 0), 100, map);
            var currentHealth = entity.Health;
            entity.Hit(currentHealth + 10);
            Assert.AreEqual(0, entity.Health);
            Assert.IsTrue(entity.IsDead);
        }

        [Test]
        public void HealWhenDead()
        {
            var entity = new Entity("Test", new Coordinate(0, 0), 100, map);
            var currentHealth = entity.Health;
            entity.Hit(currentHealth);
            entity.Heal(10);
            Assert.AreEqual(10, entity.Health);
            Assert.IsFalse(entity.IsDead);
        }

        [Test]
        public void HealEventInvoke()
        {
            var entity = new Entity("Test", new Coordinate(0, 0), 100, map);
            entity.HealthChanged += (e) => Assert.Pass();
            entity.Heal(1);
        }

        [Test]
        public void HitEventInvoke()
        {
            var entity = new Entity("Test", new Coordinate(0, 0), 100, map);
            entity.HealthChanged += (e) => Assert.Pass();
            entity.Hit(1);
        }
    }
}
