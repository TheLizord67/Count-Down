using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum Forms { Vampire, Chicken };
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputManager manager;

    [SerializeField] private float vampSpeed, vampDash, vampDashDuration, vampDashCooldown;

    [SerializeField] private float chickenSpeed, chickenDash, chickenDashDuration, chickenDashCooldown;

    [SerializeField] private float speedCap, rotateSpeed;

    [SerializeField] public bool dashing, canDash, canAttack, latched;

    private Vector2 _movement;

    private Rigidbody2D rb;
    

    [SerializeField] public Forms currentForm;

    private Vector3 mousePos;

    private Vector2 moveInput;

    [SerializeField] public Camera mainCam;

    [SerializeField] private GameObject hitBox, attackBox, chicken, vampire, dashParts;

    [SerializeField] private Switch switchForm;

    [SerializeField] public List<GameObject> hits;

    [SerializeField] public SpriteCharacterControl sprite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCam = Camera.main;
        switchForm.StartSequence();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.linearVelocityX >= 0.1)
        {
            if (currentForm == Forms.Vampire)
            {
                mainCam.GetComponent<CameraFollow>().dashOffset.x = 1;
            }
            else
            {
                mainCam.GetComponent<CameraFollow>().dashOffset.x = 2;
            }
        }
        if (rb.linearVelocityY >= 0.1)
        {
            if (currentForm == Forms.Vampire)
            {
                mainCam.GetComponent<CameraFollow>().dashOffset.y = 1;
            }
            else
            {
                mainCam.GetComponent<CameraFollow>().dashOffset.y = 2;
            }
        }
        if (rb.linearVelocityX <= -0.1)
        {
            if (currentForm == Forms.Vampire)
            {
                mainCam.GetComponent<CameraFollow>().dashOffset.x = 1;
            }
            else
            {
                mainCam.GetComponent<CameraFollow>().dashOffset.x = 2;
            }
        }
        if (rb.linearVelocityY <= -0.1)
        {
            if (currentForm == Forms.Vampire)
            {
                mainCam.GetComponent<CameraFollow>().dashOffset.y = -1;
            }
            else
            {
                mainCam.GetComponent<CameraFollow>().dashOffset.y = -2;
            }
        }
        if (hits.Count > 0)
        {
            DashToEnemy(hits);
        }
        if (currentForm == Forms.Vampire)
        {
            sprite = vampire.GetComponent<SpriteCharacterControl>();
            vampire.SetActive(true);
            chicken.SetActive(false);
        }
        if (currentForm == Forms.Chicken)
        {
            sprite = chicken.GetComponent<SpriteCharacterControl>();
            vampire.SetActive(false);
            chicken.SetActive(true);
        }
        if (dashing == true)
        {
            canAttack = false;
        }
        _movement.Set(InputManager.movement.x, InputManager.movement.y);
        FollowMovement();
        if (currentForm == Forms.Vampire && dashing == false)
        {
            if (rb.linearVelocity.magnitude > speedCap)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * speedCap;
            }
            else
            {
                rb.linearVelocity = _movement * vampSpeed;
            }
        }
        if (currentForm == Forms.Chicken && dashing == false)
        {
            if (rb.linearVelocity.magnitude > speedCap)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * speedCap;
            }
            else
            {
                rb.linearVelocity = _movement * chickenSpeed;
            }
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Breaker") && currentForm == Forms.Chicken && dashing == true)
        {
            switchForm.StartSequence();
        }
        if (dashing)
        {
            dashing = false;
        }
    }
    public void Dashing(InputAction.CallbackContext context)
    {
        if (context.started && dashing == false && canDash == true)
        {
            canAttack = false;
            if (currentForm == Forms.Vampire)
            {
                dashing = true;
                hitBox.SetActive(true);
                StartCoroutine(Cooldown(vampDashCooldown, vampDashDuration, vampDash));
            }
            if (currentForm == Forms.Chicken)
            {
                dashing = true;
                StartCoroutine(Cooldown(chickenDashCooldown, chickenDashDuration, chickenDash));
            }
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (canAttack == true && currentForm == Forms.Vampire)
        {
            canAttack = false;
            attackBox.SetActive(true);
            StartCoroutine(StopAttack());
        }
    }

    public IEnumerator StopAttack()
    {
        yield return new WaitForSeconds(0.1f);
        attackBox.SetActive(false);
        yield return new WaitForSeconds(0.8f);
        canAttack = true;
    }
    public IEnumerator Cooldown(float cooldownTime, float duration, float speed)
    {
        canDash = false;
        rb.linearVelocity = new Vector2(_movement.x * speed, _movement.y * speed);
        yield return new WaitForSeconds(duration);
        if (latched == false)
        {
            dashing = false;
            canAttack = true;
        }
        hitBox.SetActive(false);
        if (latched == false)
        {
            yield return new WaitForSeconds(cooldownTime);
            rb.linearVelocity = Vector2.zero;
            canDash = true;
            canAttack = true;
        }
        if (latched == true)
        {
            yield return new WaitForSeconds(0.9f);
            latched = false;
            dashing = false;
            canAttack = true;
            yield return new WaitForSeconds(cooldownTime);
            canDash = true;
        }
    }

    public void DashToEnemy(List<GameObject> targets)
    {
        GameObject target = targets[0];
        hits.Clear();
        if (canDash == false)
        {
            canAttack = false;
            latched = true;
            hitBox.SetActive(false);
            rb.linearVelocity = Vector2.zero;
            transform.position = target.transform.position;
            StartCoroutine(Bite(target));
            //particle effects
        }
    }

    public IEnumerator Bite(GameObject target)
    {
        target.GetComponent<EnemyController>().chomp.SetActive(true);
        mainCam.GetComponent<CameraFollow>().kill = true;
        yield return new WaitForSeconds(0.5f);
        target.GetComponent<EnemyController>().chomp.SetActive(false);
        target.GetComponent<EnemyController>().sprite.Die();
        target.GetComponent<EnemyController>().enabled = false;
        target.GetComponent<BoxCollider2D>().enabled = false;
        target.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
    }
    public void LookAtMouse()
    {
        mousePos = (Vector2)mainCam.ScreenToWorldPoint(Input.mousePosition);
        float angleRad = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x);
        float angleDeg = (180 / Mathf.PI) * angleRad - 90;

        transform.rotation = Quaternion.Euler(0f, 0f, angleDeg);
    }

    public void FollowMovement()
    {
        Vector2 moveDirection = new Vector2(_movement.x, _movement.y).normalized;

        if (moveDirection != Vector2.zero)
        {
            transform.up = moveDirection;
        }
    }
}
