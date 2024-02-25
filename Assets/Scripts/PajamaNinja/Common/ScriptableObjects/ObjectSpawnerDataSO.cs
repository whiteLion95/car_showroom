using UnityEngine;

[CreateAssetMenu(fileName = "ObjectSpawnerDataSO", menuName = "ScriptableObject/ObjectSpawnerDataSO")]
public class ObjectSpawnerDataSO : ScriptableObject
{
    [SerializeField] private float _repeatedSpawnInterval = 1f;

    public float RepeatedSpawnInterval { get => _repeatedSpawnInterval; }
}
