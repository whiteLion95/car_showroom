using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class ColorHandler : MonoBehaviour
    {
        [SerializeField] private float changeSmoothness = 0.5f;
        [SerializeField] private Color targetColor;
        [SerializeField] private List<Renderer> renderers;
        [SerializeField] private List<int> materialsIndexes;

        private List<Color> _startingColors;
        private List<Material> _materials;

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            _materials = new List<Material>();
            _startingColors = new List<Color>();

            for (int i = 0; i < materialsIndexes.Count; i++)
            {
                int index = materialsIndexes[i];

                for (int k = 0; k < renderers.Count; k++)
                {
                    if (index < renderers[k].materials.Length)
                    {
                        Material mat = renderers[k].materials[index];
                        _materials.Add(mat);
                        _startingColors.Add(mat.color);
                    }
                }
            }
        }

        /// <summary>
        /// Sets color of changing material
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color)
        {
            foreach (Material material in _materials)
            {
                material.DOColor(color, changeSmoothness);
            }
        }

        public void SetColor()
        {
            SetColor(targetColor);
        }

        /// <summary>
        /// Resets color of changing material to the starting color. It can be invoked in your OnDisable method
        /// </summary>
        public void ResetColor()
        {
            for (int i = 0; i < _materials.Count; i++)
            {
                _materials[i].color = _startingColors[i];
            }
        }
    }
}
