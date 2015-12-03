using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
	
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
	
	// Update is called once per frame
	void FixedUpdate () {

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
}
