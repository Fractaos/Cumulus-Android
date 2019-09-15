using CumulusGame.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CumulusGame.Graphics
{
    public enum TypeOfSheet
    {
        Horizontal,
        Vertical
    }

    public class Animation
    {
        #region Fields

        private readonly float _animationDuration, _animationScale;
        private float _timePassed;

        private readonly int _nbFrames;
        private int _currentFrame = 1;
        private readonly int _frameWidth, _frameHeight;

        private readonly TypeOfSheet _typeOfSheet;

        private readonly Vector2 _position;

        public bool AnimationEnded { get; private set; }
        private readonly bool _loop;

        private readonly Texture2D _animatingSheet;
        private Texture2D _hitbox;

        private Rectangle _drawRectangle;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="position">The position</param>
        /// <param name="animationDuration">The duration of the animation</param>
        /// <param name="nbFrames">The number of frames of the animations</param>
        /// <param name="animatingSheet">The texture with the differents frames</param>
        /// <param name="typeOfSheet">The sens of reading the texture (Horizontally or Vertically)</param>
        /// <param name="loop">If the animation loop when played</param>
        /// <param name="scale">The scaling of the sprite to draw</param>
        public Animation(Vector2 position, float animationDuration, int nbFrames, Texture2D animatingSheet, TypeOfSheet typeOfSheet, bool loop, float scale)
        {
            _animationDuration = animationDuration;
            _nbFrames = nbFrames;
            _animatingSheet = animatingSheet;
            _typeOfSheet = typeOfSheet;
            _position = position;
            _animationScale = scale;
            _loop = loop;
            switch (typeOfSheet)
            {
                case TypeOfSheet.Horizontal:
                    _frameWidth = animatingSheet.Width / nbFrames;
                    _frameHeight = animatingSheet.Height;
                    _drawRectangle = new Rectangle(_frameWidth * (_currentFrame - 1), 0, _frameWidth, _frameHeight);
                    break;
                case TypeOfSheet.Vertical:
                    _frameWidth = animatingSheet.Width;
                    _frameHeight = animatingSheet.Height / nbFrames;
                    _drawRectangle = new Rectangle(0, _frameHeight * (_currentFrame - 1), _frameWidth, _frameHeight);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(typeOfSheet), typeOfSheet, null);
            }
            _hitbox = Utils.CreateContouringRectangleTexture((int)(_drawRectangle.Width * _animationScale), (int)(_drawRectangle.Height * _animationScale), Color.Red);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Play the animation
        /// </summary>
        /// <param name="elapsedGameTimeInMillis">The game time elapsed since last update in milliseconds</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void Play(float elapsedGameTimeInMillis)
        {
            var timePerFrame = _animationDuration / _nbFrames;
            _timePassed += elapsedGameTimeInMillis;
            if (_timePassed > timePerFrame)
            {
                _timePassed = 0;
                _currentFrame++;
                if (_currentFrame > _nbFrames && _loop)
                {
                    _currentFrame = 1;
                }
                else if (_currentFrame > _nbFrames)
                {
                    _currentFrame = _nbFrames;
                    AnimationEnded = true;
                }
            }
            else
            {
                switch (_typeOfSheet)
                {
                    case TypeOfSheet.Horizontal:
                        _drawRectangle.X = _frameWidth * (_currentFrame - 1);
                        break;
                    case TypeOfSheet.Vertical:
                        _drawRectangle.Y = _frameHeight * (_currentFrame - 1);
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// Reset the animation cycle
        /// </summary>
        public void ResetAnimation()
        {
            _currentFrame = 1;
            AnimationEnded = false;
            switch (_typeOfSheet)
            {
                case TypeOfSheet.Horizontal:
                    _drawRectangle.X = _frameWidth * (_currentFrame - 1);
                    break;
                case TypeOfSheet.Vertical:
                    _drawRectangle.Y = _frameHeight * (_currentFrame - 1);
                    break;
                default:
                    throw new IndexOutOfRangeException();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_animatingSheet, _position, _drawRectangle,
                            Color.White, 0f, Vector2.Zero, _animationScale, SpriteEffects.None, 0f);
            //spriteBatch.Draw(hitbox, new Rectangle((int)position.X, (int)position.Y, hitbox.Width, hitbox.Height), Color.White);
        }

        public void Draw(SpriteBatch spriteBatch, float layer)
        {
            spriteBatch.Draw(_animatingSheet, _position, _drawRectangle,
                            Color.White, 0f, Vector2.Zero, _animationScale, SpriteEffects.None, layer);
            //spriteBatch.Draw(hitbox, new Rectangle((int)position.X, (int)position.Y, hitbox.Width, hitbox.Height), Color.White);
        }

        #endregion
    }
}
