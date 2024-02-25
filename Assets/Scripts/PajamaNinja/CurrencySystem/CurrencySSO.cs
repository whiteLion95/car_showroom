using System;
using UnityEngine;
using UnityEngine.Events;
using PajamaNinja.SaveSystem;
using Sirenix.OdinInspector;

namespace PajamaNinja.CurrencySystem
{
    [Serializable]
    public class CurrencySaveData : SaveDataInSO
    {
        public int CurrencyCount = 0;
    }

    [CreateAssetMenu(fileName = "CurrencySSO", menuName = "QBERA/Saveables/CurrencySSO", order = 0)]
    public class CurrencySSO : SaveableSO<CurrencySaveData>
    {
        [HideInInspector]
        public UnityEvent<int> OnCurrencyIncrease = new(), OnCurrencyDecrease = new();

        [HideInInspector]
        public UnityEvent<int, int > OnCurrencyValueChange = new ();

        public int CurrencyCount
        {
            get
            {
                TryLoad();
                return _saveData.CurrencyCount;
            }

            set
            {
                if (_saveData.CurrencyCount == value)
                    return;

                var oldValue = _saveData.CurrencyCount;

                _saveData.CurrencyCount = value;

                OnCurrencyValueChange.Invoke(oldValue, value);

                if (oldValue < value)
                    OnCurrencyIncrease.Invoke(value - oldValue);

                if (oldValue > value)
                    OnCurrencyDecrease.Invoke(oldValue - value);

                TrySave(false);
            }
        }

        [Button]
        public void ChangeMoney(int value) => CurrencyCount += value;
    }
}