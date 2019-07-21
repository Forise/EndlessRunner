[System.Serializable]
public struct LeaderboardItem
{
    public string name;
    public uint score;

    public uint Score { get => score; set => score = value; }
}