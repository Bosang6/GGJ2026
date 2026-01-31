using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lost : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        for (int i = 0; i < Grab.Instance.onFloor.Count - 2; i++)
        {
            
        } 
        if (other.CompareTag("Mask"))
        {
            Grab.Instance.GameOver();
            Destroy(this);
        }
    }
}
