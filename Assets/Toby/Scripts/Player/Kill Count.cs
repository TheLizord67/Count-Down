using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KillCount : MonoBehaviour
{
    [SerializeField] public static int kills;
    [SerializeField] public static float speedIncrease;
    [SerializeField] public List<RoomManager> rooms;
    private PlayerMovement player;
    private KillCountJuice killCountJuice;

    [SerializeField] public UnityEvent killEvent;


    public void OnDestroy()
    {
        int currentMaxKills = PlayerPrefs.GetInt("Max Kills");
        if (kills > currentMaxKills)
        {
            PlayerPrefs.SetInt("Max Kills", kills);
        }
    }

    void Start()
    {
        kills = 0;
        player = FindAnyObjectByType<PlayerMovement>();
        killCountJuice = FindAnyObjectByType<KillCountJuice>();
        RoomManager[] r = FindObjectsByType<RoomManager>();
        foreach (var r2 in r)
        {
            rooms.Add(r2);
        }
        // KILL EVENT LISTENERS
        killEvent.AddListener(player.SpeedIncrease);
        killEvent.AddListener(killCountJuice.Inflate);
    }
    public void UpdateAllRooms()
    {
        
        foreach (var room in rooms)
        {
            room.AdjustSettings();
        }
        //speed increase from this cause it's convenient
        killEvent.Invoke();

    }
}
