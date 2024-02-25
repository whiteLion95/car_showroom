using System;
using System.Collections.Generic;
using PajamaNinja.Common;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace PajamaNinja.UISystem
{
    public class UIManager : SingleReference<UIManager>
    {
        [TabGroup("General")]
        [SerializeField]
        [SceneObjectsOnly]
        private List<UIScreen> _uiScreens = new List<UIScreen>();
        
        [TabGroup("General")]
        [SerializeField] 
        private UIState _defaultUIState;

        [TabGroup("Debug")]
        [ShowInInspector, ReadOnly]
        private UIState _currentUIState;

        private Queue<UIState> _uiStateActualQueue = new Queue<UIState>();
        
        private Queue<UIState> _uiStateExpectedQueue = new Queue<UIState>();
        
        private List<UIScreen> ScreensInTransition
        {
            get
            {
                return _uiScreens.FindAll(screen =>
                    screen.CurrentComponentState is PopupState.InTransitionToHidden or PopupState.InTransitionToShown);
            }
        }

        private void Start()
        {
            EnqueueActualUIState(_defaultUIState);
        }
        
        private void Update()
        {
            if (ScreensInTransition.Count > 0) return;

            if (_uiStateActualQueue.Count > 0)
            {
                _currentUIState = _uiStateActualQueue.Dequeue();
                _onUIStateChange.Invoke(_currentUIState);
                UpdateScreensVisibility();
            }
        }

        private void UpdateScreensVisibility()
        {
            foreach (var screen in _uiScreens)
            {
                screen.Toggle(screen.IsShownOnState(_currentUIState));
            }
        }

        [Button]
        public void EnqueueActualUIState(UIState state)
        {
            _uiStateActualQueue.Enqueue(state);
        }
        
        public void EnqueueExpectedUIState(UIState state)
        {
            _uiStateExpectedQueue.Enqueue(state);
        }

        public void MoveExpectedToActualUIState()
        {
            var tempState = _uiStateExpectedQueue.Count <= 0 ? _defaultUIState : _uiStateExpectedQueue.Dequeue();
            
            EnqueueActualUIState(tempState);
        }

        public UIState NextExpectedUIState => _uiStateExpectedQueue.Count <= 0 ? _defaultUIState : _uiStateExpectedQueue.Peek();

        public UIState CurrentUIState => _currentUIState;

        [HideInInspector]
        public UnityEvent<UIState> _onUIStateChange = new UnityEvent<UIState>();
    }
}