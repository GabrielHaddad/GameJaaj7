using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAudio : MonoBehaviour
{
    [Header("Click")]
    [SerializeField] AudioClip clickClip;
    [SerializeField] [Range(0f, 1f)] float clickVolume = 1f;

    public void PlayClickSoundClip()
    {
        if (clickClip != null)
        {
            PlayClip(clickClip, clickVolume);
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
