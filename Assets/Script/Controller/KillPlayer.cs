using UnityEngine;
using System.Collections;

public class KillPlayer : MonoBehaviour {

	LevelManager levelManager;

	// Use this for initialization
	void Start () {
		levelManager = FindObjectOfType<LevelManager> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D (Collider2D coll){
		if (coll.tag == "Player") {
			levelManager.RespawnPlayer();
		}
	}

}
