using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AttackBox : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") == true)
        {
            //particles
            player.mainCam.GetComponent<CameraFollow>().kill = true;
            collision.gameObject.GetComponent<EnemyController>().sprite.Die();
            collision.gameObject.GetComponent<EnemyController>().enabled = false;
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") == true)
        {
            //particles
            player.mainCam.GetComponent<CameraFollow>().kill = true;
            collision.gameObject.GetComponent<EnemyController>().sprite.Die();
            collision.gameObject.GetComponent<EnemyController>().enabled = false;
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        }
    }
}
