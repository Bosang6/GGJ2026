using UnityEngine;

public class Bird : MonoBehaviour
{
    public Vector2 InitialVelocity = new(-5f, 0f);

    private bool firstHit = true;
    
    private void Start()
    {
        GetComponent<Rigidbody2D>().linearVelocity = InitialVelocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GetComponent<Rigidbody2D>().gravityScale = 1.5f;
        Debug.Log("hi");
        if (firstHit)
        {
            GetComponent<AudioSource>().Play();
            firstHit = false;
        }
        GetComponent<Animator>().SetBool("hasCollided",true);
    }
}
