using DG.Tweening;
using PajamaNinja.CarShowRoom;
using UnityEngine;

[RequireComponent(typeof(CarConstructionPart))]
public class Wheel : MonoBehaviour
{
    [SerializeField] private float _constructRotateSpeed = 180f;
    [SerializeField] private float _outOfConveyorRotateSpeed = 180f;
    [SerializeField] private Ease _constructRotateEase = Ease.Linear;
    [SerializeField] private Ease _moveEase = Ease.Linear;
    [SerializeField] private float _moveBaseSpeed = 40f;

    private CarConstructionPart _constructPart;
    private bool _isRight;
    private Car _car;
    private Tween _rotateTween;

    private void Awake()
    {
        _constructPart = GetComponent<CarConstructionPart>();
        _car = GetComponentInParent<Car>();
    }

    private void Start()
    {
        SetRightOrLeft();

        _constructPart.OnTweenStarted += HandleConstructTweenStarted;
        _car.OnOutOfConveyor += HandleCarOutOfConveyor;
        _car.CarController.OnStop += HandleCarStop;
        _car.CarController.OnMove += HandleCarMove;
        _car.OnCarStop += HandleCarStop;
    }

    private void SetRightOrLeft()
    {
        _isRight = transform.localPosition.x > 0;
    }

    private void Rotate(int dir, float speed, Ease ease)
    {
        _rotateTween = transform.DOLocalRotate(Vector3.right * dir, speed).SetSpeedBased(true).SetRelative(true).SetLoops(-1, LoopType.Incremental).SetEase(ease);
    }

    private void StopRotating()
    {
        _rotateTween?.Kill();
    }

    private void HandleConstructTweenStarted(Tween tween)
    {
        if (tween != null)
        {
            int dir = _isRight ? 1 : -1;
            Rotate(dir, _constructRotateSpeed, _constructRotateEase);
            tween.onComplete += () => StopRotating();
        }
    }

    private void HandleCarOutOfConveyor()
    {
        Rotate(-1, _outOfConveyorRotateSpeed, _moveEase);
    }

    private void HandleCarStop()
    {
        StopRotating();
    }

    private void HandleCarMove()
    {
        Rotate(-1, _moveBaseSpeed * _car.CarController.DefautWalkSpeed, _moveEase);
    }
}