using System;
using UnityEngine;

//Custom resource types for different projects
[Serializable]
public enum ResourceType
{
    Gear,
    MetalSheet,
    Wheel
}

public class ResourceItem : MonoBehaviour
{
    [SerializeField] private ResourceType _myType;

    public ResourceType MyType { get => _myType; set => _myType = value; }
}
