using PajamaNinja.CurrencySystem;
using PajamaNinja.Scripts.IdleBarber.General.Tutor.Progress;
using System;
using UnityEngine;

namespace PajamaNinja.CarShowRoom
{
    public class TutorContainer
    {
        private TutorSequence[] _tutors;
        private TutorController _tutorController;
        private ITutorProgress _tutorProgress;
        private UnloadingManager _unloadingManager;
        private TrashManager _trashManager;
        private ConveyorsManager _convManager;
        private Player _player;
        private Car _curCar;
        private ParkingLotsManager _parkLotsManager;
        private ScreenTutorController _screenTutor;
        private CurrencySSO _currency;
        private CarsDataSO _carsData;

        public TutorContainer(TutorController controller, ITutorProgress tutorProgress, CurrencySSO currency, CarsDataSO carsData)
        {
            _tutorController = controller;
            _tutorProgress = tutorProgress;
            _currency = currency;
            _unloadingManager = UnloadingManager.Instance;
            _trashManager = TrashManager.Instance;
            _convManager = ConveyorsManager.Instance;
            _player = Player.Instance;
            _parkLotsManager = ParkingLotsManager.Instance;
            _screenTutor = ScreenTutorController.Instance;
            _carsData = carsData;

            InstantiateTutor();
        }

        public TutorSequence[] GetTutors() => _tutors;

        private void InstantiateTutor()
        {
            _tutors = new[]
            {
                new TutorSequence(TutorSequenceId.WheelParts, _tutorProgress, new [] {
                    new TutorStep(TutorStepId.WheelParts_Take)
                        .SetTriggerHandler(args => {
                            PartTriggerHandler(ResourceType.Wheel);
                        })
                        .SetCompleteListener(args => {
                            PartCompleteListener(args);
                        }),
                    new TutorStep(TutorStepId.WheelParts_Unload)
                        .SetTriggerHandler(args => {
                            PartUnloadTriggerHandler();
                        })
                        .SetCompleteListener(args => {
                            PartUnloadCompleteListener(ResourceType.Wheel, args);
                        })
                }),
                new TutorSequence(TutorSequenceId.GearParts, _tutorProgress, new [] {
                    new TutorStep(TutorStepId.GearParts_Take)
                        .SetTriggerListener(args => {
                            AwaitSequenceCompleted(TutorSequenceId.WheelParts, () => args.TriggerHandler.Invoke(args));
                        })
                        .SetTriggerHandler(args => {
                            PartTriggerHandler(ResourceType.Gear);
                        })
                        .SetCompleteListener(args => {
                            PartCompleteListener(args);
                        }),
                    new TutorStep(TutorStepId.GearParts_Unload)
                        .SetTriggerHandler(args => {
                            PartUnloadTriggerHandler();
                        })
                        .SetCompleteListener(args => {
                            PartUnloadCompleteListener(ResourceType.Gear, args);
                        })
                }),
                new TutorSequence(TutorSequenceId.SheetParts, _tutorProgress, new [] {
                    new TutorStep(TutorStepId.SheetParts_Take)
                        .SetTriggerListener(args => {
                            AwaitSequenceCompleted(TutorSequenceId.GearParts, () => args.TriggerHandler.Invoke(args));
                        })
                        .SetTriggerHandler(args => {
                            PartTriggerHandler(ResourceType.MetalSheet);
                        })
                        .SetCompleteListener(args => {
                            PartCompleteListener(args);
                        }),
                    new TutorStep(TutorStepId.SheetParts_Unload)
                        .SetTriggerHandler(args => {
                            PartUnloadTriggerHandler();
                        })
                        .SetCompleteListener(args => {
                            PartUnloadCompleteListener(ResourceType.MetalSheet, args);
                        })
                        .SetCompleteHandler(args =>
                        {
                            _player.Stack.ResetCapacity();
                            _trashManager.EnableTrashColliders(true);
                            _unloadingManager.EnableAllAreas(true);
                        })
                }),
                new TutorSequence(TutorSequenceId.ParkCar, _tutorProgress, new [] {
                    new TutorStep(TutorStepId.ParkCar_GoToCar)
                        .SetTriggerListener(args => {
                            AwaitSequenceCompleted(TutorSequenceId.SheetParts, () => args.TriggerHandler.Invoke(args));
                        })
                        .SetTriggerHandler(args => {
                            _player.Compass.Toggle(true);
                            _player.Compass.UpdateTarget(_convManager.Conveyors[0].CarReadyPlace);
                        })
                        .SetCompleteListener(args => {
                            _player.Rider.OnInTheCar += HandlePlayerInTheCar;

                            void HandlePlayerInTheCar(Car car)
                            {
                                _player.Rider.OnInTheCar -= HandlePlayerInTheCar;
                                _curCar = car;
                                _player.Compass.Toggle(false);
                                args.Step.SetStepAsCompleted();
                            }
                        }),
                    new TutorStep(TutorStepId.ParkCar_Park)
                        .SetTriggerHandler(args => {
                            if (_curCar != null)
                            {
                                _curCar.Compass.Toggle(true);
                                _curCar.Compass.UpdateTarget(_parkLotsManager.ParkingLots[0].transform);
                                _parkLotsManager.DisableAllAvailableLotsCollidersExcept(0);
                            }
                        })
                        .SetCompleteListener(args => {
                            _curCar.OnParked += HandleCarParked;

                            void HandleCarParked(ParkingLot parkingLot)
                            {
                                _curCar.OnParked -= HandleCarParked;
                                _parkLotsManager.EnableAllAvailableLotsColliders(true);
                                _curCar.Compass.Toggle(false);
                                _curCar = null;
                                args.Step.SetStepAsCompleted();
                            }
                        })
                }),
                new TutorSequence(TutorSequenceId.SellCar, _tutorProgress, new [] {
                    new TutorStep(TutorStepId.SellCar)
                        .SetTriggerListener(args => {
                            AwaitSequenceCompleted(TutorSequenceId.ParkCar, () => args.TriggerHandler.Invoke(args));
                        })
                        .SetTriggerHandler(args => {
                            _parkLotsManager.ParkingLots[0].OnVisitorSet += HandleVisitorSet;

                            void HandleVisitorSet(Visitor visitor)
                            {
                                _parkLotsManager.ParkingLots[0].OnVisitorSet -= HandleVisitorSet;
                                _player.Compass.Toggle();
                                _player.Compass.UpdateTarget(_parkLotsManager.ParkingLots[0].SellPlace.transform);
                            }
                        })
                        .SetCompleteListener(args => {
                            _parkLotsManager.ParkingLots[0].OnCarSold += HandleCarSold;

                            void HandleCarSold(Car car)
                            {
                                _parkLotsManager.ParkingLots[0].OnCarSold -= HandleCarSold;
                                args.Step.SetStepAsCompleted();
                            }
                        })
                }),
                new TutorSequence(TutorSequenceId.GetCarMoney, _tutorProgress, new [] {
                    new TutorStep(TutorStepId.GetCarMoney)
                        .SetTriggerListener(args => {
                            AwaitSequenceCompleted(TutorSequenceId.SellCar, () => args.TriggerHandler.Invoke(args));
                        })
                        .SetTriggerHandler(args => {
                            _player.Compass.Toggle(true);
                            _player.Compass.UpdateTarget(_parkLotsManager.ParkingLots[0].CurencyGiver.CurrencyStack.transform);
                        })
                        .SetCompleteListener(args => {
                            _parkLotsManager.ParkingLots[0].CurencyGiver.CurrencyStack.PickedByPlayer += HandleCurrencyPicked;

                            void HandleCurrencyPicked()
                            {
                                _parkLotsManager.ParkingLots[0].CurencyGiver.CurrencyStack.PickedByPlayer -= HandleCurrencyPicked;

                                _player.Compass.Toggle(false);
                                args.Step.SetStepAsCompleted();
                            }
                        })
                }),
                new TutorSequence(TutorSequenceId.BuyCarDesign, _tutorProgress, new [] {
                    new TutorStep(TutorStepId.BuyCarDesign_AdminDesk)
                        .SetTriggerListener(args => {
                            AwaitSequenceCompleted(TutorSequenceId.GetCarMoney, () => args.TriggerHandler.Invoke(args));
                        })
                        .SetTriggerHandler(args => {
                            if (_currency.CurrencyCount >= _carsData.Cars[1].unlockPrice)
                            {
                                _player.Compass.Toggle(true);
                                _player.Compass.UpdateTarget(AdminDesk.Instance.Trigger);
                                _player.CanBuyUnlocks = false;

                                _screenTutor.OnButtonsAssigned += HandleTutorButtonsAssigned;

                                void HandleTutorButtonsAssigned()
                                {
                                    _screenTutor.OnButtonsAssigned -= HandleTutorButtonsAssigned;
                                    args.Step.SetStepAsCompleted();
                                }
                            }
                            else
                            {
                                _currency.OnCurrencyValueChange.AddListener(HandleCurrencyChanged);

                                void HandleCurrencyChanged(int oldValue, int newValue)
                                {
                                    if (newValue >= _carsData.Cars[1].unlockPrice)
                                    {
                                        _currency.OnCurrencyValueChange.RemoveListener(HandleCurrencyChanged);
                                        _player.Compass.Toggle(true);
                                        _player.Compass.UpdateTarget(AdminDesk.Instance.Trigger);
                                        _player.CanBuyUnlocks = false;

                                        _screenTutor.OnButtonsAssigned += HandleTutorButtonsAssigned;

                                        void HandleTutorButtonsAssigned()
                                        {
                                            _screenTutor.OnButtonsAssigned -= HandleTutorButtonsAssigned;
                                            args.Step.SetStepAsCompleted();
                                        }
                                    }
                                }
                            }
                        }),
                    new TutorStep(TutorStepId.BuyCarDesign_Buy)
                        .SetTriggerHandler(args => {
                            _screenTutor.SetStage(ScreenTutorController.ScreenTutorStage.BuyCar);
                        })
                        .SetCompleteListener(args => {
                            ButtonTutorCompleteListener(args);
                        })
                        .SetCompleteHandler(args =>
                        {
                            _player.CanBuyUnlocks = true;
                        })
                }),
                new TutorSequence(TutorSequenceId.EquipCarDesign, _tutorProgress, new [] {
                    new TutorStep(TutorStepId.EquipCarDesing_AdminDesk)
                        .SetTriggerListener(args => {
                            AwaitSequenceCompleted(TutorSequenceId.BuyCarDesign, () => args.TriggerHandler.Invoke(args));
                        })
                        .SetTriggerHandler(args => {
                            if (!AdminDesk.Instance.PlayerIsInteracting)
                            {
                                _player.Compass.Toggle(true);
                                _player.Compass.UpdateTarget(AdminDesk.Instance.Trigger);
                                _screenTutor.OnButtonsAssigned += HandleTutorButtonsAssigned;

                                void HandleTutorButtonsAssigned()
                                {
                                    _screenTutor.OnButtonsAssigned -= HandleTutorButtonsAssigned;
                                    args.Step.SetStepAsCompleted();
                                }
                            }
                            else
                                args.Step.SetStepAsCompleted();
                        }),
                    new TutorStep(TutorStepId.EquipCarDesing_Equip)
                        .SetTriggerHandler(args => {
                            _screenTutor.SetStage(ScreenTutorController.ScreenTutorStage.EquipCar);
                        })
                        .SetCompleteListener(args => {
                            ButtonTutorCompleteListener(args);
                        }),
                    new TutorStep(TutorStepId.EquipCarDesing_Close)
                        .SetTriggerHandler(args => {
                            _screenTutor.SetStage(ScreenTutorController.ScreenTutorStage.Close);
                        })
                        .SetCompleteListener(args => {
                            ButtonTutorCompleteListener(args);
                        })
                        .SetCompleteHandler(args =>
                        {
                            _player.Compass.Toggle(false);
                        })
                }),
                new TutorSequence(TutorSequenceId.OpenConveyor2Tab, _tutorProgress, new [] {
                    new TutorStep(TutorStepId.OpenConveyor2Tab_AdminDesk)
                        .SetTriggerListener(args => {
                            AwaitSequenceCompleted(TutorSequenceId.GetCarMoney, () => args.TriggerHandler.Invoke(args));
                        })
                        .SetTriggerHandler(args => {
                            if (_convManager.Conveyors[1].IsUnlocked)
                            {
                                _player.Compass.Toggle(true);
                                _player.Compass.UpdateTarget(AdminDesk.Instance.Trigger);
                                _screenTutor.OnButtonsAssigned += HandleTutorButtonsAssigned;

                                void HandleTutorButtonsAssigned()
                                {
                                    _screenTutor.OnButtonsAssigned -= HandleTutorButtonsAssigned;
                                    args.Step.SetStepAsCompleted();
                                }
                            }
                            else
                            {
                                _convManager.Conveyors[1].UnlockingObject.OnUnlockedForFirstTime += HandleUnlocked;

                                void HandleUnlocked()
                                {
                                    _convManager.Conveyors[1].UnlockingObject.OnUnlockedForFirstTime -= HandleUnlocked;
                                    _player.Compass.Toggle(true);
                                    _player.Compass.UpdateTarget(AdminDesk.Instance.Trigger);

                                    _screenTutor.OnButtonsAssigned += HandleTutorButtonsAssigned;

                                    void HandleTutorButtonsAssigned()
                                    {
                                        _screenTutor.OnButtonsAssigned -= HandleTutorButtonsAssigned;
                                        args.Step.SetStepAsCompleted();
                                    }
                                }
                            }
                        }),
                    new TutorStep(TutorStepId.OpenConveyor2Tab_Open)
                        .SetTriggerHandler(args => {
                            _screenTutor.SetStage(ScreenTutorController.ScreenTutorStage.OpenConveyorTab);
                        })
                        .SetCompleteListener(args => {
                            ButtonTutorCompleteListener(args);
                        })
                        .SetCompleteHandler(args =>
                        {
                            _player.Compass.Toggle(false);
                        })
                })
            };
        }

        private void AwaitSequenceCompleted(TutorSequenceId sequenceId, Action callback)
        {
            TutorSequence sequence = _tutorController.GetSequence(sequenceId);
            if (sequence.IsSequenceCompleted)
            {
                callback?.Invoke();
                return;
            }

            sequence.SequenceCompleted += OnSequenceCompleted;

            void OnSequenceCompleted(TutorSequenceId id)
            {
                sequence.SequenceCompleted -= OnSequenceCompleted;
                callback?.Invoke();
            }
        }

        private void PartTriggerHandler(ResourceType resType)
        {
            _unloadingManager.DisableAllAreasExcept(resType);
            UnloadArea unloadArea = _unloadingManager.GetUnloadArea(resType);
            _player.Compass.Toggle();
            _player.Compass.UpdateTarget(unloadArea.transform);
            _trashManager.EnableTrashColliders(false);
            _convManager.Conveyors[0].Collider.enabled = false;
        }

        private void PartCompleteListener(TutorStep.DelegateArgs args)
        {
            _player.Stack.SetCapacity(4);
            _player.Stack.OnFull += HandlePlayerStackFull;

            void HandlePlayerStackFull()
            {
                _player.Stack.OnFull -= HandlePlayerStackFull;
                args.Step.SetStepAsCompleted();
            }
        }

        private void PartUnloadTriggerHandler()
        {
            Transform convResTrigger = _convManager.Conveyors[0].ResourcesTrigger;
            _player.Compass.UpdateTarget(convResTrigger);
            _convManager.Conveyors[0].Collider.enabled = true;
        }

        private void PartUnloadCompleteListener(ResourceType resourceType, TutorStep.DelegateArgs args)
        {
            _convManager.Conveyors[0].ResourceBox.OnResourceFull += HandlerResourceFull;

            void HandlerResourceFull(ResourceType resType)
            {
                if (resType == resourceType)
                {
                    _convManager.Conveyors[0].ResourceBox.OnResourceFull -= HandlerResourceFull;
                    args.Step.SetStepAsCompleted();
                }
            }
        }

        private void ButtonTutorCompleteListener(TutorStep.DelegateArgs args)
        {
            _screenTutor.OnPointedButtonClicked += HandlePointedButtonClicked;

            void HandlePointedButtonClicked()
            {
                _screenTutor.OnPointedButtonClicked -= HandlePointedButtonClicked;
                args.Step.SetStepAsCompleted();
            }
        }
    }
}