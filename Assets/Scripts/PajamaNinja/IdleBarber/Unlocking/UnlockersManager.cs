using PajamaNinja.Common;
using PajamaNinja.CurrencySystem;
using PajamaNinja.Scripts.CurrencySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PajamaNinja.Scripts.IdleBarber.Unlocking
{
    public class UnlockersManager : SingleReference<UnlockersManager>
    {
        public event Action<UnlockerInfo> AnyUnlocked;
        [SerializeField] private UnlockersList _list;
        private List<UnlockerInfo> _unlockers;
        private bool _isInitialized;

        //protected override void Awake()
        //{
        //    base.Awake();

        //    if (GetFirstLocked(out BaseUnlocker unlocker))
        //        unlocker.UnlockerInfo.SetAsAvailable();
        //}

        public UnlockerInfo Get(UnlockerSSO sso)
        {
            if (_isInitialized == false) Initialize();
            return _unlockers.First(u => u.Sso == sso);
        }

        public void SetAllUnlockersVisibility(bool value)
        {
            foreach (UnlockerInfo unlocker in _unlockers)
            {
                unlocker.SetVisibility(value);
            }
        }

        private void Initialize()
        {
            _isInitialized = true;
            _unlockers = _list.Unlockers.Select(u => new UnlockerInfo(u)).ToList();

            for (var i = 0; i < _unlockers.Count; i++)
            {
                UnlockerInfo infoConfig = _unlockers[i];

                if (infoConfig.IsUnlocked && i < _unlockers.Count - 1 && !_unlockers[i + 1].IsUnlocked)
                {
                    _unlockers[i + 1].SetAsAvailable();
                }

                infoConfig.Unlocked += OnAnyUnlocked;
            }
        }

        private void OnAnyUnlocked(UnlockerInfo obj)
        {
            AnyUnlocked?.Invoke(obj);
            int index = _unlockers.IndexOf(obj);
            if (index < _unlockers.Count - 1)
                _unlockers[index + 1].SetAsAvailable();
        }

        public bool GetFirstLocked(out BaseUnlocker unlocker)
        {
            unlocker = null;

            foreach (var u in _unlockers)
            {
                if (!u.IsUnlocked)
                {
                    unlocker = u.GameInstance;
                    break;
                }
            }

            return unlocker != null;
        }
    }
}