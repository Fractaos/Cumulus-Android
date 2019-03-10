using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CumulusAndroid.Screens
{
    public abstract class Screen
    {
        protected SpriteBatch _spriteBatch;

        protected Screen()
        {
            _spriteBatch = new SpriteBatch(Main.Device);
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