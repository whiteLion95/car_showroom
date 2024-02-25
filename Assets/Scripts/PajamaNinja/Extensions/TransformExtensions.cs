using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PajamaNinja.Scripts.Extensions
{
    public static class TransformExtensions
    {
        public static async UniTaskVoid AwaitWhileDistanceMore(
            this Transform self, 
            Vector3 target, 
            float distanceThreshold,
            Action callback)
        {
            await UniTask.WaitWhile(() => Vector3.Distance(self.position, target) > distanceThreshold);
            callback.Invoke();
        }
    }
}