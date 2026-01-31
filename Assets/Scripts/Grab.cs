using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class MaskInfo
{
    public int index;
    public GameObject prefab;
    public Texture2D tex;
}

public class Grab : MonoBehaviour
{
    private static Grab instance;
    public static Grab Instance => instance;

    public Transform position;
    public GameObject squarePrefab;

    private GameObject cubeHold;

    private float moveSpeed = 2.0f;

    public Transform leftPoint;
    public Transform rightPoint;

    public Transform cameraPosition;

    private Vector3 targetPosition;

    [SerializeField]
    public MaskInfo[] Prefabs;

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
        if (Input.GetMouseButtonDown(0))
        {
            Rigidbody2D rb = cubeHold.GetComponent<Rigidbody2D>();
            rb.gravityScale = 1;
            cubeHold = null;
            //Invoke("GenerateNewTotem", 1);

            targetPosition = targetPosition + Vector3.up;
        }
        

        Vector3 p = Vector2.Lerp(cameraPosition.position, targetPosition, Time.deltaTime);
        p.z = -10;
        cameraPosition.position = p;

    }

    private void MoveHoldCube()
    {
        if (cubeHold)
        {
            cubeHold.transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            if (cubeHold.transform.position.x > rightPoint.position.x || cubeHold.transform.position.x < leftPoint.position.x)
            {
                moveSpeed = -moveSpeed;
            }            
        }
    }

    private void GenerateNewTotem()
    {
        cubeHold = Instantiate(squarePrefab, position);
    }

    public void Spawn(int index)
    {
        Debug.Log("Spawn totem index: " + index);
        // cubeHold = Instantiate(prefab, position);
    }
    
}
