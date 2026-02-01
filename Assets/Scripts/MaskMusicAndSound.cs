using System;
using UnityEngine;

public class MaskMusicAndSound : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("hi");
        if (other.gameObject.CompareTag("Mask"))
        {
            Debug.Log("i");
        }
    }

}
