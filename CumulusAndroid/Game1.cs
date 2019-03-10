using Microsoft.Xna.Framework;

namespace CumulusAndroid
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private Main _main;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            _main = new Main(_graphics, this);
            _main.Initialize();
            base.Initialize();
        }

        protected override void LoadContent()
        {
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            _main.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _main.Draw();
            base.Draw(gameTime);
        }
    }
}
