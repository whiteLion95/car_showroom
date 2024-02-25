using Dreamteck.Splines;
using PajamaNinja.CurrencySystem;
using System;
using UnityEngine;

namespace PajamaNinja.CarShowRoom
{
    [RequireComponent(typeof(CurrencyGiver))]
    [RequireComponent(typeof(UnlockingObject))]
    public class ParkingLot : MonoBehaviour
    {
        [SerializeField] private CarsDataSO _carsData;
        [SerializeField] private ParkingLotSSO _saveData;
        [SerializeField] private Transform _visitorPlace;
        [SerializeField] private ColliderEvents _playerSellPlace;
        [SerializeField] private float _sellingTime = 1.5f;
        [SerializeField] private Transform _getOutPlace;
        [SerializeField] private ParticleSystem _lotVFX;
        [SerializeField] private ParticleSystem _carSellVFX;

        public static Action<ParkingLot, Car> OnCarInit;
        public Action<Car> OnCarSold;
        public Action<Visitor> OnVisitorSet;

        private Car _parkedCar;
        private Player _player;
        private Visitor _visitor;
        private SplineComputer _spline;
        private SplineFollower _splineFollower;
        private CurrencyGiver _currencyGiver;
        private UnlockingObject _unlockingObj;
        private Collider _collider;

        public Transform VisitorPlace => _visitorPlace;
        public GameObject SellPlace => _playerSellPlace.gameObject;
        public SplineComputer Spline => _spline;
        public SplineFollower SplineFollower => _splineFollower;
        public Car ParkedCar => _parkedCar;
        public Transform GetOutPlace => _getOutPlace;
        public CurrencyGiver CurencyGiver => _currencyGiver;
        public bool IsUnlocked
        {
            get
            {
                if (_unlockingObj == null)
                    _unlockingObj = GetComponent<UnlockingObject>();

                return _unlockingObj.IsUnlocked;
            }
        }

        private void Awake()
        {
            _spline = GetComponentInChildren<SplineComputer>();
            _splineFollower = GetComponentInChildren<SplineFollower>();
            _currencyGiver = GetComponent<CurrencyGiver>();
            _unlockingObj = GetComponent<UnlockingObject>();
            _collider = GetComponent<Collider>();
        }

        private void Start()
        {
            _player = Player.Instance;

            Init();

            _playerSellPlace.TriggerEnter += HandleSellPlaceTriggerEnter;
            _playerSellPlace.TriggerExit += HandleSellPlaceTriggerExit;
        }

        private void Init()
        {
            if (_saveData.HasParkedCar)
            {
                _parkedCar = Instantiate(_carsData[_saveData.ParkedCarName].prefab, transform.position, transform.rotation);
                _parkedCar.SetParkingLot(this);
                PlayLotVFX(false);
                OnCarInit?.Invoke(this, _parkedCar);
            }
        }

        public void Park(Car car)
        {
            _parkedCar = car;
            _saveData.SetParkedCar(_parkedCar.Data.name);
            PlayLotVFX(false);
        }

        public void ActivateSellPlace(bool value)
        {
            _playerSellPlace.gameObject.SetActive(value);
        }

        public void SetVisitor(Visitor visitor)
        {
            _visitor = visitor;
            ActivateSellPlace(true);
            OnVisitorSet?.Invoke(visitor);
        }

        private void SellParkedCar()
        {
            _currencyGiver.SpawnMoney(_parkedCar.Data.incomeFromSale);
            ActivateSellPlace(false);
            _visitor.Movement.MoveTo(_parkedCar.VisitorPlace.position);
            _visitor.Movement.OnDestination += HandleDestination;
            _saveData.HasParkedCar = false;
            PlayCarSellVFX();
            _parkedCar.SetPriceUI(false);
            OnCarSold?.Invoke(_parkedCar);

            void HandleDestination()
            {
                _visitor.Movement.OnDestination -= HandleDestination;
                _visitor.Rider.GetInTheCar(_parkedCar);
                _parkedCar = null;
                PlayLotVFX(true);
            }
        }

        public void PlayLotVFX(bool value)
        {
            if (value)
                _lotVFX.Play(true);
            else
                _lotVFX.Stop(true);
        }

        public void PlayCarSellVFX()
        {
            _carSellVFX.Play(true);
        }

        public void EnableCollider(bool on)
        {
            _collider.enabled = on;
        }

        private void HandleSellPlaceTriggerEnter(ColliderEvents colEvents, Collider other)
        {
            if (other.gameObject.Equals(_player.gameObject))
            {
                _player.InteractionHandler.StartInteraction(_sellingTime, SellParkedCar);
            }
        }

        private void HandleSellPlaceTriggerExit(ColliderEvents colEvents, Collider other)
        {
            if (other.gameObject.Equals(_player.gameObject))
            {
                _player.InteractionHandler.TerminateInteraction();
            }
        }
    }
}