using UnityEngine;
using System.Collections;

public class Pickable : MonoBehaviour {

	public Sprite inventoryPicture;

	Renderer theRenderer;
	Collider2D theCollider;

	// Use this for initialization
	void Start () {
		theRenderer = GetComponent<Renderer>();
		theCollider = GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D col){
		if(col.tag == "Player"){
			col.gameObject.GetComponent<Inventory>().PickUp(gameObject, inventoryPicture);
			GetComponent<AudioSource>().Play ();
			theCollider.enabled = false;
			theRenderer.enabled = false;
		}
	}
}
