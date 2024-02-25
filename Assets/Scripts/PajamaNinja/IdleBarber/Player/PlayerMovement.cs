using DG.Tweening;
using PajamaNinja.Scripts.IdleBarber.General;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace PajamaNinja.Scripts.IdleBarber.Player
{
    public class PlayerMovement : MonoBehaviour, ICharacterMovement
    {
        [SerializeField] private TopDownMovementDataSO _data;
        [Header("Components")]
        [SerializeField] private CanvasGroup _movementTutor;
        [SerializeField] private Joystick _joystick;
        [SerializeField] private NavMeshAgent _navMeshAgent;
        

        public event Action<bool> OnLockInput;

        private CharacterAnimator _playerAnimator;
        private Vector3 _moveDirection;
        private bool _movementTutorIsShown = false;

        public bool IsMoving { get; private set; } = false;
        public bool IsInputLocked { get; private set; }
        public float OffsetAngle { get; set; }
        public float WalkSpeed { get; private set; }
        public float DefautWalkSpeed => _data.MoveSpeed;

        public bool MovementTutorIsShown => _movementTutorIsShown;
        public Joystick Joystick => _joystick;


        private void Awake()
        {
            _playerAnimator = GetComponent<CharacterAnimator>();
            _movementTutor.alpha = 0;
            _movementTutor.gameObject.SetActive(false);

            DOVirtual.DelayedCall(1f, ShowMovementTutor);
        }

        private void Start()
        {
            SetDefaultSpeed();
        }

        private void Update() => Move();

        public void MoveTo(Vector3 position, Action onReached) { }

        public void StopAnyMovement()
        {
            StopAllCoroutines();
            _playerAnimator.SetWalk(false);
        }

        [Button]
        public void LockInput(bool isLocked)
        {
            IsInputLocked = isLocked;
            IsMoving = false;
            _playerAnimator.SetWalk(false);

            if (!isLocked)
            {
                _joystick.ResetPointer();
            }

            OnLockInput?.Invoke(isLocked);
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

            IsMoving = isWalking;

            if (IsMoving && _movementTutorIsShown)
            {
                //FirebaseAnalytics.LogEvent(BarberEventType.GameStarted);
                HideMovementTutor();
            }

            if (_playerAnimator != null)
            {
                _playerAnimator.SetWalk(isWalking);
                float targetSpeed = 0f;

                if (_data.InputDependentSpeed)
                    targetSpeed = Mathf.Lerp(_playerAnimator.GetSpeed(), input.magnitude, _data.MoveBlendSpeed * Time.deltaTime);

                if (_data.AlwaysRunning)
                    targetSpeed = 1f;

                _playerAnimator.SetSpeed(targetSpeed);
            }
            //TODO ANIMATOR HERE

            if (IsMoving)
            {
                LockCollision(false);
            }
        }

        public void MoveToPoint(Vector3 movePoint, Vector3 lookPoint, Action onComplete)
        {
            var duration = 0.4f;

            lookPoint.y = transform.position.y;
            var direction = lookPoint - movePoint;
            var rotation = Quaternion.LookRotation(direction);

            transform.DOMove(movePoint, duration).SetAutoKill();
            transform.DORotateQuaternion(rotation, duration).OnComplete(() => onComplete?.Invoke()).SetAutoKill();
        }
    }
}