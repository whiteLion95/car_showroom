using DG.Tweening;
using System;
using UnityEngine;

namespace PajamaNinja.CarShowRoom
{
    public enum ConstructTweenType
    {
        Move,
        Rotate,
        None
    }

    public class CarConstructionPart : MonoBehaviour
    {
        [SerializeField] private ConstructTweenType _tweenType;
        [SerializeField] private Vector3 _startRelValue;

        private Vector3 _origLocalPos;
        private Vector3 _origLocalRot;
        private CarConstructionDataSO _data;

        public Action<Tween> OnTweenStarted;

        public ConstructTweenType TweenType => _tweenType;

        private void Awake()
        {
            _origLocalPos = transform.localPosition;
            _origLocalRot = transform.localRotation.eulerAngles;
            _data = GetComponentInParent<CarConstruction>().Data;
        }

        public void Init()
        {
            switch (_tweenType)
            {
                case ConstructTweenType.Move:
                    transform.localPosition += _startRelValue;
                    break;
                case ConstructTweenType.Rotate:
                    transform.Rotate(_startRelValue, Space.Self);
                    break;
            }
        }

        public void Tween(float duration, Ease ease = Ease.Linear, Action onComplete = null)
        {
            Tween tween = null;

            switch (_tweenType)
            {
                case ConstructTweenType.None:
                    onComplete?.Invoke();
                    return;
                case ConstructTweenType.Move:
                    tween = transform.DOLocalMove(_origLocalPos, duration).SetEase(ease);
                    break;
                case ConstructTweenType.Rotate:
                    tween = transform.DOLocalRotate(_origLocalRot, duration).SetEase(ease);
                    break;
            }

            //Invoke(nameof(PunchScale), duration - _data.OnPlaceScaleDuration);
            PunchScale();
            OnTweenStarted?.Invoke(tween);

            tween.onComplete += () =>
            {
                onComplete?.Invoke();
            };
        }

        private void PunchScale()
        {
            transform.DOScale(Vector3.one * _data.OnPlaceScaleValue, _data.PartTweenDuration / 2f).SetRelative(true).SetLoops(2, LoopType.Yoyo).SetEase(_data.OnPlaceScaleEase);
        }
    }
}