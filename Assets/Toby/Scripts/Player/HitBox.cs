using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        player.hits.Clear();
        if (collision.gameObject.CompareTag("Enemy") == true)
        {
            player.hits.Add(collision.gameObject);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        player.hits.Clear();
        if (collision.gameObject.CompareTag("Enemy") == true)
        {
            player.hits.Add(collision.gameObject);
        }
    }
}

