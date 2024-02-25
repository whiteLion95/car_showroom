using DG.Tweening;
using System;
using UnityEngine;

namespace PajamaNinja.CarShowRoom
{
    public class CarRider : MonoBehaviour
    {
        [SerializeField] private float _inCarDuration = 1f;

        public Action<Car> OnInTheCar;
        public Action<Car> OnOutOfTheCar;

        private Transform _origParent;

        protected virtual void Awake()
        {
            _origParent = transform.parent;
        }

        protected virtual void Start()
        {

        }

        public virtual void GetInTheCar(Car car)
        {
            transform.SetParent(car.transform);
            transform.DOLocalMove(Vector3.zero, _inCarDuration).SetEase(Ease.Linear);
            transform.localRotation = Quaternion.identity;
            gameObject.SetActive(false);
            car.HandleRiderIn(this);

            OnInTheCar?.Invoke(car);
        }

        public virtual void GetOutOfTheCar(Car car)
        {
            transform.SetParent(_origParent);

            OnOutOfTheCar?.Invoke(car);
        }
    }
}
