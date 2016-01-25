using UnityEngine;
using System.Collections;

public class DisableSwitch : MonoBehaviour {

	PlayerGovernor playerGovernor;
	LevelManager levelManager;

	// Use this for initialization
	void Start () {
		playerGovernor = FindObjectOfType<PlayerGovernor> ();
		levelManager = FindObjectOfType<LevelManager> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay2D(Collider2D coll){
		if (coll.gameObject.CompareTag ("Player")) {
			playerGovernor.canSwitchPlayer = false;
			levelManager.RemoveOneCharacter();
			GameObject inactivePlayer = GameObject.FindGameObjectWithTag ("InactivePlayer");
			inactivePlayer.GetComponent<Renderer>().enabled = false;
			inactivePlayer.GetComponent<Collider2D>().enabled = false;
			inactivePlayer.GetComponent<Controller2D>().enabled = false;
		}
	}
}
