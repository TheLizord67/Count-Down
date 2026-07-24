using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RoomManager : MonoBehaviour
{
    [SerializeField] public List<RoomManager> connectedRooms;

    [SerializeField] public List<Transform> enemySpawns;

    [SerializeField] public Fusebox fuseBox;

    [SerializeField] private PlayerMovement player;

    [SerializeField] private bool spawning;

    [SerializeField] private GameObject enemy;

    [SerializeField] private float spawnSpeed;

    [SerializeField] public int maxEnemiesToSpawn, minEnemiesToSpawn, enemiesToSpawn, spawnLimit, amountSpawned;
    void Start()
    {
        player = FindAnyObjectByType<PlayerMovement>();
        InvokeRepeating("Spawn", 0f, spawnSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.currentRoom == this)
        {
            spawning = true;
        }
        else
        {
            spawning = false;
        }
    }

    public void Spawn()
    {
        if (spawning)
        {
            enemiesToSpawn = Random.Range(minEnemiesToSpawn, maxEnemiesToSpawn);
            for (int i = 0; i <= enemiesToSpawn; i++)
            {
                if (amountSpawned < spawnLimit)
                {
                    amountSpawned++;
                    enemySpawns = (List<Transform>)enemySpawns.Shuffle();
                    Transform spawn = enemySpawns[0];
                    Instantiate(enemy, spawn.position, Quaternion.identity);
                }
            }
        }
    }

    public void InitalSpawn()
    {
        enemiesToSpawn = Random.Range(minEnemiesToSpawn + 1, maxEnemiesToSpawn + 1);
        for (int i = 0; i <= enemiesToSpawn; i++)
        {
            if (amountSpawned < spawnLimit)
            {
                amountSpawned++;
                enemySpawns = (List<Transform>)enemySpawns.Shuffle();
                Transform spawn = enemySpawns[0];
                Instantiate(enemy, spawn.position, Quaternion.identity);
            }
        }
    }
    public void SwitchRooms()
    {
        spawning = false;
        connectedRooms = (List<RoomManager>)connectedRooms.Shuffle();
        connectedRooms[0].fuseBox.off = true;
        player.fuseBoxOn = connectedRooms[0].fuseBox.transform;
    }
}
