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
        private bool _eated;

        #endregion

        #region Constructors

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="position">The position of the fertilizer</param>
        /// <param name="texture">The texture of the fertilizer</param>
        protected Fertilizer(Vector2 position, Texture2D texture)
        {
            this._texture = texture;
            this._position = new Vector2((position.X - (((float)this._texture.Width / 2) * FERTILIZER_SCALE)),
                (position.Y - (((float)this._texture.Height / 2) * FERTILIZER_SCALE)));

            // Fertilizer centered on position
            _drawRectangle = new Rectangle((int)this._position.X, (int)this._position.Y,
                                            (int)(this._texture.Width * FERTILIZER_SCALE), (int)(this._texture.Height * FERTILIZER_SCALE));

            _hitbox = new Rectangle((int)(position.X - (Utils.CELL_SIZE / 2)), (int)(position.Y - (Utils.CELL_SIZE / 2)),
                (int)Utils.CELL_SIZE, (int)Utils.CELL_SIZE);
            _hitboxTex = Utils.CreateContouringRectangleTexture(_hitbox.Width, _hitbox.Height, Color.Red);

            _cellOn = Utils.GameGrid.GetCellByCoord(position);
            _cellOn.IsEmpty = false;
        }

        #endregion

        #region Properties

        public float TimeToEat => _timeToEat;

        public float AmountOfAnger => _amountOfAnger;

        public bool Eated
        {
            get => _eated;
            set => _eated = value;
        }

        #endregion

        #region Public Methods

        public void Destroy()
        {
            _eated = true;
            _cellOn.IsEmpty = true;
        }

        /// <summary>
        /// Check if the current fertilizer is in the game board
        /// </summary>
        /// <returns>True if it's in, false else</returns>
        public bool CheckIfFertilizerWithinTheGameBoard()
        {
            return (_hitbox.Left >= 0 && _hitbox.Right <= Utils.SCREEN_WIDTH
                    && _hitbox.Top >= Utils.GAMEBOARD_OFFSET && _hitbox.Bottom <= Utils.SCREEN_HEIGHT - Utils.GAMEBOARD_OFFSET);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, null, Color.White, 0f, Vector2.Zero,
                FERTILIZER_SCALE, SpriteEffects.None, Utils.ENTITY_DEPTH);
            //spriteBatch.Draw(hitboxTex, hitbox, Color.White);
        }

        #endregion
    }
}