using DG.Tweening;
using PajamaNinja.Common;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace PajamaNinja.CarShowRoom
{
    public class VisitorsManager : SingleReference<VisitorsManager>
    {
        [SerializeField] private int _visitorsCount = 3;
        [SerializeField] private List<Transform> _visitorSpawnPlaces;
        [SerializeField] private List<Visitor> _visitorPrefabs;
        [SerializeField] private Vector3 _spawnRotation = new Vector3(0f, 180f, 0f);
        [SerializeField] private Transform _newVisitorSpawnPlace;
        [SerializeField] private Transform _doors;

        private List<Visitor> _curVisitors;
        private int _curVisitorToBuy;
        private RandomNoRepeate _rndNoRepeat;

        public Visitor CurVisitorToBuy { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            _rndNoRepeat = new RandomNoRepeate(_visitorPrefabs.Count);
            InitVisitors();

            Car.OnCarParked += HandleCarParked;
            ParkingLot.OnCarInit += HandleCarInit;
        }

        private void OnDisable()
        {
            Car.OnCarParked -= HandleCarParked;
            ParkingLot.OnCarInit -= HandleCarInit;
        }

        private void InitVisitors()
        {
            _curVisitors = new List<Visitor>();

            for (int i = 0; i < _visitorsCount; i++)
            {
                _curVisitors.Add(Instantiate(GetRandVisitorPrefab(), _visitorSpawnPlaces[i].position, Quaternion.Euler(_spawnRotation), transform));
            }
        }

        private Visitor GetRandVisitorPrefab()
        {
            int randIndex = _rndNoRepeat.GetAvailable();
            return _visitorPrefabs[randIndex];
        }

        private void SpawnNewVisitor()
        {
            Visitor newVisitor = Instantiate(GetRandVisitorPrefab(), _newVisitorSpawnPlace.position, Quaternion.LookRotation(_doors.position - _newVisitorSpawnPlace.position), transform);
            _curVisitors.Insert(_curVisitorToBuy, newVisitor);
            newVisitor.GoTo(_visitorSpawnPlaces[_curVisitorToBuy].position, HandleDestination);

            void HandleDestination()
            {
                newVisitor.transform.DORotate(_spawnRotation, newVisitor.Movement.Agent.angularSpeed).SetSpeedBased(true).SetEase(Ease.Linear);
            }

            _curVisitorToBuy++;
            if (_curVisitorToBuy > _curVisitors.Count - 1)
                _curVisitorToBuy = 0;
        }

        private void HandleCarParked(Car car, ParkingLot parkingLot)
        {
            if (_curVisitors != null && _curVisitors.Count > 0)
            {
                Visitor visitorToBuy = _curVisitors[_curVisitorToBuy];
                visitorToBuy.GoBuyCar(car, parkingLot);
                _curVisitors.Remove(visitorToBuy);
                SpawnNewVisitor();
            }
        }

        private void HandleCarInit(ParkingLot parkingLot, Car car)
        {
            HandleCarParked(car, parkingLot);
        }
    }
}