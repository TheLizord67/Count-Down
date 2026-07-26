using UnityEngine;
using UnityEngine.Rendering;

public class EnemyHitBox : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") == true)
        {
            if (collision.gameObject.GetComponent<PlayerMovement>().dashing == false && collision.gameObject.GetComponent<PlayerMovement>().invincible == false)
            {
                //particles
                if (collision.gameObject.GetComponent<PlayerMovement>().hp > 0)
                {
                    collision.gameObject.GetComponent<PlayerMovement>().Hurt();
                    collision.gameObject.GetComponent<PlayerMovement>().StartCoroutine(collision.gameObject.GetComponent<PlayerMovement>().PlayerParticles(collision.gameObject.GetComponent<PlayerMovement>().blood));
                }
                else
                {
                    collision.gameObject.GetComponent<PlayerMovement>().StartCoroutine(collision.gameObject.GetComponent<PlayerMovement>().PlayerParticles(collision.gameObject.GetComponent<PlayerMovement>().blood));
                    collision.gameObject.GetComponent<PlayerMovement>().sprite.Die();
                    collision.gameObject.GetComponent<PlayerMovement>().dead = true;
                    collision.gameObject.GetComponent<PlayerMovement>().enabled = false;
                    GameOver game = FindAnyObjectByType<GameOver>();
                    game.StartCoroutine(game.OverOver());
                    collision.gameObject.GetComponent<CircleCollider2D>().enabled = false;
                    collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
                }
                //game over
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") == true)
        {
            if (collision.gameObject.GetComponent<PlayerMovement>().dashing == false && collision.gameObject.GetComponent<PlayerMovement>().invincible == false)
            {
                //particles
                if (collision.gameObject.GetComponent<PlayerMovement>().hp > 0)
                {
                    collision.gameObject.GetComponent<PlayerMovement>().hp -= 1;
                    collision.gameObject.GetComponent<PlayerMovement>().StartCoroutine(collision.gameObject.GetComponent<PlayerMovement>().IFrames());
                }
                else 
                { 
                    //particles
                    collision.gameObject.GetComponent<PlayerMovement>().sprite.Die();
                    collision.gameObject.GetComponent<PlayerMovement>().dead = true;
                    collision.gameObject.GetComponent<PlayerMovement>().enabled = false;
                    collision.gameObject.GetComponent<CircleCollider2D>().enabled = false;
                    collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
                }
            }
            //game over
        }
    }
}
