using System;
using System.Collections.Generic;
using UnityEngine;

namespace PajamaNinja.CarShowRoom
{
    [CreateAssetMenu(fileName = "CarsDataSO", menuName = "ScriptableObject/CarsDataSO")]
    public class CarsDataSO : ScriptableObject
    {
        [SerializeField] private List<CarData> _cars;

        public CarData this[CarName name]
        {
            get
            {
                return _cars.Find((c) => c.name == name);
            }
        }

        public void InitExtraTip()
        {
            _cars[0].extraTip = 0;

            for (int i = 1; i < _cars.Count; i++)
            {
                _cars[i].extraTip = _cars[i].incomeFromSale - _cars[0].incomeFromSale;
            }
        }

        public List<CarData> Cars => _cars;
    }

    [Serializable]
    public class CarData
    {
        public CarName name;
        public Car prefab;
        public List<CarRequiredResources> requiredResources;
        public int unlockPrice;
        public int incomeFromSale;
        public Sprite sprite;
        public int extraTip;
    }

    public enum CarName
    {
        Car1,
        Car2,
        Car3,
        Car4,
        Car5,
        Car6
    }

    [Serializable]
    public class CarRequiredResources
    {
        public ResourceType type;
        public int amount;
    }
}