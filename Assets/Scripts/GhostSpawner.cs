using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    [SerializeField] GameObject ghostPrefabs;
    [SerializeField] float delayGhostSpawn = 2f;
    Dictionary<int, List<Vector3>> playerPositions = new Dictionary<int, List<Vector3>>();
    Dictionary<int, List<bool>> playerRunning = new Dictionary<int, List<bool>>();
    Dictionary<int, List<bool>> playerDashing = new Dictionary<int, List<bool>>();
    Dictionary<int, List<Vector3>> playerScale = new Dictionary<int, List<Vector3>>();
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
            StartCoroutine(SpawnGhosts(levelManager.GetActiveSceneIndex() - 1));
            canSpawn = false;
        }
    }

    IEnumerator SpawnGhosts(int currentLevelIndex)
    {
        yield return new WaitForSeconds(delayGhostSpawn);
        
        for (int i = 0; i < currentLevelIndex; i++)
        {
            GameObject instance = Instantiate(ghostPrefabs, playerPositions[i][0], Quaternion.identity);
            GhostMechanic ghost = instance.GetComponent<GhostMechanic>();

            ghost.SetPlayerPositions(playerPositions[i]);
            ghost.SetPlayerRunning(playerRunning[i]);
            ghost.SetPlayerDashing(playerDashing[i]);
            ghost.SetPlayerScale(playerScale[i]);
            ghost.EnableMovement();
        }
    }

    public void SpawnNewLevel()
    {
        previousLevelIndex = levelManager.GetActiveSceneIndex() - 1;
        playerPositions[previousLevelIndex] = FindObjectOfType<PlayerController>().GetPlayerPositions();
        playerRunning[previousLevelIndex] = FindObjectOfType<PlayerController>().GetPlayerRunning();
        playerDashing[previousLevelIndex] = FindObjectOfType<PlayerController>().GetPlayerDashing();
        playerScale[previousLevelIndex] = FindObjectOfType<PlayerController>().GetPlayerScale();

        levelManager.LoadNextLevel();
        canSpawn = true;
    }
}
