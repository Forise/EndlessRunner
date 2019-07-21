using System;
using Core;
using Core.EventSystem;
using UnityEngine;

public class GameplayControl : MonoSingleton<GameplayControl>
{
    #region Fields
    public Camera cam;
    public Game gamePrefab;
    private Game game;
    private Vector3 defaultCamPos;
    #endregion Fields

    #region Properties
    public Game Game { get => game; }
    public bool IsPaused { get; protected set; }
    #endregion Properties

    private void Awake()
    {
        if (!cam)
            cam = FindObjectOfType<Camera>();
        defaultCamPos = cam.transform.position;
    }

    private void Start()
    {
        EventManager.Subscribe(Events.GameEvents.GAME_STARTED, GameStarted_Handler);
        EventManager.Subscribe(Events.GameEvents.GAME_ENDED, GameEnded_Handler);
    }

    public void StartGame()
    {
        cam.transform.position = defaultCamPos;
        game = Instantiate(gamePrefab, transform);
        
        game.SetUpGame();
        
        EventManager.Notify(this, new GameEventArgs(Events.InputEvents.UNBLOCK_HUD));
        EventManager.Notify(this, new GameEventArgs(Events.GameEvents.GAME_STARTED));
    }

    public void EndGame()
    {
        
        EventManager.Notify(this, new GameEventArgs(Events.InputEvents.BLOCK_HUD));
        EventManager.Notify(this, new GameEventArgs(Events.GameEvents.GAME_ENDED));
        UIControl.Instance.OpenWindow("MainMenuWindow");
        Destroy(game.gameObject);
    }

    public void PauseGame()
    {
        IsPaused = true;
    }

    public void ResumeGame()
    {
        IsPaused = false;
    }

    #region Handlers
    private void PlayerDied_Handler(object sender, GameEventArgs args)
    {
        EventManager.Notify(this, new GameEventArgs(Events.InputEvents.BLOCK_HUD));
        EndGame();
        #region Player resurrect logic (Disabled)
        //        UIControl.Instance.ShowDialogPopUp(new UIPopUp.PopupData("wanna continue for watch AD?",
        //            /*yes*/() =>
        //                   {
        //                       if (Application.internetReachability != NetworkReachability.NotReachable)
        //                       {
        //#if UNITY_EDITOR
        //                           game.Player.Resurrect();
        //                           EventManager.Notify(this, new GameEventArgs(Events.InputEvents.UNBLOCK_HUD));
        //#endif
        //#if !UNITY_EDITOR && UNITY_ANDROID
        //                           EventManager.Notify(this, new GameEventArgs(Events.ADEvents.SHOW_AD, (int)Core.AD.InterstitialType.Reward,
        //                               //Success
        //                               () =>
        //                               {
        //                                   game.Player.Resurrect();
        //                                   EventManager_Input.Notify(this, new GameEventArgs(EventManager_Input.UNBLOCK_HUD));
        //                               },
        //                               //Cancelled
        //                               () =>
        //                               {
        //                                   EndGame();
        //                               }));
        //#endif
        //                       }
        //                       else
        //                       {
        //                           EndGame();
        //                       }
        //                   },
        //            /*no*/() =>
        //                  {
        //                      EndGame();
        //                  }, false));
        #endregion Player resurrect logic (Disabled)
    }

    private void GameStarted_Handler(object sender, GameEventArgs args)
    {
        EventManager.Subscribe(Events.PlayerEvents.PLAYER_DIED, PlayerDied_Handler);
        EventManager.Subscribe(Events.InputEvents.BACK_BUTTON_PRESSED, BackButtonPressed_Handler);
    }

    private void GameEnded_Handler(object sender, GameEventArgs args)
    {
        EventManager.Unsubscribe(Events.PlayerEvents.PLAYER_DIED, PlayerDied_Handler);
        EventManager.Unsubscribe(Events.InputEvents.BACK_BUTTON_PRESSED, BackButtonPressed_Handler);
    }
    private void BackButtonPressed_Handler(object sender, GameEventArgs e)
    {
        PauseGame();
        UIControl.Instance.ShowDialogPopUp(new UIPopUp.PopupData("Do you wanna stop play?",
            () =>
            {
                ResumeGame();
                EndGame();
            },
            () =>
            {
                ResumeGame();
            }, false));
    }
#endregion
}