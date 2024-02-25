using UnityEngine;

namespace Utils
{
    [CreateAssetMenu(fileName = "TimeData", menuName = "ScriptableObjects/Utils/TimeData")]
    public class TimeData : ScriptableObject
    {
        [SerializeField] private float _slowDownFactor = 0.05f;
        [SerializeField] private float _slowDownLength = 2f;

        public float SlowDownFactor { get => _slowDownFactor; }
        public float SlowDownLength { get => _slowDownLength; }
    }
}
