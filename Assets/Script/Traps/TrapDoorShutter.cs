using UnityEngine;
using System.Collections;

public class TrapDoorShutter : MonoBehaviour {

	TrapDoor trapDoor;
	Trap trapInfo;

	void Start(){
		trapDoor = transform.parent.GetComponent<TrapDoor> ();
		trapInfo = transform.parent.GetComponent<Trap> ();
	}

	void OnCollisionEnter2D (Collision2D coll){
		if (!trapDoor.playerCanStep && coll.gameObject.CompareTag ("Player")) {
			trapInfo.TurnOn ();
		}
	}
}
