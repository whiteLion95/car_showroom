using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "InteractableObjectsDataSO", menuName = "ScriptableObject/InteractableObjectsDataSO")]
public class InteractableObjectsDataSO : ScriptableObject
{
    [SerializeField] private float _jumpDuration = 1f;
    [SerializeField] private Ease _jumpEase = Ease.OutBack;
    [SerializeField] private float _jumpPower = 1f;
    [SerializeField] private float _rotateDuration = 80f;
    [SerializeField] private Ease _rotateEase = Ease.Linear;

    public float JumpDuration => _jumpDuration;
    public Ease JumpEase => _jumpEase;
    public float JumpPower => _jumpPower;
    public float RotateDuration => _rotateDuration;
    public Ease RotateEase => _rotateEase;
}
