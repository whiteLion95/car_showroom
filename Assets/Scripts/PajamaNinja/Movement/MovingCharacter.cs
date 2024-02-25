using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCharacter : MovingNavMeshAgent
{
    [SerializeField] private string _moveBoolParam = "isMoving";

    private Animator _anim;

    protected override void Awake()
    {
        base.Awake();
        _anim = GetComponentInChildren<Animator>();
    }

    public override void MoveTo(Vector3 pos)
    {
        base.MoveTo(pos);
        _anim.SetBool(_moveBoolParam, true);
    }

    protected override void ActionsOnDestination()
    {
        base.ActionsOnDestination();
        _anim.SetBool(_moveBoolParam, false);
    }
}
