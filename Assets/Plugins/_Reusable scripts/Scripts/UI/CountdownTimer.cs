using UnityEngine;
using TMPro;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using DG.Tweening;
using System.Collections;

public class CountdownTimer : MonoBehaviour 
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private int startingSeconds;
    [SerializeField] private bool startOnStart = true;
    [Space(10f)]
    [SerializeField] private bool blinkOnCloseToZero;
    [ShowIf("blinkOnCloseToZero")] [SerializeField] private int startBlinkTime = 10;
    [ShowIf("blinkOnCloseToZero")] [SerializeField] private Color blinkColor = Color.red;
    [ShowIf("blinkOnCloseToZero")] [SerializeField] private float bllinkTweenDuration = 0.5f;
    [ShowIf("blinkOnCloseToZero")] [SerializeField] private Ease blinkEase = Ease.Linear;

    private float _elapsedTime;
    private bool _toCountDown;
    private int _remainingSeconds;
    private bool _isBlinking;
    private Tween _blinkTween;

    [Space(20f)]
    public UnityEvent OnComplete;

	void Start () 
    {
		_elapsedTime = 0.0f;
        UpdateTimerText();

        if (startOnStart)
            _toCountDown = true;
	}
	
	void Update ()
    {
        if (_toCountDown)
        {
            _elapsedTime += Time.deltaTime;
            UpdateTimerText();
            CheckZero();

            if (!_isBlinking)
                CheckBlink();
        }
	}

    public void SetStartingSeconds(int value)
    {
        startingSeconds = value;
    }

    public void ResetTimer()
    {
        _blinkTween.Rewind();
        _blinkTween.Kill();
        UpdateTimerText();
        _elapsedTime = 0f;
        _toCountDown = false;
        _isBlinking = false;
    }

    public void StartCountingDown()
    {
        _toCountDown = true;
    }

    public IEnumerator StartCountingDownWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCountingDown();
    }

    public void StopCountingDown()
    {
        _toCountDown = false;
    }

    void UpdateTimerText() 
    {
        int timeInSeconds = (int)(_elapsedTime % 60);
        int minutes = 0;
        _remainingSeconds = startingSeconds - timeInSeconds;
        if(startingSeconds >= 60) {
          minutes = (int)((startingSeconds - timeInSeconds)/60);
          _remainingSeconds -= (minutes * 60);
        }
        timerText.text = minutes.ToString() + ":" + _remainingSeconds.ToString().PadLeft(2, '0');
    }

    private void CheckZero()
    {
        if (_remainingSeconds == 0)
        {
            _toCountDown = false;
            _isBlinking = false;
            _blinkTween.Kill();
            timerText.DOColor(blinkColor, bllinkTweenDuration).SetEase(blinkEase);

            OnComplete?.Invoke();
        }
    }

    private void CheckBlink()
    {
        if (_remainingSeconds == startBlinkTime)
        {
            _isBlinking = true;
            _blinkTween = timerText.DOColor(blinkColor, bllinkTweenDuration).SetEase(blinkEase).SetLoops(-1, LoopType.Yoyo);
        }
    }
}