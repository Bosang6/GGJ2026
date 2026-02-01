using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    private static ScoreController instance;
    public static ScoreController Instance => instance;
    

    public List<GameObject> ScoreUI= new();

    public List<Sprite> Digits = new();


    public int currentScore = 0;

    void Awake()
    {
        instance = this;    
    }

    private void Start()
    {
        ShowScore();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public int GetScore()
    {
        return currentScore;
    }
    public void AddToScore(int dscore)
    {
        currentScore += dscore;
        ShowScore();
    }

    public void SetScore(int score)
    {
        currentScore = score;
        ShowScore();
    }

    private void ShowScore()
    {
        int score = currentScore;
        if(currentScore>=1000)
            score = 999;
        ScoreUI[2].GetComponent<UnityEngine.UI.Image>().sprite = Digits[score%10];
        ScoreUI[1].GetComponent<UnityEngine.UI.Image>().sprite = Digits[(score/10)%10];
        ScoreUI[0].GetComponent<UnityEngine.UI.Image>().sprite = Digits[(score/100)%10];
    }

}