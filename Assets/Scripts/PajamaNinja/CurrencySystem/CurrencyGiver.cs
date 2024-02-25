using PajamaNinja.Scripts.IdleBarber;
using UnityEngine;

namespace PajamaNinja.CurrencySystem
{
    public class CurrencyGiver : MonoBehaviour
    {
        [SerializeField] private CurrencyStack _currencyStack;

        private PriceManager _priceManager;

        public CurrencyStack CurrencyStack => _currencyStack;

        private void Start()
        {
            _priceManager = PriceManager.Instance;
        }

        public void SpawnMoney()
        {
            var spawnPos = _currencyStack.transform.position + Vector3.up * 3;
            if (_priceManager.TryGetPrice(this, out PriceManager.PriceData priceData))
            {
                _currencyStack.AddCurrency(spawnPos, priceData.Payment, priceData.BanknotesCount);
            }
        }

        public void SpawnMoney(int payment)
        {
            var spawnPos = _currencyStack.transform.position + Vector3.up * 3;
            if (_priceManager.TryGetPrice(this, out PriceManager.PriceData priceData))
            {
                _currencyStack.AddCurrency(spawnPos, payment, priceData.BanknotesCount);
            }
        }
    }
}