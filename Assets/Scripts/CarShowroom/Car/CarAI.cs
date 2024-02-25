using ToolBox.Utils;
using UnityEngine;

public class CarAI : MonoBehaviour
{
    [SerializeField] private float _turnSpeed = 120f;

    private CarSplineFollower _carFollower;

    private void Update()
    {
        if (_carFollower)
        {
            Follow();
        }
    }

    public void SetFollower(CarSplineFollower follower)
    {
        _carFollower = follower;
    }

    private void Follow()
    {
        transform.position = _carFollower.transform.position;

        Vector3 lookDir;

        if (_carFollower.IsGoingBack)
            lookDir = -_carFollower.transform.forward;
        else
            lookDir = _carFollower.transform.forward;

        transform.SmoothLookAt(lookDir, Vector3.up, _turnSpeed);
    }
}
