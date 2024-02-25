using UnityEngine;

namespace Utils
{
    [CreateAssetMenu(fileName = "ThreeWayMovementData", menuName = "ScriptableObjects/Utils/ThreeWayMovementData")]
    public class ThreeWayMovementData : ScriptableObject
    {
        [SerializeField] private float _runningSpeed;
        [SerializeField] private float _changeLineSpeed;
        [SerializeField] private float _distanceBetweenLines;
        [SerializeField] private float _stopChangingLineSpeed;

        public float MovementSpeed { get { return _runningSpeed; } set { _runningSpeed = value; } }
        public float ChangeLineSpeed { get { return _changeLineSpeed; } }
        public float DistanceBetweenLines { get { return _distanceBetweenLines; } }
        public float StopChangingLineSpeed { get { return _stopChangingLineSpeed; } }
    }
}
