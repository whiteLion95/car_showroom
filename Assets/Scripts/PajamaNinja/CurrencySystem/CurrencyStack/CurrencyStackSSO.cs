using System;
using UnityEngine;
using PajamaNinja.SaveSystem;

namespace PajamaNinja.CurrencySystem
{
    [Serializable]
    public class CurrencyStackData : SaveDataInSO
    {
        public int CurrencyAmount;
        public int BanknotesCount;
    }

    [CreateAssetMenu(fileName = "CurrencyStackSSO", menuName = "QBERA/Saveables/CurrencyStackSSO", order = 3)]
    public class CurrencyStackSSO : SaveableSO<CurrencyStackData>
    {
        public int CurrencyAmount
        {
            get
            {
                TryLoad();
                return _saveData.CurrencyAmount;
            }

            set
            {
                if (_saveData.CurrencyAmount == value)
                    return;

                _saveData.CurrencyAmount = value;
                TrySave(false);
            }
        }

        public int BanknotesCount
        {
            get
            {
                TryLoad();
                return _saveData.BanknotesCount;
            }

            set
            {
                if (_saveData.BanknotesCount == value)
                    return;

                _saveData.BanknotesCount = value;
                TrySave(false);
            }
        }
    }
}
