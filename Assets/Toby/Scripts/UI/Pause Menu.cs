using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject PausePanel;
    public KeyCode pause;
    public bool isOpen;
    // Update is called once per frame

    public void Start()
    {
    }


    void Update()
    {
        if ((Input.GetKeyDown(pause) || Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(4) || Input.GetKeyDown(pause)) && isOpen == true)
        {
            Continue();
        }
        else if ((Input.GetKeyDown(pause) || Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(4) || Input.GetKeyDown(pause)) && isOpen == false)
        {
            Pause();
        }
    }
    public void Pause()
    {
        isOpen = true;
        Time.timeScale = 0;
        PausePanel.SetActive(true);
    }
    public void Continue()
    {
        //this.gameObject.GetComponent<UIButtons>().TriggerAnimationFalse("Active");
        isOpen = false;
        PausePanel.SetActive(false);
        Time.timeScale = 1;
    }
}
