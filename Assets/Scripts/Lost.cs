using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lost : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        var lastThree = Grab.Instance.onFloor.Skip(Grab.Instance.onFloor.Count <= 2 ? 0 : Grab.Instance.onFloor.Count - 2).ToList();
        if (lastThree.Select(rigid => rigid.gameObject).Contains(other.gameObject))
        {
            Grab.Instance.GameOver();
            Destroy(this);
        }
    }
}