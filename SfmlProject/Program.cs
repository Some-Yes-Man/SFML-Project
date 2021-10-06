using System;
using NLog;
using SFML.Audio;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Collections.Generic;
using SfmlProject.Graphic;
using SfmlProject.Geometry;

namespace SfmlProject {
    class Program {
        private static readonly Logger LOGGER = LogManager.GetLogger("SFMLTest");

        static void Main(string[] args) {
            LOGGER.Info("Hello World!");

            //JsonSerializer jsonSerializer = new JsonSerializer();
            //jsonSerializer.Formatting = Formatting.Indented;
            //using (StreamWriter sw = new StreamWriter("ExampleLevel.json")) {
            //    using (JsonWriter js = new JsonTextWriter(sw)) {
            //        jsonSerializer.Serialize(js, level);
            //    }
            //}

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
            sprite.Position = new Vector2f(400, 10);

            // WOOD :D
            Texture woodTexture = new Texture("Resources/wood.jpg");
            woodTexture.Repeated = true;
            woodTexture.Smooth = true;

            Clock fpsClock = new Clock();
            int fpsCount = 0;
            Clock frameClock = new Clock();

            List<IRenderable> stuff = new List<IRenderable>();
            stuff.Add(new Point(50, 300));
            stuff.Add(new Line(new Point(10, 10), new Point(10, 100)));
            stuff.Add(new Triangle(new Point(100, 100), new Point(200, 200), new Point(100, 180)));
            stuff.Add(new Polygon(new Point(400, 400), new Point(300, 400), new Point(500, 500), new Point(400, 300)));
            stuff.Add(new Circle(new Point(100, 300), 50));

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
                foreach (IRenderable renderItem in stuff) {
                    renderWindow.Draw(renderItem.Renderable);
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
