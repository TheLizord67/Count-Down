using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KillCount : MonoBehaviour
{
    [SerializeField] public static int kills;
    [SerializeField] public static float speedIncrease;
    [SerializeField] public List<RoomManager> rooms;


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
        RoomManager[] r = FindObjectsByType<RoomManager>();
        foreach (var r2 in r)
        {
            rooms.Add(r2);
        }
    }
    public void UpdateAllRooms()
    {

        foreach (var room in rooms)
        {
            room.AdjustSettings();
        }
        //speed increase from this cause it's convenient
    }
}
