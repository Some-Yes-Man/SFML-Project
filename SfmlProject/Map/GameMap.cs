using SfmlProject.Geometry.Base;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SfmlProject.Map {
    public class GameMap {
        public string Name { get; private set; }
        public int Size { get; private set; }
        public SFML.Graphics.Image Background { get; private set; }
        public HashSet<Shape> Obstacles { get; private set; }

        [XmlIgnore]
        public NavigationLayer NavigationLayer { get; private set; }
        [XmlIgnore]
        public EntityLocationCache EntityLocationTree { get; private set; }

        private GameMap() {
            this.Obstacles = new HashSet<Shape>();
        }

        public GameMap(string name, int size) : this() {
            this.Name = name;
            // check map size (positive & power of 2)
            if (size < 1) {
                throw new ArgumentException("Map size cannot be smaller than 1.");
            }
            int check = size;
            int power = 0;
            while (check > 1) {
                if (check % 2 != 0) {
                    throw new ArgumentException("Map size must be 'power of 2'.");
                }
                power++;
                check = size / 2;
            }
            this.Size = size;
            this.EntityLocationTree = new EntityLocationCache(size, (this.Size > 8) ? this.Size / 8 : 1);
            this.NavigationLayer = new NavigationLayer(size);
        }

        public void Initialize() {
            this.NavigationLayer.Initialize(this.Obstacles);
        }
    }
}
