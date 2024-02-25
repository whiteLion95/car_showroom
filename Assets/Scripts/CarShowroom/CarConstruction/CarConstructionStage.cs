using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PajamaNinja.CarShowRoom
{
    public class CarConstructionStage : MonoBehaviour
    {
        [SerializeField] private List<RobotArm> _robotArms;

        private List<CarConstructionPart> _parts;
        private CarConstructionDataSO _data;

        private void Awake()
        {
            _parts = new List<CarConstructionPart>(GetComponentsInChildren<CarConstructionPart>(true));
            _data = GetComponentInParent<CarConstruction>().Data;
        }

        public void PlayStage(Action onComplete = null)
        {
            for (int i = 0; i < _parts.Count; i++)
            {
                _parts[i].Init();
                _parts[i].transform.DOScale(Vector3.zero, _data.PartsAppearDuration).SetEase(_data.PartsApearEase).From();
            }

            StartCoroutine(StageRoutine(onComplete));
        }

        private IEnumerator StageRoutine(Action onComplete = null)
        {
            yield return new WaitForSeconds(_data.PlayStageDelay);

            for (int i = 0; i < _parts.Count; i++)
            {
                if (i < _parts.Count - 1)
                    _parts[i].Tween(_data.PartTweenDuration, _data.PartsTweenEase);
                else
                    _parts[i].Tween(_data.PartTweenDuration, _data.PartsTweenEase, onComplete);

                if (_parts[i].TweenType != ConstructTweenType.None)
                    yield return new WaitForSeconds(_data.IntervalBetweenParts);
            }
        }
    }
}