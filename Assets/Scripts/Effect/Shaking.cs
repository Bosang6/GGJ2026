using System.Collections;
using UnityEngine;

public class Shaking : MonoBehaviour
{


    private static Shaking instance;
    public static Shaking Instance => instance;

    public float duration = .5f;
    public AnimationCurve curve;
    public bool start = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            start = false;
            StartCoroutine(Shake());
        }    
    }

    IEnumerator Shake()
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime/duration);
            transform.position = startPosition + strength*Random.insideUnitSphere;
            yield return null;   
        }

        transform.position = startPosition;
    }
}