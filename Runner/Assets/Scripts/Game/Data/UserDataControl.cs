using UnityEngine;
using Core;

public class UserDataControl : AUserDataController<UserDataPresenter>
{
    public static event System.Action OnLocalDataLoaded = delegate { };

    public override UserDataPresenter UserData
    {
        get
        {
            if (!userData)
                userData = new UserDataPresenter();
            return base.UserData;
        }
        set => base.UserData = value;
    }

    protected override void Awake()
    {
        DontDestroyOnLoad(this);
        if (!UserData)
        {
            UserData = new UserDataPresenter();
        }

        base.Awake();
    }

    public override void LoadLocal()
    {
        var json = LoadJson();
        if(!string.IsNullOrEmpty(json))
        {
            SetNewData(json);
            SaveData();
        }
        else
        {
            SaveData();
        }
    }

    protected override void SetNewData(string json)
    {
        UserData.SetNewModelData(JsonUtility.FromJson<UserDataModel>(json));
        OnLocalDataLoaded?.Invoke();
    }
}