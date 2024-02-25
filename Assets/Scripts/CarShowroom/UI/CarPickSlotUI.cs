using PajamaNinja.CarShowRoom;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarPickSlotUI : MonoBehaviour
{
    [SerializeField] private Image _carIcon;
    [SerializeField] private Image _equippedBackground;
    [SerializeField] private Button _equipButton;
    [SerializeField] private Button _buyButton;
    [SerializeField] private Image _equippedMark;
    [SerializeField] private TMP_Text _price;
    [SerializeField] private TMP_Text _extraTipValue;
    [SerializeField] private TMP_Text _extraTipLabel;
    [SerializeField] private TMP_Text _carName;

    public Action<CarData> OnCarBought;
    public Action<CarData> OnCarEquipped;

    private CarData _carData;
    private bool _isEquipped;
    private bool _isUnlocked;

    public bool IsEquipped => _isEquipped;
    public bool IsUnlocked => _isUnlocked;
    public CarData CarData => _carData;
    public Button EquipButton => _equipButton;
    public Button BuyButton => _buyButton;

    private void Awake()
    {
        _buyButton.onClick.AddListener(Buy);
        _equipButton.onClick.AddListener(Equip);
    }

    public void Init(CarData data, bool isEquipped, bool isUnlocked)
    {
        if (!isUnlocked)
            isEquipped = false;

        _carData = data;
        _isEquipped = isEquipped;
        _isUnlocked = isUnlocked;

        _carIcon.sprite = data.sprite;
        _price.text = data.unlockPrice.ToString();
        _extraTipValue.text = data.extraTip.ToString() + "$";
        _carName.text = data.name.ToString();

        _equippedBackground.gameObject.SetActive(isEquipped);
        _equippedMark.gameObject.SetActive(isEquipped);
        _equipButton.interactable = !isEquipped;
        _equipButton.gameObject.SetActive(IsUnlocked);
        _buyButton.gameObject.SetActive(!isUnlocked);
        _buyButton.interactable = false;

        if (data.extraTip == 0)
        {
            _extraTipLabel.gameObject.SetActive(false);
            _extraTipValue.gameObject.SetActive(false);
        }
    }

    public void ActivateForPurchase()
    {
        _buyButton.interactable = true;
    }

    private void Buy()
    {
        _buyButton.gameObject.SetActive(false);
        _equipButton.gameObject.SetActive(true);
        _isUnlocked = true;
        OnCarBought?.Invoke(_carData);
    }

    private void Equip()
    {
        _isEquipped = true;
        _equippedMark.gameObject.SetActive(true);
        _equippedBackground.gameObject.SetActive(true);
        _equipButton.interactable = false;
        OnCarEquipped?.Invoke(_carData);
    }

    public void UnEquip()
    {
        _isEquipped = false;
        _equippedMark.gameObject.SetActive(false);
        _equippedBackground.gameObject.SetActive(false);
        _equipButton.interactable = true;
    }
}
