using DG.Tweening;
using Lean.Pool;
using PajamaNinja.CarShowRoom;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PajamaNinja.CurrencySystem
{
    public class CurrencyStack : MonoBehaviour
    {
        public static event Action CurrencyPickedByPlayer;

        public event Action<int> CurrencyAmountChanged;
        public Action PickedByPlayer;

        [SerializeField] private PhysCurrency _physCurrencyPrefab;
        [SerializeField] private CurrencyStackSSO _stackSSO;

        private List<PhysCurrency> _stack = new();
        private Player _player;

        public int CurrencyAmount => _stackSSO.CurrencyAmount;

        public Stacker Stacker { get; set; }

        private void Start()
        {
            LoadSavedData();
            PhysCurrency.Picked += OnCurrencyPicked;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                if (_player == null)
                    StartCoroutine(RemoveMoneyCoroutine(player.transform));

                _player = player;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                StopAllCoroutines();
                _player = null;
            }
        }

        [Button]
        public void TestCurrencyStack()
        {
            var spawnPos = transform.position + Vector3.up * 3;

            AddCurrency(spawnPos, 10, 5);
        }

        public void AddCurrency(Vector3 spawnPosition, int fullAmount, int banknotesCount, bool onInit = false)
        {
            int stepAmount = Mathf.CeilToInt((float)fullAmount / banknotesCount);
            int leftAmount = fullAmount;

            for (int i = banknotesCount; i > 0; i--)
            {
                var amount = Mathf.Min(stepAmount, leftAmount);
                leftAmount -= amount;

                AddCurrency(spawnPosition, amount, onInit);
            }

            if (!onInit)
                SaveData();
        }

        private void AddCurrency(Vector3 spawnPosition, int amount, bool onInit = false)
        {
            var newCurrency = LeanPool.Spawn(_physCurrencyPrefab, transform.position, Quaternion.identity);

            var positionIndex = _stack.Count;

            var maxPosIndex = _currencyPositionsIndexes.Count > 0 ? _currencyPositionsIndexes.Max(x => x.PositionInd) : 0;

            if (maxPosIndex >= _currencyPositionsIndexes.Count)
            {
                for (int i = 0; i <= maxPosIndex; i++)
                {
                    if (!_currencyPositionsIndexes.Exists(x => x.PositionInd == i))
                    {
                        positionIndex = i;
                        break;
                    }
                }
            }

            Vector3 nextMoneyPosition = Stacker.GetNextItemPosition(positionIndex);

            if (onInit)
            {
                newCurrency.transform.localScale = Vector3.one;
                newCurrency.transform.position = transform.position + nextMoneyPosition;
            }
            else
            {
                newCurrency.transform.localScale = Vector3.zero;
                newCurrency.transform.position = spawnPosition;

                newCurrency.transform.DOJump(transform.position + nextMoneyPosition, 1, 1, 0.25f).SetAutoKill();
                newCurrency.transform.DOScale(1, 0.25f).SetAutoKill();
            }

            newCurrency.Initialize(amount);

            _stack.Add(newCurrency);
            _currencyPositionsIndexes.Add(new CurrencyInStack { PositionInd = positionIndex, Currency = newCurrency });
        }

        struct CurrencyInStack
        {
            public PhysCurrency Currency;
            public int PositionInd;
        }

        private List<CurrencyInStack> _currencyPositionsIndexes = new();

        public void OnCurrencyPicked(PhysCurrency currency)
        {
            _stack.Remove(currency);
            _currencyPositionsIndexes.RemoveAll(x => x.Currency == currency);
            SaveData();
        }

        IEnumerator RemoveMoneyCoroutine(Transform target)
        {
            while (true)
            {
                while (_stack.Count > 0)
                {
                    yield return new WaitForSeconds(0.04f);

                    if (_stack.Count == 0) continue;

                    CurrencyPickedByPlayer?.Invoke();
                    PickedByPlayer?.Invoke();

                    int threshold = 20;
                    // float mappedCount = ((float) _stack.Count).Map(0, threshold, 0, 1);
                    // int removeForIterationAmount = 1 + Mathf.FloorToInt(EaseExtension.InQuart(mappedCount) * threshold * 0.5f);
                    float mappedCount = Mathf.Clamp((float)_stack.Count / threshold - 1, 0, float.MaxValue);
                    int removeForIterationAmount = 1 + Mathf.FloorToInt(mappedCount * threshold * 0.5f);
                    removeForIterationAmount = _stack.Count > threshold ? removeForIterationAmount : 1;

                    var targetCurrency = _stack[_stack.Count - removeForIterationAmount];
                    for (int i = _stack.Count - removeForIterationAmount + 1; i < _stack.Count; i++)
                        _stack[i].Pick();
                    // _stack.RemoveAt(_stack.Count - 1);
                    _stack.RemoveRange(_stack.Count - removeForIterationAmount, removeForIterationAmount);

                    // money fly up
                    var flyDuration = 0.25f;
                    var flyHeight = 2f;
                    var rndPosition2d = Random.insideUnitCircle;
                    var targetPosition = targetCurrency.transform.position + Vector3.up * flyHeight + new Vector3(rndPosition2d.x, 0, rndPosition2d.y);
                    targetCurrency.transform.DOMove(targetPosition, flyDuration).SetAutoKill();
                    targetCurrency.transform.DORotateQuaternion(Random.rotation, flyDuration).SetAutoKill();

                    // money magnet to player
                    var magnetDelay = flyDuration + 0.1f;
                    var magnetDuration = 0.25f;
                    var moveTween = targetCurrency.transform.DOMove(target.position + Vector3.up, magnetDuration).SetDelay(magnetDelay).SetAutoKill();
                    moveTween.OnUpdate(() =>
                    {
                        var duration = moveTween.Duration() - moveTween.Elapsed();
                        if (duration > 0)
                            moveTween.ChangeValues(targetCurrency.transform.position, target.position + Vector3.up, duration);
                    });

                    targetCurrency.transform.DOScale(0.4f, magnetDuration).SetDelay(magnetDelay)
                        .OnComplete(() => targetCurrency.Pick()).SetAutoKill();

                    SaveData();
                }

                yield return null;
            }
        }

        private void LoadSavedData()
        {
            if (_stackSSO.IsSaveExists)
                AddCurrency(transform.position, _stackSSO.CurrencyAmount, _stackSSO.BanknotesCount, true);
        }

        private void SaveData()
        {
            _stackSSO.CurrencyAmount = _stack.Sum(x => x.Amount);
            _stackSSO.BanknotesCount = _stack.Count;

            CurrencyAmountChanged?.Invoke(CurrencyAmount);
        }
    }
}
