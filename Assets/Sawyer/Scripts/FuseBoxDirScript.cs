using UnityEngine;

public class FuseBoxDirScript : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private float scale;
    [SerializeField] private SpriteRenderer mySprite;
    [SerializeField] private float poofDistance;

    private Vector3 relativePos;
    private Vector2 normalCamSize;
    private float circleSize;
    private Camera cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = playerMovement.mainCam;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
        if (playerMovement.currentForm == Forms.Chicken && Vector3.Distance(playerMovement.fuseBoxOn.position, playerMovement.transform.position) > poofDistance)
        {
            mySprite.enabled = true;
            relativePos = Vector3.Normalize(playerMovement.fuseBoxOn.position - playerMovement.transform.position);
            normalCamSize = Vector2.Normalize(new Vector2(cam.pixelHeight, cam.pixelWidth));
            circleSize = Mathf.Sqrt(normalCamSize.x * normalCamSize.x + normalCamSize.y * normalCamSize.y);
            transform.localPosition = new Vector3(Mathf.Clamp(relativePos.x * circleSize, -normalCamSize.y, normalCamSize.y) * scale, Mathf.Clamp(relativePos.y * circleSize, -normalCamSize.x, normalCamSize.x) * scale, 10);

        }
        else
        {
            mySprite.enabled = false;
        }
        
    }
}
