using UnityEngine;

public class RagdollController : MonoBehaviour
{
    [SerializeField] private GameObject ragdoll;
    [SerializeField] private GameObject animatedModel;
    public ParticleSystem collisionParticles;
    public bool dead = false;

    private void Awake()
    {
        ragdoll.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ToggleDead();
        }
    }

    private void ToggleDead()
    {
        dead = !dead;

        if (dead)
        {
            CopyTransformData(animatedModel.transform, ragdoll.transform);
            ragdoll.gameObject.SetActive(true);
            animatedModel.gameObject.SetActive(false);
        }
        else
        {
            // Switch back to the model and disable the ragdoll
            ragdoll.gameObject.SetActive(false);
            animatedModel.gameObject.SetActive(true);
        }
    }


    private void CopyTransformData(Transform sourceTransform, Transform destinationTransform)
    {
        if (sourceTransform.childCount != destinationTransform.childCount)
        {
            Debug.LogWarning("Invalid transform copy, they need to match transform hierarchies");
            return;
        }

        for (int i = 0; i < sourceTransform.childCount; i++)
        {
            var source = sourceTransform.GetChild(i);
            var destination = destinationTransform.GetChild(i);
            destination.position = source.position;
            destination.rotation = source.rotation;
            
            CopyTransformData(source, destination);
        }
    }
}
