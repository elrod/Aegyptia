using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	Vector2 velocity;
	
	public float smoothTimeY = 0.2f;
	public float smoothTimeX = 0.2f;
	
	public float switchTime = 3f;
	float elapsedTime;
	
	public float cameraSize = 5f;
	public float zoomOutSize = 10f;
	public Vector2 offsetBase;
	public Vector2 offset;

	
	bool switchingPlayer = false;
	bool moveFocus = false;
	bool moving = false;
	bool comingBack = false;
	Vector3 stopPosition;
	
	Vector3 startPos;
	Vector3 destPos;
	
	GameObject activePlayer;
	GameObject stoppedPlayer;
	
	// Use this for initialization
	void Start () {
		// It uses the Tag to know which player is active
		activePlayer = GameObject.FindGameObjectWithTag ("Player");
		startPos = transform.position;
	}
	
	// NOTE: Don't use FixedUpdate, use Update here... I think this was causing that camera vibration feeling while the player
	// was moving. FixedUpdate should be used for physics only... It could be called more than once per frame, so this could result
	// in a camera moving faster than it should and vibrating when the player is moving!
	void Update(){
		
		if (moving) {
			ReachNewPosition();
		}
		
		if (comingBack) {
			ComeBack();
		}
		
		if (!moving && !comingBack) {
			float posX = Mathf.SmoothDamp (transform.position.x, activePlayer.transform.position.x + offset.x + offsetBase.x, ref velocity.x, smoothTimeX);
			float posY = Mathf.SmoothDamp (transform.position.y, activePlayer.transform.position.y + offset.y + offsetBase.y, ref velocity.y, smoothTimeY);
			
			transform.position = new Vector3 (posX, posY, transform.position.z);
		}
	}
	
	// Used to set the variables to execute the routine associated to the change of the active player
	public void SwitchPlayer(Vector3 dest){
		activePlayer = GameObject.FindGameObjectWithTag ("Player");
		destPos = dest + new Vector3(offsetBase.x, offsetBase.y, 0);
		startPos = transform.position;
		switchingPlayer = true;
		moving = true;
		FindObjectOfType<PlayerGovernor> ().DisableInput ();
	}
	
	// Used to set the variables to execute the routine associated to the temporarary change of the focused element
	public void SwitchFocus(Vector3 dest){
		destPos = dest;
		startPos = transform.position;
		moveFocus = true;
		moving = true;
		FindObjectOfType<PlayerGovernor> ().DisableInput ();
	}
	
	void ReachNewPosition(){
		bool isArrived = elapsedTime >= switchTime;
		if (isArrived) {
			elapsedTime = 0;
			moving = false;
			if(moveFocus){
				startPos = activePlayer.transform.position;
				comingBack = true;
				moveFocus = false;
			} else {
				switchingPlayer = false;
				FindObjectOfType<PlayerGovernor> ().EnableInput ();
			}
		} else {
			elapsedTime += Time.deltaTime;
			float percTime = elapsedTime/switchTime;
			if(switchingPlayer){
				if(percTime <= 0.5f){
					Camera.main.orthographicSize = Mathf.Lerp(cameraSize, 2*zoomOutSize, percTime);
				} else {
					Camera.main.orthographicSize = Mathf.Lerp(2*zoomOutSize, cameraSize, percTime);
				}
			}
			float posX = Mathf.Lerp(startPos.x, destPos.x, percTime);
			float posY = Mathf.Lerp(startPos.y, destPos.y, percTime);
			transform.position = new Vector3 (posX, posY, transform.position.z);
		}
	}
	
	void ComeBack(){
		bool isArrived = elapsedTime >= switchTime;
		if (!isArrived) {
			elapsedTime += 2*Time.deltaTime;
			float percTime = elapsedTime/switchTime;
			float posX = Mathf.Lerp(destPos.x, startPos.x, percTime);
			float posY = Mathf.Lerp(destPos.y, startPos.y, percTime);
			
			transform.position = new Vector3 (posX, posY, transform.position.z);
		} else {
			elapsedTime = 0;
			comingBack = false;
			FindObjectOfType<PlayerGovernor> ().EnableInput ();
		}
	}

	public bool IsMoving(){
		return moving || comingBack;
	}
}
