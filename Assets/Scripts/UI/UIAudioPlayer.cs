using System;
using UnityEngine;

public class UIAudioPlayer : MonoBehaviour
{
    public AudioSource source;
    public AudioClip hoverClip;
    public AudioClip clickClip;
    public AudioClip warningClip;

    private void Start()
    {
        
    }

    public void PlayHover()
    {
        if (source != null && hoverClip != null)
            source.PlayOneShot(hoverClip);
    }

    public void PlayClick()
    {
        if (!Grab.Instance.cubeHold)
        {
            source.clip = clickClip;
            source.Play();
        }
        else
        {
            PlayWarning();
        }
    }

    public void PlayWarning()
    {
        source.clip = warningClip;
        source.Play();
    }
}