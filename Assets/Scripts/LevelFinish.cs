using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFinish : MonoBehaviour
{
    GhostSpawner ghostSpawner;
    bool canCollide = true;

    void Awake() 
    {
        ghostSpawner = FindObjectOfType<GhostSpawner>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && canCollide)
        {
            ghostSpawner.SpawnNewLevel();
            canCollide = false;
        }
    }
}
