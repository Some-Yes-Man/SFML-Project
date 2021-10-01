using System;
using NLog;
using SFML.Audio;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using SFMLTest.Data;
using Newtonsoft.Json;
using System.IO;

namespace SFMLTest {
    class Program {
        private static readonly Logger LOGGER = LogManager.GetLogger("SFMLTest");

        static void Main(string[] args) {
            LOGGER.Info("Hello World!");

            Level level = new Level("ExampleLevel");
            level.AddGeometry(new LevelGeometry(new Vector2f(100, 150), new Vector2f(120, 50), new Vector2f(200, 80), new Vector2f(140, 210)));
            level.AddGeometry(new LevelGeometry(new Vector2f(100, 200), new Vector2f(120, 250), new Vector2f(60, 300)));
            level.AddGeometry(new LevelGeometry(new Vector2f(200, 260), new Vector2f(220, 150), new Vector2f(300, 200), new Vector2f(350, 320)));
            level.AddGeometry(new LevelGeometry(new Vector2f(340, 60), new Vector2f(360, 40), new Vector2f(370, 70)));
            level.AddGeometry(new LevelGeometry(new Vector2f(450, 190), new Vector2f(560, 170), new Vector2f(540, 270), new Vector2f(430, 290)));
            level.AddGeometry(new LevelGeometry(new Vector2f(400, 95), new Vector2f(580, 50), new Vector2f(480, 150)));
            level.AddGeometry(new LevelGeometry(new Vector2f(660, 250), new Vector2f(720, 170), new Vector2f(950, 270), new Vector2f(900, 290)));
            level.AddGeometry(new LevelGeometry(new Vector2f(695, 40), new Vector2f(780, 120), new Vector2f(680, 140), new Vector2f(930, 190)));
            level.AddGeometry(new LevelGeometry(new Vector2f(600, 325), new Vector2f(750, 425), new Vector2f(900, 400), new Vector2f(850, 525), new Vector2f(900, 700)));
            // problems ^^
            level.AddGeometry(new LevelGeometry(new Vector2f(100, 500), new Vector2f(100, 600), new Vector2f(200, 500)));
            level.AddGeometry(new LevelGeometry(new Vector2f(100, 650), new Vector2f(100, 750), new Vector2f(200, 750)));
            level.AddGeometry(new LevelGeometry(new Vector2f(250, 500), new Vector2f(250, 750), new Vector2f(300, 400)));
            level.AddGeometry(new LevelGeometry(new Vector2f(500, 500), new Vector2f(600, 550), new Vector2f(600, 600), new Vector2f(500, 700)));
            // concav shape
            level.AddGeometry(new LevelGeometry(new Vector2f(600, 700), new Vector2f(800, 700), new Vector2f(750, 690), new Vector2f(700, 500), new Vector2f(650, 690)));

            JsonSerializer jsonSerializer = new JsonSerializer();
            jsonSerializer.Formatting = Formatting.Indented;
            using (StreamWriter sw = new StreamWriter("ExampleLevel.json")) {
                using (JsonWriter js = new JsonTextWriter(sw)) {
                    jsonSerializer.Serialize(js, level);
                }
            }

            // create & configure window
            ContextSettings settings = new ContextSettings();
            settings.AntialiasingLevel = 0;
            RenderWindow renderWindow = new RenderWindow(new VideoMode(1000, 800), "SFML-Test", Styles.Close, settings);
            // do NOT mix frame limit and vertical sync
            renderWindow.SetFramerateLimit(60);
            // window events
            renderWindow.Closed += RenderWindow_Closed;
            renderWindow.GainedFocus += RenderWindow_GainedFocus;
            renderWindow.LostFocus += RenderWindow_LostFocus;
            renderWindow.MouseEntered += RenderWindow_MouseEntered;
            renderWindow.MouseLeft += RenderWindow_MouseLeft;
            renderWindow.MouseMoved += RenderWindow_MouseMoved;
            // inputs
            renderWindow.KeyPressed += RenderWindow_KeyPressed;
            renderWindow.KeyReleased += RenderWindow_KeyReleased;
            renderWindow.MouseButtonPressed += RenderWindow_MouseButtonPressed;
            renderWindow.MouseButtonReleased += RenderWindow_MouseButtonReleased;
            renderWindow.MouseWheelScrolled += RenderWindow_MouseWheelScrolled;
            renderWindow.Resized += RenderWindow_Resized;

            Music music = new Music("Resources/HausAmSeeSnippet.ogg");
            music.Volume = 10;
            music.Play();

            Font font = new Font("Resources/KatetheGreat.ttf");
            Text fpsText = new Text("", font, 50);
            fpsText.LetterSpacing = 1.5f;
            fpsText.FillColor = Color.Magenta;
            Text frameText = new Text("", font, 50);
            frameText.LetterSpacing = 1.5f;
            frameText.FillColor = Color.Magenta;

            // textures are stored in video memory (fast to draw); images are stored in system memory (fast to modify)
            // use as FEW textures as possible; they are expensive; cut sprites from bigger textures instead
            Image image = new Image("Resources/AmongUs.png");
            image.FlipHorizontally();
            Texture texture = new Texture(image);
            texture.Smooth = true;
            texture.Repeated = false;
            //Texture texture = new Texture("Resources/AmongUs.png");
            Sprite sprite = new Sprite(texture);
            sprite.Color = Color.Yellow;
            sprite.Scale = new Vector2f(0.5f, 0.5f);
            sprite.Position = new Vector2f(10, 10);

            // WOOD :D
            Texture woodTexture = new Texture("Resources/wood.jpg");
            woodTexture.Repeated = true;
            woodTexture.Smooth = true;

            Clock fpsClock = new Clock();
            int fpsCount = 0;
            Clock frameClock = new Clock();

            while (renderWindow.IsOpen && !Keyboard.IsKeyPressed(Keyboard.Key.Escape)) {
                // important! trigger event processing; must be called in window thread
                renderWindow.DispatchEvents();

                // rendering (if done in another thread, deactivate renderWindow in this thread and activate it in the other first)
                renderWindow.Clear();
                // FPS
                if (fpsClock.ElapsedTime.AsMilliseconds() >= 1000) {
                    fpsText.DisplayedString = fpsCount + "fps";
                    fpsText.Position = new Vector2f(renderWindow.Size.X - fpsText.GetLocalBounds().Width - 5, 0);
                    fpsCount = 0;
                    fpsClock.Restart();
                }
                renderWindow.Draw(fpsText);

                /**
                 * SCENE
                 */
                // shapes
                foreach (LevelGeometry geometry in level.Geometry) {
                    // convex is VERY relaxed; as long as the shape can be constructed from a triangle fan, you're fine (center of gravity)
                    ConvexShape shape = new ConvexShape((uint)geometry.Coordinates.Count);
                    for (int i = 0; i < geometry.Coordinates.Count; i++) {
                        shape.SetPoint((uint)i, geometry.Coordinates[i]);
                    }
                    renderWindow.Draw(shape);
                }
                // sprite
                renderWindow.Draw(sprite);
                // textured shape
                CircleShape circle = new CircleShape(50.0f);
                circle.Position = new Vector2f(300, 300);
                circle.Texture = woodTexture;
                circle.TextureRect = new IntRect(0, 0, 100, 100);
                renderWindow.Draw(circle);

                // timing
                frameText.DisplayedString = frameClock.Restart().AsMilliseconds() + "ms";
                frameText.Position = new Vector2f(renderWindow.Size.X - frameText.GetLocalBounds().Width - 5, 50);
                renderWindow.Draw(frameText);

                // switch buffers
                renderWindow.Display();
                fpsCount++;
            }
        }

        // input events

        private static void RenderWindow_KeyPressed(object sender, KeyEventArgs e) {
            LOGGER.Debug("KeyPressed: " + e.ToString());
        }

        private static void RenderWindow_KeyReleased(object sender, KeyEventArgs e) {
            LOGGER.Debug("KeyReleased: " + e.ToString());
        }

        private static void RenderWindow_MouseButtonPressed(object sender, MouseButtonEventArgs e) {
            LOGGER.Debug("MousePressed: " + e.ToString());
        }

        private static void RenderWindow_MouseButtonReleased(object sender, MouseButtonEventArgs e) {
            LOGGER.Debug("MouseReleased: " + e.ToString());
        }

        private static void RenderWindow_MouseWheelScrolled(object sender, MouseWheelScrollEventArgs e) {
            LOGGER.Debug("MouseWheel: " + e.ToString());
        }

        private static void RenderWindow_MouseMoved(object sender, MouseMoveEventArgs e) {
            LOGGER.Debug("MouseMoved: " + e.X + ":" + e.Y);
        }

        // window events

        private static void RenderWindow_Closed(object sender, EventArgs e) {
            LOGGER.Debug("Closed");
        }

        private static void RenderWindow_GainedFocus(object sender, EventArgs e) {
            LOGGER.Debug("GainedFocus");
        }

        private static void RenderWindow_LostFocus(object sender, EventArgs e) {
            LOGGER.Debug("LostFocus");
        }

        private static void RenderWindow_MouseEntered(object sender, EventArgs e) {
            LOGGER.Debug("MouseEntered");
        }

        private static void RenderWindow_MouseLeft(object sender, EventArgs e) {
            LOGGER.Debug("MouseLeft");
        }

        private static void RenderWindow_Resized(object sender, SizeEventArgs e) {
            LOGGER.Debug("WindowResized");
        }

    }

}
