using UnityEngine;
using UnityEngine.Tilemaps;

public class WallsScript : MonoBehaviour
{
    [SerializeField] private Color darkTint;
    [SerializeField] private Color lightTint;
    [SerializeField] private Tilemap myTilemap;
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
            myTilemap.color = lightTint;
        }
        else
        {
            myTilemap.color = darkTint;
        }
    }
}
