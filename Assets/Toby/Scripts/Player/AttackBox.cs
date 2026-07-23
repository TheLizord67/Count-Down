using UnityEngine;

public class AttackBox : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") == true)
        {
            //particles
            Destroy(collision.gameObject, 0.1f);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") == true)
        {
            //particles
            Destroy(collision.gameObject, 0.1f);
        }
    }
}
