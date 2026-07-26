using UnityEngine;
using UnityEngine.UI;
public class Achievements : MonoBehaviour
{
    [SerializeField] public GameObject text;

    [SerializeField] public bool unlocked;

    [SerializeField] public string name;

    [SerializeField] public Image image;

    [SerializeField] public int index;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerPrefs.GetInt(name) == 1)
        {
            unlocked = true;
            Unlocked();
        }
        if (!unlocked)
        {
            if (text != null)
                text.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void Unlocked()
    {
        if (unlocked)
        {
            image.color = Color.white;
            if (text != null)
            text.SetActive(true);
        }
    }
    public void Unlock()
    {
        if (!unlocked)
        {
            unlocked = true;
            PlayerPrefs.SetInt(name, 1);
            Unlocked();
        }
    }
}
