using CumulusAndroid.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CumulusAndroid.Graphics
{
    public class Button
    {
        #region Fields

        // Graphics fields
        public string Title { get; set; }

        private readonly Vector2 _position;

        private Rectangle _hitbox;

        private readonly Texture2D _background;
        private Texture2D _drawedHitbox;

        private readonly SpriteFont _font = Assets.Pixel30;

        private readonly float _buttonScale = 1f;

        // Processing fields
        private bool _hovered;

        #endregion

        #region Events

        public event EventHandler Click;

        #endregion

        #region Constructors

        /// <summary>
        /// Basic constructor with position & title
        /// </summary>
        /// <param name="position">The position of the button</param>
        /// <param name="title">The text within the button</param>
        public Button(Vector2 position, string title) : this(position)
        {
            Title = title;
            _background = Assets.Button;
            _hitbox = new Rectangle((int)position.X, (int)position.Y, (int)(_background.Width * _buttonScale), (int)(_background.Height * _buttonScale));
            _drawedHitbox = Utils.CreateContouringRectangleTexture(_hitbox.Width, _hitbox.Height, Color.Red);
        }

        public Button(Vector2 position, string title, float scale) : this(position, title)
        {
            _buttonScale = scale;
            _hitbox = new Rectangle((int)position.X, (int)position.Y, (int)(_background.Width * _buttonScale), (int)(_background.Height * _buttonScale));
            _drawedHitbox = Utils.CreateContouringRectangleTexture(_hitbox.Width, _hitbox.Height, Color.Red);
        }

        public Button(Vector2 position, string title, float scale, SpriteFont font) : this(position, title, scale)
        {
            _font = font;
        }

        public Button(Vector2 position)
        {
            _position = position;
            _background = Assets.ButtonSettings;
            _hitbox = new Rectangle((int)position.X, (int)position.Y, (int)(_background.Width * _buttonScale), (int)(_background.Height * _buttonScale));
            _drawedHitbox = Utils.CreateContouringRectangleTexture(_hitbox.Width, _hitbox.Height, Color.Red);
        }

        public Button(Vector2 position, Texture2D buttonTexture)
        {
            _position = position;
            _background = buttonTexture;
            _hitbox = new Rectangle((int)position.X, (int)position.Y, (int)(_background.Width * _buttonScale), (int)(_background.Height * _buttonScale));
            _drawedHitbox = Utils.CreateContouringRectangleTexture(_hitbox.Width, _hitbox.Height, Color.Red);
        }

        public Button(Vector2 position, Texture2D buttonTexture, float scale) : this(position, buttonTexture)
        {
            _buttonScale = scale;
            _hitbox = new Rectangle((int)position.X, (int)position.Y, (int)(_background.Width * _buttonScale), (int)(_background.Height * _buttonScale));
            _drawedHitbox = Utils.CreateContouringRectangleTexture(_hitbox.Width, _hitbox.Height, Color.Red);
        }

        public Button(Vector2 position, float scale) : this(position)
        {
            _buttonScale = scale;
            _hitbox = new Rectangle((int)position.X, (int)position.Y, (int)(_background.Width * _buttonScale), (int)(_background.Height * _buttonScale));
            _drawedHitbox = Utils.CreateContouringRectangleTexture(_hitbox.Width, _hitbox.Height, Color.Red);
        }

        #endregion

        #region Public Methods

        public void Update(GameTime gameTime)
        {
            _hovered = _hitbox.Contains(Input.FirstTouchPosition);

            if (Input.OneTouched() && _hovered)
            {
                Click?.Invoke(this, new EventArgs());
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Draw(spriteBatch, 1f);
        }

        public void Draw(SpriteBatch spriteBatch, float opacity)
        {
            if (_hovered)
            {
                spriteBatch.Draw(_background, _position, null, Color.Yellow * opacity, 0f, Vector2.Zero, _buttonScale, SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.Draw(_background, _position, null, Color.White * opacity, 0f, Vector2.Zero, _buttonScale, SpriteEffects.None, 0f);
            }
            //spriteBatch.Draw(_drawedHitbox, _position, Color.White * opacity);
            if (Title != null)
            {
                spriteBatch.DrawString(_font, Title, new Vector2(_hitbox.Center.X - (_font.MeasureString(Title).X / 2),
                        _hitbox.Center.Y - (_font.MeasureString(Title).Y / 2)),
                    Color.Black * opacity, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
        }

        #endregion
    }
}
