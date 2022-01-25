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
    bool canKill = true;

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

        if (topTouchingPlayer && bottomTouchingPlayer && canKill)
        {
            Debug.Log("Morre");
            playerController.Die();
            StartCoroutine(KillCoolDown());
        }
    }

    IEnumerator KillCoolDown()
    {
        canKill = false;
        yield return new WaitForSeconds(killCoolDownDelay);
        canKill = true;
    }
}
