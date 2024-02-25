using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

namespace ToolBox.Utils
{
    /// <summary>
    /// Spawn rigidbody objects on particular height
    /// and in random position in x and z range
    /// and optionally aplies extraforce
    /// </summary>
    public class ObjectsShower : MonoBehaviour
    {
        [SerializeField] private Rigidbody _objPrefab;
        [SerializeField] private float _spawnHeight;
        [SerializeField] private float _extraForce;
        [SerializeField] private float _xRange;
        [SerializeField] private float _zRange;
        [SerializeField] private float _xCenterOffset = 0f;
        [SerializeField] private float _zCenterOffset = 0f;

        public List<Rigidbody> StartShower(int objCount, Rigidbody obj = null)
        {
            Rigidbody objToSpawn = (obj == null) ? _objPrefab : obj;
            List<Rigidbody> spawnedObjs = new List<Rigidbody>();

            for (int i = 0; i < objCount; i++)
            {
                Rigidbody spawnedRB = SpawnObj(objToSpawn);
                AddExtraForce(spawnedRB);
                spawnedObjs.Add(spawnedRB);
            }

            return spawnedObjs;
        }

        private Rigidbody SpawnObj(Rigidbody obj)
        {
            Vector3 spawnPos = transform.position + GetRandomXZPos(_zCenterOffset, _zRange, _xCenterOffset, _xRange, _spawnHeight);
            Quaternion spawnRot = Quaternion.Euler(GetRandomVector(0f, 360f));
            Rigidbody spawnedRb = LeanPool.Spawn(obj, spawnPos, spawnRot);
            return spawnedRb;
        }

        private void AddExtraForce(Rigidbody obj)
        {
            obj.velocity = Vector3.zero;
            obj.isKinematic = false;
            obj.AddForce(Vector3.down * _extraForce, ForceMode.Impulse);
        }

        private Vector3 GetRandomXZPos(float zOffset, float zRange, float xOffset, float xRange, float height)
        {
            float randX;
            float randZ;

            int zOrx = Random.Range(-1, 1);

            if (zOrx == 0)
            {
                randX = GetRandomAxis(xOffset, xRange); ;
                randZ = Random.Range(-(zOffset + zRange), (zOffset + zRange));
            }
            else
            {
                randZ = GetRandomAxis(zOffset, zRange); ;
                randX = Random.Range(-(xOffset + xRange), (xOffset + xRange));
            }

            return new Vector3(randX, height, randZ);
        }

        private Vector3 GetRandomVector(float min, float max)
        {
            float randX = Random.Range(min, max);
            float randY = Random.Range(min, max);
            float randZ = Random.Range(min, max);
            return new Vector3(randX, randY, randZ);
        }

        private float GetRandomAxis(float offset, float range)
        {
            float randAxis;
            int posOrNeg = Random.Range(-1, 1);
            float min, max;

            if (posOrNeg == -1)
            {
                min = -offset - range;
                max = -offset;
                randAxis = -Random.Range(min, max);
            }
            else
            {
                min = offset;
                max = offset + range;
                randAxis = -Random.Range(min, max);
            }

            return randAxis;
        }
    }
}
