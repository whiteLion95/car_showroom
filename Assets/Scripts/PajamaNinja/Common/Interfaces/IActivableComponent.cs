namespace PajamaNinja.Common
{
    public interface IActivableComponent
    {
        public bool IsActivated { get; }
     
        public IActivableComponent AsIActivable { get; }
        
        public void OnActivate();

        public void OnDeactivate();

        public void SetActive(bool value)
        {
            if (IsActivated == value) return;
            
            if (value)
            {
                OnActivate();
            }
            else
            {
                OnDeactivate();
            }
        }
    }
}