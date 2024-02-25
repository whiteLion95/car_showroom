using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "TruckDataSO", menuName = "ScriptableObject/TruckDataSO")]
public class TruckDataSO : ScriptableObject
{
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private Ease _moveEase = Ease.Linear;
    [SerializeField] private float _moveToUnloadDelay = 3f;
    [SerializeField] private float _moveToUnloadDuration = 3f;
    [SerializeField] private float _openDoorsDuration = 0.5f;
    [SerializeField] private Ease _openDoorsEase = Ease.Linear;
    [SerializeField] private float _openDoorsTime = 0.5f;
    [SerializeField] private float _truckDoorsYRot = 90f;
    [SerializeField] private float _truckDoorsOpenDuration = 0.3f;

    public float MoveSpeed => _moveSpeed;
    public Ease MoveEase => _moveEase;
    public float MoveToUnloadDelay => _moveToUnloadDelay;
    public float MoveToUnloadDuration => _moveToUnloadDuration;
    public float OpenDoorsDuration => _openDoorsDuration;
    public Ease OpenDoorsEase => _openDoorsEase;
    public float OpenDoorsTime => _openDoorsTime;
    public float TruckDoorsYRot => _truckDoorsYRot;
    public float TruckDoorsOpenDuration => _truckDoorsOpenDuration;
}
