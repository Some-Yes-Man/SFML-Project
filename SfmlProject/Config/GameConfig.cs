using SFML.Window;

namespace SfmlProject.Config {
    public class GameConfig {
        public uint ResolutionX { get; set; }
        public uint ResolutionY { get; set; }
        public ContextSettings Settings { get; set; }
        public uint FrameLimit { get; set; }
    }
}
