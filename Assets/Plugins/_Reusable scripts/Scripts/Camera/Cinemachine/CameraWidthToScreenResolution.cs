using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class CameraWidthToScreenResolution : MonoBehaviour
{
    [SerializeField] private float _horizontalFoV = 102.6556f;
    [SerializeField] private bool _update;

    private List<Camera> _cams;
    private List<CinemachineVirtualCamera> _virtCams;

    private void Awake()
    {
        _cams = new List<Camera>(GetComponentsInChildren<Camera>());
        _virtCams = new List<CinemachineVirtualCamera>(GetComponentsInChildren<CinemachineVirtualCamera>());
    }

    private void Start()
    {
        SetVertFOV(_horizontalFoV);
    }

    void Update()
    {
        if (_update)
            SetVertFOV(_horizontalFoV);
    }

    public void SetVertFOV(float horFOV)
    {
        float vertFOV = CalcVertFOV(horFOV);
        SetVirtCamsFOV(vertFOV);
        SetCamsFOV(vertFOV);
    }

    private float CalcVertFOV(float horFOV)
    {
        float halfWidth = Mathf.Tan(0.5f * horFOV * Mathf.Deg2Rad);
        float halfHeight = halfWidth * Screen.height / Screen.width;
        float verticalFoV = 2.0f * Mathf.Atan(halfHeight) * Mathf.Rad2Deg;
        return verticalFoV;
    }

    private void SetVirtCamsFOV(float vertFOV)
    {
        if (_virtCams != null)
        {
            for (int i = 0; i < _virtCams.Count; i++)
            {
                _virtCams[i].m_Lens.FieldOfView = vertFOV;
            }
        }
    }

    private void SetCamsFOV(float vertFOV)
    {
        if (_cams != null)
        {
            for (int i = 0; i < _cams.Count; i++)
            {
                _cams[i].fieldOfView = vertFOV;
            }
        }
    }
}
