using UnityEngine;

namespace Core
{
    public class SettingsControl : MonoSingleton<SettingsControl>
    {
        [SerializeField]
        public Settings settings = new Settings(true);
        private string path;

        private void Awake()
        {
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
        path = Application.persistentDataPath + "/Settings.json";
#elif UNITY_EDITOR
            path = Application.dataPath + "/Settings.json";
#endif
            LoadData();
            SaveData();
            Debug.Log(GetSaveDataAsJson());
        }

        public void SetMusic(bool value)
        {
            settings.isMusicOn = value;
            NotifySettingsChanged();
        }

        public void SetSounds(bool value)
        {
            settings.isSoundsOn = value;
            NotifySettingsChanged();
        }

        private void NotifySettingsChanged()
        {
            EventSystem.EventManager.Notify(this, new EventSystem.GameEventArgs(EventSystem.Events.ApplicationEvents.SETTINGS_CHANGED));
            SaveData();
        }

        public string GetSaveDataAsJson()
        {
            return JsonUtility.ToJson(settings);
        }

        public void SaveData()
        {
            var json = JsonUtility.ToJson(settings);
            System.IO.File.WriteAllText(path, json);
        }

        private void LoadData()
        {
            if (System.IO.File.Exists(path))
            {
                var json = System.IO.File.ReadAllText(path);
                settings = JsonUtility.FromJson<Settings>(json);
            }
            else
            {
                SaveData();
                LoadData();
            }
        }
    }
}