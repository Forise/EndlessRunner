using System.Collections;
using UnityEngine;
using Core;
using Core.EventSystem;
using UnityEngine.Networking;

public class LeaderboardController : MonoSingleton<LeaderboardController>
{
    public DummyLeaderboardDataObject dataObject;
    public bool useDummy = true;
    public event System.Action DataUploaded = delegate { };
    public event System.Action DataDownloaded = delegate { };

    private void Awake()
    {
        EventManager.Subscribe(Events.PlayerEvents.PLAYER_DIED, OnPlayerDied_Handler);
    }

    private void Start()
    {
        GetLeaderboardDataFromServer();
    }

    public IEnumerator SaveLeaderboardToServer(System.Action actionOnUpload = null)
    {
        if (!useDummy)
        {
            WWWForm form = new WWWForm();
            try
            {
                form.AddField("myHighScore", JsonUtility.ToJson(UserDataControl.Instance.UserData.LeaderboardData.items.Find(x => x.name == UserDataControl.Instance.UserData.Name)));
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
            dataObject.leaderboardData = UserDataControl.Instance.UserData.LeaderboardData;
            dataObject.SetDirty();
            DataUploaded += actionOnUpload;
            DataUploaded?.Invoke();
            DataUploaded = delegate { };
        }
    }

    public IEnumerator GetLeaderboardDataFromServer(System.Action actionOnDownload = null)
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
            UserDataControl.Instance.UserData.LeaderboardData = dataObject.leaderboardData;
            DataDownloaded += actionOnDownload;
            DataDownloaded?.Invoke();
            DataDownloaded = delegate { };
        }
    }

    #region Handlers
    private void OnPlayerDied_Handler(object sender, GameEventArgs e)
    {
        SaveLeaderboardToServer();
    }
    #endregion Handlers
}
