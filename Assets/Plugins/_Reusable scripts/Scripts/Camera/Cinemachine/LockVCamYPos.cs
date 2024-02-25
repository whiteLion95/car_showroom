using Cinemachine;
using UnityEngine;

namespace CameraUtils.Cinemachine
{
    /// <summary>
    /// An add-on module for Cinemachine Virtual Camera that locks the camera's Z co-ordinate
    /// </summary>
    [ExecuteInEditMode]
    [SaveDuringPlay]
    [AddComponentMenu("")] // Hide in menu
    public class LockVCamYPos : CinemachineExtension
    {
        private float _yPos;

        protected override void Awake()
        {
            base.Awake();
            _yPos = (VirtualCamera as CinemachineVirtualCamera).GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y;
        }

        protected override void PostPipelineStageCallback(
            CinemachineVirtualCameraBase vcam,
            CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            if (stage == CinemachineCore.Stage.Body)
            {
                var pos = state.RawPosition;
                pos.y = _yPos;
                state.RawPosition = pos;
            }
        }
    }
}