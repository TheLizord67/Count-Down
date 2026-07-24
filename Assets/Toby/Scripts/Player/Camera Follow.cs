using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class CameraFollow : MonoBehaviour
{
    private Vector3 velocity = Vector3.zero;
    public Camera cam;
    public Transform target;
    public Vector3 offset;
    public Vector3 dashOffset;
    public bool kill;
    //public float oldSize, newSize, currentSize, slerp;
    [Range(1, 10)]
    public float smoothFactor;

    public float mouseInfluence;
    public float distanceSwitch;
    public float mouseSmoothness;

    public float minX, maxX, minY, maxY;
    private void FixedUpdate()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        float distance = Vector3.Distance(target.position, mousePos);
        //Debug.Log(distance);
        /*if (distance > distanceSwitch)
        {
            if ((transform.position.x > minX && transform.position.x < maxX) && (transform.position.y > minY && transform.position.y < maxY))
            {
                //Debug.Log(transform.position.y > minY && transform.position.y < maxY);
                MouseFollow();
            }
            else
            {
                mousePos = target.position;
            }
        }
        else
        {*/
        //}
        if (target.gameObject.GetComponent<PlayerMovement>().dashing == false)
        {
            Follow();
        }
        if (target.gameObject.GetComponent<PlayerMovement>().dashing == true)
        {
            FollowDash();
        }
        else
        {
            this.GetComponent<Transform>().position = Camera.main.transform.position;
        }
    }

    void Follow()
    {
        Vector3 targetPosition = target.position + offset;
        Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothFactor * Time.fixedDeltaTime);
        transform.position = smoothPosition;
    }
    void FollowDash()
    {
        Vector3 targetPosition = target.position + dashOffset;
        Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothFactor * Time.fixedDeltaTime);
        transform.position = smoothPosition;
    }

    public void MouseFollow()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        Vector3 targetPos = Vector3.Lerp(target.position, mousePos, mouseInfluence);

        targetPos.z = -10;
        transform.position = Vector3.Lerp(transform.position, targetPos, mouseSmoothness * Time.fixedDeltaTime);
    }
}
