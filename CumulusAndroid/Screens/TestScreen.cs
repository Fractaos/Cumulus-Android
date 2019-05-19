using CumulusGame.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CumulusGame.Screens
{
    public class TestScreen : Screen
    {

        private Texture2D _background;

        public override void Create()
        {
            _background = Assets.MenuBg10;
        }

        public override void Update(GameTime time)
        {
        }

        public override void Draw()
        {
            _spriteBatch.Draw(_background, new Vector2(0, 0), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);

        }
    }
}