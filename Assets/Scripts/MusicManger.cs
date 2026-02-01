using UnityEngine;

public class MusicManger : MonoBehaviour
{
    private AudioSource backgourndMusic;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        backgourndMusic = GetComponent<AudioSource>();
        backgourndMusic.loop = true;
        backgourndMusic.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
