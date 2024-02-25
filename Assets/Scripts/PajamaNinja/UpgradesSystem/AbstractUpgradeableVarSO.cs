using System;
using PajamaNinja.CurrencySystem;
using PajamaNinja.SaveSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace PajamaNinja.UpgradesSystem
{
    [Serializable]
    public class UpgradeSaveData : SaveDataInSO
    {
        public string LockTime = "";
        
        public bool IsTimeLocked = false;

        public int UpgradeLevel = 0;
    }

    public abstract class UpgradeableVarSO : SaveableSO<UpgradeSaveData>
    {
        [LabelWidth(120)]
        [TabGroup("Tabs", "Settings")]
        [SerializeField]
        protected CurrencySSO _upgradeCurrency;
        
        [HideInInspector]
        public UnityEvent OnUpgrade = new UnityEvent();
        
        public virtual int UpgradeLevel
        {
            get
            {
                TryLoad();

                if (_saveData.UpgradeLevel >= UpgradesCount)
                    UpgradeLevel = UpgradesCount - 1;

                return _saveData.UpgradeLevel;
            }
            set
            {
                _saveData.UpgradeLevel = value;

                TrySave();
            } 
        }
        
        public abstract bool TryUpgrade();
        
        public abstract int UpgradesCount { get; }
        
        public abstract bool NextUpgradeExists { get; }
        
        public abstract bool NextUpgradeUnlocked { get; }
        
        public abstract bool NextUpgradeEnoughCurrency { get; }
        
        public abstract bool NextUpgradeEnoughAndUnlocked { get; }
        
        public abstract long AllUpgradesPrice { get; }
        
        public abstract long AllBoughtUpgradesPrice { get; }
    }
}