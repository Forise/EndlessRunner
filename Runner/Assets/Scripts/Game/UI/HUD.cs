using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

public class HUD : UIHUD<HUD>
{
    [SerializeField]
    private TMPro.TextMeshProUGUI cherryCounter;

    protected override void Awake()
    {
        base.Awake();
        UserDataControl.Instance.UserData.DataChanged += UpdateCherryCounter;
        UpdateCherryCounter();
    }

    private void UpdateCherryCounter()
    {
        cherryCounter.text = UserDataControl.Instance.UserData.LastScore.ToString();
    }
}
