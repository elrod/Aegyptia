using UnityEngine;
using System.Collections;

public class TrapActivator : MonoBehaviour {

	public GameObject relatedTrap;
	bool focused = false;
	
	void OnTriggerEnter2D (Collider2D col){
		relatedTrap.GetComponent<Trap>().TurnOn();
		if (!focused) {
			Camera.main.GetComponent<CameraMovement> ().SwitchFocus (relatedTrap.transform.position);
			focused = true;
		}
	}
}
