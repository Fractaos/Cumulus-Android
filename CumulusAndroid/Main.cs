using CumulusAndroid.Graphics;
using CumulusAndroid.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CumulusAndroid
{
    public class Main
    {
        public static GraphicsDeviceManager Graphics;
        public static GraphicsDevice Device;
        public static Game Instance;
        public static ContentManager Content;
        public static Screen CurrentScreen;

        public static GameState GameState;

        public Main(GraphicsDeviceManager graphics, Game game)
        {
            Graphics = graphics;
            Device = graphics.GraphicsDevice;
            Instance = game;
            Content = game.Content;
        }

        public void Initialize()
        {
            Assets.LoadAll();

            Graphics.IsFullScreen = true;
            Graphics.PreferredBackBufferWidth = 800;
            Graphics.PreferredBackBufferHeight = 600;
            Graphics.SynchronizeWithVerticalRetrace = false;
            Graphics.SupportedOrientations = DisplayOrientation.Portrait;
            Graphics.ApplyChanges();

            SetGameState(GameState.Test);
        }

        public void Update(GameTime time)
        {
            //Input.Update();
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
                    //SetScreen(new MenuScreen());
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
    }
}