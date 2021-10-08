using SfmlProject.Geometry.Base;
using System.Collections.Generic;

namespace SfmlProject.Map {
    public class NavigationLayer {

        private int size;

        public NavigationLayer(int size) {
            this.size = size;
        }

        public void Initialize(IEnumerable<Shape> obstacles) {

        }
    }
}
