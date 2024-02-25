using PajamaNinja.CarShowRoom;
using PajamaNinja.Common;
using PajamaNinja.UISystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdminDesk : SingleReference<AdminDesk>
{
    [SerializeField] private float _interactTime = 1.5f;
    [SerializeField] private UIScreen _carsPickUIScreen;

    private ColliderEvents _trigger;
    private Player _player;

    public Transform Trigger => _trigger.transform;
    public bool PlayerIsInteracting { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        _trigger = GetComponentInChildren<ColliderEvents>();

        _trigger.TriggerEnter += HandleTriggerEnter;
        _trigger.TriggerExit += HandleTriggerExit;
    }

    private void Start()
    {
        _player = Player.Instance;
    }

    private void ShowCarsPickScreen()
    {
        _carsPickUIScreen.Show();
    }

    private void HandleTriggerEnter(ColliderEvents colEvents, Collider other)
    {
        if (other.gameObject.Equals(_player.gameObject))
        {
            _player.InteractionHandler.StartInteraction(_interactTime, ShowCarsPickScreen);
            PlayerIsInteracting = true;
        }
    }

    private void HandleTriggerExit(ColliderEvents colEvents, Collider other)
    {
        if (other.gameObject.Equals(_player.gameObject))
        {
            _player.InteractionHandler.TerminateInteraction();
            PlayerIsInteracting = false;
        }
    }
}
