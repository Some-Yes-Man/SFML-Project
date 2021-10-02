using System.Collections.Generic;

namespace SfmlProject.Data {
    public class Level {
        public string Name { get; private set; }
        public List<LevelGeometry> Geometry { get; private set; }

        public Level(string name) {
            this.Name = name;
            this.Geometry = new List<LevelGeometry>();
        }

        public void AddGeometry(LevelGeometry geometry) {
            this.Geometry.Add(geometry);
        }
    }
}
