using TMPro;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI killText;
    [SerializeField] private TextMeshProUGUI killTextReward;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
