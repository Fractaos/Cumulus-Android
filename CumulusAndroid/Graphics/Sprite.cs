using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace CumulusGame.Graphics
{
    public class Sprite
    {
        private readonly Texture2D _texture;
        private readonly float _scale;


        public Sprite(Texture2D texture, float scale)
        {
            _texture = texture;
            _scale = scale;
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(_texture, position, null, Color.White, 0f, Vector2.Zero, _scale, SpriteEffects.None, 1f);
        }
    }
}