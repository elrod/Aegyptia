using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public GameObject currentCheckpointP1 = null;
	public GameObject currentCheckpointP2 = null;
	public GameObject deathParticle;
	public GameObject respawnParticle;
	public float respawnDelay;

	GameObject player;
	PlayerGovernor playerGovernor;
	
	// Update is called once per frame
	void Update () {

		// Find the active player
		player = GameObject.FindGameObjectWithTag ("Player");

		playerGovernor = FindObjectOfType<PlayerGovernor> ();

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
        player.GetComponent<Player>().MenageShapeOnRespawn();
		//Debug.Log ("Player respawn");
	
		// Now we wait the respawn delay so the death animation can be seen and then the player respawn
		// to the last activated chekpoint 
		yield return new WaitForSeconds (respawnDelay);
		if (playerGovernor.IsP1Active ()) {
			player.transform.position = currentCheckpointP1.transform.position;
		} else {
			player.transform.position = currentCheckpointP2.transform.position;
		}
		player.GetComponent<Controller2D> ().enabled = true;
		player.GetComponent<Renderer> ().enabled = true;
		Instantiate (respawnParticle, player.transform.position, player.transform.rotation);

	}
}
