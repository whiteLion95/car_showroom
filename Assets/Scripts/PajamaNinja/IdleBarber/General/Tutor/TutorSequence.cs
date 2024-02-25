using PajamaNinja.Scripts.IdleBarber.General.Tutor.Progress;
using System;
using System.Linq;

namespace PajamaNinja.CarShowRoom
{
    public class TutorSequence
    {
        public event Action<TutorSequenceId> SequenceCompleted;
        public event Action<TutorStepId> StepCompleted;
        public event Action<TutorStepId> StepTriggered;

        public TutorSequenceId Id => _id;
        public bool IsSequenceTriggered => _isSequenceTriggered;
        public bool IsSequenceCompleted => _isSequenceCompleted;

        private bool IsSequenceCompletedDb
        {
            get => _progress.IsSequenceCompleted(_id.ToString());
            set => _progress.SetSequenceCompleted(_id.ToString(), value);
        }

        private readonly TutorSequenceId _id;
        private readonly ITutorProgress _progress;
        private readonly TutorStep[] _tutorSequence;
        private bool _isSequenceTriggered;
        private bool _isSequenceCompleted;
        private int _currentlyActiveStep;
        private Action _activatingHandler;
        private Action _completingHandler;

        public TutorSequence(TutorSequenceId id, ITutorProgress progress, TutorStep[] tutorSequence)
        {
            _id = id;
            _progress = progress;
            _tutorSequence = tutorSequence;
            _isSequenceCompleted = IsSequenceCompletedDb;
        }

        public TutorSequence SetActivatingHandler(Action handler)
        {
            _activatingHandler = handler;
            return this;
        }

        public TutorSequence SetCompletingHandler(Action handler)
        {
            _completingHandler = handler;
            return this;
        }

        public void ActivateSequence()
        {
            _activatingHandler?.Invoke();
            for (int i = 0; i < _tutorSequence.Length; i++)
            {
                if (_tutorSequence[i].IsCompleted)
                {
                    if (_tutorSequence[i].IsFinalizedStep) return;
                    continue;
                }
                _currentlyActiveStep = i;
                ActivateStep(_tutorSequence[i]);
                break;
            }
        }

        public bool TryGetStep(TutorStepId stepId, out TutorStep step)
        {
            if (_tutorSequence.Count(s => s.Id == stepId) > 0)
            {
                step = _tutorSequence.First(s => s.Id == stepId);
                return true;
            }

            step = null;
            return false;
        }

        public void ForceSetCompleting(bool isComplete)
        {
            IsSequenceCompletedDb = isComplete;
            _isSequenceCompleted = false;
        }

        private void ActivateStep(TutorStep step)
        {
            step.Completed += OnStepCompleted;
            step.Triggered += OnStepTriggered;
            step.ActivateStep();
        }

        private void OnStepCompleted()
        {
            _tutorSequence[_currentlyActiveStep].Completed -= OnStepCompleted;
            _tutorSequence[_currentlyActiveStep].Triggered -= OnStepTriggered;
            StepCompleted?.Invoke(_tutorSequence[_currentlyActiveStep].Id);
            if (_tutorSequence[_currentlyActiveStep].IsFinalizedStep)
            {
                IsSequenceCompletedDb = true;
            }
            if (_currentlyActiveStep >= _tutorSequence.Length - 1)
            {
                _isSequenceCompleted = true;
                IsSequenceCompletedDb = true;
                SequenceCompleted?.Invoke(_id);
                _completingHandler?.Invoke();
                return;
            }

            _currentlyActiveStep++;
            ActivateStep(_tutorSequence[_currentlyActiveStep]);
        }

        private void OnStepTriggered()
        {
            _isSequenceTriggered = true;
            StepTriggered?.Invoke(_tutorSequence[_currentlyActiveStep].Id);
        }
    }
}