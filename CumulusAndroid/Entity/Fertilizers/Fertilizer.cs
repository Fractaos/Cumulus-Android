using CumulusGame.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CumulusGame.Entity.Fertilizers
{
    public abstract class Fertilizer : GameEntity
    {
        #region Fields

        // Constants
        protected const float FertilizerScale = Utils.SCALE * 0.25f;

        // Processing Fields
        protected float _timeToEat, _amountOfAnger;
        protected bool _eated;

        #endregion

        #region Constructors

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="position">The position of the fertilizer</param>
        /// <param name="texture">The texture of the fertilizer</param>
        protected Fertilizer(Vector2 position, Texture2D texture)
        {
            this.texture = texture;
            this.position = new Vector2((position.X - (((float)this.texture.Width / 2) * FertilizerScale)), (position.Y - (((float)this.texture.Height / 2) * FertilizerScale)));

            // Fertilizer centered on position
            drawRectangle = new Rectangle((int)this.position.X, (int)this.position.Y,
                                            (int)(this.texture.Width * FertilizerScale), (int)(this.texture.Height * FertilizerScale));

            hitbox = new Rectangle((int)(position.X - (Utils.CELL_SIZE / 2)), (int)(position.Y - (Utils.CELL_SIZE / 2)), (int)Utils.CELL_SIZE, (int)Utils.CELL_SIZE);
            hitboxTex = Utils.CreateContouringRectangleTexture(hitbox.Width, hitbox.Height, Color.Red);

            cellOn = Utils._gameGrid.GetCellByCoord(position);
            cellOn.IsEmpty = false;
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
            cellOn.IsEmpty = true;
        }

        /// <summary>
        /// Check if the current fertilizer is in the game board
        /// </summary>
        /// <returns>True if it's in, false else</returns>
        public bool CheckIfFertilizerWithinTheGameBoard()
        {
            return (hitbox.Left >= 0 && hitbox.Right <= Utils.Window_Width
                    && hitbox.Top >= Utils.GameBoardOffset && hitbox.Bottom <= Utils.Window_Height - Utils.GameBoardOffset);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0f, Vector2.Zero, FertilizerScale, SpriteEffects.None, Utils.ENTITY_DEPTH);
            //spriteBatch.Draw(hitboxTex, hitbox, Color.White);
        }

        #endregion
    }
}