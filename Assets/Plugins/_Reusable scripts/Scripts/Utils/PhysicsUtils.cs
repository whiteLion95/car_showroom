using UnityEngine;

namespace Utilities
{
    public static class PhysicsUtils
    {
        /// <summary>
        /// Add explosion force to all colliders at position with radius
        /// </summary>
        /// <param name="force">force to apply</param>
        /// <param name="pos">center of the explosion</param>
        /// <param name="radius">radius of the explosion</param>
        public static void Explode(float force, Vector3 pos, float radius)
        {
            Collider[] fragmentHits = Physics.OverlapSphere(pos, radius);

            for (int i = 0; i < fragmentHits.Length; i++)
            {
                if (fragmentHits[i].TryGetComponent(out Rigidbody rB))
                {
                    rB.AddExplosionForce(force, pos, radius);
                }
            }
        }

        /// <summary>
        /// Add explosion force to colliders with specified layerMask at position with radius
        /// </summary>
        /// <param name="force">force to apply</param>
        /// <param name="pos">center of the explosion</param>
        /// <param name="radius">radius of the explosion</param>
        /// <param name="layerMask">layers of colliders to apply force to</param>
        public static void Explode(float force, Vector3 pos, float radius, int layerMask)
        {
            Collider[] fragmentHits = Physics.OverlapSphere(pos, radius, layerMask);

            for (int i = 0; i < fragmentHits.Length; i++)
            {
                if (fragmentHits[i].TryGetComponent(out Rigidbody rB))
                {
                    rB.AddExplosionForce(force, pos, radius);
                }
            }
        }

        /// <summary>
        /// Add explosion force to colliders with specified layerMask at position with radius
        /// </summary>
        /// <param name="force">force to apply</param>
        /// <param name="pos">center of the explosion</param>
        /// <param name="radius">radius of the explosion</param>
        /// <param name="layerMask">layers of colliders to apply force to</param>
        /// <param name="qTrig">specifies whether apply force to triggers</param>
        public static void Explode(float force, Vector3 pos, float radius, int layerMask, QueryTriggerInteraction qTrig)
        {
            Collider[] fragmentHits = Physics.OverlapSphere(pos, radius, layerMask, qTrig);

            for (int i = 0; i < fragmentHits.Length; i++)
            {
                if (fragmentHits[i].TryGetComponent(out Rigidbody rB))
                {
                    rB.AddExplosionForce(force, pos, radius);
                }
            }
        }
    }
}