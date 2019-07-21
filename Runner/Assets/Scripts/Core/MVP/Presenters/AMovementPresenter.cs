using UnityEngine;

namespace Core
{
    [System.Serializable]
    public abstract class AMovementPresenter : MonoBehaviour
    {
        #region Fields
        public LayerMask groundLayer;
        [SerializeField]
        protected MovementModel movementModel = new MovementModel();
        [SerializeField]
        protected float gorundDetecterRadius = 0.2f;
        [SerializeField]
        protected Transform groundedStartRaycastPoint;

        protected bool isMoving;

        private bool isGrounded;
        #endregion

        #region Properties
        public bool IsMoving { get => isMoving; }

        public bool IsAbleToMove { get => movementModel.IsAbleToMove; }

        public float Speed { get => movementModel.Speed; set => movementModel.Speed = value; }
        public Vector3 Position { get => movementModel.Position; set => movementModel.Position = value; }

        public bool IsGrounded
        {
            get => isGrounded;
        }
        #endregion

        #region Unity Methods
        protected void Awake()
        {
            Init();
        }
        protected virtual void FixedUpdate()
        {
            CheckGrounded();
        }

        protected void OnDestroy()
        {
            movementModel.PositionChanged -= OnPositionUpdated_Handler;
            movementModel.LocalPositionChanged -= OnLocalPosUpdated_Handler;
        }

        protected virtual void OnDisable()
        {
            isMoving = false;
        }
        #endregion Unity Methods        

        #region Methods
        public void DisableMove()
        {
            GetComponent<Rigidbody2D>().simulated = false;
            movementModel.IsAbleToMove = false;
        }

        public void EnableMove()
        {
            GetComponent<Rigidbody2D>().simulated = true;
            movementModel.IsAbleToMove = true;
        }

        protected virtual void CheckGrounded()
        {
            isGrounded = Physics2D.OverlapCircle(groundedStartRaycastPoint.position, gorundDetecterRadius, groundLayer);
        }

        public virtual void StopAllMovement()
        {
            StopAllCoroutines();
            movementModel.IsAbleToMove = true;
        }

        protected virtual void Init()
        {
            movementModel = movementModel ?? new MovementModel();

            movementModel.PositionChanged += OnPositionUpdated_Handler;
            movementModel.LocalPositionChanged += OnLocalPosUpdated_Handler;
            movementModel.RotationChanged += OnRotationUpdated_Handler;
            movementModel.SpeedChanged += OnSpeedUpdated_Handler;
            movementModel.Position = transform.position;
            movementModel.LocalPosition = transform.localPosition;
            movementModel.Rotation = transform.rotation;
        }        

        public abstract void Move();
        #endregion Methods

        protected abstract void OnSpeedUpdated_Handler();
        protected virtual void OnPositionUpdated_Handler()
        {
            transform.position = movementModel.Position;
        }

        protected virtual void OnLocalPosUpdated_Handler()
        {
            transform.localPosition = movementModel.LocalPosition;
        }
        protected virtual void OnRotationUpdated_Handler()
        {
            transform.rotation = movementModel.Rotation;
        }
    }
}