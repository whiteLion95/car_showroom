using DG.Tweening;
using UnityEngine;

namespace PajamaNinja.CarShowRoom
{
    public class ActivationScaleAnimator : MonoBehaviour
    {
        [SerializeField] private float _activeScale = 1.05f;

        private Transform _transform;
        private Vector3 _initialScale;

        private void Awake()
        {
            _transform = transform;
            _initialScale = transform.localScale;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<Player>(out var _))
                Activate();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent<Player>(out var _))
            {
                Deactivate();
            }
        }

        private void Activate()
        {
            _transform.DOKill();
            _transform.DOScale(_activeScale, 0.35f).SetEase(Ease.OutCubic);
        }

        private void Deactivate()
        {
            _transform.DOKill();
            _transform.DOScale(_initialScale, 0.2f).SetEase(Ease.InCubic);
        }

    }
}
