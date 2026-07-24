using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
public enum States
{
    Following,
    Running,
    Attacking
}
public class EnemyController : MonoBehaviour
{
    [SerializeField] public Transform target;

    [SerializeField] public PlayerMovement player;

    [SerializeField] public float speed, slerp, distanceToAttack, timeToAttack;

    [SerializeField] public float nextWaypointDistance = 3f;

    [SerializeField] private GameObject exclamationPoint, hitBox;

    [SerializeField] public List<GameObject> retreatPoints;

    [SerializeField] public SpriteCharacterControl sprite;
    private Path path;

    private int currentWaypoint = 0;

    private bool reachedEndOfPath = false;

    private Seeker seeker;
    private Rigidbody2D rb;

    [SerializeField] private States state;

    [SerializeField] public GameObject chomp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
            target = player.gameObject.transform;
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
        if (player.currentForm == Forms.Chicken && state != States.Attacking)
        {
            state = States.Following;
            target = player.gameObject.transform;
        }
        if (player.currentForm == Forms.Vampire)
        {
            state = States.Running;
        }
        if (state == States.Following)
        {
            Following();
        }
        if (state == States.Running)
        {
            Running();
        }
    }

    public void Following()
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
        rb.linearVelocity = Vector2.zero;
        exclamationPoint.SetActive(true);
        yield return new WaitForSeconds(timeToAttack);
        exclamationPoint.SetActive(false);
        hitBox.SetActive(true);
        yield return new WaitForSeconds(1f);
        hitBox.SetActive(false);
        if (player.currentForm == Forms.Chicken)
        {
            state = States.Following;
        }
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

        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, direction * speed, Time.deltaTime * slerp);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        float targetDistance = Vector2.Distance(rb.position, target.position);

        float playerDistance = Vector2.Distance(rb.position, player.gameObject.transform.position);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (targetDistance <= distanceToAttack)
        {
            rb.linearVelocity = Vector2.zero;
        }
        
        if (playerDistance <= 5f)
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
        List<float> distances = new List<float>();
        foreach (var point in retreatPoints)
        {
            float distance = Vector2.Distance(rb.position, point.transform.position);
            distances.Add(distance);
        }
        distances.Sort();
        foreach (var point in retreatPoints)
        {
            float distance = Vector2.Distance(rb.position, point.transform.position);
            if (distance == distances[0])
            {
                target = point.transform;
            }
        }
        state = States.Running;
    }
    public void FindSecondRetreat()
    {
        List<float> distances = new List<float>();
        foreach (var point in retreatPoints)
        {
            float distance = Vector2.Distance(rb.position, point.transform.position);
            distances.Add(distance);
        }
        distances.Sort();
        foreach (var point in retreatPoints)
        {
            float distance = Vector2.Distance(rb.position, point.transform.position);
            if (distance == distances[1])
            {
                target = point.transform;
            }
        }
        state = States.Running;
    }
}
