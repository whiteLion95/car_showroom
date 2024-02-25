using Lean.Pool;
using System;
using System.Collections;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private ObjectSpawnerDataSO _data;
    [SerializeField] protected GameObject _objPrefab;
    [SerializeField] private Transform _spawnPlace;
    [SerializeField] private Transform _objParent;


    public Action<GameObject> OnSpawn;
    public Action OnRepeteadSpawnFinished;

    private Coroutine _repeatedSpawnRoutine;
    private bool _repeateadSpawnActive;

    #region Spawn
    public GameObject Spawn()
    {
        return Spawn(_objPrefab);
    }

    public GameObject Spawn(GameObject obj, Transform parent = null)
    {
        Vector3 spawnPos = _spawnPlace ? _spawnPlace.position : transform.position;

        if (!parent)
            parent = _objParent;

        return Spawn(obj, spawnPos, Quaternion.identity, parent);
    }

    public GameObject Spawn(GameObject obj, Vector3 pos, Quaternion rot, Transform parent = null)
    {
        GameObject spawnObj = LeanPool.Spawn(obj, pos, rot, parent);
        OnSpawn?.Invoke(spawnObj);
        return spawnObj;
    }
    #endregion

    #region RepeatedSpawn
    public void StartRepeatedSpawn()
    {
        _repeateadSpawnActive = true;
        _repeatedSpawnRoutine = StartCoroutine(RepeatedSpawnRoutine());
    }

    public void StopRepeatedSpawn()
    {
        if (_repeatedSpawnRoutine != null)
        {
            _repeateadSpawnActive = false;
            StopCoroutine(_repeatedSpawnRoutine);
            OnRepeteadSpawnFinished?.Invoke();
        }
    }

    public IEnumerator RepeatedSpawnRoutine()
    {
        while (_repeateadSpawnActive)
        {
            Spawn();
            yield return new WaitForSeconds(_data.RepeatedSpawnInterval);
        }
    }
    #endregion
}
