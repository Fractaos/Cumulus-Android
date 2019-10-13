using CumulusGame.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CumulusGame.Entity.Fertilizers
{
    public abstract class Fertilizer : GameEntity
    {
        #region Fields

        // Constants
        private const float FERTILIZER_SCALE = 0.25f;

        // Processing Fields
        protected float _timeToEat, _amountOfAnger;

        #endregion

        #region Properties
        public float TimeToEat => _timeToEat;

        public float AmountOfAnger => _amountOfAnger;

        public bool Eated { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="position">The position of the fertilizer</param>
        /// <param name="texture">The texture of the fertilizer</param>
        protected Fertilizer(Vector2 position, Texture2D texture)
        {
            _texture = texture;
            _position = new Vector2((position.X - (((float)_texture.Width / 2) * FERTILIZER_SCALE)),
                (position.Y - (((float)_texture.Height / 2) * FERTILIZER_SCALE)));

            // Fertilizer centered on position
            _drawRectangle = new Rectangle((int)_position.X, (int)_position.Y,
                                            (int)(_texture.Width * FERTILIZER_SCALE), (int)(_texture.Height * FERTILIZER_SCALE));

            _hitbox = new Rectangle((int)(position.X - (Utils.CELL_SIZE / 2)), (int)(position.Y - (Utils.CELL_SIZE / 2)),
                (int)Utils.CELL_SIZE, (int)Utils.CELL_SIZE);
            _hitboxTex = Utils.CreateContouringRectangleTexture(_hitbox.Width, _hitbox.Height, Color.Red);

            _cellOn = Utils.GameGrid.GetCellByCoord(position);
            _cellOn.IsEmpty = false;
        }

        #endregion

        #region Public Methods

        public void Destroy()
        {
            Eated = true;
            _cellOn.IsEmpty = true;
        }

        public override void Update(GameTime gameTime)
        {

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, null, Color.White, 0f, Vector2.Zero,
                FERTILIZER_SCALE, SpriteEffects.None, Utils.ENTITY_DEPTH);
#if DEBUG
            spriteBatch.Draw(_hitboxTex, _hitbox, Color.White);
#endif
        }

        #endregion
    }
}