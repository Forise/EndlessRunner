using UnityEngine;
namespace Core
{
    [System.Serializable]
    public class MovementModel : GameObjectModel
    {
        #region Fields
        [SerializeField]
        private float speed;
        private bool isAbleToMove = true;
        #endregion Fields

        #region Properties
        public bool IsAbleToMove
        {
            get => isAbleToMove;
            set
            {
                isAbleToMove = value;
                OnMoveAbilityChanged?.Invoke();
            }
        }

        public float Speed
        {
            get { return speed; }
            set
            {
                speed = value;
                SpeedChanged?.Invoke();
            }
        }
        #endregion Properties

        #region Events
        public event ModelChangedDelegate SpeedChanged;
        public event ModelChangedDelegate OnMoveAbilityChanged;
        #endregion Events

        public MovementModel() { }

        public MovementModel(Vector2 position, float speed)
        {
            this.position = position;
            this.speed = speed;
        }
    }
}