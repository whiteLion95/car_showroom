using DG.Tweening;
using System;
using UnityEngine;

namespace ToolBox.Utils
{
    /// <summary>
    /// Provides a set of static methods for spatial transformations such as movements, rotations etc...
    /// </summary>
    public static class SpatialUtils
    {
        /// <summary>
        /// Turns transform with turnSpeed until it looks at the direction
        /// </summary>
        public static void TurnToDirection(this Transform t, Vector3 direction, Vector3 upWards, float turnSpeed)
        {
            if (direction != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(direction, upWards);
                float turnAngle = Vector3.Angle(t.forward, direction);
                float turnDuration = turnAngle / turnSpeed;
                t.DORotateQuaternion(toRotation, turnDuration);
            }
        }

        /// <summary>
        /// Moves transform to target position
        /// </summary>
        /// <param name="target">target postion</param>
        /// <param name="speed">move speed</param>
        /// <param name="offset">offset to target</param>
        /// <param name="onComplete">action on complete</param>
        public static void MoveTo(this Transform t, Vector3 target, float speed, float offset = 0f, Action onComplete = null)
        {
            float distance = Vector3.Distance(t.position, target);

            if (offset > 0f)
            {
                Vector3 normalDir = (target - t.position).normalized;
                target -= offset * normalDir;
            }

            t.DOMove(target, speed).SetSpeedBased(true).SetEase(Ease.Linear).onComplete += () =>
            {
                onComplete?.Invoke();
            };
        }

        /// <summary>
        /// Turns transform to look at direction every frame
        /// </summary>
        public static void SmoothLookAt(this Transform t, Vector3 direction, Vector3 upWards, float turnSpeed)
        {
            if (direction != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(direction, upWards);
                t.rotation = Quaternion.RotateTowards(t.rotation, toRotation, turnSpeed * Time.deltaTime);
            }
        }

        /// <summary>
        /// Turns transform to look at point every frame
        /// </summary>
        public static void SmoothLookAtPoint(this Transform t, Vector3 point, Vector3 upWards, float turnSpeed)
        {
            Vector3 lookDirection = (point - t.position).normalized;
            t.SmoothLookAt(lookDirection, upWards, turnSpeed);
        }

        /// <summary>
        /// Returns random position in area in yOffset from areaOrigin and within range of xWide and zWide
        /// </summary>
        public static Vector3 GetRandomPosInArea(Vector3 areaOrigin, float yOffset, float xWide, float zWide)
        {
            float randX = UnityEngine.Random.Range(-xWide / 2, xWide / 2);
            float randZ = UnityEngine.Random.Range(-zWide / 2, zWide / 2);
            Vector3 randPos = areaOrigin + new Vector3(randX, yOffset, randZ);

            return randPos;
        }
    }
}