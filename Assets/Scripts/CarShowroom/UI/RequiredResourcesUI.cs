using Lean.Pool;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RequiredResourcesUI : MonoBehaviour
{
    [SerializeField] private ResourcesAmountUI _resAmountUIPrefab;

    private List<ResourcesAmountUI> _resAmounts = new();
    private LayoutGroup _layoutGroup;

    private void Awake()
    {
        _layoutGroup = GetComponentInChildren<LayoutGroup>();
    }

    public void AddResAmountUI(ResourceData resData, int amount, int reqAmount)
    {
        ResourcesAmountUI resAmountUI = _resAmounts.Find((r) => r.MyType == resData.resType);

        if (resAmountUI)
        {
            resAmountUI.UpdateAmount(amount, reqAmount);
        }
        else
        {
            resAmountUI = LeanPool.Spawn(_resAmountUIPrefab, _layoutGroup.transform);
            resAmountUI.Init(resData, amount, reqAmount);
            _resAmounts.Add(resAmountUI);
        }
    }

    public void UpdateAmount(ResourceType resType, int amount)
    {
        ResourcesAmountUI resAmountUI = _resAmounts.Find((r) => r.MyType == resType);
        resAmountUI.UpdateAmount(amount);
    }
}
