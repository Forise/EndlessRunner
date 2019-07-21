using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Core;
using Core.EventSystem;

public class MainMenuWindow : UIWindow
{
    public Button startButton;
    public Button exitButton;

    private void Awake()
    {
        startButton.onClick.AddListener(() => { GameplayControl.Instance.StartGame(); CloseThisWindow(); });
        exitButton.onClick.AddListener(() => { Application.Quit(); });
    }
}
