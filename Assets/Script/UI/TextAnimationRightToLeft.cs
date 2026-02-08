using UnityEngine;
using System.Collections;

public class TextAnimationRightToLeft : MonoBehaviour
{
    public RectTransform target;      
    public float duration = 0.6f;     
    public float startOffsetX = 1200f; 

    Vector2 endPos;

    void OnEnable()
    {
        // 目标位置 = 当前在编辑器里摆好的位置
        endPos = target.anchoredPosition;

        // 起点：同样的Y，只把X挪到左边屏幕外
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
            t += Time.unscaledDeltaTime / time; // UI动画通常用unscaled，暂停时也能动
            float smooth = Mathf.SmoothStep(0, 1, t);
            target.anchoredPosition = Vector2.Lerp(from, to, smooth);
            yield return null;
        }

        target.anchoredPosition = to;
    }
}