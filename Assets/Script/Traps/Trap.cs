using UnityEngine;
using System.Collections;

public class Trap : MonoBehaviour {

	public bool isActive;

	public void TurnOn(){
		isActive = true;
	}

	public void TurnOff(){
		isActive = false;
	}
}
