using System.Collections;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace PajamaNinja.CurrencySystem
{
    [AddComponentMenu("#QBERA/UICurrency")]
    public class UICurrency : MonoBehaviour
    {
        [TabGroup("Tabs", "Settings")]
        [SerializeField]
        private CurrencySSO _currency;

        [TabGroup("Tabs", "Components")]
        [SerializeField]
        private TMP_Text _amountText;

        private Coroutine _smoothValueChangeProcess;

        private void OnEnable() => _currency.OnCurrencyValueChange.AddListener(SetAmount);

        private void OnDisable() => _currency.OnCurrencyValueChange.RemoveListener(SetAmount);

        private void Start() => InitAmount(_currency.CurrencyCount);

        private void InitAmount(int amount) => _amountText.text = amount.ToString();

        private void SetAmount(int previousAmount, int amount)
        {
            _amountText.text = previousAmount.ToString();
            WaitAndHide(previousAmount, amount);
        }

        private void WaitAndHide(int previousAmount, int amount)
        {
            if (_smoothValueChangeProcess != null)
                StopCoroutine(_smoothValueChangeProcess);
            
            _smoothValueChangeProcess = StartCoroutine(WaitAndHideCoroutine(previousAmount, amount, 1f));
        }

        IEnumerator WaitAndHideCoroutine(int previousAmount, int targetAmount, float delay)
        {
            const int ITERATIONS_COUNT = 100;
            float timeStep = delay / ITERATIONS_COUNT;

            float t = Time.time + delay;
            while (Time.time < t)
            {
                int amount = (int)Mathf.Lerp(previousAmount, targetAmount, 1f - (t - Time.time) / delay);
                _amountText.text = amount.ToString();

                yield return new WaitForSeconds(timeStep);
            }
            _amountText.text = targetAmount.ToString();
            yield return new WaitForSeconds(0.5f);
        }
    }
}