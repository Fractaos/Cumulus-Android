using CumulusAndroid.Graphics;
using CumulusAndroid.Utility;
using CumulusAndroid.Utility.Tween;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CumulusAndroid.Screens
{
    public class MenuScreen : Screen
    {
        #region Fields

        private Texture2D[] _backgrounds;
        private float _offset, _timePassed, _opacity;
        private TweenValue _tOffset, _tOpacity;
        private bool _firstTransition, _secondTransition, _thirdTransition;

        private Button _playButton, _skinButton, _quitButton, _settingsButton;

        #endregion

        #region Private Methods

        private static void SkinButtonClick(object sender, EventArgs e)
        {
            //Main.SetGameState(GameState.SkinMenu);
        }

        private static void QuitButtonClick(object sender, EventArgs e)
        {
            Main.Instance.Exit();
        }

        private static void PlayButtonClick(object sender, EventArgs e)
        {
            //Main.SetGameState(GameState.Game);
        }

        #endregion

        public override void Create()
        {
            _backgrounds = new[]
            {
                Assets.MenuBg1,
                Assets.MenuBg2,
                Assets.MenuBg3,
                Assets.MenuBg4,
                Assets.MenuBg5,
                Assets.MenuBg6,
                Assets.MenuBg7,
                Assets.MenuBg8,
                Assets.MenuBg9,
                Assets.MenuBg10,
                Assets.MenuBg11
            };

            _playButton = new Button(new Vector2(((float)Utils.WINDOW_WIDTH / 2) - ((Assets.Button.Width * Utils.SCALE) / 2),
                ((float)Utils.WINDOW_HEIGHT / 2) - ((Assets.Button.Height * Utils.SCALE) / 2)),
                "Play",
                Utils.SCALE);
            _playButton.Click += PlayButtonClick;

            _skinButton = new Button(new Vector2(((float)Utils.WINDOW_WIDTH / 2) - ((Assets.Button.Width * Utils.SCALE) / 2),
                ((float)Utils.WINDOW_HEIGHT / 2) + (Assets.Button.Height * Utils.SCALE)),
                "Skins",
                Utils.SCALE);
            _skinButton.Click += SkinButtonClick;

            _quitButton = new Button(new Vector2(((float)Utils.WINDOW_WIDTH / 2) - ((Assets.Button.Width * Utils.SCALE) / 2),
                ((float)Utils.WINDOW_HEIGHT / 2) + ((Assets.Button.Height * Utils.SCALE) * 2.5f)),
                "Quit",
                Utils.SCALE);
            _quitButton.Click += QuitButtonClick;

            _settingsButton = new Button(new Vector2(Utils.WINDOW_WIDTH - (Assets.ButtonSettings.Width * Utils.SCALE) - 10, 10));

            _tOffset = new TweenValue();
            _tOpacity = new TweenValue();

        }

        private void EaseBackground(float change, float duration, EaseFunction effect)
        {
            _tOffset.Move(_offset, change, duration, effect, (value) => { _offset = value; });
        }

        private void EaseButton(float change, float duration, EaseFunction effect)
        {
            _tOpacity.Move(_opacity, change, duration, effect, (value) => { _opacity = value; });
        }

        public override void Update(GameTime time)
        {
            if (!Utils.IntroPlayed)
            {
                if (!_firstTransition)
                {
                    EaseBackground(_offset + (9.1f * (1920 * Utils.SCALE)), 4000, EaseFunction.EaseInOutCubic);
                    _firstTransition = true;
                }
                _timePassed += time.ElapsedGameTime.Milliseconds;
                if (_timePassed >= 4000 && !_secondTransition)
                {
                    _timePassed = 0;
                    EaseBackground(_offset - (0.1f * (1920 * Utils.SCALE)), 1000, EaseFunction.EaseInOutQuad);
                    _secondTransition = true;
                }
                if (_timePassed >= 1000 && !_thirdTransition && _secondTransition)
                {
                    _timePassed = 0;
                    EaseButton(_opacity + 1f, 1000, EaseFunction.EaseInQuad);
                    _thirdTransition = true;
                    Utils.IntroPlayed = true;
                }
            }
            else
            {
                _offset = 9 * (1920 * Utils.SCALE);
                _opacity = 1f;
            }

            _tOffset.Update(time.ElapsedGameTime.Milliseconds);
            _tOpacity.Update(time.ElapsedGameTime.Milliseconds);
            _playButton.Update(time);
            _skinButton.Update(time);
            _quitButton.Update(time);
            _settingsButton.Update(time);
        }

        public override void Draw()
        {
            _spriteBatch.Begin();
            {
                for (var i = 0; i < _backgrounds.Length; i++)
                {
                    _spriteBatch.Draw(_backgrounds[i], new Vector2(0, -(i * (1920 * Utils.SCALE)) + _offset), null, Color.White, 0f, Vector2.Zero, Utils.SCALE, SpriteEffects.None, 0);
                }
                _playButton.Draw(_spriteBatch, _opacity);
                _skinButton.Draw(_spriteBatch, _opacity);
                _quitButton.Draw(_spriteBatch, _opacity);
                _settingsButton.Draw(_spriteBatch, _opacity);
            }
            _spriteBatch.End();
        }
    }
}