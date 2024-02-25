using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private Transform _topPlace;

    private bool _isMoving;
    private float _speed;
    private Transform _curTarget;

    public Transform TopPlace => _topPlace;
    public bool IsInPlace { get; set; }
}
