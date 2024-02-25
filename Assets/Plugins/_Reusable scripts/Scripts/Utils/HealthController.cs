using System;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 100;
    [SerializeField] private SliderBar _healthBar;

    private float _curHealth;

    public Action OnZeroHealth;
    public Action OnMaxHealth;
    public Action<float> OnHealthChanged;

    public float MaxHealth { get => _maxHealth; }
    public float CurHealth { get => _curHealth; }

    private void Awake()
    {
        _curHealth = _maxHealth;
    }

    private void Start()
    {
        if (_healthBar != null)
        {
            _healthBar.SetMaxValue(_maxHealth);
            _healthBar.SetValue(_maxHealth);
        }
    }

    public void TakeDamage(float damage)
    {
        if (_curHealth > 0)
        {
            _curHealth -= Mathf.Abs(damage);

            if (_curHealth <= 0)
            {
                _curHealth = 0;
                OnZeroHealth?.Invoke();
            }

            OnHealthChanged?.Invoke(_curHealth);
            if (_healthBar != null) _healthBar.ChangeValue(_curHealth);
        }
    }

    public void RestoreHealth(float value)
    {
        if (_curHealth < _maxHealth)
        {
            _curHealth += Mathf.Abs(value);

            if (_curHealth >= _maxHealth)
            {
                _curHealth = _maxHealth;
                OnMaxHealth?.Invoke();
            }

            OnHealthChanged?.Invoke(_curHealth);
            if (_healthBar != null) _healthBar.ChangeValue(_curHealth);
        }
    }

    public void SetSliderActive(bool value)
    {
        if (_healthBar)
        {
            CanvasGroup canGroup = _healthBar.GetComponentInParent<CanvasGroup>();

            if (canGroup)
            {
                if (value)
                    canGroup.alpha = 1f;
                else
                    canGroup.alpha = 0f;
            }
        }
    }
}