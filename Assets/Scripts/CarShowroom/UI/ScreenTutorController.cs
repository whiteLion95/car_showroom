using PajamaNinja.Common;
using System;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace PajamaNinja.CarShowRoom
{
    public class ScreenTutorController : SingleReference<ScreenTutorController>
    {
        public enum ScreenTutorStage
        {
            None,
            BuyCar,
            EquipCar,
            Close,
            OpenConveyorTab
        }

        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _conveyorTabButton;
        [SerializeField] private GameObject _pointObj;
        [SerializeField] private CanvasGroup _canvasGroup;

        public Action OnPointedButtonClicked;
        public Action OnButtonsAssigned;

        private Button _buyCarButton;
        private Button _equipCarButton;
        private ScreenTutor _screenTutor;
        private CarsPickUI _carsPickUI;

        protected override void Awake()
        {
            base.Awake();
            _screenTutor = new ScreenTutor(_canvasGroup, _pointObj);
            _carsPickUI = GetComponentInChildren<CarsPickUI>(true);

            _screenTutor.OnPointedButtonClicked += () => OnPointedButtonClicked?.Invoke();
            _carsPickUI.OnSlotsUpdated += HandleCarsPickUIUpdated;
        }

        public void SetStage(ScreenTutorStage stage)
        {
            Button pointedButton = null;

            switch (stage)
            {
                case ScreenTutorStage.BuyCar:
                    pointedButton = _buyCarButton;
                    break;
                case ScreenTutorStage.EquipCar:
                    pointedButton = _equipCarButton;
                    break;
                case ScreenTutorStage.Close:
                    pointedButton = _closeButton;
                    break;
                case ScreenTutorStage.OpenConveyorTab:
                    pointedButton = _conveyorTabButton;
                    break;
            }

            if (pointedButton != null)
                _screenTutor.PointToButton(pointedButton);
        }

        private void HandleCarsPickUIUpdated()
        {
            CarPickSlotUI slot = _carsPickUI.GetSlot(CarName.Car2);
            _buyCarButton = slot.BuyButton;
            _equipCarButton = slot.EquipButton;
            _screenTutor.SetButtons();

            OnButtonsAssigned?.Invoke();
        }
    }
}