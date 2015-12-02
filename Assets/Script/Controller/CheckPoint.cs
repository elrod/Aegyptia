using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour {

	public Color inactiveCheckPoint;
	public Color activeCheckpoint;

	LevelManager levelManager;
	
	// Use this for initialization
	void Start () {
		levelManager = FindObjectOfType<LevelManager> ();
		gameObject.GetComponent<MeshRenderer> ().material.color = inactiveCheckPoint;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter2D (Collider2D coll){
		if (coll.tag == "Player") {
			levelManager.currentCheckpoint.GetComponent<MeshRenderer> ().material.color = inactiveCheckPoint; 	
			gameObject.GetComponent<MeshRenderer> ().material.color = activeCheckpoint;
			levelManager.currentCheckpoint = gameObject;
			Debug.Log("Activated checkpoint " + transform.position);
		}
	}
}
