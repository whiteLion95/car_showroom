using PajamaNinja.UISystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointHand : MonoBehaviour
{
    private UIPopup _uiPopUp;

    private void Awake()
    {
        _uiPopUp = GetComponent<UIPopup>();
    }

    public void Show()
    {
        _uiPopUp.Show();
    }
}
