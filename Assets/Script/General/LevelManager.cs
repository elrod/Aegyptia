using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public GameObject currentCheckpointP1 = null;
	public GameObject currentCheckpointP2 = null;
	public GameObject deathParticle;
	public GameObject respawnParticle;
	public float respawnDelay;
	bool respawning = false;

    int activeCharacters = 2;

	GameObject player;
	PlayerGovernor playerGovernor;
	
	// Update is called once per frame
	void Update () {

		// Find the active player
		player = GameObject.FindGameObjectWithTag ("Player");
		playerGovernor = FindObjectOfType<PlayerGovernor> ();

	}

	public void RespawnPlayer(){
		if (!respawning) {
			// To insert the respawn delay the code must be executed in a coroutine
			StartCoroutine ("RespawnPlayerCo");
		}
	}

	public IEnumerator RespawnPlayerCo(){

		respawning = true;

		// Instantiate the particle system representing the death of the player
		Instantiate (deathParticle, player.transform.position, player.transform.rotation);
		player.GetComponent<Player>().ManageShapeOnRespawn();
		player.GetComponent<Player> ().enabled = false;
		player.GetComponent<Renderer> ().enabled = false;
		//Debug.Log ("Player respawn");
	
		// Now we wait the respawn delay so the death animation can be seen and then the player respawn
		// to the last activated chekpoint 
		yield return new WaitForSeconds (respawnDelay);
		if (playerGovernor.IsP1Active ()) {
            Vector3 respawnPos = currentCheckpointP1.transform.position;
            respawnPos.z = -2;
            player.transform.position = respawnPos;
		} else {
            Vector3 respawnPos = currentCheckpointP2.transform.position;
            respawnPos.z = -2;
            player.transform.position = respawnPos;
        }
		player.GetComponent<Player> ().enabled = true;
		player.GetComponent<Renderer> ().enabled = true;
		Instantiate (respawnParticle, player.transform.position, player.transform.rotation);

		respawning = false;
	}

    public void PlayerExit()
    {
        player.GetComponent<Renderer>().enabled = false;
        playerGovernor.canSwitchPlayer = false;
        activeCharacters--;
        if(activeCharacters > 0)
        {
            playerGovernor.ForcePlayerSwitch();
        }
        else
        {
            Application.LoadLevel("Credits");
        }
        
    }
}
