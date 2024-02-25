using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    public class ScreenTutor
    {
        private CanvasGroup _canvasGroup;
        private List<Button> _allButtons;
        private Button _pointedButton;
        private List<Button> _turnedOffButtons;
        private GameObject _pointObj;

        public Action OnPointedButtonClicked;

        public ScreenTutor(CanvasGroup canvasGroup, GameObject pointObj)
        {
            _canvasGroup = canvasGroup;
            _pointObj = pointObj;
            _pointObj.SetActive(false);
            SetButtons();
        }

        public void SetButtons()
        {
            _allButtons = new List<Button>(_canvasGroup.GetComponentsInChildren<Button>(true));
        }

        public void PointToButton(Button button)
        {
            _turnedOffButtons = new List<Button>();

            for (int i = 0; i < _allButtons.Count; i++)
            {
                if (!_allButtons[i].Equals(button))
                {
                    if (_allButtons[i].interactable)
                    {
                        _allButtons[i].interactable = false;
                        _turnedOffButtons.Add(_allButtons[i]);
                    }
                }
                else
                {
                    _pointedButton = _allButtons[i];
                    _pointedButton.interactable = true;
                    _pointedButton.onClick.AddListener(HandlePointedButtonClicked);

                    _pointObj.transform.SetParent(_canvasGroup.transform);
                    _pointObj.transform.position = _pointedButton.transform.position;
                    _pointObj.SetActive(true);
                }
            }
        }

        private void HandlePointedButtonClicked()
        {
            _pointedButton.onClick.RemoveListener(HandlePointedButtonClicked);

            for (int i = 0; i < _turnedOffButtons.Count; i++)
            {
                _turnedOffButtons[i].interactable = true;
            }

            _turnedOffButtons.Clear();
            _pointObj.SetActive(false);

            OnPointedButtonClicked?.Invoke();
        }
    }
}