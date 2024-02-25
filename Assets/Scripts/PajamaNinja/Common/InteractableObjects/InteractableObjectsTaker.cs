using UnityEngine;

public class InteractableObjectsTaker : MonoBehaviour
{
    [SerializeField] private Transform _objPlace;

    #region UnityMethods
    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {

    }
    #endregion

    public virtual void TakeObj(InteractableObject interObj, InteractableObjectsDataSO data)
    {
        
    }

    public virtual Transform GetObjectPlace()
    {
        if (_objPlace)
            return _objPlace;
        else
            return transform;
    }

    public virtual void HandleObjInPlace(InteractableObject interObj)
    {
        interObj.IsInPlace = true;
    }

    public virtual bool CanTakeObjects()
    {
        return true;
    }
}