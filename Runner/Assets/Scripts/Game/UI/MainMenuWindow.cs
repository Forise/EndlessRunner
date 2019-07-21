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
        startButton.onClick.AddListener(() => 
        {
            StartCoroutine(StartButtonCorotine());
        }
        );
        exitButton.onClick.AddListener(() => { Application.Quit(); });
    }

    private IEnumerator StartButtonCorotine()
    {
        UIControl.Instance.OpenWindow("LoadingWindow");
        yield return new WaitForSeconds(2);
        GameplayControl.Instance.StartGame();
        UIControl.Instance.CloseWindow("LoadingWindow");
        CloseThisWindow();
    }
}
