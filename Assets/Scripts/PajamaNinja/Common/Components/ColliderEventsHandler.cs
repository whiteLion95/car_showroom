using System;
using UnityEngine;

namespace PajamaNinja.Scripts.Common.Components
{
    [RequireComponent(typeof(Collider))]
    public class ColliderEventsHandler : MonoBehaviour
    {
        public event Action<Collider> TriggerEntered;
        public event Action<Collider> TriggerStay;
        public event Action<Collider> TriggerExited;
        public event Action<Collision> CollisionEntered;
        public event Action<Collision> CollisionExited;

        public Collider Collider => _collider ??= GetComponent<Collider>();
        private Collider _collider;

        private void OnTriggerEnter(Collider other) => 
            TriggerEntered?.Invoke(other);

        private void OnTriggerStay(Collider other) => 
            TriggerStay?.Invoke(other);

        private void OnTriggerExit(Collider other) => 
            TriggerExited?.Invoke(other);

        private void OnCollisionEnter(Collision other) => 
            CollisionEntered?.Invoke(other);

        private void OnCollisionExit(Collision other) => 
            CollisionExited?.Invoke(other);
    }
}