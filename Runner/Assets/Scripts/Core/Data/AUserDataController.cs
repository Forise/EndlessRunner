using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.EventSystem;

namespace Core
{
    public abstract class AUserDataController<K> : MonoSingleton<AUserDataController<K>> where K : AUserDataPresenter
    {
        #region Fields
        [HideInInspector]
        public bool isSaving;
        public const string fileName = "UserData.json";

        [SerializeField]
        protected K userData;
        protected string path;

        protected bool isLoading;
        #endregion Fields

        #region Properties
        public bool DataWasLoaded { get; private set; }

        public virtual K UserData
        {
            get
            {
               return userData;
            }
            set
            {
                userData = value;
            }
        }
        #endregion Properties

        #region Unity Methods
        protected virtual void Awake()
        {
            Init();
        }

        private void Start()
        {
            UserData.DataChanged += SaveLocal;
        }
        #endregion UnityMethods

        protected virtual void Init()
        {
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
            path = Application.persistentDataPath + "/" + fileName;
#elif UNITY_EDITOR
            path = Application.dataPath + "/" + fileName;
#endif
            LoadLocal();

            Debug.Log(UserData.JsonUD);
        }

        public void SaveLocal()
        {
            if (System.IO.File.Exists(path))
            {
                UserData.StringDateTime = System.DateTime.Now.ToString();
            }
            else
            {
                UserData.StringDateTime = System.DateTime.MinValue.ToString();
            }
            var json = UserData.JsonUD;
            System.IO.File.WriteAllText(path, json);
        }

        public void SaveData()
        {
            if (!isSaving)
            {
                isSaving = true;
                SaveLocal();
#if UNITY_EDITOR
                isSaving = false;
#endif
            }
        }

        public string LoadJson()
        {
            if (System.IO.File.Exists(path))
            {
                return System.IO.File.ReadAllText(path);
            }
            else
                return string.Empty;
        }

        public abstract void LoadLocal();

        protected abstract void SetNewData(string json);

        #region Handlers
        protected void FailDataLoading(object sender, GameEventArgs e)
        {
            isLoading = false;
            Debug.Log(e.str);
        }
        #endregion Handlers
    }
}