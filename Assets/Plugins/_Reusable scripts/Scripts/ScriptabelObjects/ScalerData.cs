using DG.Tweening;
using UnityEngine;

namespace Utils
{
    [CreateAssetMenu(fileName = "ScalerData", menuName = "ScriptableObjects/Utils/ScalerData")]
    public class ScalerData : ScriptableObject
    {
        [SerializeField] private Vector3 _minScale;
        [SerializeField] private Vector3 _maxScale;
        [SerializeField] private float _scaleSmoothness;
        [SerializeField] private Ease _scaleEaseType;

        private int _increaseTriggers = 0;
        private int _decreaseTriggers = 0;

        public Vector3 MinScale { get { return _minScale; } }
        public Vector3 MaxScale { get { return _maxScale; } }
        public float ScaleSmoothness { get { return _scaleSmoothness; } }
        public Ease ScaleEaseType { get { return _scaleEaseType; } }
        public int IncreaseTriggers { set { _increaseTriggers = value; } }
        public int DecreaseTriggers { set { _decreaseTriggers = value; } }
        public Vector3 IncreaseStep
        {
            get
            {
                if (_increaseTriggers > 0)
                {
                    Vector3 deltaScale = _maxScale - _minScale;
                    return deltaScale / _increaseTriggers;
                }
                else
                {
                    Debug.Log("Increase step equals " + Vector3.zero);
                    return Vector3.zero;
                }
            }
        }
        public Vector3 DecreaseStep
        {
            get
            {
                if (_decreaseTriggers > 0)
                {
                    Vector3 deltaScale = _maxScale - _minScale;
                    return deltaScale / _decreaseTriggers;
                }
                else
                {
                    Debug.Log("Decrease step equals " + Vector3.zero);
                    return Vector3.zero;
                }
            }
        }
    }
}