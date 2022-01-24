using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject ghostPrefab;
    [SerializeField] float fadeDelay = 1f;
    Dictionary<int, List<Vector3>> playerPositions = new Dictionary<int, List<Vector3>>();
    int currentLevelIndex = 0;
    PlayerController playerController;
    GhostMechanic ghostMechanic;
    CameraFade cameraFade;

    void Awake() 
    {
        playerController = FindObjectOfType<PlayerController>();
        ghostMechanic = FindObjectOfType<GhostMechanic>();
        cameraFade = FindObjectOfType<CameraFade>();
    }

    void Start() 
    {
        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public List<Vector3> GetPositions()
    {
        return playerPositions[currentLevelIndex];
    }

    public void ProcessPlayerDeath()
    {
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        cameraFade.FadeIn();

        yield return new WaitForSeconds(fadeDelay);
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        cameraFade.FadeOut();

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
