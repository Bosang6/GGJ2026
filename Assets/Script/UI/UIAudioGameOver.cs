using UnityEngine;

public class UIAudioGameOver : MonoBehaviour
{
    public AudioSource source;
    public AudioClip hoverClip;
    public AudioClip clickClip;
    
    public void PlayHover()
    {
        if (source != null && hoverClip != null)
            source.PlayOneShot(hoverClip);
    }

    public void PlayClick()
    {
        source.clip = clickClip;
        source.Play();
    }
}
