using DG.Tweening;
using Lean.Pool;
using System;
using System.Collections;
using UnityEngine;

public class ResourceDestroyer : InteractableObjectsTaker
{
    [SerializeField] private float _despawnDelay = 0.3f;

    public Action<InteractableObjectsStack, bool> OnStackTrigger;

    public Collider Collider { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        Collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        InteractableObjectsStack stack = other.GetComponentInChildren<InteractableObjectsStack>();

        if (stack)
        {
            stack.StartSendingTopObjs(this);
            OnStackTrigger?.Invoke(stack, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InteractableObjectsStack stack = other.GetComponentInChildren<InteractableObjectsStack>();

        if (stack)
        {
            stack.StopSendingPeekObjs();
            OnStackTrigger?.Invoke(stack, false);
        }
    }

    public override void TakeObj(InteractableObject interObj, InteractableObjectsDataSO data)
    {
        base.TakeObj(interObj, data);
    }

    public override void HandleObjInPlace(InteractableObject interObj)
    {
        base.HandleObjInPlace(interObj);
        StartCoroutine(DespawnRoutine(interObj));
    }

    private IEnumerator DespawnRoutine(InteractableObject interObj)
    {
        yield return new WaitForSeconds(_despawnDelay);
        LeanPool.Despawn(interObj.gameObject);
    }
}
