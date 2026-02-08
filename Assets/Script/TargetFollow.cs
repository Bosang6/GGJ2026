using TMPro;
using UnityEngine;

public class TargetFollow : MonoBehaviour
{
    public Transform target;

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = Vector2.Lerp(transform.position, target.position, Time.deltaTime);
    }
}
