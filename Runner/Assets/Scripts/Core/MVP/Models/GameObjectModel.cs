using UnityEngine;

namespace Core
{
    public class GameObjectModel : Model
    {
        #region Fields
        protected Vector3 position;
        protected Vector3 localPosition;
        protected Quaternion rotation;
        #endregion Fields

        #region Properties
        public Vector3 Position
        {
            get => position;
            set
            {
                position = value;
                PositionChanged?.Invoke();
            }
        }

        public Vector3 LocalPosition
        {
            get => localPosition;
            set
            {
                localPosition = value;
                LocalPositionChanged?.Invoke();
            }
        }

        public Quaternion Rotation
        {
            get => rotation;
            set
            {
                rotation = value;
                RotationChanged?.Invoke();
            }
        }
        #endregion Properties

        #region Events
        public event ModelChangedDelegate PositionChanged;
        public event ModelChangedDelegate LocalPositionChanged;
        public event ModelChangedDelegate RotationChanged;
        #endregion Events
    }
}