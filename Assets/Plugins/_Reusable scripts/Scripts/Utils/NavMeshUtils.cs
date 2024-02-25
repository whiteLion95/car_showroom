using UnityEngine;
using UnityEngine.AI;

namespace Utils
{
    public static class NavMeshUtils
    {
        public static bool PathExists(this NavMeshPath path, Vector3 fromPos, Vector3 toPos, int passableMask = -1)
        {
            path.ClearCorners();

            if (NavMesh.CalculatePath(fromPos, toPos, passableMask, path) == false)
                return false;

            return true;
        }

        public static float GetPathLength(this NavMeshPath path)
        {
            float lng = 0.0f;

            if ((path.status != NavMeshPathStatus.PathInvalid) && (path.corners.Length > 1))
            {
                for (int i = 1; i < path.corners.Length; ++i)
                {
                    lng += Vector3.Distance(path.corners[i - 1], path.corners[i]);
                }
            }

            return lng;
        }
    }
}