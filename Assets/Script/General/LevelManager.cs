using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public GameObject currentCheckpoint;
	public GameObject deathParticle;
	public GameObject respawnParticle;
	public float respawnDelay;

	GameObject player;
	
	// Update is called once per frame
	void Update () {

		// Find the active player
		player = GameObject.FindGameObjectWithTag ("Player");

	}

	public void RespawnPlayer(){

		// To insert the respawn delay the code must be executed in a coroutine
		StartCoroutine ("RespawnPlayerCo");

	}

	public IEnumerator RespawnPlayerCo(){

		// Instantiate the particle system representing the death of the player
		Instantiate (deathParticle, player.transform.position, player.transform.rotation);
		player.GetComponent<Controller2D> ().enabled = false;
		player.GetComponent<Renderer> ().enabled = false;

		//Debug.Log ("Player respawn");
	
		// Now we wait the respawn delay so the death animation can be seen and then the player respawn
		// to the last activated chekpoint 
		yield return new WaitForSeconds (respawnDelay);
		player.transform.position = currentCheckpoint.transform.position;
		player.GetComponent<Controller2D> ().enabled = true;
		player.GetComponent<Renderer> ().enabled = true;
		Instantiate (respawnParticle, player.transform.position, player.transform.rotation);

	}
}
