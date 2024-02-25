using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "Wordl_PopUp_Data", menuName = "ScriptableObjects/Utils/Wordl_PopUp_Data")]
public class World_PopUp_Data : ScriptableObject
{
    [SerializeField] private float _YRange;
    [SerializeField] private float _tweenDuration;
    [SerializeField] private float _showDuration;
    [SerializeField] private Ease _scaleEase;

    public float CanvasYRange { get => _YRange; }
    public float CanvasTweenDuration { get => _tweenDuration; }
    public float CanvasShowDuration { get => _showDuration; }
    public Ease CanvasScaleEase { get => _scaleEase; }
}
