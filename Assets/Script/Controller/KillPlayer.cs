﻿using UnityEngine;
using System.Collections;

public class KillPlayer : MonoBehaviour {

    public bool killAfterTime = false;
    public float killTime = 5f;

	LevelManager levelManager;
    bool inTrap;
    float inTrapTime;

	// Use this for initialization
	void Start () {
		levelManager = FindObjectOfType<LevelManager> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D (Collider2D coll){
		if (coll.tag == "Player") {
            if (killAfterTime)
            {
                /* This should happen only for poison right now... */
                inTrap = true;
                inTrapTime = Time.time;
            }
            else { 
			    levelManager.RespawnPlayer();
            }
        }
	}

    void OnTriggerStay2D (Collider2D coll)
    {
        if(coll.tag == "Player" && inTrap)
        {
            if((Time.time - inTrapTime) >= killTime)
            {
                levelManager.RespawnPlayer();
                inTrap = false;
            }
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "Player")
        {
            inTrap = false;
        }
    }

}
