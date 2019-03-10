using System;

namespace CumulusAndroid.Utility
{
    public static class Utils
    {
        // Constants
        //public const float SCALE = 0.5f;

        public const int WINDOW_WIDTH = 1080, WINDOW_HEIGHT = (WINDOW_WIDTH * 16) / 9;

        public const int GAMEBOARD_OFFSET = WINDOW_HEIGHT / 10 + 10;

        public const float CELL_SIZE = (float)WINDOW_WIDTH / 10;

        public const float BACKGROUND_DEPTH = 0f;

        public const float VISUALHELPER_DEPTH = 0.1f;

        public const float ENTITY_DEPTH = 0.2f;

        public const int TIME_BEFORE_ANIMATION = 700;

        public const int TIME_BEFORE_GAME_START = 800;

        // Processing fields
        public static float TimeElapsedSinceStartEating = 0;
        public static float TimeElapsedSinceStartBreaking = 0;

        //public static SkinType CurrentSkin = SkinType.CumulusBase;

        //public static UsableObject CurrentObjectSelected = UsableObject.LittleFertilizer;

        public static bool IntroPlayed = false;

        //public static Grid _gameGrid = new Grid();

        public static Random Random = new Random();
    }
}