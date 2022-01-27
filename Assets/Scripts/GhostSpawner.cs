using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    [SerializeField] GameObject ghostPrefabs;
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
        levelManager = FindObjectOfType<LevelManager>();
    }

    void Update()
    {
        if (levelManager.HasLoadedLevel() && canSpawn)
        {
            SpawnGhosts(levelManager.GetActiveSceneIndex() - 1);
            canSpawn = false;
        }
    }

    void SpawnGhosts(int currentLevelIndex)
    {
        for (int i = 0; i < currentLevelIndex; i++)
        {
            GameObject instance = Instantiate(ghostPrefabs, playerPositions[i][0], Quaternion.identity);
            GhostMechanic ghost = instance.GetComponent<GhostMechanic>();

            ghost.SetPlayerPositions(playerPositions[i]);
            ghost.EnableMovement();
        }
    }

    public void SpawnNewLevel()
    {
        previousLevelIndex = levelManager.GetActiveSceneIndex() - 1;
        playerPositions[previousLevelIndex] = FindObjectOfType<PlayerController>().GetPlayerPositions();

        levelManager.LoadNextLevel();
        canSpawn = true;
    }
}
