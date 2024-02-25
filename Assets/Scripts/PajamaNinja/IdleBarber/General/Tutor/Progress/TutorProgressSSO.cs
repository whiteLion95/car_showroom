using System.Linq;
using PajamaNinja.SaveSystem;
using UnityEngine;

namespace PajamaNinja.Scripts.IdleBarber.General.Tutor.Progress
{
    [CreateAssetMenu(fileName = "TutorProgressSSO", menuName = "QBERA/Saveables/TutorProgressSSO")]
    public class TutorProgressSSO : SaveableSO<TutorProgressSaveData>, ITutorProgress
    {
        public bool IsSequenceCompleted(string sequenceName)
        {
            TryLoad();
            return _saveData.Sequences.FirstOrDefault(s => s.Name.Equals(sequenceName)).State;
        }

        public void SetSequenceCompleted(string sequenceName, bool value)
        {
            int indexOfSequence = _saveData.Sequences.FindIndex(s => s.Name.Equals(sequenceName));
            if (indexOfSequence < 0)
                _saveData.Sequences.Add(new TutorProgressSaveData.Sequence(sequenceName, value));
            else
                _saveData.Sequences[indexOfSequence] = new TutorProgressSaveData.Sequence(sequenceName, value);
            TrySave();
        }
    }
}