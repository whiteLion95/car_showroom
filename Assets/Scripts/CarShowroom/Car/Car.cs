using DG.Tweening;
using Dreamteck.Splines;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace PajamaNinja.CarShowRoom
{
    [RequireComponent(typeof(CarConstruction))]
    public class Car : MonoBehaviour
    {
        [SerializeField] private CarsDataSO _carsData;
        [SerializeField] private CarName _name;
        [SerializeField] private float _interactionTime = 1.5f;
        [SerializeField] private Transform _getOutPlace;
        [SerializeField] private InteractionHandler _interactionHandler;
        [SerializeField] private bool _isReadyForRide;
        [SerializeField] private Transform _visitorPlace;
        [SerializeField] private ParticleSystem _carReadyVFX;
        [SerializeField] private List<ParticleSystem> _trailParticles;
        [SerializeField] private GameObject _priceUI;
        [SerializeField] private TMP_Text _priceText;

        public static Action<Car, ParkingLot> OnCarParked;
        public Action<ParkingLot> OnParked;
        public Action<CarRider> OnCarRiderIn;
        public Action<Car> OnCarInit;
        public Action OnOutOfConveyor;
        public Action OnReadyToRide;
        public Action OnCarMove;
        public Action OnCarStop;

        private CarConstruction _carConstruction;
        private CarController _carController;
        private NavMeshAgent _agent;
        private ParkingLot _curParkingLot;
        private Player _player;
        private NavMeshObstacle _obstacle;
        private CarAI _carAI;
        private bool _isParked;
        private Compass _compass;
        private Collider _collider;

        public CarData Data => _carsData[_name];
        public bool IsReadyForRide { get => _isReadyForRide; set => _isReadyForRide = value; }
        public Transform GetOutPlace => _getOutPlace;
        public InteractionHandler InteractionHandler => _interactionHandler;
        public CarController CarController => _carController;
        public Transform VisitorPlace => _visitorPlace;
        public CarConstruction CarConstruction => _carConstruction;
        public ParkingLot CurParkingLot => _curParkingLot;
        public Compass Compass => _compass;

        private void Awake()
        {
            _carController = GetComponent<CarController>();
            _agent = GetComponent<NavMeshAgent>();
            _obstacle = GetComponentInChildren<NavMeshObstacle>();
            _carAI = GetComponent<CarAI>();
            _carConstruction = GetComponent<CarConstruction>();
            _compass = GetComponentInChildren<Compass>();
            _collider = GetComponent<Collider>();
        }

        private void Start()
        {
            _player = Player.Instance;

            OnCarInit?.Invoke(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            Player player = other.GetComponentInParent<Player>();

            if (player != null && IsReadyForRide && ParkingLotsManager.Instance.GetAvailableLots().Count > 0)
            {
                player.InteractionHandler.StartInteraction(_interactionTime, PlayerIn);
            }

            ParkingLot parkingLot = other.GetComponent<ParkingLot>();

            if (!_isParked && parkingLot && parkingLot.ParkedCar == null)
            {
                _curParkingLot = parkingLot;
                _interactionHandler.StartInteraction(_interactionTime, Park);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Player player = other.GetComponentInParent<Player>();

            if (player != null)
            {
                player.InteractionHandler.TerminateInteraction();
            }

            ParkingLot parkingLot = other.GetComponent<ParkingLot>();

            if (parkingLot)
            {
                _curParkingLot = null;
                _interactionHandler.TerminateInteraction();
            }
        }

        public void HandleSpawn(int curStage)
        {
            _isReadyForRide = false;
            _carController.enabled = false;
            _agent.enabled = false;
            _carConstruction.InitCurStage(curStage);
        }

        public void HandleReadyToRide()
        {
            _isReadyForRide = true;
            _carController.enabled = true;
            SetObstacle(true);
            _carReadyVFX.Play(true);
            ToggleColliider();
            OnReadyToRide?.Invoke();
        }

        private void ToggleColliider()
        {
            _collider.enabled = false;
            _collider.enabled = true;
        }

        public void HandleRiderIn(CarRider rider)
        {
            SetObstacle(false);
            _trailParticles.ForEach((t) => t.gameObject.SetActive(true));

            if (rider is PlayerRider)
            {
                _isReadyForRide = false;
                _carController.Joystick = _player.Joystick;
                _carController.LockInput(false);
            }

            if (rider is VisitorRider)
            {
                StartCoroutine(MoveFromSelling());
            }

            OnCarRiderIn?.Invoke(rider);
        }

        public void SetObstacle(bool value)
        {
            if (value)
            {
                _agent.enabled = false;
                _obstacle.enabled = true;
            }
            else
            {
                _obstacle.enabled = false;
            }
        }

        private void PlayerIn()
        {
            if (_player.TryGetComponent(out CarRider playerRider))
            {
                playerRider.GetInTheCar(this);
            }
        }

        private void PlayerOut()
        {
            if (_player.TryGetComponent(out CarRider playerRider))
            {
                playerRider.GetOutOfTheCar(this);
                _carController.LockInput(true);
            }
        }

        private void Park()
        {
            if (!_isParked && _curParkingLot)
            {
                _isParked = true;
                PlayerOut();
                transform.DOMove(_curParkingLot.transform.position, 1f).SetEase(Ease.InOutSine);
                transform.DORotateQuaternion(_curParkingLot.transform.rotation, 1f).SetEase(Ease.InOutSine);
                _curParkingLot.Park(this);
                SetObstacle(true);
                OnCarParked?.Invoke(this, _curParkingLot);
                OnParked?.Invoke(_curParkingLot);
                SetPriceUI(true);
            }
        }

        public void SetParkingLot(ParkingLot parkingLot)
        {
            _isParked = true;
            _curParkingLot = parkingLot;
            SetObstacle(true);
            _carConstruction.InitLastStage();
            SetPriceUI(true);
        }

        private IEnumerator MoveFromSelling()
        {
            _isParked = false;
            yield return new WaitForSeconds(0.1f);

            SplineFollower splineFollower = _curParkingLot.SplineFollower;
            splineFollower.follow = true;
            _carAI.SetFollower(splineFollower.GetComponent<CarSplineFollower>());
            splineFollower.onEndReached += HandleEndReached;

            void HandleEndReached(double value)
            {
                splineFollower.onEndReached -= HandleEndReached;
                Destroy(gameObject);
                splineFollower.Restart();
                splineFollower.follow = false;
            }
        }

        public void SetPriceUI(bool on)
        {
            _priceUI.SetActive(on);

            if (on)
                _priceText.text = _carsData[_name].incomeFromSale.ToString();
        }
    }

}