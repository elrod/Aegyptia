using UnityEngine;
using System.Collections;

public class TrapDeactivator : Tool {

	public GameObject relatedTrap;
	bool focused = false;
	
	void OnTriggerEnter2D (Collider2D col){
		relatedTrap.GetComponent<Trap>().TurnOff();
		if (!focused) {
			Camera.main.GetComponent<CameraMovement> ().SwitchFocus (relatedTrap.transform.position);
			focused = true;
		}
	}
	
	public override void Use (){
		relatedTrap.GetComponent<Trap>().TurnOff();
		if (!focused) {
			Camera.main.GetComponent<CameraMovement> ().SwitchFocus (relatedTrap.transform.position);
			focused = true;
		}
	}
}
