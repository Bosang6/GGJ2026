using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    private int score = 0;
    public int Score
    {
        get => score;
        set
        {
            score = value;
            ScoreUIController.ShowScore(score);
        }
    }

    public TotemBlock[] TotemBlocks_pref;

    public Totem Totem;
    public PlayerHand PlayerHand;

    public Transform playerTransform;
    public Transform targetTransform;
    private float targetSpace;
    private Vector3 initialTargetPosition;

    public bool IsGameOver { get; private set; } = false;

    public TotemBlockSelector TotemBlockSelector;
    public ScoreUIController ScoreUIController;
    public Canvas GameoverUI;

    private void Awake()
    {
        instance = this;
        TotemBlockSelector.OnSelectBlock = OnBlockSelected;
        PlayerHand.OnRelease += OnBlockRelease;
        
        targetTransform.position = playerTransform.position;
        initialTargetPosition = playerTransform.position;
        targetSpace = playerTransform.position.y - Totem.baseBlock.transform.position.y;
    }

    void Start()
    {
        var firstBlock = Instantiate(TotemBlocks_pref[0], PlayerHand.transform.position, default);
        PlayerHand.GrabBlock(firstBlock);
    }

    private bool OnBlockSelected(TotemBlock block)
    {
        if (PlayerHand.holdedBlock) return false;
        PlayerHand.GrabBlock(Instantiate(block, PlayerHand.transform.position, default));
        BirdSpawner.Instance.OnPlayerMoveSpawnChance();
        return true;
    }

    private void OnBlockRelease(TotemBlock block)
    {
        var newTarget = targetTransform.position;
        var topBlock = Totem.totemBlocks.LastOrDefault();
        if (topBlock)
        {
            newTarget = topBlock.transform.position + new Vector3(0, targetSpace, 0);
        }

        newTarget = Vector3.Max(targetTransform.position + new Vector3(0, .3f, 0), newTarget);
        newTarget.x = targetTransform.position.x;
        newTarget.z = targetTransform.position.z;
        targetTransform.position = newTarget;
    }

    public void GameOver()
    {
        IsGameOver = true;
        
        Destroy(PlayerHand);
        Destroy(TotemBlockSelector);
        GameoverUI.gameObject.SetActive(true);

        var newPosition = targetTransform.position + new Vector3(0f, -1.5f, 0f);
        targetTransform.position = Vector3.Max(initialTargetPosition, newPosition);
    }

    public void Restart() => SceneManager.LoadScene("GameScene");
    public void ToMenu() => SceneManager.LoadScene("BeginScene");
    public void QuitGame() => Application.Quit();
}
