using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SliderBar : MonoBehaviour
{
    [SerializeField] private bool _speedBased;
    [ShowIf("_speedBased")][SerializeField][Range(0f, 5f)] private float _speedPct = 1f;
    [SerializeField] private float _changeSmoothness;
    [SerializeField] private bool _lookAtCamera;
    [ShowIf("_lookAtCamera")][SerializeField] private bool _updateLook;
    [SerializeField] private Ease ease;

    public Action OnZero = delegate { };

    protected Slider _slider;
    private Camera _mainCam;

    protected virtual void Awake()
    {
        Init();

        _slider = GetComponent<Slider>();

        if (_lookAtCamera)
        {
            _mainCam = Camera.main;
            LookAtCamera();
        }
    }

    protected virtual void Init() { }

    private void Update()
    {
        if (_updateLook)
        {
            LookAtCamera();
        }
    }

    public void SetMaxValue(float value)
    {
        if (_slider != null)
        {
            _slider.maxValue = value;
            SetValue(value);
        }
    }

    public void SetValue(float value)
    {
        _slider.value = value;
    }

    public void ChangeValue(float value, Action onComplete = null)
    {
        if (_speedBased)
            _changeSmoothness = _speedPct * _slider.maxValue;

        _slider.DOValue(value, _changeSmoothness).SetUpdate(true).SetSpeedBased(_speedBased).SetEase(ease).onComplete +=
            () =>
            {
                if (value <= 0) OnZero?.Invoke();
                onComplete?.Invoke();
            };
    }

    private void LookAtCamera()
    {
        transform.parent.LookAt(transform.position + _mainCam.transform.rotation * Vector3.back);
    }
}
