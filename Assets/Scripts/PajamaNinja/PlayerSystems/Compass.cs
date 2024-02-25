using PajamaNinja.PopupSystem;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace PajamaNinja.CarShowRoom
{
    public class Compass : MonoBehaviour
    {
        [SerializeField]
        private GameObject _compasArrow;

        [SerializeField]
        private PopupBase _compassPopup;

        [SerializeField]
        private LookAtConstraint _lookAtConstraint;

        [SerializeField]
        private float _arrowHideRadius = 2f;

        [SerializeField]
        private float _tipHideRadius = 0.5f;

        [SerializeField]
        private PopupBase _tip;

        [SerializeField]
        private Transform _cashRegisterArrowPoint;

        [ShowInInspector, ReadOnly]
        private Transform _currentTarget;
        [ShowInInspector, ReadOnly]
        private List<Transform> _targetsQueue = new List<Transform>();

        [ShowInInspector, ReadOnly]
        private bool _isActive = false, _isPaused = false;

        private bool _showTip = true;
        private float _origTipHideRadius;

        private void Start()
        {
            _tip.transform.SetParent(null);
            _origTipHideRadius = _tipHideRadius;
        }

        private void Update()
        {
            if (!_isActive || _currentTarget == null) return;

            if (Vector3.Distance(_currentTarget.position, transform.position) > _arrowHideRadius && !_isPaused) _compassPopup.Show();
            else _compassPopup.Hide();

            if (_showTip)
            {
                if (Vector3.Distance(_tip.transform.position, transform.position) > _tipHideRadius && !_isPaused) _tip.Show();
                else _tip.Hide();
            }
        }


        private ConstraintSource NewSource(Transform point)
        {
            ConstraintSource source = new ConstraintSource();

            source.sourceTransform = point;

            source.weight = 1;

            return source;
        }


        [Button]
        public void Toggle(bool value = true, bool showTip = false, bool toggleLast = false, Transform point = null)
        {
            _showTip = showTip;

            if (value)
            {
                _compassPopup.Show();

                if (_showTip)
                    _tip.Show();
                else
                    _tip.Hide();
            }
            else
            {
                _compassPopup.Hide();
                _tip.Hide();
                _tipHideRadius = _origTipHideRadius;

                DequeueTarget(toggleLast, point);
            }
        }

        public void Toggle(float customTipHideRadius)
        {
            _tipHideRadius = customTipHideRadius;
            Toggle(true, true);
        }

        [Button]
        public void EnqueueTarget(Transform target)
        {
            if (target == null)
            {
                print("Compass target is empty");
                return;
            }

            _targetsQueue.Add(target);

            if (_currentTarget == null)
            {
                UpdateTarget(target);
            }
        }

        public void UpdateTarget(Transform target, bool withTip = false)
        {
            _currentTarget = target;
            _lookAtConstraint.SetSource(0, NewSource(_currentTarget));
            _isActive = true;

            if (withTip)
                _tip.transform.position = _currentTarget.position;
        }

        public void EnqueueTarget(Transform target, Vector3 tipOffset)
        {
            EnqueueTarget(target);

            _tip.transform.position += tipOffset;
        }

        private void DequeueTarget(bool last = false, Transform point = null)
        {
            if (_targetsQueue.Count > 0)
            {
                if (point != null && _targetsQueue.Contains(point))
                    _targetsQueue.Remove(point);
                else if (point == null)
                {
                    if (last)
                        _targetsQueue.RemoveAt(_targetsQueue.Count - 1);
                    else
                        _targetsQueue.RemoveAt(0);
                }

                if (_targetsQueue.Count > 0)
                {
                    UpdateTarget(_targetsQueue[0]);
                    Toggle(showTip: false);
                }
                else
                {
                    _currentTarget = null;
                    _isActive = false;
                }
            }
            else if (_currentTarget != null)
                _currentTarget = null;
        }

        public bool IsPaused
        {
            get => _isPaused;
            set => _isPaused = value;
        }
    }
}