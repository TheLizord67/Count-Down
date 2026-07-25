using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [SerializeField] private Color darkTint;
    [SerializeField] private Color lightTint;
    [SerializeField] private SpriteRenderer mySprite;
    [SerializeField] private BoxCollider2D myCollider;
    private PlayerMovement playerScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerScript = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerScript.currentForm == Forms.Chicken)
        {
            mySprite.color = darkTint;
            myCollider.enabled = false;
        }
        else
        {
            mySprite.color = lightTint;
            myCollider.enabled = true;
        }
    }
}
