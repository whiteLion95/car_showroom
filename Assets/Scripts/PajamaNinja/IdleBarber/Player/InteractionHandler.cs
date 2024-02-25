using System;
using System.Collections;
using PajamaNinja.UISystem;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace PajamaNinja.CarShowRoom
{
    public class InteractionHandler : UIPopup
    {
        [TabGroup("Tabs", "Settings")] [SerializeField] private bool _doNotControlVisibility;
        [FormerlySerializedAs("circularProgressBar")] [TabGroup("Tabs", "Components")]
        [SerializeField] private Image _circularProgressBar;
        [TabGroup("Tabs", "Components")] [SerializeField] private Image _interactionIcon;
        [TabGroup("Tabs", "Settings")] [SerializeField] private float defaultInteractionTime;

        private Coroutine _interactionCoroutine;
        private bool _interacting;
        public bool IsInteractionInProgress { get; private set; }

        private void Start()
        {
            _onHideComplete.AddListener(() => _circularProgressBar.fillAmount = 0);
            Hide(true);
        }

        [Button]
        public void StartInteraction(float interactionTime = 0, Action action = null)
        {
            if (_interactionCoroutine != null)
            {
                StopCoroutine(_interactionCoroutine);
            }
            
            if (!_doNotControlVisibility)
                Show();
            IsInteractionInProgress = true;
            if(interactionTime > 0) _interactionCoroutine = StartCoroutine(InteractionCoroutine(interactionTime, action));
            else _interactionCoroutine = StartCoroutine(InteractionCoroutine(defaultInteractionTime, action));

            _interacting = true;
        }

        [Button]
        public void TerminateInteraction()
        {
            if (_interactionCoroutine != null)
            {
                StopCoroutine(_interactionCoroutine);
                
                if (!_doNotControlVisibility)
                    Hide();
                else
                {
                    _circularProgressBar.fillAmount = 0;
                }

                _interacting = false;
            }

            IsInteractionInProgress = false;
        }

        public void SetIcon(Sprite icon) => _interactionIcon.sprite = icon;

        private IEnumerator InteractionCoroutine(float time, Action action)
        {
            float elapsedTime = 0;

            while (elapsedTime < time)
            {
                elapsedTime += Time.deltaTime;
                _circularProgressBar.fillAmount = elapsedTime / time;
                yield return null;
            }

            _circularProgressBar.fillAmount = 1;
            if(action != null) action.Invoke();
            
            if (!_doNotControlVisibility)
                Hide();

            IsInteractionInProgress = false;
        }

        public bool Interacting => _interacting;
    }

}