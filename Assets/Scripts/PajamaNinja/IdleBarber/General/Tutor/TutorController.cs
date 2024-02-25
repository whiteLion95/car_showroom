using PajamaNinja.CurrencySystem;
using PajamaNinja.Scripts.IdleBarber.General.Tutor.Progress;
using System;
using System.Linq;

namespace PajamaNinja.CarShowRoom
{
    public class TutorController
    {
        public event Action<TutorSequenceId> SequenceCompleted;
        public event Action<TutorStepId> StepCompleted;
        public event Action<TutorStepId> StepTriggered;

        private readonly TutorSequence[] _tutors;

        public TutorController(ITutorProgress tutorProgress, CurrencySSO currency, CarsDataSO carsData)
        {
            var tutorContainer = new TutorContainer(this, tutorProgress, currency, carsData);
            _tutors = tutorContainer.GetTutors();
            ActivateTutor();
        }

        public TutorStep GetStep(TutorStepId stepId)
        {
            foreach (TutorSequence sequence in _tutors)
            {
                if (sequence.TryGetStep(stepId, out var step))
                    return step;
            }

            throw new InvalidOperationException();
        }

        public TutorSequence GetSequence(TutorSequenceId sequenceId) =>
            _tutors.First(s => s.Id == sequenceId);

        public void ForceSetSequenceCompleting(TutorSequenceId id, bool isComplete)
        {
            GetSequence(id).ForceSetCompleting(isComplete);
        }

        public void ResetTutor()
        {
            foreach (var tutorSequence in _tutors)
            {
                tutorSequence.ForceSetCompleting(false);
            }
        }

        private void ActivateTutor()
        {
            foreach (TutorSequence tutorSequence in _tutors.Where(s => s.IsSequenceCompleted == false))
            {
                tutorSequence.ActivateSequence();
                tutorSequence.StepTriggered += TutorSequenceOnStepTriggered;
                tutorSequence.StepCompleted += TutorSequenceOnStepCompleted;
                tutorSequence.SequenceCompleted += TutorSequenceOnSequenceCompleted;
            }
        }

        private void TutorSequenceOnSequenceCompleted(TutorSequenceId sequenceId)
        {
            SequenceCompleted?.Invoke(sequenceId);
        }

        private void TutorSequenceOnStepTriggered(TutorStepId stepId)
        {
            StepTriggered?.Invoke(stepId);
        }

        private void TutorSequenceOnStepCompleted(TutorStepId stepId)
        {
            StepCompleted?.Invoke(stepId);
        }
    }
}