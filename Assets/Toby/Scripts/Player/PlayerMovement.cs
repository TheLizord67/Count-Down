using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public enum Forms { Vampire, Chicken };
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputManager manager;

    [SerializeField] public float vampSpeed, vampDash, vampDashDuration, vampDashCooldown, speedIncrease;

    [SerializeField] private float chickenSpeed, chickenDash, chickenDashDuration, chickenDashCooldown, atkCool;

    [SerializeField] public float speedCap, rotateSpeed, animWaitTime, latchedTime, IFrameTime;

    [SerializeField] public bool dashing, canDash, canAttack, latched, invincible, dead;

    [SerializeField] public int hp;

    [SerializeField] public float vampAdditionalSpeed;

    private Vector2 _movement;

    private Rigidbody2D rb;

    [SerializeField] public Forms currentForm;

    private Vector3 mousePos;

    private Vector2 moveInput;

    [SerializeField] public Camera mainCam;

    [SerializeField] private GameObject hitBox, attackBox, chicken, vampire, dashParts;

    [SerializeField] private Switch switchForm;

    [SerializeField] public List<GameObject> hits, hearts;

    [SerializeField] public SpriteCharacterControl sprite;

    [SerializeField] public RoomManager currentRoom;

    [SerializeField] public Transform fuseBoxOn;

    [SerializeField] private BackgroungLightFlicker background;

    [SerializeField] public GameObject gameOver, blood, feathers, emo;

    [SerializeField] public Volume globalVolume;

    public int killStreak, fuseBoxTouched;

    [SerializeField] public float duration, magnitude;
    [SerializeField] private ScreenShake screenShake;

    private Vignette vignetteEffect;

    [SerializeField] public AudioSource swipe, dash, footstepsVamp, footstepsChicken, bite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void Hurt()
    {
        if (currentForm == Forms.Chicken) { 
            vignetteEffect.intensity.value = 0.7f;
            hp -= 1;
            Destroy(hearts[0].gameObject);
            hearts.Remove(hearts[0]);
            StartCoroutine(IFrames());
        }
    }
    void Awake()
    {
        if (globalVolume != null && globalVolume.profile.TryGet(out vignetteEffect))
        {
            vignetteEffect.intensity.overrideState = true;
        }
        rb = GetComponent<Rigidbody2D>();
        screenShake = FindAnyObjectByType<ScreenShake>();
        mainCam = Camera.main;
        switchForm.StartSequence();
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            if (currentRoom.fuseBox.off == true)
            {
                fuseBoxOn = currentRoom.fuseBox.transform;
            }
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
                if (rb.linearVelocity.magnitude > 0.1f)
                {
                    footstepsVamp.Play();
                }
                rb.linearVelocity = _movement * (vampSpeed + vampAdditionalSpeed);

                if (rb.linearVelocity.magnitude > speedCap)
                {
                    rb.linearVelocity = rb.linearVelocity.normalized * speedCap;
                }
            }
            if (currentForm == Forms.Chicken && dashing == false)
            {
                if (rb.linearVelocity.magnitude > 0.1f)
                {
                    footstepsChicken.Play();
                }
                rb.linearVelocity = _movement * chickenSpeed;
                if (rb.linearVelocity.magnitude > speedCap)
                {
                    rb.linearVelocity = rb.linearVelocity.normalized * speedCap;
                }
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Room") && currentRoom != collision.gameObject.GetComponent<RoomManager>())
        {
            currentRoom = collision.gameObject.GetComponent<RoomManager>();
            currentRoom.InitalSpawn();
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Breaker") && currentForm == Forms.Chicken && dashing == true)
        {
            if (collision.gameObject.GetComponent<Fusebox>().off)
            {
                fuseBoxTouched += 1;
                collision.gameObject.GetComponent<Fusebox>().off = false;
                collision.gameObject.GetComponent<Fusebox>().StartCoroutine(collision.gameObject.GetComponent<Fusebox>().SparksFly());
                switchForm.StartSequence();
                background.Yellow(false);
            }
        }
        if (dashing)
        {
            dashing = false;
        }
    }
    public void Dashing(InputAction.CallbackContext context)
    {
        if (context.started && dashing == false && canDash == true && dead == false)
        {
            canAttack = false;
            if (currentForm == Forms.Vampire)
            {
                dash.Play();
                dashing = true;
                hitBox.SetActive(true);
                StartCoroutine(Cooldown(vampDashCooldown, vampDashDuration, vampDash + vampAdditionalSpeed * 1.5f));
                StartCoroutine(Cooldown(vampDashCooldown, vampDashDuration, vampDash + vampAdditionalSpeed * 1.5f));
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
            swipe.Play();
            canAttack = false;
            attackBox.SetActive(true);
            StartCoroutine(StopAttack());
        }
    }
    public IEnumerator StopAttack()
    {
        yield return new WaitForSeconds(0.1f);
        attackBox.SetActive(false);
        yield return new WaitForSeconds(atkCool);
        canAttack = true;
    }
    public IEnumerator Cooldown(float cooldownTime, float duration, float speed)
    {
        canDash = false;
        rb.linearVelocity = new Vector2(_movement.x * speed, _movement.y * speed);
        if (rb.linearVelocity.magnitude > speedCap * 3) rb.linearVelocity = rb.linearVelocity.normalized * speedCap * 3;
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
            yield return new WaitForSeconds(latchedTime);
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
    public IEnumerator PlayerParticles(GameObject particles)
    {
        particles.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        particles.SetActive(false);
    }
    public IEnumerator Bite(GameObject target)
    {
        screenShake.Shake(duration, magnitude);
        target.GetComponent<EnemyController>().chomp.SetActive(true);
        mainCam.GetComponent<CameraFollow>().kill = true;
        target.GetComponent<EnemyController>().doomed = true;
        yield return new WaitForSeconds(animWaitTime);
        currentRoom.amountSpawned -= 1;
        if (currentRoom.amountSpawned < 0)
        {
            currentRoom.amountSpawned = 0;
            currentRoom.InitalSpawn();
        }
        KillCount.kills += 1;
        KillCount kill = FindAnyObjectByType<KillCount>();
        kill.UpdateAllRooms();
        canAttack = true;
        canDash = true;
        target.GetComponent<EnemyController>().seeker.enabled = false;
        target.GetComponent<EnemyController>().chomp.SetActive(false);
        target.GetComponent<EnemyController>().sprite.Die();
        target.GetComponent<EnemyController>().enabled = false;
        target.GetComponent<CapsuleCollider2D>().enabled = false;
        target.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
    }
    public IEnumerator IFrames()
    {
        invincible = true;
        yield return new WaitForSeconds(IFrameTime);
        vignetteEffect.intensity.value = 0f;
        invincible = false;
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

    public void ResetAdditionalMovement()
    {
        vampAdditionalSpeed = 0;
        Debug.Log("count down called");
        killStreak = 0;
    }

    public void SpeedIncrease()
    {
        Debug.Log("kill event called");
        vampAdditionalSpeed += speedIncrease;
        killStreak++;
    }
}
