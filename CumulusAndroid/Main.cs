using CumulusGame.Graphics;
using CumulusGame.Screens;
using CumulusGame.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ResolutionBuddy;
using System;
using Xamarin.Essentials;
using DisplayOrientation = Microsoft.Xna.Framework.DisplayOrientation;
using Screen = CumulusGame.Screens.Screen;

namespace CumulusGame
{
    public class Main
    {
        private static GraphicsDeviceManager _graphics;
        public static GraphicsDevice Device;
        private static Game _instance;
        public static ContentManager Content;
        private static Screen _currentScreen;
        public static SpriteBatch SpriteBatch;
        private static IResolution _resolution;

        private static GameState _gameState;

        public Main(GraphicsDeviceManager graphics, Game game)
        {
            VersionTracking.Track();
            _graphics = graphics;
            Device = graphics.GraphicsDevice;
            _instance = game;
            Content = game.Content;
            SpriteBatch = new SpriteBatch(Device);
        }

        public void Initialize()
        {
            Assets.LoadAll();

            _graphics.IsFullScreen = true;
            _graphics.SupportedOrientations = DisplayOrientation.Portrait;
            _graphics.SynchronizeWithVerticalRetrace = false;
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
            _graphics.ApplyChanges();
            int realWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            int realHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            bool letterbox =
                (realWidth / (float)realHeight) <
                Utils.SCREEN_WIDTH / (float)Utils.SCREEN_HEIGHT;
            _resolution = new ResolutionComponent(_instance, _graphics,
                new Point(Utils.SCREEN_WIDTH, Utils.SCREEN_HEIGHT),
                new Point(realWidth, realHeight),
                false, letterbox);


            SetGameState(GameState.Game);
        }

        public void Update(GameTime time)
        {
            Input.Update();
            _currentScreen?.Update(time);
        }

        public void Draw()
        {
            Device.Clear(Color.Black);
            SpriteBatch.Begin(SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                null, null, null, null,
                _resolution.TransformationMatrix());
            _currentScreen?.Draw();
#if !DEBUG
            SpriteBatch.DrawString(Assets.Pixel30, "Version : " + VersionTracking.CurrentVersion,
                Utils.GetPositionOnScreenByPercent(0.05f, 0.95f), Color.Black);
#endif
            SpriteBatch.End();
        }

        private static void SetScreen(Screen screen)
        {
            _currentScreen = screen;
            _currentScreen.Create();
        }

        public static void SetGameState(GameState newGameState)
        {
            _gameState = newGameState;
            switch (_gameState)
            {
                case GameState.Game:
                    SetScreen(new GameScreen());
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
                    throw new IndexOutOfRangeException();
            }
        }
    }
}