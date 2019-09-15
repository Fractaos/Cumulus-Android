using CumulusGame.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CumulusGame.Graphics
{
    public class Background
    {
        #region Fields

        // Graphics fields
        private readonly Texture2D _texture;
        private readonly Vector2 _position;
#if DEBUG
        private readonly Texture2D _hitboxTex;
        private readonly Rectangle _hitbox;
#endif

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="texture">The texture of the background</param>
        /// <param name="position">The position of the right left corner of the background</param>
        public Background(Texture2D texture, Vector2 position)
        {
            _texture = texture;
            _position = position;
        }

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="texture">The texture of the background</param>
        public Background(Texture2D texture)
        {
            _texture = texture;
            _position = new Vector2(0, 0);
        }

        /// <summary>
        /// The constructor
        /// </summary>
        public Background()
        {
            _texture = Assets.Background;
            _position = new Vector2(0, 0);
#if DEBUG
            _hitbox = new Rectangle((int)_position.X, Utils.GAMEBOARD_OFFSET, _texture.Width,
                _texture.Height - Utils.GAMEBOARD_OFFSET);
            _hitboxTex = Utils.CreateContouringRectangleTexture(_hitbox.Width, _hitbox.Height, Color.Red);
#endif
        }

        #endregion

        #region Public Methods

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, Utils.BACKGROUND_DEPTH);
#if DEBUG
            spriteBatch.Draw(_hitboxTex, _hitbox, Color.White);
#endif
        }

        #endregion
    }
}
