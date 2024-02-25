using UnityEngine;

public class StackableObject : MonoBehaviour
{
    public bool isAdded;

    private Rigidbody _rB;
    private HingeJoint _joint;

    private void Awake()
    {
        _rB = GetComponent<Rigidbody>();
        _joint = GetComponent<HingeJoint>();

        _rB.isKinematic = true;
    }

    public void AddToStack(InteractableObjectsStack stack)
    {
        if (stack.TryGetComponent(out Rigidbody stackRB))
        {
            _joint.connectedBody = stackRB;
            _rB.isKinematic = false;
        }
    }

    public void RemoveFromStack()
    {
        _joint.connectedBody = null;
        _rB.isKinematic = true;
    }
}
