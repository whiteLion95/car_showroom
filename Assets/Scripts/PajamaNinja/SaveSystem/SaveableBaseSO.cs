using PajamaNinja.Common;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Data;
using UnityEngine;

namespace PajamaNinja.SaveSystem
{
    [Serializable]
    public class SaveDataInSO
    {

    }

    public abstract class SaveableBaseSO : SerializedScriptableObject
    {
        [TabGroup("Tabs", "Debug")]
        [ReadOnly]
        public bool _saveIsLoaded = false, _saveIsDirty = false;

        [TabGroup("Tabs", "Debug")]
        [ReadOnly]
        public string _saveKey;

        public abstract bool IsSaveExists { get; }

        public abstract void TryLoad();

        public abstract void TrySave(bool isInstant = true);
        public abstract void Initialize();
    }

    public class SaveableSO<T> : SaveableBaseSO where T : SaveDataInSO, new()
    {
        [TabGroup("Tabs", "Debug")]
        [ShowInInspector, ReadOnly]
        protected T _saveData = new T();

        protected virtual void OnEnable()
        {
            Initialize();
        }

        public override bool IsSaveExists => ES3.KeyExists(_saveKey);

        public override void Initialize()
        {
            _saveData = new T();
            _saveIsLoaded = false;
            _saveIsDirty = false;

#if UNITY_EDITOR

            if (_saveKey.IsNullOrWhitespace())
                GenerateKey();

            if (!CheckUniqness(_saveKey))
                GenerateKey();
#endif

            if (_saveKey.IsNullOrWhitespace()) throw new DataException($"[SSO-{name}] _saveKey is empty");
        }

        public override void TryLoad()
        {
            if (_saveIsLoaded) return;

            if (!ES3.KeyExists(_saveKey))
            {
                TrySave();
                _saveIsLoaded = true;
                return;
            }

            _saveData = new T();

            _saveData = ES3.Load<T>(_saveKey);

            _saveIsLoaded = true;
        }

        public override void TrySave(bool isInstant = true)
        {
            if (isInstant)
            {
                ES3.Save(_saveKey, _saveData);
                _saveIsDirty = false;
            }
            else
            {
                _saveIsDirty = true;
            }

        }

#if UNITY_EDITOR

        private bool CheckUniqness(string key)
        {
            //var allSaveables = HelperMethods.GetAllInstances<SaveableSO<T>>();
            var allSaveables = HelperMethods.GetAllInstances<SaveableBaseSO>();

            return allSaveables.FindAll(item => item._saveKey == key).Count <= 1;
        }

        [TitleGroup("Utilities")]
        [ButtonGroup("Utilities/Buttons")]
        [Button]
        public void GenerateKey() => _saveKey = Guid.NewGuid().ToString();


        [TitleGroup("Utilities")]
        [ButtonGroup("Utilities/Buttons")]
        [Button]
        private void ForceSave()
        {
            UnityEditor.EditorUtility.SetDirty(this);

            UnityEditor.AssetDatabase.SaveAssets();
        }

        [TitleGroup("Utilities")]
        [ButtonGroup("Utilities/Buttons")]
        [Button]
        public void ResetSave()
        {
            _saveIsLoaded = false;
            _saveData = new T();
            ES3.DeleteKey(_saveKey);
        }
#endif

    }
}