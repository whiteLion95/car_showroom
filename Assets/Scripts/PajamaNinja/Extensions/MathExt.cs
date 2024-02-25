using UnityEngine;

namespace Extensions
{
    public static class MathExt
    {
        public static float Map(this float value, float from1, float from2, float to1, float to2) => 
            Mathf.Lerp (to1, to2, Mathf.InverseLerp (from1, from2, value));

        public static bool InRange(this float target, float a, float b) => 
            target >= a && target <= b;
        /// <summary>
        /// Inclusive 'a' and exclusive 'b'
        /// </summary>
        /// <param name="target"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool InRange(this int target, int a, int b) => 
            target >= a && target < b;

        /// <summary>
        /// Возвращает точку на окружености радиусом 1
        /// </summary>
        /// <param name="normalizedPosition">, 0 и 1 - верхняя точка окружности, распределение по часовой стрелке</param>
        /// <returns></returns>
        public static Vector2 GetPointOnUnitCircle(float normalizedPosition)
        {
            float angle = 2 * Mathf.PI * normalizedPosition;
            return new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
        }

        public static Vector2 GetPointOnUnitSquare(float normalizedPosition)
        {
            float x = (QuadInOutEase(normalizedPosition) - 0.5f) * 2;
            float y = (QuadInOutEase(Mathf.Repeat(normalizedPosition + 0.25f, 1)) - 0.5f) * 2;

            float QuadInOutEase(float f) => 
                f < 0.5 
                    ? QuadOutEase(f.Map(0, 0.5f, 0, 1)) 
                    : QuadInEase(f.Map(0.5f, 1f, 0, 1));

            float QuadInEase(float f) => 
                1 - QuadOutEase(f);

            float QuadOutEase(float f) => 
                Mathf.Clamp01(f * 2);

            return new Vector2(x, y);
        }
    }
}