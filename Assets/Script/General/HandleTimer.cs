using UnityEngine;
using System.Collections;

public class HandleTimer : MonoBehaviour {

	public bool startTimer = false;

	void OnTriggerEnter2D(Collider2D coll){
		if (coll.gameObject.CompareTag ("Player")) {
			if (startTimer){
				FindObjectOfType<Timer>().StartTimer();
			} else {
				FindObjectOfType<Timer>().StopTimer();
			}
		}
	}

}
