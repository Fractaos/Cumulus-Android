using CumulusGame.Entity;
using CumulusGame.Entity.Fertilizers;
using CumulusGame.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace CumulusGame.Utility
{
    public static class Utils
    {
        // Constants
        public const int SCREEN_WIDTH = 1080;
        public const int SCREEN_HEIGHT = (SCREEN_WIDTH * 16) / 9;

        public const int CENTER_SCREEN_HORIZONTAL = SCREEN_WIDTH / 2;

        public const int CENTER_SCREEN_VERTICAL = SCREEN_HEIGHT / 2;

        public const int GAMEBOARD_OFFSET = SCREEN_HEIGHT / 10 + 10;

        public const float CELL_SIZE = (float)SCREEN_WIDTH / 10;

        public const float BACKGROUND_DEPTH = 0f;

        public const float VISUALHELPER_DEPTH = 0.1f;

        public const float ENTITY_DEPTH = 0.2f;

        public const int TIME_BEFORE_ANIMATION = 700;

        public const int TIME_BEFORE_GAME_START = 800;

        // Processing fields
        public static float TimeElapsedSinceStartEating = 0;
        public static float TimeElapsedSinceStartBreaking = 0;

        public static SkinType CurrentSkin = SkinType.CumulusBase;

        public static UsableObject CurrentObjectSelected = UsableObject.LittleFertilizer;

        public static bool IntroPlayed = false;

        public static readonly Grid GameGrid = new Grid();

        public static readonly Random Random = new Random();

        /// <summary>
        /// Retourne une position en pixel par rapport à un pourcentage de l'écran passé en paramètre
        /// </summary>
        /// <param name="percentX">Pourcentage sur l'axe X</param>
        /// <param name="percentY">Pourcentage sur l'axe Y</param>
        /// <returns>Retourne un Vector2 avec la position en pixel</returns>
        public static Vector2 GetPositionOnScreenByPercent(float percentX, float percentY)
        {
            return new Vector2(SCREEN_WIDTH * percentX, SCREEN_HEIGHT * percentY);
        }

        /// <summary>
        /// Retourne la valeur transformé sur la range voulue
        /// </summary>
        /// <param name="value">Valeur d'entrée</param>
        /// <param name="actualMax">Maximum de la range actuelle</param>
        /// <param name="actualMin">Minimum de la range actuelle</param>
        /// <param name="targetMax">Maximum de la range ciblée</param>
        /// <param name="targetMin">Minimum de la range ciblée</param>
        /// <returns>Retourne la valeur transformée</returns>
        public static float ScalingValue(float value, float actualMax, float actualMin, float targetMax,
            float targetMin)
        {
            return ((value - actualMin) / (actualMax - actualMin)) * (targetMax - targetMin) + targetMin;
        }

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

        /// <summary>
        /// Create a rock at the position
        /// </summary>
        /// <param name="position">The position</param>
        /// <param name="rocksList">The rock list</param>
        /// <param name="gameEntities">The game entity list</param>
        public static void CreateRock(Cell position, List<Rock> rocksList, List<GameEntity> gameEntities)
        {
            if (Rock.CooldownUp())
            {
                var newRock = new Rock(position);
                Rock.OnCooldown();
                rocksList.Add(newRock);
                gameEntities.Add(newRock);
                foreach (GameEntity entity in gameEntities)
                {
                    if (entity is Cumulus tempCumulus)
                    {
                        tempCumulus.AddObstacle(newRock);
                    }
                }
            }
        }

        /// <summary>
        /// Create a little fertilizer at the position
        /// </summary>
        /// <param name="position">The position</param>
        /// <param name="fertilizersList">The fertilizer list</param>
        /// <param name="gameEntities">The game entity list</param>
        public static void CreateLittleFertilizer(Vector2 position, List<Fertilizer> fertilizersList, List<GameEntity> gameEntities)
        {
            if (LittleFertilizer.CooldownUp())
            {
                var newFerti = new LittleFertilizer(new Vector2(position.X, position.Y));
                LittleFertilizer.OnCooldown();
                fertilizersList.Add(newFerti);
                gameEntities.Add(newFerti);
                foreach (GameEntity gameEntity in gameEntities)
                {
                    if (gameEntity is Cumulus tempCumu)
                    {
                        tempCumu.AddTarget(newFerti);
                    }
                }
            }
        }

        /// <summary>
        /// Create a medium fertilizer at the position
        /// </summary>
        /// <param name="position">The position</param>
        /// <param name="fertilizersList">The fertilizer list</param>
        /// <param name="gameEntities">The game entity list</param>
        public static void CreateMediumFertilizer(Vector2 position, List<Fertilizer> fertilizersList, List<GameEntity> gameEntities)
        {
            if (MediumFertilizer.CooldownUp())
            {
                var newFerti = new MediumFertilizer(new Vector2(position.X, position.Y));
                MediumFertilizer.OnCooldown();
                fertilizersList.Add(newFerti);
                gameEntities.Add(newFerti);
                foreach (GameEntity gameEntity in gameEntities)
                {
                    if (gameEntity is Cumulus tempCumu)
                    {
                        tempCumu.AddTarget(newFerti);
                    }
                }
            }
        }

        /// <summary>
        /// Create a large fertilizer at the position
        /// </summary>
        /// <param name="position">The position</param>
        /// <param name="fertilizersList">The fertilizer list</param>
        /// <param name="gameEntities">The game entity list</param>
        public static void CreateLargeFertilizer(Vector2 position, List<Fertilizer> fertilizersList, List<GameEntity> gameEntities)
        {
            if (LargeFertilizer.CooldownUp())
            {
                var newFerti = new LargeFertilizer(new Vector2(position.X, position.Y));
                LargeFertilizer.OnCooldown();
                fertilizersList.Add(newFerti);
                gameEntities.Add(newFerti);
                foreach (var gameEntity in gameEntities)
                {
                    if (gameEntity is Cumulus tempCumu)
                    {
                        tempCumu.AddTarget(newFerti);
                    }
                }
            }
        }

    }
}