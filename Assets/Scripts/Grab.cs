using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public enum E_EffectType
{
    Null,
    Wobble,
    PumpkinMask,
}

[System.Serializable]
public class MaskInfo
{
    public int index;
    public GameObject prefab;
    public int score;
    public E_EffectType type;
}

public class Grab : MonoBehaviour
{
    private static Grab instance;
    public static Grab Instance => instance;

    public GameObject totemRoot;
    public Transform position;
    public GameObject squarePrefab;

    public GameObject cubeHold { get; private set; }

    public float moveSpeed = 1.0f;

    public AnimationCurve moveCurve;
    public Transform leftPoint;
    public Transform rightPoint;

    public Transform cameraPosition;

    private Vector3 targetPosition;

    [SerializeField]
    public MaskInfo[] Prefabs;

    private int index = 0;

    public List<Rigidbody2D> onFloor = new List<Rigidbody2D>();
    

    void Start()
    {
        cubeHold = Instantiate(squarePrefab, position);

        targetPosition = cameraPosition.position;
    }

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        MoveHoldCube();
        if (Input.GetKeyDown(KeyCode.Space) && cubeHold != null)
        {
            cubeHold.transform.SetParent(totemRoot.transform);
            
            Rigidbody2D rb = cubeHold.GetComponent<Rigidbody2D>();
            
            // verifica se la lista è piano
            if (onFloor.Count < 5)
            {
                // Aggiunge una maschera
                onFloor.Add(rb);
            }
            else
            {
                var obj = onFloor.First();
                obj.bodyType = RigidbodyType2D.Kinematic;
                onFloor.RemoveAt(0);
                onFloor.Add(rb);
            }
            
            rb.gravityScale = 1;

            targetPosition = targetPosition + Vector3.up * 1.5f;
            
            cubeHold = null;
            
        }
        

        Vector3 p = Vector2.Lerp(cameraPosition.position, targetPosition, Time.deltaTime);
        p.z = -10;
        cameraPosition.position = p;

    }

    private void MoveHoldCube()
    {
        if (cubeHold)
        {
            var y = moveCurve.Evaluate(0.5f + Time.timeSinceLevelLoad * moveSpeed);
            var pos = Vector3.Lerp(leftPoint.position, rightPoint.position, y);
            cubeHold.transform.position = pos;
        }
    }

    public void Spawn(int index)
    {
        if (cubeHold) return;
        
        GameObject p = Prefabs[index].prefab;
        cubeHold = Instantiate(p, position);

        E_EffectType type = Prefabs[index].type;
        switch (type)
        {
            case E_EffectType.Wobble:
                PostProcessingControls.Instance.SetWobbleLevel(3);
                break;
            case E_EffectType.PumpkinMask:
                PostProcessingControls.Instance.SetPumpkinMaskLevel(3);
                break;
        }
    }

    public List<Canvas> CanvasList;
    public Canvas gameoverCanvas;

    public void GameOver()
    {
        foreach (var canvas in CanvasList)
        {
            canvas.gameObject.SetActive(false);
        }
        gameoverCanvas.gameObject.SetActive(true);
        Debug.Log("Game Over");
    }

    public void Restart()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void ToMenu()
    {
        SceneManager.LoadScene("BeginScene");
    }
}
