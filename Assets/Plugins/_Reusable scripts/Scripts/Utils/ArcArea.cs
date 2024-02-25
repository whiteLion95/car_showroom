using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToolBox.Utils
{
    public class ArcArea : MonoBehaviour
    {
        [SerializeField] [Tooltip("Offset position of the arc source")] private Vector3 _offsetPosition;
        [SerializeField] private float _angleMin = -60f;
        [SerializeField] private float _angleMax = 60f;
        [SerializeField] private float _range = 3f;
        [SerializeField] private Transform _sourceTransform;
        [Header("Gizmos")]
        [SerializeField] private float _drawSegments = 20f;
        [SerializeField] private Color _drawColor = Color.red;

        public float Range { get => _range; set => _range = value; }

        public void OnDrawGizmos()
        {
            if (_sourceTransform != null)
            {
                List<Vector2> arcPoints = new List<Vector2>();
                Vector3 position = Position();
                float angle;
                float arcLength;
                angle = AngleMin();
                arcLength = AngleMax() - angle;
                for (int i = 0; i <= _drawSegments; i++)
                {
                    float x = Mathf.Sin(Mathf.Deg2Rad * angle) * _range;
                    float y = Mathf.Cos(Mathf.Deg2Rad * angle) * _range;

                    arcPoints.Add(new Vector2(x, y));

                    angle += (arcLength / _drawSegments);
                }

                Gizmos.color = _drawColor;
                var arcPos = new Vector3(position.x + arcPoints[0].x, position.y, position.z + arcPoints[0].y);
                var prevPos = arcPos;
                Gizmos.DrawLine(position, arcPos);

                for (int i = 0; i < arcPoints.Count; ++i)
                {
                    arcPos = new Vector3(position.x + arcPoints[i].x, position.y, position.z + arcPoints[i].y);
                    Gizmos.DrawLine(prevPos, arcPos);
                    prevPos = arcPos;
                }
                arcPos = new Vector3(position.x + arcPoints[arcPoints.Count - 1].x, position.y, position.z + arcPoints[arcPoints.Count - 1].y);
                Gizmos.DrawLine(position, arcPos);
            }
        }

        private float AngleMin()
        {
            return _angleMin + _sourceTransform.eulerAngles.y;
        }

        private float AngleMax()
        {
            return _angleMax + _sourceTransform.eulerAngles.y;
        }

        private Vector3 Position()
        {
            return _sourceTransform.position + _offsetPosition;
        }

        private Plane MinPlane()
        {
            var position = Position();

            float aMin = AngleMin();
            Vector3 vMin = new Vector3(Mathf.Sin(aMin * Mathf.Deg2Rad), 0, Mathf.Cos(aMin * Mathf.Deg2Rad));

            Vector3 normal = Vector3.Cross(vMin, Vector3.down);

            return new Plane(normal, position);
        }

        private Plane MaxPlane()
        {
            var position = Position();

            float aMax = AngleMax();
            Vector3 vMax = new Vector3(Mathf.Sin(aMax * Mathf.Deg2Rad), 0, Mathf.Cos(aMax * Mathf.Deg2Rad));

            Vector3 normal = Vector3.Cross(vMax, Vector3.up);

            return new Plane(normal, position);
        }

        public bool IsInRange(Vector3 point)
        {
            var position = Position();
            float dist = Vector3.Distance(position, point);
            if (dist <= Range)
            {
                var minP = MinPlane();
                var maxP = MaxPlane();
                var direction = point - position;

                if (Vector3.Dot(minP.normal, direction) > 0 && Vector3.Dot(maxP.normal, direction) > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsInRange(Vector3 point, out Vector3 outPoint)
        {
            outPoint = point;
            return IsInRange(point);
        }
    }
}