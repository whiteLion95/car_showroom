using DG.Tweening;
using System;
using UnityEngine;

namespace Utils
{
    public class FadingDecal : MonoBehaviour
    {
        [SerializeField] private float fadingSmoothness = 1f;

        public Action<FadingDecal> OnFadeComplete;

        private SpriteRenderer _sprite;

        private void Awake()
        {
            _sprite = GetComponentInChildren<SpriteRenderer>();
        }

        public void OnEnable()
        {
            StartFading();
        }

        public void OnDisable()
        {
            ResetSpriteTransparency();
        }

        private void StartFading()
        {
            _sprite.DOFade(0f, fadingSmoothness).onComplete +=
                () => OnFadeComplete?.Invoke(this);
        }

        private void ResetSpriteTransparency()
        {
            Color tempColor = _sprite.color;
            tempColor.a = 1;
            _sprite.color = tempColor;
        }
    }
}