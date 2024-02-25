using PajamaNinja.SaveSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PajamaNinja.CarShowRoom
{
    [Serializable]
    public class ConveyorData : SaveDataInSO
    {
        public CarName curCarName;
        public bool hasAvailableCar;
        public CarName availableCarName;
        public List<CarName> openedCarNames;
    }

    [CreateAssetMenu(fileName = "ConveyorSSO", menuName = "Saveables/ConveyorSSO")]
    public class ConveyorSSO : SaveableSO<ConveyorData>
    {
        #region Properties
        public CarName CurCarName
        {
            get
            {
                TryLoad();
                return _saveData.curCarName;
            }

            set
            {
                if (_saveData.curCarName == value)
                    return;

                _saveData.curCarName = value;
                TrySave();
            }
        }
        public bool HasAvailableCar
        {
            get
            {
                TryLoad();
                return _saveData.hasAvailableCar;
            }

            set
            {
                if (_saveData.hasAvailableCar == value)
                    return;

                _saveData.hasAvailableCar = value;
                TrySave();
            }
        }
        public CarName AvailableCarName
        {
            get
            {
                TryLoad();
                return _saveData.availableCarName;
            }

            set
            {
                if (_saveData.availableCarName == value)
                    return;

                _saveData.availableCarName = value;
                TrySave();
            }
        }
        public List<CarName> OpenedCarNames
        {
            get
            {
                TryLoad();
                _saveData.openedCarNames ??= new List<CarName>();
                return _saveData.openedCarNames;
            }
        }
        #endregion

        public void AddOpenedCarName(CarName carName)
        {
            if (!OpenedCarNames.Contains(carName))
            {
                _saveData.openedCarNames.Add(carName);
                TrySave();
            }
        }

        public void SetAvailableCar(CarName carName)
        {
            HasAvailableCar = true;
            AvailableCarName = carName;
        }
    }
}
