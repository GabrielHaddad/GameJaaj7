using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [Header("Dash")]
    [SerializeField] AudioClip dashClip;
    [SerializeField] [Range(0f, 1f)] float dashVolume = 1f;

    [Header("Grappling Hook")]
    [SerializeField] AudioClip grapplingClip;
    [SerializeField] [Range(0f, 1f)] float grapplingVolume = 1f;

    [Header("Laser")]
    [SerializeField] AudioClip laserClip;
    [SerializeField] [Range(0f, 1f)] float laserVolume = 1f;

    static AudioPlayer instance;

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
    }

    public void PlayDashClip()
    {
        if (dashClip != null)
        {
            PlayClip(dashClip, dashVolume);
        }
    }

    public void PlayGrapplingClip()
    {
        if (grapplingClip != null)
        {
            PlayClip(grapplingClip, grapplingVolume);
        }
    }

    public void PlayLaserClip()
    {
        if (laserClip != null)
        {
            PlayClip(laserClip, laserVolume);
        }
    }

    private void PlayClip(AudioClip clip, float volume)
    {
        if (clip != null)
        {
            Vector3 cameraPos = Camera.main.transform.position;
            AudioSource.PlayClipAtPoint(clip, 
                cameraPos, volume);
        }
    }
}
