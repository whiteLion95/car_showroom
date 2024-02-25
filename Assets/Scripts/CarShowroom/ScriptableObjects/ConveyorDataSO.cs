using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConveyorDataSO", menuName = "ScriptableObject/ConveyorDataSO")]
public class ConveyorDataSO : ScriptableObject
{
    [SerializeField] private float _nextCarSpawnDelay = 2f;
    [SerializeField] private List<float> _stagesDelays;
    [SerializeField] private float _carStageMoveSpeed = 3f;
    [SerializeField] private float _outOfConveyorDuration = 3f;

    public float NextCarSpawnDelay => _nextCarSpawnDelay;
    public List<float> StagesDelays => _stagesDelays;
    public float CarStageMoveSpeed => _carStageMoveSpeed;
    public float OutOfConveyorDuration => _outOfConveyorDuration;
}
