using PajamaNinja.CarShowRoom;
using PajamaNinja.CurrencySystem;
using PajamaNinja.UISystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UIScreen))]
public class CarsPickUI : MonoBehaviour
{
    [SerializeField] private CarsDataSO _carsData;
    [SerializeField] private Button _closeButton;
    [SerializeField] private CarPickSlotUI _slotPrefab;
    [SerializeField] private Transform _slotsParent;
    [SerializeField] private CurrencySSO _currency;

    public Action OnSlotsUpdated;

    private UIScreen _screen;
    private ConveyorsManager _conveyorsManager;
    private ConveyorTabsManagerUI _conveyorTabsManagerUI;
    private List<CarPickSlotUI> _slots;
    private int _curTab;

    private void Awake()
    {
        _carsData.InitExtraTip();

        _screen = GetComponent<UIScreen>();
        _closeButton.onClick.AddListener(() => _screen.Hide());
        _conveyorTabsManagerUI = GetComponentInChildren<ConveyorTabsManagerUI>();
        _screen.OnShowStart.AddListener(HandleShowStart);

        _conveyorTabsManagerUI.OnTabActivated += UpdateSlots;
    }

    private void Start()
    {
        _conveyorsManager = ConveyorsManager.Instance;

        Init();
    }

    private void Init()
    {
        InitSlots();
    }

    private void InitSlots()
    {
        _slots = new List<CarPickSlotUI>();

        for (int i = 0; i < _carsData.Cars.Count; i++)
        {
            CarPickSlotUI slot = Instantiate(_slotPrefab, _slotsParent);
            _slots.Add(slot);
            slot.OnCarBought += HandleBuyButtonClicked;
            slot.OnCarEquipped += HandleEquipButtonClicked;
        }
    }

    public CarPickSlotUI GetSlot(CarName name)
    {
        return _slots.Find((s) => s.CarData.name == name);
    }

    private void UpdateSlots(int tabID)
    {
        if (tabID >= 0 && tabID < _conveyorsManager.Conveyors.Count)
        {
            _curTab = tabID;
            bool currencyChecked = false;

            for (int i = 0; i < _carsData.Cars.Count; i++)
            {
                bool isEquipped = false;
                bool isUnlocked = false;

                if (i < _conveyorsManager.Conveyors.Count)
                {
                    isEquipped = _conveyorsManager.Conveyors[tabID].SaveData.CurCarName == _carsData.Cars[i].name;
                    isUnlocked = _conveyorsManager.Conveyors[tabID].SaveData.OpenedCarNames.Contains(_carsData.Cars[i].name);
                }

                _slots[i].Init(_carsData.Cars[i], isEquipped, isUnlocked);

                if (!isUnlocked && !currencyChecked && i < _conveyorsManager.Conveyors.Count)
                {
                    if (_carsData.Cars[i].unlockPrice <= _currency.CurrencyCount)
                        _slots[i].ActivateForPurchase();

                    currencyChecked = true;
                }
            }

            OnSlotsUpdated?.Invoke();
        }
    }

    private void HandleShowStart()
    {
        _conveyorTabsManagerUI.InitTabs(_conveyorsManager.Conveyors);
        _conveyorTabsManagerUI.ActivateTab(0);
    }

    private void HandleEquipButtonClicked(CarData carData)
    {
        _slots.Find((s) => s.CarData.name == _conveyorsManager.Conveyors[_curTab].SaveData.CurCarName).UnEquip();
        _conveyorsManager.Conveyors[_curTab].EquipCar(carData.name);
    }

    private void HandleBuyButtonClicked(CarData carData)
    {
        _conveyorsManager.Conveyors[_curTab].SaveData.AddOpenedCarName(carData.name);
        _currency.ChangeMoney(-carData.unlockPrice);
        CarPickSlotUI firstNotUnlocked = _slots.First((s) => !s.IsUnlocked);
        int index = _slots.IndexOf(firstNotUnlocked);

        if (index < _conveyorsManager.Conveyors.Count && _carsData.Cars[index].unlockPrice <= _currency.CurrencyCount)
            firstNotUnlocked.ActivateForPurchase();
    }
}
