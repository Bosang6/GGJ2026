using System;
using UnityEngine;
using UnityEngine.Events;

public class SoundManager : MonoBehaviour
{
    public AudioSource fallSound;
    public AudioClip fall;

    private void Awake()
    {
        
    }

    void Start()
    {
        fallSound = GetComponent<AudioSource>();
    }
    

    public void PlayFallSound()
    {
        fallSound.Play();
    }
    
    
}
