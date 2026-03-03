using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;
    public bool cheatsEnabled = false;

    private int score = 0;
    public int Score
    {
        get => score;
        private set
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

    public GameoverTrigger gameoverBounds;

    public bool IsGameOver { get; private set; } = false;
    public UnityEvent OnGameOver;

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

        InputAction cheatAction = InputSystem.actions.FindAction("Cheat");
        cheatAction.performed += _ =>
        {
            cheatsEnabled = !cheatsEnabled;
            Debug.Log("Cheats " + (cheatsEnabled ? "enabled" : "disabled"));
        };
    }

    void Start()
    {
        var firstBlock = Instantiate(TotemBlocks_pref[0]);
        PlayerHand.GrabBlock(firstBlock);
    }

    private bool OnBlockSelected(TotemBlock block)
    {
        if (PlayerHand.holdedBlock) return false;
        PlayerHand.GrabBlock(Instantiate(block));
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

            var pos = PlayerHand.transform.position;
            pos.x = topBlock.transform.position.x;
            PlayerHand.transform.position = pos;

            var baseBlock = Totem.GetTop(gameoverBounds.BlocksToEmerge).First();
            pos = gameoverBounds.transform.position;
            pos.x = baseBlock.transform.position.x;
            gameoverBounds.transform.position = pos;
        }

        newTarget = Vector3.Max(targetTransform.position + new Vector3(0, .3f, 0), newTarget);
        newTarget.x = targetTransform.position.x;
        newTarget.z = targetTransform.position.z;
        targetTransform.position = newTarget;
    }

    public void AddScore(int amount)
    {
        if (IsGameOver) return;
        Score += amount;
    }

    public void GameOver()
    {
        IsGameOver = true;
        GameoverUI.gameObject.SetActive(true);

        OnGameOver?.Invoke();

        var newPosition = targetTransform.position + new Vector3(0f, -1.5f, 0f);
        targetTransform.position = Vector3.Max(initialTargetPosition, newPosition);
    }

    public void Restart() => SceneManager.LoadScene("GameScene");
    public void ToMenu() => SceneManager.LoadScene("BeginScene");
    public void QuitGame() => Application.Quit();
}
