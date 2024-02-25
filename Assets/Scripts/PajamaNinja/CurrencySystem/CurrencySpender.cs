using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using Lean.Pool;
using PajamaNinja.Scripts.IdleBarber.Player;
using Sirenix.OdinInspector;
using Random = UnityEngine.Random;

namespace PajamaNinja.CurrencySystem
{
    [AddComponentMenu("#QBERA/CurrencySpender")]
    public class CurrencySpender : MonoBehaviour
    {
        public event Action OnSpendStarted;
        public event Action OnSpendEnded;

        [TabGroup("Tabs", "Settings")]
        [SerializeField]
        private CurrencySSO _currency;
        
        [TabGroup("Tabs", "Settings")]
        [SerializeField]
        private Transform _moneyPrefab;

        [TitleGroup("Tabs/Settings/Timings")]
        [SerializeField]
        private float _startDelay = 0.5f;
        
        [TitleGroup("Tabs/Settings/Timings")]
        [SerializeField]
        private float _purchaseDuration = 1.35f;

        [TabGroup("Tabs", "Components")]
        [SerializeField] 
        private Transform _moneySpendPoint;

        private bool IsSpending { get; set; }

        private CurrencyReceiver _currentCurrencyReceiver = null;

        public void StartSpendingMoney(CurrencyReceiver receiver, PlayerMovement player)
        {
            if (player.IsInputLocked)
                return;

            StopAllCoroutines();

            _currentCurrencyReceiver = receiver;
            StartCoroutine(SpendingMoneyCoroutine(receiver, player));
        }

        private IEnumerator SpendingMoneyCoroutine(CurrencyReceiver receiver, PlayerMovement player)
        {
            yield return new WaitWhile(() => player.IsMoving);
            yield return new WaitForSeconds(_startDelay);

            var startTime = Time.time;

            Coroutine animationCoroutine = null;
            while (receiver && receiver.CanReceiveCurrency && _currency.CurrencyCount > 0)
            {
                if (!IsSpending)
                {
                    IsSpending = true;
                    OnSpendStarted?.Invoke();
                }

                if (animationCoroutine == null)
                    animationCoroutine = StartCoroutine(SpendMoneyAnimation(receiver.transform.position));

                yield return null;

                if (player.IsInputLocked)
                {
                    startTime = Time.time;
                    continue;
                }
                
                var deltaTime = Time.time - startTime;

                receiver.ReceiveProgress(deltaTime, _purchaseDuration, _currency.CurrencyCount, out int moneySpend);
                SpendMoney(moneySpend);

                startTime = Time.time;
            }

            if (animationCoroutine != null)
                StopCoroutine(animationCoroutine);

            if (IsSpending)
            {
                IsSpending = false;
                OnSpendEnded?.Invoke();
            }

            _currentCurrencyReceiver = null;
        }

        private void SpendMoney(int spendAmount)
        {
            if (_currency.CurrencyCount >= spendAmount)
            {
                _currency.CurrencyCount -= spendAmount;
            }
        }

        IEnumerator SpendMoneyAnimation(Vector3 target)
        {
            while (true)
            {
                var money = LeanPool.Spawn(_moneyPrefab, _moneySpendPoint.position, Quaternion.identity);
                money.transform.localScale = Vector3.one;
                money.transform.rotation = Random.rotation;

                money.DOScale(0.8f, 0.3f).SetAutoKill();
                money.DORotateQuaternion(Random.rotation, 0.35f).SetAutoKill();
                var tween = money.DOJump(target + Vector3.down * 0.5f, 1.5f, 1, 0.35f).SetEase(Ease.InOutSine)
                    .OnComplete(() => LeanPool.Despawn(money)).SetAutoKill();

                yield return new WaitForSeconds(0.1f);
            }
        }

        public void StopSpendingMoney()
        {
            StopAllCoroutines();
            
            if (IsSpending)
            {
                IsSpending = false;
                OnSpendEnded?.Invoke();
            }
        }

        public void StopSpendingMoney(CurrencyReceiver currencyReceiver)
        {
            if (currencyReceiver != _currentCurrencyReceiver)
                return;

            StopSpendingMoney();
        }
    }
}