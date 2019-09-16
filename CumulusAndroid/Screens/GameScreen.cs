using CumulusGame.Entity;
using CumulusGame.Entity.Fertilizers;
using CumulusGame.Graphics;
using CumulusGame.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace CumulusGame.Screens
{
    public class GameScreen : Screen
    {
        #region Fields

        // Processing fields
        private bool _controlEnabled;
        private float _timeCounter;

        private const float HUD_Y_POSITION = Utils.SCREEN_HEIGHT - (Utils.SCREEN_HEIGHT * 0.05f);

        // Graphics fields
        private Background _bg;
        private UserInterface _hud;

        // Game Objects
        private Cumulus _cumulus;
        private Score _score;
        private readonly List<GameEntity> _gameEntities = new List<GameEntity>();
        private readonly List<Fertilizer> _fertilizers = new List<Fertilizer>();
        private readonly List<Button> _buttons = new List<Button>();
        private readonly List<Rock> _rocks = new List<Rock>();

        //Debug
        private Button _backButton;

        #endregion

        #region Public Methods

        public override void Create()
        {
            _score = new Score(new Vector2(Utils.SCREEN_WIDTH * .83f, Utils.SCREEN_HEIGHT * 0.022f));
            _hud = new UserInterface(new Vector2(0, HUD_Y_POSITION));

            _controlEnabled = false;
            _bg = new Background();

            _backButton = new Button(new Vector2(Utils.SCREEN_WIDTH * 0.08f, Utils.SCREEN_WIDTH * 0.05f), Assets.ButtonArrow, null, .1f);
            _backButton.Click += BackButton_Click;
            _buttons.Add(_backButton);

            _cumulus = new Cumulus(Utils.GameGrid.GetCellByRowColumn(5, 7));
            _gameEntities.Add(_cumulus);
        }

        private static void BackButton_Click(object sender, EventArgs e)
        {
            Main.SetGameState(GameState.Menu);
        }

        public override void Update(GameTime gameTime)
        {
            //Get the mouse
            MouseState mouse = Mouse.GetState();

            //Update all Fertilizer and Rock time passed and cooldown
            LittleFertilizer.UpdateElapsedTime(gameTime.ElapsedGameTime.Milliseconds);
            MediumFertilizer.UpdateElapsedTime(gameTime.ElapsedGameTime.Milliseconds);
            LargeFertilizer.UpdateElapsedTime(gameTime.ElapsedGameTime.Milliseconds);
            Rock.UpdateElapsedTime(gameTime.ElapsedGameTime.Milliseconds);

            //Lock the control during the beginning of the game
            if (_controlEnabled)
            {
                Utils.GameGrid.CreateObjectOnCell(_fertilizers, _rocks, _gameEntities);
            }
            else
            {
                _timeCounter += gameTime.ElapsedGameTime.Milliseconds;
                if (_timeCounter > Utils.TIME_BEFORE_GAME_START + Utils.TIME_BEFORE_ANIMATION)
                {
                    _controlEnabled = true;
                    _timeCounter = 0;
                }
            }


            _cumulus.Update(gameTime);
            Utils.GameGrid.Update(gameTime);
            foreach (Fertilizer fertilizer in _fertilizers)
            {
                fertilizer.Update(gameTime);
            }

            foreach (Rock rock in _rocks)
            {
                rock.Update(gameTime);
            }

            for (var i = _fertilizers.Count - 1; i >= 0; i--)
            {
                if (_fertilizers[i].Eated)
                {
                    _score.FertilizerEated(_fertilizers[i]);
                    _gameEntities.Remove(_fertilizers[i]);
                    _fertilizers.Remove(_fertilizers[i]);
                }
            }

            for (var i = _rocks.Count - 1; i >= 0; i--)
            {
                if (_rocks[i].Broken && !_rocks[i].Animate)
                {
                    _score.RockBreaked(_rocks[i]);
                    _gameEntities.Remove(_rocks[i]);
                    _rocks.Remove(_rocks[i]);
                }
            }

            _score.Update(gameTime);
            _buttons.ForEach(button => button.Update(gameTime));
            _hud.Update(gameTime);
            _gameEntities.Sort();
        }

        public override void Draw()
        {
            {
                _bg.Draw(_spriteBatch);
                Utils.GameGrid.Draw(_spriteBatch);

                foreach (GameEntity entity in _gameEntities)
                {
                    entity.Draw(_spriteBatch);
                }

                _buttons.ForEach(button => button.Draw(_spriteBatch));


                _score.Draw(_spriteBatch);
            }
            _hud.Draw(_spriteBatch);
        }

        #endregion

        #region Private class

        private class UserInterface
        {
            private const float WIDTH_PERCENT_ICON = Utils.SCREEN_WIDTH * 0.08f;
            private const float WIDTH_PERCENT_SPACE = Utils.SCREEN_WIDTH * 0.15f;
            private readonly float _scale = WIDTH_PERCENT_ICON / Assets.IconeLittleFertilizer.Width;
            private const float X_START_POSITION = WIDTH_PERCENT_SPACE;
            private readonly List<ObjectButton> _theButtons;

            public UserInterface(Vector2 position)
            {
                _theButtons = new List<ObjectButton>
                {
                    new LittleFertilizerButton(new Vector2(position.X + X_START_POSITION, position.Y), _scale),
                    new MediumFertilizerButton(new Vector2(
                        position.X + X_START_POSITION * 2 + (Assets.IconeLittleFertilizer.Width * _scale),
                        position.Y), _scale),
                    new LargeFertilizerButton(new Vector2(
                        position.X + X_START_POSITION * 3 + (Assets.IconeLittleFertilizer.Width * 2 * _scale),
                        position.Y), _scale),
                    new RockButton(new Vector2(
                        position.X + X_START_POSITION * 4 + (Assets.IconeLittleFertilizer.Width * 3 * _scale),
                        position.Y), _scale)
                };
            }

            public void Update(GameTime gameTime)
            {

                _theButtons.ForEach(button => button.Update(gameTime));
            }

            public void Draw(SpriteBatch spriteBatch)
            {
                _theButtons.ForEach(button => button.Draw(spriteBatch));
            }
        }

        private abstract class ObjectButton
        {
            protected readonly Vector2 Position;
            protected float _percentCircle;
            protected float _percentCirclePerUpdate;
            protected readonly Vector4 TheColor;

            protected readonly float Scale;
            protected const float SCALE_DIFFERENCE_COOLDOWN = 0.01f;

            protected readonly Texture2D CooldownCircle;
            protected readonly Effect Effect;

            protected Button _button;

            protected ObjectButton(Vector2 position, Effect effect, float scale)
            {
                Position = position;
                Effect = effect;
                Scale = scale;
                _percentCircle = 1f;

                TheColor = new Vector4(0.0f, 0.0f, 1.0f, 0.6f);

                CooldownCircle = Assets.CooldownCircle;

            }

            public abstract void Update(GameTime gameTime);

            public abstract void Draw(SpriteBatch spriteBatch);
        }
        //TODO : FIX POSITIONNEMENT ET TAILLE DES CERCLES DE COOLDOWN + VOIR SOLUTION PLUS PROPRE NIVEAU DES SPRITEBATCH
        private class LittleFertilizerButton : ObjectButton
        {
            public LittleFertilizerButton(Vector2 position, float scale) : base(position, Assets.EffectLittle, scale)
            {
                _percentCirclePerUpdate = _percentCircle / LittleFertilizer.CurrentCooldown;
                _button = new Button(Position, Assets.IconeLittleFertilizer, null, Scale);
                _button.Click += Button_Click;
            }

            private static void Button_Click(object sender, EventArgs e)
            {
                Utils.CurrentObjectSelected = UsableObject.LittleFertilizer;
            }

            public override void Update(GameTime gameTime)
            {
                _button.Update(gameTime);
                UpdatePercentCircle(gameTime);
            }

            public override void Draw(SpriteBatch spriteBatch)
            {
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate,
                    BlendState.AlphaBlend);
                _button.Draw(spriteBatch);
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate,
                    BlendState.AlphaBlend);
                DrawShader(spriteBatch);
            }

            private void DrawShader(SpriteBatch spriteBatch)
            {
                if (!LittleFertilizer.CooldownUp())
                {
                    Effect.CurrentTechnique.Passes[0].Apply();
                    Effect.Parameters["percent"].SetValue(_percentCircle);
                    Effect.Parameters["R"].SetValue(TheColor.X);
                    Effect.Parameters["G"].SetValue(TheColor.Y);
                    Effect.Parameters["B"].SetValue(TheColor.Z);
                    Effect.Parameters["A"].SetValue(TheColor.W);
                    spriteBatch.Draw(CooldownCircle,
                        new Vector2(
                            Position.X - ((SCALE_DIFFERENCE_COOLDOWN / 2) * CooldownCircle.Width),
                            Position.Y - ((SCALE_DIFFERENCE_COOLDOWN / 2) * CooldownCircle.Height)),
                        null,
                        Color.White, 0f, Vector2.Zero, Scale + SCALE_DIFFERENCE_COOLDOWN, SpriteEffects.None, 1);
                }
            }

            private void UpdatePercentCircle(GameTime gameTime)
            {
                if (!LittleFertilizer.CooldownUp())
                {
                    if (_percentCircle > 0)
                    {
                        _percentCircle -= _percentCirclePerUpdate * gameTime.ElapsedGameTime.Milliseconds;
                        if (_percentCircle <= 0)
                            _percentCircle = 1f;
                    }
                    else
                    {
                        _percentCircle = 1f;
                    }
                }
            }
        }

        private class MediumFertilizerButton : ObjectButton
        {
            public MediumFertilizerButton(Vector2 position, float scale) : base(position, Assets.EffectMedium, scale)
            {
                _percentCirclePerUpdate = _percentCircle / MediumFertilizer.CurrentCooldown;
                _button = new Button(Position, Assets.IconeMediumFertilizer, null, Scale);
                _button.Click += Button_Click; ;
            }

            private static void Button_Click(object sender, EventArgs e)
            {
                Utils.CurrentObjectSelected = UsableObject.MediumFertilizer;
            }

            public override void Update(GameTime gameTime)
            {
                _button.Update(gameTime);

                UpdatePercentCircle(gameTime);
            }

            public override void Draw(SpriteBatch spriteBatch)
            {
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate,
                    BlendState.AlphaBlend);
                _button.Draw(spriteBatch);
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate,
                    BlendState.AlphaBlend);
                DrawShader(spriteBatch);
            }

            private void DrawShader(SpriteBatch spriteBatch)
            {
                if (!MediumFertilizer.CooldownUp())
                {
                    Effect.CurrentTechnique.Passes[0].Apply();
                    Effect.Parameters["percent"].SetValue(_percentCircle);
                    Effect.Parameters["R"].SetValue(TheColor.X);
                    Effect.Parameters["G"].SetValue(TheColor.Y);
                    Effect.Parameters["B"].SetValue(TheColor.Z);
                    Effect.Parameters["A"].SetValue(TheColor.W);
                    spriteBatch.Draw(CooldownCircle,
                        new Vector2(
                            Position.X - ((SCALE_DIFFERENCE_COOLDOWN / 2) * CooldownCircle.Width),
                            Position.Y - ((SCALE_DIFFERENCE_COOLDOWN / 2) * CooldownCircle.Height)),
                        null,
                        Color.White, 0f, Vector2.Zero, Scale + SCALE_DIFFERENCE_COOLDOWN, SpriteEffects.None, 1);
                }
            }

            private void UpdatePercentCircle(GameTime gameTime)
            {
                if (!MediumFertilizer.CooldownUp())
                {
                    if (_percentCircle > 0)
                    {
                        _percentCircle -= _percentCirclePerUpdate * gameTime.ElapsedGameTime.Milliseconds;
                        if (_percentCircle <= 0)
                            _percentCircle = 1f;
                    }
                    else
                    {
                        _percentCircle = 1f;
                    }
                }
            }
        }

        private class LargeFertilizerButton : ObjectButton
        {
            public LargeFertilizerButton(Vector2 position, float scale) : base(position, Assets.EffectLarge, scale)
            {
                _percentCirclePerUpdate = _percentCircle / LargeFertilizer.CurrentCooldown;
                _button = new Button(Position, Assets.IconeLargeFertilizer, null, Scale);
                _button.Click += Button_Click; ;
            }

            private static void Button_Click(object sender, EventArgs e)
            {
                Utils.CurrentObjectSelected = UsableObject.LargeFertilizer;
            }

            public override void Update(GameTime gameTime)
            {
                _button.Update(gameTime);

                UpdatePercentCircle(gameTime);
            }

            public override void Draw(SpriteBatch spriteBatch)
            {
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate,
                    BlendState.AlphaBlend);
                _button.Draw(spriteBatch);
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate,
                    BlendState.AlphaBlend);
                DrawShader(spriteBatch);
            }

            private void DrawShader(SpriteBatch spriteBatch)
            {
                if (!LargeFertilizer.CooldownUp())
                {
                    Effect.CurrentTechnique.Passes[0].Apply();
                    Effect.Parameters["percent"].SetValue(_percentCircle);
                    Effect.Parameters["R"].SetValue(TheColor.X);
                    Effect.Parameters["G"].SetValue(TheColor.Y);
                    Effect.Parameters["B"].SetValue(TheColor.Z);
                    Effect.Parameters["A"].SetValue(TheColor.W);
                    spriteBatch.Draw(CooldownCircle,
                        new Vector2(
                            Position.X - ((SCALE_DIFFERENCE_COOLDOWN / 2) * CooldownCircle.Width),
                            Position.Y - ((SCALE_DIFFERENCE_COOLDOWN / 2) * CooldownCircle.Height)),
                        null,
                        Color.White, 0f, Vector2.Zero, Scale + SCALE_DIFFERENCE_COOLDOWN, SpriteEffects.None, 1);
                }
            }

            private void UpdatePercentCircle(GameTime gameTime)
            {
                if (!LargeFertilizer.CooldownUp())
                {
                    if (_percentCircle > 0)
                    {
                        _percentCircle -= _percentCirclePerUpdate * gameTime.ElapsedGameTime.Milliseconds;
                        if (_percentCircle <= 0)
                            _percentCircle = 1f;
                    }
                    else
                    {
                        _percentCircle = 1f;
                    }
                }
            }
        }

        private class RockButton : ObjectButton
        {
            public RockButton(Vector2 position, float scale) : base(position, Assets.EffectRock, scale)
            {
                _percentCirclePerUpdate = _percentCircle / Rock.CurrentCooldown;
                _button = new Button(Position, Assets.IconeRock, null, Scale);
                _button.Click += Button_Click; ;
            }

            private static void Button_Click(object sender, EventArgs e)
            {
                Utils.CurrentObjectSelected = UsableObject.Rock;
            }

            public override void Update(GameTime gameTime)
            {
                _button.Update(gameTime);

                UpdatePercentCircle(gameTime);
            }

            public override void Draw(SpriteBatch spriteBatch)
            {
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate,
                    BlendState.AlphaBlend);
                _button.Draw(spriteBatch);
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate,
                    BlendState.AlphaBlend);
                DrawShader(spriteBatch);
            }

            private void DrawShader(SpriteBatch spriteBatch)
            {
                if (!Rock.CooldownUp())
                {
                    Effect.CurrentTechnique.Passes[0].Apply();
                    Effect.Parameters["percent"].SetValue(_percentCircle);
                    Effect.Parameters["R"].SetValue(TheColor.X);
                    Effect.Parameters["G"].SetValue(TheColor.Y);
                    Effect.Parameters["B"].SetValue(TheColor.Z);
                    Effect.Parameters["A"].SetValue(TheColor.W);
                    spriteBatch.Draw(CooldownCircle,
                        new Vector2(
                            Position.X - ((SCALE_DIFFERENCE_COOLDOWN / 2) * CooldownCircle.Width),
                            Position.Y - ((SCALE_DIFFERENCE_COOLDOWN / 2) * CooldownCircle.Height)),
                        null,
                        Color.White, 0f, Vector2.Zero, Scale + SCALE_DIFFERENCE_COOLDOWN, SpriteEffects.None, 1);
                }
            }

            private void UpdatePercentCircle(GameTime gameTime)
            {
                if (!Rock.CooldownUp())
                {
                    if (_percentCircle > 0)
                    {
                        _percentCircle -= _percentCirclePerUpdate * gameTime.ElapsedGameTime.Milliseconds;
                        if (_percentCircle <= 0)
                            _percentCircle = 1f;
                    }
                    else
                    {
                        _percentCircle = 1f;
                    }
                }
            }
        }

        #endregion
    }
}