using System;
using System.Collections.Generic;
using UnityEngine;

namespace PajamaNinja.CarShowRoom
{
    public class CarConstruction : MonoBehaviour
    {
        [SerializeField] private CarConstructionDataSO _data;

        private List<CarConstructionStage> _stages;
        private int _curStage;

        public CarConstructionDataSO Data => _data;

        private void Awake()
        {
            _stages = new List<CarConstructionStage>(GetComponentsInChildren<CarConstructionStage>(true));
        }

        public void InitCurStage(int stage)
        {
            _curStage = Math.Clamp(stage, 0, _stages.Count - 1);

            for (int i = 0; i < _stages.Count; i++)
            {
                if (stage > 0 && i == 0)
                {
                    _stages[i].gameObject.SetActive(false);
                    continue;
                }

                if (i <= stage)
                    _stages[i].gameObject.SetActive(true);
                else
                    _stages[i].gameObject.SetActive(false);
            }
        }

        public void InitLastStage()
        {
            InitCurStage(_stages.Count - 1);
        }

        public void NextStage(Action onComplete = null)
        {
            _stages[0].gameObject.SetActive(false);
            _curStage++;

            if (_curStage < _stages.Count)
            {
                _stages[_curStage].gameObject.SetActive(true);
                _stages[_curStage].PlayStage(onComplete);
            }
        }
    }
}