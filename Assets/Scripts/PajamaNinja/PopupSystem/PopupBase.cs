using DG.Tweening;
using PajamaNinja.Common;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace PajamaNinja.PopupSystem
{
    [AddComponentMenu("#QBERA/Popup")]
    [DefaultExecutionOrder(100)]
    public class PopupBase : MonoBehaviour
    {
        [TabGroup("Tabs", "Settings")]
        [TitleGroup("Tabs/Settings/Popup")]
        [SerializeField]
        protected private bool _isShownOnAwake = false, _withSetActive = false;

        [InfoBox("Please do not try animate RectTransforms! Results might be unpredictable")]
        [TabGroup("Tabs", "Tweens")]
        [SerializeField]
        protected DOTweenAnimation _toggleTweenAnim;

        [TabGroup("Tabs", "Tweens")]
        [AssetsOnly]
        [SerializeField]
        protected DOTweenAnimation _tweenPrefab;

        [TabGroup("Tabs", "Events")]
        [SerializeField]
        protected UnityEvent _onShowStart, _onShowComplete, _onHideStart, _onHideComplete;

        [TabGroup("Tabs", "Debug")]
        [ShowInInspector]
        [ReadOnly]
        protected PopupState _currentComponentState;

        protected string _toggleTweenId;

        protected DOTweenAnimation _currentTweenGroup = null;

        public virtual void Awake()
        {
            if (!_tweenPrefab.SafeIsUnityNull())
            {
                _currentTweenGroup = Instantiate(_tweenPrefab, transform);
                _currentTweenGroup.targetGO = gameObject;
                _currentTweenGroup.target = transform;
                //_currentTweenGroup.CreateTween(true, false);
                _currentTweenGroup.CreateTween();
            }
            else
            {
                _currentTweenGroup = _toggleTweenAnim;
            }

            _toggleTweenId = _currentTweenGroup.id;

            var targetTween = _currentTweenGroup.GetTweens().OrderByDescending(tween => tween.Duration()).First();
            targetTween.OnComplete(OnToggleComplete);
            targetTween.OnRewind(OnToggleComplete);


            if (_isShownOnAwake)
                Show(true);
            else
                Hide(true);
        }

        [Button]
        public virtual void Show(bool isInstant = false)
        {
            if (_currentComponentState is PopupState.Shown or PopupState.InTransitionToShown) return;

            if (_withSetActive)
                gameObject.SetActive(true);

            _onShowStart.Invoke();

            _currentComponentState = PopupState.InTransitionToShown;

            _currentTweenGroup.DOPlayForwardById(_toggleTweenId);

            if (isInstant)
                _currentTweenGroup.DOComplete();
        }

        [Button]
        public virtual void Hide(bool isInstant = false)
        {
            if (_currentComponentState is PopupState.Hidden or PopupState.InTransitionToHidden) return;

            _onHideStart.Invoke();

            _currentComponentState = PopupState.InTransitionToHidden;

            _currentTweenGroup.DOPlayBackwardsById(_toggleTweenId);

            if (isInstant)
                _currentTweenGroup.DORewind();
        }

        public virtual void Toggle(bool value)
        {
            if (value)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        public virtual void OnToggleComplete()
        {
            switch (_currentComponentState)
            {
                case PopupState.InTransitionToShown:
                    ShowComplete();
                    break;

                case PopupState.InTransitionToHidden:
                    HideComplete();
                    break;

                default:
                    return;
            }
        }

        protected virtual void ShowComplete()
        {
            _currentComponentState = PopupState.Shown;
            _onShowComplete.Invoke();
        }

        protected virtual void HideComplete()
        {
            _currentComponentState = PopupState.Hidden;
            if (_withSetActive)
                gameObject.SetActive(false);
            _onHideComplete.Invoke();
        }

        public virtual void DebugString(string message) => Debug.Log($"[{name}] {message}");

        public PopupState CurrentComponentState => _currentComponentState;

        public UnityEvent OnShowStart => _onShowStart;

        public UnityEvent OnShowComplete => _onShowComplete;

        public UnityEvent OnHideStart => _onHideStart;

        public UnityEvent OnHideComplete => _onHideComplete;
    }
}