using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace PajamaNinja.Scripts.IdleBarber.General.Tutor.TutorViewLogic
{
    [RequireComponent(typeof(RectTransform))]
    public class TutorPointer : MonoBehaviour
    {
        [SerializeField] private RectTransform _pointerTransform;
        [SerializeField] private Image _pointerImage;
        private RectTransform _transform;
        private Vector3 _pointerDefaultLocalPosition;
        private Vector3 _pointerDefaultLocalScale;
        private Sequence _pointerAnimationSequence;

        private void Awake()
        {
            _transform = GetComponent<RectTransform>();
            _pointerDefaultLocalPosition = _pointerTransform.localPosition;
            _pointerDefaultLocalScale = _pointerTransform.localScale;
            gameObject.SetActive(false);
        }

        public void Show(Vector3 position, float rotationZ)
        {
            gameObject.SetActive(true);
            _transform.position = position;
            _transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
            _pointerImage.color = new Color(1f, 1f, 1f, 0f);
            _pointerImage.DOFade(1, 0.5f);
            _pointerAnimationSequence = DOTween.Sequence();
            _pointerAnimationSequence.Append(_pointerTransform.DOScale(1.1f, 0.5f));
            _pointerAnimationSequence.Join(_pointerTransform.DOLocalMoveY(_pointerDefaultLocalPosition.y - 20, 0.5f));
            _pointerAnimationSequence.Append(_pointerTransform.DOScale(1f, 0.5f));
            _pointerAnimationSequence.Join(_pointerTransform.DOLocalMoveY(_pointerDefaultLocalPosition.y, 0.5f));
            _pointerAnimationSequence.SetDelay(1f);
            _pointerAnimationSequence.SetLoops(-1);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            _pointerAnimationSequence?.Kill();
            _pointerTransform.localPosition = _pointerDefaultLocalPosition;
            _pointerTransform.localScale = _pointerDefaultLocalScale;
        }
    }
}