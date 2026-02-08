using System;
using UnityEngine;

public class UIAudioPlayer : MonoBehaviour
{
    public AudioSource source;
    public AudioClip hoverClip;
    public AudioClip clickClip;
    public AudioClip warningClip;

    public void PlayHover()
    {
        source.PlayOneShot(hoverClip);
    }

    public void PlayClick()
    {
        source.clip = clickClip;
        source.Play();
    }

    public void PlayWarning()
    {
        source.clip = warningClip;
        source.Play();
    }
}