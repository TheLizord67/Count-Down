using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.ParticleSystem;
public enum States
{
    Following,
    Running,
    Attacking
}

public enum AttackStyles
{
    Forward,
    Left,
    Right,
    Direct
}

public class EnemyController : MonoBehaviour
{
    [SerializeField] public Transform target;

    [SerializeField] public PlayerMovement player;

    [SerializeField] public float speed, slerp, distanceToAttack, timeToAttack, run, distanceToDespawn;

    [SerializeField] public float nextWaypointDistance = 3f;

    [SerializeField] private GameObject exclamationPoint, hitBox, blood;

    [SerializeField] public List<GameObject> retreatPoints;

    [SerializeField] public SpriteCharacterControl sprite;
    private Path path;

    private int currentWaypoint = 0;

    private bool reachedEndOfPath = false;

    public Seeker seeker;
    private Rigidbody2D rb;
    private GameObject tempObject;

    private AttackStyles[] attackStylesList = { AttackStyles.Direct, AttackStyles.Forward, AttackStyles.Left, AttackStyles.Right };

    [SerializeField] private States state;

    [SerializeField] private AttackStyles attackStyle;

    [SerializeField] public GameObject chomp, torch, warning;

    [SerializeField] public bool ranged;

    public AudioSource attack;

    public bool doomed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnDestroy()
    {
        KillCount.enemiesSpawnedGlobal -= 1;
    }
    void Start()
    {
        doomed = false;
        KillCount.enemiesSpawnedGlobal += 1;
        speed = speed + KillCount.speedIncrease;
        attackStyle = attackStylesList[Random.Range(0, attackStylesList.Length)];
        GameObject[] points = GameObject.FindGameObjectsWithTag("Retreat");
        foreach (var point in points)
        {
            retreatPoints.Add(point);
        }
        seeker = GetComponent<Seeker>();   
        rb = GetComponent<Rigidbody2D>();
        player = FindAnyObjectByType<PlayerMovement>();
        if (player.currentForm == Forms.Chicken)
        {
            ChooseTarget();
        }
        else
        {
            FindRetreat();
        }
        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    void UpdatePath()
    {

        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    // Update is called once per frame
    void Update()
    {
        float playerDistance = Vector2.Distance(rb.position, player.gameObject.transform.position);
        //if (playerDistance >= distanceToDespawn)
        //{
            //Destroy(gameObject, 1f);
        //}
        if (player.currentForm == Forms.Chicken && state != States.Attacking)
        {
            state = States.Following;
        }
        if (player.currentForm == Forms.Vampire)
        {
            state = States.Running;
        }
        if (state == States.Following)
        {
            if (player.isActiveAndEnabled == false)
            {
                state = States.Running;
            }
            else
            {
                Following();
            }
        }
        if (state == States.Running)
        {
            Running();
        }
    }

    public void Following()
    {
        ChooseTarget();
        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;

        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, direction * speed, Time.deltaTime * slerp);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        float targetDistance = Vector2.Distance(rb.position, player.gameObject.transform.position);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (targetDistance <= distanceToAttack)
        {
            state = States.Attacking;
            StartCoroutine(Attack());
        }
    }
    public IEnumerator Attack()
    {
        if (!ranged)
        {
            attack.Play();
            sprite.Attack(timeToAttack);
            rb.linearVelocity = Vector2.zero;
            exclamationPoint.SetActive(true);
            yield return new WaitForSeconds(timeToAttack);
            exclamationPoint.SetActive(false);
            hitBox.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            hitBox.SetActive(false);
            if (player.currentForm == Forms.Chicken)
            {
                state = States.Following;
            }
        }
        else
        {
            attack.Play();
            sprite.Attack(timeToAttack);
            rb.linearVelocity = Vector2.zero;
            exclamationPoint.SetActive(true);
            yield return new WaitForSeconds(timeToAttack);
            sprite.weapon.SetActive(false);
            exclamationPoint.SetActive(false);
            Throw();
            yield return new WaitForSeconds(3f);
            sprite.weapon.SetActive(true);
            if (player.currentForm == Forms.Chicken)
            {
                state = States.Following;
            }
        }
    }

    public void Throw()
    {
        Transform target = player.transform;
        GameObject warning_ = Instantiate(warning, target.position, Quaternion.identity);
        GameObject torchThrown = Instantiate(torch, this.transform.position, Quaternion.identity);
        torchThrown.GetComponent<Torch>().Thrown(warning_.transform);
        torchThrown.GetComponent<Torch>().isThrown = true;
    }
    public void Running()
    {
        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;

        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, direction * (speed + 2), Time.deltaTime * slerp);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        float targetDistance = Vector2.Distance(rb.position, target.position);

        float playerDistance = Vector2.Distance(rb.position, player.gameObject.transform.position);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (targetDistance <= distanceToAttack)
        {
            FindRetreat();
        }
        
        if (playerDistance <= run)
        {
            FindSecondRetreat();
        }
    }
    public void FindRetreat()
    {
        retreatPoints.Clear();
        GameObject[] points = GameObject.FindGameObjectsWithTag("Retreat");
        foreach (var point in points)
        {
            retreatPoints.Add(point);
        }
        retreatPoints = (List<GameObject>)retreatPoints.Shuffle();
        target = retreatPoints[0].transform;
        state = States.Running;
    }
    public void FindSecondRetreat()
    {
        List<float> distances = new List<float>();
        foreach (var point in retreatPoints)
        {
            float distance = Vector2.Distance(player.transform.position, point.transform.position);
            distances.Add(distance);
        }
        distances.Sort();
        foreach (var point in retreatPoints)
        {
            float distance = Vector2.Distance(rb.position, point.transform.position);
            if (distance == distances.IndexOf(distances.Count - 1))
            {
                target = point.transform;
            }
        }
        state = States.Running;
    }
    public void ChooseTarget()
    {
        if (attackStyle == AttackStyles.Direct)
        target = player.transform;

        if (attackStyle == AttackStyles.Forward)
        {
            target = player.transform;
            //tempObject = new GameObject();
        }
            //check for wall and don't pathfind into the middle of a wall or beyond a wall just because player moves towards wall
            //RaycastHit hitInfo;

            //if (Physics.Raycast(player.transform.position, player.GetComponent<Rigidbody>().linearVelocity.normalized, out hitInfo, Mathf.Abs((transform.position - player.transform.position).magnitude), LayerMask.GetMask("Obstacle")))
            //{
                //tempObject.transform.position = player.transform.position + player.GetComponent<Rigidbody>().linearVelocity.normalized * hitInfo.distance/2;
            //}
            //else
            //{
                //tempObject.transform.position = player.transform.position + player.GetComponent<Rigidbody>().linearVelocity.normalized * Mathf.Abs((transform.position - player.transform.position).magnitude)/2;

                //target = tempObject.transform;
            //}   

        //}
        
        if (attackStyle == AttackStyles.Left)
        {
            target = player.transform;
        }
        
        if (attackStyle == AttackStyles.Right)
        {
            target = player.transform;
        }
    }
    public IEnumerator EnemyParts()
    {
        blood.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        blood.SetActive(false);
    }
}
