using PajamaNinja.CurrencySystem;
using PajamaNinja.Scripts.IdleBarber.General.Tutor.Progress;
using UnityEngine;

namespace PajamaNinja.CarShowRoom
{
    public class TutorLauncher : MonoBehaviour
    {
        [SerializeField] private TutorProgressSSO _progressSso;
        [SerializeField] private CurrencySSO _currency;
        [SerializeField] private CarsDataSO _carsData;

        private TutorController _tutorController;

        public void Start()
        {
            _tutorController = new TutorController(_progressSso, _currency, _carsData);
        }
    }
}