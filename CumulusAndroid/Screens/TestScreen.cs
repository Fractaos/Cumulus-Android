using CumulusAndroid.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CumulusAndroid.Screens
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
            _spriteBatch.Begin();
            {
                _spriteBatch.Draw(_background, new Vector2(Main.OriginalScreenWidth / 2f, 0), null, Color.White, 0f, Vector2.Zero, Main.Scale, SpriteEffects.None, 0);
            }
            _spriteBatch.End();
        }
    }
}