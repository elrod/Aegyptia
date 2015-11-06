using UnityEngine;
using System.Collections;

public class TrapSwitch : MonoBehaviour {

	public GameObject relatedTrap;

	void OnTriggerEnter2D(Collider2D col){
		relatedTrap.SetActive(true);
	}
}
