using UnityEngine;

public class ConveyorLent : MonoBehaviour
{
    [SerializeField] private Material _idleMaterial;
    [SerializeField] private Material _activeMaterial;

    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();

        Activate(false);
    }

    public void Activate(bool value)
    {
        if (value)
            _renderer.material = _activeMaterial;
        else
            _renderer.material = _idleMaterial;
    }
}
