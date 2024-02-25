using DG.Tweening;
using PajamaNinja.Scripts.IdleBarber.Unlocking;
using System.Collections.Generic;
using UnityEngine;

namespace PajamaNinja.Scripts.IdleBarber.LevelLogic
{
    public class UnlockingArea : MonoBehaviour, IUnlockingObject
    {
        [SerializeField] private List<GameObject> _disableOnUnlock;
        [SerializeField] private List<GameObject> _interPointsToRegisterOnUnlock;

        public GameObject GameObject => gameObject;
        public bool Unlocked { get; private set; }

        public void UnlockForFirstTime()
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetAutoKill();
            DoDisableOnUnlock();
            Unlocked = true;
        }

        public void Unlock()
        {
            transform.localScale = Vector3.one;
            DoDisableOnUnlock();
            Unlocked = true;
        }

        private void DoDisableOnUnlock()
        {
            foreach (GameObject obj in _disableOnUnlock)
            {
                obj.SetActive(false);
            }
        }
    }
}