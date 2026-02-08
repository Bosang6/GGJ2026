using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUIController : MonoBehaviour
{
    public List<GameObject> ScoreUI= new();
    
    public List<Sprite> Digits = new();

    void Start()
    {
        ShowScore(0);
    }

    public void ShowScore(int score)
    {
        //if(score > 999) score = 999;
        ScoreUI[2].GetComponent<UnityEngine.UI.Image>().sprite = Digits[score%10];
        ScoreUI[1].GetComponent<UnityEngine.UI.Image>().sprite = Digits[(score/10)%10];
        ScoreUI[0].GetComponent<UnityEngine.UI.Image>().sprite = Digits[(score/100)%10];
    }

}