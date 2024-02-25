using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MovingNavMeshAgent : MonoBehaviour
{
    [SerializeField] private MovingAgentDataSO _data;

    public Action OnDestination;

    private NavMeshAgent _agent;
    private bool _isMoving;

    public NavMeshAgent Agent => _agent;

    protected virtual void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Update()
    {
        if (_isMoving)
        {
            CheckDestination();
        }
    }

    public virtual void MoveTo(Vector3 pos)
    {
        _agent.SetDestination(pos);
        _isMoving = true;
    }

    private void CheckDestination()
    {
        if (!_agent.pathPending)
        {
            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
                {
                    _isMoving = false;
                    ActionsOnDestination();
                }
            }
        }
    }

    protected virtual void ActionsOnDestination()
    {
        OnDestination?.Invoke();
    }
}
