using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtons : MonoBehaviour
{
    [SerializeField] private Animator settings;
    [SerializeField] private Animator credits;
    [SerializeField] private TextMeshProUGUI killText;
    [SerializeField] private TextMeshProUGUI killText2;
    [SerializeField] private bool reg;
    void Start()
    {
        killText.text = PlayerPrefs.GetInt("Max Kills").ToString();
    }

    void Update()
    {
        if (reg)
        {
            killText2.text = KillCount.kills.ToString();
        }
    }
    public void LoadScene(Animator animator)
    {
        animator.SetBool("Transition", true);

    }
    public void LoadSceneWithNoTransition(string scene)
    {
        Time.timeScale = 1;
        StartCoroutine(Scene(scene));
    }

    public IEnumerator Scene(string scene)
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(scene);
    }
    public void Quit()
    {

        Application.Quit();
    }
    public void PlaySound(AudioSource audioSource)
    {
        audioSource.Play();
    }
    public void TriggerAnimationTrue(string boolName)
    {
        settings.SetBool(boolName, true);
    }

    public void TriggerAnimationFalse(string boolName)
    {
        settings.SetBool(boolName, false);
    }

    public void TriggerCreditsTrue(string boolName)
    {
        credits.SetBool(boolName, true);
    }

    public void TriggerCreditsFalse(string boolName)
    {
        credits.SetBool(boolName, false);
    }

    public void AnimationSet(Animator animator)
    {
        animator.SetBool("Clicked", true);
        StartCoroutine(Unset(animator));
    }


    public IEnumerator Unset(Animator animator)
    {
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("Clicked", false);

    }
}
