using UnityEngine;
using System.Collections;

public class UI_SlideIn : MonoBehaviour
{
    public RectTransform target;      
    public float duration = 0.6f;     
    public float startOffsetX = -1200f; 

    Vector2 endPos;

    void OnEnable()
    {
        endPos = target.anchoredPosition;
        
        target.anchoredPosition = new Vector2(endPos.x + startOffsetX, endPos.y);

        StopAllCoroutines();
        StartCoroutine(MoveTo(endPos, duration));
    }

    IEnumerator MoveTo(Vector2 to, float time)
    {
        Vector2 from = target.anchoredPosition;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / time;
            float smooth = Mathf.SmoothStep(0, 1, t);
            target.anchoredPosition = Vector2.Lerp(from, to, smooth);
            yield return null;
        }

        target.anchoredPosition = to;
    }
}