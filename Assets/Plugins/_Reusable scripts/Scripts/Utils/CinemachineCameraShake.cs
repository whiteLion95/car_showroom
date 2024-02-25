using Cinemachine;
using UnityEngine;

namespace Utils
{
    public class CinemachineCameraShake : MonoBehaviour
    {
        [SerializeField] private float shakeIntensity;
        [SerializeField] private float shakeDuration;

        private CinemachineVirtualCamera _virtualCam;
        private CinemachineBasicMultiChannelPerlin _multiChannelPerlin;
        private float _shakeTimer;

        private void Awake()
        {
            _virtualCam = GetComponent<CinemachineVirtualCamera>();
            _multiChannelPerlin = _virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            _multiChannelPerlin.m_AmplitudeGain = 0f;
        }

        private void Update()
        {
            ShakeCountDown();
        }

        public void Shake(float intensity, float time)
        {
            _multiChannelPerlin.m_AmplitudeGain = intensity;
            _shakeTimer = time;
        }

        public void Shake()
        {
            Shake(shakeIntensity, shakeDuration);
        }

        private void ShakeCountDown()
        {
            if (_shakeTimer > 0f)
            {
                _shakeTimer -= Time.deltaTime;

                if (_shakeTimer <= 0f)
                {
                    _multiChannelPerlin.m_AmplitudeGain = 0f;
                }
            }
        }
    }
}
