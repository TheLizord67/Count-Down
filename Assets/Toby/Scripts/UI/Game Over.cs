using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GameOver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI killText;
    [SerializeField] private TextMeshProUGUI killTextReward;
    [SerializeField] private Animator gameOver;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public IEnumerator OverOver()
    {
        yield return new WaitForSeconds(1f);
        gameOver.SetBool("Transition", true);
    }
    // Update is called once per frame
    void Update()
    {
        killText.text = KillCount.kills.ToString();
        if (KillCount.kills > PlayerPrefs.GetInt("Max Kills"))
        {
            killTextReward.gameObject.SetActive(true);
        }
    }
}
