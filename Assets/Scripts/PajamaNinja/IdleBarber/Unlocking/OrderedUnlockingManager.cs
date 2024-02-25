using System;
using System.Collections.Generic;
using PajamaNinja.UpgradesSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PajamaNinja.Scripts.IdleBarber.Unlocking
{
    public class OrderedUnlockingManager : MonoBehaviour
    {
        [TabGroup("Tabs", "Settings"), SerializeField] private IntUSO _unlockingUSO;
        [TabGroup("Tabs", "Components"), SerializeField] private List<UnlockingPair> _unlocks;

        private void Start()
        {
           InitUnlockPairs(true);

           _unlockingUSO.OnUpgrade.AddListener(() => InitUnlockPairs(false));
        }

        private void InitUnlockPairs(bool onInit)
        {
            for (int i = 0; i < _unlocks.Count; i++)
            {
                var unlockPair = _unlocks[i];
                bool isActiveInHierarchy = unlockPair.Target.activeInHierarchy;
                
                unlockPair.Target.SetActive(i < _unlockingUSO.CurrentValue);

                if (unlockPair.Target.TryGetComponent(out IUnlockingObject unlockingObject) == false)
                    throw new Exception(
                        $"Unlocking gameObject {unlockPair.Target.name} doesn't contain any 'IUnlockingObject' component");
                
                if (i >= _unlockingUSO.CurrentValue) continue;
                if (onInit == false && isActiveInHierarchy == false)
                {
                    unlockingObject.UnlockForFirstTime();
                    continue;
                }
                    
                unlockingObject.Unlock();
            }

            if (_unlockingUSO.NextUpgradeExists)
            {
                // var unlocker = _unlocks[_unlockingUSO.CurrentValue].Unlocker;
                // unlocker.Initialize(_unlockingUSO.CurrentUpgrade.Price);
                // unlocker.Unlocked += () => _unlockingUSO.TryUpgrade();
            }
        }
    }
}