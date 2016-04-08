﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserWallManager : MonoBehaviour
{

    [Header("WallManager: Config")]
    public GameObject[] WallSegments = new GameObject[3];
    public GameObject[] Turrets = new GameObject[2];
    public float respawnTime = 60f; //seconds
    public Team team = Team.Neutral;

    [Header("WallManager: Status")]
    public float currRespawnTime = 0f;
    public bool isDown = false;

    private List<AI_WallTurret> turretAI = new List<AI_WallTurret>();
    // Use this for initialization
    void Start()
    {
        currRespawnTime = respawnTime;

        foreach (GameObject turret in Turrets)
        {
            AI_WallTurret comp = turret.GetComponent<AI_WallTurret>();
            if (comp == null)
            {
                Debug.LogError("All turrets need to be AI_WallTurrets");
            }
            turretAI.Add(comp);
        }
    }

    void OnGUI()
    {
        //temp solution for notifying downed shields
        if (isDown)
        {
            string text = team + " Shield is Down! (" + currRespawnTime.ToString("n0") + "s left)";
            GUI.Label(new Rect(Screen.width / 3, 0, Screen.width, Screen.height), text);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (allTurretsDown && !isDown)
        {
            isDown = true;
            disableShields();
        }

        if (isDown)
        {
            currRespawnTime -= Time.deltaTime;
        }

        if (isDown && currRespawnTime <= 0f)
        {
            isDown = false;
            enableShields();
            respawnTurrets();
            currRespawnTime = respawnTime;
        }
    }

    void disableShields()
    {
        foreach (GameObject wall in WallSegments)
        {
            wall.SetActive(false);
        }
    }

    void enableShields()
    {
        foreach (GameObject wall in WallSegments)
        {
            wall.SetActive(true);
        }
    }

    void respawnTurrets()
    {
        foreach (AI_WallTurret turret in turretAI)
        {
            turret.Respawn();
        }
    }

    bool allTurretsDown
    {
        get
        {
            foreach (AI_WallTurret turret in turretAI)
            {
                if (!turret.isDestroyed)
                {
                    return false;
                }
            }
            return true;
        }
    }

}