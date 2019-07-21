using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using Core.EventSystem;

public class Game : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private Transform playerStartAnchor;
    [SerializeField]
    private Player player;

    private Camera cam;
    #endregion Fields

    #region Properties
    public Player Player { get => player; }
    #endregion Properties

    private void Awake()
    {
        cam = FindObjectOfType<Camera>();
        cam.gameObject.GetComponent<FollowToObject>().SetupObject(player.gameObject);
    }

    public void SetUpGame()
    {
        player.movement.Position = playerStartAnchor.position;
    }
}
