using PajamaNinja.Common;
using System.Collections.Generic;
using UnityEngine;

namespace PajamaNinja.CarShowRoom
{
    public class TrashManager : SingleReference<TrashManager>
    {
        [SerializeField] private List<Trash> _trashCans;

        public List<Trash> TrashCans => _trashCans;

        public void EnableTrashColliders(bool on)
        {
            foreach (var trash in _trashCans)
            {
                trash.EnableCollider(on);
            }
        }
    }
}