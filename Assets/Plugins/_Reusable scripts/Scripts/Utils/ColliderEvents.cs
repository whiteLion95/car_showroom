using System;
using UnityEngine;

public class ColliderEvents : MonoBehaviour
{
    public Action<ColliderEvents, Collider> TriggerEnter;
    public Action<ColliderEvents, Collider> TriggerExit;
    public Action<ColliderEvents, Collider> TriggerStay;
    public Action<ColliderEvents, Collision> CollisionEnter;
    public Action<ColliderEvents, Collision> CollisionExit;
    public Action<ColliderEvents, Collision> CollisionStay;

    private void OnTriggerEnter(Collider other)
    {
        TriggerEnter?.Invoke(this, other);
    }

    private void OnTriggerExit(Collider other)
    {
        TriggerExit?.Invoke(this, other);
    }

    private void OnTriggerStay(Collider other)
    {
        TriggerStay?.Invoke(this, other);
    }

    private void OnCollisionEnter(Collision collision)
    {
        CollisionEnter?.Invoke(this, collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        CollisionExit?.Invoke(this, collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        CollisionStay?.Invoke(this, collision);
    }
}