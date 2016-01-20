using UnityEngine;
using System.Collections;

public abstract class Animal : MonoBehaviour {

	// With this abstract class it is easier from the player to interact with its own 
	// transformation independently from it being crocodile, scarab, etc.

	public abstract void TurnOn ();

	public abstract void TurnOff ();

	//public abstract bool IsGoingLeft();
}
