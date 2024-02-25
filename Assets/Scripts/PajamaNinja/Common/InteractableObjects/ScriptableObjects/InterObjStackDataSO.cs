using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InterObjStackDataSO", menuName = "ScriptableObject/InterObjStackDataSO")]
public class InterObjStackDataSO : ScriptableObject
{
    [SerializeField] private bool _cappedCapacity = true;
    [ShowIf("_cappedCapacity")][SerializeField] private int _maxCapacity = 8;
    [SerializeField] private float _sendObjectsInterval = 0.1f;

    public bool CappedCapacity { get => _cappedCapacity; }
    public int MaxCapacity { get => _maxCapacity; set => _maxCapacity = value; }
    public float SendObjectsInterval { get => _sendObjectsInterval; }
}
