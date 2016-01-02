using UnityEngine;
using System.Collections;

public class ChangeZoom : MonoBehaviour {

	public bool changeZoom;
	public float newSize;
	public float zoomTime = 1f;
	public bool withOffset;
	public Vector2 offset;
	public bool moveFocus;
	public Transform positionToReach;
	bool focusMoved = false;
	float cameraSize;
	bool zoomIn = false;
	bool zoomOut = false;
	float stepTime = 0f;
	
	void Start(){
		cameraSize = Camera.main.GetComponent<CameraMovement> ().cameraSize;
	}
	
	void Update(){
		if (zoomIn) {
			ChangeCameraZoom (true);
		} else if (zoomOut) {
			ChangeCameraZoom (false);
		}
	}
	
	void ChangeCameraZoom(bool zoomingIn){
		stepTime += Time.deltaTime;
		float percTime = stepTime / zoomTime;
		if (zoomingIn) {
			Camera.main.orthographicSize = Mathf.Lerp(newSize, cameraSize, percTime);
		} else {
			Camera.main.orthographicSize = Mathf.Lerp (cameraSize, newSize, percTime);
		}
		if (stepTime > zoomTime) {
			stepTime = 0f;
			zoomIn = false;
			zoomOut = false;
		}
	}
	
	void OnTriggerStay2D (Collider2D coll){
		if (coll.gameObject.CompareTag ("Player")) {
			if(Camera.main.orthographicSize != newSize && changeZoom){
				zoomIn = false;
				zoomOut = true;
			}
			if (withOffset){
				Camera.main.GetComponent<CameraMovement> ().offset = offset;
			}
		}
	}
	
	void OnTriggerExit2D (Collider2D coll){
		if (coll.gameObject.CompareTag ("Player")) {
			if(changeZoom){
				zoomIn = true;
				zoomOut = false;
			}
			Camera.main.GetComponent<CameraMovement> ().offset = Vector2.zero;
		}
	}
	
	void OnTriggerEnter2D(Collider2D coll){
		if(coll.gameObject.CompareTag ("Player")){
			if(moveFocus && !focusMoved){
				Camera.main.GetComponent<CameraMovement>().SwitchFocus(positionToReach.position);
				focusMoved = true;
			}
		}
	}
	
	

}
