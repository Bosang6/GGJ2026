using UnityEngine;

public class Bird : MonoBehaviour
{
    public Vector2 InitialVelocity = new(-5f, 0f);
    public GameObject poof_pref;

    private bool hasCollided = false;
    
    private void Start()
    {
        GetComponent<Rigidbody2D>().linearVelocity = InitialVelocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GetComponent<Rigidbody2D>().gravityScale = 1.5f;
        if (!hasCollided)
        {
            GetComponent<AudioSource>().Play();
            hasCollided = true;
        }
        GetComponent<Animator>().SetBool("isStunned", true);
        
        var block = collision.collider.GetComponent<TotemBlock>();
        if (block && block.totem == null)
        {
            Instantiate(poof_pref, transform.position, Quaternion.identity);
            Destroy(gameObject, 0.1f);
            foreach (var col in GetComponents<Collider2D>())
            {
                col.enabled = false;
            }
        }
    }
}
