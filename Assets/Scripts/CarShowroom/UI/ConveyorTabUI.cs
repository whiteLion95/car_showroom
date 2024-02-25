using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConveyorTabUI : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Color _activeImgColor;
    [SerializeField] private Color _inActiveImgColor;
    [SerializeField] private Color _activeTextColor;
    [SerializeField] private Color _inActiveTextColor;
    [SerializeField] private GameObject _lockImg;

    public Action<ConveyorTabUI, bool> OnActivated;

    private bool _isUnlocked;
    private Button _button;

    private void Awake()
    {
        _button = GetComponentInChildren<Button>();
        _button.interactable = false;
        _button.onClick.AddListener(HandleButtonClicked);
        _lockImg.SetActive(true);
    }

    public void Unlock()
    {
        _lockImg.SetActive(false);
        _isUnlocked = true;
        _button.interactable = true;
    }

    public void Activate(bool value)
    {
        if (value)
        {
            _image.color = _activeImgColor;
            _text.color = _activeTextColor;
        }
        else
        {
            _image.color = _inActiveImgColor;
            _text.color = _inActiveTextColor;
        }

        OnActivated?.Invoke(this, value);
    }

    private void HandleButtonClicked()
    {
        if (_isUnlocked)
            Activate(true);
    }
}
