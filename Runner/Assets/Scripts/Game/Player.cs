using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.EventSystem;
using Core;

[RequireComponent(typeof(Movement2D_Runner), typeof(Collider2D), typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    #region Fields
    [Tooltip("You can attach any AMovementPresenter by your passion.")]
    public AMovementPresenter movement;
    [SerializeField, Tooltip("The value at which the speed reduces.")]
    private float slowFactorByGem = 1f;
    private Collider2D coll;
    private Rigidbody2D rb;
    #endregion Fields

    #region Unity Methods
    private void Awake()
    {
        rb = rb ?? GetComponent<Rigidbody2D>();
        movement = movement ?? GetComponent<Movement2D_Runner>();
        movement.Speed = UserDataControl.Instance.UserData.LastSpeed;
        coll = coll ?? GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {
        rb.simulated = !GameplayControl.Instance.IsPaused;
        if (!GameplayControl.Instance.IsPaused)
        {
            movement.Move();
            UserDataControl.Instance.UserData.LastSpeed = movement.Speed;
            (movement as Movement2D_Runner).ResumeAnimation();
        }
        else
        {
            (movement as Movement2D_Runner).StopAnimation();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var pickable = collision.gameObject.GetComponent<PickableItemPresenter2D>();
        if(pickable)
        {
            switch(pickable.ItemType)
            {
                case PickableItemType.Cherry:
                    EventManager.Notify(this, new GameEventArgs(Events.PlayerEvents.PLAYER_CHERRY_PICKED_UP));
                    break;
                case PickableItemType.Gem:
                    EventManager.Notify(this, new GameEventArgs(Events.PlayerEvents.PLAYER_GEM_PICKED_UP));
                    movement.Speed -= slowFactorByGem;
                    if (movement.Speed < 1f)
                        movement.Speed = 1f;
                    break;
            }
            pickable.PickUp();
            return;
        }
        switch(collision.tag) //For future extension. It is able to add new behaviors by enother tags.
        {
            case "Enemy":
                Die();
                break;
        }
    }
    #endregion UnityMethods

    #region Methods
    public void Resurrect()
    {
        gameObject.SetActive(true);
    }
    private void Die()
    {
        gameObject.SetActive(false);
        EventManager.Notify(this, new GameEventArgs(Events.PlayerEvents.PLAYER_DIED));
    }

    #endregion Methods
}
