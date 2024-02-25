using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "CarConstructionDataSO", menuName = "ScriptableObject/CarConstructionDataSO")]
public class CarConstructionDataSO : ScriptableObject
{
    [SerializeField] private float _intervalBetweenParts = 0.5f;
    [SerializeField] private float _playStageDelay = 0.3f;
    [SerializeField] private float _partsAppearDuration = 0.2f;
    [SerializeField] private Ease _partsApearEase = Ease.OutBack;
    [SerializeField] private float _partTweenDuration = 0.3f;
    [SerializeField] private Ease _partsTweenEase = Ease.OutBack;
    [SerializeField] private float _onPlaceScaleValue = 0.2f;
    [SerializeField] private Ease _onPlaceScaleEase = Ease.OutBounce;

    public float IntervalBetweenParts { get => _intervalBetweenParts; }
    public float PlayStageDelay { get => _playStageDelay; }
    public float PartsAppearDuration { get => _partsAppearDuration; }
    public Ease PartsApearEase { get => _partsApearEase; }
    public float PartTweenDuration { get => _partTweenDuration; }
    public Ease PartsTweenEase { get => _partsTweenEase; }
    public float OnPlaceScaleValue { get => _onPlaceScaleValue; }
    public Ease OnPlaceScaleEase { get => _onPlaceScaleEase; }
}
