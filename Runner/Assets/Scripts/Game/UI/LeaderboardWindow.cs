using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class LeaderboardWindow : Core.UIWindow
{
    public ScrollRect scroll;
    public RectTransform content;
    public Transform firstItemAnchor;
    public LeaderboardUIItem itemPrefab;
    public Button backButton;
    public float itemsOffsetByY;
    private List<LeaderboardUIItem> items = new List<LeaderboardUIItem>();
    private DummyLeaderboardData leaderboardData;

    private void Awake()
    {
        backButton.onClick.AddListener(() => CloseThisWindow());
    }

    protected override void Start()
    {
        base.Start();
        leaderboardData = UserDataControl.Instance.UserData.LeaderboardData;
        LeaderboardController.Instance.GetLeaderboardDataFromServer(SetContent);
    }

    protected override void OpenAnimationStateInitHandler()
    {
        base.OpenAnimationStateInitHandler();
        leaderboardData = UserDataControl.Instance.UserData.LeaderboardData;
        LeaderboardController.Instance.GetLeaderboardDataFromServer(SetContent);
    }

    private void SetContent()
    {
        for (int i = 0; i < items.Count; i++)
        {
            Destroy(items[i].gameObject);
        }
        items.Clear();

        leaderboardData.items.ToList().Sort((x, y) => x.score.CompareTo(y.score));
        leaderboardData.items.Reverse();
        for (int i = 0; i < leaderboardData.items.Length; i++)
        {
            items.Add(Instantiate(itemPrefab, content));
            items[i].transform.localPosition = new Vector3(firstItemAnchor.localPosition.x, firstItemAnchor.localPosition.y - ((itemPrefab.transform as RectTransform).rect.height * i) - itemsOffsetByY);
            items[i].nameText.text = leaderboardData.items[i].name;
            items[i].scoreText.text = leaderboardData.items[i].score.ToString();
            content.rect.Set(content.rect.position.x, content.rect.position.y, content.rect.width, (items.Count * (itemPrefab.transform as RectTransform).rect.height) + (itemsOffsetByY * items.Count));
        }
    }
}
