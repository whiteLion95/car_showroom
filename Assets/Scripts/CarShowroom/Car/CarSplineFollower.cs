using Dreamteck.Splines;
using System.Collections;
using UnityEngine;

public class CarSplineFollower : MonoBehaviour
{
    [SerializeField] private float _fromBackToForwardDelay = 0.5f;

    private SplineFollower _splineFollower;
    private bool _isGoingBack;

    public bool IsGoingBack => _isGoingBack;

    private void Awake()
    {
        _splineFollower = GetComponent<SplineFollower>();
    }

    public void Stop()
    {
        StartCoroutine(StopRoutine());
    }

    public void Destroy()
    {
        
    }

    private IEnumerator StopRoutine()
    {
        _splineFollower.follow = false;
        yield return new WaitForSeconds(_fromBackToForwardDelay);
        _isGoingBack = false;
        _splineFollower.follow = true;
    }
}
