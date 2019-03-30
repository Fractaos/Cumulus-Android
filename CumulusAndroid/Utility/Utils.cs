﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CumulusAndroid.Utility
{
    public static class Utils
    {
        // Constants

        public const int BASE_BACKGROUND_WIDTH = 1080, BASE_BACKGROUND_HEIGHT = (BASE_BACKGROUND_WIDTH * 16) / 9;

        public const int GAMEBOARD_OFFSET = BASE_BACKGROUND_HEIGHT / 10 + 10;

        public const float CELL_SIZE = (float)BASE_BACKGROUND_WIDTH / 10;

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

        //public static Grid GameGrid = new Grid();

        public static Random Random = new Random();

        /// <summary>
        /// Dessine une texture pleine
        /// </summary>
        /// <param name="width">La largeur</param>
        /// <param name="height">La hauteur</param>
        /// <param name="color">La couleur</param>
        /// <returns></returns>
        public static Texture2D CreateRectangleTexture(int width, int height, Color color)
        {
            if (width <= 0)
            {
                width = 1;
            }
            var texture = new Texture2D(Main.Device, width, height);
            var colors = new Color[width * height];
            for (var i = 0; i < colors.Length; i++)
            {
                colors[i] = color;
            }
            texture.SetData(colors);
            return texture;
        }

        /// <summary>
        /// Dessine une texture creuse, avec seulement les contours
        /// </summary>
        /// <param name="width">La largeur</param>
        /// <param name="height">La hauteur</param>
        /// <param name="color">La couleur</param>
        /// <returns></returns>
        public static Texture2D CreateContouringRectangleTexture(int width, int height, Color color)
        {
            var texture = new Texture2D(Main.Device, width, height);
            var colors = new Color[width * height];
            for (var i = 0; i < colors.Length; i++)
            {
                if ((i >= 0 && i < width) || (i > ((colors.Length - 1) - width) && i <= colors.Length - 1))
                {
                    colors[i] = color;
                }
                if (i % width == 0)
                {
                    colors[i] = color;
                    if (i > 0)
                    {
                        colors[i - 1] = color;
                    }
                }
            }

            texture.SetData(colors);

            return texture;
        }

    }
}