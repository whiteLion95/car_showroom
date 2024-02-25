using Cinemachine;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

/// <summary>
/// A base class for virtual cameras managers
/// </summary>
public class VirtualCamerasManager : MonoBehaviour
{
    public float changeTargetDamping = 2f;

    public Action<string> OnCameraSwitched;

    protected CinemachineVirtualCamera[] virtualCameras;
    protected CinemachineVirtualCamera currentCamera;
    private float _origDamping;

    public static VirtualCamerasManager Instance { get; private set; }
    public CinemachineVirtualCamera CurrentCamera { get { return currentCamera; } }

    protected virtual void Awake()
    {
        Instance = this;

        virtualCameras = GetComponentsInChildren<CinemachineVirtualCamera>();
        SetCurrentCamera();
    }

    public virtual void SwitchCamera(string targetCamName, Transform followTarget = null, float delay = 0f)
    {
        CinemachineVirtualCamera targetCam = virtualCameras.Single(cam => cam.gameObject.name == targetCamName);

        if (followTarget)
            targetCam.Follow = followTarget;

        if (!targetCam.Equals(currentCamera))
        {
            if (targetCam == null)
            {
                Debug.LogError("There is no virtual cameras with gameobject's name: " + targetCamName);
            }
            else
            {
                StartCoroutine(SwitchCameraRoutine(targetCam, delay));
            }
        }
    }

    public virtual void SwitchCameraAndBack(string targetCamName, Transform followTarget = null, float delay = 0f, float showDuration = 1f)
    {
        StartCoroutine(SwitchCameraAndBackRoutine(targetCamName, followTarget, delay, showDuration));
    }

    private IEnumerator SwitchCameraRoutine(CinemachineVirtualCamera targetCam, float delay)
    {
        yield return new WaitForSeconds(delay);
        (currentCamera.Priority, targetCam.Priority) = (targetCam.Priority, currentCamera.Priority);
        SetCurrentCamera();
        OnCameraSwitched?.Invoke(targetCam.name);
    }

    private IEnumerator SwitchCameraAndBackRoutine(string targetCamName, Transform followTarget = null, float delay = 0f, float showDuration = 1f)
    {
        string prevCamName = currentCamera.name;
        SwitchCamera(targetCamName, followTarget, delay);
        yield return new WaitForSeconds(showDuration);
        SwitchCamera(prevCamName);
    }

    protected void SetCurrentCamera()
    {
        CinemachineVirtualCamera camWithHighestPriority = virtualCameras[0];

        for (int i = 1; i < virtualCameras.Length; i++)
        {
            camWithHighestPriority = (virtualCameras[i].Priority > camWithHighestPriority.Priority) ? virtualCameras[i] : camWithHighestPriority;
        }

        currentCamera = camWithHighestPriority;
    }

    protected CinemachineVirtualCamera GetCam(string name)
    {
        foreach (var cam in virtualCameras)
        {
            if (cam.gameObject.name == name)
            {
                return cam;
            }
        }

        return null;
    }

    public virtual void SetTarget(Transform target)
    {
        currentCamera.Follow = target;
        currentCamera.LookAt = target;
    }

    public virtual void SetTarget(Transform target, string camName)
    {
        CinemachineVirtualCamera cam = GetCam(camName);

        if (cam != null)
        {
            cam.Follow = target;
        }
    }

    public void Follow(Transform target)
    {
        currentCamera.Follow = target;
    }
}