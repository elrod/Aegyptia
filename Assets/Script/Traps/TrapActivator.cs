using UnityEngine;
using System.Collections;

public class TrapActivator : MonoBehaviour {

	public GameObject relatedTrap;
	
	void OnTriggerEnter2D (Collider2D col){
		relatedTrap.GetComponent<Trap>().TurnOn();
	}
}
