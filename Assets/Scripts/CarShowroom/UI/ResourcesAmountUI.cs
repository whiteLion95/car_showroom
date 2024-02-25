using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesAmountUI : MonoBehaviour
{
    [SerializeField] private Image _resIcon;
    [SerializeField] private TMP_Text _amountText;
    private int _reqAmount;

    public ResourceType MyType { get; private set; }

    private void Awake()
    {
        _resIcon = GetComponentInChildren<Image>();
        _amountText = GetComponentInChildren<TMP_Text>();
    }

    public void Init(ResourceData resData, int amount, int reqAmount)
    {
        MyType = resData.resType;
        _resIcon.sprite = resData.resSprite;
        UpdateAmount(amount, reqAmount);
    }

    public void UpdateAmount(int amount)
    {
        if (amount > _reqAmount)
            amount = _reqAmount;

        _amountText.text = amount.ToString() + " / " + _reqAmount.ToString();
    }

    public void UpdateAmount(int amount, int reqAmount)
    {
        _reqAmount = reqAmount;
        UpdateAmount(amount);
    }
}
