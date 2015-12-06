using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour {

	public Color inactiveCheckPoint;
	public Color activeCheckpoint;

	LevelManager levelManager;
	PlayerGovernor playerGovernor;
	
	// Use this for initialization
	void Start () {
		levelManager = FindObjectOfType<LevelManager> ();
		playerGovernor = FindObjectOfType<PlayerGovernor> ();

		gameObject.GetComponent<MeshRenderer> ().material.color = inactiveCheckPoint;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter2D (Collider2D coll){
		if (coll.tag == "Player") {
			if (playerGovernor.IsP1Active()){
				if (levelManager.currentCheckpointP1 != null){
					levelManager.currentCheckpointP1.GetComponent<MeshRenderer> ().material.color = inactiveCheckPoint; 	
				}
				levelManager.currentCheckpointP1 = gameObject;
			} else {
				if (levelManager.currentCheckpointP2 != null){
					levelManager.currentCheckpointP2.GetComponent<MeshRenderer> ().material.color = inactiveCheckPoint; 	
				}
				levelManager.currentCheckpointP2 = gameObject;
			}
			gameObject.GetComponent<MeshRenderer> ().material.color = activeCheckpoint;
			// Debug.Log("Activated checkpoint " + transform.position);
		}
	}
}
