namespace PajamaNinja.Scripts.IdleBarber.General.Tutor.Progress
{
    public interface ITutorProgress
    {
        public bool IsSequenceCompleted(string sequenceName);
        public void SetSequenceCompleted(string sequenceName, bool value);
    }
}