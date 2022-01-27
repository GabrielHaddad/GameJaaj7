using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [SerializeField] float fadeDelay = 1f;
    [SerializeField] TextMeshProUGUI deathText;
    [SerializeField] TextMeshProUGUI levelText;
    int deathCount = 0;
    CameraFade cameraFade;
    bool loadedLevel = false;

    static LevelManager instance;

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
        cameraFade = FindObjectOfType<CameraFade>();
    }

    void Update() 
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        int sceneCount = SceneManager.sceneCountInBuildSettings - 1;
        deathText.text = "DEATH COUNT - " + deathCount;
        levelText.text = "LEVEL - " + currentLevel + "/" + sceneCount;
    }

    public void AddToDeathCount()
    {
        deathCount++;
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public int GetActiveSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public bool HasLoadedLevel()
    {
        return loadedLevel;
    }

    IEnumerator LoadLevel(int sceneIndex)
    {
        loadedLevel = false;

        cameraFade.FadeIn();

        yield return new WaitForSeconds(fadeDelay);
        
        SceneManager.LoadScene(sceneIndex);

        cameraFade.FadeOut();

        yield return new WaitForSeconds(fadeDelay);

        loadedLevel = true;

    }
}
