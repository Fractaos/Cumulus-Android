using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cumulus.Screens
{
    public abstract class Screen
    {
        protected SpriteBatch _spriteBatch;

        protected Screen()
        {
            _spriteBatch = Main.SpriteBatch;
        }

        public abstract void Create();

        public abstract void Update(GameTime time);

        public abstract void Draw();

        public void Dispose()
        {
            _spriteBatch.Dispose();
        }
    }
}