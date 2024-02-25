using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MoneyAnimationUI : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private int _countImages;
    [SerializeField] private int _spreadX;
    [SerializeField] private int _spreadY;
    [SerializeField] private Image _target;
    [SerializeField] private Ease _ease;
    [SerializeField] private float _duration = 1.25f;
    [SerializeField] private float _delaySpawningImages = 0.02f;
    [SerializeField] private float targetImageScale = 0.2f;
    [SerializeField] private float targetImageScaleDuration = 0.1f;
    
    private List<Image> _moneyImages = new List<Image>();
    private Tween targetImageTween;

    public UnityEvent OnFirstMoneyOnIcon;

    private IEnumerator StartMoneyAnimationCoroutine(float delaySpawning)
    {
        for (int i = 0; i < _countImages; i++)
        {
            float randomPosX = Random.Range(-_spreadX, _spreadX);
            float randomPosY = Random.Range(-_spreadY, _spreadY);
            Image money = Instantiate(_image, transform.position,Quaternion.Euler(0,0,0), transform);
            money.rectTransform.anchoredPosition3D = new Vector3(randomPosX, randomPosY, 0);
            money.transform.localEulerAngles = Vector3.zero;
            _moneyImages.Add(money);
            yield return new WaitForSeconds(delaySpawning);
        }

        for (int i = 0; i < _moneyImages.Count; i++)
        {
            Image imageMoney = _moneyImages[i];
            _moneyImages[i].transform.DOMove(_target.transform.position, _duration).SetEase(_ease).OnComplete(() =>
            {
                if (targetImageTween != null)
                    targetImageTween.Restart();
                else
                {
                    targetImageTween = _target.transform.DOScale(targetImageScale, targetImageScaleDuration).SetEase(_ease).SetRelative(true).SetLoops(2, LoopType.Yoyo);
                    OnFirstMoneyOnIcon?.Invoke();
                }

                imageMoney.transform.DOScale(Vector3.zero, 0.2f).OnComplete(() =>
                {
                    Destroy(imageMoney.gameObject);
                    _moneyImages.Remove(imageMoney);
                });
            });

            if (i == _moneyImages.Count - 1)
                targetImageTween = null;
        }
    }

    public void StartMoneyAnimation() 
        => StartCoroutine(StartMoneyAnimationCoroutine(_delaySpawningImages));
}