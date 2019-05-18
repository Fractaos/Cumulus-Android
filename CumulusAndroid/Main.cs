using Cumulus.Graphics;
using Cumulus.Screens;
using Cumulus.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ResolutionBuddy;
using System;
using Xamarin.Essentials;
using DisplayOrientation = Microsoft.Xna.Framework.DisplayOrientation;
using Screen = Cumulus.Screens.Screen;

namespace Cumulus
{
    public class Main
    {
        public static GraphicsDeviceManager Graphics;
        public static GraphicsDevice Device;
        public static Game Instance;
        public static ContentManager Content;
        public static Screen CurrentScreen;
        public static SpriteBatch SpriteBatch;
        public static IResolution Resolution;

        public static GameState GameState;

        public Main(GraphicsDeviceManager graphics, Game game)
        {
            VersionTracking.Track();
            Graphics = graphics;
            Device = graphics.GraphicsDevice;
            Instance = game;
            Content = game.Content;
            SpriteBatch = new SpriteBatch(Device);
        }

        public void Initialize()
        {
            Assets.LoadAll();

            Graphics.IsFullScreen = true;
            Graphics.SupportedOrientations = DisplayOrientation.Portrait;
            Graphics.SynchronizeWithVerticalRetrace = false;
            Graphics.GraphicsProfile = GraphicsProfile.HiDef;
            Graphics.ApplyChanges();
            bool letterbox =
                (Graphics.PreferredBackBufferHeight / (float)Graphics.PreferredBackBufferWidth) <
                Utils.SCREEN_WIDTH / (float)Utils.SCREEN_HEIGHT;
            Resolution = new ResolutionComponent(Instance, Graphics,
                new Point(Utils.SCREEN_WIDTH, Utils.SCREEN_HEIGHT),
                new Point(Graphics.PreferredBackBufferHeight, Graphics.PreferredBackBufferWidth),
                false, letterbox);


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
            SpriteBatch.Begin(SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                null, null, null, null,
                Resolution.TransformationMatrix());
            CurrentScreen?.Draw();
#if RELEASE
            SpriteBatch.DrawString(Assets.Pixel30, "Version : " + VersionTracking.CurrentVersion, 
                Utils.GetPositionOnScreenByPercent(0.05f, 0.95f), Color.Black);
#endif
            SpriteBatch.End();
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
    }
}