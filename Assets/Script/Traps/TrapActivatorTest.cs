using UnityEngine;
using System.Collections;

public class TrapActivatorTest : Tool {

	public GameObject relatedTrap;
	public Transform placeCameraHere;
	public float pipCameraSize = 10f;
	public float turnOffCameraTime = 2f;
	float elapsedTime = 0f;
	bool turnOff = false;
	GameObject pipCamera;

	void Start() {
		pipCamera = GameObject.FindGameObjectWithTag ("PiPCamera");
	}

	void Update() {
		if (turnOff) {
			if(elapsedTime > turnOffCameraTime){
				pipCamera.GetComponent<Camera> ().enabled = false;
				turnOff = false;
				elapsedTime = 0f;
			} else {
				elapsedTime += Time.deltaTime;
			}
		}
	}

	void OnTriggerEnter2D (Collider2D col){
		relatedTrap.GetComponent<Trap>().TurnOn();
		pipCamera.transform.position = new Vector3(placeCameraHere.position.x, 
		                                           placeCameraHere.position.y, 
		                                           pipCamera.transform.position.z);
		pipCamera.GetComponent<Camera> ().orthographicSize = pipCameraSize;
		pipCamera.GetComponent<Camera> ().enabled = true;
		turnOff = true;
	}

	public override void Use (){
		relatedTrap.GetComponent<Trap>().TurnOn();
		pipCamera.transform.position = new Vector3(placeCameraHere.position.x, 
		                                           placeCameraHere.position.y, 
		                                           pipCamera.transform.position.z);
		pipCamera.GetComponent<Camera> ().orthographicSize = pipCameraSize;
		pipCamera.GetComponent<Camera> ().enabled = true;
		turnOff = true;
	}
}
