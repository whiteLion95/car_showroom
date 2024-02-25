using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] protected Animator _animator;

    protected readonly int _isWalking = Animator.StringToHash("IsWalking");
    protected readonly int _speed = Animator.StringToHash("speed");

    public Animator Animator => _animator;

    public void SetWalk(bool state)
    {
        _animator.SetBool(_isWalking, state);
    }

    public void SetSpeed(float value)
    {
        _animator.SetFloat(_speed, value);
    }

    public float GetSpeed()
    {
        return _animator.GetFloat(_speed);
    }
}
