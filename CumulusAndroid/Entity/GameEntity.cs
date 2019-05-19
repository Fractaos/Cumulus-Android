using CumulusGame.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CumulusGame.Entity
{
    public abstract class GameEntity : IComparable<GameEntity>
    {
        #region Fields

        // Graphics Fields
        protected Texture2D texture, hitboxTex;
        protected Rectangle drawRectangle;
        protected Vector2 position;

        // Processing Fields
        protected Rectangle hitbox;
        protected Cell cellOn;

        #endregion

        #region Properties

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Rectangle Hitbox
        {
            get { return hitbox; }
        }

        public Cell Cell
        {
            get { return cellOn; }
        }

        #endregion

        #region Public Methods

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);

        public int CompareTo(GameEntity other)
        {
            if (other == null)
            {
                return 1;
            }
            else
            {
                return cellOn.Row.CompareTo(other.cellOn.Row);
            }
        }

        #endregion
    }
}
