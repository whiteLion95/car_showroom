using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(InteractableObjectSender))]
public class InteractableObjectsStack : InteractableObjectsTaker
{
    [SerializeField] private InterObjStackDataSO _data;
    [SerializeField] private bool _isDynamic;
    [SerializeField] private bool _showTextOnFull;
    [ShowIf("_showTextOnFull")][SerializeField] private DOTweenAnimation _onFullText;
    [ShowIf("_showTextOnFull")][SerializeField] private float _textYOffset = 2f;

    protected List<InteractableObject> _stackedObjects = new();
    protected InteractableObjectSender _sender;
    private Coroutine _sendObjsRoutine;
    private Coroutine _fullTextRoutine;
    private int _maxCapacity;
    private bool _capacitySet;

    public Action<InteractableObject> OnFirstObjectTake;
    public Action<InteractableObject> OnLastObjectSend;
    public Action OnFull;
    public Action OnCountChanged;

    public List<InteractableObject> StackedObjects => _stackedObjects;
    public bool IsDynamic => _isDynamic;
    public InterObjStackDataSO Data => _data;

    #region UnityMethods
    protected override void Awake()
    {
        base.Awake();
        _sender = GetComponent<InteractableObjectSender>();

        if (!_capacitySet)
            _maxCapacity = _data.MaxCapacity;

        if (_isDynamic)
            gameObject.AddComponent<Rigidbody>().isKinematic = true;
    }

    protected override void Start()
    {
        base.Start();

        _sender.OnSend += HandleObjectSent;
    }
    #endregion

    #region Methods
    public override void TakeObj(InteractableObject interObj, InteractableObjectsDataSO data)
    {
        base.TakeObj(interObj, data);
        _stackedObjects.Add(interObj);
        interObj.transform.DOLocalRotate(Vector3.zero, data.RotateDuration).SetEase(data.RotateEase);
        OnCountChanged?.Invoke();

        if (_stackedObjects.Count == 1)
            OnFirstObjectTake?.Invoke(interObj);
    }

    public override void HandleObjInPlace(InteractableObject interObj)
    {
        base.HandleObjInPlace(interObj);

        if (_data.CappedCapacity && _stackedObjects.IndexOf(interObj) == _maxCapacity - 1)
            ActionsOnFull();

        if (_isDynamic)
        {
            StackableObject stackObj = interObj.gameObject.GetComponent<StackableObject>();

            if (!stackObj)
                stackObj = interObj.gameObject.AddComponent<StackableObject>();

            stackObj.transform.SetParent(null);
            stackObj.AddToStack(this);
        }
    }

    public override Transform GetObjectPlace()
    {
        if (_stackedObjects.Count > 0)
            return _stackedObjects.Last().TopPlace;
        else
            return transform;
    }

    public void SendTopObject(InteractableObjectsTaker taker, bool local = false)
    {
        SendObject(_stackedObjects.Last(), taker, local);
    }

    public void SendObject(InteractableObject interObj, InteractableObjectsTaker taker, bool local = false)
    {
        if (_stackedObjects.Count > 0 && _stackedObjects.Contains(interObj))
        {
            _sender.SendInterObj(interObj, taker, local);
        }
    }

    public void StartSendingTopObjs(InteractableObjectsTaker taker, bool local = false)
    {
        _sendObjsRoutine = StartCoroutine(SendTopObjsRoutine(taker));
    }

    public void SendObjects(List<InteractableObject> interObjs, InteractableObjectsTaker taker, bool local = false)
    {
        _sender.SendInterObjs(interObjs, taker, _data.SendObjectsInterval, local);
    }

    public void StopSendingPeekObjs()
    {
        if (_sendObjsRoutine != null)
            StopCoroutine(_sendObjsRoutine);
    }

    public void SetCapacity(int amount)
    {
        _maxCapacity = amount;
        _capacitySet = true;
    }

    public void ResetCapacity()
    {
        _maxCapacity = _data.MaxCapacity;
    }

    private IEnumerator SendTopObjsRoutine(InteractableObjectsTaker taker, bool local = false)
    {
        while (_stackedObjects.Count > 0)
        {
            SendTopObject(taker, local);
            yield return new WaitForSeconds(_data.SendObjectsInterval);
        }
    }

    public override bool CanTakeObjects()
    {
        if (_data.CappedCapacity)
            return _stackedObjects.Count < _maxCapacity;
        else
            return true;
    }

    private void HandleObjectSent(InteractableObject interObj, InteractableObjectsTaker taker, bool local)
    {
        if (_stackedObjects.Count > 0 && _stackedObjects.Contains(interObj))
        {
            if (!interObj.Equals(_stackedObjects.Last()))
            {
                InteractableObject upperObj = _stackedObjects[_stackedObjects.IndexOf(interObj) + 1];
                upperObj.transform.SetParent(interObj.transform.parent);
                upperObj.transform.localPosition = Vector3.zero;
            }
            
            if (_stackedObjects.Count == _maxCapacity)
                SetOnFullText(false);

            _stackedObjects.Remove(interObj);

            if (_stackedObjects.Count == 0)
                OnLastObjectSend?.Invoke(interObj);

            if (!local)
                interObj.transform.SetParent(null);
        }
    }

    private void ActionsOnFull()
    {
        if (_showTextOnFull)
        {
            if (_fullTextRoutine != null)
                StopCoroutine(_fullTextRoutine);

            _fullTextRoutine = StartCoroutine(ShowOnFullTextRoutine(true, 0.1f));
        }

        OnFull?.Invoke();
    }

    private void SetOnFullText(bool value)
    {
        if (value)
            _onFullText.transform.position = _stackedObjects.Last().TopPlace.position + new Vector3(0f, _textYOffset, 0f);

        _onFullText.gameObject.SetActive(value);

        if (value)
            _onFullText.DORestart();

    }

    private IEnumerator ShowOnFullTextRoutine(bool value, float delay)
    {
        yield return new WaitForSeconds(delay);
        SetOnFullText(value);
        _fullTextRoutine = null;
    }
    #endregion
}
