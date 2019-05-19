using Microsoft.Xna.Framework;

namespace CumulusGame.Utility.Tween
{
    public class TweenPosition
    {
        public Sprite Owner;
        public float Time;
        public Vector2 Begin;
        public Vector2 Change;
        public float Duration;
        public EaseFunction Function;

        public TweenPosition(Sprite sprite)
        {
            Owner = sprite;
            Time = 0;
            Begin = Owner.Position;
            Change = Vector2.Zero;
            Duration = 0;
            Function = EaseFunction.Linear;
        }

        public void Update(float time, ref Vector2 value)
        {
            if (Time < Duration)
            {
                Time += time;
                value.X = Ease.Easing(Time, Begin.X, Change.X, Duration, Function);
                value.Y = Ease.Easing(Time, Begin.Y, Change.Y, Duration, Function);
            }
        }

        public void Move(Vector2 _change, float _duration, EaseFunction _function = EaseFunction.Linear)
        {
            Begin = Owner.Position;
            Change = new Vector2(_change.X - Owner.Position.X, _change.Y - Owner.Position.Y);
            Duration = _duration;
            Function = _function;
            Time = 0;
        }
    }
}