using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameoverTrigger : MonoBehaviour
{
    public int BlocksToEmerge = 3;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var block = other.GetComponent<TotemBlock>();
        if (block)
        {
            if (block.totem == null)
            {
                GameManager.Instance.GameOver();
            }
            else
            {
                var reverseIndexInTotem = block.totem.totemBlocks.Count - block.indexInTotem - 1;
                if (reverseIndexInTotem < BlocksToEmerge)
                {
                    GameManager.Instance.GameOver();
                }
            }
        }
    }


}