using CumulusGame.Entity;
using CumulusGame.Entity.Fertilizers;
using CumulusGame.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CumulusGame.Utility
{
    public class Score
    {

        private int _points;
        private float _multiplicator;
        private Vector2 _position;

        private float _elapsedGameTime;

        public Score(Vector2 position)
        {
            _points = 0;
            _multiplicator = 1f;
            _position = position;
        }


        public void FertilizerEated(Fertilizer fertilizer)
        {
            switch (fertilizer)
            {
                case LittleFertilizer _:
                    _points += (int)(5 * _multiplicator);
                    _multiplicator += _multiplicator * 0.01f;
                    break;
                case MediumFertilizer _:
                    _points += (int)(10 * _multiplicator);
                    _multiplicator += _multiplicator * 0.05f;
                    break;
                case LargeFertilizer _:
                    _points += (int)(15 * _multiplicator);
                    _multiplicator += _multiplicator * 0.1f;
                    break;
            }
        }

        public void RockBreaked(Rock rock)
        {
            _points -= 30;
            _multiplicator = 1f;
        }

        public void Update(GameTime gametime)
        {
            _elapsedGameTime += gametime.ElapsedGameTime.Milliseconds;
            if (_elapsedGameTime >= 1000)
            {
                _elapsedGameTime = 0;
                _points++;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
#pragma warning disable CA1305 // Spécifier IFormatProvider
            spriteBatch.DrawString(Assets.Pixel18, _points.ToString(),
#pragma warning restore CA1305 // Spécifier IFormatProvider
                new Vector2(_position.X - (Assets.Pixel18.MeasureString(_points.ToString()).X / 2), _position.Y - (Assets.Pixel18.MeasureString(_points.ToString()).Y / 2)),
                Color.White);
        }
    }
}