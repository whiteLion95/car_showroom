using DG.Tweening;
using PajamaNinja.Common;
using PajamaNinja.CurrencySystem;
using PajamaNinja.Scripts.IdleBarber.Unlocking;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PajamaNinja.Scripts.CurrencySystem
{
    public abstract class BaseUnlocker : CurrencyReceiver
    {
        [SerializeField] private UnlockerSSO _unlockerSSO;
        [SerializeField] private UnlockersList _unlockersList;
        [SerializeField] private Transform _onUnlockCameraTarget;
        [SerializeField] private bool _disableLerpOnAvailable;
        [SerializeField] private string _lerpCamName = "supportCam";
        [SerializeField] private float _camLerpDuration = 2f;
        [SerializeField] private float _camLerpDelay;

        [Space]
        [SerializeField] private TextMeshProUGUI _currencyText;
        [SerializeField] private Image _progressBar;

        private float _progress;
        private int _currentCurrency;
        private int _currencyTarget;
        [ShowInInspector, ReadOnly] private UnlockerInfo _unlockerInfo;
        private CameraOperator _cameraOperator;

        public override bool CanReceiveCurrency => gameObject.activeInHierarchy;
        public override int TargetCurrencyAmount => _currencyTarget;
        public override int CurrencyAmountLeft => _currencyTarget - _currentCurrency;
        public bool ShowAnimationEnabled { get; set; } = true;
        protected UnlockerSSO Sso => _unlockerSSO;
        public UnlockerInfo UnlockerInfo => _unlockerInfo;


        public override void ReceiveProgress(float deltaTime, float purchaseDuration, int currencyAmount, out int currencySpent)
        {
            currencySpent = 0;

            if (currencyAmount == 0)
                return;

            _progress = Mathf.Clamp01(_progress + deltaTime / purchaseDuration);
            currencySpent = (int)(_progress * TargetCurrencyAmount - _currentCurrency);
            currencySpent = Mathf.Clamp(currencySpent, 0, CurrencyAmountLeft);

            if (currencySpent > currencyAmount)
            {
                currencySpent = Mathf.Clamp(currencySpent, 0, currencyAmount);
                if (currencySpent > 0)
                    _progress = (float)(_currentCurrency + currencySpent) / TargetCurrencyAmount;
                else
                    _progress -= deltaTime / purchaseDuration;
            }
            _currentCurrency += currencySpent;

            _unlockerInfo.SpentCurrency = _currentCurrency;

            RefreshUI();

            if (_currentCurrency == _currencyTarget)
                Unlock();
        }

        protected abstract void BeforeStart();

        protected abstract void ActivateDependency(bool forFirstTime);

        private void Awake()
        {
            _unlockerInfo = UnlockersManager.Instance.Get(_unlockerSSO);
            _unlockerInfo.SetGameInstance(this);
        }

        private void Start()
        {
            _cameraOperator = Camera.main.GetComponent<CameraOperator>();
            BeforeStart();
            gameObject.SetActive(false);

            if (_unlockerInfo.IsUnlocked)
            {
                ActivateDependency(false);
                return;
            }

            if (_unlockerInfo.IsAvailable == false)
            {
                _unlockerInfo.BecameAvailable += OnBecameAvailable;
                return;
            }

            Initialize();
        }

        private void OnBecameAvailable()
        {
            if (_unlockerInfo.IsAvailable == false)
                return;
            _unlockerInfo.BecameAvailable -= OnBecameAvailable;
            if (_disableLerpOnAvailable == false)
            {
                VirtualCamerasManager.Instance.SwitchCameraAndBack(_lerpCamName, transform, _camLerpDelay, _camLerpDuration);
            }
            Initialize();
        }

        private void Initialize()
        {
            _currencyTarget = _unlockerInfo.UnlockPrice;

            if (_unlockerInfo.IsSaveExists)
            {
                _currentCurrency = _unlockerInfo.SpentCurrency;
                _progress = (float)_currentCurrency / TargetCurrencyAmount;
            }

            if (_progress >= 1)
            {
                Unlock();
                return;
            }

            RefreshUI();

            gameObject.SetActive(true);

            PlayShowAnimation();
        }

        private void Unlock()
        {
            //_cameraOperator.ShowObject(_onUnlockCameraTarget);
            if (_onUnlockCameraTarget)
                VirtualCamerasManager.Instance.SwitchCameraAndBack(_lerpCamName, _onUnlockCameraTarget, showDuration: _camLerpDuration);

            _unlockerInfo.IsUnlocked = true;
            ActivateDependency(true);
            gameObject.SetActive(false);
        }

        private void RefreshUI()
        {
            _currencyText.SetText($"{CurrencyAmountLeft}");
            _progressBar.fillAmount = _progress;
        }

        private void PlayShowAnimation()
        {
            if (!ShowAnimationEnabled)
                return;

            transform
                .DOScale(Vector3.one, 0.3f)
                .SetEase(Ease.OutBack)
                .From(Vector3.zero)
                .SetDelay(0.3f)
                .SetAutoKill()
                .SetTarget(gameObject);
        }
    }
}