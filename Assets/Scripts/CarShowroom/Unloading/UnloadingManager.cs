using PajamaNinja.Common;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PajamaNinja.CarShowRoom
{
    public class UnloadingManager : SingleReference<UnloadingManager>
    {
        [SerializeField] private List<UnloadArea> _unloadAreas;

        public List<UnloadArea> UnloadAreas => _unloadAreas;

        public void DisableAllAreasExcept(ResourceType resourceType)
        {
            foreach (var area in _unloadAreas)
            {
                area.EnableCollider(area.ResType == resourceType);
            }
        }

        public void EnableAllAreas(bool on)
        {
            foreach (var area in _unloadAreas)
            {
                area.EnableCollider(on);
            }
        }

        public UnloadArea GetUnloadArea(ResourceType resourceType)
        {
            return _unloadAreas.First((a) => a.ResType == resourceType);
        }
    }
}