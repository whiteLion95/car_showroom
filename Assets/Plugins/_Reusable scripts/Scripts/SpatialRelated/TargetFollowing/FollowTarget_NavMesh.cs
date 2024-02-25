using UnityEngine;
using UnityEngine.AI;

public class FollowTarget_NavMesh : FollowTarget
{
    private NavMeshAgent _agent;

    protected override void Awake()
    {
        base.Awake();

        _agent = GetComponent<NavMeshAgent>();
    }

    protected override void Start()
    {
        base.Start();

        _agent.stoppingDistance = stoppingDistance;
        _agent.speed = followSpeed;
    }

    protected override void Follow()
    {
        if (_agent.isOnNavMesh)
            _agent.SetDestination(target.position);
    }
}
