using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ResourceSpawner))]
public class UnloadArea : InteractableObjectsArea
{
    [SerializeField] private Transform _unloadPlace;
    [SerializeField] private bool _fullOnAwake;
    [SerializeField] private Image _resourceIcon;

    private ResourceSpawner _resourceSpawner;
    private InteractableObjectSender _sender;
    private Collider _collider;

    public Transform UnloadPlace => _unloadPlace;
    public bool FullOnAwake => _fullOnAwake;
    public ResourceType ResType => _resourceSpawner.ResType;

    protected override void Awake()
    {
        base.Awake();

        _resourceSpawner = GetComponent<ResourceSpawner>();
        _sender = GetComponent<InteractableObjectSender>();
        _collider = GetComponent<Collider>();
    }

    protected override void Start()
    {
        base.Start();

        Init();
    }

    private void Init()
    {
        if (_resourceIcon)
            _resourceIcon.sprite = _resourceSpawner.ResData[_resourceSpawner.ResType].resSprite;

        if (_fullOnAwake)
        {
            Fill();
        }
    }

    public void EnableCollider(bool on)
    {
        _collider.enabled = on;
    }

    private void Fill()
    {
        for (int i = 0; i < MaxCapacity; i++)
        {
            InteractableObject interObj = _resourceSpawner.Spawn(transform.position, Quaternion.identity).GetComponent<InteractableObject>();
            _sender.SendInterObj(interObj, this, true, true);
        }
    }
}
