using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PajamaNinja.Scripts.IdleBarber.General.Tutor.TutorViewLogic.ItemsSpawner
{
    public class TutorItemsSpawner : MonoBehaviour
    {
        public enum ItemsId
        {
            None = 0,
            ControlsFinger = 1,
            StartTrashBarrels = 2,
        }

        public enum PositionId
        {
            None = 0,
            HudBeforeBlackout = 1,
            HudAfterBlackout = 2,
            
            StartTrashBarrels = 3,
            FirstTruckSpawn = 4,
            FirstTruckBotSpawn = 5,
        }
        
        [Serializable] private struct Item
        {
            public ItemsId Id;
            public SpawningObject Object;
        }

        private struct SpawnedItemData
        {
            public ItemsId Id;
            public PositionId PositionId;
            public SpawningObject Object;
            public string RequesterId;
        }

        [SerializeField] private Item[] _items;
        [SerializeField] private TutorSpawnPosition[] _positions;

        private Dictionary<ItemsId, List<SpawnedItemData>> _currentlySpawnedItems = new Dictionary<ItemsId, List<SpawnedItemData>>();

        public SpawningObject SpawnItem(ItemsId itemId, string requesterId, PositionId positionId)
        {
            Item item = _items.First(i => i.Id == itemId);
            TutorSpawnPosition parent = _positions.First(p => p.Id == positionId);
            var newItem = Instantiate(item.Object, parent.Transform);
            SpawnedItemData itemData = new SpawnedItemData()
                {Id = itemId, PositionId = positionId, Object = newItem, RequesterId = requesterId};
            if(_currentlySpawnedItems.ContainsKey(itemId))
                _currentlySpawnedItems[itemId].Add(itemData);
            else
                _currentlySpawnedItems.Add(itemId, new List<SpawnedItemData>(){itemData});
            return newItem;
        }

        public SpawningObject GetItem(ItemsId itemId, string requesterId, PositionId positionId)
        {
            if (_currentlySpawnedItems.ContainsKey(itemId) == false) throw new ArgumentException();
            foreach (var item in _currentlySpawnedItems[itemId])
            {
                if (item.RequesterId == requesterId && item.PositionId == positionId)
                    return item.Object;
            }

            throw new ArgumentException();
        }

        public void DestroyItem(ItemsId id, string requesterId, PositionId positionId)
        {
            if (_currentlySpawnedItems.ContainsKey(id) == false) return;
            List<SpawnedItemData> removingList = new List<SpawnedItemData>();
            foreach (var item in _currentlySpawnedItems[id])
            {
                if(item.RequesterId != requesterId && item.PositionId != positionId) continue;
                Destroy(item.Object.gameObject);
                removingList.Add(item);
            }

            foreach (var item in removingList)
            {
                _currentlySpawnedItems[id].Remove(item);
            }
        }

        public Transform GetSpawnPosition(PositionId positionId)
        {
            return _positions.First(p => p.Id == positionId).Transform;
        }
    }
}