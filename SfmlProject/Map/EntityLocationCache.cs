using SfmlProject.Entities;
using SfmlProject.Geometry;
using System.Collections.Generic;

namespace SfmlProject.Map {
    public class EntityLocationCache {
        private const int INITIAL_CHUNK_LOOKUP_SIZE = 1000;

        private readonly int dimension;
        private readonly int chunkCount;
        private readonly int chunkSize;
        private readonly Dictionary<GameEntity, EntityLocationChunk> chunkLookup = new Dictionary<GameEntity, EntityLocationChunk>(INITIAL_CHUNK_LOOKUP_SIZE);
        private readonly EntityLocationChunk[,] chunkArray;

        public int Count {
            get {
                return this.chunkLookup.Count;
            }
        }
        public IEnumerable<GameEntity> Entities {
            get {
                return chunkLookup.Keys;
            }
        }

        public EntityLocationCache(int dimension, int chunkCount = 1) {
            this.chunkCount = chunkCount;
            if (this.chunkCount < 1) {
                throw new EntityLocationCacheException("Cannot construct location tree with less than one chunk.");
            }

            this.dimension = dimension;
            if (this.dimension <= 0) {
                throw new EntityLocationCacheException("Map size can't be smaller than 1.");
            }
            if (this.dimension % this.chunkCount != 0) {
                throw new EntityLocationCacheException("Map size must be a multiple of the chunk count.");
            }

            this.chunkSize = this.dimension / this.chunkCount;
            this.chunkArray = new EntityLocationChunk[this.chunkCount, this.chunkCount];

            for (int y = 0; y < this.chunkCount; y++) {
                for (int x = 0; x < this.chunkCount; x++) {
                    this.chunkArray[x, y] = new EntityLocationChunk();
                }
            }
        }

        public void AddEntity(GameEntity entity) {
            EntityLocationChunk chunk = this.chunkArray[(int)entity.Position.X / this.chunkSize, (int)entity.Position.Y / this.chunkSize];
            chunk.Add(entity);
            this.chunkLookup.Add(entity, chunk);
        }

        public void UpdateEntity(GameEntity entity) {
            EntityLocationChunk oldChunk = this.chunkLookup.GetValueOrDefault(entity);
            if (oldChunk == null) {
                throw new EntityLocationCacheException(string.Format("Given entity ({0}) not present in current location map.", entity));
            }
            oldChunk.Remove(entity);
            EntityLocationChunk newChunk = this.chunkArray[(int)entity.Position.X / this.chunkSize, (int)entity.Position.Y / this.chunkSize];
            newChunk.Add(entity);
            this.chunkLookup[entity] = newChunk;
        }

        public void RemoveEntity(GameEntity entity) {
            EntityLocationChunk oldChunk = this.chunkLookup.GetValueOrDefault(entity);
            if (oldChunk == null) {
                throw new EntityLocationCacheException(string.Format("Given entity ({0}) not present in current location map.", entity));
            }
            oldChunk.Remove(entity);
            this.chunkLookup.Remove(entity);
        }

        // Predicate<GameEntity> predicate
        public HashSet<GameEntity> FindEntities(Rectangle rectangle) {
            int minX = (int)rectangle.UpperLeft.X / chunkSize;
            int minY = (int)rectangle.UpperLeft.Y / chunkSize;
            int maxX = (int)rectangle.LowerRight.X / chunkSize;
            int maxY = (int)rectangle.LowerRight.Y / chunkSize;

            HashSet<GameEntity> entitiesInRange = new HashSet<GameEntity>();
            for (int y = minY; y <= maxY; y++) {
                for (int x = minX; x <= maxX; x++) {
                    bool completelyContained = (y > minY) && (y < maxY) && (x > minX) && (x < maxX);
                    foreach (GameEntity entity in this.chunkArray[x, y]) {
                        if (completelyContained || ((entity.Position.X >= rectangle.UpperLeft.X) && (entity.Position.X <= rectangle.LowerRight.X) && (entity.Position.Y >= rectangle.UpperLeft.Y) && (entity.Position.Y <= rectangle.LowerRight.Y))) {
                            entitiesInRange.Add(entity);
                        }
                    }
                }
            }
            return entitiesInRange;
        }

        // Predicate<GameEntity> predicate
        public HashSet<GameEntity> FindEntities(Circle circle) {
            int minX = (int)(circle.Center.X - circle.Radius) / chunkSize;
            int minY = (int)(circle.Center.Y - circle.Radius) / chunkSize;
            int maxX = (int)(circle.Center.X + circle.Radius) / chunkSize;
            int maxY = (int)(circle.Center.Y + circle.Radius) / chunkSize;

            HashSet<GameEntity> entitiesInRange = new HashSet<GameEntity>();
            for (int y = minY; y <= maxY; y++) {
                for (int x = minX; x <= maxX; x++) {
                    foreach (GameEntity entity in this.chunkArray[x, y]) {
                        float xDiff = entity.Position.X - circle.Center.X;
                        float yDiff = entity.Position.Y - circle.Center.Y;
                        if (xDiff * xDiff + yDiff * yDiff <= circle.Radius * circle.Radius) {
                            entitiesInRange.Add(entity);
                        }
                    }
                }
            }
            return entitiesInRange;
        }
    }
}
