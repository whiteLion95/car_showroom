using Lean.Pool;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PajamaNinja.CarShowRoom
{
    public class ResourcesBox : InteractableObjectsTaker
    {
        [SerializeField] private ResourcesDataSO _resData;
        [SerializeField] private ResourcesBoxSSO _saveData;

        private Conveyor _conveyor;
        private RequiredResourcesUI _reqResUI;

        public Action OnFull;
        public Action<ResourceType> OnResourceFull;
        public Action<ResourceType, int> OnCountChanged;

        public List<ResourceItem> CurResItems { get; private set; }
        public Dictionary<ResourceType, int> ResTypesAmounts { get; private set; } = new();
        public bool IsFull { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            InitResItems();
            _conveyor = GetComponentInParent<Conveyor>();
            _reqResUI = GetComponentInChildren<RequiredResourcesUI>();
            _reqResUI.gameObject.SetActive(false);

            _conveyor.OnCarSpawned += HandleCarSpawned;
        }

        protected override void Start()
        {
            base.Start();
        }

        public void InitResItems()
        {
            CurResItems = new List<ResourceItem>();

            for (int i = 0; i < _saveData.CurItemsTypes.Count; i++)
            {
                ResourceType resType = _saveData.CurItemsTypes[i];
                ResourceItem resItem = LeanPool.Spawn(_resData[resType].resItem, transform.position, Quaternion.identity);
                resItem.gameObject.SetActive(false);
                CurResItems.Add(resItem);

                if (!ResTypesAmounts.ContainsKey(resType))
                    ResTypesAmounts.Add(resType, 1);
                else
                    ResTypesAmounts[resType]++;
            }
        }

        public override void TakeObj(InteractableObject interObj, InteractableObjectsDataSO data)
        {
            base.TakeObj(interObj, data);
            ResourceItem resItem = interObj.GetComponent<ResourceItem>();

            if (resItem)
            {
                CurResItems.Add(resItem);
                _saveData.AddItem(resItem);
            }

            if (!ResTypesAmounts.ContainsKey(resItem.MyType))
                ResTypesAmounts.Add(resItem.MyType, 1);
            else
                ResTypesAmounts[resItem.MyType]++;

            if (IsEnoughAmount(resItem.MyType))
                OnResourceFull?.Invoke(resItem.MyType);

            _reqResUI.UpdateAmount(resItem.MyType, ResTypesAmounts[resItem.MyType]);
            CheckResourcesRequirements();
            OnCountChanged?.Invoke(resItem.MyType, ResTypesAmounts[resItem.MyType]);
        }

        private void CheckResourcesRequirements()
        {
            if (ResTypesAmounts.Count < _conveyor.CarReqResources.Count)
                return;

            foreach (var resTypes in ResTypesAmounts)
            {
                if (!IsEnoughAmount(resTypes.Key))
                    return;
            }

            ActionsOnFull();
        }

        public bool IsEnoughAmount(ResourceType resType)
        {
            if (ResTypesAmounts.ContainsKey(resType))
            {
                CarRequiredResources reqResources = _conveyor.CarReqResources.Find((r) => r.type == resType);

                if (ResTypesAmounts[resType] >= reqResources.amount)
                    return true;
            }

            return false;
        }

        public override void HandleObjInPlace(InteractableObject interObj)
        {
            base.HandleObjInPlace(interObj);
            interObj.gameObject.SetActive(false);
        }

        private void ActionsOnFull()
        {
            IsFull = true;
            OnFull?.Invoke();

            Refresh();
        }

        public void Refresh()
        {
            for (int i = 0; i < CurResItems.Count; i++)
            {
                LeanPool.Despawn(CurResItems[i]);
            }

            CurResItems.Clear();
            _saveData.ClearItems();
            ResTypesAmounts.Clear();
            _reqResUI.gameObject.SetActive(false);
        }

        private void HandleCarSpawned(Car car)
        {
            _reqResUI.gameObject.SetActive(true);

            foreach (var reqResources in _conveyor.CarReqResources)
            {
                _reqResUI.AddResAmountUI(_resData[reqResources.type], 0, reqResources.amount);

                if (ResTypesAmounts.ContainsKey(reqResources.type))
                    _reqResUI.UpdateAmount(reqResources.type, ResTypesAmounts[reqResources.type]);
            }
        }
    }
}