using UnityEngine;
using Core;

public class UserDataModel : AUserDataModel
{
    [SerializeField]
    private string name;
    [SerializeField]
    private uint highScore;
    [SerializeField]
    private uint lastScore;
    [SerializeField]
    private float lastSpeed;
    [SerializeField]
    private DummyLeaderboardData leaderboardData;
    
    public string Name
    {
        get => name;
        set
        {
            name = value;
            NotifyUserDataChanged();
        }
    }

    public uint HighScore
    {
        get => highScore;
        set
        {
            highScore = value;
            NotifyUserDataChanged();
        }
    }

    public uint LastScore
    {
        get => lastScore;
        set
        {
            lastScore = value;
            NotifyUserDataChanged();
        }
    }

    public float LastSpeed
    {
        get => lastSpeed;
        set
        {
            lastSpeed = value;
            NotifyUserDataChanged();
        }
    }

    public DummyLeaderboardData LeaderboardData
    {
        get => leaderboardData;
        set
        {
            leaderboardData = value;
            NotifyUserDataChanged();
        }
    }
}
