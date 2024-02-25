using DG.Tweening;
using System;
using UnityEngine;

namespace PajamaNinja.CarShowRoom
{
    [RequireComponent(typeof(MovingCharacter))]
    [RequireComponent(typeof(VisitorRider))]
    public class Visitor : MonoBehaviour
    {
        private MovingCharacter _movement;
        private VisitorRider _carRider;

        public VisitorRider Rider => _carRider;
        public MovingCharacter Movement => _movement;

        private void Awake()
        {
            _movement = GetComponent<MovingCharacter>();
            _carRider = GetComponent<VisitorRider>();
        }

        public void GoBuyCar(Car car, ParkingLot parkingLot)
        {
            GoTo(parkingLot.VisitorPlace.position, HandleDestination);

            void HandleDestination()
            {
                transform.DOLookAt(car.transform.position, _movement.Agent.angularSpeed).SetSpeedBased(true).SetEase(Ease.Linear);
                parkingLot.SetVisitor(this);
            }
        }

        public void GoTo(Vector3 targetPos, Action onDestination = null)
        {
            _movement.MoveTo(targetPos);
            _movement.OnDestination += HandleDestination;

            void HandleDestination()
            {
                _movement.OnDestination -= HandleDestination;
                onDestination?.Invoke();
            }
        }
    }
}
