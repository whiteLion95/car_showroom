using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.AI;

public class TopDownMovement : MonoBehaviour
{
    [SerializeField] protected TopDownMovementDataSO _data;
    [SerializeField] protected CanvasGroup _movementTutor;
    [SerializeField] protected Joystick _joystick;
    [SerializeField] protected NavMeshAgent _navMeshAgent;
    [SerializeField] protected CharacterAnimator _charAnimator;
    [SerializeField] protected bool _toShowMovementTutor;

    public event Action<bool> OnLockInput;
    public Action OnMove;
    public Action OnStop;

    protected Vector3 _moveDirection;
    protected bool _movementTutorIsShown = false;

    public bool IsMoving { get; private set; } = false;
    public bool IsInputLocked { get; protected set; }
    public float OffsetAngle { get; set; }
    public float WalkSpeed { get; private set; }
    public float DefautWalkSpeed => _data.MoveSpeed;
    public bool MovementTutorIsShown => _movementTutorIsShown;
    public Joystick Joystick { get => _joystick; set => _joystick = value; }

    private void Awake()
    {
        _movementTutor.alpha = 0;
        _movementTutor.gameObject.SetActive(false);

        if (_toShowMovementTutor)
            DOVirtual.DelayedCall(1f, ShowMovementTutor);
    }

    private void Start()
    {
        SetDefaultSpeed();
    }

    private void Update()
    {
        Move();
    }

    public void StopAnyMovement()
    {
        StopAllCoroutines();

        if (_charAnimator)
            _charAnimator.SetWalk(false);
    }

    [Button]
    public void LockInput(bool isLocked)
    {
        IsInputLocked = isLocked;
        IsMoving = false;

        if (_charAnimator)
            _charAnimator.SetWalk(false);

        if (!isLocked)
        {
            _joystick.ResetPointer();
        }

        OnLockInput?.Invoke(isLocked);
        OnStop?.Invoke();
    }

    public bool IsPositionNear(Vector3 position, bool useNavMesh = true)
    {
        if (useNavMesh && NavMesh.SamplePosition(position, out NavMeshHit hit, float.MaxValue, NavMesh.AllAreas))
            position = hit.position;
        return Vector3.Distance(transform.position, position) <= _navMeshAgent.stoppingDistance;
    }

    public void LockCollision(bool isLocked) => _navMeshAgent.enabled = !isLocked;
    public void SetDefaultSpeed() => SetSpeed(DefautWalkSpeed);
    public void SetSpeed(float speed) => WalkSpeed = speed;

    [Button]
    public void ShowMovementTutor()
    {
        _movementTutor.DOComplete();
        _movementTutor.gameObject.SetActive(true);
        _movementTutor.DOFade(1, 0.2f).SetAutoKill();
        _movementTutorIsShown = true;
    }

    [Button]
    public void HideMovementTutor()
    {
        _movementTutor.DOComplete();
        _movementTutor.DOFade(0, 0.2f).OnComplete(() =>
        {
            _movementTutor.gameObject.SetActive(false);
        }).SetAutoKill();
        _movementTutorIsShown = false;
    }

    private void Move()
    {
        if (IsInputLocked)
            return;

        var input = new Vector2(_joystick.Horizontal, _joystick.Vertical);
#if UNITY_EDITOR
        if (input == Vector2.zero)
            input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
#endif
        input = Quaternion.Euler(0, 0, OffsetAngle) * input;

        var direction = new Vector3(input.x, 0, input.y);

        Quaternion targetRotation = transform.rotation;

        bool isWalking = direction != Vector3.zero;

        if (isWalking)
            targetRotation = Quaternion.LookRotation(direction);

        float tRot = _data.RotSpeed * Time.deltaTime;

        if (_data.InputDependentSpeed)
            tRot *= input.magnitude;

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, tRot);

        if (isWalking)
            _moveDirection = direction.normalized * WalkSpeed;
        else
            _moveDirection = Vector3.zero;

        _moveDirection.y = _moveDirection.y - (_data.Gravity * Time.deltaTime);

        if (_data.InputDependentSpeed)
            _moveDirection *= input.magnitude;

        if (_navMeshAgent.enabled)
            _navMeshAgent.Move(_moveDirection * Time.deltaTime);

        if (!IsMoving && isWalking)
            OnMove?.Invoke();
        else if (IsMoving && !isWalking)
            OnStop?.Invoke();

        IsMoving = isWalking;

        if (IsMoving && _movementTutorIsShown)
        {
            HideMovementTutor();
        }

        if (_charAnimator != null)
        {
            _charAnimator.SetWalk(isWalking);
            float targetSpeed = 0f;

            if (_data.InputDependentSpeed)
                targetSpeed = Mathf.Lerp(_charAnimator.GetSpeed(), input.magnitude, _data.MoveBlendSpeed * Time.deltaTime);

            if (_data.AlwaysRunning)
                targetSpeed = 1f;

            _charAnimator.SetSpeed(targetSpeed);
        }

        if (IsMoving)
        {
            LockCollision(false);
        }
    }
}
