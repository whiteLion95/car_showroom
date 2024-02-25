using PajamaNinja.CarShowRoom;
using PajamaNinja.SaveSystem;
using System;
using UnityEngine;

[Serializable]
public class ParkingLotData : SaveDataInSO
{
    public bool hasParkedCar;
    public CarName parkedCarName;
}

[CreateAssetMenu(fileName = "ParkingLotSSO", menuName = "Saveables/ParkingLotSSO")]
public class ParkingLotSSO : SaveableSO<ParkingLotData>
{
    public CarName ParkedCarName
    {
        get
        {
            TryLoad();
            return _saveData.parkedCarName;
        }

        set
        {
            if (_saveData.parkedCarName == value)
                return;

            _saveData.parkedCarName = value;
            TrySave();
        }
    }

    public bool HasParkedCar
    {
        get
        {
            TryLoad();
            return _saveData.hasParkedCar;
        }

        set
        {
            if (_saveData.hasParkedCar == value)
                return;

            _saveData.hasParkedCar = value;
            TrySave();
        }
    }

    public void SetParkedCar(CarName carName)
    {
        HasParkedCar = true;
        ParkedCarName = carName;
    }
}
