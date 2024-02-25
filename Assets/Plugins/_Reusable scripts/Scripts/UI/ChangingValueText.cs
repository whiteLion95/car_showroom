using UnityEngine;
using DG.Tweening;
using TMPro;
using System;

/// <summary>
/// Base class for changing ui text according to changing value
/// </summary>
public class ChangingValueText : MonoBehaviour
{
    [SerializeField] private float _smoothness = 0.3f;
    [SerializeField] private float _valueModifier = 1;
    [SerializeField] [Tooltip("To change thousands to K and millions to M")] private bool _reduceBigNumbers;

    private TMP_Text _valueText;
    private float _currentValue;

    protected virtual void Awake()
    {
        _valueText = GetComponent<TMP_Text>();   
    }

    /// <summary>
    /// Subscribe this method for the events when targetValue changes to change value text
    /// </summary>
    /// <param name="targetValue"></param>
    public void TweenText(float targetValue, Action onComplete = null)
    {
        DOTween.To(() => _currentValue, x => _currentValue = x, targetValue * _valueModifier, _smoothness).OnUpdate(() =>
        {
            _currentValue = Mathf.Round(_currentValue);
            SetValueText(_currentValue);
        }).OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }

    public void SetValueText(float value)
    {
        _currentValue = value;

        if (_valueText == null)
            _valueText = GetComponent<TMP_Text>();

        if (_reduceBigNumbers)
        {
            _valueText.text = ReducedBigText.GetText(value);
        }
        else
            _valueText.text = value.ToString();
    }
}
