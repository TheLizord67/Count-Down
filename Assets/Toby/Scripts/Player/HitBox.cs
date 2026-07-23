using UnityEngine;

public class HitBox : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") == true)
        {
            player.DashToEnemy(collision.gameObject);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") == true)
        {
            player.DashToEnemy(collision.gameObject);
        }
    }
}
