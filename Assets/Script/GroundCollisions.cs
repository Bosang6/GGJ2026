using UnityEngine;

public class GroundCollisions : MonoBehaviour
{
    public GameObject poof_pref;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Mask"))
        {
            Instantiate(poof_pref, collision.GetContact(0).point, Quaternion.identity);
        }
    }
}
