using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Trap))]
public class TrapDoor : MonoBehaviour {

	Trap trapInfo;
	public float speed = 50f;
	public bool oneTime = false;
	public bool playerCanStep;
	public bool closeAgain = true;
	public float interval = 1f;
	float elapsedTime = 0f;
	Transform leftShutter;
	Transform rightShutter;
	Transform leftRotCenter;
	Transform rightRotCenter;
	Vector3 leftShutterInitPos;
	Vector3 rightShutterInitPos;
	bool opening = true;
	bool closing = false;
	bool waiting = false;

	// Use this for initialization
	void Start () {
		trapInfo = GetComponent<Trap> ();
		if (transform.childCount == 4) {
			leftShutter = transform.GetChild(0);
			rightShutter = transform.GetChild (1);
			leftShutterInitPos = leftShutter.position;
			rightShutterInitPos = rightShutter.position;
			leftRotCenter = transform.GetChild(2);
			rightRotCenter = transform.GetChild (3);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (trapInfo.isActive && opening) {
			//leftShutter.RotateAround (leftRotCenter.position, Vector3.forward, -speed * Time.deltaTime);
			//rightShutter.RotateAround (rightRotCenter.position, Vector3.forward, speed * Time.deltaTime);
			Open ();
		}
		/*if (rightShutter.eulerAngles.z > 90f && opening) {
			opening = false;
			waiting = true;
			rightShutter.eulerAngles = new Vector3(rightShutter.eulerAngles.x, rightShutter.eulerAngles.y, 90f);
			leftShutter.eulerAngles = new Vector3(leftShutter.eulerAngles.x, leftShutter.eulerAngles.y, 270f);
		}*/
		if (waiting && closeAgain) {
			if (elapsedTime > interval){
				closing = true;
				waiting = false;
			} else {
				elapsedTime += Time.deltaTime;
			}
		}
		if (trapInfo.isActive && closing) {
			//leftShutter.RotateAround (leftRotCenter.position, Vector3.forward, speed * Time.deltaTime);
			//rightShutter.RotateAround (rightRotCenter.position, Vector3.forward, -speed * Time.deltaTime);
			//leftShutter.GetComponent<BoxCollider2D>().enabled = false;
			//rightShutter.GetComponent<BoxCollider2D>().enabled = false;
			Close ();
		}
		/*if (Mathf.Abs (rightShutter.eulerAngles.z - 0f) < 1f && closing) {
			closing = false;
			leftShutter.GetComponent<BoxCollider2D>().enabled = true;
			rightShutter.GetComponent<BoxCollider2D>().enabled = true;
			rightShutter.eulerAngles = new Vector3(rightShutter.eulerAngles.x, rightShutter.eulerAngles.y, 0f);
			leftShutter.eulerAngles = new Vector3(leftShutter.eulerAngles.x, leftShutter.eulerAngles.y, 0f);
		}*/
		if (!oneTime && !opening && !waiting && !closing) {
			Reset ();
		}
	}

	void Reset(){
		trapInfo.TurnOff();
		opening = true;
		elapsedTime = 0f;
		leftShutter.position = leftShutterInitPos;
		rightShutter.position = rightShutterInitPos;
	}

	public void Open(){
		leftShutter.RotateAround (leftRotCenter.position, Vector3.forward, -speed * Time.deltaTime);
		rightShutter.RotateAround (rightRotCenter.position, Vector3.forward, speed * Time.deltaTime);
		if (rightShutter.eulerAngles.z > 90f && opening) {
			opening = false;
			waiting = true;
			rightShutter.eulerAngles = new Vector3(rightShutter.eulerAngles.x, rightShutter.eulerAngles.y, 90f);
			leftShutter.eulerAngles = new Vector3(leftShutter.eulerAngles.x, leftShutter.eulerAngles.y, 270f);
		}
	}

	void Close(){
		leftShutter.RotateAround (leftRotCenter.position, Vector3.forward, speed * Time.deltaTime);
		rightShutter.RotateAround (rightRotCenter.position, Vector3.forward, -speed * Time.deltaTime);
		leftShutter.GetComponent<BoxCollider2D>().enabled = false;
		rightShutter.GetComponent<BoxCollider2D>().enabled = false;
		if (Mathf.Abs (rightShutter.eulerAngles.z - 0f) < 1f && closing) {
			closing = false;
			leftShutter.GetComponent<BoxCollider2D>().enabled = true;
			rightShutter.GetComponent<BoxCollider2D>().enabled = true;
			rightShutter.eulerAngles = new Vector3(rightShutter.eulerAngles.x, rightShutter.eulerAngles.y, 0f);
			leftShutter.eulerAngles = new Vector3(leftShutter.eulerAngles.x, leftShutter.eulerAngles.y, 0f);
		}
	}

	void OnCollisionEnter2D(Collision2D coll){
		if(coll.gameObject.CompareTag ("Player")){
			if(!playerCanStep){
				Open ();
			}
		}
	}
}
