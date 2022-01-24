using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMechanic : MonoBehaviour
{
    [SerializeField] float ghostVelocity = 15f;
    int positionIndex = 0;
    bool canMove = false;
    List<Vector3> playerPositions;

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

    void MoveGhost()
    {
        if(positionIndex < playerPositions.Count)
        {
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

}
