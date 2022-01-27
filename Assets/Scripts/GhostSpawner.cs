using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> ghostPrefabs;
    Dictionary<int, List<Vector3>> playerPositions = new Dictionary<int, List<Vector3>>();
    int previousLevelIndex = 0;
    PlayerController playerController;
    LevelManager levelManager;
    bool canSpawn = false;

    static GhostSpawner instance;

    void ManageSingleton()
    {
        if (instance != null)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Awake()
    {
        ManageSingleton();
        playerController = FindObjectOfType<PlayerController>();
        levelManager = FindObjectOfType<LevelManager>();
    }

    void Update()
    {
        if (levelManager.HasLoadedLevel() && canSpawn)
        {
            SpawnGhosts(levelManager.GetActiveSceneIndex() + 1);
            canSpawn = false;
        }
    }

    void SpawnGhosts(int currentLevelIndex)
    {
        for (int i = 0; i <= currentLevelIndex - 1; i++)
        {
            GameObject instance = Instantiate(ghostPrefabs[i], playerPositions[i][0], Quaternion.identity);
            GhostMechanic ghost = instance.GetComponent<GhostMechanic>();

            ghost.SetPlayerPositions(playerPositions[i]);
            ghost.EnableMovement();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            previousLevelIndex = levelManager.GetActiveSceneIndex() - 1;
            playerPositions[previousLevelIndex] = playerController.GetPlayerPositions();

            levelManager.LoadNextLevel();
            canSpawn = true;
        }
    }
}
