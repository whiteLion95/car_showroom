using PajamaNinja.CurrencySystem;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace PajamaNinja.Scripts.CurrencySystem
{
    [Serializable]
    public class UnlockerInfoConfig
    {
        public int UnlockingOrder => _order;
        public UnlockerSSO Sso => _sso;
        public bool UnlockedByDefault => _unlockedByDefault;
        public int UnlockPrice => _unlockPrice;

        [SerializeField, HideLabel, HorizontalGroup("HG", Width = 35), Delayed] public int _order;
        [SerializeField, HideLabel, HorizontalGroup("HG", Width = 250)] private UnlockerSSO _sso;
        [SerializeField, LabelText("Unlocked"), HorizontalGroup("HG", Width = 80)] private bool _unlockedByDefault;
        [SerializeField, LabelText("Price"), HorizontalGroup("HG"), HideIf(nameof(_unlockedByDefault))] private int _unlockPrice;
    }

    public class UnlockerInfo
    {
        public event Action<UnlockerInfo> Unlocked;
        public event Action BecameAvailable;
        public event Action<bool> VisibilityChanged;

        public bool IsUnlocked
        {
            get => _config.Sso.IsUnlocked || _config.UnlockedByDefault;
            set
            {
                _config.Sso.IsUnlocked = value;
                Unlocked?.Invoke(this);
            }
        }
        public bool IsUnlockedByDefault
        {
            get => _config.UnlockedByDefault;
        }

        public int SpentCurrency
        {
            get => _config.Sso.SpentCurrency;
            set => _config.Sso.SpentCurrency = value;
        }
        public bool IsSaveExists => _config.Sso.IsSaveExists;
        public int UnlockPrice => _config.UnlockPrice;
        public bool IsAvailable => _isAvailable || IsUnlocked;
        public UnlockerSSO Sso => _config.Sso;
        public bool IsVisible { get; private set; }
        public BaseUnlocker GameInstance { get; private set; }

        private bool _isAvailable;
        private readonly UnlockerInfoConfig _config;

        public UnlockerInfo(UnlockerInfoConfig config)
        {
            _config = config;
            IsVisible = true;
        }

        public void SetAsAvailable()
        {
            if (_isAvailable) return;
            _isAvailable = true;
            BecameAvailable?.Invoke();
        }

        public void SetGameInstance(BaseUnlocker unlocker)
        {
            if (GameInstance != null)
                throw new Exception("Game instance already assigned");
            GameInstance = unlocker;
        }

        public void SetVisibility(bool value)
        {
            if (value == IsVisible) return;
            IsVisible = value;
            VisibilityChanged?.Invoke(value);
            //TODO visibility logic in BaseUnlocker.cs
        }
    }
}