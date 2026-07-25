using UnityEngine;

public class Torch : MonoBehaviour
{
    [SerializeField] private float speed, timeAlive, rotateSpeed;

    [SerializeField] public bool isThrown;

    [SerializeField] private Transform currentTarget;

    [SerializeField] private GameObject hitBox;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isThrown)
        {
            Thrown(currentTarget);
            transform.Rotate(0f, 0f, -rotateSpeed * Time.deltaTime);
        }
    }

    public void Thrown(Transform target)
    {
        currentTarget = target;
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Warning"))
        {
            Destroy(collision.gameObject);
            isThrown = false;
            hitBox.SetActive(true);
            //particle puff
            Destroy(this.gameObject, timeAlive);
        }
    }
}
