using DG.Tweening;
using PajamaNinja.Scripts.Extensions;
using UnityEngine;

namespace PajamaNinja.Scripts.IdleBarber.Unlocking
{
    public class UnlockingDecoration : MonoBehaviour, IUnlockingObject
    {
        [SerializeField] private GameObject _visualModel;
        public GameObject GameObject => gameObject;

        public void Unlock() =>
            _visualModel.transform.localScale = Vector3.one;

        public void UnlockForFirstTime() =>
            _visualModel.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack)
                .OnComplete(() => gameObject.TriggerChildNavMeshObstacles());

        private void Awake()
        {
            _visualModel.transform.localScale = Vector3.zero;
        }
    }
}