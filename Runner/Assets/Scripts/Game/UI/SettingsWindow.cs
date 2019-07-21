using UnityEngine;
using UnityEngine.UI;
using Core.EventSystem;
using Core;

public class SettingsWindow : UIWindow
{
    #region Fields
    public MyButton backButton;
    public Toggle musicToggle;
    public Toggle soundsToggle;
    #endregion Fields

    #region Unity Methods
    protected override void Start()
    {
        Init();
        backButton.onClick.AddListener(BackButton_Handler);
        musicToggle.onValueChanged.AddListener(OffMusicToggle_Handler);
        soundsToggle.onValueChanged.AddListener(OffSoundsToggle_Handler);
        base.Start();
    }

    private void OnDestroy()
    {
        backButton.onClick.RemoveListener(BackButton_Handler);
        musicToggle.onValueChanged.RemoveListener(OffMusicToggle_Handler);
        soundsToggle.onValueChanged.RemoveListener(OffSoundsToggle_Handler);
    }
    #endregion Unity Methods

    #region Methods
    private void Init()
    {
        musicToggle.isOn = SettingsControl.Instance.settings.isMusicOn;
        soundsToggle.isOn = SettingsControl.Instance.settings.isSoundsOn;
    }

    protected override void OpenAnimationStateInitHandler()
    {
        base.OpenAnimationStateInitHandler();
        GameplayControl.Instance.PauseGame();
    }

    protected override void InactiveStateInitHandler()
    {
        base.InactiveStateInitHandler();
        GameplayControl.Instance.ResumeGame();
    }
    #endregion Methods

    #region Handlers
    private void BackButton_Handler()
    {
        CloseThisWindow();
    }

    private void OffMusicToggle_Handler(bool value)
    {
        SettingsControl.Instance.SetMusic(value);
        EventManager.Notify(this, new GameEventArgs(Events.InputEvents.UI_TAPPED));
    }

    private void OffSoundsToggle_Handler(bool value)
    {
        SettingsControl.Instance.SetSounds(value);
        EventManager.Notify(this, new GameEventArgs(Events.InputEvents.UI_TAPPED));
    }
    #endregion Handlers
}
