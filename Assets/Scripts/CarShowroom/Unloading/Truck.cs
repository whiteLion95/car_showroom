using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

namespace PajamaNinja.CarShowRoom
{
    [RequireComponent(typeof(ResourceSpawner))]
    [RequireComponent(typeof(InteractableObjectSender))]
    public class Truck : MonoBehaviour
    {
        [SerializeField] private TruckDataSO _data;
        [SerializeField] private UnloadArea _unloadArea;
        [SerializeField] private InteractionHandler _interHandler;
        [SerializeField] private Transform _doors;
        [SerializeField] private Transform _leftTruckDoor;
        [SerializeField] private Transform _rightTruckDoor;
        [SerializeField] private Ease _truckDoorsEase = Ease.OutBounce;

        private ResourceSpawner _resSpawner;
        private InteractableObjectSender _interObjSender;
        private Vector3 _originPos;
        private Tween _moveTween;
        private Coroutine _moveToUnloadRoutine;

        private void Awake()
        {
            _resSpawner = GetComponent<ResourceSpawner>();
            _interObjSender = GetComponent<InteractableObjectSender>();
            _originPos = transform.position;
        }

        private void Start()
        {
            Init();

            _unloadArea.OnEmpty += HandleUnloadAreaEmpty;
        }

        private void Init()
        {
            if (_unloadArea.GetCurCount() == 0 && !_unloadArea.FullOnAwake)
            {
                _moveToUnloadRoutine = StartCoroutine(MoveToUnloadRoutine(0f));
            }
        }

        private void MoveTo(Vector3 pos, Action onDestination = null)
        {
            _moveTween = transform.DOMove(pos, _data.MoveToUnloadDuration).SetEase(_data.MoveEase);
            _moveTween.onComplete += () => onDestination?.Invoke();
        }

        private void Unload()
        {
            _resSpawner.OnSpawn -= HandleResourceSpawn;
            _resSpawner.OnSpawn += HandleResourceSpawn;
            _resSpawner.StartRepeatedSpawn();
        }

        private void StopUnloading()
        {
            _resSpawner.StopRepeatedSpawn();
            MoveTo(_originPos);
            _resSpawner.OnSpawn -= HandleResourceSpawn;
            StartCoroutine(OpenDoorsRoutine(_data.OpenDoorsTime, false));
            OpenTruckDoors(false);
        }

        private IEnumerator MoveToUnloadRoutine(float delay)
        {
            _interHandler.StartInteraction(delay + _data.MoveToUnloadDuration);
            StartCoroutine(OpenDoorsRoutine(delay + _data.MoveToUnloadDuration - _data.OpenDoorsTime, true));
            yield return new WaitForSeconds(delay);

            if (_unloadArea.GetCurCount() == 0)
            {
                MoveTo(_unloadArea.UnloadPlace.position, OnDestination);

                void OnDestination()
                {
                    OpenTruckDoors(true);
                    Unload();
                }
            }

            _moveToUnloadRoutine = null;
        }

        private IEnumerator OpenDoorsRoutine(float delay, bool toOpen)
        {
            yield return new WaitForSeconds(delay);
            OpenDoors(toOpen);
        }

        private void OpenDoors(bool toOpen)
        {
            float targetY = toOpen ? 0f : 1f;
            _doors.DOScaleY(targetY, _data.OpenDoorsDuration).SetEase(_data.OpenDoorsEase);
        }

        private void OpenTruckDoors(bool toOpen)
        {
            float targetYRot = toOpen ? _data.TruckDoorsYRot : 0f;
            _rightTruckDoor.DOLocalRotate(new Vector3(0f, -targetYRot, 0f), _data.TruckDoorsOpenDuration).SetEase(_truckDoorsEase);
            _leftTruckDoor.DOLocalRotate(new Vector3(0f, targetYRot, 0f), _data.TruckDoorsOpenDuration).SetEase(_truckDoorsEase);
        }

        private void HandleResourceSpawn(GameObject spawnedObj)
        {
            if (spawnedObj.TryGetComponent(out InteractableObject interObj))
            {
                _interObjSender.SendInterObj(interObj, _unloadArea);

                if (!_unloadArea.CanTakeObjects())
                {
                    StopUnloading();
                }
            }
        }

        private void HandleUnloadAreaEmpty()
        {
            if (_moveToUnloadRoutine != null)
                StopCoroutine(_moveToUnloadRoutine);

            _moveToUnloadRoutine = StartCoroutine(MoveToUnloadRoutine(_data.MoveToUnloadDelay));
        }
    }
}