using UnityEngine;

public class ObjectsDestroyer : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.attachedRigidbody.gameObject);
    }
}
