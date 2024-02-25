using System.Linq;
using TMPro;
using UnityEngine;

namespace PajamaNinja.Common
{
    [RequireComponent(typeof(TMP_Text))]
    [ExecuteInEditMode]
    public class TextSetter : MonoBehaviour
    {
        private void Awake()
        {
            _textHolder = GetComponent<TMP_Text>();
        }

        [SerializeField]
        private string _prefix, _suffix;
        
        private TMP_Text _textHolder;

        public void Set(int value)
        {
            _textHolder.text = $"{_prefix}{value.ToString()}{_suffix}";
        }
        
        public void Set(long value)
        {
            _textHolder.text = $"{_prefix}{value.ToString()}{_suffix}";
        }
        
        public void SetCurrency(long value)
        {
            _textHolder.text = $"{_prefix}{value.CurrencyFormat()}{_suffix}";
        }
        
        public void Set(float value, string format = "F")
        {
            _textHolder.text = $"{_prefix}{value.ToString(format)}{_suffix}";
        }
        
        public void Set(double value, string format = "F")
        {
            _textHolder.text = $"{_prefix}{value.ToString(format)}{_suffix}";
        }
        
        public void Set(string value)
        {
            _textHolder.text = $"{_prefix}{value}{_suffix}";
        }

        public void SetPercent(float value)
        {
            _textHolder.text = $"{_prefix}{Mathf.Round(value * 100f).ToString()}{_suffix}";
        }
    }
}