namespace Core.EventSystem
{
    public class Events
    {
        public class SaveData
        {
            public const string NEW_DATA_APPLIED = "NewDataApplied";
        }
        public class DateTime
        {
            public const string NIST_DATE_GOTTEN = "NistDateGotten";
            public const string URMOBI_DATE_GOTTEN = "UrmobiDateGotten";
        }

        public class PlayerEvents
        {
            public const string PLAYER_JUMP = "PlayerJump";
            public const string PLAYER_JUMPED = "PlayerJumped";
        }

        public class InputEvents
        {
            public const string BLOCK_HUD = "BlockHUD";
            public const string UNBLOCK_HUD = "UnblockHUD";
            public const string BLOCK_INPUT = "BlockInput";
            public const string UNBLOCK_INPUT = "UnblockInput";
            public const string UI_TAPPED = "UITapped";
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
        }
    }
}