using PajamaNinja.Common;
using System.Collections.Generic;
using UnityEngine;

namespace PajamaNinja.CarShowRoom
{
    public class ConveyorsManager : SingleReference<ConveyorsManager>
    {
        [SerializeField] private List<Conveyor> _conveyors;

        public List<Conveyor> Conveyors => _conveyors;
    }
}