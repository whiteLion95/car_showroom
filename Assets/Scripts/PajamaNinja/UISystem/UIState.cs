using Sirenix.OdinInspector;
using UnityEngine;

namespace PajamaNinja.UISystem
{
    [CreateAssetMenu(fileName = "NewUiState", menuName = "QBERA/UiState", order = 0)]
    public class UIState : ScriptableObject
    {
        [Button]
        public void EnqueueThisState()
        {
            UIManager.Instance.EnqueueActualUIState(this);
        }
    }
}