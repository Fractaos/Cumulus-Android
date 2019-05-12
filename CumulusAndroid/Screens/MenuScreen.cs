using Android.OS;
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
            Process.KillProcess(Process.MyPid());
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

            _playButton = new Button(new Vector2(Utils.CENTER_SCREEN_HORIZONTAL,
                Utils.CENTER_SCREEN_VERTICAL), Assets.Button, "Play", 0.45f);
            _playButton.Click += PlayButtonClick;

            _skinButton = new Button(Utils.GetPositionOnScreenByPercent(0.5f, 0.65f),
                Assets.Button, "Skins", 0.45f);
            _skinButton.Click += SkinButtonClick;

            _quitButton = new Button(Utils.GetPositionOnScreenByPercent(0.5f, 0.75f),
                Assets.Button, "Quit", 0.45f);
            _quitButton.Click += QuitButtonClick;

            _settingsButton = new Button(Utils.GetPositionOnScreenByPercent(0.90f, 0.05f),
                Assets.ButtonSettings, null, 0.1f);

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
                    EaseBackground(_offset + (9.1f * Utils.SCREEN_HEIGHT), 4000, EaseFunction.EaseInOutCubic);
                    _firstTransition = true;
                }
                _timePassed += time.ElapsedGameTime.Milliseconds;
                if (_timePassed >= 4000 && !_secondTransition)
                {
                    _timePassed = 0;
                    EaseBackground(_offset - (0.1f * Utils.SCREEN_HEIGHT), 1000, EaseFunction.EaseInOutQuad);
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
                _offset = 9 * Utils.SCREEN_HEIGHT;
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
            for (var i = 0; i < _backgrounds.Length; i++)
            {
                _spriteBatch.Draw(_backgrounds[i], new Vector2(0, -(i * Utils.SCREEN_HEIGHT) + _offset), Color.White);
            }
            _playButton.Draw(_spriteBatch, _opacity);
            _skinButton.Draw(_spriteBatch, _opacity);
            _quitButton.Draw(_spriteBatch, _opacity);
            _settingsButton.Draw(_spriteBatch, _opacity);
        }
    }
}