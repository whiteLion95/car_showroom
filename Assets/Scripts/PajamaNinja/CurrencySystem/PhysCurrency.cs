using System;
using UnityEngine;
using DG.Tweening;
using Lean.Pool;
using Sirenix.OdinInspector;
using Random = UnityEngine.Random;

namespace PajamaNinja.CurrencySystem
{
    [AddComponentMenu("#QBERA/PhysCurrency")]
    public class PhysCurrency : MonoBehaviour, IPoolable
    {
        public static Action<PhysCurrency> Picked;

        [TabGroup("Tabs", "Settings")]
        [SerializeField]
        private CurrencySSO _currency;

        [TabGroup("Tabs", "Components")]
        [SerializeField]
        private Collider _collider;

        [TabGroup("Tabs", "Components")]
        [SerializeField]
        private Rigidbody _rigidbody;
        
        private int _amount;
        private bool _isInitialKinematic;
        private Vector3 _initialScale;

        public CurrencySSO Currency => _currency;
        public int Amount => _amount;


        private void Awake()
        {
            _isInitialKinematic = _rigidbody.isKinematic;
            _initialScale = transform.localScale;
        }

        public void OnSpawn()
        {
            transform.localScale = _initialScale;
            _rigidbody.isKinematic = _isInitialKinematic;
            if (_rigidbody.isKinematic == false)
            {
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.angularVelocity = Vector3.zero;
            }
            _collider.enabled = true;
        }
        
        
        public void OnDespawn()
        {
            //throw new System.NotImplementedException();
        }
        
        public void Initialize(int amount)
        {
            _amount = amount;
        }

        public void Throw(Vector3 direction, float force = 12)
        {
            _rigidbody.AddForce(direction * force);
            _rigidbody.AddTorque(Random.insideUnitSphere * Random.Range(0.2f, 2f));
        }

        public void Pick()
        {
            _currency.CurrencyCount += Amount;
                    
            LeanPool.Despawn(this);
        }

        public void PickWithMagnet(Transform target)
        {
            // Destroy(_collider);
            _collider.enabled = false;
            _rigidbody.isKinematic = true;
            // Destroy(_rigidbody);

            Picked?.Invoke(this);

            var flyDuration = 0.25f + Random.Range(-0.08f, 0.08f);
            var flyHeight = 2f;
            var rndPosition2d = Random.insideUnitCircle;

            var magnetDelay = flyDuration;
            var magnetDuration = 0.25f + Random.Range(-0.08f, 0.08f);

            var targetPosition = transform.position + Vector3.up * flyHeight + new Vector3(rndPosition2d.x, 0, rndPosition2d.y);
            transform.DOMove(targetPosition, flyDuration).SetEase(Ease.OutQuad).SetAutoKill();

            var moveTween = transform.DOMove(target.position, magnetDuration).SetDelay(magnetDelay).SetEase(Ease.InQuad).SetAutoKill();
            moveTween.OnUpdate(() =>
            {
                var duration = moveTween.Duration() - moveTween.Elapsed();
                if (duration > 0)
                    moveTween.ChangeValues(transform.position, target.position, duration);
            });

            transform.DORotateQuaternion(Random.rotation, flyDuration + magnetDuration).SetAutoKill();

            transform.DOScale(0.4f, magnetDuration).SetDelay(magnetDelay)
                .OnComplete(() => Pick()).SetAutoKill();
        }
    }
}
