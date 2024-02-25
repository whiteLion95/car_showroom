namespace PajamaNinja.Scripts.IdleBarber.Unlocking
{
    public interface IUnlockingObject
    {
        public void UnlockForFirstTime();
        public void Unlock();
    }
}