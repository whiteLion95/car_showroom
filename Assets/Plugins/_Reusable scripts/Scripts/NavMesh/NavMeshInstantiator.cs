using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Used to make sure the GO is on the NavMesh.
/// </summary>
public class NavMeshInstantiator : MonoBehaviour
{
    void Start()
    {
        NavMeshHit closestHit;

        if (NavMesh.SamplePosition(gameObject.transform.position, out closestHit, 500f, NavMesh.AllAreas))
            gameObject.transform.position = closestHit.position;
        else
            Debug.LogError("Could not find position on NavMesh!");
    }
}
