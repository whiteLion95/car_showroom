using PajamaNinja.UpgradesSystem;
using UnityEngine;
using UnityEngine.UI;

namespace PajamaNinja.Scripts.CurrencySystem
{
    public class UnlockerUpgradeableFloat : BaseUnlocker
    {
        [SerializeField] private FloatUSO _variable;
        [SerializeField] private Image _image;
        [SerializeField] private int _unlockingLevel;
        [SerializeField] private Sprite[] _spritesForLevels;

        protected override void BeforeStart()
        {
            _image.sprite = _spritesForLevels[Mathf.Clamp(_unlockingLevel, 0, _spritesForLevels.Length)];
        }

        protected override void ActivateDependency(bool forFirstTime)
        {
            if (_variable.UpgradeLevel + 1 != _unlockingLevel) return;

            if (_variable.TryUpgrade())
            {
                //HoopslyIntegration.RaiseUpgradeEvent(_variable.name, _unlockingLevel, ChangeCondition.buy);
            }
        }
    }
}