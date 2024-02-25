using PajamaNinja.Scripts.IdleBarber.LevelLogic;
using PajamaNinja.Scripts.IdleBarber.Unlocking;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace PajamaNinja.Scripts.CurrencySystem
{
    public class Unlocker : BaseUnlocker
    {
        [SerializeField, ValidateInput("HasIUnlockingObject", "GO must have any 'IUnlockingObject' component")]
        private GameObject[] _coupledObjects;

        protected override void BeforeStart()
        {
            foreach (GameObject obj in _coupledObjects)
            {
                if (obj.GetComponent<UnlockingArea>() == null)
                {
                    obj.SetActive(false);
                }
            }
        }

        protected override void ActivateDependency(bool forFirstTime) =>
            ActivateCoupledGameObject(forFirstTime);

        private void ActivateCoupledGameObject(bool forFirstTime)
        {
            foreach (GameObject coupledObject in _coupledObjects)
            {
                if (coupledObject.TryGetComponent(out IUnlockingObject unlockingObject) == false)
                    throw new Exception(
                        $"Unlocking gameObject {coupledObject.name} doesn't contain any 'IUnlockingObject' component");

                if (forFirstTime)
                    unlockingObject.UnlockForFirstTime();
                else
                    unlockingObject.Unlock();

                coupledObject.SetActive(true);
            }
        }
    }
}