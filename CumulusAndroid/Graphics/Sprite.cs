using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace CumulusGame.Graphics
{
    public class Sprite
    {
        private readonly Texture2D _texture;
        public float Scale { get; set; } = 1f;
        public Sprite(Texture2D texture)
        {
            _texture = texture;
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(_texture, position, null, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, 1f);
        }
    }
}