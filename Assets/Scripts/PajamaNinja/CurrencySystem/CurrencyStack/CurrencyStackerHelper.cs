using System.Collections.Generic;
using UnityEngine;

namespace PajamaNinja.CurrencySystem
{
    [ExecuteAlways]
    [RequireComponent(typeof(CurrencyStack))]
    public class CurrencyStackerHelper : MonoBehaviour
    {
        [SerializeField] private PhysCurrency _currencyPrefab;
        [SerializeField] private CurrencyStack _currencyStack;

        private Stack<PhysCurrency> _currency = new();

        [SerializeField] private bool _enabled;
        [Space]
        [SerializeField] private Vector3 _base;
        [Space]
        [SerializeField] private Vector3 _raw;
        [SerializeField] private Vector3 _column;
        [SerializeField] private Vector3 _level;
        [Space]
        [Min(1)][SerializeField] private int _rawsInColumn = 1;
        [Min(1)][SerializeField] private int _columnsInLevel = 1;
        [Space]
        [Min(1)][SerializeField] private int _moneyNum = 10;

        private void Awake()
        {
            if (!Application.isPlaying)
                return;

            ResetChilds();

            _currencyStack.Stacker = new Stacker(_raw, _column, _rawsInColumn, _base, _level, _columnsInLevel);

            Destroy(this);
        }

        private void Update()
        {
            if (!Application.isPlaying)
                Refresh();
        }

        [ContextMenu("Clear")]
        private void ResetChilds()
        {
            _currency = new();

            foreach (var child in transform.GetComponentsInChildren<PhysCurrency>())
                DestroyImmediate(child.gameObject);
        }

        public void Refresh()
        {
            if (!_enabled)
                return;

            _currencyStack.Stacker = new Stacker(_raw, _column, _rawsInColumn, _base, _level, _columnsInLevel);

            while (_currency.Count > 0)
                DestroyImmediate(_currency.Pop().gameObject);

            _currency = new();

            for (int i = 0; i < _moneyNum; i++)
            {
                var newMoney = Instantiate(_currencyPrefab, transform);

                newMoney.transform.localPosition = _currencyStack.Stacker.GetNextItemPosition(_currency.Count);

                _currency.Push(newMoney);
            }
        }
    }
}