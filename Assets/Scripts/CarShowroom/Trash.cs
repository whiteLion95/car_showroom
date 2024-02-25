using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour
{
    [SerializeField] private Transform _lid;
    [SerializeField] private Vector3 _openRot;
    [SerializeField] private float _openDuration = 0.3f;
    [SerializeField] private Ease _openEase = Ease.Linear;

    private ResourceDestroyer _resDestroyer;
    private Vector3 _lidOrigRot;

    private void Awake()
    {
        _resDestroyer = GetComponentInChildren<ResourceDestroyer>();
        _lidOrigRot = _lid.localRotation.eulerAngles;

        _resDestroyer.OnStackTrigger += HandleStackTrigger;
    }

    private void HandleStackTrigger(InteractableObjectsStack stack, bool enter)
    {
        Vector3 targetRot = enter ? _openRot : _lidOrigRot;
        _lid.DOLocalRotate(targetRot, _openDuration).SetEase(_openEase);
    }

    public void EnableCollider(bool on)
    {
        _resDestroyer.Collider.enabled = on;
    }
}
