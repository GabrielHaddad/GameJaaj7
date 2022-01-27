using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    UIAudio uIAudio;

    void Awake() 
    {
        uIAudio = FindObjectOfType<UIAudio>();
    }

    public void LoadGame()
    {
        uIAudio.PlayClickSoundClip();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        uIAudio.PlayClickSoundClip();
        Application.Quit();
    }
}
