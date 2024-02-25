using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "MovingAgentDataSO", menuName = "ScriptableObject/MovingAgentDataSO")]
public class MovingAgentDataSO : ScriptableObject
{
    [SerializeField] private float _moveSpeed = 5f;

    public float MoveSpeed => _moveSpeed;
}
