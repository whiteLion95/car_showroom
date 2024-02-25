using System;
using UnityEngine;

namespace PajamaNinja.Scripts.IdleBarber.General
{
    public interface ICharacterMovement
    {
        public void MoveTo(Vector3 position, Action onReached);
        public void StopAnyMovement();
        public void LockInput(bool isLocked);
        public bool IsPositionNear(Vector3 position, bool useNavMesh = true);
        public void MoveToPoint(Vector3 movePoint, Vector3 lookPoint, Action action);
    }
}