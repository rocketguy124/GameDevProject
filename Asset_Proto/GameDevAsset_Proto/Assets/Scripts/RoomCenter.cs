﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCenter : MonoBehaviour
{
    public bool openWhenEnemiesCleared;
    public List<GameObject> enemies = new List<GameObject>();

    

    //[HideInInspector]
    public Room theRoom;
    public bool enemiesActive;

    // Start is called before the first frame update
    void Start()
    {
        if (openWhenEnemiesCleared)
        {
            theRoom.closeWhenEntered = true;
        }
        enemiesActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemies.Count > 0 && theRoom.roomActive && openWhenEnemiesCleared)
        {
            if (!enemiesActive)
            {
                theRoom.ActivateEnemies(enemies);
                enemiesActive = true;
            }
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] == null)
                {
                    enemies.RemoveAt(i);
                    i--;
                }
            }
            if (enemies.Count == 0)
            {
                theRoom.OpenDoors();
            }
        }
    }
}
