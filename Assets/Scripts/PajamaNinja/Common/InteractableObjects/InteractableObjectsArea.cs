using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(InteractableObjectSender))]
public class InteractableObjectsArea : InteractableObjectsTaker
{
    [SerializeField] private InteractableObjectAreaDataSO _data;
    [SerializeField] private List<InteractableObjectsStack> _objectStacks;

    public Action OnEmpty;

    private int _curStackIndex = -1;
    private Coroutine _sendObjectsRoutine;
    private InteractableObjectsTaker _curTaker;

    public int MaxCapacity => _data.MaxCapacity;
    public InteractableObjectsTaker CurTaker => _curTaker;
    public  InteractableObjectsStack CurStack
    {
        get
        {
            if (_curStackIndex >= 0 && _curStackIndex < _objectStacks.Count)
                return _objectStacks[_curStackIndex];
            else
                return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        InteractableObjectsTaker taker = other.GetComponentInChildren<InteractableObjectsTaker>();

        if (taker)
        {
            _sendObjectsRoutine = StartCoroutine(SendObjectsRoutine(taker));

            if (!_curTaker)
                _curTaker = taker;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InteractableObjectsTaker taker = other.GetComponentInChildren<InteractableObjectsTaker>();

        if (taker)
        {
            if (_sendObjectsRoutine != null)
                StopCoroutine(_sendObjectsRoutine);

            if (_curTaker && _curTaker.Equals(taker))
                _curTaker = null;
        }
    }

    public override Transform GetObjectPlace()
    {
        IncreaseCurStackIndex();
        return CurStack.GetObjectPlace();
    }

    public override void TakeObj(InteractableObject interObj, InteractableObjectsDataSO data)
    {
        base.TakeObj(interObj, data);
        CurStack.TakeObj(interObj, data);
    }

    private void IncreaseCurStackIndex()
    {
        _curStackIndex++;

        if (_curStackIndex == _objectStacks.Count)
            _curStackIndex = 0;
    }

    private void DecreaseCurStackIndex()
    {
        if (_curStackIndex > 0)
            _curStackIndex--;
        else
            _curStackIndex = _objectStacks.Count - 1;
    }

    public int GetCurCount()
    {
        int count = 0;

        for (int i = 0; i < _objectStacks.Count; i++)
        {
            count += _objectStacks[i].StackedObjects.Count;
        }

        return count;
    }

    private IEnumerator SendObjectsRoutine(InteractableObjectsTaker taker)
    {
        while (taker.CanTakeObjects())
        {
            int curCount = GetCurCount();

            if (curCount > 0 && CurStack)
            {
                if (CurStack.StackedObjects.Count > 0 && CurStack.StackedObjects.Last().IsInPlace)
                    CurStack.SendTopObject(taker, true);
                
                DecreaseCurStackIndex();

                if (curCount == 1)
                    OnEmpty?.Invoke();

                yield return new WaitForSeconds(_data.SendObjectsInterval);
            }
            else
                break;
        }

        _sendObjectsRoutine = null;
    }

    public override bool CanTakeObjects()
    {
        return GetCurCount() < _data.MaxCapacity;
    }
}
