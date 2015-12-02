using UnityEngine;
using System.Collections;

public class RotatingPlatform : MonoBehaviour {
	
	public float speed = 1;
	public GameObject rotationCentre;
	Vector3 pivot;
	Vector3 dir;

	// Use this for initialization
	void Start () {
		pivot = rotationCentre.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		dir = transform.position - pivot;
		dir = Quaternion.Euler(new Vector3 (0, 0, speed))*dir;
		transform.position = pivot + dir;
	}

}
