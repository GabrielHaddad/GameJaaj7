using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    bool isPaused = false;

    static PauseMenu instance;
    LevelManager levelManager;
    Canvas pauseCanvas;

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
        pauseCanvas = GetComponent<Canvas>();
        levelManager = FindObjectOfType<LevelManager>();
    }

    public void LoadMainMenu()
    {
        pauseCanvas.enabled = false;
        Time.timeScale = 1;
        isPaused = false;
        levelManager.LoadMainMenu();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused) 
        {
            pauseCanvas.enabled = true;
            Time.timeScale = 0;
            isPaused = true;
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && isPaused)
        {
            pauseCanvas.enabled = false;
            Time.timeScale = 1;
            isPaused = false;
        }
    }
}
