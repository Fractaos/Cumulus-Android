using Microsoft.Xna.Framework;

namespace CumulusGame.Entity.Fertilizers
{
    public class LittleFertilizer : Fertilizer
    {
        #region Fields & Properties

        // Differents cooldown fields 
        private static float _timeElapsedSinceLastCreated;

        private const float FixedCooldown = 5000;
        private const float FixedTimeToEat = 1000;
        private const float FixedAmountAnger = 1;

        public static float BaseCooldown => FixedCooldown;
        public static float CurrentCooldown { get; private set; } = FixedCooldown;

        public static float ActualCooldown
        {
            get { return _timeElapsedSinceLastCreated; }
        }

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="position">The position of the fertilizer</param>
        public LittleFertilizer(Vector2 position) : base(position, Assets.LittleFertilizer)
        {
            _timeToEat = FixedTimeToEat;
            _amountOfAnger = FixedAmountAnger;
            texture = Assets.LittleFertilizer;
        }

        #endregion

        #region Class Methods

        /// <summary>
        /// Update the elapsed game time since the last Little Fertilizer has been created
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
        /// Put the Little Fertilizer on cooldown
        /// </summary>
        public static void OnCooldown()
        {
            _timeElapsedSinceLastCreated = CurrentCooldown;
        }

        #endregion

        #region Public Methods

        public override void Update(GameTime gameTime)
        { }

        #endregion
    }
}