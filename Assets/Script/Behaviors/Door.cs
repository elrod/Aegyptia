using UnityEngine;
using System.Collections;

public class Door : Tool {

	public bool isClosed = true;
	bool changeState;
	
	// Update is called once per frame
	void Update () {
		if (changeState) {
			if(isClosed){
				Open();
			} else {
				gameObject.GetComponent<Renderer>().enabled = true;
				Close();
			}
		}
	}
	
	public override void Use(){
		changeState = true;
	}
	
	void Open(){
		if (transform.eulerAngles.y < 90){
			transform.Rotate(new Vector3(0, 1, 0));
		} else {
			isClosed = false;
			gameObject.GetComponent<Renderer>().enabled = false;
			changeState = false;
		}
	}
	
	void Close(){
		if (transform.eulerAngles.y < 359){
			Debug.Log(transform.eulerAngles.y);
			transform.Rotate(new Vector3(0, -1, 0));
		} else {
			isClosed = true;
			changeState = false;
			transform.eulerAngles = Vector3.zero;
		}
	}
}
