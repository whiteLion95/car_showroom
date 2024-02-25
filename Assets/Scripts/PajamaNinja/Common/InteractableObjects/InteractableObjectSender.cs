using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObjectSender : MonoBehaviour
{
    [SerializeField] private InteractableObjectsDataSO _interObjData;

    public Action<InteractableObject, InteractableObjectsTaker, bool> OnSend;
    public Action OnCantSendObjects;

    public void SendInterObj(InteractableObject interObj, InteractableObjectsTaker taker, bool local = false, bool instant = false)
    {
        if (taker.CanTakeObjects())
        {
            interObj.IsInPlace = false;
            Transform targetPlace = taker.GetObjectPlace();
            taker.TakeObj(interObj, _interObjData);

            if (targetPlace != null)
            {
                Sequence sendSequence = null;

                if (local)
                {
                    interObj.transform.SetParent(targetPlace);

                    if (!instant)
                        sendSequence = interObj.transform.DOLocalJump(Vector3.zero, _interObjData.JumpPower, 1, _interObjData.JumpDuration).SetEase(_interObjData.JumpEase);
                    else
                        interObj.transform.localPosition = Vector3.zero;
                }
                else
                {
                    if (!instant)
                        sendSequence = interObj.transform.DOLocalJump(targetPlace.position, _interObjData.JumpPower, 1, _interObjData.JumpDuration).SetEase(_interObjData.JumpEase);
                    else
                        interObj.transform.position = targetPlace.position;
                }

                if (!instant)
                    sendSequence.onComplete += () => taker.HandleObjInPlace(interObj);
                else
                    taker.HandleObjInPlace(interObj);

                OnSend?.Invoke(interObj, taker, local);
            }
        }
        else
            OnCantSendObjects?.Invoke();
    }

    public void SendInterObjs(List<InteractableObject> interObjs, InteractableObjectsTaker taker, float interval, bool local = false)
    {
        StartCoroutine(SendObjsRoutine(interObjs, taker, interval, local));
    }

    private IEnumerator SendObjsRoutine(List<InteractableObject> interObjs, InteractableObjectsTaker taker, float interval, bool local = false)
    {
        for (int i = 0; i < interObjs.Count; i++)
        {
            SendInterObj(interObjs[i], taker, local);
            yield return new WaitForSeconds(interval);
        }
    }
}