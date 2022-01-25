using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    
    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Player")
        {
            PlayerController player = other.GetComponent<PlayerController>();
            bool canPlayerCollide = player.CanPlayerCollide();

            if (canPlayerCollide)
            {
                player.Die();
            }
        }
    }
}
