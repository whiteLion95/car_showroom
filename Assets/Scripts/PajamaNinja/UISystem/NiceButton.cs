using System.Linq;
using DG.Tweening;
using PajamaNinja.UISystem;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PajamaNinja.UISystem
{
    public class NiceButton : UIPopup
    {
        [TabGroup("Tabs", "Settings")]
        [TitleGroup("Tabs/Settings/Features")]
        [OnValueChanged("OnFeatureChange")]
        [SerializeField]
        private bool _isShiny, _isBouncy, _isLocked;
        
        [TabGroup("Tabs", "Tweens")]
        [SerializeField]
        protected DOTweenAnimation _clickTweenAnim, _bounceTweenAnim;
        
        [TabGroup("Tabs", "Settings")]
        [TitleGroup("Tabs/Settings/Color")]
        [OnValueChanged("OnColorChange")]
        [SerializeField]
        private bool _useColorOverrides;
        
        [TabGroup("Tabs", "Settings")]
        [TitleGroup("Tabs/Settings/Color")]
        [OnValueChanged("OnColorChange")]
        [HideIf("_useColorOverrides")]
        [SerializeField]
        private Color _baseMidColor;
        
        [TabGroup("Tabs", "Settings")]
        [TitleGroup("Tabs/Settings/Color")]
        [OnValueChanged("OnColorChange")]
        [HideIf("_useColorOverrides")]
        [Range(-1f, 1f)]
        [SerializeField]
        private float _upperBright = 0f, _backBright = 0f;
        
        [TabGroup("Tabs", "Settings")]
        [TitleGroup("Tabs/Settings/Color")]
        [OnValueChanged("OnColorChange")]
        [ShowIf("_useColorOverrides")]
        [SerializeField]
        private Color _upperColorOverride, _midColorOverride, _backColorOverride;
        
        [TabGroup("Tabs", "Settings")]
        [TitleGroup("Tabs/Settings/Other")]
        [OnValueChanged("OnFeatureChange")]
        [SerializeField]
        private bool _isInteractable = true;

        [TabGroup("Tabs", "UIComponents")]
        [TitleGroup("Tabs/UIComponents/Button")]
        [SerializeField]
        private Button _button;

        [TabGroup("Tabs", "UIComponents")]
        [TitleGroup("Tabs/UIComponents/Images")]
        [SerializeField]
        private Image _upperPart, _midPart, _backPart;
        
        // [TabGroup("Tabs", "UIComponents")]
        // [TitleGroup("Tabs/UIComponents/Other")]
        // [SerializeField]
        // private ButtonHighlight _shinyComponent;

        [TabGroup("Tabs", "UIComponents")]
        [TitleGroup("Tabs/UIComponents/Other")]
        [SerializeField]
        private GameObject _lockedGroup;
        
        [SerializeField]
        private UnityEvent _onButtonClicked;

        public override void Awake()
        {
            OnFeatureChange();
            OnColorChange();
            base.Awake();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClick);
        }
        
        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            _onButtonClicked.Invoke();
            _clickTweenAnim.DORestartById(_clickTweenAnim.id);
            _clickTweenAnim.DOPlayById(_clickTweenAnim.id);
            
            DebugString("button clicked");
        }

        [Button]
        public override void Show(bool isInstant = false)
        {
            base.Show(isInstant);
            _button.interactable = !_isLocked && _isInteractable;
        }

        [Button]
        public override void Hide(bool isInstant = false)
        {
            base.Hide(isInstant);
            _button.interactable = false;
        }

        private void OnFeatureChange()
        {
            _button.interactable = !_isLocked && _isInteractable;
            
            _lockedGroup.SetActive(_isLocked);

            if (_isLocked || !_isBouncy)
            {
                _bounceTweenAnim.DORestart();
                _bounceTweenAnim.DOPause();
            }
            else
            {
                _bounceTweenAnim.DOPlay();
            }
            
            // _shinyComponent.SetEnable(!_isLocked &&_isShiny);
        }

        private void OnColorChange()
        {
            if (_useColorOverrides)
            {
                _midPart.color = _midColorOverride;
            
                _upperPart.color = _upperColorOverride;
            
                _backPart.color = _backColorOverride;
            }
            else
            {
                float hue, saturation, brightness;
            
                Color.RGBToHSV(_baseMidColor, out hue, out saturation, out brightness);

                _midPart.color = _baseMidColor;
            
                _upperPart.color = Color.HSVToRGB(hue, saturation, brightness + _upperBright);
            
                _backPart.color = Color.HSVToRGB(hue, saturation, brightness + _backBright);
            }

            
        }

        public UnityEvent OnButtonClicked => _onButtonClicked;

        public bool IsShiny
        {
            get => _isShiny;
            set
            {
                _isShiny = value;
                OnFeatureChange();
            }
        }

        public bool IsBouncy
        {
            get => _isBouncy;
            set
            {
                _isBouncy = value;
                OnFeatureChange();
            }
        }

        public bool IsLocked
        {
            get => _isLocked;
            set
            {
                _isLocked = value;
                OnFeatureChange();
            }
        }
    }
}