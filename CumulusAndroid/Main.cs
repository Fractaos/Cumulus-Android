using CumulusAndroid.Graphics;
using CumulusAndroid.Screens;
using CumulusAndroid.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using Screen = CumulusAndroid.Screens.Screen;

namespace CumulusAndroid
{
    public class Main
    {
        public static GraphicsDeviceManager Graphics;
        public static GraphicsDevice Device;
        public static Game Instance;
        public static ContentManager Content;
        public static Screen CurrentScreen;

        public static int OriginalScreenWidth, OriginalScreenHeight;
        public static int OriginalScreenWidthMiddle, OriginalScreenHeightMiddle;
        public static float Scale;

        public static GameState GameState;

        public Main(GraphicsDeviceManager graphics, Game game)
        {
            Graphics = graphics;
            Device = graphics.GraphicsDevice;
            Instance = game;
            Content = game.Content;
            OriginalScreenHeight = Graphics.PreferredBackBufferWidth;
            OriginalScreenHeightMiddle = OriginalScreenHeight / 2;
            OriginalScreenWidth = Graphics.PreferredBackBufferHeight;
            OriginalScreenWidthMiddle = OriginalScreenWidth / 2;
            Scale = (float)OriginalScreenHeight / Utils.BASE_BACKGROUND_HEIGHT <= 1 ? (float)OriginalScreenHeight / Utils.BASE_BACKGROUND_HEIGHT : 1;
        }

        public void Initialize()
        {
            Assets.LoadAll();

            Graphics.IsFullScreen = true;
            Graphics.SupportedOrientations = DisplayOrientation.Portrait;
            Graphics.PreferredBackBufferWidth = Utils.BASE_BACKGROUND_WIDTH;
            Graphics.PreferredBackBufferHeight = Utils.BASE_BACKGROUND_HEIGHT;
            Graphics.SynchronizeWithVerticalRetrace = false;
            Graphics.GraphicsProfile = GraphicsProfile.HiDef;
            Graphics.ApplyChanges();

            SetGameState(GameState.Menu);
        }

        public void Update(GameTime time)
        {
            Input.Update();
            CurrentScreen?.Update(time);
        }

        public void Draw()
        {
            Device.Clear(Color.Black);
            CurrentScreen?.Draw();
        }

        private static void SetScreen(Screen screen)
        {
            CurrentScreen = screen;
            CurrentScreen.Create();
        }

        public static void SetGameState(GameState newGameState)
        {
            GameState = newGameState;
            switch (GameState)
            {
                case GameState.Game:
                    //SetScreen(new GameScreen());
                    break;
                case GameState.Menu:
                    SetScreen(new MenuScreen());
                    break;
                case GameState.SkinMenu:
                    //SetScreen(new SkinScreen());
                    break;
                case GameState.Test:
                    SetScreen(new TestScreen());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static int GetScreenWidthPositionByPercent(float percent)
        {
            return (int)(OriginalScreenWidth * percent);
        }

        public static int GetScreenHeightPositionByPercent(float percent)
        {
            return (int)(OriginalScreenHeight * percent);
        }
    }
}