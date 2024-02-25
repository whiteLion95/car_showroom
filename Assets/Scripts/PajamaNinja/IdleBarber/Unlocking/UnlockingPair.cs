using System;
using PajamaNinja.CurrencySystem;
using PajamaNinja.Scripts.CurrencySystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PajamaNinja.Scripts.IdleBarber.Unlocking
{
    [Serializable]
    public struct UnlockingPair
    {
        [HorizontalGroup, HideLabel] public Unlocker Unlocker;
        [HorizontalGroup, HideLabel] public GameObject Target;
    }
}