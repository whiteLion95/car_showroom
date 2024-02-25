using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PajamaNinja.CarShowRoom
{
    [Serializable]
    public class CarStage
    {
        public List<RobotArm> arms;
        public ConveyorLent convLent;
    }

    [RequireComponent(typeof(UnlockingObject))]
    public class Conveyor : MonoBehaviour
    {
        [SerializeField] private ConveyorDataSO _data;
        [SerializeField] private CarsDataSO _carsData;
        [SerializeField] private ConveyorSSO _saveData;
        [SerializeField] private List<Transform> _carStagesPlaces;
        [SerializeField] private float _nextStageDelay = 0.3f;
        [SerializeField] private List<Transform> _outOfConveyourPath;
        [SerializeField] private List<CarStage> _stages;
        [SerializeField] private UnlockingObject _unlockingObj;

        public Action<Car> OnCarSpawned;
        public Action OnUnlocked;

        private const int STAGES_COUNT = 3;
        private ColliderEvents _colEvents;
        private ResourcesBox _resBox;
        private InteractableObjectsStack _curStack;
        private Coroutine _sendResourcesRoutine;
        private int _curStage;
        private CarName _curCarName;

        public Car CurrentCar { get; private set; }
        public List<CarRequiredResources> CarReqResources => _carsData[_curCarName].requiredResources;
        public GameObject GameObject => throw new NotImplementedException();
        public ConveyorSSO SaveData => _saveData;
        public Transform ResourcesTrigger => _colEvents.transform;
        public Transform CarReadyPlace => _outOfConveyourPath.Last();
        public UnlockingObject UnlockingObject => _unlockingObj;
        public ResourcesBox ResourceBox => _resBox;
        public Collider Collider => _colEvents.GetComponent<Collider>();
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
            _resBox = GetComponentInChildren<ResourcesBox>();
            _colEvents = GetComponentInChildren<ColliderEvents>();

            _unlockingObj.OnUnlockedForFirstTime += HandleUnlockForFirstTime;
        }

        private void Start()
        {
            if (_unlockingObj.IsUnlocked)
            {
                Init();
            }

            _colEvents.TriggerEnter += HandleOnTrigger;
            _colEvents.TriggerExit += HandleTriggerExit;
            _resBox.OnFull += HandleResBoxFull;
        }

        private void OnEnable()
        {
            Car.OnCarParked += HandleCarParked;
        }

        private void OnDisable()
        {
            Car.OnCarParked -= HandleCarParked;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                BuildCar();
            }
        }

        private void Init()
        {
            if (!_saveData.HasAvailableCar)
                SpawnCar();
            else
                InitAvailableCar();

            _saveData.AddOpenedCarName(_saveData.CurCarName);
        }

        private void SpawnCar()
        {
            _curCarName = _saveData.CurCarName;
            CurrentCar = Instantiate(_carsData[_saveData.CurCarName].prefab, _carStagesPlaces[_curStage].position, _carStagesPlaces[_curStage].rotation);
            CurrentCar.OnCarInit += HandleCarInit;
            OnCarSpawned?.Invoke(CurrentCar);

            if (_curStack)
                _sendResourcesRoutine = StartCoroutine(SendResourcesRoutine(_curStack));

            void HandleCarInit(Car car)
            {
                CurrentCar.OnCarInit -= HandleCarInit;
                CurrentCar.HandleSpawn(_curStage);
            }

            StartCoroutine(SpawnPosSafeCheck());
        }

        private void DespawnCurrentCar()
        {
            Destroy(CurrentCar.gameObject);
            CurrentCar = null;
        }

        public void EquipCar(CarName carName)
        {
            SaveData.CurCarName = carName;

            if (!_saveData.HasAvailableCar)
            {
                _resBox.Refresh();
                DespawnCurrentCar();
                SpawnCar();
            }
        }

        private IEnumerator SpawnPosSafeCheck()
        {
            yield return new WaitForSeconds(0.2f);

            if (CurrentCar && CurrentCar.transform.position != _carStagesPlaces[0].position)
                CurrentCar.transform.position = _carStagesPlaces[0].position;
        }

        private void InitAvailableCar()
        {
            CurrentCar = Instantiate(_carsData[_saveData.AvailableCarName].prefab, _outOfConveyourPath.Last().position, _outOfConveyourPath.Last().rotation);
            CurrentCar.HandleReadyToRide();
            CurrentCar.CarConstruction.InitCurStage(STAGES_COUNT);
        }

        private void HandleOnTrigger(ColliderEvents colEvent, Collider collider)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                InteractableObjectsStack stack = collider.gameObject.GetComponentInChildren<InteractableObjectsStack>();

                if (stack && !_saveData.HasAvailableCar)
                {
                    _curStack = stack;
                    _sendResourcesRoutine = StartCoroutine(SendResourcesRoutine(stack));
                }
            }
        }

        private void HandleTriggerExit(ColliderEvents colEvent, Collider collider)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                _curStack = null;
            }
        }

        private void HandleResBoxFull()
        {
            if (_sendResourcesRoutine != null)
                StopCoroutine(_sendResourcesRoutine);

            BuildCar();
        }

        private IEnumerator SendResourcesRoutine(InteractableObjectsStack stack)
        {
            for (int i = stack.StackedObjects.Count - 1; i > -1; i--)
            {
                ResourceItem resItem = stack.StackedObjects[i].GetComponent<ResourceItem>();

                if (resItem)
                {
                    if (!_resBox.IsEnoughAmount(resItem.MyType))
                    {
                        stack.SendObject(stack.StackedObjects[i], _resBox);
                        yield return new WaitForSeconds(stack.Data.SendObjectsInterval);
                    }
                }
            }

            _sendResourcesRoutine = null;
        }

        private void BuildCar()
        {
            _saveData.SetAvailableCar(CurrentCar.Data.name);
            ActivateStage(_curStage, true);
            CurrentCar.CarConstruction.NextStage(MoveCarToNextStage);
        }

        private void MoveCarToNextStage()
        {
            StartCoroutine(MoveCarToNextStageRoutine());
        }

        private IEnumerator MoveCarToNextStageRoutine()
        {
            yield return new WaitForSeconds(_nextStageDelay);

            if (CurrentCar != null)
            {
                ActivateStage(_curStage, false);
                ActivateLent(_curStage, true);
                _curStage++;

                if (_curStage >= STAGES_COUNT)
                {
                    MoveCarOutOfConveyor();
                }
                else
                {
                    CurrentCar.transform.DOMove(_carStagesPlaces[_curStage].position, _data.CarStageMoveSpeed).SetSpeedBased(true).SetEase(Ease.Linear)
                        .OnComplete(() =>
                        {
                            ActivateLent(_curStage - 1, false);
                            ActivateStage(_curStage, true);
                            CurrentCar.CarConstruction.NextStage(MoveCarToNextStage);
                        });
                }
            }
        }

        private void MoveCarOutOfConveyor()
        {
            _curStage = 0;
            CurrentCar.OnOutOfConveyor?.Invoke();

            Sequence outOfConveyourSequence = DOTween.Sequence();
            float outOfConveyorPathDistance = 0;
            for (int i = 0; i < _outOfConveyourPath.Count; i++)
            {
                if (i == 0)
                    outOfConveyorPathDistance += Vector3.Distance(_outOfConveyourPath[i].position, CurrentCar.transform.position);
                else
                    outOfConveyorPathDistance += Vector3.Distance(_outOfConveyourPath[i - 1].position, _outOfConveyourPath[i].position);
            }
            for (int i = 0; i < _outOfConveyourPath.Count; i++)
            {
                float distance = 0f;

                if (i == 0)
                    distance = Vector3.Distance(_outOfConveyourPath[i].position, CurrentCar.transform.position);
                else
                    distance = Vector3.Distance(_outOfConveyourPath[i - 1].position, _outOfConveyourPath[i].position);

                float disKoef = distance / outOfConveyorPathDistance;

                outOfConveyourSequence.Append(CurrentCar.transform.DOMove(_outOfConveyourPath[i].position, _data.OutOfConveyorDuration * disKoef).SetEase(Ease.Linear));
                outOfConveyourSequence.Insert(i, CurrentCar.transform.DORotate(_outOfConveyourPath[i].rotation.eulerAngles, 0.3f)).SetEase(Ease.Linear);
            }
            outOfConveyourSequence.OnComplete(() =>
            {
                CurrentCar.HandleReadyToRide();
                CurrentCar.OnCarStop?.Invoke();
                ActivateLent(STAGES_COUNT - 1, false);
            });
        }

        private void ActivateStage(int index, bool value)
        {
            if (index >= 0 && index < _stages.Count)
            {
                for (int i = 0; i < _stages[index].arms.Count; i++)
                {
                    _stages[index].arms[i].Work(value);
                }
            }
        }

        private void ActivateLent(int index, bool value)
        {
            _stages[index].convLent.Activate(value);
        }

        private void HandleCarParked(Car car, ParkingLot parkingLot)
        {
            if (car.Equals(CurrentCar))
            {
                CurrentCar = null;
                _saveData.HasAvailableCar = false;
                Invoke(nameof(SpawnCar), _data.NextCarSpawnDelay);
            }
        }

        private void HandleUnlockForFirstTime()
        {
            _resBox.InitResItems();
            Init();
            OnUnlocked?.Invoke();
        }
    }
}