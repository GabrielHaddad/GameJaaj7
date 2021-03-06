using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMechanic : MonoBehaviour
{
    [SerializeField] float ghostVelocity = 15f;
    int positionIndex = 0;
    bool canMove = false;
    List<Vector3> playerPositions;
    List<Vector3> playerScale;
    List<bool> playerRunning;
    List<bool> playerDashing;
    List<bool> playerJumping;
    List<bool> playerGrapling;
    List<bool> playerSliding;
    Animator ghostAnimator;

    void Awake() 
    {
        ghostAnimator = GetComponent<Animator>();
    }

    void Update() 
    {
        if(canMove)
        {
            MoveGhost();
        }
    }

    public void EnableMovement()
    {
        canMove = true;
    }

    public void DisableMovement()
    {
        canMove = false;
    }

    public void SetPlayerPositions(List<Vector3> positions)
    {
        playerPositions = positions;
    }

    public void SetPlayerRunning(List<bool> runningPos)
    {
        playerRunning = runningPos;
    }

    public void SetPlayerDashing(List<bool> dashingPos)
    {
        playerDashing = dashingPos;
    }

    public void SetPlayerJumping(List<bool> jumping)
    {
        playerJumping = jumping;
    }

    public void SetPlayerGrappling(List<bool> grappling)
    {
        playerGrapling = grappling;
    }

    public void SetPlayerScale(List<Vector3> scales)
    {
        playerScale = scales;
    }

    public void SetPlayerSliding(List<bool> sliding)
    {
        playerSliding = sliding;
    }

    void MoveGhost()
    {
        if(positionIndex < playerPositions.Count)
        {
            ghostAnimator.SetBool("isRunning", playerRunning[positionIndex]);
            ghostAnimator.SetBool("isDashing", playerDashing[positionIndex]);
            ghostAnimator.SetBool("isGrapling", playerGrapling[positionIndex]);
            ghostAnimator.SetBool("isJumping", playerJumping[positionIndex]);
            ghostAnimator.SetBool("isWallSliding", playerSliding[positionIndex]);
            FlipSprite(positionIndex);


            Vector3 targetPosition = playerPositions[positionIndex];

            float delta = ghostVelocity * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, delta);

            if (transform.position == targetPosition)
            {
                positionIndex++;
            }
        }
        else
        {
            //DisableMovement();
            //Destroy(gameObject);
            positionIndex = 0;
            transform.position = playerPositions[0];
        }
    }

    void FlipSprite(int index)
    {        
        transform.localScale = playerScale[index];
    }
}
