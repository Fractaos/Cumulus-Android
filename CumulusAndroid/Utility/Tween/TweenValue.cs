using System;

namespace CumulusGame.Utility.Tween
{
    public class TweenValue
    {
        public float Value;
        public float Time;
        public float Begin;
        public float Change;
        public float Duration;
        public EaseFunction Function;

        public Action<float> Functor;

        public bool Used, ReadyToRemove;

        public TweenValue()
        {
            Time = 0;
            Duration = 0;
            Function = EaseFunction.Linear;
            Used = ReadyToRemove = false;
        }

        public void Update(float time)
        {
            if (Time < Duration)
            {
                Time += time;
                Value = Ease.Easing(Time, Begin, Change, Duration, Function);
                Functor(Value);
            }

            if (Used && Time >= Duration)
            {
                ReadyToRemove = true;
            }
        }

        public void Move(float value, float _change, float _duration, EaseFunction _function, Action<float> _functor)
        {
            Functor = _functor;
            Begin = value;
            Change = _change - Begin;
            Duration = _duration;
            Function = _function;
            Time = 0;
            Used = true;
        }
    }
}