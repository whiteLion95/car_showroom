using PajamaNinja.SaveSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ResourcesBoxData : SaveDataInSO
{
    public List<ResourceType> curItemsTypes;
}

[CreateAssetMenu(fileName = "ResourcesBoxSSO", menuName = "ScriptableObject/Saveables/ResourcesBoxSSO")]
public class ResourcesBoxSSO : SaveableSO<ResourcesBoxData>
{
    public List<ResourceType> CurItemsTypes
    {
        get
        {
            if (!_saveIsLoaded)
                TrySave(false);

            TryLoad();
            _saveData.curItemsTypes ??= new();

            return _saveData.curItemsTypes;
        }

        set
        {
            _saveData.curItemsTypes = value;
            TrySave(false);
        }
    }

    public void AddItem(ResourceItem item)
    {
        _saveData.curItemsTypes.Add(item.MyType);
        TrySave();
    }

    public void ClearItems()
    {
        _saveData.curItemsTypes.Clear();
        TrySave();
    }
}
