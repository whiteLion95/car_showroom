using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourcesDataSO", menuName = "ScriptableObject/ResourcesDataSO")]
public class ResourcesDataSO : ScriptableObject
{
    [SerializeField] private List<ResourceData> _resourcesData;

    public ResourceData this[ResourceType resType]
    {
        get
        {
            return _resourcesData.Find((d) => d.resType == resType);
        }
    }
}

[Serializable]
public class ResourceData
{
    public ResourceType resType;
    public ResourceItem resItem;
    public Sprite resSprite;
}
