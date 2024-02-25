using UnityEngine.Events;

namespace PajamaNinja.Common
{
    public interface ISelectableComponent
    {
        public bool IsSelected {get; }

        public void Select();

        public void Deselect();

        UnityEvent OnSelected { get; }
    }
}