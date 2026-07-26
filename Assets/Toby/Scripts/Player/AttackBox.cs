using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

public class AttackBox : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    [SerializeField] private ScreenShake screenShake;
    [SerializeField] public float duration, magnitude;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        //screenShake = FindAnyObjectByType<ScreenShake>();
        if (collision.gameObject.CompareTag("Enemy") == true)
        {
            //particles
            collision.gameObject.GetComponent<EnemyController>().StartCoroutine(collision.gameObject.GetComponent<EnemyController>().EnemyParts());
            KillCount.enemiesSpawnedGlobal -= 1;
            player.currentRoom.amountSpawned -= 1;
            if (player.currentRoom.amountSpawned < 0)
            {
                player.currentRoom.amountSpawned = 0;
                player.currentRoom.InitalSpawn();
            }
            player.mainCam.GetComponent<CameraFollow>().kill = true;
            if (!collision.GetComponent<EnemyController>().doomed) { 
                KillCount.kills += 1;
                KillCount kill = FindAnyObjectByType<KillCount>();
                kill.UpdateAllRooms();
            }
            //screenShake.Shake(duration, magnitude);
            collision.gameObject.GetComponent<EnemyController>().seeker.enabled = false;
            collision.gameObject.GetComponent<EnemyController>().sprite.Die();
            collision.gameObject.GetComponent<EnemyController>().enabled = false;
            collision.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") == true)
        {
            KillCount.enemiesSpawnedGlobal -= 1;
            //particles
            player.mainCam.GetComponent<CameraFollow>().kill = true;
            KillCount.kills += 1;
            KillCount kill = FindAnyObjectByType<KillCount>();
            kill.UpdateAllRooms();
            collision.gameObject.GetComponent<EnemyController>().seeker.enabled = false;
            collision.gameObject.GetComponent<EnemyController>().sprite.Die();
            collision.gameObject.GetComponent<EnemyController>().enabled = false;
            collision.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        }
    }
}
