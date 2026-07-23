using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Switch : MonoBehaviour
{
    [SerializeField] private float timerMin;
    
    [SerializeField] private float timerMax;

    [SerializeField] private PlayerMovement player;

    [SerializeField] private UnityEvent lightFlicker;

    [SerializeField] private UnityEvent countDownVampire;

    [SerializeField] private UnityEvent countDownChicken;

    [SerializeField] private GameObject count;

    [SerializeField] private GameObject down;

    [SerializeField] private List<GameObject> lights;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartSequence()
    {
        StartCoroutine(InitateCountDown());
    }
    public IEnumerator InitateCountDown()
    {
        if (player.currentForm == Forms.Chicken)
        {
            countDownVampire.Invoke();
        }
        if (player.currentForm == Forms.Vampire)
        {
            float time = Random.Range(timerMin, timerMax);
            yield return new WaitForSeconds(time / 2);
            time = time / 2;
            lightFlicker.Invoke();
            yield return new WaitForSeconds(time / 2);
            time = time / 2;
            lightFlicker.Invoke();
            yield return new WaitForSeconds(time);
            countDownChicken.Invoke();
        }
    }

    public void LightsFlicker()
    {
        foreach(var light in lights)
        {
            StartCoroutine(Flicker(light));
        }
    }

    public void CountDown(GameObject text)
    {
        StartCoroutine(Count(text));
    }

    public IEnumerator Count(GameObject text)
    {
        //play sound
        StartCoroutine(FlickerText(text));
        yield return new WaitForSeconds(3f);
        text.SetActive(true);
        if (text == down)
        {
            foreach (var light in lights)
            {
                light.SetActive(true);
            }
            player.currentForm = Forms.Chicken;
            yield return new WaitForSeconds(1f);
            //fun effect
            text.SetActive(false);
        }
        else
        {
            player.dashing = false;
            player.canDash = true;
            player.canAttack = true;
            foreach (var light in lights)
            {
                light.SetActive(false);
            }
            player.currentForm = Forms.Vampire;
            yield return new WaitForSeconds(1f);
            //fun effect
            text.SetActive(false);
            StartSequence();
        }
    }
    public IEnumerator Flicker(GameObject light)
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
        light.SetActive(false);
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
