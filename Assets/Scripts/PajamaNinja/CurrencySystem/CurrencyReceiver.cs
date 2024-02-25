using UnityEngine;

namespace PajamaNinja.CurrencySystem
{
    public abstract class CurrencyReceiver : MonoBehaviour
    {
        public abstract bool CanReceiveCurrency { get; }

        public abstract int TargetCurrencyAmount { get; }
        public abstract int CurrencyAmountLeft { get; }

        public abstract void ReceiveProgress(float deltaTime, float purchaseDuration, int currencyAmount, out int currencySpent);
    }
}
