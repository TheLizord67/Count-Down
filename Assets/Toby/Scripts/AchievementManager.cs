using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AchievementManager : MonoBehaviour
{
    [SerializeField] public List<Achievements> achievementsStreak;
    [SerializeField] public List<Achievements> achievementsKills;
    [SerializeField] public List<Achievements> achievementsSurvive;
    [SerializeField] public Achievements chicken;

    [SerializeField] public float timeAsChicken;
    [SerializeField] public GameObject text;

    public PlayerMovement player;
    void Start()
    {
        player = GetComponent<PlayerMovement>();
        foreach (var ach in achievementsStreak)
        {
            if (ach.unlocked == true)
            {
                achievementsStreak.Remove(ach);
            }
        }
        foreach (var ach in achievementsKills)
        {
            if (ach.unlocked == true)
            {
                achievementsKills.Remove(ach);
            }
        }
        foreach (var ach in achievementsSurvive)
        {
            if (ach.unlocked == true)
            {
                achievementsSurvive.Remove(ach);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        ResetCheck();
        if (player.currentForm == Forms.Chicken)
        {
            timeAsChicken += Time.deltaTime;
        }
        else
        {
            timeAsChicken = 0;
        }
        CheckForUnlocks();
    }

    public void ResetCheck()
    {
        if (achievementsStreak != null)
        {
            foreach (var ach in achievementsStreak)
            {
                if (ach.unlocked == true)
                {
                    achievementsStreak.Remove(ach);
                }
            }
        }
        if (achievementsKills != null)
        {
            foreach (var ach in achievementsKills)
            {
                if (ach.unlocked == true)
                {
                    achievementsKills.Remove(ach);
                }
            }
        }
        if (achievementsSurvive != null)
        {
            foreach (var ach in achievementsSurvive)
            {
                if (ach.unlocked == true)
                {
                    achievementsSurvive.Remove(ach);
                }
            }
        }
    }
    public void CheckForUnlocks()
    {
        foreach (var ach in achievementsStreak)
        {
            if (player.killStreak >= ach.index)
            {
                ach.Unlock();
                text.SetActive(true);
                achievementsStreak.Remove(ach);
                StartCoroutine(TurnOffText());
            }
        }
        foreach (var ach in achievementsKills)
        {
            if (KillCount.kills >= ach.index)
            {
                ach.Unlock();
                text.SetActive(true);
                achievementsKills.Remove(ach);
                StartCoroutine(TurnOffText());
            }
        }
        foreach (var ach in achievementsSurvive)
        {
            if (player.fuseBoxTouched >= ach.index)
            {
                ach.Unlock();
                text.SetActive(true);
                achievementsSurvive.Remove(ach);
                StartCoroutine(TurnOffText());
            }
        }
        if (timeAsChicken >= chicken.index)
        {
            chicken.Unlock();
            text.SetActive(true);
            StartCoroutine(TurnOffText());
        }
    }

    public IEnumerator TurnOffText()
    {
        yield return new WaitForSeconds(2f);
        text.SetActive(false);
    }
}
