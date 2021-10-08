using SfmlProject.Geometry;

namespace SfmlProject.Entities {
    public class GameEntity {
        public Point Position { get; private set; }

        public GameEntity(Point position) {
            this.Position = position;
        }
    }
}
