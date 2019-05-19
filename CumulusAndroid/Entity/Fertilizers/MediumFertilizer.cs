using Microsoft.Xna.Framework;

namespace CumulusGame.Entity.Fertilizers
{
    public class MediumFertilizer : Fertilizer
    {
        #region Fields & Properties

        // Differents cooldown fields 
        private static float _timeElapsedSinceLastCreated;

        private const float FixedCooldown = 3000;
        private const float FixedTimeToEat = 2000;
        private const float FixedAmountAnger = 2.5f;

        public static float BaseCooldown => FixedCooldown;
        public static float CurrentCooldown { get; private set; } = FixedCooldown;

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="position">The position of the fertilizer</param>
        public MediumFertilizer(Vector2 position) : base(position, Assets.MediumFertilizer)
        {
            _timeToEat = FixedTimeToEat;
            _amountOfAnger = FixedAmountAnger;
            texture = Assets.MediumFertilizer;
        }

        #endregion

        #region Class Methods

        /// <summary>
        /// Update the elapsed game time since the last Medium Fertilizer has been created
        /// </summary>
        /// <param name="elapsedGameTimeInMillis">The elapsed game time (In milliseconds)</param>
        public static void UpdateElapsedTime(float elapsedGameTimeInMillis)
        {
            _timeElapsedSinceLastCreated -= elapsedGameTimeInMillis;
            if (_timeElapsedSinceLastCreated <= 0)
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
        /// Put the Medium Fertilizer on cooldown
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