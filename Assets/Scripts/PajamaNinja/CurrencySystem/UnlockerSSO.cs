using PajamaNinja.SaveSystem;
using System;
using UnityEngine;

namespace PajamaNinja.CurrencySystem
{
    [Serializable]
    public class UnlockerSaveData : SaveDataInSO
    {
        public bool IsUnlocked;
        public int SpentCurrency;
    }

    [CreateAssetMenu(fileName = "UnlockerSSO", menuName = "QBERA/Saveables/UnlockerSSO", order = 1)]
    public class UnlockerSSO : SaveableSO<UnlockerSaveData>
    {
        public bool IsUnlocked
        {
            get
            {
                TryLoad();
                return _saveData.IsUnlocked;
            }

            set
            {
                _saveData.IsUnlocked = value;
                TrySave(true);
            }
        }

        public int SpentCurrency
        {
            get
            {
                TryLoad();
                return _saveData.SpentCurrency;
            }

            set
            {
                _saveData.SpentCurrency = value;
                TrySave(false);
            }
        }
    }
}
