using UnityEngine;
using System.Collections;

public class KillPlayer : MonoBehaviour {

    public bool killAfterTime = false;
    public float killTime = 5f;
	public bool killOnlyHumans = false;

	LevelManager levelManager;
    PlayerGovernor playerGovernor;
    bool inTrap;
    float inTrapTime;



	// Use this for initialization
	void Start () {
		levelManager = FindObjectOfType<LevelManager> ();
        playerGovernor = FindObjectOfType<PlayerGovernor>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D (Collider2D coll){
		if (coll.tag == "Player") {
			if((killOnlyHumans && coll.gameObject.GetComponent<Player>().IsHuman()) || !killOnlyHumans){
	            if (killAfterTime)
	            {
	                /* This should happen only for poison right now... */
	                inTrap = true;
	                inTrapTime = Time.time;
                    playerGovernor.canSwitchPlayer = false;
	            }
	            else { 
				    levelManager.RespawnPlayer();
	            }
			}
            
        }
	}

    void OnTriggerStay2D (Collider2D coll)
    {
        if(coll.tag == "Player" && inTrap)
        {
			if((killOnlyHumans && coll.gameObject.GetComponent<Player>().IsHuman()) || !killOnlyHumans){
	            if((Time.time - inTrapTime) >= killTime)
	            {
	                levelManager.RespawnPlayer();
	                inTrap = false;
	            }
                
			}
            if (killAfterTime)
            {
                playerGovernor.canSwitchPlayer = false;
            }
        }
        
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "Player")
        {
            inTrap = false;
        }
        if (killAfterTime)
        {
            playerGovernor.canSwitchPlayer = true;
        } 
    }

}
