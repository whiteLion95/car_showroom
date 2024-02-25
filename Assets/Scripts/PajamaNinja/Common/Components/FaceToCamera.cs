using UnityEngine;

namespace MentalIdle
{
    public class FaceToCamera : MonoBehaviour
    {
        [SerializeField] private bool _useFace2d;
        
        private Camera _camera;
        private Transform _subject;

        private void Awake()
        {
            _camera = Camera.main;
            _subject = transform;
        }

        private void LateUpdate()
        {
            var dir = transform.position - _camera.transform.position;
            dir.y = _useFace2d ? 0 : dir.y;

            Vector3 look = _camera.transform.TransformDirection(Vector3.forward);
            
            var rot = Quaternion.LookRotation(look, Vector3.up);
            _subject.rotation = rot;
        }
    }
}
