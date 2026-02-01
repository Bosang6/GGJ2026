using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    public static BirdSpawner Instance;

    public GameObject BirdPref;

    public float spawnInterval = 5f;
    public float IntervalSpawnChance = .1f;
    public float PLayerMoveSpawnChance = .5f;
    public List<Transform> SpawnPoints;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        InvokeRepeating(nameof(OnIntervalSpawnChance), spawnInterval, spawnInterval);
    }

    public void OnPlayerMoveSpawnChance()
    {
        if (Random.value < PLayerMoveSpawnChance)
        {
            SpawnBird();
        }
    }
    private void OnIntervalSpawnChance()
    {
        if (Random.value < IntervalSpawnChance)
        {
            SpawnBird();
        }
    }

    private void SpawnBird()
    {
        var bird = Instantiate(BirdPref, transform);

        int randomIndex = Random.Range(0, SpawnPoints.Count);
        bird.transform.position = SpawnPoints[randomIndex].position + 1f * (Vector3)Random.insideUnitCircle;
        var velocityX = Random.Range(2f, 5f);
        var birdComp = bird.GetComponent<Bird>();
        if (bird.transform.position.x > Camera.main.transform.position.x)
        {
            birdComp.InitialVelocity.x = -velocityX;
        }
        else
        {
            birdComp.InitialVelocity.x = velocityX;
            bird.GetComponent<SpriteRenderer>().flipX = true;
        }
    }
}
