using System.Collections;
using UnityEngine;
using Core;
using Core.EventSystem;
using UnityEngine.Networking;
using System.Linq;

public class LeaderboardController : MonoSingleton<LeaderboardController>
{
    public DummyLeaderboardDataObject dataObject;
    public bool useDummy = true;
    public event System.Action DataUploaded = delegate { };
    public event System.Action DataDownloaded = delegate { };

    protected string path;
    public const string fileName = "Leaderboard.json";

    private void Awake()
    {
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
        path = Application.persistentDataPath + "/" + fileName;
#elif UNITY_EDITOR
        path = Application.dataPath + "/" + fileName;
#endif
    }

    private void Start()
    {
        EventManager.Subscribe(Events.PlayerEvents.PLAYER_DIED, OnPlayerDied_Handler);
        GetLeaderboardDataFromServer();
    }

    public void SaveLeaderboardToServer(System.Action actionOnUpload = null)
    {
        StartCoroutine(SaveLeaderboardToServerCoroutine(actionOnUpload));
    }

    public void GetLeaderboardDataFromServer(System.Action actionOnDownload = null)
    {
        StartCoroutine(GetLeaderboardDataFromServerCoroutine(actionOnDownload));
    }

    private IEnumerator SaveLeaderboardToServerCoroutine(System.Action actionOnUpload = null)
    {
        if (!useDummy)
        {
            WWWForm form = new WWWForm();
            try
            {
                form.AddField("myHighScore", JsonUtility.ToJson(UserDataControl.Instance.UserData.LeaderboardData.items.ToList().Find(x => x.name == UserDataControl.Instance.UserData.Name)));
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex, this);
            }

            UnityWebRequest www = UnityWebRequest.Post("http://www.my-server.com/myform", form);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
                DataUploaded += actionOnUpload;
                DataUploaded?.Invoke();
                DataUploaded = delegate { };
            }
        }
        else
        {
            SaveLocal();
            DataUploaded += actionOnUpload;
            DataUploaded?.Invoke();
            DataUploaded = delegate { };
        }
    }

    private void SaveLocal()
    {
        var json = JsonUtility.ToJson(UserDataControl.Instance.UserData.LeaderboardData);
        System.IO.File.WriteAllText(path, json);
    }

    private string LoadJson()
    {
        if (!System.IO.File.Exists(path))
        {
            SaveLocal();
            return System.IO.File.ReadAllText(path);
        }
        else
            return System.IO.File.ReadAllText(path);
    }

    private IEnumerator GetLeaderboardDataFromServerCoroutine(System.Action actionOnDownload = null)
    {
        if (!useDummy)
        {
            UnityWebRequest www = UnityWebRequest.Get("http://www.my-server.com");
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                //Apply new data
                UserDataControl.Instance.UserData.LeaderboardData = JsonUtility.FromJson<DummyLeaderboardData>(www.downloadHandler.text);
                byte[] results = www.downloadHandler.data;
                DataDownloaded += actionOnDownload;
                DataDownloaded?.Invoke();
                DataDownloaded = delegate { };
            }
        }
        else
        {
            UserDataControl.Instance.UserData.LeaderboardData = JsonUtility.FromJson<DummyLeaderboardData>(LoadJson());
            DataDownloaded += actionOnDownload;
            DataDownloaded?.Invoke();
            DataDownloaded = delegate { };
        }
    }

    #region Handlers
    private void OnPlayerDied_Handler(object sender, GameEventArgs e)
    {
        var userData = UserDataControl.Instance.UserData;
        for (int i = 0; i < userData.LeaderboardData.items.Length; i++)
        {
            if (userData.LeaderboardData.items[i].name == userData.Name)
            {
                userData.LeaderboardData.items[i].score = userData.HighScore;
                break;
            }
        }
        SaveLeaderboardToServer();
    }
    #endregion Handlers
}
