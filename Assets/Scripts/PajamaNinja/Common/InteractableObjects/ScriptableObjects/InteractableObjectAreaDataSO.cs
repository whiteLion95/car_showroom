using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InteractableObjectAreaDataSO", menuName = "ScriptableObject/InteractableObjectAreaDataSO")]
public class InteractableObjectAreaDataSO : ScriptableObject
{
    [SerializeField] private int _maxCapacity;
    [SerializeField] private float _sendObjectsInterval = 1f;

    public int MaxCapacity { get => _maxCapacity; }
    public float SendObjectsInterval { get => _sendObjectsInterval; }
}
