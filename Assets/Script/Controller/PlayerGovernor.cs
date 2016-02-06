using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class PlayerGovernor : MonoBehaviour {
	
	public GameObject player1;
	public GameObject player2;

    public bool canSwitchPlayer = true;
	public float musicVolume = .1f;

    public GameObject p1Panel;
    public GameObject p2Panel;

	public AudioSource[] audioSources;
	public AudioSource[] animalsBGM;
	int currentAnimal = -1;
	bool switchAudio;
	float elapsedTime = 0f;
	float switchTime;

	bool isP1Active;
	bool enabled = true;
	
	// Use this for initialization
	void Start () {
		//audioSources = GetComponents<AudioSource> ();
		switchTime = Camera.main.GetComponent<CameraMovement> ().switchTime;
		// This is needed to understand which player is active at the beginning
		// Can be avoided if the game starts everytime with one specific player
		if (player1.Equals (GameObject.FindGameObjectWithTag ("Player"))) {
			isP1Active = true;
			if(FindObjectOfType<LevelEventsManager>() != null){
            	FindObjectOfType<LevelEventsManager>().NotifyEvent("osiris", "OSIRIS_BEGIN");
			}
			audioSources[0].volume = musicVolume;
			audioSources[1].volume = 0f;
            player2.GetComponent<Player>().TurnOff();
		} else {
			isP1Active = false;
			if(FindObjectOfType<LevelEventsManager>() != null){
            	FindObjectOfType<LevelEventsManager>().NotifyEvent("isis", "ISIS_BEGIN");
			}
			audioSources[0].volume = 0f;
			audioSources[1].volume = musicVolume;
            player1.GetComponent<Player>().TurnOff();
		}
	}
	
	// Update is called once per frame
	void Update () {
		// Check if the player is changed
		if (Input.GetButtonDown("SwitchPlayer") && enabled && canSwitchPlayer) {
            PerformSwitch();
        }
		if (switchAudio) {
			SwitchAudio();
		}
	}
	
    void PerformSwitch(){
		bool activeNowIsHuman;
        if (isP1Active)
        {
			activeNowIsHuman = SwitchPlayer(player1, player2);
            p1Panel.SetActive(false);
            p2Panel.SetActive(true);
            if (FindObjectOfType<LevelEventsManager>() != null)
            {
                FindObjectOfType<LevelEventsManager>().NotifyEvent("isis", "ISIS_BEGIN");
            }
        }
        else {
			activeNowIsHuman = SwitchPlayer(player2, player1);
            p1Panel.SetActive(true);
            p2Panel.SetActive(false);
            if (FindObjectOfType<LevelEventsManager>() != null)
            {
                FindObjectOfType<LevelEventsManager>().NotifyEvent("osiris", "OSIRIS_BEGIN");
            }
		}
		if (activeNowIsHuman || currentAnimal == -1) {
			switchAudio = true;
			elapsedTime = 0f;
		}
    }

	bool SwitchPlayer (GameObject activeBefore, GameObject activeNow){
		//activeBefore.transform.gameObject.tag = "InactivePlayer";
		//activeNow.transform.gameObject.tag = "Player";
		activeBefore.GetComponent<Player> ().TurnOff ();
		activeNow.GetComponent<Player> ().TurnOn ();
		isP1Active = !isP1Active;
		Camera.main.GetComponent<CameraMovement> ().offset = Vector2.zero;
		Camera.main.GetComponent<CameraMovement>().SwitchPlayer(activeNow.transform.position);
		return activeNow.GetComponent<Player> ().IsHuman ();
	}

	public bool IsP1Active(){
		return isP1Active;
	}

	public void DisableInput(){
		enabled = false;
		if (isP1Active) {
			player1.GetComponent<Player> ().TurnOff ();
		} else {
			player2.GetComponent<Player> ().TurnOff ();
		}
	}

	public void EnableInput(){
		enabled = true;
		if (isP1Active) {
			player1.GetComponent<Player> ().TurnOn ();
		} else {
			player2.GetComponent<Player> ().TurnOn ();
		}
	}

    public void ForcePlayerSwitch()
    {
        PerformSwitch();
    }

	void SwitchAudio(){
		elapsedTime += Time.deltaTime;
		float percTime = elapsedTime / switchTime;
		if (isP1Active) {
			audioSources[0].volume = Mathf.Lerp(0f, musicVolume, percTime);
			audioSources[1].volume = Mathf.Lerp(musicVolume, 0f, percTime);
		} else {
			audioSources[0].volume = Mathf.Lerp(musicVolume, 0f, percTime);
			audioSources[1].volume = Mathf.Lerp(0f, musicVolume, percTime);
		}
		if (elapsedTime >= switchTime) {
			switchAudio = false;
		}
	}

	public void SetP1Active(bool active){
		isP1Active = active;
		switchAudio = true;
		elapsedTime = 0f;
	}

	public void PlayAnimalBGM(string animal){
		if (animal.Equals ("crocodile")){
			currentAnimal = 0;
		} else if (animal.Equals ("scarab")){
			currentAnimal = 1;
		} else if (animal.Equals ("cat")){
			currentAnimal = -1;
		} else {
			currentAnimal = -1;
		}
		if (currentAnimal != -1) {
			if (isP1Active) {
				audioSources [0].volume = 0f;
			} else {
				audioSources [1].volume = 0f;
			}
			animalsBGM [currentAnimal].volume = musicVolume;
		}
	}

	public void StopAnimalBGM(){
		if (currentAnimal != -1) {
			if (isP1Active) {
				audioSources [0].volume = musicVolume;
			} else {
				audioSources [1].volume = musicVolume;
			}
			animalsBGM [currentAnimal].volume = 0f;
		}
	}
}
