using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof (AudioSource))]
public class LevelManager : MonoBehaviour {

	public GameObject currentCheckpointP1 = null;
	public GameObject currentCheckpointP2 = null;
	public GameObject deathParticle;
	public GameObject respawnParticle;
	public float respawnDelay;
	public AudioClip isisDeathClip;
	public AudioClip osirisDeathClip;
    public GameObject endLevelPanel;
    public Text endLevelFound;
    public Text endLevelTotal;
	public string nextLevel = "Credits";
	bool respawning = false;

    int activeCharacters = 2;

	GameObject player;
	PlayerGovernor playerGovernor;

    // Level Manual Selection
    float timeToSelect = 3f;
    float selectingStartTime = 0f;
    bool selectingLevel = false;
    int selectedLevel = 0;

    // Self-kill
    float selfKillStartTime = 0f;
    bool killingYourself = false;

    // Update is called once per frame
    void Update () {

		// Find the active player
		player = GameObject.FindGameObjectWithTag ("Player");
		playerGovernor = FindObjectOfType<PlayerGovernor> ();
        if(endLevelPanel == null)
        {
            Debug.LogError("Please drag endLevelPanel from GUI to inspector in LevelManager");
        }

        SecretCommandsUpdate();

    }

	public void RespawnPlayer(){
		if (!respawning) {
            playerGovernor.canSwitchPlayer = false;
			// To insert the respawn delay the code must be executed in a coroutine
			StartCoroutine ("RespawnPlayerCo");
		}
	}

	public IEnumerator RespawnPlayerCo(){

		respawning = true;
		
        Debug.Log("1: " +playerGovernor.canSwitchPlayer);
		// Instantiate the particle system representing the death of the player
		Instantiate (deathParticle, player.transform.position, player.transform.rotation);
		if (playerGovernor.IsP1Active ()) {
			GetComponent<AudioSource> ().clip = osirisDeathClip;
		} else {
			GetComponent<AudioSource>().clip = isisDeathClip;
		}
		GetComponent<AudioSource> ().Play ();
		player.GetComponent<Player>().ManageShapeOnRespawn();
		player.GetComponent<Player> ().enabled = false;
		player.GetComponent<Renderer> ().enabled = false;
		//Debug.Log ("Player respawn");

		// Now we wait the respawn delay so the death animation can be seen and then the player respawn
		// to the last activated chekpoint 
       
		yield return new WaitForSeconds (respawnDelay);
        playerGovernor.canSwitchPlayer = false;

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
		playerGovernor.canSwitchPlayer = true;
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
            //Application.LoadLevel(nextLevel);
            player.GetComponent<Player>().enabled = false;
            player.GetComponent<Controller2D>().enabled = false;
            endLevelFound.text = FindObjectOfType<Collector>().getCollected().ToString();
            endLevelTotal.text = FindObjectOfType<Collector>().getTotal().ToString();
            endLevelPanel.SetActive(true);
        }
        
    }

    // TRICKS AREA:

    void SecretCommandsUpdate()
    {
        // Manual level switch check

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectingLevel = true;
            selectedLevel = 1;
            selectingStartTime = Time.time;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectingLevel = true;
            selectedLevel = 2;
            selectingStartTime = Time.time;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectingLevel = true;
            selectedLevel = 3;
            selectingStartTime = Time.time;
        }
        if (Input.GetKeyUp(KeyCode.Alpha1) || Input.GetKeyUp(KeyCode.Alpha2) || Input.GetKeyUp(KeyCode.Alpha3))
        {
            selectingLevel = false;
            selectedLevel = 0;
            selectingStartTime = 0f;
        }

        if (selectingLevel && (Time.time - selectingStartTime) >= timeToSelect)
        {
            ManuallyLoadLevel(selectedLevel);
        }

        // Kill yourself check
        if (Input.GetKeyDown(KeyCode.K))
        {
            killingYourself = true;
            selfKillStartTime = Time.time;
        }
        if (Input.GetKeyUp(KeyCode.K))
        {
            killingYourself = false;
            selfKillStartTime = Time.time;
        }
        if (killingYourself && (Time.time - selfKillStartTime >= timeToSelect))
        {
            RespawnPlayer();
        }
    }

    // Keep pressed the number of the level for 3 seconds to load it:
    void ManuallyLoadLevel(int selectedLevel)
    {
        switch (selectedLevel)
        {
            case 1:
                Application.LoadLevel("Tutorial");
                break;
            case 2:
                Application.LoadLevel("DemoLevel");
                break;
            case 3:
                Application.LoadLevel("FinalLevel");
                break;
        }
    }
}
