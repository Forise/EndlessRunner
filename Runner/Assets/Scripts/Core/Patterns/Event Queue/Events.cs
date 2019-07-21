namespace Core.EventSystem
{
    public class Events
    {
        public class SaveData
        {
            public const string NEW_DATA_APPLIED = "NewDataApplied";
        }

        public class PlayerEvents
        {
            public const string PLAYER_JUMPED = "PlayerJumped";
            public const string PLAYER_GEM_PICKED_UP = "PlayerGemPickedUp";
            public const string PLAYER_CHERRY_PICKED_UP = "PlayerCherryPickedUp";
            public const string PLAYER_DIED = "PlayerDied";
        }

        public class InputEvents
        {
            public const string BLOCK_HUD = "BlockHUD";
            public const string UNBLOCK_HUD = "UnblockHUD";
            public const string BLOCK_INPUT = "BlockInput";
            public const string UNBLOCK_INPUT = "UnblockInput";
            public const string UI_TAPPED = "UITapped";
            public const string BACK_BUTTON_PRESSED = "BackButtonPressed";
        }

        public class ApplicationEvents
        {
            public const string APPLICATION_STARTED = "ApplicationStarted";
            public const string FOCUS_ON = "FocusOn";
            public const string FOCUS_LOST = "FocusLost";
            public const string SETTINGS_CHANGED = "SettingsChanged";
        }

        public class GameEvents
        {
            public const string GAME_STARTED = "GameStarted";
            public const string GAME_ENDED = "GameEnded";
            public const string PAUSE_GAME = "PauseGame";
            public const string UNPAUSE_GAME = "UnpauseGame";
        }

        public class AudioEvents
        {
            public const string PLAY_MUSIC = "PlayMusic";
        }
    }
}