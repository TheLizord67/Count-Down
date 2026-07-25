using UnityEngine;

public class EnemyHitBox : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") == true)
        {
                //particles
                if (collision.gameObject.GetComponent<PlayerMovement>().dashing == false)
                {
                    collision.gameObject.GetComponent<PlayerMovement>().sprite.Die();
                    collision.gameObject.GetComponent<PlayerMovement>().enabled = false;
                    collision.gameObject.GetComponent<CircleCollider2D>().enabled = false;
                    collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
                }
            //game over
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") == true)
        {
            //particles
            if (collision.gameObject.GetComponent<PlayerMovement>().dashing == false || collision.gameObject.GetComponent<PlayerMovement>().invincible == false)
            {
                if (collision.gameObject.GetComponent<PlayerMovement>().hp > 0)
                {
                    collision.gameObject.GetComponent<PlayerMovement>().hp -= 1;
                    collision.gameObject.GetComponent<PlayerMovement>().StartCoroutine(collision.gameObject.GetComponent<PlayerMovement>().IFrames());
                }
                else 
                { 
                    collision.gameObject.GetComponent<PlayerMovement>().sprite.Die();
                    collision.gameObject.GetComponent<PlayerMovement>().enabled = false;
                    collision.gameObject.GetComponent<CircleCollider2D>().enabled = false;
                    collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
                }
            }
            //game over
        }
    }
}
