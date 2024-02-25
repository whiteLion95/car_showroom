using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PajamaNinja.UISystem
{
    public class UIScreen : UIPopup
    {
        [TabGroup("Tabs", "Settings")]
        [TitleGroup("Tabs/Settings/UIScreen")]
        [SerializeField] 
        [AssetList]
        protected List<UIState> _visibleForStates;

        public bool IsShownOnState(UIState value) => _visibleForStates.Contains(value);
    }
}