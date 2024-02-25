using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace PajamaNinja.Scripts.Extensions
{
    public static class MixExtensions
    {
        public static void TriggerChildNavMeshObstacles(this GameObject go) =>
            go.GetComponentsInChildren<NavMeshObstacle>().ToList().ForEach(o =>
            {
                o.enabled = false;
                o.enabled = true;
            });
    }
}