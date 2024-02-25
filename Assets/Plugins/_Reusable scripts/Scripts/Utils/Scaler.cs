using DG.Tweening;
using System;
using UnityEngine;

namespace Utils
{
    [Serializable]
    public class Scaler
    {
        [SerializeField] private ScalerData _data;
        [SerializeField] private Transform _scaledTransform;

        public static Action<float> OnScale = delegate { };

        public void SetTriggersCount(int increaseTriggers, int decreaseTriggers)
        {
            _data.IncreaseTriggers = increaseTriggers;
            _data.DecreaseTriggers = decreaseTriggers;
        }

        public void Scale(bool increase)
        {
            Vector3 newScale;

            if (increase)
            {
                newScale = _scaledTransform.localScale + _data.IncreaseStep;
                if (newScale.x > _data.MaxScale.x) newScale = _data.MaxScale;
            }
            else
            {
                newScale = _scaledTransform.localScale - _data.DecreaseStep;
                if (newScale.x < _data.MinScale.x) newScale = _data.MinScale;
            }

            float scalePct = (newScale.x / _scaledTransform.localScale.x) - 1f;
            OnScale?.Invoke(scalePct);

            _scaledTransform.DOScale(newScale, _data.ScaleSmoothness).SetEase(_data.ScaleEaseType);
        }
    }
}