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

    [SerializeField] private List<GameObject> enemy;

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
           //spawning = false;
           amountSpawned = 0;
        }
    }

    public void Spawn()
    {
        if (spawning && KillCount.enemiesSpawnedGlobal < 200)
        {
            enemiesToSpawn = Random.Range(minEnemiesToSpawn, maxEnemiesToSpawn);
            for (int i = 0; i <= enemiesToSpawn; i++)
            {
                if (amountSpawned < spawnLimit)
                {
                    amountSpawned++;
                    enemySpawns = (List<Transform>)enemySpawns.Shuffle();
                    Transform spawn = enemySpawns[0];
                    int loops = 0;

                    while ((spawn.position - player.transform.position).magnitude < 20 && loops < 10)
                    {
                        enemySpawns = (List<Transform>)enemySpawns.Shuffle();
                        spawn = enemySpawns[0];
                        loops++;

                        Debug.Log(loops);
                        Debug.Log(enemySpawns[0]);
                    }
                    if (loops < 10)
                        enemy = (List<GameObject>)enemy.Shuffle();
                        Instantiate(enemy[0], spawn.position, Quaternion.identity);
                }
            }
        }
    }
    public void InitalSpawn()
    {
        if (KillCount.enemiesSpawnedGlobal < 200)
        {
            enemiesToSpawn = Random.Range(minEnemiesToSpawn + 1, maxEnemiesToSpawn + 1);
            for (int i = 0; i <= enemiesToSpawn; i++)
            {
                if (amountSpawned < spawnLimit)
                {
                    amountSpawned++;
                    enemySpawns = (List<Transform>)enemySpawns.Shuffle();
                    Transform spawn = enemySpawns[0];
                    int loops = 0;

                    while ((spawn.position - player.transform.position).magnitude < 20 && loops < 10)
                    {
                        enemySpawns = (List<Transform>)enemySpawns.Shuffle();
                        spawn = enemySpawns[0];
                        loops++;
                        Debug.Log(loops);
                        Debug.Log(enemySpawns[0]);
                    }

                    if (loops < 10)
                        enemy = (List<GameObject>)enemy.Shuffle();
                        Instantiate(enemy[0], spawn.position, Quaternion.identity);
                }
            }
        }
    }
    public void AdjustSettings()
    {
        Debug.Log(KillCount.kills);
        if (KillCount.kills == 20)
        {
            minEnemiesToSpawn += 1;
            maxEnemiesToSpawn += 1;
        }
        if (KillCount.kills == 40)
        {
            minEnemiesToSpawn += 1;
            maxEnemiesToSpawn += 1;
            KillCount.speedIncrease += 0.5f;
        }
        if (KillCount.kills == 80)
        {
            minEnemiesToSpawn += 1;
            maxEnemiesToSpawn += 1;
        }
        if (KillCount.kills == 100)
        {
            minEnemiesToSpawn += 2;
            maxEnemiesToSpawn += 2;
            spawnLimit += 5;
        }
        if (KillCount.kills == 120)
        {
            minEnemiesToSpawn += 2;
            maxEnemiesToSpawn += 2;
        }
        if (KillCount.kills == 140)
        {
            minEnemiesToSpawn += 2;
            maxEnemiesToSpawn += 2;
            KillCount.speedIncrease += 1f;
        }
        if (KillCount.kills == 180)
        {
            minEnemiesToSpawn += 2;
            maxEnemiesToSpawn += 2;
        }
        if (KillCount.kills == 200)
        {
            minEnemiesToSpawn += 3;
            maxEnemiesToSpawn += 3;
            spawnLimit += 5;
        }
        if (KillCount.kills == 220)
        {
            minEnemiesToSpawn += 3;
            maxEnemiesToSpawn += 3;
        }
        if (KillCount.kills == 240)
        {
            minEnemiesToSpawn += 3;
            maxEnemiesToSpawn += 3;
            KillCount.speedIncrease += 1.5f;
        }
        if (KillCount.kills == 280)
        {
            minEnemiesToSpawn += 3;
            maxEnemiesToSpawn += 3;
        }
        if (KillCount.kills == 300)
        {
            minEnemiesToSpawn += 4;
            maxEnemiesToSpawn += 4;
            spawnLimit += 5;
        }
        if (KillCount.kills == 320)
        {
            minEnemiesToSpawn += 4;
            maxEnemiesToSpawn += 4;
        }
        if (KillCount.kills == 340)
        {
            minEnemiesToSpawn += 4;
            maxEnemiesToSpawn += 4;
            KillCount.speedIncrease += 2f;
        }
        if (KillCount.kills == 380)
        {
            minEnemiesToSpawn += 4;
            maxEnemiesToSpawn += 4;
        }
        if (KillCount.kills == 400)
        {
            minEnemiesToSpawn += 5;
            maxEnemiesToSpawn += 5;
            spawnLimit += 5;
        }
        if (KillCount.kills == 420)
        {
            minEnemiesToSpawn += 5;
            maxEnemiesToSpawn += 5;
        }
        if (KillCount.kills == 440)
        {
            minEnemiesToSpawn += 5;
            maxEnemiesToSpawn += 5;
            KillCount.speedIncrease += 2.5f;
        }
        if (KillCount.kills == 480)
        {
            minEnemiesToSpawn += 5;
            maxEnemiesToSpawn += 5;
        }
        if (KillCount.kills == 500)
        {
            minEnemiesToSpawn += 6;
            maxEnemiesToSpawn += 6;
            spawnLimit += 5;
        }
        if (KillCount.kills > 500)
        {
            spawnLimit += 1;
            KillCount.speedIncrease += 0.1f;
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
