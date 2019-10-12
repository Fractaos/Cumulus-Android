using System;

namespace CumulusGame.Utility.Tween
{
    public static class Ease
    {
        #region private Methods

        public static float Easing(float t, float b, float c, float d, EaseFunction f)
        {
            switch (f)
            {
                case EaseFunction.Linear: return LinearTween(t, b, c, d);
                case EaseFunction.EaseInQuad: return EaseInQuad(t, b, c, d);
                case EaseFunction.EaseOutQuad: return EaseOutQuad(t, b, c, d);
                case EaseFunction.EaseInOutQuad: return EaseInOutQuad(t, b, c, d);
                case EaseFunction.EaseInCubic: return EaseInCubic(t, b, c, d);
                case EaseFunction.EaseOutCubic: return EaseOutCubic(t, b, c, d);
                case EaseFunction.EaseInOutCubic: return EaseInOutCubic(t, b, c, d);
                case EaseFunction.EaseInQuart: return EaseInQuart(t, b, c, d);
                case EaseFunction.EaseOutQuart: return EaseOutQuart(t, b, c, d);
                case EaseFunction.EaseInOutQuart: return EaseInOutQuart(t, b, c, d);
                case EaseFunction.EaseInQuint: return EaseInQuint(t, b, c, d);
                case EaseFunction.EaseOutQuint: return EaseOutQuint(t, b, c, d);
                case EaseFunction.EaseInOutQuint: return EaseInOutQuint(t, b, c, d);
                case EaseFunction.EaseInSine: return EaseInSine(t, b, c, d);
                case EaseFunction.EaseOutSine: return EaseOutSine(t, b, c, d);
                case EaseFunction.EaseInOutSine: return EaseInOutSine(t, b, c, d);
                case EaseFunction.EaseInExpo: return EaseInExpo(t, b, c, d);
                case EaseFunction.EaseOutExpo: return EaseOutExpo(t, b, c, d);
                case EaseFunction.EaseInOutExpo: return EaseInOutExpo(t, b, c, d);
                case EaseFunction.EaseInCirc: return EaseInCirc(t, b, c, d);
                case EaseFunction.EaseOutCirc: return EaseOutCirc(t, b, c, d);
                case EaseFunction.EaseInOutCirc: return EaseInOutCirc(t, b, c, d);
                default:
                    throw new ArgumentOutOfRangeException(nameof(f), f, null);
            }
        }

        private static float LinearTween(float t, float b, float c, float d)
        {
            return ((c * t) / d) + b;
        }

        private static float EaseInQuad(float t, float b, float c, float d)
        {
            t /= d;
            return ((c * t) * t) + b;
        }

        private static float EaseOutQuad(float t, float b, float c, float d)
        {
            t /= d;
            return ((-c * t) * (t - 2)) + b;
        }

        private static float EaseInOutQuad(float t, float b, float c, float d)
        {
            t /= d / 2;
            if (t < 1) return (((c / 2) * t) * t) + b;
            t--;
            return ((-c / 2) * (t * (t - 2) - 1)) + b;
        }

        private static float EaseInCubic(float t, float b, float c, float d)
        {
            t /= d;
            return (((c * t) * t) * t) + b;
        }

        private static float EaseOutCubic(float t, float b, float c, float d)
        {
            t /= d;
            t--;
            return (c * (((t * t) * t) + 1)) + b;
        }

        private static float EaseInOutCubic(float t, float b, float c, float d)
        {
            t /= d / 2;
            if (t < 1) return ((((c / 2) * t) * t) * t) + b;
            t -= 2;
            return ((c / 2) * (((t * t) * t) + 2)) + b;
        }

        private static float EaseInQuart(float t, float b, float c, float d)
        {
            t /= d;
            return ((((c * t) * t) * t) * t) + b;
        }

        private static float EaseOutQuart(float t, float b, float c, float d)
        {
            t /= d;
            t--;
            return (-c * ((((t * t) * t) * t) - 1)) + b;
        }

        private static float EaseInOutQuart(float t, float b, float c, float d)
        {
            t /= d / 2;
            if (t < 1) return (((((c / 2) * t) * t) * t) * t) + b;
            t -= 2;
            return ((-c / 2) * ((((t * t) * t) * t) - 2)) + b;
        }

        private static float EaseInQuint(float t, float b, float c, float d)
        {
            t /= d;
            return (((((c * t) * t) * t) * t) * t) + b;
        }

        private static float EaseOutQuint(float t, float b, float c, float d)
        {
            t /= d;
            t--;
            return (c * (((((t * t) * t) * t) * t) + 1)) + b;
        }

        private static float EaseInOutQuint(float t, float b, float c, float d)
        {
            t /= d / 2;
            if (t < 1) return ((((((c / 2) * t) * t) * t) * t) * t) + b;
            t -= 2;
            return ((c / 2) * (((((t * t) * t) * t) * t) + 2)) + b;
        }

        private static float EaseInSine(float t, float b, float c, float d)
        {
            return (float)(((-c * Math.Cos((t / d) * (Math.PI / 2))) + c) + b);
        }

        private static float EaseOutSine(float t, float b, float c, float d)
        {
            return (float)((c * Math.Sin((t / d) * (Math.PI / 2))) + b);
        }

        private static float EaseInOutSine(float t, float b, float c, float d)
        {
            return (float)(((-c / 2) * (Math.Cos((Math.PI * t) / d) - 1)) + b);
        }

        private static float EaseInExpo(float t, float b, float c, float d)
        {
            return (float)((c * Math.Pow(2, 10 * ((t / d) - 1))) + b);
        }

        private static float EaseOutExpo(float t, float b, float c, float d)
        {
            return (float)((c * (-Math.Pow(2, (-10 * t) / d) + 1)) + b);
        }

        private static float EaseInOutExpo(float t, float b, float c, float d)
        {
            t /= d / 2;
            if (t < 1) return (float)(((c / 2) * Math.Pow(2, 10 * (t - 1))) + b);
            t--;
            return (float)(((c / 2) * (-Math.Pow(2, (-10 * t)) + 2)) + b);
        }

        private static float EaseInCirc(float t, float b, float c, float d)
        {
            t /= d;
            return (float)((-c * (Math.Sqrt(1 - (t * t)) - 1)) + b);
        }

        private static float EaseOutCirc(float t, float b, float c, float d)
        {
            t /= d;
            t--;
            return (float)((c * Math.Sqrt(1 - (t * t))) + b);
        }

        private static float EaseInOutCirc(float t, float b, float c, float d)
        {
            t /= d / 2;
            if (t < 1) return (float)(((-c / 2) * (Math.Sqrt(1 - (t * t)) - 1)) + b);
            t -= 2;
            return (float)(((c / 2) * (Math.Sqrt(1 - (t * t)) + 1)) + b);
        }

        #endregion
    }
}