using SFML.System;
using System.Collections.Generic;

namespace SFMLTest.Data {
    public class LevelGeometry {
        public List<Vector2f> Coordinates { get; private set; }

        public LevelGeometry() {
            this.Coordinates = new List<Vector2f>();
        }

        public LevelGeometry(params Vector2f[] points) : this() {
            for (int i = 0; i < points.Length; i++) {
                this.Coordinates.Add(points[i]);
            }
        }
    }
}
