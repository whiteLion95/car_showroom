using PajamaNinja.Common;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PajamaNinja.SaveSystem
{
    public class SaveManager : SingleReference<SaveManager>
    {
        [SerializeField]
        [AssetList]
        private List<SaveableBaseSO> _saveableScriptables = new List<SaveableBaseSO>();

        //[SerializeField]
        //private float _saveTimeoutSec = 30f;

        //[ShowInInspector, ReadOnly]
        //private float _saveTimer;

        protected override void Awake()
        {
            base.Awake();
            _saveableScriptables.ForEach((s) => s.Initialize());
        }

        //private void Start()
        //{
        //    _saveTimer = _saveTimeoutSec;
        //}

        //private void Update()
        //{
        //    _saveTimer -= Time.deltaTime;

        //    if ( _saveTimer < 0 )
        //    {
        //        StartCoroutine(SaveAllDirtyCoroutine("OnTimer"));
        //        _saveTimer = _saveTimeoutSec;
        //    }
        //}

        private void OnApplicationQuit()
        {
            SaveAllDirty("OnApplicationQuit");
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            SaveAllDirty("OnApplicationPause");
        }

        public void SaveAllDirty(string placement)
        {
            var count = 0;

            for (int i = 0; i < _saveableScriptables.Count; i++)
            {
                if (_saveableScriptables[i]._saveIsDirty)
                {
                    _saveableScriptables[i].TrySave();
                    count++;
                }
            }

            //Debug.Log($"[SaveManger] saving dirty saveableSO | Count: {count} | Place: {placement}");
        }

        public IEnumerator SaveAllDirtyCoroutine(string placement)
        {
            var count = 0;

            var waiter = new WaitForEndOfFrame();

            for (int i = 0; i < _saveableScriptables.Count; i++)
            {
                if (_saveableScriptables[i]._saveIsDirty)
                {
                    _saveableScriptables[i].TrySave();
                    count++;
                }
                yield return waiter;
            }

            Debug.Log($"[SaveManger] saving dirty saveableSO | Count: {count} | Place: {placement}");

            yield return null;
        }
    }
}