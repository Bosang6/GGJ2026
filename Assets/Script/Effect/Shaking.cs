using System.Collections;
using UnityEngine;

public class Shaking : MonoBehaviour
{


    private static Shaking instance;
    public static Shaking Instance => instance;

    public float duration = .5f;
    public AnimationCurve strenghtCurve;
    public float strenghtMultiplier = 1f;
    public AnimationCurve offsetCurve;
    public float offsetMultiplier = 1f;
    public Vector2 maxCamOffset = new(.5f, 2f);
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
        float elapsedTime = 0f;

        Vector2 lastShake = new();
        float lastOffset = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float offset = offsetMultiplier * offsetCurve.Evaluate(elapsedTime / duration);
            float strength = strenghtMultiplier * strenghtCurve.Evaluate(elapsedTime / duration);
            Vector2 shake = strength * Random.insideUnitCircle;
            transform.position += (Vector3) (shake - lastShake + new Vector2(0, offset - lastOffset));
            transform.localPosition = Vector3.Min(transform.localPosition, (Vector3) maxCamOffset);
            transform.localPosition = Vector3.Max(transform.localPosition, -(Vector3) maxCamOffset);
            lastOffset = offset;
            lastShake = shake;
            yield return null;   
        }
    }
}