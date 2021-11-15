using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace chips {
    public class GameRoot : Game {
        public static readonly Vector2 ScreenSize = new Vector2(1366, 768);
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public GameRoot() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize() {
            graphics.PreferredBackBufferWidth = (int)ScreenSize.X;
            graphics.PreferredBackBufferHeight = (int)ScreenSize.Y;
            graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Art.Load(Content);

            GateManager.Add(new Gate(new Vector2(200, 100), "one"));
            GateManager.Add(new Gate(new Vector2(500, 100), "and"));
            GateManager.Add(new Gate(new Vector2(500, 100), "led"));
            GateManager.Add(new Wire(new Vector2(300, 300), new Vector2(400, 450)));
        }

        protected override void Update(GameTime gameTime) {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            Input.Update();
            GateManager.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();
            GateManager.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
