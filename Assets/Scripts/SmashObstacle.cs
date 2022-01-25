using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashObstacle : MonoBehaviour
{
    [SerializeField] LayerMask playerLayer;
    [SerializeField] BoxCollider2D topBox;
    [SerializeField] BoxCollider2D bottomBox;
    [SerializeField] float killCoolDownDelay = 1f;
    PlayerController playerController;

    void Awake() 
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        bool topTouchingPlayer = topBox.IsTouchingLayers(playerLayer);
        bool bottomTouchingPlayer = bottomBox.IsTouchingLayers(playerLayer);

        bool canKillPlayer = playerController.CanPlayerCollide();

        if (topTouchingPlayer && bottomTouchingPlayer && canKillPlayer)
        {
            playerController.Die();
        }
    }
}
