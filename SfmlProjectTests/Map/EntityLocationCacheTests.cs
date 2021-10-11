using Microsoft.VisualStudio.TestTools.UnitTesting;
using SfmlProject.Entities;
using SfmlProject.Geometry;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace SfmlProject.Map.Tests {
    [TestClass()]
    public class EntityLocationCacheTests {
        [DataTestMethod]
        [DataRow(new int[] { 0, 10 }, DisplayName = "Zero Size")]
        [DataRow(new int[] { -1, 10 }, DisplayName = "Negative Size")]
        [DataRow(new int[] { 10, 0 }, DisplayName = "Zero Chunk")]
        [DataRow(new int[] { 10, -1 }, DisplayName = "Negative Chunk")]
        [DataRow(new int[] { 10, 3 }, DisplayName = "Chunk-Size-Mismatch")]
        public void EntityLocationCacheFailTest(int[] ints) {
            Assert.ThrowsException<EntityLocationCacheException>(() => new EntityLocationCache(ints[0], ints[1]));
        }

        [TestMethod()]
        public void EntityLocationCacheSuccessTest() {
            EntityLocationCache locationCache = new EntityLocationCache(512, 8);

            Assert.AreEqual(0, locationCache.Count);
        }

        [TestMethod()]
        public void AddEntityTest() {
            EntityLocationCache locationCache = new EntityLocationCache(512, 8);

            locationCache.AddEntity(new GameEntity(new Point(5, 5)));

            Assert.AreEqual(1, locationCache.Count);
        }

        [TestMethod()]
        public void UpdateEntityTest() {
            EntityLocationCache locationCache = new EntityLocationCache(512, 8);
            GameEntity entity = new GameEntity(new Point(5, 5));
            locationCache.AddEntity(entity);

            entity.Position.X = 6;
            entity.Position.Y = 7;
            locationCache.UpdateEntity(entity);

            Assert.AreEqual(entity.Position.X, locationCache.Entities.First().Position.X);
            Assert.AreEqual(entity.Position.Y, locationCache.Entities.First().Position.Y);
        }

        [TestMethod()]
        public void UpdateUnknownEntityTest() {
            EntityLocationCache locationCache = new EntityLocationCache(512, 8);
            GameEntity entity = new GameEntity(new Point(5, 5));

            Assert.ThrowsException<EntityLocationCacheException>(() => locationCache.UpdateEntity(entity));
        }

        [TestMethod()]
        public void FindEntitiesWithCircleTest() {
            EntityLocationCache locationCache = new EntityLocationCache(10, 1);
            HashSet<GameEntity> validEntities = new HashSet<GameEntity>(new[] {
                new GameEntity(new Point(5, 5)),
                new GameEntity(new Point(6, 6)),
            });
            HashSet<GameEntity> invalidEntities = new HashSet<GameEntity>(new[] {
                new GameEntity(new Point(7, 5)),
                new GameEntity(new Point(8, 6)),
            });
            foreach (GameEntity entity in validEntities) {
                locationCache.AddEntity(entity);
            }
            foreach (GameEntity entity in invalidEntities) {
                locationCache.AddEntity(entity);
            }

            HashSet<GameEntity> foundEntities = locationCache.FindAllEntities(new Vector2(4, 4), 3);

            Assert.AreEqual(validEntities.Count, foundEntities.Count);
            Assert.IsTrue(validEntities.All(x => foundEntities.Any(y => y.Equals(x))));
        }

        [TestMethod()]
        public void FindEntitiesWithRectangleTest() {
            EntityLocationCache locationCache = new EntityLocationCache(10, 1);
            HashSet<GameEntity> validEntities = new HashSet<GameEntity>(new[] {
                new GameEntity(new Point(5, 5)),
                new GameEntity(new Point(7, 5)),
            });
            HashSet<GameEntity> invalidEntities = new HashSet<GameEntity>(new[] {
                new GameEntity(new Point(6, 6)),
                new GameEntity(new Point(8, 6)),
            });
            foreach (GameEntity entity in validEntities) {
                locationCache.AddEntity(entity);
            }
            foreach (GameEntity entity in invalidEntities) {
                locationCache.AddEntity(entity);
            }

            HashSet<GameEntity> foundEntities = locationCache.FindAllEntities(new Rectangle(new Point(4, 4), new Point(7.5f, 5.5f)));

            Assert.AreEqual(validEntities.Count, foundEntities.Count);
            Assert.IsTrue(validEntities.All(x => foundEntities.Any(y => y.Equals(x))));
        }
    }
}
