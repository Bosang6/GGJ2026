using UnityEngine;

public class SlowBlock : MonoBehaviour
{
    private Rigidbody2D rb;

    public float damping = 0.4f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rb.bodyType == RigidbodyType2D.Static) return;

        rb.angularVelocity *= 1f - damping * Time.fixedDeltaTime;
        rb.linearVelocity *= 1f - damping * Time.fixedDeltaTime;

        if (rb.angularVelocity < 0.01f && rb.angularVelocity > -0.01f)
        {
            rb.angularVelocity = 0f;
        }
        if (rb.linearVelocity.magnitude < 0.01f)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}
