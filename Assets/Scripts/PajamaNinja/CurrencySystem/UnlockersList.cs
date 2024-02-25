using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PajamaNinja.Scripts.CurrencySystem
{
    [CreateAssetMenu(fileName = "UnlockerList", menuName = "IdleBarber/UnlockerList", order = 3)]
    public class UnlockersList : ScriptableObject
    {
        public List<UnlockerInfoConfig> Unlockers => _unlockersInfo;
        [SerializeField] private List<UnlockerInfoConfig> _unlockersInfo;
        
        
        private void OnValidate()
        {
            _unlockersInfo = _unlockersInfo.OrderBy(info => info.UnlockingOrder).ToList();
        }
    }
}