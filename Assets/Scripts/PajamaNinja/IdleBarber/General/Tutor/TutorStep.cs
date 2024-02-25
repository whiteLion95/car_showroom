using System;

namespace PajamaNinja.CarShowRoom
{
    public class TutorStep
    {
        public class DelegateArgs
        {
            public TutorStep Step;
            public Action<DelegateArgs> TriggerHandler;
            public Action<DelegateArgs> CompletedHandler;
        }

        public event Action Completed;
        public event Action Triggered;

        public TutorStepId Id => _id;
        public bool IsTriggered => _isTriggered;
        public bool IsCompleted => _isCompleted;
        public bool IsFinalizedStep => _isFinalizedStep;

        private readonly TutorStepId _id;
        private readonly DelegateArgs _delegateArgs;
        private bool _isCompleted;
        private bool _isTriggered;
        private bool _isFinalizedStep;
        private Action<DelegateArgs> _triggerListener;
        private Action<DelegateArgs> _completeListener;


        public TutorStep(TutorStepId id)
        {
            _id = id;
            _delegateArgs = new DelegateArgs { Step = this };
        }

        public void ActivateStep()
        {
            if (_triggerListener == null)
            {
                _delegateArgs.TriggerHandler.Invoke(_delegateArgs);
                return;
            }

            _triggerListener.Invoke(_delegateArgs);
        }

        public void SetStepAsCompleted()
        {
            if (_delegateArgs.CompletedHandler != null)
            {
                _delegateArgs.CompletedHandler.Invoke(_delegateArgs);
                return;
            }

            _isCompleted = true;
            Completed?.Invoke();
        }

        public TutorStep SetTriggerListener(Action<DelegateArgs> listener)
        {
            _triggerListener = listener;
            return this;
        }

        public TutorStep SetTriggerHandler(Action<DelegateArgs> handler)
        {
            _delegateArgs.TriggerHandler = handler;
            _delegateArgs.TriggerHandler += args =>
            {
                _isTriggered = true;
                Triggered?.Invoke();
            };
            return this;
        }

        public TutorStep SetCompleteListener(Action<DelegateArgs> listener)
        {
            _completeListener = listener;
            _delegateArgs.TriggerHandler += args =>
            {
                _completeListener.Invoke(args);
            };
            return this;
        }

        public TutorStep SetCompleteHandler(Action<DelegateArgs> handler)
        {
            _delegateArgs.CompletedHandler = handler;
            _delegateArgs.CompletedHandler += args =>
            {
                _isCompleted = true;
                Completed?.Invoke();
            };
            return this;
        }

        public TutorStep SetStepAsFinalSequence()
        {
            _isFinalizedStep = true;
            return this;
        }
    }
}