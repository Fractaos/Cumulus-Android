using CumulusGame.Graphics;
using Microsoft.Xna.Framework;

namespace CumulusGame.Screens
{
    public class TestScreen : Screen
    {

        private Sprite _testSprite;

        public override void Create()
        {
            _testSprite = new Sprite(Assets.LittleFertilizer, 1f);
        }

        public override void Update(GameTime time)
        {
        }

        public override void Draw()
        {
            _testSprite.Draw(_spriteBatch, new Vector2(150));

        }
    }
}