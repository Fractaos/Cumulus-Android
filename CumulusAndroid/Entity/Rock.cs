using CumulusGame.Graphics;
using CumulusGame.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CumulusGame.Entity
{
    public class Rock : GameEntity
    {
        #region Fields & Properties

        // Constant
        private const float ROCK_SCALE = 0.6f;
        private const float LIGHT_SCALE = 0.8f;

        // Differents cooldown fields 
        private static float _timeElapsedSinceLastCreated;

        private const float FIXED_COOLDOWN = 1000;
        private const float BREAK_TIME = 500;

        // Graphics fields
        private readonly Animation _spriteAnimated;
        private readonly Animation _lightning;
        private Texture2D _blackShader;

        // Processing fields
        public bool Broken { get; private set; }
        public bool Animate { get; private set; }
        public float TimeToBreak { get; }
        public static float BaseCooldown => FIXED_COOLDOWN;
        public static float CurrentCooldown { get; private set; } = FIXED_COOLDOWN;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="position">The cell we want the rock to be on</param>
        public Rock(Cell position)
        {
            _cellOn = position;
            _cellOn.IsEmpty = false;
            TimeToBreak = BREAK_TIME;
            this._position = new Vector2(_cellOn.Position.X - (((((float)Assets.Rock.Width / 6) * ROCK_SCALE) - Utils.CELL_SIZE) / 2),
                _cellOn.Position.Y - (((float)Assets.Rock.Height / 2) * ROCK_SCALE));
            _spriteAnimated = new Animation(this._position,
                350,
                6,
                Assets.Rock,
                TypeOfSheet.Horizontal,
                false,
                ROCK_SCALE);
            _lightning = new Animation(new Vector2(this._position.X - 30,
                this._position.Y - (_cellOn.Hitbox.Height - 30)),
                350,
                7,
                Assets.Lightning,
                TypeOfSheet.Horizontal,
                false,
                LIGHT_SCALE);
        }

        #endregion

        #region Class Methods

        /// <summary>
        /// Update the elapsed game time since the last Rock has been created
        /// </summary>
        /// <param name="elapsedGameTimeInMillis">The elapsed game time (In milliseconds)</param>
        public static void UpdateElapsedTime(float elapsedGameTimeInMillis)
        {
            _timeElapsedSinceLastCreated -= elapsedGameTimeInMillis;
            if (_timeElapsedSinceLastCreated < 0)
            {
                _timeElapsedSinceLastCreated = 0;
            }
        }

        public static void ApplyCooldownModifier(float modifier)
        {
            CurrentCooldown *= modifier;
        }

        /// <summary>
        /// Check if the cooldown is up or not
        /// </summary>
        /// <returns>Return true if it's up and false if not</returns>
        public static bool CooldownUp()
        {
            return _timeElapsedSinceLastCreated <= 0;
        }

        /// <summary>
        /// Put the Rock on cooldown
        /// </summary>
        public static void OnCooldown()
        {
            _timeElapsedSinceLastCreated = CurrentCooldown;
        }

        #endregion

        #region Public Methods

        public void Destroy()
        {
            Broken = true;
            _cellOn.IsEmpty = true;
            Animate = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _spriteAnimated.Draw(spriteBatch, Utils.ENTITY_DEPTH);
            if (Animate)
            {
                _blackShader = Utils.CreateRectangleTexture(Utils.SCREEN_WIDTH, Utils.SCREEN_HEIGHT, Color.Black);
                spriteBatch.Draw(_blackShader, new Vector2(0, 0), null, Color.White * 0.2f,
                    0f, Vector2.Zero, 1, SpriteEffects.None, 0.3f);
                _lightning.Draw(spriteBatch, 1);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (!_spriteAnimated.AnimationEnded && !_lightning.AnimationEnded)
            {
                if (Animate)
                {
                    _spriteAnimated.Play(gameTime.ElapsedGameTime.Milliseconds);
                    _lightning.Play(gameTime.ElapsedGameTime.Milliseconds);
                }
            }
            else
            {
                Animate = false;
            }
        }

        #endregion
    }
}
