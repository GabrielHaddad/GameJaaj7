using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashObstacle : MonoBehaviour
{
    [SerializeField] LayerMask playerLayer;
    [SerializeField] CapsuleCollider2D topBox;
    [SerializeField] CapsuleCollider2D bottomBox;
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
