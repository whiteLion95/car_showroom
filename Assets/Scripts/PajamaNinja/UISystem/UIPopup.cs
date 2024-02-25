using PajamaNinja.PopupSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PajamaNinja.UISystem
{
    [AddComponentMenu("#QBERA/UIPopup")]
    public class UIPopup : PopupBase
    {
        [TabGroup("Tabs", "Settings")]
        [TitleGroup("Tabs/Settings/UIPopup")]
        [SerializeField]
        protected CanvasGroup _canvasGroup;

        public override void Show(bool isInstant = false)
        {
            base.Show(isInstant);
            _canvasGroup.blocksRaycasts = true;
        }

        public override void Hide(bool isInstant = false)
        {
            base.Hide(isInstant);
            _canvasGroup.interactable = false;
        }

        protected override void ShowComplete()
        {
            base.ShowComplete();
            _canvasGroup.interactable = true;
        }

        protected override void HideComplete()
        {
            base.HideComplete();
            _canvasGroup.blocksRaycasts = false;
        }
    }
}