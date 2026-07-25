using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class Switch : MonoBehaviour
{
    [SerializeField] private List<EnemyController> enemies;

    [SerializeField] private float timerMin;
    
    [SerializeField] private float timerMax;

    [SerializeField] private PlayerMovement player;

    [SerializeField] private UnityEvent lightFlicker;

    [SerializeField] private UnityEvent countDownVampire;

    [SerializeField] private UnityEvent countDownChicken;

    [SerializeField] private GameObject count;

    [SerializeField] private GameObject down;

    [SerializeField] private List<GameObject> lights;

    [SerializeField] private Light2D globalLight;

    [SerializeField] private bool sequenceStarted;

    [SerializeField] private BackgroungLightFlicker background;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FindEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FindEnemies()
    {
        enemies.Clear();
        EnemyController[] enemy = FindObjectsByType<EnemyController>();
        foreach (var e in enemy)
        {
            enemies.Add(e);
        }
    }
    public void StartSequence()
    {
        if (!sequenceStarted)
        {
            StartCoroutine(InitateCountDown());
        }
    }
    public IEnumerator InitateCountDown()
    {
        sequenceStarted = true;
        if (player.currentForm == Forms.Chicken)
        {
            countDownVampire.Invoke();
        }
        if (player.currentForm == Forms.Vampire)
        {
            float time = Random.Range(timerMin, timerMax);
            yield return new WaitForSeconds(time / 2);
            time = time / 2;
            //lightFlicker.Invoke();
            yield return new WaitForSeconds(time / 2);
            time = time / 2;
            lightFlicker.Invoke();
            yield return new WaitForSeconds(time);
            countDownChicken.Invoke();
            globalLight.intensity = 1f;
            background.Yellow(true);
        }
    }

    public void LightsFlicker()
    {
        StartCoroutine(Flicker());
    }

    public void CountDown(GameObject text)
    {
        if (sequenceStarted)
        {
            text.SetActive(false);
            StartCoroutine(Count(text));
        }
    }

    public IEnumerator Count(GameObject text)
    {
        //play sound
        if (text == down)
        {
            FindEnemies();
            text.SetActive(true);
            player.currentForm = Forms.Chicken;
            player.currentRoom.SwitchRooms();
            foreach (var e in enemies)
            {
                e.ChooseTarget();
            }
            foreach (var light in lights)
            {
                light.SetActive(true);
            }
            yield return new WaitForSeconds(1f);
            //fun effect
            text.SetActive(false);
            sequenceStarted = false;
        }
        else
        {
            FindEnemies();
            player.dashing = false;
            player.canDash = true;
            player.canAttack = true;
            foreach (var light in lights)
            {
                light.SetActive(false);
            }
            text.SetActive(true);
            globalLight.intensity = 0.3f;
            player.currentForm = Forms.Vampire;
            StartCoroutine(TurnOff(text));
            yield return new WaitForSeconds(1f);
            FindEnemies();
            foreach (var e in enemies)
            {
                e.FindRetreat();
            }
            //fun effect
            text.SetActive(false);
            sequenceStarted = false;
            StartSequence();
        }
    }

    public IEnumerator TurnOff(GameObject text)
    {
        yield return new WaitForSeconds(1f);
        text.SetActive(false);
    }
    public IEnumerator Flicker()
    {
        globalLight.intensity = 1f;
        background.Yellow(true);
        yield return new WaitForSeconds(0.5f);
        globalLight.intensity = 0.3f;
        background.Yellow(false);
        yield return new WaitForSeconds(0.5f);
        globalLight.intensity = 1f;
        background.Yellow(true);
        yield return new WaitForSeconds(0.5f);
        globalLight.intensity = 0.3f;
        background.Yellow(false);
        yield return new WaitForSeconds(0.5f);
        globalLight.intensity = 1f;
        background.Yellow(false);
        yield return new WaitForSeconds(0.5f);
        globalLight.intensity = 0.3f;
        background.Yellow(true);
        yield return new WaitForSeconds(0.5f);
        globalLight.intensity = 1f;
        background.Yellow(true);
    }
    public IEnumerator FlickerText(GameObject light)
    {
        light.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        light.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        light.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        light.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        light.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        light.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        light.SetActive(true);
    }
}
