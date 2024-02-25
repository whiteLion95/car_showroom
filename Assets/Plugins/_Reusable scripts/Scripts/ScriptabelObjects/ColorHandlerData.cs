using UnityEngine;

namespace Utils
{
    [CreateAssetMenu(fileName = "ColorHandlerData", menuName = "ScriptableObjects/Utils/ColorHandlerData")]

    public class ColorHandlerData : ScriptableObject
    {
        [SerializeField] private Material _changingMaterial;
        [SerializeField] private float _changingColorSmoothness;

        public Material ChangingMaterial { get { return _changingMaterial; } }
        public float ChangingColorSmoothness { get { return _changingColorSmoothness; } }
    }
}
