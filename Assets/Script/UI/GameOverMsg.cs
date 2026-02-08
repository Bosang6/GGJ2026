using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class GameOverMsg : MonoBehaviour
{
    public static GameOverMsg Instance;

    public GameObject msg;
    public GameObject longMsg;

    public List<Sprite> messages;

    public void Start()
    {
        int i = Random.Range(0, messages.Count + 1);
        if (i == messages.Count)
        {
            longMsg.SetActive(true);
            msg.SetActive(false);
        }
        else
        {
            longMsg.SetActive(false);
            msg.SetActive(true);
            msg.GetComponent<UnityEngine.UI.Image>().sprite = messages[i];
        }
    }
}
