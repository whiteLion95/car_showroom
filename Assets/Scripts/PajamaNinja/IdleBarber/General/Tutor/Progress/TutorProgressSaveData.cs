using System;
using System.Collections.Generic;
using PajamaNinja.SaveSystem;

namespace PajamaNinja.Scripts.IdleBarber.General.Tutor.Progress
{
    [Serializable]
    public class TutorProgressSaveData : SaveDataInSO
    {
        public List<Sequence> Sequences = new();
            
        [Serializable]
        public struct Sequence
        {
            public string Name;
            public bool State;

            public Sequence(string name, bool state)
            {
                Name = name;
                State = state;
            }
        }
    }
}