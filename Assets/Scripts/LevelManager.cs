using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject ghostPrefab;
    Dictionary<int, List<Vector3>> playerPositions = new Dictionary<int, List<Vector3>>();
    int currentLevelIndex = 0;
    PlayerController playerController;
    GhostMechanic ghostMechanic;

    void Awake() 
    {
        playerController = FindObjectOfType<PlayerController>();
        ghostMechanic = FindObjectOfType<GhostMechanic>();
    }

    void Start() 
    {
        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public List<Vector3> GetPositions()
    {
        return playerPositions[currentLevelIndex];
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Player")
        {
            playerPositions[currentLevelIndex] = playerController.GetPlayerPositions();
            Instantiate(ghostPrefab, playerPositions[currentLevelIndex][0], Quaternion.identity);
        }
    }
}
