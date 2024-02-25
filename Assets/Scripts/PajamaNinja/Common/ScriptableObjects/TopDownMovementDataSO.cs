using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "TopDownMovementDataSO", menuName = "ScriptableObject/TopDownMovementDataSO")]
public class TopDownMovementDataSO : ScriptableObject
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotSpeed = 15f;
    [SerializeField] private float _gravity = 100f;
    [SerializeField] private bool _inputDependentSpeed = true;
    [ShowIf("_inputDependentSpeed")][SerializeField] private float _moveAnimBlendSpeed = 10f;
    [SerializeField] private bool _alwaysRunning = true;

    public float MoveSpeed => _moveSpeed;
    public float RotSpeed { get => _rotSpeed; }
    public float MoveBlendSpeed { get => _moveAnimBlendSpeed; }
    public float Gravity { get => _gravity; }
    public bool InputDependentSpeed { get => _inputDependentSpeed; }
    public bool AlwaysRunning { get => _alwaysRunning; }
}
