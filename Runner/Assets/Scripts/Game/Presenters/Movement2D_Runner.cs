using UnityEngine;
using Core;
using Core.EventSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class Movement2D_Runner : AMovementPresenter
{
    #region Fields
    public Animator animator;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private float animationSpeedMultiplyer;
    [SerializeField]
    private float jumpTime = .35f;
    private float jumpTimer;
    private bool isJumping = false;
    #endregion Fields

    #region Unity Methods
    private void Start()
    {
        rb.velocity = Vector3.right * movementModel.Speed;
    }
    #endregion Unity Methods

    #region Methods
    protected override void Init()
    {
        base.Init();
        animator = animator ?? GetComponent<Animator>();
        rb = rb ?? GetComponent<Rigidbody2D>();
        animator.SetFloat("Speed", movementModel.Speed * animationSpeedMultiplyer);
    }

    private void Jump()
    {
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && IsGrounded)
        {
            rb.velocity = new Vector2(movementModel.Speed, Vector2.up.y * jumpForce);
            isJumping = true;
            jumpTimer = jumpTime;
            EventManager.Notify(this, new GameEventArgs(Events.PlayerEvents.PLAYER_JUMPED));
        }
        if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)) && isJumping)
        {
            if (jumpTimer > 0)
            {
                rb.velocity = new Vector2(movementModel.Speed, Vector2.up.y * jumpForce);
                jumpTimer -= Time.deltaTime;
            }
            else
                isJumping = false;
        }
        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonUp(0))
        {
            isJumping = false;
        }
    }

    public override void Move()
    {
        rb.velocity = new Vector2(movementModel.Speed, rb.velocity.y);
        Jump();
        movementModel.Speed += Time.deltaTime * .1f;
        animator.SetBool("IsGrounded", IsGrounded);
        animator.SetFloat("Speed", movementModel.Speed * animationSpeedMultiplyer);
    }

    public void StopAnimation()
    {
        animator.SetFloat("Speed", 0);
    }
    public void ResumeAnimation()
    {
        animator.SetFloat("Speed", movementModel.Speed * animationSpeedMultiplyer);
    }
    #endregion Methods

    #region Handlers
    protected override void OnSpeedUpdated_Handler()
    {
        animator.SetFloat("Speed", movementModel.Speed * animationSpeedMultiplyer);
    }
    #endregion Handlers
}