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
        protected Texture2D _texture, _hitboxTex;
        protected Rectangle _drawRectangle;
        protected Vector2 _position;

        // Processing Fields
        protected Rectangle _hitbox;
        protected Cell _cellOn;

        #endregion

        #region Properties

        protected Vector2 Position => _position;

        public Rectangle Hitbox => _hitbox;

        public Cell Cell => _cellOn;

        #endregion

        #region Public Methods

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);

        public int CompareTo(GameEntity other)
        {
            return other == null ? 1 : _cellOn.Row.CompareTo(other._cellOn.Row);
        }

        #endregion
    }
}
