using UnityEngine;
using System.Collections;

public class RotatingAxeActivator : MonoBehaviour {

	public GameObject relatedRotatingAxe;
	
	void OnTriggerEnter2D (Collider2D col){
		relatedRotatingAxe.GetComponent<RotatingAxe>().TurnOn();
	}
}
