using PajamaNinja.Common;
using System.Collections.Generic;
using UnityEngine;

namespace PajamaNinja.CarShowRoom
{
    public class ParkingLotsManager : SingleReference<ParkingLotsManager>
    {
        [SerializeField] private List<ParkingLot> _parkLots;

        public List<ParkingLot> ParkingLots => _parkLots;

        public List<ParkingLot> GetAvailableLots()
        {
            List<ParkingLot> unlockedLots = ParkingLots.FindAll((l) => l.IsUnlocked);
            return unlockedLots.FindAll((l) => l.ParkedCar == null);
        }

        public void DisableAllAvailableLotsCollidersExcept(int index)
        {
            List<ParkingLot> availableLots = GetAvailableLots();

            for (int i = 0; i < availableLots.Count; i++)
            {
                availableLots[i].EnableCollider(i == index);
            }
        }

        public void EnableAllAvailableLotsColliders(bool on)
        {
            foreach (var lot in GetAvailableLots())
            {
                lot.EnableCollider(on);
            }
        }
    }
}
