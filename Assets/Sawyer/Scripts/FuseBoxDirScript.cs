using UnityEngine;

public class FuseBoxDirScript : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private int margins;
    [SerializeField] private float scaleDown;

    private Vector3 relativePos;
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
        if (playerMovement.currentForm == Forms.Chicken)
        {
            relativePos = Vector3.Normalize(playerMovement.fuseBoxOn.position - transform.position);
            circleSize = Mathf.Sqrt(cam.pixelHeight * cam.pixelHeight + cam.pixelWidth * cam.pixelWidth);
            transform.localPosition = new Vector3((Mathf.Clamp(relativePos.x * circleSize, -cam.pixelWidth, cam.pixelWidth) - margins) * scaleDown, (Mathf.Clamp(relativePos.y * circleSize, -cam.pixelHeight, cam.pixelHeight) - margins) * scaleDown, 0);

        }
        
    }
}
