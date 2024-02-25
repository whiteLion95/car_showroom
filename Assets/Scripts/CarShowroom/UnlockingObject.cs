using DG.Tweening;
using PajamaNinja.CurrencySystem;
using PajamaNinja.Scripts.Extensions;
using PajamaNinja.Scripts.IdleBarber.Unlocking;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UnlockingObject : MonoBehaviour, IUnlockingObject
{
    [SerializeField] protected UnlockerSSO _unlockerData;
    [SerializeField] private List<GameObject> _models;

    public Action OnUnlocked;
    public Action OnUnlockedForFirstTime;

    public bool IsUnlocked => UnlockersManager.Instance.Get(_unlockerData).IsUnlocked;

    public void Unlock()
    {
        OnUnlocked?.Invoke();
    }

    public void UnlockForFirstTime()
    {
        foreach (var model in _models)
        {
            model.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutBack).From()
                .OnComplete(() =>
                {
                    model.gameObject.TriggerChildNavMeshObstacles();
                });
        }

        OnUnlockedForFirstTime?.Invoke();
    }
}
