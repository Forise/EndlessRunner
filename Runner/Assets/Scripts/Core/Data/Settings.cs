using UnityEngine;

namespace Core
{
    [System.Serializable, SerializeField]
    public struct Settings
    {
        public bool isMusicOn;
        public bool isSoundsOn;

        public Settings(bool setDefault)
        {
            isMusicOn = setDefault;
            isSoundsOn = setDefault;
        }
    }
}