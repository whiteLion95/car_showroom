using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

namespace ToolBox.Utils
{
    public class SphereCaster : MonoBehaviour
    {
        [SerializeField] private Collider[] _collidersInRange;
        [SerializeField] private float _radius;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private QueryTriggerInteraction _triggerInteraction;
        [SerializeField] private Transform _transform;

        private List<Collider> _prevCollidersInRange;

        public UnityEvent OnFirstCollider;
        public UnityEvent OnLastCollider;
        public Action<Collider> OnColliderEnter;
        public Action<Collider> OnColliderExit;

        public Collider[] CollidersInRange { get => _collidersInRange; }
        public float Radius { get => _radius; set => _radius = value; }

        // Start is called before the first frame update
        private void Start()
        {
            if (_transform == null)
                _transform = transform;

            _prevCollidersInRange = new List<Collider>();
        }

        private void FixedUpdate()
        {
            CheckColliders();
        }

        private void CheckColliders()
        {
            _prevCollidersInRange.Clear();
            _prevCollidersInRange.AddRange(_collidersInRange);
            _collidersInRange = Physics.OverlapSphere(_transform.position, _radius, _layerMask, _triggerInteraction);

            if (_prevCollidersInRange.Count == 0 && _collidersInRange.Length > _prevCollidersInRange.Count)
            {
                OnFirstCollider?.Invoke();
                OnColliderEnter?.Invoke(_collidersInRange[_collidersInRange.Length - 1]);
                return;
            }
            else if (_collidersInRange.Length == 0 && _collidersInRange.Length < _prevCollidersInRange.Count)
            {
                OnLastCollider?.Invoke();
                OnColliderExit?.Invoke(_prevCollidersInRange[_prevCollidersInRange.Count - 1]);
                return;
            }

            for (int i = 0; i < _collidersInRange.Length; i++)
            {
                if (!_prevCollidersInRange.Contains(_collidersInRange[i]))
                {
                    OnColliderEnter?.Invoke(_collidersInRange[i]);
                    return;
                }
            }

            for (int i = 0; i < _prevCollidersInRange.Count; i++)
            {
                if (!_collidersInRange.Contains(_prevCollidersInRange[i]))
                {
                    OnColliderExit?.Invoke(_prevCollidersInRange[i]);
                    return;
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (_transform != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(_transform.position, _radius);
            }
        }
    }
}
