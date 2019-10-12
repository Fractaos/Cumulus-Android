using System;

namespace CumulusGame.Utility.Tween
{
    public class TweenValue
    {
        private float _value;
        private float _time;
        private float _begin;
        private float _change;
        private float _duration;
        private EaseFunction _function;

        private Action<float> _functor;

        private bool _used;

        public TweenValue()
        {
            _time = 0;
            _duration = 0;
            _function = EaseFunction.Linear;
            _used = false;
        }

        public void Update(float time)
        {
            if (_time < _duration)
            {
                _time += time;
                _value = Ease.Easing(_time, _begin, _change, _duration, _function);
                _functor(_value);
            }

            if (_used && _time >= _duration)
            {
            }
        }

        public void Move(float value, float change, float duration, EaseFunction function, Action<float> functor)
        {
            _functor = functor;
            _begin = value;
            _change = change - _begin;
            _duration = duration;
            _function = function;
            _time = 0;
            _used = true;
        }
    }
}