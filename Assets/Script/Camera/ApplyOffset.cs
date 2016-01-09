using UnityEngine;
using System.Collections;

public class ApplyOffset : MonoBehaviour {

	public Vector2 offset;

	void OnTriggerStay2D (Collider2D coll){
		if (coll.gameObject.CompareTag ("Player")) {
			Camera.main.GetComponent<CameraMovement> ().offset = offset;
		}
	}

	void OnTriggerExit2D (Collider2D coll){
		if (coll.gameObject.CompareTag ("Player")) {
			Camera.main.GetComponent<CameraMovement> ().offset = Vector2.zero;
		}
	}
}
