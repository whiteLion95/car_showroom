using PajamaNinja.Common;
using PajamaNinja.CurrencySystem;
using PajamaNinja.Scripts.IdleBarber.Player;
using System.Collections.Generic;
using UnityEngine;

namespace PajamaNinja.CarShowRoom
{
    public class Player : SingleReference<Player>
    {
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private InteractionHandler _interactionHandler;
        [SerializeField] private CurrencySpender _currencySpender;
        [SerializeField] private Compass _playerCompass;
        [Header("Animation")]
        [SerializeField] private float _changeAnimLayerSpeed = 1f;

        private Animator _anim;
        private bool _toSetLayerWeight;
        private int _targetLayerWeight;
        private float _changeLayerCounter;
        private InteractableObjectsStack _stack;
        private List<int> _targetLayers;

        public PlayerMovement Movement => _playerMovement;
        public InteractionHandler InteractionHandler => _interactionHandler;
        public Compass Compass => _playerCompass;
        public Joystick Joystick => _playerMovement.Joystick;
        public InteractableObjectsStack Stack => _stack;
        public PlayerRider Rider { get; private set; }
        public bool CanBuyUnlocks { get; set; } = true;

        protected override void Awake()
        {
            base.Awake();
            _anim = GetComponentInChildren<Animator>();
            _stack = GetComponentInChildren<InteractableObjectsStack>();
            _targetLayers = new List<int> { 1, 2 };
            Rider = GetComponent<PlayerRider>();

            _stack.OnFirstObjectTake += HandleStackFirstObjTake;
            _stack.OnLastObjectSend += HandleStackLastObjSend;
        }

        private void Update()
        {
            if (_toSetLayerWeight)
                SetTargetLayers();
        }

        private void OnEnable()
        {
            if (_stack.StackedObjects != null && _stack.StackedObjects.Count > 0)
                StartSettingLayers(1);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (CanBuyUnlocks && other.gameObject.TryGetComponent<CurrencyReceiver>(out var receiver))
            {
                _currencySpender.StartSpendingMoney(receiver, _playerMovement);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent<CurrencyReceiver>(out var receiver))
            {
                _currencySpender.StopSpendingMoney(receiver);
            }
        }

        public void StartSettingLayers(int weight)
        {
            _targetLayerWeight = weight;
            _toSetLayerWeight = true;
        }

        private void SetTargetLayers()
        {
            for (int i = 0; i < _targetLayers.Count; i++)
            {
                int layer = _targetLayers[i];
                _anim.SetLayerWeight(layer, Mathf.Lerp(_anim.GetLayerWeight(layer), _targetLayerWeight, _changeLayerCounter));
                _changeLayerCounter += Time.deltaTime * _changeAnimLayerSpeed;

                if (_anim.GetLayerWeight(layer) == _targetLayerWeight)
                {
                    _changeLayerCounter = 0f;
                    _toSetLayerWeight = false;
                }
            }
        }

        private void HandleStackFirstObjTake(InteractableObject interObj)
        {
            StartSettingLayers(1);
        }

        private void HandleStackLastObjSend(InteractableObject interObj)
        {
            StartSettingLayers(0);
        }
    }
}