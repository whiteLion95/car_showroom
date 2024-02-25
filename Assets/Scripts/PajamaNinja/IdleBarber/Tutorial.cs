using DG.Tweening;
using PajamaNinja.UpgradesSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PajamaNinja.CarShowRoom
{
    public class Tutorial : MonoBehaviour
    {
        [SerializeField]
        private IntUSO _unlockingSave;

        [SerializeField]
        private Player _player;

        private void Awake()
        {
            DOVirtual.DelayedCall(1f, StartTutor);
        }

        [Button]
        public void StartTutor()
        {
            if (_unlockingSave.CurrentValue > 1) return;
        }
    }
}