using PajamaNinja.Common;
using PajamaNinja.CurrencySystem;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PajamaNinja.Scripts.IdleBarber
{
    public class PriceManager : SingleReference<PriceManager>
    {
        [SerializeField] private CurrencySSO _currencySSO;
        [SerializeField] private int _startMoney;
        [SerializeField] private List<PriceData> _currencyGivers;

        protected override void Awake()
        {
            base.Awake();

            if (!_currencySSO.IsSaveExists)
            {
                _currencySSO.CurrencyCount = _startMoney;
            }
        }

        [Serializable]
        public struct PriceData
        {
            [HorizontalGroup, HideLabel] public CurrencyGiver CurrencyGiver;
            [HorizontalGroup] public int Payment;
            [HorizontalGroup] public int BanknotesCount;
        }

        public bool TryGetPrice(CurrencyGiver currencyGiver, out PriceData data)
        {
            data = _currencyGivers.FirstOrDefault(o => o.CurrencyGiver == currencyGiver);
            return _currencyGivers.Any(o => o.CurrencyGiver == currencyGiver);
        }

        public bool TryGetPrice(GameObject currencyGiver, out PriceData data)
        {
            data = _currencyGivers.FirstOrDefault(o => o.CurrencyGiver == currencyGiver);
            return _currencyGivers.Any(o => o.CurrencyGiver == currencyGiver);
        }
    }
}