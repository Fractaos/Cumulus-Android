using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CumulusGame.Graphics
{
    public class Background
    {
        #region Fields

        // Graphics fields
        private Texture2D _texture, _hitboxTex;
        private Vector2 _position;
        private Rectangle _hitbox;

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
            _hitbox = new Rectangle((int)_position.X, (int)(Utils.GameBoardOffset * Utils.SCALE), (int)(_texture.Width * Utils.SCALE),
                (int)(_texture.Height * Utils.SCALE) - Utils.GameBoardOffset);
            _hitboxTex = Utils.CreateContouringRectangleTexture(_hitbox.Width, _hitbox.Height, Color.Red);
        }

        #endregion

        #region Public Methods

        public void Update(GameTime gameTime)
        { }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, null, Color.White, 0f, Vector2.Zero, Utils.SCALE, SpriteEffects.None, Utils.BACKGROUND_DEPTH);
            //spriteBatch.Draw(_hitboxTex, _hitbox, Color.White);
        }

        #endregion
    }
}
