using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PajamaNinja.CarShowRoom
{
    public class Map : MonoBehaviour
    {
        [SerializeField] private Vector3 _inGameRotation;

        private void Awake()
        {
            transform.rotation = Quaternion.Euler(_inGameRotation);
        }
    }
}