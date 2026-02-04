using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public enum E_EffectType
{
    Null,
    Wobble,
    PumpkinMask,
    Wresler,
    Venice,
}

[System.Serializable]
public class MaskInfo
{
    public int index;
    public GameObject prefab;
    public int score;
    public E_EffectType type;
    public AudioClip[] sounds;
}

public class Grab : MonoBehaviour
{
    private static Grab instance;
    public static Grab Instance => instance;

    public GameObject totemRoot;
    public Transform position;
    public GameObject squarePrefab;

    public GameObject cubeHold { get; private set; }

    public float moveSpeed = 0.75f;

    public Transform leftPoint;
    public Transform rightPoint;

    public AnimationCurve moveCurve;
    
    public Transform cameraPosition;

    private Vector3 targetPosition;
    private Vector3 targetSpace;

    [SerializeField]
    public MaskInfo[] Prefabs;

    public List<Rigidbody2D> onFloor = new List<Rigidbody2D>();

    private E_EffectType currentEEffectType;

    public Canvas canvas;

    private BlockScript bs;

    private bool isGameOver = false;

    private Vector3 initCameraPosition;

    private AudioSource fallTower;

    private InputAction releaseAction;

    void Start()
    {
        Spawn(0);
        targetPosition = cameraPosition.position;
        targetSpace = targetPosition - totemRoot.transform.position;
        
        canvas.gameObject.SetActive(false);
        initCameraPosition = cameraPosition.position;

        fallTower = GetComponent<AudioSource>();
        releaseAction = InputSystem.actions.FindAction("Release");
    }

    private void Awake()
    {
        instance = this;
    }

    private float timer = 0;
    
    void Update()
    {
        // MoveHoldCube();
        if (isGameOver)
        {
            
        }
        else if (timer > 0)
        {
            timer -= Time.deltaTime;
        }    
        else if (releaseAction.IsPressed() && cubeHold != null)
        {
            timer = .5f;
            cubeHold.transform.SetParent(totemRoot.transform);
            
            Collider2D[] colliders = cubeHold.GetComponents<Collider2D>();
            foreach (var c in colliders)
            {
                c.enabled = true;
            }
            
            Rigidbody2D rb = cubeHold.GetComponent<Rigidbody2D>();

            bs.PlayFall();
            
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

            if (currentEEffectType == E_EffectType.Wresler)
                rb.gravityScale = 5.0f;
            else
            {
                rb.gravityScale = 1;
            }

            var upperTarget = targetPosition;
            if (onFloor.Count >= 2)
            {
                upperTarget = onFloor.Skip(onFloor.Count - 2).First().gameObject.transform.position + targetSpace;
            }

            var avgTarget = Vector3.Lerp(targetPosition, upperTarget, 0.65f);
            targetPosition.y = Math.Max(targetPosition.y, avgTarget.y + 0.3f);
            
            cubeHold = null;

            moveSpeed = 0.75f;
        }
        

        // Vector3 p = Vector2.Lerp(cameraPosition.position, targetPosition, Time.deltaTime);
        // p.z = -10;
        // cameraPosition.position = p;

    }

    private void FixedUpdate()
    {
        if (isGameOver) return;
        MoveHoldCube();
        Scrichiolamento();
    }

    private void LateUpdate()
    {
        Vector3 p = Vector2.Lerp(cameraPosition.position, targetPosition, Time.deltaTime);
        p.z = -10;
        cameraPosition.position = p;
    }

    private void MoveHoldCube()
    {
        if (cubeHold)
        {
            // cubeHold.transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            // if (cubeHold.transform.position.x > rightPoint.position.x || cubeHold.transform.position.x < leftPoint.position.x)
            // {
            //     moveSpeed = -moveSpeed;
            // }     
            if (cubeHold)
            {
                var y = moveCurve.Evaluate(0.5f + Time.timeSinceLevelLoad * moveSpeed);
                var pos = Vector3.Lerp(leftPoint.position, rightPoint.position, y);
                cubeHold.transform.position = pos;
            }
        }
    }

    public void Spawn(int index)
    {
        if (cubeHold) return;
        
        GameObject p = Prefabs[index].prefab;
        cubeHold = Instantiate(p, position);

        bs = cubeHold.GetComponent<BlockScript>();
        bs.maskInfo = Prefabs[index];

        if (Prefabs[index].type == E_EffectType.PumpkinMask || Prefabs[index].type == E_EffectType.Wobble)
        {
            bs.PlaySelected();
        }

        Collider2D[] colliders = cubeHold.GetComponents<Collider2D>();
        foreach (var c in colliders)
        {
            c.enabled = false;
        }

    }
    
    public List<Canvas> CanvasList;
    public Canvas gameoverCanvas;

    public void GameOver()
    {
        isGameOver = true;
        foreach (var canvas in CanvasList)
        {
            canvas.gameObject.SetActive(false);
        }

        if (cubeHold)
        {
            Destroy(cubeHold);
            cubeHold = null;
        }
        gameoverCanvas.gameObject.SetActive(true);

        targetPosition.y -= 2.0f;

        targetPosition.y = Math.Max(initCameraPosition.y, targetPosition.y);
    }

    public void Restart()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void ToMenu()
    {
        SceneManager.LoadScene("BeginScene");
    }
    
    private int crickCount = 0;
    private Vector3? last24offset = null;
    private void Scrichiolamento()
    {

        var secondMask = onFloor.Reverse<Rigidbody2D>().Skip(1).FirstOrDefault();
        var fourthMask = onFloor.Reverse<Rigidbody2D>().Skip(3).FirstOrDefault();
        if (fourthMask != null)
        {
            var offset24 = secondMask.transform.position - fourthMask.transform.position;
            if (last24offset == null)
            {
                last24offset = offset24;
                return;
            }
            float angularChange = Math.Abs(Vector3.Angle(offset24, last24offset.Value));
            if (angularChange > 0.021f)
            {
                crickCount += 1;
                if (crickCount >= 15)
                {
                    fallTower.Play();
                    crickCount = 0;
                }

            }
            else
            {
                crickCount = 0;
            }
            last24offset = offset24;
        }
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }

}
