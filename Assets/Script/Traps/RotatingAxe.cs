using UnityEngine;
using System.Collections;

public class RotatingAxe : MonoBehaviour {

	public GameObject RotationCentre;
	public float rotationSpeed;
	public bool invertRotation;
	public float minAngle;
	public float maxAngle;
	public bool goRight = true;
	public bool isActive;
	float offset;
	float tolerance = 1f;
	
	// Use this for initialization
	void Start () { 
		offset = minAngle - maxAngle;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (isActive) {
			if (goRight) {
				transform.RotateAround (RotationCentre.transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);
			} else {
				transform.RotateAround (RotationCentre.transform.position, Vector3.forward, -rotationSpeed * Time.deltaTime);
			}
			if (invertRotation) {
				if ((Mathf.Abs (transform.eulerAngles.z - maxAngle) < tolerance && goRight) || 
					(Mathf.Abs (transform.eulerAngles.z - minAngle) < tolerance && !goRight)) {
					goRight = !goRight;
				}
			}
		}
	}

	public void TurnOn(){ 
		isActive = true;
	}

	public void TurnOff(){
		isActive = false;
	}
}
