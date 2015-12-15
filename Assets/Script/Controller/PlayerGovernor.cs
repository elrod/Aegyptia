using UnityEngine;
using System.Collections;


public class PlayerGovernor : MonoBehaviour {
	
	public GameObject player1;
	public GameObject player2;

	bool isP1Active;

	
	// Use this for initialization
	void Start () {

		// This is needed to understand which player is active at the beginning
		// Can be avoided if the game starts everytime with one specific player
		if (player1.Equals (GameObject.FindGameObjectWithTag ("Player"))) {
			isP1Active = true;
            FindObjectOfType<LevelEventsManager>().NotifyEvent("osiris", "OSIRIS_BEGIN");
            player2.GetComponent<Player>().TurnOff();
		} else {
			isP1Active = false;
            FindObjectOfType<LevelEventsManager>().NotifyEvent("isis", "ISIS_BEGIN");
            player1.GetComponent<Player>().TurnOff();
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
		// Check if the player is changed
		if (Input.GetButtonDown("SwitchPlayer")) {
			if (isP1Active) { 
				SwitchPlayer(player1, player2);
                FindObjectOfType<LevelEventsManager>().NotifyEvent("isis", "ISIS_BEGIN");
            }
            else { 
				SwitchPlayer(player2, player1);
                FindObjectOfType<LevelEventsManager>().NotifyEvent("osiris", "OSIRIS_BEGIN");
            }
        }

	}
	
	void SwitchPlayer (GameObject activeBefore, GameObject activeNow){
		//activeBefore.transform.gameObject.tag = "InactivePlayer";
		//activeNow.transform.gameObject.tag = "Player";
		activeBefore.GetComponent<Player> ().TurnOff ();
		activeNow.GetComponent<Player> ().TurnOn ();
		isP1Active = !isP1Active;
		Camera.main.GetComponent<CameraMovement>().SwitchingFocus(activeNow.transform.position);
	}

	public bool IsP1Active(){
		return isP1Active;
	}
}
