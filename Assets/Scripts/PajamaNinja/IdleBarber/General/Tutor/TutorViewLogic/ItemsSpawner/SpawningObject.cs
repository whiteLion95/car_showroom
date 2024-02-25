using System;
using System.Linq;
using UnityEngine;

namespace PajamaNinja.Scripts.IdleBarber.General.Tutor.TutorViewLogic.ItemsSpawner
{
    public class SpawningObject : MonoBehaviour
    {
        [Serializable]
        private struct Item
        {
            public string Key;
            public MonoBehaviour GameObject;
        }
        
        [SerializeField] private Item[] _internalObjects;

        public MonoBehaviour GetInternalObject(string key)
        {
            return _internalObjects.First(i => i.Key.Equals(key)).GameObject;
        }

        public MonoBehaviour[] GetAllInternalObjects() => _internalObjects.Select(i => i.GameObject).ToArray();
    }
}