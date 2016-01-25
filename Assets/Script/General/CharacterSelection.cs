using UnityEngine;
using System.Collections;

public class CharacterSelection : MonoBehaviour {

	public GameObject osiris;
	public GameObject isis;
	public GameObject osirisPanel;
	public GameObject isisPanel;

	void Start(){
	}

	public void SpawnOsiris(){
		osiris.GetComponent<Renderer> ().enabled = true;
		osiris.GetComponent<Collider2D> ().enabled = true;
		osiris.GetComponent<Controller2D> ().enabled = true;
		osiris.GetComponent<Player> ().enabled = true;
		osiris.GetComponent<Player> ().TurnOn ();
		osiris.tag = "Player";
		osirisPanel.SetActive (true);
		gameObject.SetActive (false);
	}

	public void SpawnIsis(){
		isis.GetComponent<Renderer> ().enabled = true;
		isis.GetComponent<Collider2D> ().enabled = true;
		isis.GetComponent<Controller2D> ().enabled = true;
		isis.GetComponent<Player> ().enabled = true;
		isis.GetComponent<Player> ().TurnOn ();
		isis.tag = "Player";
		isisPanel.SetActive (true);
		gameObject.SetActive (false);
	}
}
