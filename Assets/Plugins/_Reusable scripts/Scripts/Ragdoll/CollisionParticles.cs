using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionParticles : MonoBehaviour
{
    private new Rigidbody rigidbody;
    private RagdollController ragdollController;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        ragdollController = GetComponentInParent<RagdollController>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (rigidbody.velocity.magnitude > 1f && collision.collider.CompareTag("Ground"))
        {
            ragdollController.collisionParticles.transform.position = collision.contacts[0].point;
            ragdollController.collisionParticles.Play();
        }
    }
}
