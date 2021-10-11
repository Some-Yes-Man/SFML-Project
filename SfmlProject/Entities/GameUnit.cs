using SfmlProject.Geometry;

namespace SfmlProject.Entities {
    public class GameUnit : GameEntity {
        public float DetectionRange { get; private set; }

        public GameUnit(Point position, float range) : base(position) {
            this.DetectionRange = range;
        }
    }
}
