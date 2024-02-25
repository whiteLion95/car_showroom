using System;
using System.Collections.Generic;
using System.Linq;
//using Firebase.Analytics;
using PajamaNinja.Scripts.Common.AnalyticsLogic;
using PajamaNinja.UISystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PajamaNinja.UpgradesSystem
{
    [Serializable]
    public class UpgradeNode<T>
    {
        [HorizontalGroup("Alpha")]
        [LabelWidth(40)]
        public T Value;

        [HorizontalGroup("Alpha")]
        [LabelWidth(40)]
        public int Price;

        [HorizontalGroup("Alpha", Width = 0.1f)]
        [LabelWidth(80)]
        public bool IsDependent, IsCoupled;
        
        [PropertySpace]
        [TableList]
        [ShowIf("IsDependent")]
        public List<UpgradeDependency> DependsOn;
        
        [PropertySpace]
        [ShowIf("IsCoupled")]
        public List<UpgradeableVarSO> CoupledWith;
    }
    
    
    [Serializable]
    public class UpgradeDependency
    {
        public UpgradeableVarSO Upgrade;

        [TableColumnWidth(20)]
        public int Level;
    }

    public class UpgradeableVarSO<T> : UpgradeableVarSO where T : IComparable<T>, IEquatable<T>
    {
        [PropertySpace]
        [TabGroup("Tabs", "Settings")]
        [ListDrawerSettings(ShowFoldout = true, AddCopiesLastElement = true, NumberOfItemsPerPage = 30)]
        [SerializeField]
        private List<UpgradeNode<T>> _upgrades;

        [TabGroup("Tabs", "Settings")]
        [SerializeField]
        private bool IgnoreCurrency;

        [Button]
        public override bool TryUpgrade()
        {
            if (!NextUpgradeExists || !NextUpgradeUnlocked || !NextUpgradeEnoughCurrency) return false;

            if (!IgnoreCurrency)
                _upgradeCurrency.CurrencyCount -= NextUpgrade.Price;
            
            if (NextUpgrade.IsCoupled)
                NextUpgrade.CoupledWith.ForEach(up => up.TryUpgrade());
            
            UpgradeLevel += 1;

            OnUpgrade.Invoke();

            return true;
        }

        [Button]
        public void ForceUpgrade()
        {
            if (!NextUpgradeExists) return;
            
            if (NextUpgrade.IsCoupled)
                NextUpgrade.CoupledWith.ForEach(up => up.TryUpgrade());
            
            UpgradeLevel += 1;
            
            OnUpgrade.Invoke();
        }
#if UNITY_EDITOR
        [ButtonGroup("Utilities/Buttons")]
        [Button]
        public void ResetSaveInv()
        {
            ResetSave();
            OnUpgrade.Invoke(); 
        }
#endif
        public virtual T CurrentValue => CurrentUpgrade.Value;
        
        public virtual T LastValue => Upgrades.Last().Value;
        
        public List<UpgradeNode<T>> Upgrades => _upgrades;

        public virtual UpgradeNode<T> NextUpgrade => UpgradeLevel + 1 >= _upgrades.Count ? null : _upgrades[UpgradeLevel + 1];
        
        public virtual UpgradeNode<T> CurrentUpgrade => UpgradeLevel >= _upgrades.Count ? null : _upgrades[UpgradeLevel];

        public override int UpgradesCount => Upgrades.Count;

        public override bool NextUpgradeExists => NextUpgrade != null;

        public override bool NextUpgradeUnlocked => !NextUpgrade.IsDependent || 
                                                    NextUpgrade.DependsOn.TrueForAll(item =>
                                                      item.Level <= item.Upgrade.UpgradeLevel);
        
        public override bool NextUpgradeEnoughCurrency => NextUpgradeExists && (_upgradeCurrency.CurrencyCount >= NextUpgrade.Price || IgnoreCurrency);
        
        public override bool NextUpgradeEnoughAndUnlocked => NextUpgradeEnoughCurrency && NextUpgradeUnlocked;

        public override long AllUpgradesPrice => Upgrades.Select(item => item.Price).Sum();
        
        public override long AllBoughtUpgradesPrice => Upgrades.Take(UpgradeLevel + 1).Select(item => item.Price).Sum();
    }
}