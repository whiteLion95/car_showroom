using UnityEngine;

namespace PajamaNinja.Scripts.IdleBarber.General.Tutor.TutorViewLogic.ItemsSpawner
{
    public class TutorSpawnPosition : MonoBehaviour
    {
        [SerializeField] private TutorItemsSpawner.PositionId _id;
        [SerializeField] private Transform _transform;

        public TutorItemsSpawner.PositionId Id => _id;
        public Transform Transform => _transform;
    }
}