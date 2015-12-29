using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	Vector2 velocity;
	
	public float smoothTimeY = 0.2f;
	public float smoothTimeX = 0.2f;
	
	public float step = 0.02f;
	float distance = 0f;
	
	public float cameraSize = 5f;
	public float zoomOutSize = 10f;
	public Vector2 offset;
	
	bool switchingPlayer = false;
	bool moveFocus = false;
	bool moving = false;
	bool comingBack = false;
	
	Vector3 startPos;
	Vector3 destPos;
	
	GameObject activePlayer;
	
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
			float posX = Mathf.SmoothDamp (transform.position.x, activePlayer.transform.position.x + offset.x, ref velocity.x, smoothTimeX);
			float posY = Mathf.SmoothDamp (transform.position.y, activePlayer.transform.position.y + offset.y, ref velocity.y, smoothTimeY);
			
			transform.position = new Vector3 (posX, posY, transform.position.z);
		}
	}

	// Used to set the variables to execute the routine associated to the change of the active player
	public void SwitchPlayer(Vector3 dest){
		activePlayer = GameObject.FindGameObjectWithTag ("Player");
		destPos = dest;
		startPos = transform.position;
		switchingPlayer = true;
		moving = true;
		
	}

	// Used to set the variables to execute the routine associated to the temporarary change of the focused element
	public void SwitchFocus(Vector3 dest){
		destPos = dest;
		startPos = transform.position;
		moveFocus = true;
		moving = true;
	}
	
	void ReachNewPosition(){
		bool isArrived = Vector2.Distance (transform.position, destPos) == 0;
		if (isArrived) {
			distance = 0;
			moving = false;
			if(moveFocus){
				startPos = activePlayer.transform.position;
				comingBack = true;
				moveFocus = false;
			} else {
				switchingPlayer = false;
			}
		} else {
			distance += step;
			if(switchingPlayer){
				if(distance <= 0.5f){
					Camera.main.orthographicSize = Mathf.Lerp(cameraSize, 2*zoomOutSize, distance);
				} else {
					Camera.main.orthographicSize = Mathf.Lerp(2*zoomOutSize, cameraSize, distance);
				}
			}
			float posX = Mathf.Lerp(startPos.x, destPos.x, distance);
			float posY = Mathf.Lerp(startPos.y, destPos.y, distance);
			transform.position = new Vector3 (posX, posY, transform.position.z);
		}
	}
	
	void ComeBack(){
		bool isArrived = Vector2.Distance (transform.position, startPos) == 0;
		if (!isArrived) {
			distance += 2*step;
			float posX = Mathf.Lerp(destPos.x, startPos.x, distance);
			float posY = Mathf.Lerp(destPos.y, startPos.y, distance);
			
			transform.position = new Vector3 (posX, posY, transform.position.z);
		} else {
			distance = 0;
			comingBack = false;
		}
	}


	/*
	Vector2 velocity;
	
	public float smoothTimeY = 0.2f;
	public float smoothTimeX = 0.2f;

	public float normalCameraSize = 5f;
	public float zoomOutCameraSize = 10f;
	public float zoomFactor = 1.01f;
	float cameraZValue;

	bool isSwitching = false;
	bool zoomingOut = false;
	Vector3 destination;
	
	GameObject activePlayer;
	
	public bool bounds;
	public Vector3 minCameraPos;
	public Vector3 maxCameraPos;

	
	// Use this for initialization
	void Start () {
		// It uses the Tag to know which player is active
		activePlayer = GameObject.FindGameObjectWithTag ("Player");
		cameraZValue = Mathf.Abs(Camera.main.transform.position.z)+.1f;
	}
	
	// NOTE: Don't use FixedUpdate, use Update here... I think this was causing that camera vibration feeling while the player
    // was moving. FixedUpdate should be used for physics only... It could be called more than once per frame, so this could result
    // in a camera moving faster than it should and vibrating when the player is moving!
	void Update () {

		if (isSwitching && zoomingOut) {
			ZoomOut ();
		}

		if (isSwitching && !zoomingOut) {
			// With SmoothDamp we reach the position of the prayer gradually 
			float posX = Mathf.SmoothDamp (transform.position.x, destination.x, ref velocity.x, smoothTimeX);
			float posY = Mathf.SmoothDamp (transform.position.y, destination.y, ref velocity.y, smoothTimeY);
			
			transform.position = new Vector3 (posX, posY, transform.position.z);
		}

		// The camera moves only if it is not zooming out, so we are sure that if the destination is close
		// the camera doesn't reach it before completing the zoom out
		if (!isSwitching && !zoomingOut) {
			// With SmoothDamp we reach the position of the prayer gradually 
			float posX = Mathf.SmoothDamp (transform.position.x, activePlayer.transform.position.x, ref velocity.x, smoothTimeX);
			float posY = Mathf.SmoothDamp (transform.position.y, activePlayer.transform.position.y, ref velocity.y, smoothTimeY);
		
			transform.position = new Vector3 (posX, posY, transform.position.z);
		}

		
		bool isArrived = Vector3.Distance(transform.position, destination) <= cameraZValue ;
		//Debug.Log (Vector3.Distance (transform.position, destination));
		if (isArrived && isSwitching && !zoomingOut) {
			ZoomIn ();
		}
		
		// If the bounds option is active, we stop the camera when it reaches the min/max position
		if (bounds) {
			transform.position = new Vector3(Mathf.Clamp(transform.position.x, minCameraPos.x, maxCameraPos.x),
			                                 Mathf.Clamp(transform.position.y, minCameraPos.y, maxCameraPos.y),
			                                 Mathf.Clamp(transform.position.z, minCameraPos.z, maxCameraPos.z)
			                                 );
		}
		
	}

	void ZoomOut(){
		if (Camera.main.orthographicSize <= zoomOutCameraSize) {
			Camera.main.orthographicSize *= zoomFactor;
		} else {
			zoomingOut = false;
		}
	}

	void ZoomIn(){
		if (Camera.main.orthographicSize >= normalCameraSize) {
			Camera.main.orthographicSize /= zoomFactor;
		} else {
			isSwitching = false;
		}
	}

	public void SwitchingFocus(Vector3 dest){

		activePlayer = GameObject.FindGameObjectWithTag ("Player");
		
		isSwitching = true;
		zoomingOut = true;
		destination = dest;
	}
	*/
}
