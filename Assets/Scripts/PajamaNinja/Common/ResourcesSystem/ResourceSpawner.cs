using UnityEngine;

public class ResourceSpawner : ObjectSpawner
{
    [SerializeField] private ResourcesDataSO _resData;
    [SerializeField] private ResourceType _resType;

    public ResourcesDataSO ResData => _resData;
    public ResourceType ResType => _resType;

    private void Awake()
    {
        _objPrefab = _resData[_resType].resItem.gameObject;
    }

    public ResourceItem Spawn(Vector3 pos, Quaternion rot, Transform parent = null)
    {
        if (_objPrefab == null)
            _objPrefab = _resData[_resType].resItem.gameObject;

        return Spawn(_objPrefab, pos, rot, parent).GetComponent<ResourceItem>();
    }
}
