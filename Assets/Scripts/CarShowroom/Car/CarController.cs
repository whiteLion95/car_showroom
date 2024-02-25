using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : TopDownMovement
{
    private void Awake()
    {
        IsInputLocked = true;
    }
}
