using Unity.VisualScripting;
using UnityEngine;

public class SpriteCharacterControl : MonoBehaviour
{
    [SerializeField] private GameObject head;
    [SerializeField] private GameObject body;
    [SerializeField] private GameObject foot1;
    [SerializeField] private GameObject foot2;
    [SerializeField] private GameObject weapon;
    [SerializeField] private GameObject blood;
    [SerializeField] private bool hasWeapon;
    [SerializeField] private float headBobAmount;
    [SerializeField] private float headBobSpeed;
    [SerializeField] private float bodyBobAmount;
    [SerializeField] private float bodyBobSpeed;
    [SerializeField] private float footMaxDistance;
    [SerializeField] private float footStepSpeed;
    [SerializeField] private float footRaiseAmount;
    [SerializeField] private bool isDead;
    [SerializeField] private Color deadTint;

    private Vector3 headStartPos;
    private Vector3 bodyStartPos;
    private Vector3 footCenterFloorPos;

    private Vector3 lastFrameFoot1Pos;
    private Vector3 lastFrameFoot2Pos;
    private Vector3 lastFramePos;
    private Vector3 foot1Target;
    private Vector3 foot2Target;
    private bool facingFlipped;

    private Vector3 weaponPos;
    private Quaternion weaponRot;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        headStartPos = head.transform.localPosition;
        bodyStartPos = body.transform.localPosition;
        footCenterFloorPos = (foot1.transform.localPosition + foot2.transform.localPosition)/ 2;
        lastFrameFoot1Pos = foot1.transform.position;
        lastFrameFoot2Pos = foot2.transform.position;
        lastFramePos = transform.position;
        foot1Target = foot1.transform.position;
        foot2Target = foot2.transform.position;
        weaponPos = weapon.transform.localPosition;
        weaponRot = weapon.transform.localRotation;
        if (isDead)
        {
            Die();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {

        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);

            //HeadBob
            head.transform.localPosition = headStartPos + new Vector3(0, (Mathf.Sin(Time.time * headBobSpeed) * 0.1f * headBobAmount), 0);

            //BodyBob
            body.transform.localPosition = bodyStartPos + new Vector3(0, (Mathf.Sin(Time.time * bodyBobSpeed) * 0.1f * bodyBobAmount), 0);

            //Foot1WalkTargetCalc
            if (Vector3.Distance(lastFrameFoot1Pos, footCenterFloorPos + transform.position) > footMaxDistance)
            {
                foot1Target = (Vector3.Normalize((footCenterFloorPos + transform.position) - lastFrameFoot1Pos) * footMaxDistance * 0.98f) + transform.position + footCenterFloorPos;
            }

            //Foot2WalkTargetCalc
            if (Vector3.Distance(lastFrameFoot2Pos, footCenterFloorPos + new Vector3(0, 0.2f, 0) + transform.position) > footMaxDistance)
            {
                foot2Target = (Vector3.Normalize((footCenterFloorPos + transform.position) - lastFrameFoot2Pos) * footMaxDistance * 0.94f) + transform.position + footCenterFloorPos + new Vector3(0, 0.4f, 0);
            }
            foot1.transform.position = lastFrameFoot1Pos;
            foot2.transform.position = lastFrameFoot2Pos;

            foot1.transform.position =
                new Vector3(0, Vector3.Distance(foot1Target, foot1.transform.position) * footRaiseAmount * Time.deltaTime, 0)
                +
                Vector3.Lerp(foot1.transform.position, foot1Target, footStepSpeed * Time.deltaTime);
            foot2.transform.position =
                new Vector3(0, Vector3.Distance(foot2Target, foot2.transform.position) * footRaiseAmount * Time.deltaTime, 0)
                +
                Vector3.Lerp(foot2.transform.position, foot2Target, footStepSpeed * Time.deltaTime);

            if (!facingFlipped && lastFramePos.x - transform.position.x > 0.1 * Time.deltaTime)
            {
                facingFlipped = true;
                transform.localScale = new Vector3(-1, 1, 1);
                footCenterFloorPos = new Vector3(footCenterFloorPos.x * -1, footCenterFloorPos.y, footCenterFloorPos.z);
            }
            else if (facingFlipped && lastFramePos.x - transform.position.x < -0.1 * Time.deltaTime)
            {
                facingFlipped = false;
                transform.localScale = new Vector3(1, 1, 1);
                footCenterFloorPos = new Vector3(footCenterFloorPos.x * -1, footCenterFloorPos.y, footCenterFloorPos.z);
            }


            lastFrameFoot1Pos = foot1.transform.position;
            lastFrameFoot2Pos = foot2.transform.position;
            lastFramePos = transform.position;
        }
    }

    public void Attack(float timeToAttack)
    {
        
    }
    public void Die()
    {
        body.GetComponent<SpriteRenderer>().color = deadTint;
        weapon.GetComponent<SpriteRenderer>().color = deadTint;

        isDead = true;
        body.transform.localRotation = Quaternion.Euler(0, 0, -90);
        body.transform.localPosition = bodyStartPos - new Vector3(0, 0.5f, 0);
        blood.gameObject.SetActive(true);
        foot1.gameObject.SetActive(false);
        foot2.gameObject.SetActive(false);
        head.gameObject.SetActive(false);
        weapon.transform.localRotation = weaponRot * Quaternion.Euler(0, 0, -90);
        weapon.transform.localPosition = bodyStartPos - new Vector3(0, 0.5f, 0);
    }
}
