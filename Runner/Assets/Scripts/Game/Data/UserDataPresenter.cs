using System;
using Core;
using Core.EventSystem;

public class UserDataPresenter : AUserDataPresenter
{
    #region Properties
    protected override AUserDataModel Model { get; set; } = new UserDataModel();

    public string Name { get => (Model as UserDataModel).Name; set => (Model as UserDataModel).Name = value; }
    public uint HighScore { get => (Model as UserDataModel).HighScore; set => (Model as UserDataModel).HighScore = value; }
    public uint LastScore { get => (Model as UserDataModel).LastScore; set => (Model as UserDataModel).LastScore = value; }
    public float LastSpeed { get => (Model as UserDataModel).LastSpeed; set => (Model as UserDataModel).LastSpeed = value; }
    #endregion Properties

    public UserDataPresenter ()
    {
        Model = new UserDataModel();
        HighScore = 0;
        LastScore = 0;
        LastSpeed = 1f;
    }

    private void Awake()
    {
        EventManager.Subscribe(Events.PlayerEvents.PLAYER_CHERRY_PICKED_UP, OnCherryPickedUp_Handler);
        EventManager.Subscribe(Events.PlayerEvents.PLAYER_DIED, OnPlayerDied_Handler);
    }

    #region Methods
    public override void SetNewModelData(AUserDataModel model)
    {
        Model = (model as UserDataModel);
    }
    #endregion Methods

    #region Handlers
    private void OnCherryPickedUp_Handler(object sender, GameEventArgs e)
    {
        LastScore++;
    }

    private void OnPlayerDied_Handler(object sender, GameEventArgs e)
    {
        HighScore = HighScore < LastScore ? LastScore : HighScore;
        LastScore = 0;
        LastSpeed = 1;
    }
    #endregion Handlers
}