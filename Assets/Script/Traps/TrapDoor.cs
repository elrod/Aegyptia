using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Trap))]
public class TrapDoor : MonoBehaviour {

	Trap trapInfo;
	public float speed = 50f;
	public bool oneTime = false;
	public bool playerCanStep = true;
	public bool closeAgain = true;
	public float interval = 1f;
	float elapsedTime = 0f;
	Transform leftShutter;
	Transform rightShutter;
	Transform leftRotCenter;
	Transform rightRotCenter;
	Vector3 leftShutterInitPos;
	Vector3 rightShutterInitPos;
    bool ready = true;
	bool opening = false;
	bool closing = false;
	bool waiting = false;

	// Use this for initialization
	void Start () {
		trapInfo = GetComponent<Trap> ();
		if (transform.childCount >= 2) {

            rightRotCenter = transform.GetChild(0);
            rightShutter = rightRotCenter.transform.GetChild(0);
			leftRotCenter = transform.GetChild(1);
            leftShutter = leftRotCenter.transform.GetChild(0);
            rightShutterInitPos = rightRotCenter.rotation.eulerAngles;
            leftShutterInitPos = leftRotCenter.rotation.eulerAngles;
            leftShutter.GetComponent<BoxCollider2D>().enabled = true;
            rightShutter.GetComponent<BoxCollider2D>().enabled = true;
          
		}
	}
	
	// Update is called once per frame
	void Update () {
        

		if (trapInfo.isActive && ( ready || opening )) {
           // Debug.Log("opening " +  opening);
			Open ();
		}
		if (waiting && closeAgain) {
           // Debug.Log("wait " + waiting);
			if (elapsedTime > interval){
				closing = true;
				waiting = false;
			} else {
				elapsedTime += Time.deltaTime;
			}
		}
		if (trapInfo.isActive && closing) {
          //  Debug.Log("close " + closing);
			Close ();
		}

        ready = !opening && !closing && !waiting;
        if (ready)
        {
            leftShutter.GetComponent<BoxCollider2D>().enabled = true;
            rightShutter.GetComponent<BoxCollider2D>().enabled = true;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            Reset();
        }
        else {
            leftShutter.GetComponent<BoxCollider2D>().enabled = false;
            rightShutter.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
	}

	void Reset(){
		trapInfo.TurnOff();
		ready = true;
		elapsedTime = 0f;
	}

	public void Open(){

        float angle = Mathf.MoveTowardsAngle(leftRotCenter.eulerAngles.z, 290, speed * Time.deltaTime);
        leftRotCenter.eulerAngles = new Vector3(0, 0, angle);
        angle = Mathf.MoveTowardsAngle(rightRotCenter.eulerAngles.z, 70, speed * Time.deltaTime);
        rightRotCenter.eulerAngles = new Vector3(0, 0, angle);
        
        opening = true;
       // Debug.Log("DX " + rightRotCenter.eulerAngles.z + "SX " + leftRotCenter.eulerAngles.z);
        if (Mathf.RoundToInt(rightRotCenter.eulerAngles.z) == 70 && Mathf.RoundToInt(leftRotCenter.eulerAngles.z) == 290)
        {
            opening = false;
            waiting = true;
        }
	}

	void Close(){

            float angle = Mathf.MoveTowardsAngle(leftRotCenter.eulerAngles.z, leftShutterInitPos.z, speed * Time.deltaTime);
            leftRotCenter.eulerAngles = new Vector3(0, 0, angle);
            angle = Mathf.MoveTowardsAngle(rightRotCenter.eulerAngles.z, rightShutterInitPos.z, speed * Time.deltaTime);
            rightRotCenter.eulerAngles = new Vector3(0, 0, angle);
            closing = true;

        if (Mathf.RoundToInt(rightRotCenter.eulerAngles.z) == rightShutterInitPos.z && Mathf.RoundToInt(leftRotCenter.eulerAngles.z) == leftShutterInitPos.z)
        {
			closing = false;
		}
        
	}
}
