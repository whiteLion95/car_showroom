using UnityEngine;

public class RobotArm : MonoBehaviour
{
    private Animator _anim;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    public void Work(bool value)
    {
        _anim.SetBool("isWorking", value);
    }
}
