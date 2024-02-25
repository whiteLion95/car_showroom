using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PajamaNinja.Scripts.Extensions
{
    public static class AnimatorExtensions
    {
        public static UniTask AwaitAnimatorState(this Animator target, string stateName, CancellationToken ct, int layerIndex = 0)
        {
            return UniTask.WaitUntil(() => target.GetCurrentAnimatorStateInfo(layerIndex).IsName(stateName),
                cancellationToken: ct);
        }
    }
}