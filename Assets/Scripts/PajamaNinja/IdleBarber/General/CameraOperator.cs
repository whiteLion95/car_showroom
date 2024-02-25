using Cinemachine;
using Cysharp.Threading.Tasks;
using PajamaNinja.CarShowRoom;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PajamaNinja.Common
{
    public class CameraOperator : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _cam;
        [SerializeField] private Player _player;
        [SerializeField] private float _showObjDamping = 1f;

        private readonly Queue<ShowRequest> _requests = new();
        private bool _isCurrentlyHandlingRequests;
        private float _origDamping;

        private void Awake()
        {
            _origDamping = _cam.GetCinemachineComponent<CinemachineTransposer>().m_XDamping;
        }

        public void ShowObject(Transform obj, float duration = 2f, Action onCompleted = null)
        {
            var showRequest = new ShowRequest(obj, () => UniTask.Delay(TimeSpan.FromSeconds(duration),
                cancellationToken: this.GetCancellationTokenOnDestroy()));
            ShowObject(showRequest, onCompleted);
        }

        public void ShowObject(Transform obj, Func<bool> waitWhile)
        {
            var showRequest = new ShowRequest(obj,
                () => UniTask.WaitWhile(waitWhile, cancellationToken: this.GetCancellationTokenOnDestroy()));
            ShowObject(showRequest);
        }

        public void ShowObject(Transform obj, float delay, float duration)
        {
            StartCoroutine(ShowObjectRoutine(obj, delay, duration));
        }

        private IEnumerator ShowObjectRoutine(Transform obj, float delay, float duration = 2f)
        {
            yield return new WaitForSeconds(delay);
            ShowObject(obj, duration);
        }

        private void ShowObject(ShowRequest request, Action onCompleted = null)
        {
            _requests.Enqueue(request);
            if (_isCurrentlyHandlingRequests)
                return;

            _isCurrentlyHandlingRequests = true;
            StartHandleRequests(onCompleted).Forget();
        }

        private async UniTaskVoid StartHandleRequests(Action onCompleted = null)
        {
            _isCurrentlyHandlingRequests = true;
            _player.Movement?.LockInput(true);

            while (_requests.Count > 0)
            {
                var request = _requests.Dequeue();
                if (request.Transform == null) continue;
                SetDamping(_showObjDamping);
                _cam.Follow = request.Transform;
                await request.GetTask.Invoke();
            }

            _player.Movement?.LockInput(false);
            _cam.Follow = _player.transform;
            _isCurrentlyHandlingRequests = false;

            onCompleted?.Invoke();
        }

        private class ShowRequest
        {
            public Transform Transform;
            public Func<UniTask> GetTask;

            public ShowRequest(Transform transform, Func<UniTask> getTask)
            {
                Transform = transform;
                GetTask = getTask;
            }
        }

        private void SetDamping(float damping)
        {
            CinemachineTransposer transposer = _cam.GetCinemachineComponent<CinemachineTransposer>();
            transposer.m_XDamping = damping;
            transposer.m_YDamping = damping;
            transposer.m_ZDamping = damping;
        }
    }
}