using System;
using UnityEngine;

namespace PajamaNinja.Scripts.Extensions
{
    public static class EaseExtension
    {
        public static float InSine (float x) => 
            1 - Mathf.Cos((x * Mathf.PI) / 2);
        
        public static float OutSine (float x) => 
            Mathf.Cos((x * Mathf.PI) / 2);
        
        public static float InOutSine (float x) => 
            -(Mathf.Cos(Mathf.PI * x) - 1) / 2;
        
        public static float InQuad (float x) => 
            x * x;
        
        public static float OutQuad (float x) => 
            1 - (1 - x) * (1 - x);
        
        public static float InOutQuad (float x) => 
            x < 0.5 ? 2 * x * x : 1 - Mathf.Pow(-2 * x + 2, 2) / 2;
        
        public static float InCubic (float x) => 
            x * x * x;
        
        public static float OutCubic (float x) => 
            1 - Mathf.Pow(1 - x, 3);

        public static float InOutCubic(float x) =>
            x < 0.5 ? 4 * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 3) / 2;

        public static float InQuart (float x) => 
            x * x * x * x;
        
        public static float OutQuart (float x) => 
            1 - Mathf.Pow(1 - x, 4);
        
        public static float InOutQuart (float x) => 
            x < 0.5 ? 8 * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 4) / 2;
        
        public static float OutExpo(float x) => 
            x >= 1 ? 1 : 1 - Mathf.Pow(2, -10 * x);

        public static float EvaluateEase(float x, Type type)
        {
            switch (type)
            {
                case Type.Linear: return x;
                case Type.OutExpo: return OutExpo(x);
                case Type.InSine: return InSine(x);
                case Type.OutSine: return OutSine(x);
                case Type.InOutSine: return InOutSine(x);
                case Type.InQuad: return InQuad(x);
                case Type.OutQuad: return OutQuad(x);
                case Type.InOutQuad: return InOutQuad(x);
                case Type.InCubic: return InCubic(x);
                case Type.OutCubic: return OutCubic(x);
                case Type.InOutCubic: return InOutCubic(x);
                case Type.InQuart: return InQuart(x);
                case Type.OutQuart: return OutQuart(x);
                case Type.InOutQuart: return InOutQuart(x);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public enum Type
        {
            Linear = 0,
            InSine = 1,
            OutSine = 2,
            InOutSine = 3,
            InQuad = 4,
            OutQuad = 5,
            InOutQuad = 6,
            InCubic = 7,
            OutCubic = 8,
            InOutCubic = 9,
            InQuart = 10,
            OutQuart = 11,
            InOutQuart = 12,
            
            OutExpo,
        }
    }
}