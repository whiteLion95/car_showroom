using DG.Tweening;
using System;
using UnityEngine;

namespace PajamaNinja.CarShowRoom
{
    public class PlayerRider : CarRider
    {
        [SerializeField] private float _outOfCarJumpPower = 1f;
        [SerializeField] private float _outOfCarJumpDuration = 0.5f;
        [SerializeField] private Ease _outOfCarJumpEase = Ease.Linear;

        private VirtualCamerasManager _camManager;
        private Player _player;

        protected override void Start()
        {
            base.Start();
            _camManager = VirtualCamerasManager.Instance;
            _player = Player.Instance;
        }

        public override void GetInTheCar(Car car)
        {
            base.GetInTheCar(car);
            _player.Movement.LockInput(true);
        }

        public override void GetOutOfTheCar(Car car)
        {
            base.GetOutOfTheCar(car);
            gameObject.SetActive(true);

            transform.DOJump(car.CurParkingLot.GetOutPlace.position, _outOfCarJumpPower, 1, _outOfCarJumpDuration).SetEase(_outOfCarJumpEase).
                    OnComplete(() =>
                    {
                        _player.Movement.LockInput(false);
                    });
        }
    }
}