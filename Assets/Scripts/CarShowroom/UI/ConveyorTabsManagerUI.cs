using PajamaNinja.CarShowRoom;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorTabsManagerUI : MonoBehaviour
{
    private List<ConveyorTabUI> _tabs;
    private ConveyorTabUI _activeTab;

    public Action<int> OnTabActivated;

    private void Awake()
    {
        _tabs = new List<ConveyorTabUI>(GetComponentsInChildren<ConveyorTabUI>());
        _tabs.Reverse();
    }

    public void InitTabs(List<Conveyor> conveyors)
    {
        if (_tabs.Count == conveyors.Count)
        {
            for (int i = 0; i < conveyors.Count; i++)
            {
                _tabs[i].OnActivated -= HandleTabActivated;
                _tabs[i].OnActivated += HandleTabActivated;

                if (conveyors[i].IsUnlocked)
                {
                    UnlockTab(i);
                }
            }
        }
        else
        {
            Debug.LogError("Conveyors tabs count isn't equal to conveyors count");
        }
    }

    public void UnlockTab(int index)
    {
        if (index >= 0 && index < _tabs.Count)
            _tabs[index].Unlock();
    }

    private void ReorderTabs()
    {
        _activeTab.transform.SetAsLastSibling();
        int nextIndex = _tabs.Count - 2;

        for (int i = _tabs.Count - 1; i >= 0; i--)
        {
            ConveyorTabUI tab = _tabs[i];

            if (!tab.Equals(_activeTab))
            {
                tab.transform.SetSiblingIndex(nextIndex);
                nextIndex--;
            }
        }
    }

    public void ActivateTab(int tabID)
    {
        if (tabID >= 0 && tabID < _tabs.Count)
            _tabs[tabID].Activate(true);
    }

    private void HandleTabActivated(ConveyorTabUI tab, bool value)
    {
        if (value)
        {
            if (_activeTab && !_activeTab.Equals(tab))
                _activeTab.Activate(false);

            _activeTab = tab;
            ReorderTabs();

            OnTabActivated?.Invoke(_tabs.IndexOf(_activeTab));
        }
    }
}
