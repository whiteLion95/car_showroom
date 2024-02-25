using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] protected Transform target;
    [SerializeField] protected float followSpeed = 1f;
    [SerializeField] protected float stoppingDistance;

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        Follow();   
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    protected virtual void Follow()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (distanceToTarget > stoppingDistance)
            transform.position = Vector3.MoveTowards(transform.position, target.position, followSpeed * Time.deltaTime);
    }
}
