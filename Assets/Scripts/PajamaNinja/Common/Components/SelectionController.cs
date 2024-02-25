using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PajamaNinja.Common
{
    public class SelectionController : SerializedMonoBehaviour
    {
        [SerializeField] 
        private List<ISelectableComponent> _selectables;

        // private void Start()
        // {
        //     _selectables.First().Select();
        // }

        private void OnEnable()
        {
            foreach (var selectable in _selectables)
            {
                selectable.OnSelected.AddListener(() => OnSelect(_selectables.IndexOf(selectable)));
            }
        }
        
        private void OnSelect(int value)
        {
            for (var i = 0; i < _selectables.Count; i++)
            {
                if (i == value) continue;
                
                _selectables[i].Deselect();
            }
        }
        
        private void OnDisable()
        {
            foreach (var selectable in _selectables)
            {
                selectable.OnSelected.RemoveAllListeners();
            }
        }
    }
}